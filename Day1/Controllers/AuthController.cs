using Day1.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Day1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;


        public AuthController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        //register
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserDto registerUserDto)
        {
            var user = new IdentityUser
            {
                Email = registerUserDto.email,
                UserName = registerUserDto.email
            };

            var CheckUser = await _userManager.FindByEmailAsync(registerUserDto.email);

            if (CheckUser != null)
                return BadRequest(CheckUser);

            var result = await _userManager.CreateAsync(user , registerUserDto.password);
            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserDto loginUserDto)
        {
            var check = await _userManager.FindByEmailAsync(loginUserDto.email);

            if (check == null)
                return BadRequest("User not found");

            var checkPassword = await _userManager.CheckPasswordAsync(check, loginUserDto.password);
            if (!checkPassword)
                return BadRequest("password is Wrong");

            //Create Token

            var token = CreateToken(check);
            return Ok(token);
        }
        private string CreateToken(IdentityUser user)
        {
            //step1
            //create Claims
            var Claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Email,user.Email??=""),
                new Claim(ClaimTypes.NameIdentifier,user.Id)
            };
            //step2


            //step3
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("YourSecretKey23asldjfklsdjflksdflsadflk"));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);


            var token = new JwtSecurityToken(
                claims: Claims,
                expires: DateTime.UtcNow.AddMinutes(150),
                signingCredentials: credentials
                );

            var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);
            return jwtToken;
        }

    }
}
