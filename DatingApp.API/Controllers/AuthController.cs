using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using DatingApp.API.Data;
using DatingApp.API.Dtos;
using DatingApp.API.models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace DatingApp.API.Controllers
{
    // api/Auth
    [Route("api/[controller]")]
    [ApiController]

    public class AuthController : ControllerBase
    {
        // Inject Repository, Configuration (for the token stuff done in Login method)
        private readonly IAuthRepository _repo;
        private readonly IConfiguration _config;
        public AuthController(IAuthRepository repo, IConfiguration config)
        {
            _config = config;
            _repo = repo;
        }

        // Two HTTP post methods - register and login - additional part to route
        // DTO - Username and password will be in a JSON object
        //     - Map main model (User class) into simpler objects that get returned or displayed by view.
        [HttpPost("register")]
        public async Task<IActionResult> Register(UserForRegisterDto userForRegisterDto)
        {
            // validate request

            // convert Username to lowercase to store ... no dupes (JJ vs jj)
            userForRegisterDto.Username = userForRegisterDto.Username.ToLower();

            // check to see if Username is taken
            if (await _repo.UserExists(userForRegisterDto.Username))
                return BadRequest("Username already exists");

            // create user
            var userToCreate = new User
            {
                Username = userForRegisterDto.Username
            };

            var createdUser = await _repo.Register(userToCreate, userForRegisterDto.Password);

            // return route of user
            return StatusCode(201);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserForLoginDto userForLoginDto)
        {
            // Check User exists
            var userFromRepo = await _repo.Login(userForLoginDto.Username.ToLower(), userForLoginDto.Password);

            // Return unauthorized message - this doesnt tell them username exists because they can brute force the password 
            if (userFromRepo == null)
                return Unauthorized();

            // Build the Token to return - Info: user Id and Username 
            // Token can be validated by server and doesnt need Database so you can pass data here.
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userFromRepo.Id.ToString()),
                new Claim(ClaimTypes.Name, userFromRepo.Username)
            };

            // We need a Key to assign our token - Unreadable: Hashed
            // Store in AppSettings (appsettings.json) - used in a couple other places (like connection string)
            // Use Instance of IConfiguration to get the configuration section from appsettings.json
            var key = new SymmetricSecurityKey(Encoding.UTF8.
                GetBytes(_config.GetSection("AppSettings:Token").Value));

            // Signing Credentials - args: key from above and the algorithm to hash the key
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            // Token Description - contain claims, signing date, and expiring credentials
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };

            // Token Handler 
            var tokenHandler = new JwtSecurityTokenHandler();

            // Using Handler - Create Token and pass in token handler descriptor
            var token = tokenHandler.CreateToken(tokenDescriptor);

            // Returning Token as an object to client (WriteToken writes it to the response.)
            return Ok(new {
                token = tokenHandler.WriteToken(token)
            });

        }
    }
}