﻿using System;
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
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Identity;
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

            var user = _context.AppUsers.FirstOrDefault(u => u.Username == login.username);

            if(user == null)
            {
                return Unauthorized("Username or Password have been wrong");
            }
            string saltSave = user.Password.Split("|")[1];
            string password2 = user.Password.Split("|")[0];

            string []byteArrString = saltSave.Split(" ");
            byteArrString = byteArrString.Take(byteArrString.Count() - 1).ToArray();
            var salt = Array.ConvertAll(byteArrString, Byte.Parse); ;
            
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                   password: login.password,
                   salt: salt,
                   prf: KeyDerivationPrf.HMACSHA1,
                   iterationCount: 10000,
                   numBytesRequested: 256 / 8));
            
            if(password2 != hashed)
            {
                return Unauthorized(salt);
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
                byte[] salt = new byte[128 / 8];
                using (var rng = RandomNumberGenerator.Create())
                {
                    rng.GetBytes(salt);
                }
                string saltSave = "";
                for(int i = 0; i < salt.Length; i++)
                {
                    saltSave = saltSave + salt[i] + " ";
                }
                //Console.WriteLine($"Salt: {Convert.ToBase64String(salt)}");

                // derive a 256-bit subkey (use HMACSHA1 with 10,000 iterations)
                string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                    password: acc.Password,
                    salt: salt,
                    prf: KeyDerivationPrf.HMACSHA1,
                    iterationCount: 10000,
                    numBytesRequested: 256 / 8));
                acc.Password = hashed+"|"+ saltSave;
                _context.AppUsers.Add(acc);
                _context.SaveChanges();
                return Ok(salt);
            }

            return BadRequest("user have been exited");
        }

        //url: api/AppUser/update
    }
}
