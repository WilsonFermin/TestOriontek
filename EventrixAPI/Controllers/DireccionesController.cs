using AutoMapper;
using EventrixAPI.DTOs;
using EventrixAPI.Entidades;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EventrixAPI.Controllers
{
    [ApiController]
    [Route("api/clientes/{clienteId:int}/direcciones")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class DireccionesController: ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public DireccionesController(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<List<DireccionDTO>>> Get(int clienteId)
        {
            if (!ClienteExiste(clienteId).Result)
            {
                return NotFound("Ese cliente no existe para poder buscarle su direccion");
            }

            var direcciones = await context.Direcciones.Where(x => x.ClienteId == clienteId).ToListAsync();
            if(direcciones.Count == 0)
            {
                return NotFound("Este cliente no tiene direccion registrada");
            }

            return mapper.Map<List<DireccionDTO>>(direcciones);
        }

        [HttpGet("{id:int}", Name = "ObtenerDireccion")]
        [AllowAnonymous]
        public async Task<ActionResult<DireccionDTO>> Get(int id, int clienteId)
        {
            if (!ClienteExiste(clienteId).Result)
            {
                return NotFound("Ese cliente no existe para poder buscarle su direccion");
            }

            var direccion = await context.Direcciones.Where(x => x.ClienteId == clienteId).FirstOrDefaultAsync(x => x.Id == id);

            if(direccion == null)
            {
                return NotFound("Direccion no existe para este cliente");
            }

            return mapper.Map<DireccionDTO>(direccion);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] DireccionCreaccionDTO direccionCreaccionDTO, int clienteId)
        {
            if (!ClienteExiste(clienteId).Result)
            {
                return NotFound("Ese cliente no existe para poder crearle una direccion");
            }

            var direccion = mapper.Map<Direccion>(direccionCreaccionDTO);
            direccion.ClienteId = clienteId; 
            context.Add(direccion);
            await context.SaveChangesAsync();

            var direccionLectura = mapper.Map<DireccionDTO>(direccion);

            return new CreatedAtRouteResult("ObtenerDireccion", new {id = direccion.Id, clienteId = clienteId}, direccionLectura);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put([FromBody] DireccionCreaccionDTO direccionCreaccionDTO, int id, int clienteId)
        {
            if (!ClienteExiste(clienteId).Result)
            {
                return NotFound("Ese cliente no existe para poder actualizar su direccion");
            }

            var direccionDB = await context.Direcciones.Where(x => x.ClienteId == clienteId).FirstOrDefaultAsync(x => x.Id == id);

            var direccionCliente = await context.Direcciones.FirstOrDefaultAsync(x => x.Id == id);
            var clienteDireccion = await context.Clientes.FirstOrDefaultAsync(x => x.Id == clienteId);

            if (direccionDB == null)
            {
                if(direccionCliente != null)
                {
                    return NotFound("Esa direccion no pertenece al cliente " + clienteDireccion.Nombre);
                }
                return NotFound("Esa direccion no existe");
            }

            var direccion = mapper.Map<Direccion>(direccionCreaccionDTO);
            direccion.Id = id;
            direccion.ClienteId = clienteId;
            context.Update(direccion);
            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "esAdmin")]
        public async Task<ActionResult> Delete(int id, int clienteId)
        {
            if (!ClienteExiste(clienteId).Result)
            {
                return NotFound("Ese cliente no existe para poder borrarle una direccion");
            }

            var existe = await context.Direcciones.Where(x => x.ClienteId == clienteId).AnyAsync(x => x.Id == id);

            var direccionCliente = await context.Direcciones.FirstOrDefaultAsync(x => x.Id == id);
            var clienteDireccion = await context.Clientes.FirstOrDefaultAsync(x => x.Id == clienteId);

            if (!existe)
            {
                if (direccionCliente != null)
                {
                    return NotFound("Esa direccion no pertenece al cliente " + clienteDireccion.Nombre);
                }

                return NotFound("Direccion no existe.");
            }

            context.Remove(new Direccion() { Id = id, ClienteId = clienteId });
            await  context.SaveChangesAsync();
            return NoContent();
        }

        protected async Task<bool> ClienteExiste(int clienteId)
        {
            var cliente = await context.Clientes.FirstOrDefaultAsync(x => x.Id == clienteId);

            if(cliente == null)
            {
                return false;
            }

            return true;
        }
    }
}
