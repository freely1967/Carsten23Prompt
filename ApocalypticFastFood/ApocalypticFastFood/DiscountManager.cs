namespace ApocalypticFastFood;

public enum PaymentMethod
{
    Unknown,
    Cash,
    Credit,
    Debit,
    Paypal,
    App
}

public enum CustomerCategory
{
    Regular = 1,
    VIP = 2,
    Employee = 3,
    Senior = 4,
    Student = 5,
    Minor = 6,
    Banned = 7
}

public class DiscountManager
{
    private readonly ApocalypticFastFood.Services.ILoyaltyPointsCalculator _pointsCalculator;
    private readonly DiscountEngine _discountEngine;
    private readonly DiscountEngine _promoEngine;
    private readonly DiscountEngine _visitEngine;
    private readonly DiscountEngine _timeEngine;
    private readonly DiscountEngine _familyEngine;
    private readonly IEmailSender _emailSender;
    private readonly ISmsSender _smsSender;
    public const decimal DiscountCapFactor = 0.95m;
    public const decimal TaxRate = 0.08m;

    public DiscountManager(DiscountEngine? discountEngine = null, ApocalypticFastFood.Services.ILoyaltyPointsCalculator? pointsCalculator = null, IEmailSender? emailSender = null, ISmsSender? smsSender = null)
    {
        _pointsCalculator = pointsCalculator ?? new ApocalypticFastFood.Services.DefaultLoyaltyPointsCalculator();
        // Default rule set for legacy DiscountManager: create per-category engines to avoid allocations during CalculateDiscount
        _promoEngine = new DiscountEngine(new IDiscountRule[] { new PromoCodeRule() });
        _visitEngine = new DiscountEngine(new IDiscountRule[] { new VisitCountRule() });
        _timeEngine = new DiscountEngine(new IDiscountRule[] { new TimeOfDayRule() });
        _familyEngine = new DiscountEngine(new IDiscountRule[] { new FamilyRule() });

        // Combined engine (used when injected or if callers want a single engine)
        _discountEngine = discountEngine ?? new DiscountEngine(new IDiscountRule[] { new PromoCodeRule(), new VisitCountRule(), new TimeOfDayRule(), new FamilyRule() });

        ItemPrices = new Dictionary<string, decimal>();
        ItemQuantities = new Dictionary<string, int>();
        Items = new List<string>();
        PastOrders = new List<string>();
        _emailSender = emailSender ?? new ConsoleEmailSender();
        _smsSender = smsSender ?? new ConsoleSmsSender();
    }

    // Public state properties (converted from fields to follow C# conventions)
    public bool AcceptsMarketing { get; set; }
    public int Age { get; set; }
    public decimal AverageSpend { get; set; }
    public bool CheckedIn { get; set; }
    public int ComplaintsCount { get; set; }
    public int ConsecutiveVisits { get; set; }
    public string? CreditCardType { get; set; }
    public int CurrentMonth { get; set; }
    public CustomerCategory CustomerType { get; set; } = CustomerCategory.Regular;
    public string Day { get; set; } = string.Empty;
    public int DaysLastVisit { get; set; }
    public string DeviceType { get; set; } = string.Empty;
    public string DietaryPreference { get; set; } = string.Empty;
    public bool EmailSubscribed { get; set; }
    public string EmployeeName { get; set; } = string.Empty;
    public int FamilyMembers { get; set; }
    public string FavoriteItem { get; set; } = string.Empty;
    public bool HasAllergies { get; set; }
    public bool HasApp { get; set; }
    public bool HasKids { get; set; }
    public bool HasLoyaltyCard { get; set; }
    public int Hour { get; set; }
    public bool IsBirthday { get; set; }
    public bool IsCurbside { get; set; }
    public bool IsDelivery { get; set; }
    public bool IsDineIn { get; set; }
    public bool IsDriveThru { get; set; }
    public bool IsFirstOrder { get; set; }
    public bool IsHoliday { get; set; }
    public bool IsRushHour { get; set; }
    public bool IsWeekend { get; set; }
    public int ItemCount { get; set; }
    public Dictionary<string, decimal> ItemPrices { get; set; }
    public Dictionary<string, int> ItemQuantities { get; set; }
    public List<string> Items { get; set; }
    public decimal LastTipAmount { get; set; }
    // Geographical coordinates - intentionally `double`, not a monetary value.
    // TODO: keep as double; do NOT mix with `decimal` arithmetic for money.
    public double Latitude { get; set; }
    public bool LeftReview { get; set; }
    // Geographical coordinates - intentionally `double`, not a monetary value.
    // TODO: keep as double; do NOT mix with `decimal` arithmetic for money.
    public double Longitude { get; set; }
    public int ManagerApproval { get; set; }
    public string MembershipLevel { get; set; } = string.Empty;
    public int Minute { get; set; }
    public int MonthsSinceMembership { get; set; }
    public int OrderNumber { get; set; }
    public List<string> PastOrders { get; set; }
    private PaymentMethod _paymentMethodEnum = PaymentMethod.Unknown;

