namespace Volunteers.Services.Comments

{
    public interface ICommentService
    {
        string Add(string content, string ProjectId, string UserId);
    }
}