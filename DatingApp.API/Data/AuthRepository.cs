using System;
using System.Threading.Tasks;
using DatingApp.API.models;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Data
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DataContext _context;
        
        // Constructor to pass in and set DataContext
        public AuthRepository(DataContext context)
        {
            _context = context;

        }
        public async Task<User> Login(string username, string password)
        {
            // Use username to identify user in DB and store in variable
            // Store variable - Go out to database (_context) - Lambda expression to find user we are looking for
            // Using FirstOrDefaultAsync - Returns a username or null if not found (unlike FirstAsync)
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Username == username);

            if(user == null)
                return null;

            // Use password to compare the hashed password - compute the hash the password generates to the one in DB
            if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
                return null;

            return user;
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            // We will use the same HMACSHA512 but also pass in the KEY - computes the hash
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

                // ComputedHash is a byte[] - Need to loop over each element in the array
                for(int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != passwordHash[i]) return false;
                }
            }
            return true;
        }

        // Adds User entity to database & storing the Hash/Salt for the password
        public async Task<User> Register(User user, string password)
        {
            // Variables - HASH, KEY(SALT)
            byte[] passwordHash, passwordSalt;

            // Passing reference to passwordHash and passwordSalt (not value) - use "out" keyword
            CreatePasswordHash(password, out passwordHash, out passwordSalt);

            // Store variables
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            // Add variables to DB - Returning Task means specify async on this method
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return user;
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            // Generate KEY(SALT)/HASH
            // HMACSHA512 implements iDesposible - Dispose method calls for the using statement (all contents are disposed)
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                // Key getter - already generated
                passwordSalt = hmac.Key;
                // Hash method - convert password to a byte array from string for ComputeHash method
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        public async Task<bool> UserExists(string username)
        {
            // Compare username to any other user in DB
            if (await _context.Users.AnyAsync(x => x.Username == username))
                return true;

            return false;
        }
    }
}