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
using ToDoListProject.Services;

namespace ToDoListProject.Repositories
{
    public class UserRepository
    {
        private readonly ToDoListDbContext _context;
        private readonly PasswordHash _passwordHash;
        private readonly TokenService _tokenService;
        private readonly IEmailService _emailService;

        public UserRepository(ToDoListDbContext dbManagementContext, PasswordHash passwordHash, TokenService tokenService, IEmailService emailService)
        {
            _context = dbManagementContext;
            _passwordHash = passwordHash;
            _tokenService = tokenService;
            _emailService = emailService;
        }

        public async Task<User> GetUser(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task SendResetPasswordEmail(User user)
        {
            user.ResetPasswordToken = Convert.ToBase64String(_passwordHash.GetSalt());
            user.ResetTokenExpires = DateTime.Now.AddHours(1);
            await _context.SaveChangesAsync();

            _emailService.SendEmail(user.Email, user.ResetPasswordToken);
        }

        public async Task<bool> ValidateToken(PasswordDto passwordDto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.ResetPasswordToken == passwordDto.Token);
            if (user == null || user.ResetTokenExpires < DateTime.Now)
            {
                return false;
            }

            var newSalt = _passwordHash.GetSalt();
            var passwordHash = _passwordHash.EncryptPassword(passwordDto.NewPassword, newSalt);

            user.PasswordSalt = Convert.ToBase64String(newSalt);
            user.PasswordHash = passwordHash;
            user.ResetPasswordToken = null;
            user.ResetTokenExpires = null;
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<string> GenerateToken(AuthDto authDto)
        {
            var user = await _context.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.Email == authDto.Email);
            if (user == null)
            {
                return null;
            }

            var passwordHash = _passwordHash.EncryptPassword(authDto.Password, Convert.FromBase64String(user.PasswordSalt));
            if (user.PasswordHash == passwordHash)
            {
                return _tokenService.CreateToken(user);
            }

            return null;
        }

        public async Task<bool> CreateUser(RegistedDto registerDto)
        {
            var userExist = await _context.Users.FirstOrDefaultAsync(u => u.Email == registerDto.Email);

            if (userExist == null)
            {
                byte[] salt = _passwordHash.GetSalt();
                string saltString = Convert.ToBase64String(salt);
                string hash = _passwordHash.EncryptPassword(registerDto.Password, salt);
                await _context.Users.AddAsync(new User() { Email = registerDto.Email, PasswordHash = hash, PasswordSalt = saltString });
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }
    }
}
