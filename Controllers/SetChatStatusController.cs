using MatchNow.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MatchNow.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SetChatStatusController: ControllerBase
    {
        private readonly MatchNowDb _matchNowDb;

        public SetChatStatusController(MatchNowDb matchNowDb)
        {
            _matchNowDb = matchNowDb;
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult SetChatStatus(int fromId, int toId)
        {
            var row = _matchNowDb.ChatReadings.AsNoTracking().Where(c => c.FromId == fromId && c.ToId == toId).SingleOrDefault();
            row.Read = true;
            _matchNowDb.ChatReadings.Update(row);
            _matchNowDb.SaveChanges();
            return Ok();
        }
    }
}
