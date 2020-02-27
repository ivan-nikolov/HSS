namespace Hss.Data.Models
{
    public class TeamService
    {
        public string TeamId { get; set; }

        public Team Team { get; set; }

        public int ServiceId { get; set; }

        public Service Service { get; set; }
    }
}
