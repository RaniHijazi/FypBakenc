﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Fyp.Dto;
using Fyp.Models;

namespace Fyp.Interfaces
{
    public interface IUserRepository
    {
        Task<bool> VerifyVerificationCode(string userEmail, string verificationCode);
        Task<User> SignUp(SignUpDto dto);
        Task<User> SignIn(SignInDto dto);
        Task<List<User>> GetAllUsers();
        Task<bool> UpdateUserProfile(int userId, UpdateUserDto dto);
    }
}