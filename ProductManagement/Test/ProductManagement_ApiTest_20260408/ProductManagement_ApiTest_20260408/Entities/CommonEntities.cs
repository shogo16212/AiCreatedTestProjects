using System.Globalization;

public record class Product(int ProductId, string ProductName, int Stock, int Price, string CreatedAt);
public record class History(int HistoryId, int ProductId, string ProductName, string ActionType, int Amount, string Memo, string CreatedAt);
public record class RecentOperation(string CreatedAt, string ProductName, string ActionType);
public record class Dashboard(int ProductCount, int TotalStock, List<RecentOperation> RecentOperations);