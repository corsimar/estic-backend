using MatchNow.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MatchNow.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MatchUserController: ControllerBase
    {
        private readonly MatchNowDb _matchNowDb;

        public MatchUserController(MatchNowDb matchNowDb)
        {
            _matchNowDb = matchNowDb;
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult FindMatch(int fromId, User user)
        {
            var fromUser = _matchNowDb.Users.AsNoTracking().Where(u => u.UserId == fromId).SingleOrDefault();
            fromUser.Matches = fromUser.Matches - 1;
            _matchNowDb.Users.Update(fromUser);
            _matchNowDb.SaveChanges();

            var age1 = 0;
            var age2 = 0;
            if(user.Age > 99)
            {
                age1 = (int)(user.Age / 100);
                age2 = (int)(user.Age % 100);
            }
            else
            {
                age1 = (int)user.Age;
                age2 = (int)user.Age;
            }

            var height1 = 0;
            var height2 = 0;
            if(user.Height > 999)
            {
                height1 = (int)(user.Height / 1000);
                height2 = (int)(user.Height % 1000);
            }
            else
            {
                height1 = (int)user.Height;
                height2 = (int)user.Height;
            }

            var weight1 = 0;
            var weight2 = 0;
            if(user.Weight > 999 && user.Weight < 10000)
            {
                weight1 = (int)(user.Weight / 100);
                weight2 = (int)(user.Weight % 100);
            }
            else if(user.Weight > 9999 && user.Weight < 1000000)
            {
                weight1 = (int)(user.Weight / 1000);
                weight2 = (int)(user.Weight % 1000);
            }
            else
            {
                weight1 = (int)user.Weight;
                weight2 = (int)user.Weight;
            }

            var matchingUsers = _matchNowDb.Users.AsNoTracking().Where(u => u.Gender == user.Gender
            && u.Age >= age1 && u.Age <= age2
            && u.Height >= height1 && u.Height <= height2
            && u.Weight >= weight1 && u.Weight <= weight2
            && u.ProfileCompleted == true).ToList();

            if (matchingUsers.Count == 0) return NotFound("No such matching founds!");

            if (user.EyeColor != "null")
            {
                matchingUsers = matchingUsers.Where(u => u.EyeColor == user.EyeColor).ToList();
            }

            if (matchingUsers.Count == 0) return NotFound("No such matching founds!");

            if(user.HairColor != "null")
            {
                matchingUsers = matchingUsers.Where(u => u.HairColor == user.HairColor).ToList();
            }

            if (matchingUsers.Count == 0) return NotFound("No such matching founds!");

            if(user.Tattoo != "null")
            {
                matchingUsers = matchingUsers.Where(u => u.Tattoo == user.Tattoo).ToList();
            }

            if (matchingUsers.Count == 0) return NotFound("No such matching founds!");

            string[] personalityTraits = _matchNowDb.StringToArray(user.PersonalityTraits);
            for (int i = 0; i < personalityTraits.Length; i++)
            {
                matchingUsers = matchingUsers.Where(u => u.PersonalityTraits.Contains(personalityTraits[i])).ToList();
                if (matchingUsers.Count == 0) return NotFound("No such matching founds!");
            }

            string[] hobbies = _matchNowDb.StringToArray(user.Hobbies);
            if (hobbies.Length > 0)
            {
                for (int i = 0; i < hobbies.Length; i++)
                {
                    matchingUsers = matchingUsers.Where(u => u.Hobbies.Contains(hobbies[i])).ToList();
                    if (matchingUsers.Count == 0) return NotFound("No such matching founds!");
                }
            }

            string[] sports = _matchNowDb.StringToArray(user.Sports);
            if (sports.Length > 0)
            {
                for (int i = 0; i < sports.Length; i++)
                {
                    matchingUsers = matchingUsers.Where(u => u.Sports.Contains(sports[i])).ToList();
                    if (matchingUsers.Count == 0) return NotFound("No such matching founds!");
                }
            }

            string[] music = _matchNowDb.StringToArray(user.Music);
            if (music.Length > 0)
            {
                for (int i = 0; i < music.Length; i++)
                {
                    matchingUsers = matchingUsers.Where(u => u.Music.Contains(music[i])).ToList();
                    if (matchingUsers.Count == 0) return NotFound("No such matching founds!");
                }
            }

            for (int i = 0; i < matchingUsers.Count; i++)
            {
                var existingMatches = _matchNowDb.Matches.AsNoTracking().Where(m => m.FromId == fromId && m.ToId == matchingUsers[i].UserId).ToList();
                if (existingMatches.Count == 0 && matchingUsers[i].UserId != fromId)
                {
                    var match = new Match();
                    match.FromId = fromId;
                    match.ToId = matchingUsers[i].UserId;

                    DateTime dateTime = DateTime.Now;
                    string date = dateTime.ToString("yyyy-MM-dd HH:mm:ss");

                    match.Date = date;

                    _matchNowDb.Matches.Add(match);
                    _matchNowDb.SaveChanges();

                    return Ok(new { match = matchingUsers[i].UserId });
                }
            }

            return NotFound("No such matching founds!");
        }
    }
}
