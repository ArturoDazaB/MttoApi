using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MttoApi.Model;
using MttoApi.Model.Context;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MttoApi.Controllers
{
    [Route("mttoapp/login")]
    [ApiController]
    public class LogInController : ControllerBase
    {
        private readonly MTTOAPP_V6Context _context;

        public LogInController(MTTOAPP_V6Context context)
        {
            this._context = context;
        }

        //========================================================================================================
        //========================================================================================================
        // GET: mttoapp/login?username=<username>&password=<password>
        // GET: mttoapp/login?USERNAME=<username>&PASSWORD=<password>
        [HttpGet]
        public async Task<IActionResult> Get(string username, string password)
        {
            //SE CREA E INICIALIZA LA VARIABLE QUE SE RETORNARA SI TODAS LAS CONDICIONES SE CUMPLEN
            LogInResponse response = null;

            //SE EVALUA QUE EXISTA UN NOMBRE DE USUARIO QUE OBEDESCA AL NOMBRE DE USUARIO
            if(this._context.Usuarios.Any(x => x.Username.ToLower() == username.ToLower()))
            {
                //SI EXISTE, SE OBTIENE TODA LA INFORMACION DE DICHO REGISTRO Y SE ALMACENA EN UNA VARIABLE DEL TIPO USUARIO
                Usuarios usuario = this._context.Usuarios.First(x => x.Username.ToLower() == username.ToLower());

                //SE COMPARA QUE LA CONTRASEÑA SEA LA CORRECTA
                if(usuario.Password == password)
                {
                    //SE INICIA LA TRANSACCION CON LA BASE DE DATOS
                    using (var transaction = this._context.Database.BeginTransaction())
                    {
                        //SE BUSCA LA INFORMACION PERSONAL DEL USUARIO QUE DESEA INGRESAR
                        var persona = await this._context.Personas.FindAsync(usuario.Cedula);
                        //--------------------------------------------------------------------------------------------
                        //SE EVALUA SI SE OBTUVO UN REGISTRO DE LA BUSQUEDA ANTERIOR
                        if (persona != null)
                            //DE EXISTIR SE DESECHA LA ENTIDAD RETENIDA
                            this._context.Entry(persona).State = EntityState.Detached;
                        //--------------------------------------------------------------------------------------------
                        //SE CREA E INICIALIZA UN OBJETO DEL TIPO "InformacionGeneral" 
                        var fullinfo = InformacionGeneral.NewInformacionGeneral(persona, usuario);
                        //--------------------------------------------------------------------------------------------
                        //SE CREA E INICALIZA UN OBJETO DEL TIPO "UltimaConexion"
                        var ultimaconexion = new Ultimaconexion().NewUltimaConexion(persona, usuario);
                        //SE REGISTRA/AÑADE EN LA BASE DE DATOS
                        this._context.Ultimaconexion.Add(ultimaconexion);
                        //SE CAMBIA EL ESTADO DE LA ENTIDAD RETENIDA
                        this._context.Entry(ultimaconexion).State = EntityState.Added;
                        //--------------------------------------------------------------------------------------------
                        //SE CREA E INICIALIZA UNA LISTA DE OBJETOS "UltimaConexion"
                        List<Ultimaconexion> lista = new List<Ultimaconexion>();
                        //SE LLENA LA LISTA PREVIAMENTE CREADA CON TODOS LOS REGISTROS DE CONEXION DEL USUARIO QUE DESEA INGRESAR
                        foreach (Ultimaconexion y in this._context.Ultimaconexion.ToList())
                        {
                            if (y.UserId== persona.Cedula)
                                lista.Add(y);
                        }
                        //--------------------------------------------------------------------------------------------
                        //SE CREA UN NUEVO REGISTRO DE SOLICITUDES WEB Y SE AÑADE A LA TABLA "HistorialSolicitudesWeb"
                        Historialsolicitudesweb solicitudweb = Historialsolicitudesweb.NewHistorialSolocitudesWeb(fullinfo.Usuario.Cedula, 0);
                        //SE REGISTRA/AÑADE UN NUEVO REGISTRO DE SOLICITUDES WEB
                        this._context.Historialsolicitudesweb.Add(solicitudweb);
                        this._context.Entry(solicitudweb).State = EntityState.Added;
                        await this._context.SaveChangesAsync();
                        //--------------------------------------------------------------------------------------------
                        if (lista.Count > 0)
                            response = LogInResponse.NewLogInResponse(fullinfo, lista[lista.Count - 1].UltimaConexion1);
                        if (lista.Count == 0)
                            response = LogInResponse.NewLogInResponse(fullinfo, ultimaconexion.UltimaConexion1);
                        //--------------------------------------------------------------------------------------------
                        await transaction.CommitAsync();
                    }
                }
                else
                {
                    //SI EL NOMBRE DE USUARIO CONICIDE PERO LA CONTRASEÑA NO SE RETORNA UN BADREQUEST
                    return BadRequest("Contraseña incorrecta");
                }
            }
            else
            {
                //SI EL NOMBRE DE USUARIO NO CONIDIDE SE RETORNA UN NOT FOUND
                return NotFound("Nombre de usuario no encontrado");
            }

            return Ok(response);
        }
    }
}