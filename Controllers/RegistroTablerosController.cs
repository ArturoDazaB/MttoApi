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
    [Route("mttoapp/registrotableros")]
    [ApiController]
    public class RegistroTablerosController : ControllerBase
    {
        private readonly MTTOAPP_V6Context _context;

        public RegistroTablerosController(MTTOAPP_V6Context context)
        {
            this._context = context;
        }

        //========================================================================================================
        //========================================================================================================
        // POST mttoapp/registrotableros
        [HttpPost]
        public async Task<IActionResult> NewTablero([FromBody] RegistroTablero newtablero)
        {
            //SE VERIFICA SI LOS SIGUIENTES DATOS YA SE ENCUENTRAN REGISTRADOS DENTRO DE LA TABLA:
            if (!MatchTableroID(newtablero.tableroInfo.TableroId) &&                //TRUE: SE ENCONTRO UN REGISTRO CON EL MISMO ID DE TABLERO
                !MatchTableroID(newtablero.tableroInfo.SapId) &&                    //TRUE: SE ENCONTRO UN REGISTRO CON EL MISMO ID DE SAP
                !MatchTableroCodigoQRData(newtablero.tableroInfo.CodigoQrdata))     //TRUE: SE ENCONTRO UN REGISTRO CON EL MIDMO CodigoQRData
            {
                //SE INICIA LA TRANSACCION
                using (var transaction = this._context.Database.BeginTransaction())
                {
                    try
                    {
                        //SE AÑADE EL OBJETO "newtablero"
                        this._context.Tableros.Add(newtablero.tableroInfo);
                        //SE CAMBIA EL ESTADO DE LA ENTIDAD QUE ESTA RETENIDA POR EF
                        this._context.Entry(newtablero.tableroInfo).State = EntityState.Added;
                        //SE RECORRE LA LISTA DE ITEMS ENVIADA JUNTO CON LA INFORMACION DEL TABLERO
                        foreach (Items x in newtablero.itemsTablero)
                        {
                            //SE AÑADE EL OBJETO "Items"
                            this._context.Items.Add(x);
                            //SE CAMBIA EL ESTADO DE LA ENTIDAD QUE ESTA SIENDO RETENIDA POR EF
                            this._context.Entry(x).State = EntityState.Added;
                        }

                        //CREACION E INICIALIZACION DEL OBJETO solicitudweb
                        Historialsolicitudesweb solicitudweb =
                            Historialsolicitudesweb.NewHistorialSolocitudesWeb(newtablero.tableroInfo.Idcreador, 4);

                        //SE AÑADE EL NUEVO REGISTRO A LA BASE DE DATOS
                        this._context.Historialsolicitudesweb.Add(solicitudweb);
                        this._context.Entry(solicitudweb).State = EntityState.Added;

                        //SE GUARDAN LOS CAMBIOS
                        await this._context.SaveChangesAsync();
                        //SE CULMINA LA TRANSACCION
                        await transaction.CommitAsync();
                    }
                    //OCURRIO UN ERROR DURANTE EL TRY
                    catch (Exception ex)
                    {
                        //SE RETORNA EL CODIGO 400 Bad Request JUNTO CON EL MENSAJE DE ERROR
                        return BadRequest("Ocurrio un error:  " + ex.ToString());
                    }
                }
            }
            //NO SE CUMPLIO ALGUNA DE LAS TRES CONDICIONES, SE RETORNA UN MENSAJE INFORMANDO CUAL CONDICION FALLO.
            else
            {
                if (MatchTableroSAPID(newtablero.tableroInfo.TableroId))
                    return BadRequest("El ID del tablero que intenta registrar ya se encuentra registrado: " + newtablero.tableroInfo.TableroId);

                if (MatchTableroSAPID(newtablero.tableroInfo.SapId))
                    return BadRequest("El ID de SAP del tablero que intenta registrar ya se encuentra registrado: " + newtablero.tableroInfo.SapId);

                if (MatchTableroCodigoQRData(newtablero.tableroInfo.CodigoQrdata))
                    return BadRequest("El codigo QR del tablero que intenta registrar ya se encuentra asignado a otro tablero");
            }

            //SI TODAS LAS CONDICIONES SE CUMPLEN SE REGISTRA EL TABLERO, SE RETORNA EL CODIGO DE ESTATUS 200
            //OK Y SE INFORMA MEDIANTE UN MENSAJE QUE SE REGISTRO CON EXITO EL TABLERO.
            return Ok("Registro exitoso");
        }

        //========================================================================================================
        //========================================================================================================
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