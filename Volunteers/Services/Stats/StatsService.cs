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

        public (int, int, int) GetHomePageStats()
        {
            int projectsCount = data.Projects.Where(p => p.IsCompleted == true).Count();
            int townsCount = data.Projects.Where(p => p.IsCompleted == true).Select(c => c.City).Distinct().Count();
            int usersCount = data.Projects.Select(u => u.OwnerId).ToList().Distinct().Count();

            return (projectsCount, townsCount, usersCount);
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