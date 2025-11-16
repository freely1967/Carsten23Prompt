namespace ApocalypticFastFood;

public class DiscountManager
{
    private readonly ApocalypticFastFood.Services.ILoyaltyPointsCalculator _pointsCalculator;

    public DiscountManager(ApocalypticFastFood.Services.ILoyaltyPointsCalculator? pointsCalculator = null)
    {
        _pointsCalculator = pointsCalculator ?? new ApocalypticFastFood.Services.DefaultLoyaltyPointsCalculator();
        ItemPrices = new Dictionary<string, double>();
        ItemQuantities = new Dictionary<string, int>();
        Items = new List<string>();
        PastOrders = new List<string>();
    }

    // Public state fields (kept as-is for compatibility with existing code/tests)
    public bool AcceptsMarketing;
    public int Age;
    public double AverageSpend;
    public bool CheckedIn;
    public int ComplaintsCount;
    public int ConsecutiveVisits;
    public string? CreditCardType;
    public int CurrentMonth;
    public int CustomerType; // 1=regular, 2=vip, 3=employee, 4=senior, 5=student, 6=minor, 7=banned
    public string Day = string.Empty;
    public int DaysLastVisit;
    public string DeviceType = string.Empty;
    public string DietaryPreference = string.Empty;
    public bool EmailSubscribed;
    public string EmployeeName = string.Empty;
    public int FamilyMembers;
    public string FavoriteItem = string.Empty;
    public bool HasAllergies;
    public bool HasApp;
    public bool HasKids;
    public bool HasLoyaltyCard;
    public int Hour;
    public bool IsBirthday;
    public bool IsCurbside;
    public bool IsDelivery;
    public bool IsDineIn;
    public bool IsDriveThru;
    public bool IsFirstOrder;
    public bool IsHoliday;
    public bool IsRushHour;
    public bool IsWeekend;
    public int ItemCount;
    public Dictionary<string, double> ItemPrices;
    public Dictionary<string, int> ItemQuantities;
    public List<string> Items;
    public double LastTipAmount;
    public double Latitude;
    public bool LeftReview;
    public double Longitude;
    public int ManagerApproval;
    public string MembershipLevel = string.Empty;
    public int Minute;
    public int MonthsSinceMembership;
    public int OrderNumber;
    public List<string> PastOrders;
    public string PaymentMethod = string.Empty;
    public string PreviousOrder = string.Empty;
    public string PromoCode = string.Empty;
    public int ReferralCount;
    public string Region = string.Empty;
    public int RestaurantId;
    public int ReviewStars;
    public bool SmsSubscribed;
    public string SocialMediaFollow = string.Empty;
    public int StreakDays;
    public int Temperature;
    public double TotalAmount;
    public int VisitCount;
    public string Weather = string.Empty;
    public bool HasParentApproval; // kept for compatibility with adapters
    public int Id { get; set; }

    // Minimal, safe implementation: keep API stable, avoid reintroducing large legacy logic here.
    public double CalculateDiscount()
    {
        // Compose legacy, rule-based engines to preserve previous DiscountManager integration points.
        double discount = 0.0;

        // Promo-based discount
        if (!string.IsNullOrEmpty(PromoCode))
        {
            discount += new DiscountEngine(new IDiscountRule[] { new PromoCodeRule() }).Calculate(this);
        }

        // Visit-count based discount (apply only for meaningful visit-counts to avoid stacking small visit discounts)
        if (VisitCount >= 10)
        {
            discount += new DiscountEngine(new IDiscountRule[] { new VisitCountRule() }).Calculate(this);
        }

        // Time-of-day discounts
        discount += new DiscountEngine(new IDiscountRule[] { new TimeOfDayRule() }).Calculate(this);

        // Family-based discounts
        discount += new DiscountEngine(new IDiscountRule[] { new FamilyRule() }).Calculate(this);

        // Ensure discount does not exceed a safety cap when there's a positive total amount
        if (TotalAmount > 0 && discount > TotalAmount * 0.95) discount = TotalAmount * 0.95;

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
        Console.WriteLine("Connecting to smtp.example.com:587");
        Console.WriteLine("Sending to: " + email);
    }

    public void SendSmsReceipt(string phone)
    {
        Console.WriteLine("Sending SMS to: " + phone);
    }
}

