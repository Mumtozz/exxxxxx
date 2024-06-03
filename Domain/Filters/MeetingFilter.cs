namespace Domain.Filters;

public class MeetingFilter : PaginationFilter
{
    public string? Name { get; set; }
    public string? Description { get; set; }
}
