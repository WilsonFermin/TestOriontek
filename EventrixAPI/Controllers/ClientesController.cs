using AutoMapper;
using EventrixAPI.DTOs;
using EventrixAPI.Entidades;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EventrixAPI.Controllers
{
    [ApiController]
    [Route("api/clientes")]
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

        [HttpGet("{id:int}", Name = "ObtenerCliente")]
        public async Task<ActionResult<ClienteDTOConDirecciones>> Get(int id)
        {
            var cliente = await context.Clientes.Include(x => x.Direcciones).FirstOrDefaultAsync(x => x.Id == id);

            if(cliente == null)
            {
                return NotFound("Ese cliente no existe");
            }

            return mapper.Map<ClienteDTOConDirecciones>(cliente);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] ClienteCreaccionDTO clienteCreaccionDTO)
        {
            var cliente = mapper.Map<Cliente>(clienteCreaccionDTO);
            context.Add(cliente);
            await context.SaveChangesAsync();
            var clienteLectura = mapper.Map<ClienteDTO>(cliente);
            return new CreatedAtRouteResult("ObtenerCliente", new { id = cliente.Id}, clienteLectura);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put([FromBody] ClienteEditarDTO clienteEditarDTO, int id)
        {
            var clienteDB = await context.Clientes.FirstOrDefaultAsync(x => x.Id == id);

            if (clienteDB == null)
            {
                return NotFound("Ese cliente no existe");
            }

            var cliente = mapper.Map<Cliente>(clienteEditarDTO);
            cliente.Id = id;
            var cedula = clienteDB.Cedula;
            cliente.Cedula = cedula;

            context.Update(cliente);
            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var existe = await context.Clientes.AnyAsync(x => x.Id == id);

            if (!existe)
            {
                return NotFound();
            }
            
            var cliente = new Cliente() { Id = id };
            context.Remove(cliente);
            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPatch("{id:int}")]
        public async Task<ActionResult> Patch(int id, JsonPatchDocument<ClientePatchDTO> patchDocumnet)
        {
            if(patchDocumnet == null)
            {
                return BadRequest();
            }

            var clienteDB = await context.Clientes.FirstOrDefaultAsync(x => x.Id == id);

            if(clienteDB == null)
            {
                return NotFound("Cliente no existe");
            }

            var clienteDTO = mapper.Map<ClientePatchDTO>(clienteDB);
       
            patchDocumnet.ApplyTo(clienteDTO, ModelState);

            var esValido = TryValidateModel(clienteDTO);

            if (!esValido)
            {
                return BadRequest(ModelState);
            }

            mapper.Map(clienteDTO, clienteDB);
            
            await context.SaveChangesAsync();
            return NoContent();
        }
    }
}
