using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MttoApi.Model;
using MttoApi.Model.Context;
using System;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MttoApi.Controllers
{
    //===================================================================================================
    //===================================================================================================
    //SE AÑADE A LA CLASE EL ROUTING "ApiController" LA CUAL IDENTIFICARA A LA CLASE "RegistroUsuarios-
    //Controller" COMO UN CONTROLADOR DEL WEB API.
    [ApiController]

    //SE AÑADE A LA CLASE EL ROUTING "Route" JUNTO CON LA DIRECCION A LA CUAL SE DEBE LLAMAR PARA PODER
    //ACCESO A LA CLASE CONTROLLADOR. EJ:
    //https:/<ipadress>:<port>/mttoapp/registro <=> https://192.168.1.192:8000/mttoapp/registro
    [Route("mttoapp/registro")]
    public class RegistroUsuariosController : ControllerBase
    {
        //SE CREA UNA VARIABLE LOCAL DEL TIPO "Context" LA CUAL FUNCIONA COMO LA CLASE
        //QUE MAPEARA LA INFORMACION PARA LECTURA Y ESCRITURA EN LA BASE DE DATOS
        private readonly MTTOAPP_V7Context _context;

        //===============================================================================================
        //===============================================================================================
        //CONSTRUCTOR
        public RegistroUsuariosController(MTTOAPP_V7Context context)
        {
            //SE INICIALIZA LA VARIABLE LOCAL
            this._context = context;
        }

        //===============================================================================================
        //===============================================================================================
        //SE ADICIONA EL ROUTING "HttpPost" LO CUAL INDICARA QUE LA FUNCION "RegistroUsuario" RESPONDERA A
        //A SOLICITUDES HTTP DE TIPO POST
        // POST mttoapp/registro
        [HttpPost]
        //--------------------------------------------------------------------------------------------------
        //FUNCION QUE REGISTRA LA INFORMACION DE UN NUEVO USUARIO EN LA BASE DE DATOS
        //--------------------------------------------------------------------------------------------------
        public async Task<ActionResult<string>> RegistroUsuario([FromBody] RequestRegistroUsuario newuser)
        {
            //SE EVALUA SI LOS SIGUIENTES CAMPOS YA SE ENCUENTRAN REGISTRADOS DENTRO DE LA TABLA PERSONAS
            if (!MatchCedula(newuser.NewUser.Persona.Cedula) &&             //TRUE: SE ENCONTRO UN REGISTRO CON LA MISMA CEDULA (ID)
                !MatchNumeroFicha(newuser.NewUser.Persona.NumeroFicha) &&   //TRUE: SE ENCONTRO UN REGISTRO CON EL MISMO NUMERO DE FICHA
                !MatchUsername(newuser.NewUser.Usuario.Username))           //TRUE: SE ENCONTRO UN REGISTRO CON EL MISMO NOMBRE DE USUARIO
            {
                //SE INICIA EL CICLO TRY... CATCH
                try
                {
                    using (var transaction = this._context.Database.BeginTransaction())
                    {
                        //--------------------------------------------------------------------------------------------------------
                        //SE CREAN LOS OBJETOS DEL TIPO "Personas" Y "Usuarios" QUE CONTENDRAN LA NUEVA INFORMACION
                        Personas persona = Personas.NewPersonas(newuser.NewUser.Persona);
                        Usuarios usuario = Usuarios.NewUsuarios(newuser.NewUser.Usuario);

                        //--------------------------------------------------------------------------------------------------------
                        //SE REGISTRA LA INFORMACION NUEVA
                        this._context.Add(persona);
                        this._context.Entry(persona).State = EntityState.Added;

                        this._context.Add(usuario);
                        this._context.Entry(usuario).State = EntityState.Added;

                        //--------------------------------------------------------------------------------------------------------
                        //SE CREA E INICIALIZA UN NUEVO OBJETO "HistorialSolicitudesWeb"
                        Historialsolicitudesweb solicitudweb =
                            Historialsolicitudesweb.NewHistorialSolocitudesWeb(newuser.UserId, 5);

                        //--------------------------------------------------------------------------------------------------------
                        //SE AÑADE EL NUEVO REGISTRO A LA BASE DE DATOS
                        this._context.Historialsolicitudesweb.Add(solicitudweb);      //=> SE CREA LA INFORMACION DE UN NUEVO REGISTRO EN LA TABLA HistorialSolicitudesWeb.
                        this._context.Entry(solicitudweb).State = EntityState.Added;  //=> SE CAMBIA EL ESTADO DEL OBJETO CREADO COMO REFERENCIA.

                        //--------------------------------------------------------------------------------------------------------
                        //SE COMPRUEBAN QUE LAS TABLAS "Personas" Y "Usuarios" TENGAN LA MISMA CANTIDAD DE REGISTROS
                        //NOTA: ESTO DEBIDO A QUE ESTAS DOS TABLAS LLEVAN REGISTROS PARALELOS QUE SE CORRESPONDEN
                        if (this._context.Personas.Count() != this._context.Usuarios.Count()) //=> true => LAS DOS TABLAS TIENEN CANTIDAD DE REGISTROS DISTINTOS
                        {
                            //SE RETORNA UNA RESPUESTA A LA SOLICITUD Y SE PROCEDE A INFORMAR AL USUARIO
                            return BadRequest("Error al intentar regitrar datos. Intente nuevente");
                        }

                        //--------------------------------------------------------------------------------------------------------
                        //SE GUARDAN LOS CAMBIOS
                        await this._context.SaveChangesAsync();
                        //SE CIERRA LA TRANSACCION
                        await transaction.CommitAsync();
                    }
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
            //NO SE CUMPLIO ALGUNA DE LAS TRES CONDICIONES, SE RETORNA UN MENSAJE INFORMANDO CUAL CONDICION FALLO.
            else
            {
                //===============================================================================================
                //SI ALGUNA DE LOS CONDICIONALES EN LA EVALUACION ANTERIOR ES FALSA SE PROCEDE A INFORMAR CUAL
                //DE LOS CAMPOS EVALUADOS YA SE ENCUENTRA REGISTRADO

                //SE EVALUA SI EXISTE ALGUN REGISTRO DE USUARIO CON EL MISMO ID (CEDULA) DEL USUARIO QUE DESEA REGISTRAR
                if (MatchCedula(newuser.NewUser.Persona.Cedula))
                    return BadRequest("Numero de cedula ya registrado: " + newuser.NewUser.Persona.Cedula.ToString());

                //SE EVALUA SI EXISTE ALGUN REGISTRO DE USUARIO CON EL MISMO NUMERO DE FICHA DEL USUARIO QUE DESEA REGISTRAR
                if (MatchNumeroFicha(newuser.NewUser.Persona.NumeroFicha))
                    return BadRequest("Numero de ficha ya registrado: " + newuser.NewUser.Persona.NumeroFicha.ToString());

                //SE EVALUA SI EXISTE ALGUN REGISTRO DE USUARIO CON EL NOMBRE DE USUARIO DEL USUARIO QUE DESEA REGISTRAR
                if (MatchUsername(newuser.NewUser.Usuario.Username))
                    return BadRequest("Nombre de usuario" + newuser.NewUser.Usuario.Username + "ya se encuentra registrado");
            }

            //SI TODAS LAS CONDICIONES SE CUMPLEN SE REGISTRA EL USUARIO, SE RETORNA EL CODIGO DE ESTATUS 200
            //OK Y SE INFORMA MEDIANTE UN MENSAJE QUE SE REGISTRO CON EXITO EL TABLERO.
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