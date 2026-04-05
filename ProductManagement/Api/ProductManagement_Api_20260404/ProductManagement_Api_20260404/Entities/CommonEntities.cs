public record class JwtLoginData(string? username, string? password);
public record class PostProduct(string? ProductName, int? Stock, int? Price);
public record class PostHistory(int? ProductId, string? ActionType, int? Amount, string? Memo);