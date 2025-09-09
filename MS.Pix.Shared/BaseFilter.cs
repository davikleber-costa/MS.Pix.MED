namespace MS.Pix.Shared;

public abstract class BaseFilter
{
    public int Page { get; set; } = 1;

    public int PageSize { get; set; } = 50;

    public int Offset => (Page - 1) * PageSize;

    public virtual bool IsValid()
    {
        return Page > 0 && PageSize > 0 && PageSize <= 1000;
    }
}