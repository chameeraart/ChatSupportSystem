
using ChatSupportSystem.Models;
using ChatSupportSystem.Models.Enums;

namespace ChatSupportSystem.Repositories
{
    public static class AgentRegistry 
    {
        public static List<Agent> Agents { get; set; } = new();
        public static List<Agent> OverflowAgents { get; set; } = new();

        public static void InitializeDefaultAgents()
        {
            Agents = new List<Agent>
        {
            new Agent { Name = "A-Chameera", Level = Seniority.TeamLead, Shift = 1 },
            new Agent { Name = "A-Chathun", Level = Seniority.MidLevel, Shift = 1 },
            new Agent { Name = "A-Chathuranghi", Level = Seniority.MidLevel, Shift = 1 },
            new Agent { Name = "A-kitmal", Level = Seniority.Junior, Shift = 1 },

            new Agent { Name = "B-Ashan", Level = Seniority.Senior, Shift = 2 },
            new Agent { Name = "B-Heshan", Level = Seniority.MidLevel, Shift = 2 },
            new Agent { Name = "B-chathura", Level = Seniority.Junior, Shift = 2 },
            new Agent { Name = "B-Dinech", Level = Seniority.Junior, Shift = 2 },

            new Agent { Name = "C-Amali", Level = Seniority.MidLevel, Shift = 3 },
            new Agent { Name = "C-Nuwan", Level = Seniority.MidLevel, Shift = 3 }
        };

            var currentShift = GetCurrentShift();

            OverflowAgents = new List<Agent>();
            for (int i = 1; i <= 6; i++)
            {
                OverflowAgents.Add(new Agent
                {
                    Name = $"Overflow{i}",
                    Level = Seniority.Junior,
                    IsOverflow = true,
                    Shift = currentShift
                });
            }
        }

        public static List<Agent> GetOverflowAgents() => OverflowAgents;

        public static int GetCurrentShift()
        {
            var nowUtc = DateTime.UtcNow;
            var sriLankaZone = TimeZoneInfo.FindSystemTimeZoneById("Sri Lanka Standard Time");
            var localTime = TimeZoneInfo.ConvertTimeFromUtc(nowUtc, sriLankaZone);
            var hour = localTime.Hour;

            if (hour >= 0 && hour < 8) return 1;
            if (hour >= 8 && hour < 16) return 2;
            return 3;
        }

        public static List<Agent> GetAgentsOnCurrentShift() =>
            Agents.Where(a => a.Shift == GetCurrentShift()).ToList();

        public static bool IsOfficeHours()
        {
            var now = DateTime.UtcNow;
            var local = TimeZoneInfo.ConvertTimeFromUtc(now, TimeZoneInfo.FindSystemTimeZoneById("Sri Lanka Standard Time"));
            return local.Hour >= 9 && local.Hour < 17;
        }
    }
}
