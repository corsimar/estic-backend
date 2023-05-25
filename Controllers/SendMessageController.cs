using MatchNow.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MatchNow.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SendMessageController: ControllerBase
    {
        private readonly MatchNowDb _matchNowDb;

        public SendMessageController(MatchNowDb matchNowDb)
        {
            _matchNowDb = matchNowDb;
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult SendMessage(Message message)
        {
            _matchNowDb.Messages.Add(message);

            var existingChat = _matchNowDb.ChatReadings.AsNoTracking().Where(c => c.FromId == message.FromId && c.ToId == message.ToId).Count();
            if (existingChat == 0) 
            {
                ChatReading chatReading = new ChatReading();
                chatReading.FromId = message.FromId;
                chatReading.ToId = message.ToId;
                chatReading.Read = false;
                _matchNowDb.ChatReadings.Add(chatReading);
            }

            _matchNowDb.SaveChanges();
            return Ok("Message sent successfully!");
        }
    }
}
