using ChatSupportSystem.Models.Enums;
using ChatSupportSystem.Repositories;

namespace ChatSupportSystem.Models
{

    public class Agent
    {

        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public Seniority Level { get; set; }
        public int Shift { get; set; }
        public int ActiveChats { get; set; }
        public bool IsOverflow { get; set; }

        public double Efficiency => Level switch
        {
            Seniority.Junior => 0.4,
            Seniority.MidLevel => 0.6,
            Seniority.Senior => 0.8,
            Seniority.TeamLead => 0.5,
            _ => 0.4
        };

        public int MaxConcurrency => (int)(10 * Efficiency);
        public bool OnShift => Shift == AgentRegistry.GetCurrentShift();
    }
}
