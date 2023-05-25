using MatchNow.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MatchNow.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SaveUserController: ControllerBase
    {
        private readonly MatchNowDb _matchNowDb;
        private readonly IConfiguration _configuration;
        public SaveUserController(MatchNowDb matchNowDb, IConfiguration configuration)
        {
            _matchNowDb = matchNowDb;
            _configuration = configuration;
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult SaveUser(User user)
        {
            var userExists = _matchNowDb.Users.AsNoTracking().Where(u => u.UserId == user.UserId).FirstOrDefault();
            if (userExists != null)
            {
                userExists = user;
                _matchNowDb.Users.Update(userExists);
                _matchNowDb.SaveChanges();
                return Ok("User has been saved successfully!");
            }
            else return NotFound("User does not exist!");
        }
    }
}
