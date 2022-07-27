namespace CRM_codeFirst.Entidades
{
    public class VendedorCliente
    {
        public int VendedorId { get; set; }
        public int ClienteId { get; set; }
        
        public DateTime FechaeAsignacion { get; set; }

        public Vendedor Vendedor { get; set; }
        public Cliente Cliente { get; set; }


    }
}
