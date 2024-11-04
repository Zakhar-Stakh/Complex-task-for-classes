using System;
using System.Collections.Generic;

// Інтерфейс IProduct
public interface IProduct
{
    string Name { get; set; }
    decimal Price { get; set; }
    decimal CalculateDiscount(decimal discountPercentage);
}

// Абстрактний клас Product
public abstract class Product : IProduct
{
    public string Name { get; set; }
    public decimal Price { get; set; }

    protected Product(string name, decimal price)
    {
        if (string.IsNullOrEmpty(name))
            throw new ArgumentException("Product name cannot be null or empty.");
        if (price < 0)
            throw new ArgumentException("Product price cannot be negative.");

        Name = name;
        Price = price;
    }

    public decimal CalculateDiscount(decimal discountPercentage)
    {
        return Price - (Price * discountPercentage / 100);
    }

    public abstract decimal CalculateCost();
}

// Клас Book
public class Book : Product
{
    public int NumberOfPages { get; set; }

    public Book(string name, decimal price, int numberOfPages)
        : base(name, price)
    {
        NumberOfPages = numberOfPages;
    }

    public override decimal CalculateCost()
    {
        return Price; 
    }
}

// Клас Electronics
public class Electronics : Product
{
    public int MemorySize { get; set; } 

    public Electronics(string name, decimal price, int memorySize)
        : base(name, price)
    {
        MemorySize = memorySize;
    }

    public override decimal CalculateCost()
    {
        return Price; 
    }
}

// Клас Clothing
public class Clothing : Product
{
    public string Size { get; set; }

    public Clothing(string name, decimal price, string size)
        : base(name, price)
    {
        Size = size;
    }

    public override decimal CalculateCost()
    {
        return Price; 
    }
}

// Клас Order
public class Order
{
    public int OrderNumber { get; set; }
    public List<Product> Products { get; set; } = new List<Product>();
    public decimal TotalCost => CalculateTotalCost();

    // Делегат для зміни статусу замовлення
    public delegate void OrderStatusChangedHandler(string status);
    public event OrderStatusChangedHandler OnOrderStatusChanged;

    public Order(int orderNumber)
    {
        OrderNumber = orderNumber;
    }

    private decimal CalculateTotalCost()
    {
        decimal total = 0;
        foreach (var product in Products)
        {
            total += product.CalculateCost();
        }
        return total;
    }

    public void ChangeOrderStatus(string status)
    {
        OnOrderStatusChanged?.Invoke(status);
    }
}

// Клас OrderProcessor
public class OrderProcessor
{
    public void ProcessOrder(Order order)
    {
        // Обробка замовлення
        Console.WriteLine($"Processing order #{order.OrderNumber}...");
        Console.WriteLine($"Total cost: {order.TotalCost:C}");
        order.ChangeOrderStatus("Processed");
    }
}

// Клас NotificationService
public class NotificationService
{
    public void SendNotification(string status)
    {
        Console.WriteLine($"Notification: Order status changed to '{status}'.");
    }
}

// Клас Main
class Program
{
    static void Main(string[] args)
    {
        // Створення продуктів
        var book = new Book("The Great Gatsby", 10.99m, 180);
        var phone = new Electronics("Smartphone", 699.99m, 128);
        var shirt = new Clothing("T-shirt", 19.99m, "M");

        // Створення замовлення
        var order = new Order(1);
        order.Products.Add(book);
        order.Products.Add(phone);
        order.Products.Add(shirt);

        // Налаштування спостерігача
        var notificationService = new NotificationService();
        order.OnOrderStatusChanged += notificationService.SendNotification;

        // Обробка замовлення
        var orderProcessor = new OrderProcessor();
        orderProcessor.ProcessOrder(order);
    }
}