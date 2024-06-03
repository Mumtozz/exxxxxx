namespace Domain.Filters;

public class UserFilter : PaginationFilter
{
    public string? Name { get; set; }
    public string? Email { get; set; }
}