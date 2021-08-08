namespace Volunteers.Services.Stats

{
    public interface IStatsService
    {
        public (int, int, int) GetStats();

        public (int, int, int) GetHomePageStats();

    }
}