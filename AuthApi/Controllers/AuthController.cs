using AuthApi.Data;
using AuthApi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<AuthController> _logger;
    private readonly IConfiguration _configuration;
    private readonly UserManager<ApplicationUser> _userManager;
    //UserManager  kullanıcı oluşturma, silme, güncelleme, şifre kontrolü vb. görevi görür.
    private readonly SignInManager<ApplicationUser> _signInManager;
    //SignInManager kullanıcı giriş ve çıkış işlemlerini yönetir.
    public AuthController(
        ApplicationDbContext context,
        ILogger<AuthController> logger,
        IConfiguration configuration,
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager)
    {
        _context = context;
        _logger = logger;
        _configuration = configuration;
        _userManager = userManager;
        _signInManager = signInManager;
    }

    //private string GenerateJwtToken(ApplicationUser user)
    //{
    //    var tokenHandler = new JwtSecurityTokenHandler();
    //    var key = Encoding.ASCII.GetBytes(_configuration["Jwt:SecretKey"]);
    //    var tokenDescriptor = new SecurityTokenDescriptor
    //    {
    //        Subject = new ClaimsIdentity(new Claim[]
    //        {
    //            new Claim(ClaimTypes.Email, user.Email),
    //            new Claim(ClaimTypes.Name, user.UserName)
    //        }),
    //        Expires = DateTime.UtcNow.AddSeconds(int.Parse(_configuration["Jwt:TokenExpireSeconds"])),
    //        Issuer = _configuration["Jwt:Issuer"],
    //        Audience = _configuration["Jwt:Audience"],
    //        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
    //    };
    //    var token = tokenHandler.CreateToken(tokenDescriptor);
    //    return tokenHandler.WriteToken(token);
    //}

    [HttpPost("register")]
    public async Task<ActionResult> Register([FromBody] RegisterRequest registerRequest)
    {
        try
        {
            if (ModelState.IsValid)
            {
                // RegisterRequest modelinden gelen verilerle yeni kullanıcı olusturuyorum.
                var newUser = new ApplicationUser
                {
                    UserName = registerRequest.FirstName,  // Kullanıcı adı e-posta olarak ayarlanıyor
                    Email = registerRequest.Email,
                    FirstName = registerRequest.FirstName,
                    LastName = registerRequest.LastName,
                    //UserName = registerRequest.FirstName + " " + registerRequest.LastName, // Kullanıcı adını oluştur
                    //Email = registerRequest.Email
                };
                var result = await _userManager.CreateAsync(newUser, registerRequest.Password);
                if (result.Succeeded)
                {
                    _logger.LogInformation($"User {newUser.UserName} ({newUser.Email}) has been created.");
                    return StatusCode(201, $"User '{newUser.UserName}' has been created.");
                }
                else
                {
                    throw new Exception(
                       string.Format("Error: {0}", string.Join(" ",
                           result.Errors.Select(e => e.Description))));
                }
            }
            else
            {
                var details = new ValidationProblemDetails(ModelState);
                {
                    details.Type = "<https://tools.ietf.org/html/rfc7231#section-6.5.1>";
                    details.Status = StatusCodes.Status400BadRequest;
                };
                return new BadRequestObjectResult(details);
            }
        }
        catch (Exception e)
        {
            var exceptionDetails = new ProblemDetails();
            {
                exceptionDetails.Detail = e.Message;
                exceptionDetails.Status = StatusCodes.Status500InternalServerError;
                exceptionDetails.Type = "<https://tools.ietf.org/html/rfc7231#section-6.6.1>";
            };
            return StatusCode(StatusCodes.Status500InternalServerError, exceptionDetails);
        }
    }

    [HttpPost("login")]
    public async Task<ActionResult> Login([FromBody] LoginRequest loginRequest)
    {
        try
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(loginRequest.Email);
                if (user == null || !await _userManager.CheckPasswordAsync(user, loginRequest.Password))
                    //throw new Exception("Invalıd login attempt.");
                    return Unauthorized("Invalıd email or password");
                else
                {
                    var SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(
                        System.Text.Encoding.UTF8.GetBytes(
                            _configuration["Jwt:SecretKey"])), SecurityAlgorithms.HmacSha256Signature);

                    //Claim, kullanıcı hakkında bilgi taşır ve token'a eklenir. Bu claim’ler, kullanıcı kimliğini doğrulamak ve yetkilendirme işlemlerini gerçekleştirmek için kullanılır. 
                    var claims = new List<Claim>();
                    claims.Add(new Claim(ClaimTypes.Email, user.Email));
                    claims.Add(new Claim(ClaimTypes.Name, user.UserName));

                    var JwtObject = new JwtSecurityToken(
                        issuer: _configuration["Jwt:Issuer"],//Token'ı oluşturan taraf
                        audience: _configuration["Jwt:Audience"],//
                        claims: claims,
                        expires: DateTime.UtcNow.AddSeconds(int.Parse(_configuration["Jwt:TokenExpireSeconds"])),
                        signingCredentials: SigningCredentials);

                    var jswtString = new JwtSecurityTokenHandler().WriteToken(JwtObject);
                    //return StatusCode(StatusCodes.Status200OK, jswtString);
                    return Ok(new { Token = jswtString, User = new { user.FirstName, user.LastName, user.Email } });
                    //var token = GenerateJwtToken(user);
                    //return Ok(new { Token = token });
                }
                //else
                //{
                //    return Unauthorized();
                //}
            }
            else
            {
            var details = new ValidationProblemDetails(ModelState);
            details.Type = "<https://tools.ietf.org/html/rfc7231#section-6.5.1>";
            details.Status = StatusCodes.Status400BadRequest;
            return new BadRequestObjectResult(details);
            }
        }
        catch (Exception e)
        {
            var exceptionDetails = new ProblemDetails();
            {
                exceptionDetails.Detail = e.Message;
                exceptionDetails.Status = StatusCodes.Status401Unauthorized;
                exceptionDetails.Type = "<https://tools.ietf.org/html/rfc7231#section-6.6.1>";
            };
            return StatusCode(StatusCodes.Status401Unauthorized, exceptionDetails);
        }
    }
}
