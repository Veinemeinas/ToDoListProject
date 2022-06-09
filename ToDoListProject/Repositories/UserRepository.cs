using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using ToDoListProject.Context;
using ToDoListProject.Dto;
using ToDoListProject.Model;

namespace ToDoListProject.Repositories
{
    public class UserRepository
    {
        private readonly ToDoListDbContext _dbManagementContext;

        public UserRepository(ToDoListDbContext dbManagementContext)
        {
            _dbManagementContext = dbManagementContext;
        }

        public async Task<User> GetUser(string email, string password)
        {
            var user = await _dbManagementContext.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.Email == email && u.Password == password);
            if (user != null)
            {
                return user;
            }
            return null;
        }

        public string EncodePassword(string password)
        {
            byte[] data = Encoding.ASCII.GetBytes(password);
            data = new SHA256Managed().ComputeHash(data);
            return Encoding.ASCII.GetString(data);
        }

        public async Task CreateUser(UserDto userDto)
        {
            byte[] salt = new byte[128 / 8];
            using (var rngCsp = new RNGCryptoServiceProvider())
            {
                rngCsp.GetNonZeroBytes(salt);
            }

            var hash = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: userDto.Password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8
                ));

            await _dbManagementContext.Users.AddAsync(new User() { Email = userDto.Email, Password = hash, });
            await _dbManagementContext.SaveChangesAsync();
        }
    }
}
