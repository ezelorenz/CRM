using CRM_codeFirst.DTO;
using CRM_codeFirst.Entidades;
using AutoMapper;
using System.Linq.Expressions;

namespace CRM_codeFirst.Utils
{
    public class AutoMapperProfile :Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<VendedorCreacionDTO, Vendedor>();
            CreateMap<Vendedor, VendedorDTO>();
            CreateMap<Vendedor, VendedorConClientesDTO>()
                .ForMember(d => d.Clientes, o =>
                    o.MapFrom(MapFromVendedoresClientesAClienteDTO));
            
           
            CreateMap<ClienteCreacionDTO, Cliente>()
                .ForMember(d => d.VendedoresClientes, o =>
                    o.MapFrom(MapIntToVendedorCliente));
            CreateMap<Cliente, ClienteDTO>();
            CreateMap<Cliente, ClienteConVendedoresDTO>()
                .ForMember(d => d.Vendedores, o =>
                    o.MapFrom(MapFromVendedoresClientesToVendedorDTO));
        }


        private List<ClienteDTO> MapFromVendedoresClientesAClienteDTO(Vendedor vendedor, VendedorDTO vendedorDTO)
        {
            List<ClienteDTO> respuesta = new List<ClienteDTO>();
            if (vendedor.VendedoresClientes == null)
            {
                return respuesta;
            }
            foreach (var item in vendedor.VendedoresClientes)
                respuesta.Add(new ClienteDTO { Id = item.ClienteId, Nombre = item.Cliente.Nombre, UrlPerfil = item.Cliente.UrlPerfil });
            return respuesta;
        }




        private List<VendedorCliente> MapIntToVendedorCliente(ClienteCreacionDTO clienteCreacionDTO, Cliente cliente)
        {
            List<VendedorCliente> respuesta = new List<VendedorCliente>();

            if (clienteCreacionDTO.VendedoresIds == null)
                return respuesta;

            foreach (int id in clienteCreacionDTO.VendedoresIds)
            {
                respuesta.Add(new VendedorCliente { VendedorId = id });
            }
            return respuesta;
        }
        



        private List<VendedorDTO> MapFromVendedoresClientesToVendedorDTO(Cliente cliente, ClienteDTO clienteDTO)
        {
            List<VendedorDTO> respuesta = new List<VendedorDTO>();
            if (cliente.VendedoresClientes == null)
                return respuesta;

            foreach (var item in cliente.VendedoresClientes)
                respuesta.Add(new VendedorDTO { Id = item.VendedorId, Nombre = item.Vendedor.Nombre });
            return respuesta;
        }
    }
}
