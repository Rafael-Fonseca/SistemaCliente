using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaCliente.Models;
using Microsoft.AspNetCore.JsonPatch;
using SistemaCliente;

namespace SistemaCliente.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClienteController : ControllerBase
    {
        private readonly ClienteContext _context;
        public ClienteController(ClienteContext context)
        {
            _context = context;
        }

        // GET: api/Cliente
        [HttpGet("{page:int}/{take:int}")]
        public async Task<ActionResult<IEnumerable<ClienteDTO>>> GetCliente(
            [FromRoute] int page = 1,
            [FromRoute] int take = 2)
        {
            if (take > 1000)
                return BadRequest("Você não pode coletar mais que 1000 itens por requisição");

            var total_items = await _context.Clientes.CountAsync();
            int total_pages = total_items/take;
            if(total_items % take != 0)
                total_pages += 1;
            
            if(page > total_pages)
                return BadRequest($"Você está tentando acessar uma página que não existe.\ncurrent page = {page}\nmax page = {total_pages}");

            var clientes = await _context
                .Clientes
                .Select(item => ClienteToDTO(item))
                .AsNoTracking()
                .Skip((page - 1) * take)
                .Take(take)
                .ToListAsync();
            
            //Estudar ViewModel parece que este retorno pode ser feito por ela
            //Talvez seja melhor que retornar objeto dinâmico
            return Ok(new {
                total_items,
                total_pages,
                current_page = page,
                items_page = take,  //Melhor take ou items_page
                data = clientes
            });

        }
        // Assim retorna todos os dados
        //public async Task<ActionResult<IEnumerable<Cliente>>> GetCliente()
        //{
        //    return await _context.Clientes.ToListAsync();
        //}

        // GET: api/Cliente/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ClienteDTO>> GetCliente(long id)
        {
            var cliente = await _context.Clientes.FindAsync(id);

            if (cliente == null)
            {
                return NotFound();
            }

            return ClienteToDTO(cliente);
        }

        // PUT: api/Cliente/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCliente(long id, ClienteDTO clienteDTO)
        {
            if (id != clienteDTO.Id)
            {
                return BadRequest("O id do cliente atual não coincide com o id do cliente a ser atualizado");
            }

            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null)
                return NotFound();

            cliente.Name = clienteDTO.Name;
            cliente.BirthDate = clienteDTO.BirthDate;
            cliente.Gender = clienteDTO.Gender;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClienteExists(id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        // Patch: api/cliente/5
        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateCliente(long id, JsonPatchDocument<Cliente> clienteUpdates)
        {
            //Se alterar o JsonPatchDocument para <ClienteDTO> o ModelState para de validar os meus atributos... porque?
            var cliente = await _context.Clientes.FindAsync(id);

            if (cliente == null)
                return NotFound();
            
            clienteUpdates.ApplyTo(cliente, ModelState);

            if (!TryValidateModel(cliente)) //ModelState.IsValid
                return BadRequest(ModelState);

            _context.Entry(cliente).State = EntityState.Modified;
            
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClienteExists(id))
                    return NotFound();
                else{
                    throw;
                }
            }

            return NoContent();
        }


        // POST: api/Cliente
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ClienteDTO>> PostCliente(ClienteDTO clienteDTO)
        {
            var cliente = new Cliente{
                Name = clienteDTO.Name,
                BirthDate = clienteDTO.BirthDate,
                Gender = clienteDTO.Gender,
                Active = true
            };

            _context.Clientes.Add(cliente);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetCliente),
                new { id = cliente.Id },
                ClienteToDTO(cliente));
        }

        [HttpPost("reactivate/{id}")]
        public async Task<IActionResult> Reactivate(long id)
        {
            //var cliente = await _context.Clientes.FindAsync(id);
            var cliente = await _context.Clientes.IgnoreQueryFilters()
            .FirstOrDefaultAsync();
            



            if (cliente == null)
                return NotFound();
            
            cliente.Active = true;

            _context.Entry(cliente).State = EntityState.Modified;
            
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClienteExists(id))
                    return NotFound();
                else{
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/Cliente/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCliente(long id)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null)
            {
                return NotFound();
            }
            
            cliente.Active = false;

            _context.Clientes.Update(cliente);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ClienteExists(long id)
        {
            return _context.Clientes.Any(e => e.Id == id);
        }

        private static ClienteDTO ClienteToDTO(Cliente cliente) => new ClienteDTO
        {
        Id = cliente.Id,
        Name = cliente.Name,
        BirthDate = cliente.BirthDate,
        Gender = cliente.Gender
        };
    }
}
