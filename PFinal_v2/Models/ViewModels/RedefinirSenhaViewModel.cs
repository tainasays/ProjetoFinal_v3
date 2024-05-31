using System.ComponentModel.DataAnnotations;

namespace PFinal_v2.Models.ViewModels
{
    public class RedefinirSenhaViewModel
    {
        public int UsuarioId { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string? NovaSenha { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("NovaSenha", ErrorMessage = "As senhas não coincidem.")]
        public string? ConfirmarSenha { get; set; }

    }
}
