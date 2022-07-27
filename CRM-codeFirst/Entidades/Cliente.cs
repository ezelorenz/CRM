using System.ComponentModel.DataAnnotations;

namespace CRM_codeFirst.Entidades
{
    public class Cliente
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(70, ErrorMessage = "El campo {0} no debe superar de {1} caracter.")]
        public string Nombre { get; set; }
        public string UrlPerfil { get; set; }

        
        public List<VendedorCliente> VendedoresClientes { get; set; }
    }
}