// Responsibility: format a receipt string for a given DiscountManager state
public class ReceiptFormatter
{
    public string Format(DiscountManager dm)
    {
        var receipt = "====================================\n";
        receipt += "    FAST FOOD MEGA CHAIN\n";
        receipt += "====================================\n";

        receipt += "Customer: ";
        switch (dm.CustomerType)
        {
            case 1: receipt += "Regular\n"; break;
            case 2:
                receipt += "VIP - " + dm.MembershipLevel + "\n";
                if (dm.MembershipLevel == "Diamond") receipt += "*** PREMIUM CUSTOMER ***\n";
                break;
            case 3: receipt += "Employee\n"; break;
            case 4: receipt += "Senior (Age: " + dm.Age + ")\n"; break;
            case 5: receipt += "Student\n"; break;
            case 6: receipt += "Minor - NEEDS APPROVAL\n"; break;
            case 7: receipt += "BANNED CUSTOMER\n"; break;
            default: receipt += "Unknown\n"; break;
        }

        receipt += "Order #: " + dm.OrderNumber + "\n";
        receipt += "Date: " + dm.Day + "\n";
        receipt += "Time: " + dm.Hour + ":" + dm.Minute + "\n";
        receipt += "Location: Restaurant #" + dm.RestaurantId + "\n";

        if (dm.IsDineIn)
            receipt += "Service: Dine-In\n";
        else if (dm.IsDriveThru)
            receipt += "Service: Drive-Thru\n";
        else if (dm.IsCurbside)
            receipt += "Service: Curbside\n";
        else if (dm.IsDelivery)
            receipt += "Service: Delivery\n";

        receipt += "------------------------------------\n";
        receipt += "ITEMS:\n";

        foreach (var item in dm.ItemQuantities)
        {
            double price = dm.ItemPrices.TryGetValue(item.Key, out var p) ? p : item.Key switch
            {
                "burger" => 8.99,
                "fries" => 3.49,
                "shake" => 4.99,
                "nuggets" => 6.49,
                "salad" => 7.99,
                _ => 0
            };

            receipt += "  " + item.Value + "x " + item.Key.ToUpper() + " @ $" + price + " = $" +
                       (item.Value * price).ToString("F2") + "\n";
        }

        receipt += "------------------------------------\n";
        receipt += "Subtotal: $" + dm.TotalAmount.ToString("F2") + "\n";

        var discount = dm.CalculateDiscount();
        receipt += "Discount: -$" + discount.ToString("F2") + "\n";

        var tax = Math.Max(0, (dm.TotalAmount - discount) * 0.08);
        receipt += "Tax (8%): $" + tax.ToString("F2") + "\n";

        var total = dm.TotalAmount - discount + tax;
        receipt += "------------------------------------\n";
        receipt += "TOTAL: $" + total.ToString("F2") + "\n";
        receipt += "====================================\n";

        var points = dm.CalculateLoyaltyPoints();
        receipt += "Loyalty Points Earned: " + points + "\n";

        if (dm.IsBirthday) receipt += "\n*** HAPPY BIRTHDAY! ***\n";

        return receipt;
    }
}

// Price catalog abstractions to centralize item pricing
public interface IPriceCatalog
{
    double GetPrice(string item);
}

public class InMemoryPriceCatalog : IPriceCatalog
{
    private readonly Dictionary<string, double> _prices = new()
    {
        ["burger"] = 8.99,
        ["fries"] = 3.49,
        ["shake"] = 4.99,
        ["nuggets"] = 6.49,
        ["salad"] = 7.99
    };

    public double GetPrice(string item)
    {
        if (item is null) return 0.0;
        return _prices.TryGetValue(item, out var p) ? p : 0.0;
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
        _dm.CustomerType = custType;
        _dm.Day = day;
        _dm.Hour = hr;
        _dm.Items = items;

        _dm.TotalAmount = 0;
        foreach (var item in items)
        {
            _dm.TotalAmount += _priceCatalog.GetPrice(item);
        }

        var ctx = new CustomerContext(0, _dm.Age, _dm.VisitCount, _dm.MembershipLevel ?? string.Empty, _dm.HasParentApproval);
        var disc = _discountProvider.GetDiscount(ctx);

        Console.WriteLine(_dm.GenerateReceipt());

        _orderRepo.SaveOrder(_dm.OrderNumber, _dm.TotalAmount, disc);

        // simplified payment handling
        switch (_dm.PaymentMethod)
        {
            case "cash":
                _paymentProcessor.ProcessCash(_dm.TotalAmount);
                break;
            case "credit":
                _paymentProcessor.ProcessCreditCard("4111111111111111", "123", "12/29");
                break;
            case "debit":
                _paymentProcessor.ProcessDebitCard("4111111111111111", "0000");
                break;
            case "paypal":
                    _paymentProcessor.ProcessPaypal("customer@paypal", "password");
                break;
            default:
                break;
        }

        if (_dm.EmailSubscribed) _emailSender.Send("customer@email.com", "Receipt");
    }
}

// The payment and adapter types referenced in other parts of the codebase are defined in their own files (Adapters/),
// keeping this file focused and compilable.