using curso.api.Business.Entities;
using curso.api.Business.Repositories;
using curso.api.Configurations;
using curso.api.Filters;
using curso.api.Infraestrutura.Data;
using curso.api.Models;
using curso.api.Models.Usuarios;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace curso.api.Controllers
{
    [Route("api/v1/usuario")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IAuthenticationService _authenticationService;
        public UsuarioController(
            IUsuarioRepository usuarioRepository,
            IAuthenticationService authentication)
        {
            _usuarioRepository = usuarioRepository;
            _authenticationService = authentication;
        }
        /// <summary>
        /// Este serviço permite autenticar um usuário cadastrado e ativo.
        /// </summary>
        /// <param name="loginViewModelInput">View model do login</param>
        /// <returns>Retorna status ok, dados do usuario e o token em caso de sucesso</returns>
        [SwaggerResponse(statusCode: 200, description: "Sucesso ao autenticar", Type = typeof(LoginViewModelInput))]
        [SwaggerResponse(statusCode: 400, description: "Campos obrigatórios", Type = typeof(ValidaCampoViewModelOutput))]
        [SwaggerResponse(statusCode: 500, description: "Erro interno", Type = typeof(ErroGenericoViewModel))]
        [HttpPost]
        [Route("logar")]
        [ValidacaoModelStateCustomizado]
        public IActionResult Logar(LoginViewModelInput loginViewModelInput)
        {
            var usuario = _usuarioRepository.ObterUsuario(loginViewModelInput.Login);
            
            if (usuario == null)
            {
                return BadRequest("Houve um erro ao tentar acessar.");
            }

            //if (usuario.Senha != loginViewModelInput.Senha.GerarSenhaCriptografada())
            //{
            //    return BadRequest("Houve um erro ao tentar acessar.");
            //}

            

            var usuarioViewModelOutput = new UsuarioViewModelOutput()
            {
                Codigo = usuario.Codigo,
                Login = loginViewModelInput.Login,
                Email = usuario.Email
            };

            var token = _authenticationService.GerarToken(usuarioViewModelOutput);

            return Ok(new
            {
                Token = token,
                Usuario = usuarioViewModelOutput
            });
        }

        /// <summary>
        /// Este serviço permite cadastrar um usuário não existente
        /// </summary>
        /// <param name="loginViewModelInput">View model do registro de login</param>
        /// <returns></returns>
        [SwaggerResponse(statusCode: 200, description: "Sucesso ao autenticar", Type = typeof(LoginViewModelInput))]
        [SwaggerResponse(statusCode: 400, description: "Campos obrigatórios", Type = typeof(ValidaCampoViewModelOutput))]
        [SwaggerResponse(statusCode: 500, description: "Erro interno", Type = typeof(ErroGenericoViewModel))]
        [HttpPost]
        [Route("registrar")]
        [ValidacaoModelStateCustomizado]
        public IActionResult Registrar(RegistroViewModelInput loginViewModelInput)
        {
            //var optionsBuilder = new DbContextOptionsBuilder<CursoDBContext>();
            //optionsBuilder.UseSqlServer("Server=localhost;Database=CURSO;user=sa;password=App@223020");
            //CursoDBContext contexto = new CursoDBContext(optionsBuilder.Options);

            //var migracoesPendentes = contexto.Database.GetPendingMigrations();

            //if (migracoesPendentes.Count() > 0)
            //{
            //    contexto.Database.Migrate();
            //}

            var usuario = new Usuario();
            usuario.Login = loginViewModelInput.Login;
            usuario.Senha = loginViewModelInput.Senha;
            usuario.Email = loginViewModelInput.Email;            
            _usuarioRepository.Adicionar(usuario);
            _usuarioRepository.Commit();

            return Created("", loginViewModelInput);

            //IUsuarioRepository _usuarioRepository = new UsuarioRepository();

        }
    }
}
