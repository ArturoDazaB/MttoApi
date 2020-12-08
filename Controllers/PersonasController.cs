﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MttoApi.Model;
using MttoApi.Model.Context;

namespace MttoApi.Controllers
{
    [Route("mttoapp/personas")]
    [ApiController]
    public class PersonasController : ControllerBase
    {
        private readonly MTTOAPP_V6Context _context;

        public PersonasController(MTTOAPP_V6Context context)
        {
            _context = context;
        }

        //===============================================================================================
        //===============================================================================================
        // GET: api/Personas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Personas>>> GetPersonas()
        {
            return await _context.Personas.ToListAsync();
        }

        // GET: mttoapp/personas/cedula/12345678
        [HttpGet]
        [Route("cedula/{cedula}")]
        [Route("id/{cedula}")]
        public async Task<ActionResult<Personas>> GetPersonasCedula(double cedula)
        {
            var personas = await _context.Personas.FindAsync(cedula);

            if (personas == null)
            {
                return NotFound("Cedula no encontrada:  " + cedula);
            }

            return personas;
        }

        // GET: mttoapp/personas/5
        [HttpGet]
        [Route("ficha/{ficha}")]
        [Route("numeroficha/{ficha}")]
        public async Task<ActionResult<Personas>> GetPersonasFicha(double ficha)
        {
            Personas personas = null;
            List<Personas> Lista = await this._context.Personas.ToListAsync();

            foreach (Personas x in Lista)
            {
                if (ficha == x.NumeroFicha)
                {
                    personas = x;
                }
            }

            if (personas == null)
            {
                return NotFound("Numero de ficha no registrado:  " + ficha);
            }

            return personas;
        }

        //===============================================================================================
        //===============================================================================================
    }
}