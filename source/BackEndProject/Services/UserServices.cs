using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Helpers;
using Models;
using Data;

namespace Services
{
    public interface IUserService
    {
        User Authenticate(string username, string password);
        IEnumerable<User> GetAll();
        User GetById(int id);
        User Create(User user, string password);
        void Update(User user, string currentPassword, string password, string confirmPassword);
        string ForgotPassword(string username);
        void Delete(int id);
    }

    public class UserService : IUserService
    {
        private Context _context;
        private readonly IEmailService _emailService;

        public UserService(Context context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        public User Authenticate(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                return null;
            }

            var user = _context.User.FirstOrDefault(x => x.userName == username) ?? null;

            // check if username exists
            if (user == null)
            {
                return null;
            }

            // Granting access if the hashed password in the database matches with the password(hashed in computeHash method) entered by user.
            if(computeHash(password) != user.passwordHash)
            {
                return null;
            }
            return user;        
        }

        public IEnumerable<User> GetAll()
        {
            return _context.User;
        }

        public User GetById(int id)
        {
            return _context.User.Find(id);
        }

        public User Create(User user, string password)
        {
            // validation
            if (string.IsNullOrWhiteSpace(password))
            {
                throw new AppException("Password is required");
            }

            if (_context.User.Any(x => x.userName == user.userName))
            {
                throw new AppException("Username \"" + user.userName + "\" is already taken");
            }

            //Saving hashed password into Database table
            user.passwordHash = computeHash(password);  
            user.levelAccess = null;
            //user.Created = DateTime.UtcNow;
            //user.LastModified = DateTime.UtcNow;

            _context.User.Add(user);
            _context.SaveChanges();

            return user;
        }

        public void Update(User userParam, string currentPassword = null, string password = null, string confirmPassword = null)
        {
            //Find the user by Id
            var user = _context.User.Find(userParam.id);

            if (user == null) 
            {
                throw new AppException("User not found");
            }
            // update user properties if provided
            if (!string.IsNullOrWhiteSpace(userParam.userName) && userParam.userName != user.userName)
            {
                // throw error if the new username is already taken
                if (_context.User.Any(x => x.userName == userParam.userName))
                {
                    throw new AppException("Username " + userParam.userName + " is already taken");
                }
                else
                {
                    user.userName = userParam.userName;
                    //user.LastModified = DateTime.UtcNow;
                }
            }
            if (!string.IsNullOrWhiteSpace(userParam.firstName))
            {
                user.firstName = userParam.firstName;
                //user.LastModified = DateTime.UtcNow;
            }
            if (!string.IsNullOrWhiteSpace(userParam.lastName))
            {
                user.lastName = userParam.lastName;
                //user.LastModified = DateTime.UtcNow;
            }
            if (!string.IsNullOrWhiteSpace(currentPassword))
            {   
                if(computeHash(currentPassword) != user.passwordHash)
                {
                    throw new AppException("Invalid Current password!");
                }

                if(currentPassword == password)
                {
                    throw new AppException("Please choose another password!");
                }
    
                //Updating hashed password into Database table
                user.passwordHash = computeHash(password);
                //user.LastModified = DateTime.UtcNow; 
            }
            
            _context.User.Update(user);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var user = _context.User.Find(id);
            if (user != null)
            {
                _context.User.Remove(user);
                _context.SaveChanges();
            }
        }

        private static string computeHash(string Password)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            var input = md5.ComputeHash(Encoding.UTF8.GetBytes(Password));
            var hashstring = "";
            foreach(var hashbyte in input)
            {
                hashstring += hashbyte.ToString("x2"); 
            } 
            return hashstring;
        }

        public string ForgotPassword(string username)
        {
            if(string.IsNullOrEmpty(username))
            {
                throw new AppException("Valid Username is requred");
            }
            else
            {
                var user = _context.User.SingleOrDefault(x => x.userName == username);
                if(user != null)
                {
                    string password = GenerateRandomCryptographicKey(5);
                    user.passwordHash = computeHash(password);
                    //user.LastModified = DateTime.UtcNow;
                    _context.SaveChanges();
                    
                    var emailAddress = new List<string>(){username};
                    var emailSubject = "Password Recovery";
                    var messageBody = password;

                    var response = _emailService.SendEmailAsync(emailAddress,emailSubject,messageBody);
                    System.Console.WriteLine(response.Result.StatusCode);

                    if(response.IsCompletedSuccessfully)
                    {
                        return new string("If your account exists, your new password will be emailed to you shortly");
                    }
                }
                return new string("If your account exists, your new password will be emailed to you shortly");
            }
        }

        // helper method to generate random password
        private static string GenerateRandomCryptographicKey(int keyLength)
        {
            char[] SPECIAL_CHARACTERS = @"!#$%&*@\".ToCharArray();
            char[] UPPERCASE_CHARACTERS = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
            Random rand = new Random();
            int randomSpecialCharNumber = rand.Next(0,SPECIAL_CHARACTERS.Length -1);
            int randomUppercasChars = rand.Next(0,UPPERCASE_CHARACTERS.Length -1);
            RNGCryptoServiceProvider rngCryptoServiceProvider = new RNGCryptoServiceProvider();
            byte[] randomBytes = new byte[keyLength];
            rngCryptoServiceProvider.GetBytes(randomBytes);
            string hashstring = "";
            foreach(var hashbyte in randomBytes)
            {
                hashstring += hashbyte.ToString("x2"); 
            }
            return UPPERCASE_CHARACTERS[randomUppercasChars] + hashstring + SPECIAL_CHARACTERS[randomSpecialCharNumber];
        }
    }
}