using MatchNow.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MatchNow.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GetNewMessagesController: ControllerBase
    {
        private readonly MatchNowDb _matchNowDb;

        public GetNewMessagesController(MatchNowDb matchNowDb)
        {
            _matchNowDb = matchNowDb;
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult GetNewMessages(int userId)
        {
            var newMessagesCount = _matchNowDb.ChatReadings.AsNoTracking().Where(c => c.ToId == userId && c.Read == false).Count();
            return Ok(new { messageCount = newMessagesCount });
        }
    }
}
