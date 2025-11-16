namespace ApocalypticFastFood;

// Interfaces extracted from the original Customer implementation to preserve contracts
public interface IDiscountStrategy
{
    double GetDiscount(CustomerContext ctx);
}

public interface IAlcoholPolicy
{
    bool CanOrderAlcohol(CustomerContext ctx);
}

public interface IPurchaseApprovalPolicy
{
    bool CanMakePurchase(CustomerContext ctx);
}

public interface ILoyaltyCalculator
{
    int GetMultiplier(CustomerContext ctx);
}

// Adapter contract used across the codebase
public interface IDiscountProvider
{
    double GetDiscount(CustomerContext ctx);
}
