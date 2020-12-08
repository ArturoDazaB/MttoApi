using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MttoApi.Model;
using MttoApi.Model.Context;

namespace MttoApi.Controllers
{
    [Route("mttoapp/usuarios")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly MTTOAPP_V6Context _context;

        public UsuariosController(MTTOAPP_V6Context context)
        {
            _context = context;
        }

        //===============================================================================================
        //===============================================================================================
        // GET: mttoapp/Usuarios
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Usuarios>>> GetUsuarios()
        {
            return await _context.Usuarios.ToListAsync();
        }

        // GET: mttoapp/usuarios/cedula/12345678
        // GET: mttoapp/usuarios/id/12345678
        [HttpGet]
        [Route("cedula/{cedula}")]
        [Route("id/{cedula}")]
        public async Task<ActionResult<Usuarios>> GetUsuariosCedula(double cedula)
        {
            var usuarios = await _context.Usuarios.FindAsync(cedula);

            if (usuarios == null)
            {
                return NotFound("Cedula no encontrada:  " + cedula);
            }

            return Ok(usuarios);
        }

        // GET: api/Usuarios/5
        [HttpGet]
        [Route("ficha/{ficha}")]
        [Route("numeroficha/{ficha}")]
        public async Task<ActionResult<Usuarios>> GetUsuariosFicha(double ficha)
        {
            Usuarios usuario = null;
            List<Personas> Lista = await this._context.Personas.ToListAsync();

            foreach (Personas x in Lista)
            {
                if (ficha == x.NumeroFicha)
                {
                    usuario = await this._context.Usuarios.FindAsync(x.Cedula);
                    break;
                }
            }

            if (usuario == null)
            {
                return NotFound("Cedula no encontrada:  " + ficha);
            }

            return Ok(usuario);
        }

        //===============================================================================================
        //===============================================================================================
        /*
        // PUT: api/Usuarios/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUsuarios(double id, Usuarios usuarios)
        {
            if (id != usuarios.Cedula)
            {
                return BadRequest();
            }

            _context.Entry(usuarios).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsuariosExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Usuarios
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Usuarios>> PostUsuarios(Usuarios usuarios)
        {
            _context.Usuarios.Add(usuarios);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (UsuariosExists(usuarios.Cedula))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetUsuarios", new { id = usuarios.Cedula }, usuarios);
        }

        // DELETE: api/Usuarios/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Usuarios>> DeleteUsuarios(double id)
        {
            var usuarios = await _context.Usuarios.FindAsync(id);
            if (usuarios == null)
            {
                return NotFound();
            }

            _context.Usuarios.Remove(usuarios);
            await _context.SaveChangesAsync();

            return usuarios;
        }

        */

        private bool UsuariosExists(double id)
        {
            return _context.Usuarios.Any(e => e.Cedula == id);
        }
    }
}