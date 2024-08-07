﻿using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using wot_api.Classes;
using wot_api.Data;
using wot_api.Entities;

namespace wot_api.Controllers
{
    [ApiController]
    [Route("users")]
    public class UsersController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly AuthService _authService;

        public UsersController(DataContext context, AuthService authService)
        {
            _context = context;
            _authService = authService;
        }

        [HttpPost("register")]
        public IActionResult RegisterUser([FromBody] Users user)
        {
            try
            {
                if (user == null)
                {
                    return BadRequest("Invalid user data");
                }
                
                bool isPasswordValid = this.IsPasswordValid(user.Password, out string errorPasswordMessage);
                bool isEmailValid = this.IsEmailValid(user.Email, out string errorEmailMessage);
                bool isDupplicateUsers = this.IsDupplicateUser(user, out string errorDupplicateUser);

                if (!isPasswordValid)
                {
                    return BadRequest(errorPasswordMessage);
                }

                if (!isEmailValid)
                {
                    return BadRequest(errorEmailMessage);
                }

                if (isDupplicateUsers)
                {
                    return BadRequest(errorDupplicateUser);
                }

                

                var passwordEncryption = new DataProtection().HashPassword(user);

                user.Password = passwordEncryption.HashPassword;
                user.Salt = passwordEncryption.Salt;

                _context.Users.Add(user);
                _context.SaveChanges();

                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("login")]
        public IActionResult LoginUser([FromBody] Users user)
        {
            try
            {
                if (user == null)
                {
                    return NotFound();
                }

                var existingUsers = _context.Users.FirstOrDefault(u => u.Email == user.Email);

                if (existingUsers != null)
                {
                    var convertPass = new DataProtection().VerifyHashPassword(user.Password, existingUsers.Salt);

                    if (convertPass == existingUsers.Password)
                    {
                        var token = _authService.GenerateJwtToken(existingUsers.Email);
                        return Ok( new { token });
                    }
                }


                return NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        private Boolean IsPasswordValid(string password, out string errorPasswordMessage)
        {
            var passwordLength = password.Length;
            var hasSpecialCharacters = password.Any(ch => ! char.IsLetterOrDigit(ch));
            errorPasswordMessage = null;

            if(passwordLength < 6)
            {
                errorPasswordMessage = "Please enter a password which contains more than 5 characters.";
                return false;
            } 
            else if (!hasSpecialCharacters)
            {
                errorPasswordMessage = "Password should contain at least one speacial character '@#$%^&*'";
                return false;
            }

            return true;
        }

        private Boolean IsEmailValid(string email, out string errorEmailMessage)
        {
            var trimmedEmail = email.Trim();

            if (trimmedEmail.EndsWith("."))
            {
                errorEmailMessage = "Email is invalid";
                return false;
            }
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);

                errorEmailMessage = null;
                return addr.Address == trimmedEmail;
            }
            catch
            {
                errorEmailMessage = "Email is not valid";
                return false;
            }
        }

        private bool IsDupplicateUser(Users user, out string errorDupplicateUser)
        {
            var userEmail = _context.Users.FirstOrDefault(u => u.Email == user.Email);

            if (userEmail != null)
            {
                errorDupplicateUser = "User already exists.";
                return true;
            }

            errorDupplicateUser = null;
            return false;
        }

    }
}
