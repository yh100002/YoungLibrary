using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Mvc.Versioning;
using Models;
using Web.Auth;
using Web.Dto;

namespace Web.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository authRepository;
        private readonly IConfiguration configuration;
        public AuthController(IAuthRepository authRepository, IConfiguration configuration)
        {
            this.authRepository = authRepository;
            this.configuration = configuration;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserForRegiDto userForRegiDto)
        {
            userForRegiDto.UserName = userForRegiDto.UserName.ToLower();

            if (await this.authRepository.PassengerExists(userForRegiDto.UserName))
                return StatusCode(400);
                //return BadRequest("Passenger Name already exists");                

            var userToCreate = new User
            {
                UserName = userForRegiDto.UserName
            };

            var createdPassenger = await this.authRepository.Register(userToCreate, userForRegiDto.Password);

            return StatusCode(201); //Created Status
        }
        
        [HttpPost("login")]
        public async Task<IActionResult> Login(UserForLoginDto userForLoginDto)
        {
            var userFromRepository = await this.authRepository.Login(userForLoginDto.UserName.ToLower(), userForLoginDto.Password);

            if (userFromRepository == null)
                return Unauthorized();

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userFromRepository.UserId.ToString()),
                new Claim(ClaimTypes.Name, userFromRepository.UserName)
            };

            //Passing the super key :) as byte array
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.configuration.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            //Reading super key this is just to show how it works
            var span = Int64.Parse(this.configuration.GetSection("AppSettings:TokenExpireSpan").Value);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(span),//Valid for one day
                SigningCredentials = creds
            };

            /*
            On successful authentication the Authenticate method generates a JWT (JSON Web Token) using the JwtSecurityTokenHandler class 
            that generates a token that is digitally signed using a secret key stored in appsettings.json. 
            The JWT token is returned to the client application 
            which then must include it in the HTTP Authorization header of subsequent web api requests for authentication.
             */

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return Ok(new {token = tokenHandler.WriteToken(token)});
        }
    }
}