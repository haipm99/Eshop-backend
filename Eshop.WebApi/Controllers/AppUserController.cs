using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Eshop.WebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Eshop.WebApi.Controllers
{
    [Route("api/AppUser")]
    [ApiController]
    public class AppUserController : ControllerBase
    {
        protected EshopContext _context;

        public AppUserController(EshopContext context)
        {
            _context = context;
        }
        // url : api/AppUser
        // desc: login
        [HttpPost("")]
        public IActionResult Login(LoginView login)
        {
            var user = _context.AppUsers.FirstOrDefault(u => u.Username == login.username && u.Password == login.password);

            if(user == null)
            {
                return Unauthorized("Username or Password have been wrong !");
            }

            var roleId = _context.UserRoles.FirstOrDefault(r => r.UserId == user.Id);
            var role = _context.AppRoles.FirstOrDefault(aR => aR.Id == roleId.RoleId);

            var resp = new Dictionary<string, object>();
            //generate token
            #region Generate JWT token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.Default.GetBytes(JWT.SECRET_KEY);
            var issuer = JWT.ISSUER;
            var audience = JWT.AUDIENCE;

            var identity = new ClaimsIdentity("Application");
            identity.AddClaim(new Claim(ClaimTypes.Name, user.Id.ToString()));
            identity.AddClaim(new Claim(ClaimTypes.Role, role.DisplayName));
            identity.AddClaim(new Claim(ClaimTypes.Actor, user.Fullname));

            var now = DateTime.UtcNow;
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = issuer,
                Audience = audience,
                Subject = identity,
                IssuedAt = now,
                Expires = now.AddDays(1),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature),
                NotBefore = now
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);
            #endregion

            resp["access_token"] = tokenString;
            resp["token_type"] = "bearer";
            resp["expires_utc"] = tokenDescriptor.Expires;
            resp["issued_utc"] = tokenDescriptor.IssuedAt;

            return Ok(resp);
        }


        // url : api/AppUser/admin
        //desc : login with admin
        [HttpGet("admin")]
        [Authorize(Roles ="admin")]
        public IActionResult AdminOnly()
        {
            return Ok("Hello admin");
        }

        //url : api/AppUser/shopOwner
        //desc : login with shop owner
        [HttpGet("owner")]
        [Authorize(Roles = "shop owner")]
        public IActionResult shopOwner()
        {
            return Ok("Hello Shop owner");
        }

        //url: api/AppUser/register
        // desc : register new account
        [HttpPost("register")]
        public IActionResult Register(AppUsers acc)
        {

            var user = _context.AppUsers.Where(u => u.Username == acc.Username).ToList();

            if(user.Count == 0)
            {
                _context.AppUsers.Add(acc);
                _context.SaveChanges();
                return Ok(acc);
            }

            return BadRequest("user have been exited");
        }

        //url: api/AppUser/update
    }
}
