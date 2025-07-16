using AutoMapper;
using IntricomMVC.Data;
using IntricomMVC.DTO;
using IntricomMVC.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace IntricomMVC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        public IUserService _userService { get; }

        public UserController(
                                    ApplicationDbContext context,
                                    IHttpContextAccessor httpContextAccessor,
                                    UserManager<IdentityUser> userManager,
                                    SignInManager<IdentityUser> signInManager,
                                    IMapper mapper,
                                    IConfiguration configuration,
                                    IUserService userService
            )
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
            _configuration = configuration;
            _userService = userService;
        }

        [HttpPost("registerUser")]
        [AllowAnonymous]
        public async Task<ActionResult<RSAuthenticationDTO>> RegisterUser(UserCredentialsDTO userCredentials)
        {
            var user = new IdentityUser
            {
                UserName = userCredentials.Email,
                Email = userCredentials.Email,
                PasswordHash = userCredentials.Password,
                EmailConfirmed = true,
                LockoutEnabled = true,
            };
            var resultado = await _userManager.CreateAsync(user, userCredentials.Password!);
            if (resultado.Succeeded)
            {
                var respuestaAutenticacion = await BuildToken(userCredentials);
                return respuestaAutenticacion;
            }
            foreach (var error in resultado.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return ValidationProblem();
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<RSAuthenticationDTO>> Login(UserCredentialsDTO userCredentials)
        {
            var usuario = await _userManager.FindByEmailAsync(userCredentials.Email);
            if (usuario is null)
            {
                return ReturnIncorrectLogin();
            }

            var resultado = await _signInManager.CheckPasswordSignInAsync(usuario, userCredentials.Password!, lockoutOnFailure: false);

            if (resultado.Succeeded)
            {
                return await BuildToken(userCredentials);
            }
            else
            {
                return ReturnIncorrectLogin();
            }
        }

        [HttpPost("makeAdmin")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "Admin")]
        public async Task<ActionResult> MakeAdmin(ClaimEditDTO claimEdit)
        {
            var user = await _userManager.FindByEmailAsync(claimEdit.Email);
            if (user is null)
                return NotFound();

            await _userManager.AddClaimAsync(user, new Claim("Admin", "true"));
            return NoContent();
        }

        [HttpPost("removeAdmin")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "Admin")]
        public async Task<ActionResult> RemoveAdmin(ClaimEditDTO claimEdit)
        {
            var user = await _userManager.FindByEmailAsync(claimEdit.Email);
            if (user is null)
                return NotFound();

            await _userManager.RemoveClaimAsync(user, new Claim("Admin", "true"));
            return NoContent();
        }

        private async Task<RSAuthenticationDTO> BuildToken(UserCredentialsDTO userCredentials)
        {
            var claims = new List<Claim>
            {
                new ("email", userCredentials.Email),
                new ("userEmail", userCredentials.Email)
            };

            var usuario = await _userManager.FindByEmailAsync(userCredentials.Email);
            var claimsDB = await _userManager.GetClaimsAsync(usuario!);

            claims.AddRange(claimsDB);

            var llave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["jwt:key"]!));
            var credenciales = new SigningCredentials(llave, SecurityAlgorithms.HmacSha256);

            var expiracion = DateTime.UtcNow.AddYears(1);

            var tokenDeSeguridad = new JwtSecurityToken(issuer: null, audience: null,
                claims: claims, expires: expiracion, signingCredentials: credenciales);

            var token = new JwtSecurityTokenHandler().WriteToken(tokenDeSeguridad);

            return new RSAuthenticationDTO
            {
                Token = token,
                Expiration = expiracion
            };
        }
        private ActionResult ReturnIncorrectLogin()
        {
            ModelState.AddModelError(string.Empty, "Login incorrecto");
            return ValidationProblem();
        }

    }
}
