namespace ApocalypticFastFood;

// Interfaces extracted from the original Customer implementation to preserve contracts
public interface IDiscountStrategy
{
    decimal GetDiscount(CustomerContext ctx);
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
    decimal GetDiscount(CustomerContext ctx);
}
