using MatchNow.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MatchNow.Controllers
{
    [Route("/api/[controller]")]
    [ApiController]
    public class GetMatchesController: ControllerBase
    {
        private readonly MatchNowDb _matchNowDb;

        public GetMatchesController(MatchNowDb matchNowDb)
        {
            _matchNowDb = matchNowDb;
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult GetMatches(int userId)
        {
            var matches = _matchNowDb.Matches.AsNoTracking().Where(m => m.FromId == userId).ToList();

            if (matches.Count == 0) return NotFound("No matches yet!");

            User user;
            string date;
            MatchInfo[] matchInfo = new MatchInfo[matches.Count];
            for(int i = 0; i < matches.Count; i++)
            {
                user = _matchNowDb.Users.AsNoTracking().Where(u => u.UserId == matches[i].ToId).SingleOrDefault();
                date = _matchNowDb.Matches.AsNoTracking().Where(m => m.FromId == userId && m.ToId == matches[i].ToId).SingleOrDefault().Date;

                matchInfo[i] = new MatchInfo();
                matchInfo[i].UserId = user.UserId;
                matchInfo[i].LastName = user.LastName;
                matchInfo[i].FirstName = user.FirstName;
                matchInfo[i].ProfilePhotoURL = user.ProfilePhotoURL;
                matchInfo[i].Date = date;
            }
            var result = matchInfo.OrderByDescending(m => m.Date).ToList();

            return Ok(new {matches = result});
        }
    }
}
