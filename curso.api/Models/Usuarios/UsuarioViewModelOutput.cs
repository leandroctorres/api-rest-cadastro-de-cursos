using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace curso.api.Models.Usuarios
{
    public class UsuarioViewModelOutput
    {
        [Required(ErrorMessage = "O Codigo é obrigatório")]
        public int Codigo { get; set; }
        [Required(ErrorMessage = "O Login é obrigatório")]
        public string Login { get; set; }
        [Required(ErrorMessage = "O E-mail é obrigatório")]
        public string Email { get; set; }
    }
}
