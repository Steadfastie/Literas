﻿namespace LiterasModels.Responses;

public class DocResponseModel
{
    public Guid Id { get; set; }
    public Guid CreatorId { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
}