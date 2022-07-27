using CRM_codeFirst.Contexto;
using Microsoft.AspNetCore.Mvc;
using CRM_codeFirst.Entidades;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using CRM_codeFirst.DTO;

namespace CRM_codeFirst.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientesController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        public ClientesController(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<ClienteDTO>>> Get()
        {
            var clientes = await context.Clientes.ToListAsync();
            return mapper.Map<List<ClienteDTO>>(clientes);
        }



        [HttpGet("{id:int}", Name ="ObtenerCliente")]
        public async Task<ActionResult<ClienteConVendedoresDTO>> Get(int id)
        {
            var cliente = await context.Clientes
                .Include(x => x.VendedoresClientes)
                    .ThenInclude(x => x.Vendedor)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (cliente == null)
            {
               return NotFound("No se encontro el Cliente solicitado");
            }
            
            return mapper.Map<ClienteConVendedoresDTO>(cliente);
        }



        [HttpPost]
        public async Task<ActionResult> Post([FromBody] ClienteCreacionDTO clienteCreacionDTO)
        {
            if (clienteCreacionDTO.VendedoresIds == null)
            {
                return BadRequest("No se puede insertar un cliente sin al menos asignale un vendedor");
            }

            var vendedoresIds = await context.Vendedores.Where(x => clienteCreacionDTO.VendedoresIds.Contains(x.Id)).
                                Select(x =>x.Id).ToListAsync();

            if (vendedoresIds.Count != clienteCreacionDTO.VendedoresIds.Count)
            {
                return BadRequest("Se ingreso al menos un agente que no existe");
            }

            var cliente = mapper.Map<Cliente>(clienteCreacionDTO);

            context.Clientes.Add(cliente);
            
            await context.SaveChangesAsync();
            
            var clienteDTO = mapper.Map<ClienteDTO>(cliente);
            return CreatedAtRoute("ObtenerCliente", new { id = cliente.Id }, clienteDTO);
        }



        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, ClienteCreacionDTO clienteCreacionDTO)
        {
            var cliente = await context.Clientes.Include(x => x.VendedoresClientes)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (cliente == null)
                return NotFound("El cliente no existe");

            cliente = mapper.Map(clienteCreacionDTO, cliente);

            context.Clientes.Update(cliente);
            await context.SaveChangesAsync();
            return NoContent();
        }



        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var existeCliente = await context.Clientes.AnyAsync(x => x.Id == id);
            if (!existeCliente)
                return NotFound("El cliente no existe");

            context.Clientes.Remove(new Cliente { Id = id });
            await context.SaveChangesAsync();
            return NoContent();
        }

    }
}
