using MatchNow.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace MatchNow.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController: ControllerBase
    {
        private readonly MatchNowDb _matchNowDb;
        public UserController(MatchNowDb matchNowDb) {
            _matchNowDb = matchNowDb;
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult GetUser(int userId)
        {
            var user = _matchNowDb.Users.Where(u => u.UserId == userId).SingleOrDefault();
            return Ok(new { User = user });
        }
    }
}
