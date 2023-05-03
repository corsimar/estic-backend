using MatchNow;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SocialMedia.Models;
using System.Security.Cryptography;
using System.Text;

namespace SocialMedia.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegisterController : ControllerBase
    {
        private readonly SocialMediaDb _socialMediaDb;
        public RegisterController(SocialMediaDb socialMediaDb)
        {
            _socialMediaDb = socialMediaDb;
        }
        [AllowAnonymous]
        [HttpPost]
        public ActionResult Register([FromBody]User User)
        {
            var user = new User();
            var UserHashedPassword = HashPassword(User.Password);
            if (User != null )
            {
                if(CheckDuplicateEmail(User) == true)
                {
                    return BadRequest("Mail already exists");
                }
                user.Email = User.Email;
                user.UserId = User.UserId;
                user.Password = UserHashedPassword;
                _socialMediaDb.Add(user);
                _socialMediaDb.SaveChanges();
                return Ok("Registration Succesfull");
            }
            else
            {
                return BadRequest("User is missing information");
            }
        }
        [AllowAnonymous]
        private bool CheckDuplicateEmail(User User)
        {
            var userList = _socialMediaDb.Users.ToList();
            foreach(var user in userList)
            {
                if(user.Email == User.Email)
                {
                    return true;
                }
            }
            return false;
        }
        [AllowAnonymous]
        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var passwordBytes = Encoding.UTF8.GetBytes(password);
                var hashedPasswordBytes = sha256.ComputeHash(passwordBytes);
                var base64HashedPasswordBytes = Convert.ToBase64String(hashedPasswordBytes);
                return base64HashedPasswordBytes;
            }
        }


    }
}
