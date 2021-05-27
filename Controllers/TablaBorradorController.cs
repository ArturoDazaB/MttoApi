using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MttoApi.Model;
using MttoApi.Model.Context;
using System;
using System.Threading.Tasks;

namespace MttoApi.Controllers
{
    //===================================================================================================
    //===================================================================================================
    //SE AÑADE A LA CLASE EL ROUTING "ApiController" LA CUAL IDENTIFICARA A LA CLASE "LogInController"
    //COMO UN CONTROLADOR DEL WEB API.
    [ApiController]

    //===================================================================================================
    //===================================================================================================
    //SE AÑADE A LA CLASE EL ROUTING "Route" JUNTO CON LA DIRECCION A LA CUAL SE DEBE LLAMAR PARA PODER
    //ACCESO A LA CLASE CONTROLLADOR. EJ:
    //https:/<ipadress>:<port>/mttoapp/login <=> https://192.168.1.192:8000/mttoapp/borrador
    [Route("mttoapp/borrador")]
    public class TablaBorradorController : Controller
    {
        //===============================================================================================
        //===============================================================================================
        //SE CREA UNA VARIABLE LOCAL DEL TIPO "Context" LA CUAL FUNCIONA COMO LA CLASE
        //QUE MAPEARA LA INFORMACION PARA LECTURA Y ESCRITURA EN LA BASE DE DATOS
        private readonly MTTOAPP_V6Context _context;

        //===============================================================================================
        //===============================================================================================
        //CONSTRUCTOR
        public TablaBorradorController(MTTOAPP_V6Context context)
        {
            //SE INICIALIZA LA VARIABLE LOCAL
            this._context = context;
        }

        //===============================================================================================
        //===============================================================================================
        //SE ADICIONA EL ROUTING "HttpPost" LO CUAL INDICARA QUE LA FUNCION "Registro" RESPONDERA
        //A SOLICITUDES HTTP DE TIPO POST
        // POST mttoapp/borrador
        [HttpPost]
        public async Task<ActionResult<string>> Registro(string c1, string c2)
        {
            //--------------------------------------------------------------------------------------------------------
            //CONVERTIMOS A SUS TIPOS DE DATOS EN LA BASE DE DATOS
            var columna1 = Convert.ToString(c1);
            var columna2 = Convert.ToDouble(c2);

            //--------------------------------------------------------------------------------------------------------
            //SE CREAN EL OBJETO "Tabla_Borrador" QUE CONTENDRAN LA NUEVA INFORMACION
            TablaBorrador registro = new TablaBorrador();
            registro.Columna1 = columna1; registro.Columna2 = columna2;

            //SE CREA E INICIALIZA UN MENSAJE PROVICIONAL QUE REGRESAREMOS CON LA RESPUESTA DE LA SOLICITUD
            string mensaje = string.Empty;
            //--------------------------------------------------------------------------------------------------------

            //SE INICIA EL CICLO TRY... CATCH
            try
            {
                using (var transaction = this._context.Database.BeginTransaction())
                {
                    //--------------------------------------------------------------------------------------------------------
                    //SE REGISTRA LA INFORMACION NUEVA
                    this._context.Add(registro);                                //=> SE CREA LA INFORMACION DE UN NUEVO REGISTRO EN LA TABLA Tabla_borrador.
                    this._context.Entry(registro).State = EntityState.Added;    //=> SE CAMBIA EL ESTADO DEL OBJETO CREADO COMO REFERENCIA.

                    //--------------------------------------------------------------------------------------------------------
                    //SE GUARDAN LOS CAMBIOS
                    await this._context.SaveChangesAsync();

                    //--------------------------------------------------------------------------------------------------------
                    //MENSAJE PROVICIONAL PARA RETORNAR INFORMACION RESPECTO AL REGISTRO
                    mensaje = "SE REGISTRO EXITOSAMENTE:" + "\n\t" +
                                        "Valor columna 1: " + Convert.ToString(registro.Columna1) + "\n\t" +
                                        "Valor columna 2: " + Convert.ToString(registro.Columna2);

                    //SE CIERRA LA TRANSACCION
                    await transaction.CommitAsync();
                }
            }
            //SI OCURRE ALGUNA EXCEPCION EN EL PROCESO DE LECTURA Y ESCRITURA DE LA BASE DE DATOS EL CODIGO
            //SE REDIRIGE A LA SECCION CATCH DEL CICLO TRY...CATCH
            catch (Exception ex) when (ex is DbUpdateException ||
                                       ex is DbUpdateConcurrencyException)
            {
                //SE RETONA LA RESPUESTA "BadRequest" JUNTO CON UN MENSAJE INFORMANDO SOBRE EL ERROR
                return BadRequest("\nHa ocurrico un error:\n" + ex.Message.ToString());
            }

            //SE RETORNA UN MENSAJE INFORMATIVO
            return Ok(mensaje);
        }
    }
}