using CRM_codeFirst.Contexto;
using Microsoft.AspNetCore.Mvc;
using CRM_codeFirst.Entidades;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using CRM_codeFirst.DTO;

namespace CRM_codeFirst.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VendedoresController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        public VendedoresController(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<VendedorDTO>>> Get()
        {
            var vendedores = await context.Vendedores.ToListAsync();
            return mapper.Map<List<VendedorDTO>>(vendedores);
        }

        [HttpGet("{id:int}", Name = "ObtenerVendedor")]
        public async Task<ActionResult<VendedorConClientesDTO>> Get(int id)
        {
            var vendedor = await context.Vendedores
                .Include(x => x.VendedoresClientes)
                    .ThenInclude(x => x.Cliente)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (vendedor == null)
            {
                return NotFound("No se encontro el Vendedor solicitado");
            }
            return mapper.Map<VendedorConClientesDTO>(vendedor);
        }


        [HttpPost]
        public async Task<ActionResult> Post([FromBody] VendedorCreacionDTO vendedorCreacionDTO)
        {
            var existe = await context.Vendedores.AnyAsync(x => x.Nombre == vendedorCreacionDTO.Nombre);
            if (existe)
            {
                return BadRequest($"Ya existe el vendedor con el nombre {vendedorCreacionDTO.Nombre}");
            }

            var vendedor = mapper.Map<Vendedor>(vendedorCreacionDTO);
            
            context.Add(vendedor);
            await context.SaveChangesAsync();

            var vendedorDTO = mapper.Map<VendedorDTO>(vendedor);

            return CreatedAtRoute("ObtenerVendedor", new { id = vendedor.Id }, vendedorDTO);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, VendedorCreacionDTO vendedorCreacionDTO)
        {
            var existe = await context.Vendedores.AnyAsync(x => x.Id == id);
            if (!existe)
                return NotFound("No existe el vendedor");

            var vendedor = mapper.Map<Vendedor>(vendedorCreacionDTO);
            vendedor.Id = id;

            context.Update(vendedor);

            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var existeVendedor = await context.Vendedores.AnyAsync(x => x.Id == id);
            if (!existeVendedor)
                return NotFound("El vendedor no existe");

            context.Vendedores.Remove(new Vendedor { Id = id });
            await context.SaveChangesAsync();
            return NoContent();
        }
    }
}
