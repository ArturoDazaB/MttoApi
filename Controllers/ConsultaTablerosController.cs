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
    [Route("mttoapp/consultatableros")]
    [ApiController]
    public class ConsultaTablerosController : ControllerBase
    {
        private readonly MTTOAPP_V6Context _context;

        public ConsultaTablerosController(MTTOAPP_V6Context context)
        {
            this._context = context;
        }

        //========================================================================================================
        //========================================================================================================
        // GET: mttoapp/consultatableros/consultatableroid
        [HttpGet]
        [Route("tableroid")]
        public async Task<ActionResult<RegistroTablero>> ConsultaTableroId([FromBody] RequestConsultaTablero info)
        {
            //SE CREA E INICIALIZA EL OBJETO DEL TIPO "RegistroTablero" QUE CONTENDRA Y RETORNARA
            //TODA LA INFORMACION DEL TABLERO CONSULTADO.
            RegistroTablero tablero = null;

            //SE EVALUA SI EXISTE ALGUN TABLERO DENTRO DE LA BASE DE DATOS QUE POSEEA EL ID ENVIADO COMO PARAMETRO
            if (this._context.Tableros.Any(x => x.TableroId.ToLower() == info.TableroId.ToLower()))
            {
                //SE INICIA LA TRANSACCIONES CON LA BASE DE DATOS
                using(var transaction = this._context.Database.BeginTransaction())
                {
                    try
                    {
                        //CREAMOS E INICIALIZAMOS UN OBJETO "Tableros" CON LA INFORMACION DEL TABLERO
                        Tableros tableroinfo = await this._context.Tableros.FirstAsync(x => x.TableroId.ToLower() == info.TableroId.ToLower());
                        //SE EVALUA QUE EL OBJETO NO SEA NULO
                        if (tableroinfo != null)
                            //SI ES DIFERENTE DE NULO DESECHAMOS LA ENTIDAD RETENIDA POR EF
                            this._context.Entry(tableroinfo).State = EntityState.Detached;
                        
                        //CREAMOS E INICIALIZAMOS UNA LISTA DE OBJETOS "Items" CON LOS ITEMS QUE 
                        //POSEE DICHO TABLERO
                        List<Items> itemstablero = await this._context.Items.Where(x => x.TableroId.ToLower() == info.TableroId.ToLower()).ToListAsync();

                        //CREAMOS E INICIALIZAMOS UN OBJETO "HistorialConsultaTableros"
                        Historialconsultatableros newregistro =
                            Historialconsultatableros.NewHistorialConsultaTableros(tableroinfo.TableroId, "CONSULTA_POR_TABLERO_ID", info.UserId);
                        
                        //SE AÑADE A LA TABLAS "HistorialConsultaTableros" EL NUEVO REGISTRO
                        this._context.Historialconsultatableros.Add(newregistro);
                        this._context.Entry(newregistro).State = EntityState.Added;

                        //CREAMOS E INICIALIZAMOS UN OBJETO "HistorialSolicitudesWeb"
                        Historialsolicitudesweb solicitudweb =
                            Historialsolicitudesweb.NewHistorialSolocitudesWeb(info.UserId,2);

                        //SE AÑADE A LA TABLA "HistorialSolicitudesWeb" EL NUEVO REGISTRO
                        this._context.Historialsolicitudesweb.Add(solicitudweb);
                        this._context.Entry(solicitudweb).State = EntityState.Added;

                        //INICIALIZAMOS EL OBJETO "RegistroTablero" CON TODA LA INFORMACION DEL TABLERO SOLICITADO 
                        tablero = RegistroTablero.NewRegistroTablero(tableroinfo, itemstablero);
                        //SE GUARDAN LOS CAMBIOS REALIZADOS SOBRE LA BASE DE DATOS
                        await this._context.SaveChangesAsync();
                        //SE CULMINA LA TRANSACCION CON LA BASE DE DATOS
                        await transaction.CommitAsync();
                    }
                    //OCURRIO UN ERROR DURANTE EL TRY
                    catch (Exception ex)
                    {
                        //SE RETORNA EL CODIGO 400 Bad Request JUNTO CON EL MENSAJE DE ERROR
                        return BadRequest("Error: " + ex.Message);
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
        // GET: mttoapp/consultatableros/consultasapid
        [HttpGet]
        [Route("sapid")]
        public async Task<ActionResult<RegistroTablero>> ConsultaTableroSapId([FromBody] RequestConsultaTablero info)
        {
            //SE CREA E INICIALIZA EL OBJETO DEL TIPO "RegistroTablero" QUE CONTENDRA Y RETORNARA
            //TODA LA INFORMACION DEL TABLERO CONSULTADO.
            RegistroTablero tablero = null;

            //SE EVALUA SI EXISTE ALGUN TABLERO DENTRO DE LA BASE DE DATOS QUE POSEEA EL ID ENVIADO COMO PARAMETRO
            if (this._context.Tableros.Any(x => x.SapId.ToLower() == info.SapId.ToLower()))
            {
                //SE INICIA LA TRANSACCIONES CON LA BASE DE DATOS
                using (var transaction = this._context.Database.BeginTransaction())
                {
                    try
                    {
                        //CREAMOS E INICIALIZAMOS UN OBJETO "Tableros" CON LA INFORMACION DEL TABLERO
                        Tableros tableroinfo = this._context.Tableros.First(x => x.SapId.ToLower() == info.SapId.ToLower());

                        //SE EVALUA QUE EL OBJETO NO SEA NULO
                        if (tableroinfo != null)
                            //SI ES DIFERENTE DE NULO DESECHAMOS LA ENTIDAD RETENIDA POR EF
                            this._context.Entry(tableroinfo).State = EntityState.Detached;

                        //CREAMOS E INICIALIZAMOS UNA LISTA DE OBJETOS "Items" CON LOS ITEMS QUE POSEE DICHO TABLERO
                        List<Items> itemstablero = await this._context.Items.Where(x => x.TableroId.ToLower() == tableroinfo.TableroId.ToLower()).ToListAsync();

                        //CREAMOS E INICIALIZAMOS UN OBJETO "HistorialConsultaTableros"
                        Historialconsultatableros newregistro =
                            Historialconsultatableros.NewHistorialConsultaTableros(tableroinfo.TableroId, "CONSULTA_POR_TABLERO_ID", info.UserId);

                        //SE AÑADE A LA TABLAS "HistorialConsultaTableros" EL NUEVO REGISTRO
                        this._context.Historialconsultatableros.Add(newregistro);
                        this._context.Entry(newregistro).State = EntityState.Added;

                        //CREAMOS E INICIALIZAMOS UN OBJETO "HistorialSolicitudesWeb"
                        Historialsolicitudesweb solicitudweb =
                            Historialsolicitudesweb.NewHistorialSolocitudesWeb(info.UserId, 3);

                        //SE AÑADE A LA TABLA "HistorialSolicitudesWeb" EL NUEVO REGISTRO
                        this._context.Historialsolicitudesweb.Add(solicitudweb);
                        this._context.Entry(solicitudweb).State = EntityState.Added;

                        //INICIALIZAMOS EL OBJETO "RegistroTablero" CON TODA LA INFORMACION DEL TABLERO SOLICITADO 
                        tablero = RegistroTablero.NewRegistroTablero(tableroinfo, itemstablero);
                        //SE CULMINA LA TRANSACCION CON LA BASE DE DATOS
                        await transaction.CommitAsync();
                    }
                    //OCURRIO UN ERROR DURANTE EL TRY
                    catch (Exception ex)
                    {
                        //SE RETORNA EL CODIGO 400 Bad Request JUNTO CON EL MENSAJE DE ERROR
                        return BadRequest("Error: " + ex.ToString());
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
