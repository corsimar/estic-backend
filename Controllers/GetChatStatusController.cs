using MatchNow.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MatchNow.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GetChatStatusController: ControllerBase
    {
        private readonly MatchNowDb _matchNowDb;

        public GetChatStatusController(MatchNowDb matchNowDb)
        {
            _matchNowDb = matchNowDb;
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult GetChatStatus(int fromId, int toId)
        {
            Boolean result = _matchNowDb.ChatReadings.AsNoTracking().Where(c => c.FromId == fromId && c.ToId == toId).SingleOrDefault().Read;
            return Ok(new { status = result });
        }
    }
}
