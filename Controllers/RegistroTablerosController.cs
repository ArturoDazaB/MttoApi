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
    //SE AÑADE A LA CLASE EL ROUTING "ApiController" LA CUAL IDENTIFICARA A LA CLASE "RegistroTableros-
    //Controller" COMO UN CONTROLADOR DEL WEB API.
    [ApiController]

    //SE AÑADE A LA CLASE EL ROUTING "Route" JUNTO CON LA DIRECCION A LA CUAL SE DEBE LLAMAR PARA PODER
    //ACCESO A LA CLASE CONTROLLADOR. EJ:
    //https:/<ipadress>:<port>/mttoapp/registrotableros <=> https://192.168.1.192:8000/mttoapp/registrotableros
    [Route("mttoapp/registrotableros")]
    public class RegistroTablerosController : ControllerBase
    {
        //SE CREA UNA VARIABLE LOCAL DEL TIPO "Context" LA CUAL FUNCIONA COMO LA CLASE
        //QUE MAPEARA LA INFORMACION PARA LECTURA Y ESCRITURA EN LA BASE DE DATOS
        private readonly MTTOAPP_V6Context _context;

        //===============================================================================================
        //===============================================================================================
        //CONSTRUCTOR
        public RegistroTablerosController(MTTOAPP_V6Context context)
        {
            //SE INICIALIZA LA VARIABLE LOCAL
            this._context = context;
        }

        //========================================================================================================
        //========================================================================================================
        //SE ADICIONA EL ROUTING "HttpPost" LO CUAL INDICARA QUE LA FUNCION "NewTablero" RESPONDERA A
        //A SOLICITUDES HTTP DE TIPO POST
        // POST mttoapp/registrotableros
        [HttpPost]
        //--------------------------------------------------------------------------------------------------
        //FUNCION QUE REGISTRA LA INFORMACION DE UN NUEVO TABLERO EN LA BASE DE DATOS
        //--------------------------------------------------------------------------------------------------
        public async Task<IActionResult> NewTablero([FromBody] RegistroTablero newtablero)
        {
            //SE VERIFICA SI LOS SIGUIENTES DATOS YA SE ENCUENTRAN REGISTRADOS DENTRO DE LA TABLA:
            if (!MatchTableroID(newtablero.tableroInfo.TableroId) &&                //TRUE: SE ENCONTRO UN REGISTRO CON EL MISMO ID DE TABLERO
                !MatchTableroID(newtablero.tableroInfo.SapId) &&                    //TRUE: SE ENCONTRO UN REGISTRO CON EL MISMO ID DE SAP
                !MatchTableroCodigoQRData(newtablero.tableroInfo.CodigoQrdata))     //TRUE: SE ENCONTRO UN REGISTRO CON EL MIDMO CodigoQRData
            {
                //SE INICIA EL CICLO TRY... CATCH
                try
                {
                    //SE INICIA LA TRANSACCION
                    using (var transaction = this._context.Database.BeginTransaction())
                    {
                        //--------------------------------------------------------------------------------------------------------
                        //SE AÑADE EL OBJETO "newtablero"
                        this._context.Tableros.Add(newtablero.tableroInfo);

                        //--------------------------------------------------------------------------------------------------------
                        //SE CAMBIA EL ESTADO DE LA ENTIDAD QUE ESTA RETENIDA POR EF
                        this._context.Entry(newtablero.tableroInfo).State = EntityState.Added;

                        //--------------------------------------------------------------------------------------------------------
                        //SE RECORRE LA LISTA DE ITEMS ENVIADA JUNTO CON LA INFORMACION DEL TABLERO
                        foreach (Items x in newtablero.itemsTablero)
                        {
                            //SE AÑADE EL OBJETO "Items"
                            this._context.Items.Add(x);

                            //SE CAMBIA EL ESTADO DE LA ENTIDAD QUE ESTA SIENDO RETENIDA POR EF
                            this._context.Entry(x).State = EntityState.Added;
                        }

                        //--------------------------------------------------------------------------------------------------------
                        //CREACION E INICIALIZACION DEL OBJETO solicitudweb
                        Historialsolicitudesweb solicitudweb =
                            Historialsolicitudesweb.NewHistorialSolocitudesWeb(newtablero.tableroInfo.Idcreador, 4);

                        //--------------------------------------------------------------------------------------------------------
                        //SE AÑADE EL NUEVO REGISTRO A LA BASE DE DATOS
                        this._context.Historialsolicitudesweb.Add(solicitudweb);      //=> SE CREA LA INFORMACION DE UN NUEVO REGISTRO EN LA TABLA HistorialSolicitudesWeb.
                        this._context.Entry(solicitudweb).State = EntityState.Added;  //=> SE CAMBIA EL ESTADO DEL OBJETO CREADO COMO REFERENCIA.

                        //--------------------------------------------------------------------------------------------------------
                        //SE GUARDAN LOS CAMBIOS
                        await this._context.SaveChangesAsync();
                        //SE CULMINA LA TRANSACCION
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
                //SE EVALUA CUAL DE LAS PROPIEDADES DEL OBJETO "newtablero" ENVIADO COINCIDE CON EL LA INFORMACION DE REGISTRO DE
                //ALGUN OTRO TABLERO

                //SE EVALUA SI EXISTE ALGUN TABLERO CON EL ID DEL TABLERO QUE SE DESEA REGISTRAR
                if (MatchTableroSAPID(newtablero.tableroInfo.TableroId))
                    return BadRequest("El ID del tablero que intenta registrar ya se encuentra registrado: " + newtablero.tableroInfo.TableroId);

                //SE EVALUA SI EXISTE ALGUN TABLERO CON EL ID DE SAP DEL TABLERO QUE SE DESEA REGISTRAR
                if (MatchTableroSAPID(newtablero.tableroInfo.SapId))
                    return BadRequest("El ID de SAP del tablero que intenta registrar ya se encuentra registrado: " + newtablero.tableroInfo.SapId);

                //SE EVALUA SI EXISTE ALGUN TABLERO QUE POSEA EL CODIGOQR (IMAGEN) QUE SE DESEA REGISTRAR
                if (MatchTableroCodigoQRData(newtablero.tableroInfo.CodigoQrdata))
                    return BadRequest("El codigo QR del tablero que intenta registrar ya se encuentra asignado a otro tablero");
            }

            //SI TODAS LAS CONDICIONES SE CUMPLEN SE REGISTRA EL TABLERO, SE RETORNA EL CODIGO DE ESTATUS 200
            //OK Y SE INFORMA MEDIANTE UN MENSAJE QUE SE REGISTRO CON EXITO EL TABLERO.
            return Ok("Registro exitoso");
        }

        //========================================================================================================
        //========================================================================================================
        //FUNCIONES QUE EVALUAN SI EXISTE ALGUN TABLERO REGISTRADO QUE YA POSEA EL PARAMETRO QUE SE LE ES ENVIADO
        private bool MatchTableroID(string id)
        {
            return this._context.Tableros.Any(x => x.TableroId.ToLower() == id.ToLower());
        }

        private bool MatchTableroSAPID(string id)
        {
            return this._context.Tableros.Any(x => x.SapId.ToLower() == id.ToLower());
        }

        private bool MatchTableroCodigoQRData(string data)
        {
            return this._context.Tableros.Any(x => x.CodigoQrdata == data);
        }
    }
}