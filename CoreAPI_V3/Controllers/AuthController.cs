using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CoreAPI_V3.Models;
using CoreAPI_V3.Interface;
using Google.Apis.Auth;
using CoreAPI_V3.DTO;
using CoreAPI_V3.CommonMethods;
using static CoreAPI_V3.Models.ContextModel;
using Newtonsoft.Json.Linq;
using System.Net;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using CoreAPI_V3.Repository;

namespace CoreAPI_V3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _userRepo;
        private readonly IAdminRepository _adminRepo;  
        private readonly IConfiguration _config;
        private readonly CommonHelper _commonHelper;
        private readonly ApplicationDbContext _context;
        private readonly GetLocation _location;


        public AuthController(IUserRepository userRepo, IAdminRepository adminRepo, IConfiguration config, CommonHelper commonHelper, ApplicationDbContext context, GetLocation location)
        {
            _userRepo = userRepo;
            _adminRepo = adminRepo;
            _config = config;
            _commonHelper = commonHelper;
            _context = context;
            _context = context;
            _location = location;
        }



        [HttpPost("UserLogin")]
        public async Task<IActionResult> UserLogin([FromBody] GoogleAuthDto googleAuthDto)
        {
            try
            {
                GoogleJsonWebSignature.Payload payload;
                try
                {
                    payload = await GoogleJsonWebSignature.ValidateAsync(googleAuthDto.IdToken);
                }
                catch (InvalidJwtException)
                {
                    var ResponseStatus = CommonHelper.UnAuthorized("Invalid Google token.");
                    return StatusCode(StatusCodes.Status401Unauthorized, new { ResponseStatus });
                }
                catch (Exception ex)
                {
                    return StatusCode(500, $"Internal server error: {ex.Message}");
                }

                string userLocale = payload.Locale;

                try
                {
                    if (userLocale == null)
                    {
                        string publicIpAddress = await _location.GetPublicIPAddressAsync();
                        string locationInfo = await _location.GetLocationFromIpAsync(publicIpAddress);
                        userLocale = locationInfo ?? payload.Locale;  
                    }
                }
                catch (Exception ex)
                {
                    userLocale = payload.Locale ?? null; 
                }

                var userEmail = payload.Email;
                var userName = payload.Name;

                var existingUser = await _userRepo.GetUserByEmailAsync(userEmail);
                if (existingUser == null)
                {
                    var newUser = new User
                    {
                        Email = userEmail,
                        Name = userName,
                        Locale = userLocale,
                        EmailVerified = payload.EmailVerified.ToString(),
                        Role = "User",
                        SessionStart = DateTime.Now,
                    };
                    await _userRepo.CreateUserAsync(newUser);
                    existingUser = newUser;
                } 
                else
                {
                    existingUser.Name = userName;
                    existingUser.Locale = userLocale;
                    existingUser.SessionStart = DateTime.Now;
                    await _userRepo.UpdateUserAsync(existingUser);
                }

                var jwtToken = GenerateJwtToken(userEmail, "User");
                var refreshToken = GenerateRefreshToken();
                existingUser.RefreshToken = refreshToken;
                existingUser.RefreshTokenExpiryTime = DateTime.Now.AddDays(3);
                await _userRepo.UpdateUserAsync(existingUser);


                var getUserId = await _userRepo.GetUserByEmailAsync(userEmail);
                
                return Ok(new
                {
                    UserId = getUserId.Id,
                    Email = userEmail,
                    Name = userName,
                    Role = "User",
                    Token = jwtToken,
                    RefreshToken = refreshToken,
                    SessionStart = DateTime.Now

                });
            }
            catch (Exception ex)
            {
                _commonHelper.SendErrorToText("Controller", "Auth", "Login", ex);

                var  ResponseStatus = CommonHelper.UnAuthorized(ex.Message);
                return StatusCode(StatusCodes.Status401Unauthorized, new { ResponseStatus });
            }
        }



        [HttpPost("AdminLogin")]
        public async Task<IActionResult> AdminLogin([FromBody] AdminLoginDto adminLoginDto)
        {
            try
            {
                var admin = await _adminRepo.GetAdminByUsernameAsync(adminLoginDto.Username);
                if (admin == null || !VerifyPassword(adminLoginDto.Password, admin.PasswordHash, admin.PasswordSalt))
                {
                    var ResponseStatus = CommonHelper.UnAuthorized("Invalid username or password");
                    return StatusCode(StatusCodes.Status401Unauthorized, new { ResponseStatus });
                }

                var jwtToken = GenerateJwtToken(admin.Username, "Admin");

                var refreshToken = GenerateRefreshToken();
                admin.RefreshToken = refreshToken;
                admin.RefreshTokenExpiryTime = DateTime.Now.AddDays(3);
                admin.SessionStart= DateTime.Now;
                await _adminRepo.UpdateAdminAsync(admin);


                var getAdminId = await _adminRepo.GetAdminByUsernameAsync(adminLoginDto.Username);

                return Ok(new
                {
                    AdminId = getAdminId.Id,
                    Username = admin.Username,
                    Role = "Admin",
                    Token = jwtToken,
                    RefreshToken = refreshToken,
                    SessionStart = DateTime.Now,

                });
            }
            catch (Exception ex)
            {
                _commonHelper.SendErrorToText("Controller", "Auth", "AdminLogin", ex);

                var ResponseStatus = CommonHelper.UnAuthorized(ex.Message);
                return StatusCode(StatusCodes.Status401Unauthorized, new { ResponseStatus });
            }
           
        }

        private string GenerateJwtToken(string identifier, string role)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, identifier),
                new Claim(ClaimTypes.Role, role)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string GenerateRefreshToken()
        {
            return Guid.NewGuid().ToString();
        }

        private bool VerifyPassword(string enteredPassword, byte[] storedHash, byte[] storedSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(enteredPassword));
                return computedHash.SequenceEqual(storedHash);
            }
        }

        [HttpPost("RefreshToken")]
        public async Task<IActionResult> RefreshToken([FromBody] string refreshToken)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);
            var admin = await _context.Admins.FirstOrDefaultAsync(a => a.RefreshToken == refreshToken);

            if (user == null && admin == null)
            {
                var ResponseStatus = CommonHelper.UnAuthorized("Invalid refresh token");
                return StatusCode(StatusCodes.Status401Unauthorized, new { ResponseStatus });
            }

            string role;
            string identifier;  
            if (user != null)
            {
                role = user.Role;  
                identifier = user.Email;  
                if (user.RefreshTokenExpiryTime <= DateTime.Now || user.SessionStart.AddDays(3) <= DateTime.Now)
                {
                    var ResponseStatus = CommonHelper.UnAuthorized("Refresh token has expired");
                    return StatusCode(StatusCodes.Status401Unauthorized, new { ResponseStatus });
                }
            }
            else
            {
                role = admin.Role;  
                identifier = admin.Username;  
                if (admin.RefreshTokenExpiryTime <= DateTime.Now || admin.SessionStart.AddDays(3) <= DateTime.Now)
                {
                    var ResponseStatus = CommonHelper.UnAuthorized("Refresh token has expired");
                    return StatusCode(StatusCodes.Status401Unauthorized, new { ResponseStatus });
                }
            }

            var newJwtToken = GenerateJwtToken(identifier, role);

            var newRefreshToken = GenerateRefreshToken();

            if (user != null)
            {
                user.RefreshToken = newRefreshToken;
                user.RefreshTokenExpiryTime = DateTime.Now.AddDays(3);
                _context.Users.Update(user);
            }
            else
            {
                admin.RefreshToken = newRefreshToken;
                admin.RefreshTokenExpiryTime = DateTime.Now.AddDays(3);
                _context.Admins.Update(admin);
            }

            await _context.SaveChangesAsync();

            return Ok(new
            {
                Token = newJwtToken,
                RefreshToken = newRefreshToken
            });
        }


    }

}

