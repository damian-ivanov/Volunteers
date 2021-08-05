namespace Volunteers.Services.Stats

{
    public interface IStatsService
    {
        public (int, int, int) GetStats();

    }
}