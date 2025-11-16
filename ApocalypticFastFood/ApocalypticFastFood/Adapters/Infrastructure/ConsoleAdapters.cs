namespace ApocalypticFastFood;

public class ConsoleEmailSender : IEmailSender
{
    public void Send(string to, string subject, string body = "")
    {
        Console.WriteLine($"Connecting to smtp.example.com:587");
        Console.WriteLine($"Sending to: {to} - {subject}");
    }
}

public class ConsoleSmsSender : ISmsSender
{
    public void Send(string phone, string message = "")
    {
        Console.WriteLine($"Sending SMS to: {phone} - {message}");
    }
}
