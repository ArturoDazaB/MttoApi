using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MttoApi.Model;
using MttoApi.Model.Context;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MttoApi.Controllers
{
    //===================================================================================================
    //===================================================================================================
    //SE AÑADE A LA CLASE EL ROUTING "ApiController" LA CUAL IDENTIFICARA A LA CLASE "ConfiguracionCon-
    //troller" COMO UN CONTROLADOR DEL WEB API.
    [ApiController]

    //SE AÑADE A LA CLASE EL ROUTING "Route" JUNTO CON LA DIRECCION A LA CUAL SE DEBE LLAMAR PARA PODER
    //ACCESO A LA CLASE CONTROLLADOR. EJ:
    //https:/<ipadress>:<port>/mttoapp/cosultatableros <=> https://192.168.1.192:8000/mttoapp/consultatableros
    [Route("mttoapp/consultatableros")]
    public class ConsultaTablerosController : ControllerBase
    {
        //SE CREA UNA VARIABLE LOCAL DEL TIPO "Context" LA CUAL FUNCIONA COMO LA CLASE
        //QUE MAPEARA LA INFORMACION PARA LECTURA Y ESCRITURA EN LA BASE DE DATOS
        private readonly MTTOAPP_V6Context _context;

        //===============================================================================================
        //===============================================================================================
        //CONSTRUCTOR
        public ConsultaTablerosController(MTTOAPP_V6Context context)
        {
            //SE INICIALIZA LA VARIABLE LOCAL
            this._context = context;
        }

        //========================================================================================================
        //========================================================================================================
        // POST: mttoapp/consultatableros/tableroid
        //SE ADICIONA EL ROUTING "HttpPost" LO CUAL INDICARA QUE LA FUNCION "ConsultaTableroId" RESPONDERA A
        //A SOLICITUDES HTTP DE TIPO POST
        [HttpPost]

        //SE ADICIONA EL ROUTING "Route" JUNTO A DIRECCION A ADICIONAR PARA REALIZAR EL LLAMADO A ESTA 
        //FUNCION MEDIANTE UNA SOLICITUD HTTP. EJ:
        //https:/<ipadress>:<port>/mttoapp/consultatableros/tableroid <=> 
        //https://192.168.1.192:8000/mttoapp/consultatableros/tableroid
        [Route("tableroid")]

        //--------------------------------------------------------------------------------------------------
        //FUNCION QUE ACTUALIZARA LA INFORMACION DE UN USUARIO CUANDO SE REALICE EL LLAMADO DESDE 
        //LA PAGINA "PaginaConsultaTableros" DE LA APLICACION "Mtto App". EN ESTA FUNCION SE RECIBEN 
        //LOS PARAMETROS: 
        // -info => OBJETO DEL TIPO "RequestConsultaTablero" ENVIADO EN EL BODY DE LA SOLICITUD HTTP
        // JUNTO CON LA INFORMACION NECESARIA PARA PROCESAR LA CONSULTA DE TABLEROS.
        //--------------------------------------------------------------------------------------------------
        public async Task<ActionResult<RegistroTablero>> ConsultaTableroId([FromBody] RequestConsultaTablero info)
        {
            //SE CREA E INICIALIZA EL OBJETO DEL TIPO "RegistroTablero" QUE CONTENDRA Y RETORNARA
            //TODA LA INFORMACION DEL TABLERO CONSULTADO.
            RegistroTablero tablero = null;

            //SE EVALUA SI EXISTE ALGUN TABLERO DENTRO DE LA BASE DE DATOS QUE POSEEA EL ID ENVIADO COMO PARAMETRO
            if (this._context.Tableros.Any
                (x => x.TableroId.ToLower() == info.TableroId.ToLower())) //=> true => EXISTE UN REGISTRO DENTRO DE LA CLASE TABLERO CON DICHO DATO
            {
                //SE INICIA LA TRANSACCIONES CON LA BASE DE DATOS
                using(var transaction = this._context.Database.BeginTransaction())
                {
                    //SE INICIA EL CICLO TRY... CATCH
                    try
                    {
                        //--------------------------------------------------------------------------------------------------------
                        //CON LA INFORMACION ENVIADA SE PROCEDE A BUSCAR LA INFORMACION DE USUARIO DENTRO DE LA BASE DE DATOS
                        //SE CREA EL OBJETO "tableroinfo", Y SE INICIALIZA CON EL METODO DE BUSQUEDA PROPIO DEL OBJETO "Context"
                        //DECRITO AL INICIO DE LA CLASE.
                        Tableros tableroinfo = await this._context.Tableros.FirstAsync
                            (x => x.TableroId.ToLower() == info.TableroId.ToLower());   //=> METODO QUE INSPECCIONA TODOS LOS REGISTROS DE
                                                                                        //LA TABLA "Tableros" Y COMPARA EL ID DE CADA REGISTRO
                                                                                        //CON EL ID DEL OBJETO ENVIADO COMO PARAMETRO DE FUNCION

                        //--------------------------------------------------------------------------------------------------------
                        //SE EVALUA QUE EL OBJETO NO SEA NULO
                        if (tableroinfo != null)
                        {
                            //SI ES DIFERENTE DE NULO DESECHAMOS LA ENTIDAD RETENIDA POR EF (ENTITYFRAMEWORK)
                            this._context.Entry(tableroinfo).State = EntityState.Detached;
                        }
                            
                        //--------------------------------------------------------------------------------------------------------
                        //CREAMOS E INICIALIZAMOS UNA LISTA DE OBJETOS "Items" CON LOS ITEMS QUE POSEE DICHO TABLERO
                        List<Items> itemstablero = 
                            await this._context.Items.Where(x => x.TableroId.ToLower() == info.TableroId.ToLower()).ToListAsync();

                        //--------------------------------------------------------------------------------------------------------
                        //SE CREA E INICIALIZA UN OBJETO DEL TIPO "HistorialConsultaTableros" QUE SERVIRA PARA CONTENER
                        //LA INFORMACION DEL NUEVO REGISTRO DENTRO DE LA TABLA "Modificacionesusuario"
                        Historialconsultatableros newregistro =
                            Historialconsultatableros.NewHistorialConsultaTableros(tableroinfo.TableroId, "CONSULTA_POR_TABLERO_ID", info.UserId);

                        //--------------------------------------------------------------------------------------------------------
                        //SE AÑADE A LA TABLAS "HistorialConsultaTableros" EL NUEVO REGISTRO
                        this._context.Historialconsultatableros.Add(newregistro);   //=> SE CREA LA INFORMACION DE UN NUEVO REGISTRO EN LA TABLA HistorialConsultaTableros.
                        this._context.Entry(newregistro).State = EntityState.Added; //=> SE CAMBIA EL ESTADO DEL OBJETO CREADO COMO REFERENCIA.

                        //--------------------------------------------------------------------------------------------------------
                        //SE CREA E INICIALIZA UN OBJETO DEL TIPO "HistorialSolicitudesWeb" CON LA INFORMACION DEL NUEVO REGISTRO
                        //DE LA TABLA "HistorialSolicitudesWeb".
                        Historialsolicitudesweb solicitudweb =
                            Historialsolicitudesweb.NewHistorialSolocitudesWeb(info.UserId,2);

                        //--------------------------------------------------------------------------------------------------------
                        //SE AÑADE A LA TABLA "HistorialSolicitudesWeb" EL NUEVO REGISTRO
                        this._context.Historialsolicitudesweb.Add(solicitudweb);    //=> SE CREA LA INFORMACION DE UN NUEVO REGISTRO EN LA TABLA HistorialSolicitudesWeb.
                        this._context.Entry(solicitudweb).State = EntityState.Added;//=> SE CAMBIA EL ESTADO DEL OBJETO CREADO COMO REFERENCIA.

                        //--------------------------------------------------------------------------------------------------------
                        //INICIALIZAMOS EL OBJETO "RegistroTablero" (CREADO AL INICIO DE LA FUNCION) CON TODA LA 
                        //INFORMACION DEL TABLERO SOLICITADO 
                        tablero = RegistroTablero.NewRegistroTablero(tableroinfo, itemstablero);

                        //--------------------------------------------------------------------------------------------------------
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
                        //SE RETONA LA RESPUESTA "BadRequest" JUNTO CON UN MENSAJE INFORMANDO SOBRE EL ERROR
                        return BadRequest("\nHa ocurrico un error:\n" + ex.Message.ToString());
                    }
                }
            }
            //NO SE CONSIGUIO NINGUN REGISTRO QUE POSEEA EL ID ENVIADO COMO PARAMETRO
            else
            {
                //SE RETORNA EL CODIGO 404 NOT FOUND (NO ENCONTRADO)
                return NotFound("No se encontro registro de tableros con el siguiente Id: " + info.TableroId);
            }

            //SE RETORNA EL CODIGO 200 OK JUNTO CON TODA LA INFORMACION DEL TABELRO SOLICITADO
            return Ok(tablero);
        }

        //========================================================================================================
        //========================================================================================================
        // POST: mttoapp/consultatableros/consultasapid
        //SE ADICIONA EL ROUTING "HttpPost" LO CUAL INDICARA QUE LA FUNCION "ConsultaTableroId" RESPONDERA A
        //A SOLICITUDES HTTP DE TIPO POST
        [HttpPost]

        //SE ADICIONA EL ROUTING "Route" JUNTO A DIRECCION A ADICIONAR PARA REALIZAR EL LLAMADO A ESTA 
        //FUNCION MEDIANTE UNA SOLICITUD HTTP. EJ:
        //https:/<ipadress>:<port>/mttoapp/consultatableros/sapid <=> 
        //https://192.168.1.192:8000/mttoapp/consultatableros/sapid
        [Route("sapid")]

        //--------------------------------------------------------------------------------------------------
        //FUNCION QUE ACTUALIZARA LA INFORMACION DE UN USUARIO CUANDO SE REALICE EL LLAMADO DESDE 
        //LA PAGINA "PaginaConsultaTableros" DE LA APLICACION "Mtto App". EN ESTA FUNCION SE RECIBEN 
        //LOS PARAMETROS: 
        // -info => OBJETO DEL TIPO "RequestConsultaTablero" ENVIADO EN EL BODY DE LA SOLICITUD HTTP
        // JUNTO CON LA INFORMACION NECESARIA PARA PROCESAR LA CONSULTA DE TABLEROS.
        //--------------------------------------------------------------------------------------------------
        public async Task<ActionResult<RegistroTablero>> ConsultaTableroSapId([FromBody] RequestConsultaTablero info)
        {
            //SE CREA E INICIALIZA EL OBJETO DEL TIPO "RegistroTablero" QUE CONTENDRA Y RETORNARA
            //TODA LA INFORMACION DEL TABLERO CONSULTADO.
            RegistroTablero tablero = null;

            //SE EVALUA SI EXISTE ALGUN TABLERO DENTRO DE LA BASE DE DATOS QUE POSEEA EL ID ENVIADO COMO PARAMETRO
            if (this._context.Tableros.Any
                (x => x.SapId.ToLower() == info.SapId.ToLower())) //=> true => EXISTE UN REGISTRO DENTRO DE LA TABLA "Tableros" CON DICHO DATO
            {
                //SE INICIA LA TRANSACCIONES CON LA BASE DE DATOS
                using (var transaction = this._context.Database.BeginTransaction())
                {
                    //SE INICIA LA TRANSACCIONES CON LA BASE DE DATOS
                    try
                    {
                        //--------------------------------------------------------------------------------------------------------
                        //CON LA INFORMACION ENVIADA SE PROCEDE A BUSCAR LA INFORMACION DE USUARIO DENTRO DE LA BASE DE DATOS
                        //SE CREA EL OBJETO "tableroinfo", Y SE INICIALIZA CON EL METODO DE BUSQUEDA PROPIO DEL OBJETO "Context"
                        //DECRITO AL INICIO DE LA CLASE.
                        //CREAMOS E INICIALIZAMOS UN OBJETO "Tableros" CON LA INFORMACION DEL TABLERO
                        Tableros tableroinfo = this._context.Tableros.First
                            (x => x.SapId.ToLower() == info.SapId.ToLower());   //=> METODO QUE INSPECCIONA TODOS LOS REGISTROS DE
                                                                                //LA TABLA "Tableros" Y COMPARA EL ID DE CADA REGISTRO
                                                                                //CON EL ID DEL OBJETO ENVIADO COMO PARAMETRO DE FUNCION

                        //--------------------------------------------------------------------------------------------------------
                        //SE EVALUA QUE EL OBJETO NO SEA NULO
                        if (tableroinfo != null)
                        {
                            //SI ES DIFERENTE DE NULO DESECHAMOS LA ENTIDAD RETENIDA POR EF
                            this._context.Entry(tableroinfo).State = EntityState.Detached;
                        }


                        //--------------------------------------------------------------------------------------------------------
                        //CREAMOS E INICIALIZAMOS UNA LISTA DE OBJETOS "Items" CON LOS ITEMS QUE POSEE DICHO TABLERO
                        List<Items> itemstablero = await this._context.Items.Where
                            (x => x.TableroId.ToLower() == tableroinfo.TableroId.ToLower()).ToListAsync();

                        //--------------------------------------------------------------------------------------------------------
                        //SE CREA E INICIALIZA UN OBJETO DEL TIPO "HistorialConsultaTableros" QUE SERVIRA PARA CONTENER
                        //LA INFORMACION DEL NUEVO REGISTRO DENTRO DE LA TABLA "Modificacionesusuario"
                        Historialconsultatableros newregistro =
                            Historialconsultatableros.NewHistorialConsultaTableros(tableroinfo.TableroId, "CONSULTA_POR_TABLERO_ID", info.UserId);

                        //--------------------------------------------------------------------------------------------------------
                        //SE AÑADE A LA TABLAS "HistorialConsultaTableros" EL NUEVO REGISTRO
                        this._context.Historialconsultatableros.Add(newregistro);   //=> SE CREA LA INFORMACION DE UN NUEVO REGISTRO EN LA TABLA HistorialConsultaTableros.
                        this._context.Entry(newregistro).State = EntityState.Added; //=> SE CAMBIA EL ESTADO DEL OBJETO CREADO COMO REFERENCIA.

                        //--------------------------------------------------------------------------------------------------------
                        //SE CREA E INICIALIZA UN OBJETO DEL TIPO "HistorialSolicitudesWeb" CON LA INFORMACION DEL NUEVO REGISTRO
                        //DE LA TABLA "HistorialSolicitudesWeb".
                        Historialsolicitudesweb solicitudweb =
                            Historialsolicitudesweb.NewHistorialSolocitudesWeb(info.UserId, 3);

                        //--------------------------------------------------------------------------------------------------------
                        //SE AÑADE A LA TABLA "HistorialSolicitudesWeb" EL NUEVO REGISTRO
                        this._context.Historialsolicitudesweb.Add(solicitudweb);    //=> SE CREA LA INFORMACION DE UN NUEVO REGISTRO EN LA TABLA HistorialSolicitudesWeb.
                        this._context.Entry(solicitudweb).State = EntityState.Added;//=> SE CAMBIA EL ESTADO DEL OBJETO CREADO COMO REFERENCIA.

                        //--------------------------------------------------------------------------------------------------------
                        //INICIALIZAMOS EL OBJETO "RegistroTablero" (CREADO AL INICIO DE LA FUNCION) CON TODA LA 
                        //INFORMACION DEL TABLERO SOLICITADO 
                        tablero = RegistroTablero.NewRegistroTablero(tableroinfo, itemstablero);

                        //--------------------------------------------------------------------------------------------------------
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
                        //SE RETONA LA RESPUESTA "BadRequest" JUNTO CON UN MENSAJE INFORMANDO SOBRE EL ERROR
                        return BadRequest("\nHa ocurrico un error:\n" + ex.Message.ToString());
                    }
                }
            }
            //NO SE CONSIGUIO NINGUN REGISTRO QUE POSEEA EL ID ENVIADO COMO PARAMETRO
            else
            {
                //SE RETORNA EL CODIGO 404 NOT FOUND (NO ENCONTRADO)
                return NotFound();
            }

            //SE RETORNA EL CODIGO 200 OK JUNTO CON TODA LA INFORMACION DEL TABELRO SOLICITADO
            return Ok(tablero);
        }
    }
}
