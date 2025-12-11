using Microsoft.AspNetCore.Mvc;
using proyectoSistemaPropietarios.Services;
using proyectoSistemaPropietarios.DTOs;

namespace proyectoSistemaPropietarios.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;
        private readonly JwtService _jwtService;

        public AuthController(IUsuarioService usuarioService, JwtService jwtService)
        {
            _usuarioService = usuarioService;
            _jwtService = jwtService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO login)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _usuarioService.ObtenerPorCorreoAsync(login.Correo);

            if (user == null || user.Contrasena != login.Contrasena)
                return Unauthorized("Correo o contraseña incorrectos");

            var token = _jwtService.GenerarToken(user.Id.ToString(), user.Correo);

            return Ok(new { token });
        }
    }
}
