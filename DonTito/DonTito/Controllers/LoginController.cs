﻿using Core.Response;
using DonTito.Custom;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models.Models;

namespace DonTito.Controllers
{
    [Route("api/[controller]")]
    [AllowAnonymous]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly DonTitoContext _context;
        private readonly Utilidades _utilidades;

        public LoginController(DonTitoContext context, Utilidades utilidades)
        {
            _context = context;
            _utilidades = utilidades;
        }
        [HttpPost]
        [Route("Registrarse")]
        public async Task<IActionResult> Registrarse(UsuarioDtoOut usuario)
        {
            var newregistro = new Usuario
            {
                Email = usuario.Email,
                Password = _utilidades.EncriptarSHA256(usuario.Password!)
            };
            await _context.Usuario.AddAsync(newregistro);
            await _context.SaveChangesAsync();

            if(newregistro.Id != 0)
            {
                return StatusCode(StatusCodes.Status200OK, new {isSuccess = true});
            }
            else
            {
                return StatusCode(StatusCodes.Status200OK, new {isSuccess = false});
            }
        }
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(UsuarioDtoOut login)
        {
            var usuarioEncontrado = await _context.Usuario.Where(u => 
                                                                    u.Email == login.Email && 
                                                                    u.Password == _utilidades.EncriptarSHA256(login.Password!)).FirstOrDefaultAsync();
            if (usuarioEncontrado == null)
            {
                return StatusCode(StatusCodes.Status200OK, new { isSuccess = false, token="" });
            }
            else
            {
                return StatusCode(StatusCodes.Status200OK, new { isSuccess = true, token = _utilidades.GenerarJWT(usuarioEncontrado) });
            }
        }
    }
}
