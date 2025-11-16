namespace ApocalypticFastFood;

public class DatabaseRepositoryAdapter : IOrderRepository
{
    private readonly DatabaseService _db;
    public DatabaseRepositoryAdapter(DatabaseService db) => _db = db;
    public void SaveOrder(int orderId, decimal total, decimal discount) => _db.SaveOrder(orderId, total, discount);
}

public class DatabaseService
{
    public void SaveOrder(int orderId, decimal total, decimal disc)
    {
        Console.WriteLine("Saving order " + orderId + " to database");
    }
}
