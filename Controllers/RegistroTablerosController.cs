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
        private readonly MTTOAPP_V7Context _context;

        //===============================================================================================
        //===============================================================================================
        //CONSTRUCTOR
        public RegistroTablerosController(MTTOAPP_V7Context context)
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
        //SE ADICIONA EL ROUTING "HttpPost" LO CUAL INDICARA QUE LA FUNCION "ModifyItem" RESPONDERA A
        //A SOLICITUDES HTTP DE TIPO POST
        // POST mttoapp/registrotableros/createitem
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
                //NO SE CONSIGUIO EL OBJETO EN LA BASE DE DATOS 
                return BadRequest("Ha ocurrido un error");
            }

            return await Task.FromResult(tableroitems);
        }

        //========================================================================================================
        //========================================================================================================
        //SE ADICIONA EL ROUTING "HttpPost" LO CUAL INDICARA QUE LA FUNCION "CreateItem" RESPONDERA A
        //A SOLICITUDES HTTP DE TIPO POST
        // POST mttoapp/registrotableros/modifyitem
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
        // POST mttoapp/registrotableros/deleteitem
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