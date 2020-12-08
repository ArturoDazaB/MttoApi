using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using MttoApi.Model;
using MttoApi.Model.Context;

using System.Text.Json.Serialization;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MttoApi.Controllers
{
    [Route("mttoapp/registro")]
    [ApiController]
    public class RegistroUsuariosController : ControllerBase
    {
        private readonly MTTOAPP_V6Context _context;

        public RegistroUsuariosController(MTTOAPP_V6Context context)
        {
            this._context = context;
        }

        //===============================================================================================
        //===============================================================================================
        // POST mttoapp/registro
        [HttpPost]
        public async Task<ActionResult<string>> Post([FromBody] RequestRegistroUsuario newuser)
        {
            //SE EVALUA SI LOS SIGUIENTES CAMPOS YA SE ENCUENTRAN REGISTRADOS DENTRO DE LA TABLA PERSONAS
            if (!MatchCedula(newuser.NewUser.Persona.Cedula) &&             //TRUE: SE ENCONTRO UN REGISTRO CON LA MISMA CEDULA (ID)
                !MatchNumeroFicha(newuser.NewUser.Persona.NumeroFicha) &&   //TRUE: SE ENCONTRO UN REGISTRO CON EL MISMO NUMERO DE FICHA
                !MatchUsername(newuser.NewUser.Usuario.Username))           //TRUE: SE ENCONTRO UN REGISTRO CON EL MISMO NOMBRE DE USUARIO
            {
                using (var transaction = this._context.Database.BeginTransaction())
                {
                    try
                    {
                        //SE CREAN LOS OBJETOS DEL TIPO "Personas" Y "Usuarios" QUE CONTENDRAN LA NUEVA INFORMACION
                        Personas persona = Personas.NewPersonas(newuser.NewUser.Persona);
                        Usuarios usuario = Usuarios.NewUsuarios(newuser.NewUser.Usuario);

                        //SE REGISTRA LA INFORMACION NUEVA
                        this._context.Add(persona);
                        this._context.Entry(persona).State = EntityState.Added;

                        this._context.Add(usuario);
                        this._context.Entry(usuario).State = EntityState.Added;

                        //SE CREA E INICIALIZA UN NUEVO OBJETO "HistorialSolicitudesWeb"
                        Historialsolicitudesweb solicitudweb = 
                            Historialsolicitudesweb.NewHistorialSolocitudesWeb(newuser.UserId, 5);

                        //SE REGISTRA EL NUEVO REGISTRO EN LA TABLA "Historial Solicitudes Web"
                        this._context.Historialsolicitudesweb.Add(solicitudweb);
                        this._context.Entry(solicitudweb).State = EntityState.Added;

                        //SE COMPRUEBAN QUE LAS TABLAS "Personas" Y "Usuarios" TENGAN LA MISMA CANTIDAD DE REGISTROS
                        //NOTA: ESTO DEBIDO A QUE ESTAS DOS TABLAS LLEVAN REGISTROS PARALELOS QUE SE CORRESPONDEN 
                        if(this._context.Personas.Count() != this._context.Usuarios.Count())
                        {
                            return BadRequest("Error al intentar regitrar datos. Intente nuevente");
                        }

                        //SE GUARDAN LOS CAMBIOS
                        await this._context.SaveChangesAsync();

                        //SE CIERRA LA TRANSACCION
                        await transaction.CommitAsync();
                    }
                    catch (Exception ex)
                    {
                        return BadRequest(ex.Message.ToString());
                    }
                }
            }
            else
            {
                //===============================================================================================
                //SI ALGUNA DE LOS CONDICIONALES EN LA EVALUACION ANTERIOR ES FALSA SE PROCEDE A INFORMAR CUAL
                //DE LOS CAMPOS EVALUADOS YA SE ENCUENTRA REGISTRADO

                if (MatchCedula(newuser.NewUser.Persona.Cedula))
                    return BadRequest("Numero de cedula ya registrado: " + newuser.NewUser.Persona.Cedula.ToString());

                if (MatchNumeroFicha(newuser.NewUser.Persona.NumeroFicha))
                    return BadRequest("Numero de ficha ya registrado: " + newuser.NewUser.Persona.NumeroFicha.ToString());

                if (MatchUsername(newuser.NewUser.Usuario.Username))
                    return BadRequest("Nombre de usuario" + newuser.NewUser.Usuario.Username + "ya se encuentra registrado");
            }

            return Ok("Registro Exitoso");
        }

        //===============================================================================================
        //===============================================================================================
        //METODOS LOCALES
        private bool MatchCedula(double cedula)
        {
            var response = this._context.Personas.Any(x => x.Cedula == cedula);
            /*
             * TRUE = YA EXISTE UN REGISTRO CON DICHA CEDULA
             * FALSE = NO EXISTE UN REGISTRO CON DICHA CEDULA
             */
            return response;
        }

        private bool MatchUsername(string username)
        {
            var response = this._context.Usuarios.Any(x => x.Username.ToLower() == username.ToLower());
            /*
             * TRUE = YA EXISTE UN REGISTRO CON DICHO NOMBRE DE USUARIO
             * FALSE = NO EXISTE NINGUN REGISTRO CON DICHO NOMBRE DE USUARIO
             */
            return response;
        }

        private bool MatchNumeroFicha(double ficha)
        {
            var response = this._context.Personas.Any(x => x.NumeroFicha == ficha);
            /*
             * TRUE = YA EXISTE UN REGISTRO CON DICHO NUMERO DE FICHA
             * FALSE = NO EXISTE NINGUN REGISTRO CON DICHO NUMERO DE FICHA
             */
            return response;
        }

        //===============================================================================================
        //===============================================================================================
    }
}