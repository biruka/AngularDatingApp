using System;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Configuration;
using System.Text;
using DatingApp.API.Data;
using DatingApp.API.Models;
using DatingApp.API.Dtos;

namespace DatingApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        IAuthRepositary _repo;
        IConfiguration _config;
        public AuthController(IAuthRepositary repo, IConfiguration config)
        {
            _repo = repo;
            _config = config;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody]UserForRegisterDto userForRegisterDto)
        {
            //validate request
            if(!ModelState.IsValid){
                return BadRequest(ModelState);
            }
            userForRegisterDto.Username = userForRegisterDto.Username.ToLower();
            if(await _repo.UserExists(userForRegisterDto.Username)){
                return BadRequest("Username already exists");
            }
            var userToCreate = new User
            {
                 UserName = userForRegisterDto.Username,
            };
            var createdUser = await _repo.Register(userToCreate,userForRegisterDto.Password);
            
            return StatusCode(201);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserForLoginDto userForLoginDto)
        {
              var userFromRepo = await _repo.Login(userForLoginDto.username.ToLower(), userForLoginDto.password);
              if(userFromRepo == null)
              {
                  return Unauthorized();
              }

              var claims = new[] 
              {
                  new Claim(ClaimTypes.NameIdentifier, userFromRepo.Id.ToString()),
                  new Claim(ClaimTypes.Name, userFromRepo.UserName)
              };

              var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:Token").Value));

              var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

              var tokenDescriptor = new SecurityTokenDescriptor{
                  Subject = new ClaimsIdentity(claims) ,
                  Expires = DateTime.Now.AddDays(1),
                  SigningCredentials = creds
              };

              var tokenHandler = new JwtSecurityTokenHandler();

              var token = tokenHandler.CreateToken(tokenDescriptor);

              return Ok(new {
                    token = tokenHandler.WriteToken(token)
              });        
        }
    }
}