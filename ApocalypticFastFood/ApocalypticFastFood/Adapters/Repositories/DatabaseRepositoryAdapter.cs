namespace ApocalypticFastFood;

public class DatabaseRepositoryAdapter : IOrderRepository
{
    private readonly DatabaseService _db;
    public DatabaseRepositoryAdapter(DatabaseService db) => _db = db;
    public void SaveOrder(int orderId, double total, double discount) => _db.SaveOrder(orderId, total, discount);
}

public class DatabaseService
{
    public void SaveOrder(int orderId, double total, double disc)
    {
        Console.WriteLine("Saving order " + orderId + " to database");
    }
}
