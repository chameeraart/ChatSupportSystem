using ChatSupportSystem.Models.Enums;

namespace ChatSupportSystem.Models
{
    public class ChatSession
    {

        public Guid SessionId { get; set; } = Guid.NewGuid();
        public Guid? AssignedAgentId { get; set; }
        public string? AssignedAgentName { get; set; }
        public Seniority? AssignedAgentLevel { get; set; }
        public DateTime LastPoll { get; set; } = DateTime.UtcNow;

    }
}
