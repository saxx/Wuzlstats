

namespace Wuzlstats.Hubs
{
    public partial class ApiHub
    {

        public void NotifyCallerToReloadStatistics(string league)
        {
            Clients.Caller.reloadStatistics();
        }


        public void NotifyGroupToReloadStatistics(string league)
        {
            Clients.Group(league).reloadStatistics();
        }
    }
}
