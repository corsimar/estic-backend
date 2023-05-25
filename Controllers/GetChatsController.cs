using MatchNow.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections;

namespace MatchNow.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GetChatsController: ControllerBase
    {
        private readonly MatchNowDb _matchNowDb;

        public GetChatsController(MatchNowDb matchNowDb)
        {
            _matchNowDb = matchNowDb;
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult GetChats(int userId)
        {
            var foundPairs = new List<int>();
            var result = new List<Message>();
            var userNames = new List<string>();
            var profilePhotos = new List<string>();
            var read = new List<Boolean>();

            var chats = _matchNowDb.Messages.AsNoTracking().Where(m => m.ToId == userId || m.FromId == userId).OrderByDescending(m => m.DateSent).ToList();

            int pairId;
            for (int i = 0; i < chats.Count; i++)
            {
                if (chats[i].FromId == userId) pairId = chats[i].ToId;
                else pairId = chats[i].FromId;
                if (!foundPairs.Contains(pairId))
                {
                    foundPairs.Add(pairId);
                }
            }

            for(int i = 0; i < foundPairs.Count; i++)
            {
                result.Add(chats.Where(m => m.FromId == foundPairs[i] || m.ToId == foundPairs[i]).OrderByDescending(m => m.DateSent).FirstOrDefault());
                var user = _matchNowDb.Users.AsNoTracking().Where(u => u.UserId == foundPairs[i]).FirstOrDefault();
                userNames.Add(user.LastName + " " + user.FirstName);
                profilePhotos.Add(user.ProfilePhotoURL);
                var chatReading = _matchNowDb.ChatReadings.AsNoTracking().Where(c => c.FromId == foundPairs[i] && c.ToId == userId).SingleOrDefault();
                if (chatReading == null) read.Add(true);
                else read.Add(chatReading.Read);
            }

            if (chats.Count == 0)
            {
                return NotFound("No messages found!");
            }

            return Ok(new { messages = result, names = userNames, profilePhotos = profilePhotos, read = read });
        }
    }
}
