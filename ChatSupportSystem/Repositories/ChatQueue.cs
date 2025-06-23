using ChatSupportSystem.Models;
using ChatSupportSystem.Repositories;

public static class ChatQueue 
{
    public static Queue<ChatSession> Sessions { get; } = new();
    public static List<ChatSession> ActiveSessions { get; } = new();

    public static void Enqueue(ChatSession session)
    {
        Sessions.Enqueue(session);
    }

    public static void Dequeue()
    {
        Sessions.Dequeue();
    }

    public static int GetMaxQueueLength()
    {
        var cap = AgentRegistry.GetAgentsOnCurrentShift().Sum(a => a.MaxConcurrency);
        return (int)(cap * 1.5);
    }
}
