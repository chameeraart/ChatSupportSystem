
using ChatSupportSystem.Models;
using ChatSupportSystem.Models.Enums;
using ChatSupportSystem.Repositories;

namespace ChatSupportSystem.Services
{
    public class AgentAssignmentService
    {
        private int _lastAssignedIndex = -1;

        public bool TryAssignAgent(ChatSession session)
        {
            var agentsInShift = AgentRegistry.GetAgentsOnCurrentShift();
            var useOverflow = ChatQueue.Sessions.Count >= ChatQueue.GetMaxQueueLength();

            if (useOverflow && AgentRegistry.IsOfficeHours())
            {
                agentsInShift.AddRange(AgentRegistry.GetOverflowAgents());
            }

            var priority = new[] { Seniority.Junior, Seniority.MidLevel, Seniority.TeamLead, Seniority.Senior };

            foreach (var level in priority)
            {
                var candidates = agentsInShift
                    .Where(a => a.Level == level && a.ActiveChats < a.MaxConcurrency)
                    .OrderBy(a => a.ActiveChats)
                    .ToList();

                if (candidates.Any())
                {
                    _lastAssignedIndex = (_lastAssignedIndex + 1) % candidates.Count;
                    var agent = candidates[_lastAssignedIndex];

                    agent.ActiveChats++;
                    session.AssignedAgentId = agent.Id;
                    session.AssignedAgentName = agent.Name;
                    session.AssignedAgentLevel = agent.Level;
                    ChatQueue.ActiveSessions.Add(session);
                    return true;
                }
            }

            return false;
        }
    }
}
