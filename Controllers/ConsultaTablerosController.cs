using Microsoft.AspNetCore.Authorization;
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
    //SE AÑADE A LA CLASE EL ROUTING "ApiController" LA CUAL IDENTIFICARA A LA CLASE "ConsultaTablerosCon-
    //troller" COMO UN CONTROLADOR DEL WEB API.
    [Authorize]
    [ApiController]

    //SE AÑADE A LA CLASE EL ROUTING "Route" JUNTO CON LA DIRECCION A LA CUAL SE DEBE LLAMAR PARA PODER
    //ACCESO A LA CLASE CONTROLLADOR. EJ:
    //https:/<ipadress>:<port>/mttoapp/cosultatableros <=> https://192.168.1.192:8000/mttoapp/consultatableros
    [Route("mttoapp/consultatableros")]
    public class ConsultaTablerosController : ControllerBase
    {
        //SE CREA UNA VARIABLE LOCAL DEL TIPO "Context" LA CUAL FUNCIONA COMO LA CLASE
        //QUE MAPEARA LA INFORMACION PARA LECTURA Y ESCRITURA EN LA BASE DE DATOS
        private readonly MTTOAPP_V7Context _context;

        //===============================================================================================
        //===============================================================================================
        //CONSTRUCTOR
        public ConsultaTablerosController(MTTOAPP_V7Context context)
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
                using (var transaction = this._context.Database.BeginTransaction())
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
                            Historialsolicitudesweb.NewHistorialSolocitudesWeb(info.UserId, 2);

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
                        Console.WriteLine("\nHa ocurrico un error:\n" + ex.Message.ToString());
                        //SE RETONA LA RESPUESTA "BadRequest" JUNTO CON UN MENSAJE INFORMANDO SOBRE EL ERROR
                        return BadRequest("\nHa ocurrico un error, intentelo nuevamente");
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
        //SE ADICIONA EL ROUTING "HttpPost" LO CUAL INDICARA QUE LA FUNCION "ConsultaTableroSapId" RESPONDERA A
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
            //NO SE CONSIGUIO NINGUN REGISTRO QUE POSEEA EL ID ENVIADO COMO PARAMETRO
            else
            {
                //SE RETORNA EL CODIGO 404 NOT FOUND (NO ENCONTRADO)
                return NotFound("No se encontro registro de tableros con el siguiente Id: " + info.SapId);
            }

            //SE RETORNA EL CODIGO 200 OK JUNTO CON TODA LA INFORMACION DEL TABELRO SOLICITADO
            return Ok(tablero);
        }

        //========================================================================================================
        //========================================================================================================
        // DELETE: mttoapp/consultatableros/deleteinfo
        //https://<ipaddress>:<port>/mttoapp/consultatableros/delete
        [HttpPost]
        [Route("delete")]
        public async Task<IActionResult> EliminarTablero([FromBody] RequestConsultaTablero info)
        {
            //SE VERIFICA QUE EL OBJETO QUE CONTIENE LA INFOMACION RECIBIDA NO SEA NULO
            if (info != null)   //=> OBJETO "info" NO NULO
            {
                //SE BUSCAN REGISTROS EXISTENTES QUE COINCIDAN CON LA INFORMACION RECIBIDA
                if (this._context.Tableros.Any
                    (x => x.SapId.ToLower() == info.SapId.ToLower()) &&       //=> SE VERIFICA LA EXISTENCIA DE UN REGISTRO CON LA INFORMACION CONTENIDA EN "info.SapId"
                    this._context.Tableros.Any
                    (x => x.TableroId.ToLower() == info.TableroId.ToLower())) //=> SE VERIFICA LA EXISTENCIA DE UN REGISTRO CON LA INFORMACION CONTENIDA EN "info.TableroID"
                {
                    //SE INICIA LA TRANSACCIONES CON LA BASE DE DATOS
                    using (var transaction = this._context.Database.BeginTransaction())
                    {
                        try
                        {
                            //COMPARAMOS LA INFOMACION DE TABLERO DEVUELTA CUANDO SE REALIZAN LAS CONSULTAS POR:
                            // - CONSULTA DE TABLERO FILTRANDO EL CAMPO "TableroID"
                            // - CONSULTA DE TABLERO FILTRANDO EL CAMPO "SapID"
                            //NOTA: CONDICIONAL USADO PARA CERTIFICAR LA INFORMACION CONTENIDA EN EL OBJETO "info"
                            if ((this._context.Tableros.First(x => x.TableroId.ToLower() == info.TableroId.ToLower())) ==
                                (this._context.Tableros.First(x => x.SapId.ToLower() == info.SapId.ToLower())))
                            {
                                //CREAMOS EL OBJETO QUE RECIBIRA TODA LA INFORMACION RELACIONADA CON EL TABLERO
                                Tableros tableroinfo = 
                                    await this._context.Tableros.FirstAsync(x => x.TableroId.ToLower() == info.TableroId.ToLower());

                                //SE ACTUALIZA EL REGISRO DENTRO DE LA BASE DE DATOS
                                this._context.Tableros.Remove(tableroinfo);

                                //SE ENCONTRO EL TABLERO EN LA BASE DE DATOS SE DESECHA 
                                this._context.Entry(tableroinfo).State = EntityState.Deleted;

                                //RECORREMOS TODOS LOS REGISTROS DE ITEMS 
                                foreach (Items x in this._context.Items.ToList())
                                {
                                    //VERIFICAMOS QUE EL CAMPO "TableroID" DEL REGISTRO EVALUADO COINCIDA CON EL
                                    //DATO CONTENIDO EN EL OBJETO REIBIDO.
                                    if (x.TableroId.ToLower() == info.TableroId.ToLower())
                                    {
                                        //SE ELIMINA EL REGISTO DEL ITEM
                                        this._context.Remove(x);
                                        //SE ENCONTRO EL ITEM EN LA BASE DE DATOS SE DESECHA 
                                        this._context.Entry(x).State = EntityState.Deleted;
                                    }
                                    else
                                    {
                                        //SE ENCONTRO EL ITEM EN LA BASE DE DATOS SE DESECHA 
                                        this._context.Entry(x).State = EntityState.Detached;
                                    }
                                }

                                //SE GUARDAN LOS CAMBIOS.
                                await this._context.SaveChangesAsync();
                            }
                            else //LA INFORMACION DE TABLERO DEVUELTA CUANDO SE REALIZAN LAS CONSULTAS POR LOS CAMPOS
                                //"TableroId" Y "SapID" NO COINCIDE.
                            {
                                //SE RETONA LA RESPUESTA "BadRequest" JUNTO CON UN MENSAJE INFORMANDO SOBRE EL ERROR
                                BadRequest("Informacion de tablero inconsistente");
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

                        //SE TERMINA LA TRANSACCION
                        await transaction.CommitAsync();
                    }
                }
            }
            else //=> OBJETO "info" NULO
            {
                //SE RETONA LA RESPUESTA "BadRequest" JUNTO CON UN MENSAJE INFORMANDO SOBRE EL ERROR
                return BadRequest("Objeto vacio O nulo");
            }

            return Ok();
        }

        //========================================================================================================
        //========================================================================================================
        //SE ADICIONA EL ROUTING "HttpPost" LO CUAL INDICARA QUE LA FUNCION "ModifyItem" RESPONDERA A
        //A SOLICITUDES HTTP DE TIPO POST
        // POST mttoapp/consultatableros/createitem
        [HttpPost("createitem")]
        public async Task<ActionResult<List<Items>>> CreateItem([FromBody] Items item2create)
        {
            //CREAMOS E INICIALIZAMOS LA LISTA QUE CONTENDRA LOS REGISTROS A RETORNAR
            List<Items> tableroitems = new List<Items>();

            //SE VERIFICA QUE EL OBJETO RECIBIDO EN EL BODY DE LA SOLICITUD NO SE ENCUENTE VACIO
            if (item2create != null)
            {
                //SE INICIA LA TRASACCION CON LA BASE DE DATOS
                using (var transaction = this._context.Database.BeginTransaction())
                {
                    //SE INICIA EL CICLO TRY... CATCH PARA MANEJO DE EXCEPCIONES
                    //CON LAS TRANSACCIONES CON LA BASE DE DATOS
                    try
                    {
                        //SE AÑADE EL OBJETO "Items"
                        this._context.Items.Add(item2create);
                        //SE CAMBIA EL ESTADO DE LA ENTIDAD QUE ESTA SIENDO RETENIDA POR EF
                        this._context.Entry(item2create).State = EntityState.Added;

                        //SE GUARDAN LOS CAMBIOS.
                        await this._context.SaveChangesAsync();

                        //SE LISTAN TODOS LOS REGISTROS DE LA TABLA "Items".
                        List<Items> allitemlist = await this._context.Items.ToListAsync();

                        //INSPECCIONAMOS CADA UNO DE LOS ELEMENTOS DENTRO DE LS LISTA "allitemslist"
                        foreach (Items x in allitemlist)
                        {
                            //SI EL id DEL ELEMENTO INSPECCIONADO ES IGUAL AL ID DEL TABLERO 
                            //DEL ITEM MODIFICADO
                            if (x.TableroId == item2create.TableroId)
                            {
                                //SI LOS ID COINCIDEN AÑADIMOS EL ELEMENTO A LA LISTA "tableroitems".
                                tableroitems.Add(x);
                            }
                        }
                    }
                    //SI OCURRE ALGUNA EXCEPCION EN EL PROCESO DE LECTURA Y ESCRITURA DE LA BASE DE DATOS EL CODIGO
                    //SE REDIRIGE A LA SECCION CATCH DEL CICLO TRY...CATCH
                    catch (Exception ex) when (ex is DbUpdateException ||
                                               ex is DbUpdateConcurrencyException)
                    {
                        //SE RETONA LA RESPUESTA "BadRequest" JUNTO CON UN MENSAJE INFORMANDO SOBRE EL ERROR
                        BadRequest("Ha ocurrido un error");
                    }

                    //SE TERMINA LA TRANSACCION
                    await transaction.CommitAsync();

                }
            }
            else
            {
                //NO SE CONSIGUIO EL OBJETO EN LA BASE DE DATOS 
                return BadRequest("Ha ocurrido un error");
            }

            return Ok(tableroitems);
        }

        //========================================================================================================
        //========================================================================================================
        //SE ADICIONA EL ROUTING "HttpPost" LO CUAL INDICARA QUE LA FUNCION "CreateItem" RESPONDERA A
        //A SOLICITUDES HTTP DE TIPO POST
        // POST mttoapp/consultatableros/modifyitem
        [HttpPost("modifyitem")]
        public async Task<ActionResult<List<Items>>> ModifyItem([FromBody] Items item2modify)
        {
            //CREAMOS E INICIALIZAMOS LA LISTA QUE CONTENDRA LOS REGISTROS A RETORNAR
            List<Items> tableroitems = new List<Items>();

            //SE VERIFICA QUE EL OBJETO RECIBIDO EN EL BODY DE LA SOLICITUD NO SE ENCUENTE VACIO
            if (item2modify != null)
            {
                //SE INICIA LA TRASACCION CON LA BASE DE DATOS
                using (var transaction = this._context.Database.BeginTransaction())
                {
                    //SE INICIA EL CICLO TRY... CATCH PARA MANEJO DE EXCEPCIONES
                    //CON LAS TRANSACCIONES CON LA BASE DE DATOS
                    try
                    {
                        //SE BUSCA LA INFORMACION DEL ITEM QUE SE DESEA MODIFICAR
                        var infoitem = await this._context.Items.FindAsync(item2modify.Id);

                        //SE VERIFICA QUE LA INFORMACION DEL OBJETO DEVUELTO DE LA BUSQUEDA NO
                        //SE ENCUENTRE NULO O VACIO
                        if (infoitem != null)
                        {
                            //SE ENCONTRO EL ITEM EN LA BASE DE DATOS SE DESECHA 
                            this._context.Entry(infoitem).State = EntityState.Detached;

                            //SE ACTUALIZA EL OBJETO CON LA INFORMACION REGISTRADA EN LA BASE 
                            //DE DATOS CON LA INFORMACION RECIBIDA
                            infoitem = item2modify;

                            //SE ACTUALIZA EL REGISRO DENTRO DE LA BASE DE DATOS
                            this._context.Update(infoitem);
                            this._context.Entry(infoitem).State = EntityState.Modified;

                            //SE GUARDAN LOS CAMBIOS.
                            await this._context.SaveChangesAsync();

                            //SE LISTAN TODOS LOS REGISTROS DE LA TABLA "Items".
                            List<Items> allitemlist = await this._context.Items.ToListAsync();

                            //INSPECCIONAMOS CADA UNO DE LOS ELEMENTOS DENTRO DE LS LISTA "allitemslist"
                            foreach (Items x in allitemlist)
                            {
                                //SI EL id DEL ELEMENTO INSPECCIONADO ES IGUAL AL ID DEL TABLERO 
                                //DEL ITEM MODIFICADO
                                if (x.TableroId == item2modify.TableroId)
                                {
                                    //SI LOS ID COINCIDEN AÑADIMOS EL ELEMENTO A LA LISTA "tableroitems".
                                    tableroitems.Add(x);
                                }
                            }

                            //CONTAMOS LA CANTIDAD DE ITEMS DE LA LISTA "tableroitems"
                            if (tableroitems.Count == 0)
                            {
                                //SI LA LISTA NO TIENE NINGUN REGISTRO (LO CUAL NO PUEDE SER POSIBLE
                                //DEBIDO A QUE DEBE EXISTIR MINIMO UN REGISTRO DEBIDO A QUE ESTA
                                //FUNCION SE ACTIVA AL MOMENTO DE MODIFICAR UN REGISTRO DE ITEM) 
                                //RETORNAREMOS UN MENSAJE DE ERROR JUNTO CON EL CODIGO DE SOLICITUD 
                                //400 BAD REQUEST.
                                return BadRequest("Error");
                            }
                        }
                        else
                        {
                            //NO SE CONSIGUIO EL OBJETO EN LA BASE DE DATOS 
                            return BadRequest("El item que desea modificar no se encuentra registado");
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
                        BadRequest("Ha ocurrido un error");
                    }

                    //SE TERMINA LA TRANSACCION
                    await transaction.CommitAsync();
                }
            }
            else
            {
                BadRequest("Ha ocurrido un error, intente nuevamente");
            }
            return Ok(tableroitems);
        }

        //========================================================================================================
        //========================================================================================================
        //SE ADICIONA EL ROUTING "HttpPost" LO CUAL INDICARA QUE LA FUNCION "DliminarItem" RESPONDERA A
        //A SOLICITUDES HTTP DE TIPO POST
        // POST mttoapp/consultatableros/deleteitem
        [HttpPost("deleteitem")]
        public async Task<ActionResult<List<Items>>> DeleteItem([FromBody] Items item2delete)
        {
            List<Items> tableroitems = new List<Items>();

            //SE VERIFICA QUE EL OBJETO RECIBIDO EN EL BODY DE LA SOLICITUD NO SE ENCUENTE VACIO
            if (item2delete != null)
            {
                //SE INICIA LA TRASACCION CON LA BASE DE DATOS
                using (var transaction = this._context.Database.BeginTransaction())
                {
                    //SE INICIA EL CICLO TRY... CATCH PARA MANEJO DE EXCEPCIONES
                    //CON LAS TRANSACCIONES CON LA BASE DE DATOS
                    try
                    {
                        //SE BUSCA LA INFORMACION DEL ITEM QUE SE DESEA MODIFICAR
                        var infoitem = await this._context.Items.FindAsync(item2delete.Id);
                        //SE VERIFICA QUE LA INFORMACION DEL OBJETO DEVUELTO DE LA BUSQUEDA NO
                        //SE ENCUENTRE NULO O VACIO
                        if (infoitem != null)
                        {
                            //SE ENCONTRO EL ITEM EN LA BASE DE DATOS SE DESECHA 
                            this._context.Entry(infoitem).State = EntityState.Detached;

                            //SE ACTUALIZA EL REGISRO DENTRO DE LA BASE DE DATOS
                            this._context.Remove(infoitem);

                            //SE GUARDAN LOS CAMBIOS.
                            await this._context.SaveChangesAsync();

                            //SE LISTAN TODOS LOS REGISTROS DE LA TABLA "Items".
                            List<Items> allitemlist = await this._context.Items.ToListAsync();

                            //INSPECCIONAMOS CADA UNO DE LOS ELEMENTOS DENTRO DE LS LISTA "allitemslist"
                            foreach (Items x in allitemlist)
                            {
                                //SI EL id DEL ELEMENTO INSPECCIONADO ES IGUAL AL ID DEL TABLERO 
                                //DEL ITEM MODIFICADO
                                if (x.TableroId == item2delete.TableroId)
                                {
                                    //SI LOS ID COINCIDEN AÑADIMOS EL ELEMENTO A LA LISTA "tableroitems".
                                    tableroitems.Add(x);
                                }
                            }
                        }
                        else
                        {
                            //NO SE CONSIGUIO EL OBJETO EN LA BASE DE DATOS 
                            return BadRequest("El item que desea modificar no se encuentra registado");
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
                        BadRequest("Ha ocurrido un error");
                    }

                    //SE TERMINA LA TRANSACCION
                    await transaction.CommitAsync();

                }
            }
            return Ok(tableroitems);
        }
    }
}