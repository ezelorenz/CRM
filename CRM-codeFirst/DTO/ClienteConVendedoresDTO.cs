namespace CRM_codeFirst.DTO
{
    public class ClienteConVendedoresDTO : ClienteDTO
    {
        public List<VendedorDTO> Vendedores { get; set; }
    }
}
