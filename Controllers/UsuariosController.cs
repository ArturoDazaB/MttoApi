using Microsoft.AspNetCore.Mvc;
using MttoApi.Model.Context;
using System.Linq;

namespace MttoApi.Controllers
{
    //===================================================================================================
    //===================================================================================================
    //SE AÑADE A LA CLASE EL ROUTING "ApiController" LA CUAL IDENTIFICARA A LA CLASE "UsuariosController
    //COMO UN CONTROLADOR DEL WEB API.
    [ApiController]

    //SE AÑADE A LA CLASE EL ROUTING "Route" JUNTO CON LA DIRECCION A LA CUAL SE DEBE LLAMAR PARA PODER
    //ACCESO A LA CLASE CONTROLLADOR. EJ:
    //https:/<ipadress>:<port>/mttoapp/usuarios <=> https://192.168.1.192:8000/mttoapp/usuarios
    [Route("mttoapp/usuarios")]
    public class UsuariosController : ControllerBase
    {
        //SE CREA UNA VARIABLE LOCAL DEL TIPO "Context" LA CUAL FUNCIONA COMO LA CLASE
        //QUE MAPEARA LA INFORMACION PARA LECTURA Y ESCRITURA EN LA BASE DE DATOS
        private readonly MTTOAPP_V7Context _context;

        //===============================================================================================
        //===============================================================================================
        //CONSTRUCTOR
        public UsuariosController(MTTOAPP_V7Context context)
        {
            //SE INICIALIZA LA VARIABLE LOCAL
            this._context = context;
        }

        //===============================================================================================
        //===============================================================================================
        //SE ADICIONA EL ROUTING "HttpGet" LO CUAL INDICARA QUE LA FUNCION "VerifyUsername" RESPONDERA A
        //A SOLICITUDES HTTP DE TIPO GET
        // POST mttoapp/registro
        [HttpGet]

        //SE ADICIONA EL ROUTING "Route" JUNTO A DIRECCION A ADICIONAR PARA REALIZAR EL LLAMADO A ESTA
        //FUNCION MEDIANTE UNA SOLICITUD HTTP. EJ:
        //https:/<ipadress>:<port>/mttoapp/usuarios/verifygeneratedusername <=> https://192.168.1.192:8000/mttoapp/usuarios/verifygeneratedusername
        [Route("verifygeneratedusername")]
        //--------------------------------------------------------------------------------------------------
        //FUNCION QUE RECIBIRA EL NOMBRE DE USUARIO GENERADO POR LA APLICACION, EVALUA SI DICHO APELLIDO
        //SE ENCUENTRA O NO REGISTRADO Y RETORNA VERDADERO O FALSO DEPENIENDO DE LA EVALUACION
        //--------------------------------------------------------------------------------------------------
        public ActionResult<bool> VerifyUsername(string generatedusername)
        {
            //SE VERIFICA QUE EL NOMBRE DE USUARIO ENVIADO NO SEA NULO O VACIO
            if (string.IsNullOrEmpty(generatedusername) == false)
            {
                //SE CONSULTA EL NOMBRE DE USARIO EN CADA UNO DE LOS REGISTROS DENTRO DE LA TABLA "Usuarios"
                //DE EXISTIR => return TRUE
                //DE NO EXISTIR => return false
                return Ok(this._context.Usuarios.Any(x => x.Username.ToLower() == generatedusername.ToLower()));
            }

            //SI EL NOMBRE DE USUARIO SE ENCUENTRA VACIO SE RETORNA UN MENSAJE BAD REQUEST INFORMANDO AL USUARIO
            return BadRequest("El nombre de usuario enviado se encuentra vacio o nulo");
        }

        //===============================================================================================
        //===============================================================================================
        // GET: mttoapp/Usuarios
        [HttpGet]
        //public async Task<ActionResult<IEnumerable<Usuarios>>> GetUsuarios()
        public ActionResult<string> GetUsuarios()
        {
            //return await _context.Usuarios.ToListAsync();
            return Ok("FUNCION DESACTIVADA");
        }

        // GET: mttoapp/usuarios/cedula/12345678
        // GET: mttoapp/usuarios/id/12345678
        [HttpGet]
        [Route("cedula/{cedula}")]
        [Route("id/{cedula}")]
        //public async Task<ActionResult<Usuarios>> GetUsuariosCedula(double cedula)
        public ActionResult<string> GetUsuariosCedula(double cedula)
        {
            /*var usuarios = await _context.Usuarios.FindAsync(cedula);

            if (usuarios == null)
            {
                return NotFound("Cedula no encontrada:  " + cedula);
            }

            return Ok(usuarios);*/

            return Ok("FUNCION DESACTIVADA");
        }

        // GET: api/Usuarios/5
        [HttpGet]
        [Route("ficha/{ficha}")]
        [Route("numeroficha/{ficha}")]
        //public async Task<ActionResult<Usuarios>> GetUsuariosFicha(double ficha)
        public ActionResult<string> GetUsuariosFicha(double ficha)
        {
            /*
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
            }*/

            return Ok("FUNCION DESACTIVADA");
        }

        private bool UsuariosExists(double id)
        {
            return _context.Usuarios.Any(e => e.Cedula == id);
        }
    }
}