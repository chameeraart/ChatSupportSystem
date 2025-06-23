using ChatSupportSystem.Models;
using ChatSupportSystem.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ChatSupportSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {

        [HttpPost("start")]
        public IActionResult StartSession()
        {
            var session = new ChatSession();

            var baseCapacity = AgentRegistry.GetAgentsOnCurrentShift().Sum(a => a.MaxConcurrency);
            var maxQueueLength = (int)(baseCapacity * 1.5);

            if (AgentRegistry.IsOfficeHours())
            {
                var overflowCapacity = AgentRegistry.GetOverflowAgents().Sum(a => a.MaxConcurrency);
                var overflowQueue = (int)(overflowCapacity * 1.5);
                maxQueueLength += overflowQueue;
            }

            if (ChatQueue.Sessions.Count >= maxQueueLength)
                return BadRequest(new { status = "queue full" });

            ChatQueue.Sessions.Enqueue(session);
            return Ok(new { sessionId = session.SessionId });
        }


        [HttpGet("poll")]
        public IActionResult PollStatus([FromQuery] Guid sessionId)
        {
            var session = ChatQueue.ActiveSessions.FirstOrDefault(s => s.SessionId == sessionId);
            if (session != null)
            {
                session.LastPoll = DateTime.UtcNow;
                return Ok(new
                {
                    status = "assigned",
                    agentId = session.AssignedAgentId,
                    agentName = session.AssignedAgentName,
                    agentLevel = session.AssignedAgentLevel.ToString()
                });
            }

            var queuedSession = ChatQueue.Sessions.FirstOrDefault(s => s.SessionId == sessionId);
            if (queuedSession != null)
            {
                queuedSession.LastPoll = DateTime.UtcNow;
                return Ok(new { status = "queued" });
            }

            return NotFound(new { status = "expired" });
        }


    }
}