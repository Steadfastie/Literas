namespace LiterasWebAPI.Models.Responses;

public class EditorResponse
{
    public Guid Id { get; set; }
    public bool IsCreator { get; set; }
    public Guid DocId { get; set; }
    public DocResponse? Doc { get; set; }
    public Guid UserId { get; set; }
    public UserResponseModel? User { get; set; }
}