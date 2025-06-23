using ChatSupportSystem.Repositories;

namespace ChatSupportSystem.Services
{
    public class QueueMonitorService : BackgroundService
    {
        private readonly AgentAssignmentService _assignmentService = new();

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                ChatQueue.ActiveSessions.RemoveAll(s =>
                    (DateTime.UtcNow - s.LastPoll).TotalSeconds > 60);

                while (ChatQueue.Sessions.Count > 0)
                {
                    var session = ChatQueue.Sessions.Peek();
                    var assigned = _assignmentService.TryAssignAgent(session);
                    if (assigned)
                        ChatQueue.Dequeue();
                    else
                        break;
                }

                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
