namespace Volunteers.Services.Users

{
    public interface IUserService
    {
        public bool IsOwner(string projectId, string userId);

        public bool IsAdministrator(string userId);
    }
}