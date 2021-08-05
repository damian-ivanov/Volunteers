using System.Linq;
using Volunteers.Data;


namespace Volunteers.Services.Stats

{
    public class StatsService : IStatsService
    {
        private readonly VolunteersDbContext data;

        public StatsService(VolunteersDbContext data)
        {
            this.data = data;
        }

        public (int, int, int) GetStats()
        {
            int ProjectsCount = data.Projects.Where(p => p.IsPublic == false).Count();
            int CommentsCount = data.Comments.Where(c => c.IsPublic == false).Count();
            int UsersCount = data.Users.Count();

            return (ProjectsCount,CommentsCount,UsersCount);
        }
    }
}