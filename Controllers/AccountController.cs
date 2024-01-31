using DemoIdentity.DTO;
using DemoIdentity.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Mail;
using System.Net;
using System.Security.Claims;
using System.Text;
using DemoIdentity.Helpers;

namespace DemoIdentity.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
     

        private readonly UserManager<AppUser> userManager;
        private readonly IConfiguration configuration;
        public AccountController(UserManager<AppUser> userManager ,IConfiguration configuration)
        {
            this.userManager = userManager;
            this.configuration = configuration;
        }
        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            if(ModelState.IsValid)
            {
                AppUser user = new AppUser();
                user.FullName = registerDto.FullName;
                
                user.UserName = registerDto.UserName;
               
                var str =OTPGenerator.GenerateOTP();
                user.OTP = str;
                user.OTPGeneratedAt = DateTime.Now;
                IdentityResult result = await userManager.
                   CreateAsync(user, registerDto.Password);
                if (result.Succeeded)
                {
                    return Ok("Rigister Succesfuly ");
                }
                return BadRequest(result.Errors);
            }
            return BadRequest(ModelState);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDTO loginDTO)
        {
            if(ModelState.IsValid)
            {
                var modeluser = await userManager.FindByNameAsync(loginDTO.UserName);
                if (modeluser != null)
                {
                    if(await userManager.CheckPasswordAsync(modeluser ,loginDTO.Password))
                    {
                        // create token 

                        List<Claim> myClaims = new List<Claim>();
                        myClaims.Add(new Claim(ClaimTypes.NameIdentifier, modeluser.Id));
                        myClaims.Add(new Claim(ClaimTypes.Name, modeluser.UserName));
                        myClaims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));

                        var roles = await userManager.GetRolesAsync(modeluser);
                        if(roles != null)
                        {
                            foreach (var role in roles)
                            {
                                myClaims.Add(new Claim(ClaimTypes.Role, role));

                            }
                        }

                        // my cred

                        SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Key"]));
                        SigningCredentials credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                        JwtSecurityToken myToken = new JwtSecurityToken(
                            issuer: configuration["JWT:Issuer"],
                            audience : configuration["JWT:Audience"] ,
                            expires : DateTime.Now.AddHours(1),
                            claims:myClaims,
                            signingCredentials :credentials
                            );

                        var token =new  JwtSecurityTokenHandler().WriteToken(myToken);
                        return Ok(new
                        {
                            Token = token,
                            Exp = myToken.ValidTo
                        });
                    }
                    return BadRequest("Invalid Password");

                }
                return BadRequest("Invalid UserName");
            }
            return BadRequest(ModelState);
        }
    }
}
