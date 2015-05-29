using Wuzlstats.Models;

namespace Wuzlstats.ViewModels.Home
{
    public class IndexViewModel
    {


        public IndexViewModel Fill(League league)
        {
            Name = league.Name;

            return this;
        }

  
        public string Name { get; set; } 
    }
}