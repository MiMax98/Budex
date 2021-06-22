using BuildingMaterialRent.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BuildingMaterialRent.Controllers
{
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IOptions<ApiBehaviorOptions> _apiBehaviorOptions;

        public AccountController(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            IOptions<ApiBehaviorOptions> apiBehaviorOptions)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _apiBehaviorOptions = apiBehaviorOptions;
        }

        [HttpGet]
        [Route("[controller]")]
        public LoginInfo GetUserInfo()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return LoginInfo.NotSignedIn;
            }
            var roles = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value);
            return LoginInfo.SignedIn(User.Identity.Name, roles.First());
        }

        [HttpPost]
        [Route("[controller]/[action]")]
        public async Task<IActionResult> Login(LoginViewModel model, [FromServices] IOptions<ApiBehaviorOptions> apiBehaviorOptions)
        {
            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, true, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                return Ok();
            }

            ModelState.AddModelError("General", "Nieudana próba logowania");
            return _apiBehaviorOptions.Value.InvalidModelStateResponseFactory(ControllerContext);
        }

        [HttpPost]
        [Route("[controller]/[action]")]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            var user = new User { UserName = model.Email, Email = model.Email, FirstName = model.FirstName, LastName = model.LastName };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                ModelState.AddModelError("General", $"Użytkownik z adresem e-mail {model.Email} już istnieje");
                return _apiBehaviorOptions.Value.InvalidModelStateResponseFactory(ControllerContext);
            }
            await _userManager.AddToRoleAsync(user, "User");
            await _signInManager.SignInAsync(user, isPersistent: false);
            return Ok();
        }

        [HttpPost]
        [Route("[controller]/[action]")]
        public async Task<IActionResult> LogOff()
        {
            await _signInManager.SignOutAsync();
            return Ok();
        }
    }
}
