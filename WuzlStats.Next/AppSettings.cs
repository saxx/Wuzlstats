
namespace Wuzlstats
{
    public class AppSettings
    {
        public AppSettings(IConfiguration configuration)
        {
            DatabaseConnectionString = Get(configuration, "DatabaseConnectionString",
                "Data Source=(localdb)\\ProjectsV12;Initial Catalog=Wuzlstats;Integrated Security=True;MultipleActiveResultSets=True;");
            DaysForStatistics = Get(configuration, "DaysForStatistics", 90);
        }


        public string DatabaseConnectionString { get; set; }

        public int DaysForStatistics { get; set; }


        private int Get(IConfiguration configuration, string key, int defaultValue)
        {
            int value;
            return int.TryParse(Get(configuration, key, defaultValue.ToString()), out value) ? value : defaultValue;
        }


        private static string Get(IConfiguration configuration, string key, string defaultValue)
        {
            return configuration.GetValue(key, defaultValue);
        }
    }
}
