using MatchNow.DTOs;
using MatchNow.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace SocialMedia.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegisterController : ControllerBase
    {
        private readonly MatchNowDb _matchNowDb;
        private readonly IConfiguration _config;
        public RegisterController(MatchNowDb matchNowDb, IConfiguration config)
        {
            _matchNowDb = matchNowDb;
            _config = config;
        }
        [AllowAnonymous]
        [HttpPost]
        public ActionResult Register([FromBody]User User)
        {
            var user = new User();
            var UserHashedPassword = HashPassword(User.Password);
            if (User != null)
            {
                if(CheckDuplicateEmail(User) == true)
                {
                    return BadRequest("This email is already in use!");
                }
                user = User;
                user.Password = UserHashedPassword;
                _matchNowDb.Add(user);
                _matchNowDb.SaveChanges();
                var jwt = GenerateJSONWebToken(user);
                return Ok(new { token = jwt, userId = user.UserId });
            }
            else
            {
                return BadRequest("User is missing information");
            }
        }
        [AllowAnonymous]
        private bool CheckDuplicateEmail(User User)
        {
            var UserDuplicate = _matchNowDb.Users
                .Where(x => x.Email == User.Email)
                .FirstOrDefault();
            if (UserDuplicate == null)
            {
                return false;
            }
            return true;
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

        [AllowAnonymous]
        private string GenerateJSONWebToken(User userInfo)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[] {
        new Claim(JwtRegisteredClaimNames.Email, userInfo.Email),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        new Claim("UserId", userInfo.UserId.ToString())

    };

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                _config["Jwt:Issuer"],
                claims,
                expires: DateTime.Now.AddMinutes(120),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
