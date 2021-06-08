using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MttoApi.Model;
using MttoApi.Model.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MttoApi.Controllers
{
    //===================================================================================================
    //===================================================================================================
    //SE AÑADE A LA CLASE EL ROUTING "ApiController" LA CUAL IDENTIFICARA A LA CLASE "LogInController"
    //COMO UN CONTROLADOR DEL WEB API.
    [ApiController]

    //SE AÑADE A LA CLASE EL ROUTING "Route" JUNTO CON LA DIRECCION A LA CUAL SE DEBE LLAMAR PARA PODER
    //ACCESO A LA CLASE CONTROLLADOR. EJ:
    //https:/<ipadress>:<port>/mttoapp/login <=> https://192.168.1.192:8000/mttoapp/login
    [Route("mttoapp/login")]
    public class LogInController : ControllerBase
    {
        //SE CREA UNA VARIABLE LOCAL DEL TIPO "Context" LA CUAL FUNCIONA COMO LA CLASE
        //QUE MAPEARA LA INFORMACION PARA LECTURA Y ESCRITURA EN LA BASE DE DATOS
        private readonly MTTOAPP_V7Context _context;

        //===============================================================================================
        //===============================================================================================
        //CONSTRUCTOR
        public LogInController(MTTOAPP_V7Context context)
        {
            //SE INICIALIZA LA VARIABLE LOCAL
            this._context = context;
        }

        //========================================================================================================
        //========================================================================================================
        // GET: mttoapp/login?username=<username>&password=<password>
        // GET: mttoapp/login?USERNAME=<username>&PASSWORD=<password>
        //SE ADICIONA EL ROUTING "HttpGet" LO CUAL INDICARA QUE LA FUNCION "Get" RESPONDERA A
        //A SOLICITUDES HTTP DE TIPO GET
        [HttpGet]

        //--------------------------------------------------------------------------------------------------
        //FUNCION QUE ACTUALIZARA LA INFORMACION DE UN USUARIO CUANDO SE REALICE EL LLAMADO DESDE
        //LA PAGINA "PaginaConsultaTableros" DE LA APLICACION "Mtto App". EN ESTA FUNCION SE RECIBEN
        //LOS PARAMETROS:
        // -username: PARAMETRO ENVIADO EN EL URL DE LA SOLICITUD (username=<username> || USERNAME=<username>)
        // -password: PARAMETRO ENVIADO EN EL URL DE LA SOLICITUD (password=<password> || PASSWORD=<password>)
        //--------------------------------------------------------------------------------------------------
        public async Task<IActionResult> LogInRequest(string username, string password)
        {
            //SE CREA E INICIALIZA LA VARIABLE QUE SE RETORNARA SI TODAS LAS CONDICIONES SE CUMPLEN
            LogInResponse response = null;

            //SE EVALUA QUE EXISTA UN NOMBRE DE USUARIO QUE OBEDESCA AL NOMBRE DE USUARIO
            if (this._context.Usuarios.Any
                (x => x.Username.ToLower() == username.ToLower()))  //=> true => EXISTE UN REGISTRO EN LA TABLA USUARIOS QUE RESPONDE AL
                                                                    //           NOMBRE DE USUARIO ENVIADO COMO PARAMETRO.
            {
                //SI EXISTE, SE OBTIENE TODA LA INFORMACION DE DICHO REGISTRO Y SE ALMACENA EN UNA VARIABLE DEL TIPO USUARIO
                Usuarios usuario = this._context.Usuarios.First         //=> METODO QUE RETORNA EL PRIMER REGISTRO QUE COINCIDA
                    (x => x.Username.ToLower() == username.ToLower());  //CON LA COMPARACION DE NOMBRE DE USUARIOS

                //SE COMPARA QUE LA PROPIEDAD DEL OBJETO usuario (OBJETO QUE CONTIENE TODA LA INFORMACION DE USUARIO
                //QUE DESEA INGRESAR) CON EL PARAMETRO "password"
                if (usuario.Password == password) //=> true => LA CONTRASEÑA ENVIADA ES CORRECTA
                {
                    //SE INICIA LA TRANSACCION CON LA BASE DE DATOS
                    using (var transaction = this._context.Database.BeginTransaction())
                    {
                        //SE INICIA LA TRANSACCIONES CON LA BASE DE DATOS
                        try
                        {
                            //--------------------------------------------------------------------------------------------
                            //SE BUSCA LA INFORMACION PERSONAL DEL USUARIO QUE DESEA INGRESAR
                            var persona = await this._context.Personas.FindAsync(usuario.Cedula);

                            //--------------------------------------------------------------------------------------------
                            //SE EVALUA SI SE OBTUVO UN REGISTRO DE LA BUSQUEDA ANTERIOR
                            if (persona != null)
                            {
                                //DE EXISTIR SE DESECHA LA ENTIDAD RETENIDA
                                this._context.Entry(persona).State = EntityState.Detached;
                            }

                            //--------------------------------------------------------------------------------------------
                            //SE CREA E INICIALIZA UN OBJETO DEL TIPO "InformacionGeneral" (OBJETO QUE RETORNARA TODA LA
                            //INFORMACION DEL USUARIO QUE DESEA INGRESAR)
                            var fullinfo = InformacionGeneral.NewInformacionGeneral(persona, usuario);

                            //--------------------------------------------------------------------------------------------
                            //SE CREA E INICALIZA UN OBJETO DEL TIPO "UltimaConexion"
                            var ultimaconexion = new Ultimaconexion().NewUltimaConexion(persona, usuario);

                            //--------------------------------------------------------------------------------------------
                            //SE AÑADE A LA TABLAS "UltimaConexion" EL NUEVO REGISTRO
                            this._context.Ultimaconexion.Add(ultimaconexion);               //SE REGISTRA/AÑADE EN LA BASE DE DATOS
                            this._context.Entry(ultimaconexion).State = EntityState.Added;  //SE CAMBIA EL ESTADO DE LA ENTIDAD RETENIDA

                            //--------------------------------------------------------------------------------------------
                            //SE CREA E INICIALIZA UNA LISTA DE OBJETOS "UltimaConexion"
                            List<Ultimaconexion> lista = new List<Ultimaconexion>();
                            //SE LLENA LA LISTA PREVIAMENTE CREADA CON TODOS LOS REGISTROS DE CONEXION DEL USUARIO QUE DESEA INGRESAR
                            foreach (Ultimaconexion y in this._context.Ultimaconexion.ToList())
                            {
                                //SE EVAUA CADA UNO DE LOS REGISTROS DE LA LISTA Y SE COMPARA SI EL PARAMETRO UserId
                                //DEL REGISTRO ES IGUAL AL ID (CEDULA) DEL USUARIO QUE ESTA INGRESANDO
                                if (y.UserId == persona.Cedula)
                                    //SE AÑADE A LA LISTA EL REGISTRO.
                                    lista.Add(y);
                            }

                            //--------------------------------------------------------------------------------------------
                            //SE CREA UN NUEVO REGISTRO DE SOLICITUDES WEB Y SE AÑADE A LA TABLA "HistorialSolicitudesWeb"
                            Historialsolicitudesweb solicitudweb =
                                Historialsolicitudesweb.NewHistorialSolocitudesWeb(fullinfo.Usuario.Cedula, 0);

                            //--------------------------------------------------------------------------------------------
                            //SE REGISTRA/AÑADE UN NUEVO REGISTRO DE SOLICITUDES WEB
                            this._context.Historialsolicitudesweb.Add(solicitudweb);
                            this._context.Entry(solicitudweb).State = EntityState.Added;

                            //--------------------------------------------------------------------------------------------
                            //SE EVALUA CUANTOS REGISTROS SE ACUMULARON EN LA LISTA "lista"
                            //MAS DE UN REGISTRO
                            if (lista.Count > 0)
                            {
                                //SE ENVIA LA INFORMACION DEL USUARIO Y EL PENULTIMO REGISTRO (ULTIMA CONEXION PREVIA A LA ACTUAL)
                                response = LogInResponse.NewLogInResponse(fullinfo, lista[lista.Count - 1].UltimaConexion1);
                            }
                            if (lista.Count == 0)
                            {
                                //SE ENVIA LA INFORMACION DEL USUARIO Y EL ULTIMO REGISTRO (CONEXION ACTUAL)
                                response = LogInResponse.NewLogInResponse(fullinfo, ultimaconexion.UltimaConexion1);
                            }
                            //--------------------------------------------------------------------------------------------
                            //SE GUARDAN LOS CAMBIOS REALIZADOS SOBRE LA BASE DE DATOS
                            await this._context.SaveChangesAsync();
                            //SE CULMINA LA TRANSACCION CON LA BASE DE DATOS
                            await transaction.CommitAsync();
                        }
                        //SI OCURRE ALGUNA EXCEPCION EN EL PROCESO DE LECTURA Y ESCRITURA DE LA BASE DE DATOS EL CODIGO
                        //SE REDIRIGE A LA SECCION CATCH DEL CICLO TRY...CATCH
                        catch (Exception ex) when (ex is DbUpdateException ||
                                                   ex is DbUpdateConcurrencyException)
                        {
                            Console.WriteLine("\n=================================================");
                            Console.WriteLine("=================================================");
                            Console.WriteLine("\nHa ocurrico un error:\n" + ex.Message.ToString());
                            Console.WriteLine("=================================================");
                            Console.WriteLine("=================================================\n");
                            //SE RETONA LA RESPUESTA "BadRequest" JUNTO CON UN MENSAJE INFORMANDO SOBRE EL ERROR
                            return BadRequest("\nHa ocurrico un error, intentelo nuevamente");
                        }
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

            //SE RETORNA EL CODIGO 200 OK JUNTO CON TODA LA INFORMACION DEL USUARIO
            return Ok(response);
        }
    }
}