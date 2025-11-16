namespace ApocalypticFastFood.Services
{
    public interface IPurchaseService
    {
        void MakePurchase(CustomerContext ctx);
    }

    // Adapter that delegates to an existing Customer instance for compatibility
    public class CustomerPurchaseService : IPurchaseService
    {
        private readonly Customer _customer;

        public CustomerPurchaseService(Customer customer)
        {
            _customer = customer ?? throw new System.ArgumentNullException(nameof(customer));
        }

        public void MakePurchase(CustomerContext ctx)
        {
            // Apply context to the wrapped customer and delegate
            _customer.Id = ctx.Id;
            _customer.Age = ctx.Age;
            _customer.VisitCount = ctx.VisitCount;
            _customer.MembershipLevel = ctx.MembershipLevel ?? string.Empty;
            _customer.HasParentApproval = ctx.HasParentApproval;

            // Note: Do not call the legacy Customer.MakePurchase() here — that API is obsolete.
            // This adapter applies the incoming context to the wrapped Customer instance so
            // callers that later rely on Customer state can observe the updated values.
        }
    }
}
