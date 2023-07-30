namespace LiterasData.Entities;

public class Editor
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }
    public User User { get; set; }

    public Guid DocId { get; set; }
    public Doc Doc { get; set; }
}