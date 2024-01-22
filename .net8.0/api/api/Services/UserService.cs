using api.Data;
using api.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace api.Services
{
    
    public class UserService
    {
        private readonly DBcontext _dbcontext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _config;
        public UserService(DBcontext dbcontext, UserManager<ApplicationUser> userManager, IConfiguration config, SignInManager<ApplicationUser> signInManager)
        {
            _dbcontext = dbcontext;
            _userManager = userManager;
            _config = config;
            _signInManager = signInManager;
        }

        public async Task<Usertoken> Register(User userModel)
        {
            var user = new ApplicationUser { UserName = userModel.Email, Email = userModel.Email };
            var result = await _userManager.CreateAsync(user, userModel.Password);
            _dbcontext.SaveChanges();
            return buildToken(userModel);
        }

        public async Task<Usertoken> Login(User userModel)
        {
            var result = await _signInManager.PasswordSignInAsync(userModel.Email, userModel.Password,
                 isPersistent: false, lockoutOnFailure: false);
            return buildToken(userModel);
            
        }

        private Usertoken buildToken(User userModel)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.UniqueName, userModel.Email),
                new Claim("chave_secreta", "123"),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:chave_superSecreta"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expiration = DateTime.UtcNow.AddDays(1);

            JwtSecurityToken token = new JwtSecurityToken(
                issuer: null,
                audience: null,
                claims: claims,
                expires: expiration,
                signingCredentials: creds
                );

            return new Usertoken()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = expiration,
            };
        }
    }
}
