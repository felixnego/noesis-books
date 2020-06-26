using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using noesis_api.Helpers;
using noesis_api.Models;
using noesis_api.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Options;
using System.Text;
using System.Security.Claims;
using System.Linq;

namespace noesis_api.Services
{
    public interface IUserService
    {
        Task<User> Authenticate(string username, string password);
        Task<IEnumerable<User>> GetAll();
        Task<User> GetUserById(long id);
        Task<User> Register(User user);
        bool UsernameExists(string username);
        Task<bool> SaveAll();
    }


    public class UserService : IUserService
    {
        private readonly AppSettings _appSettings;
        private readonly NoesisApiContext _context;

        public UserService(IOptions<AppSettings> appSettings, NoesisApiContext context)
        {
            _appSettings = appSettings.Value;
            _context = context;
        }

        public async Task<User> Authenticate(string username, string password)
        {
            var user = await _context.User.SingleOrDefaultAsync(x => x.Username == username && x.Password == HashUtils.GetHashString(password));

            if (user == null) return null;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Role, user.UserRole)
                }),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            user.Token = tokenHandler.WriteToken(token);

            return user;
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            return await _context.User.ToListAsync();
        }

        public async Task<User> GetUserById(long id)
        {
            return await _context.User.FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<User> Register(User user)
        {
            user.CreationDate = DateTime.Now;
            await _context.User.AddAsync(user);

            return user;
        }

        public bool UsernameExists(string username)
        {
            return _context.User.FirstOrDefault(u => u.Username == username) != null;
        }

        public async Task<bool> SaveAll()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
