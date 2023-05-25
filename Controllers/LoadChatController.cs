using MatchNow.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MatchNow.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoadChatController: ControllerBase
    {
        private readonly MatchNowDb _matchNowDb;

        public LoadChatController(MatchNowDb matchNowDb)
        {
            _matchNowDb = matchNowDb;
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult LoadChat(int fromId, int toId)
        {
            var messages = _matchNowDb.Messages.AsNoTracking().Where(m => (m.FromId == fromId && m.ToId == toId) || (m.FromId == toId && m.ToId == fromId)).OrderByDescending(m => m.DateSent).ToList();

            var chatRead = _matchNowDb.ChatReadings.AsNoTracking().Where(c => c.FromId == toId && c.ToId == fromId).SingleOrDefault();
            if (chatRead != null)
            {
                chatRead.Read = true;
                _matchNowDb.ChatReadings.Update(chatRead);
                _matchNowDb.SaveChanges();
            }

            return Ok(new { messages = messages });
        }
    }
}
