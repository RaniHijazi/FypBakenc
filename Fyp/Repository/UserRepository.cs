using Fyp.Dto;
using Fyp.Interfaces;
using Fyp.Models;
using Microsoft.EntityFrameworkCore;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Fyp.Repository
{
    public class UserRepository:IUserRepository
    {

        private readonly DataContext _context;
       
        public UserRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<bool> VerifyVerificationCode(string userEmail, string verificationCode)
        {
            var user = await _context.users.FirstOrDefaultAsync(u => u.Email == userEmail);

            if (user == null || user.VerificationCode != verificationCode)
            {
                return false;
            }
            return true;
        }

        public async Task<User> SignUp(SignUpDto dto)
        {
 
            bool isUAEduLbEmail = dto.Email.EndsWith("@ua.edu.lb");

            
            string role = isUAEduLbEmail ? "student" : "default_role"; 

            
            if (await _context.users.AnyAsync(u => u.FullName == dto.FullName))
            {
                throw new InvalidOperationException("Username already exists");
            }

           
            if (await _context.users.AnyAsync(u => u.Email == dto.Email))
            {
                throw new InvalidOperationException("Email already used");
            }

            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(dto.Password);
            int preCommunityId = 1;

            var newUser = new User
            {
                FullName = dto.FullName,
                Email = dto.Email,
                Password = hashedPassword,
                Gender = dto.Gender,
                Age = dto.Age,
                JoinDate = DateTime.Now,
                Role = role, 
                PrecommunityId = preCommunityId,
            };

            _context.users.Add(newUser);
            await _context.SaveChangesAsync();

            return newUser;
        }



        public async Task<User> SignIn(SignInDto dto)
        {
            var user = await _context.users.FirstOrDefaultAsync(u => u.FullName == dto.FullName);

            if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.Password))
            {
                throw new InvalidOperationException("Invalid username or password");
            }
            return user;
        }

        public async Task<List<User>> GetAllUsers()
        {
            return await _context.users.ToListAsync();
        }

        public async Task<bool> UpdateUserProfile(int userId, UpdateUserDto dto)
        {
            var user = await _context.users.FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
            {
                return false;
            }

            if (!string.IsNullOrEmpty(dto.NFullName) && dto.NFullName != user.FullName)
            {
                if (await _context.users.AnyAsync(u => u.FullName == dto.NFullName))
                {
                    throw new InvalidOperationException("Username already exists");
                }

                user.FullName = dto.NFullName;
            }

            if (!string.IsNullOrEmpty(dto.NPassword))
            {
                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(dto.NPassword);
                user.Password = hashedPassword;
            }

            _context.users.Update(user);
            await _context.SaveChangesAsync();

            return true;
        }

        



    }
}