    public PaymentMethod PaymentMethodEnum
    {
        get => _paymentMethodEnum;
        set => _paymentMethodEnum = value;
    }

    // Legacy parsing helper removed as migration to typed `PaymentMethod` is complete.
    public string PreviousOrder { get; set; } = string.Empty;
    public string PromoCode { get; set; } = string.Empty;
    public int ReferralCount { get; set; }
    public string Region { get; set; } = string.Empty;
    public int RestaurantId { get; set; }
    public int ReviewStars { get; set; }
    public bool SmsSubscribed { get; set; }
    public string SocialMediaFollow { get; set; } = string.Empty;
    public int StreakDays { get; set; }
    public int Temperature { get; set; }
    public decimal TotalAmount { get; set; }
    public int VisitCount { get; set; }
    public string Weather { get; set; } = string.Empty;
    public bool HasParentApproval { get; set; } // kept for compatibility with adapters
    public int Id { get; set; }

    // Minimal, safe implementation: keep API stable, avoid reintroducing large legacy logic here.
    public decimal CalculateDiscount()
    {
        // Use per-category engines to preserve previous conditional behavior while avoiding allocations
        decimal discount = 0.0m;

        // Promo-based discount
        if (!string.IsNullOrEmpty(PromoCode))
        {
            discount += _promoEngine.Calculate(this);
        }

        // Visit-count based discount (apply only for meaningful visit-counts)
        if (VisitCount >= 10)
        {
            discount += _visitEngine.Calculate(this);
        }

        // Time-of-day discounts
        discount += _timeEngine.Calculate(this);

        // Family-based discounts
        discount += _familyEngine.Calculate(this);

        // Ensure discount does not exceed a safety cap when there's a positive total amount
        if (TotalAmount > 0 && discount > TotalAmount * DiscountCapFactor) discount = TotalAmount * DiscountCapFactor;

        return discount;
    }

    public string GenerateReceipt()
    {
        return new ReceiptFormatter().Format(this);
    }

    // Exposed so ReceiptFormatter can access points
    public int CalculateLoyaltyPoints()
    {
        return _pointsCalculator.CalculatePoints(this);
    }

    public void SendEmailReceipt(string email)
    {
        _emailSender.Send(email, "Receipt", GenerateReceipt());
    }

    public void SendSmsReceipt(string phone)
    {
        _smsSender.Send(phone, "Your receipt is ready.");
    }
}

// Responsibility: format a receipt string for a given DiscountManager state
public class ReceiptFormatter
{
    private readonly IPriceCatalog _priceCatalog;

    public ReceiptFormatter(IPriceCatalog? priceCatalog = null)
    {
        _priceCatalog = priceCatalog ?? new InMemoryPriceCatalog();
    }

    public string Format(DiscountManager dm)
    {
        var sb = new System.Text.StringBuilder();
        sb.AppendLine("====================================");
        sb.AppendLine("    FAST FOOD MEGA CHAIN");
        sb.AppendLine("====================================");

        sb.Append("Customer: ");
        switch (dm.CustomerType)
        {
            case CustomerCategory.Regular: sb.AppendLine("Regular"); break;
            case CustomerCategory.VIP:
                sb.Append("VIP - "); sb.AppendLine(dm.MembershipLevel);
                if (dm.MembershipLevel == "Diamond") sb.AppendLine("*** PREMIUM CUSTOMER ***");
                break;
            case CustomerCategory.Employee: sb.AppendLine("Employee"); break;
            case CustomerCategory.Senior: sb.AppendLine($"Senior (Age: {dm.Age})"); break;
            case CustomerCategory.Student: sb.AppendLine("Student"); break;
            case CustomerCategory.Minor: sb.AppendLine("Minor - NEEDS APPROVAL"); break;
            case CustomerCategory.Banned: sb.AppendLine("BANNED CUSTOMER"); break;
            default: sb.AppendLine("Unknown"); break;
        }

        sb.AppendLine($"Order #: {dm.OrderNumber}");
        sb.AppendLine($"Date: {dm.Day}");
        sb.AppendLine($"Time: {dm.Hour}:{dm.Minute}");
        sb.AppendLine($"Location: Restaurant #{dm.RestaurantId}");

        if (dm.IsDineIn)
            sb.AppendLine("Service: Dine-In");
        else if (dm.IsDriveThru)
            sb.AppendLine("Service: Drive-Thru");
        else if (dm.IsCurbside)
            sb.AppendLine("Service: Curbside");
        else if (dm.IsDelivery)
            sb.AppendLine("Service: Delivery");

        sb.AppendLine("------------------------------------");
        sb.AppendLine("ITEMS:");

        foreach (var kv in dm.ItemQuantities)
        {
            var itemName = kv.Key;
            var qty = kv.Value;

            decimal price = dm.ItemPrices != null && dm.ItemPrices.TryGetValue(itemName, out var p)
                ? p
                : _priceCatalog.GetPrice(itemName);

            sb.AppendLine($"  {qty}x {itemName.ToUpperInvariant()} @ ${price:F2} = ${(price * qty):F2}");
        }

        sb.AppendLine("------------------------------------");
        sb.AppendLine($"Subtotal: ${dm.TotalAmount:F2}");

        var discount = dm.CalculateDiscount();
        sb.AppendLine($"Discount: -${discount:F2}");

        var taxValue = (dm.TotalAmount - discount) * DiscountManager.TaxRate;
        var tax = taxValue > 0m ? taxValue : 0m;
        sb.AppendLine($"Tax ({(DiscountManager.TaxRate * 100m):F1}%): ${tax:F2}");

        var total = dm.TotalAmount - discount + tax;
        sb.AppendLine("------------------------------------");
        sb.AppendLine($"TOTAL: ${total:F2}");
        sb.AppendLine("====================================");

        var points = dm.CalculateLoyaltyPoints();
        sb.AppendLine($"Loyalty Points Earned: {points}");

        if (dm.IsBirthday) sb.AppendLine("\n*** HAPPY BIRTHDAY! ***");

        return sb.ToString();
    }
}

