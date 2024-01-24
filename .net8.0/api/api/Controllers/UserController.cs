using api.Model;
using api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;
        public UserController(UserService userService)
        {
            _userService = userService;
        }
        /// <summary>
        /// Register
        /// </summary>
        /// <returns>IActionResult</returns>
        /// <response code="201">Caso inserção seja feita com sucesso</response>                                                                                                                    
        [HttpPost("register")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Register(User user)
        {
            var result = await _userService.Register(user);
            return Ok(result);
        }

        /// <summary>
        /// Logar
        /// </summary>
        /// <returns>IActionResult</returns>
        /// <response code="201">Caso inserção seja feita com sucesso</response>
        [HttpPost("login")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Login(User user)
        {
            var result = await _userService.Login(user);
            if (result == null)
            {
                return NotFound(new { message = "Usuário não encontrado" });
            }

            return Ok(result);
        }
    }
}