// Price catalog abstractions to centralize item pricing
// Price catalog abstractions to centralize item pricing
public interface IPriceCatalog
{
    decimal GetPrice(string item);
}

public class InMemoryPriceCatalog : IPriceCatalog
{
    private readonly Dictionary<string, decimal> _prices = new()
    {
        ["burger"] = 8.99m,
        ["fries"] = 3.49m,
        ["shake"] = 4.99m,
        ["nuggets"] = 6.49m,
        ["salad"] = 7.99m
    };

    public decimal GetPrice(string item)
    {
        if (item is null) return 0.0m;
        return _prices.TryGetValue(item, out var p) ? p : 0.0m;
    }
}

// Lightweight order processing helper retained for backward compatibility
public class OrderProcessor
{
    private readonly IOrderRepository _orderRepo;
    private readonly DiscountManager _dm;
    private readonly IEmailSender _emailSender;
    private readonly ISmsSender _smsSender;
    private readonly IPriceCatalog _priceCatalog;
    private readonly IPaymentProcessor _paymentProcessor;
    private readonly IDiscountProvider _discountProvider;

    public OrderProcessor(IPriceCatalog? priceCatalog = null,
                          IOrderRepository? orderRepo = null,
                          DiscountManager? dm = null,
                          IEmailSender? emailSender = null,
                          ISmsSender? smsSender = null,
                          IPaymentProcessor? paymentProcessor = null,
                          IDiscountProvider? discountProvider = null)
    {
        _priceCatalog = priceCatalog ?? new InMemoryPriceCatalog();
        _orderRepo = orderRepo ?? new DatabaseRepositoryAdapter(new DatabaseService());
        _dm = dm ?? new DiscountManager();
        _emailSender = emailSender ?? new ConsoleEmailSender();
            _smsSender = smsSender ?? new ConsoleSmsSender();
        _paymentProcessor = paymentProcessor ?? new PaymentProcessorFacade(new CashPaymentProcessor(), new CardPaymentProcessor());
        _discountProvider = discountProvider ?? new Adapters.DiscountManagerDiscountProvider(_dm);
    }

    public void ProcessOrder(int custType, string day, int hr, List<string> items)
    {
        _dm.CustomerType = (CustomerCategory)custType;
        _dm.Day = day;
        _dm.Hour = hr;
        _dm.Items = items;

        _dm.TotalAmount = 0m;
        foreach (var item in items)
        {
            _dm.TotalAmount += _priceCatalog.GetPrice(item);
        }

        var ctx = new CustomerContext(0, _dm.Age, _dm.VisitCount, _dm.MembershipLevel ?? string.Empty, _dm.HasParentApproval);
        var disc = _discountProvider.GetDiscount(ctx);

        Console.WriteLine(_dm.GenerateReceipt());

        _orderRepo.SaveOrder(_dm.OrderNumber, _dm.TotalAmount, disc);

        // simplified payment handling
        switch (_dm.PaymentMethodEnum)
        {
            case PaymentMethod.Cash:
                _paymentProcessor.ProcessCash(_dm.TotalAmount);
                break;
            case PaymentMethod.Credit:
                _paymentProcessor.ProcessCreditCard("4111111111111111", "123", "12/29");
                break;
            case PaymentMethod.Debit:
                _paymentProcessor.ProcessDebitCard("4111111111111111", "0000");
                break;
            case PaymentMethod.Paypal:
                _paymentProcessor.ProcessPaypal("customer@paypal", "password");
                break;
            case PaymentMethod.App:
                // Application wallet processing falls back to cash for now
                _paymentProcessor.ProcessCash(_dm.TotalAmount);
                break;
            default:
                break;
        }

        if (_dm.EmailSubscribed) _emailSender.Send("customer@email.com", "Receipt");
    }
}

// The payment and adapter types referenced in other parts of the codebase are defined in their own files (Adapters/),
// keeping this file focused and compilable.