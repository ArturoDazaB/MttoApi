using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using MttoApi.Model;
using MttoApi.Model.Context;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MttoApi   .Controllers
{
    [Route("mttoapp/configuracion")]
    [ApiController]
    public class ConfiguracionController : ControllerBase
    {
        private readonly MTTOAPP_V6Context _context;

        public ConfiguracionController(MTTOAPP_V6Context context)
        {
            this._context = context;
        }

        //===============================================================================================
        //===============================================================================================

        [HttpPut]
        [Route("usuario/{cedula}")]
        public async Task<IActionResult> ActualizarUsuario(double cedula, [FromBody] ConfiguracionU newinfo)
        {
            //SE VERIFICA QUE EL ATRIBUTO "Cedula" DENTRO DEL OBJETO "NewInfo" CONTENGA EL MISMO
            //VALOR EL QUE PARAMETRO "cedula"
            if (cedula != newinfo.Cedula)
                return BadRequest("La cedula no coincide con la informacion del objeto enviado");

            //SE INICIA LA TRASACCION
            using (var transaction = this._context.Database.BeginTransaction())
            {
                try
                {
                    //SE RECIBE INFORMACION QUE SE TIENE ACTUALMENTE EN LA BASE DE DATOS
                    Personas persona = await this._context.Personas.FindAsync(newinfo.Cedula);
                    if (persona != null)
                        this._context.Entry(persona).State = EntityState.Detached;

                    Usuarios usuario = await this._context.Usuarios.FindAsync(newinfo.Cedula);
                    if (usuario != null)
                        this._context.Entry(usuario).State = EntityState.Detached;

                    //SE VERIFICA QUE LOS OBJETOS ENVIADOS NO SEAN NULOS
                    if (persona == null && usuario == null)
                        return NotFound("Numero de cedula no registrado: " + cedula);

                    //SE CREA E INICIALIZA UN OBJETO DEL TIPO "ModificacionesUsuario" QUE SERVIRA PARA CONTENER
                    //LA INFORMACION DEL NUEVO REGISTRO DENTRO DE LA TABLA "Modificacionesusuario"
                    Modificacionesusuario newmodificacionesiusuario = 
                        Modificacionesusuario.NewModificacionesUsuarios(persona, Personas.NewPersonaInfo(persona, newinfo),
                                                                        usuario, Usuarios.NewUsuarioInfo(usuario, newinfo),
                                                                        DateTime.Now, newinfo.Cedula);

                    //SE ACTUALIZA LA INFORMACION DENTRO DE LOS OBJETOS PERSONAS
                    persona = Personas.NewPersonaInfo(persona, newinfo);
                    usuario = Usuarios.NewUsuarioInfo(usuario, newinfo);

                    //SE ACTUALIZA LA INFORMACION DENTRO DE LA BASE DE DATOS
                    this._context.Personas.Update(persona);
                    this._context.Entry(persona).State = EntityState.Modified;

                    this._context.Usuarios.Update(usuario);
                    this._context.Entry(usuario).State = EntityState.Modified;

                    this._context.Modificacionesusuario.Add(newmodificacionesiusuario); 
                    this._context.Entry(newmodificacionesiusuario).State = EntityState.Added;

                    //--------------------------------------------------------------------------------------------------------
                    //SE CREA E INICIALIZA UN OBJETO DEL TIPO "HistorialSolicitudesWeb" CON LA INFORMACION DEL NUEVO REGISTRO
                    //DE LA TABLA "HistorialSolicitudesWeb".
                    Historialsolicitudesweb solicitudesweb =
                        Historialsolicitudesweb.NewHistorialSolocitudesWeb(cedula, 11);

                    //SE ALMACENA EL REGISTRO DENTRO DE LA BASE DE DATOS
                    this._context.Historialsolicitudesweb.Add(solicitudesweb);
                    this._context.Entry(solicitudesweb).State = EntityState.Added;
                    //-------------------------------------------------------------------------------------------------------

                    //SE GUARDAN LOS CAMBIOS
                    await this._context.SaveChangesAsync();

                    //SE TERMINA LA TRANSACCION
                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    return BadRequest("\nHa ocurrico un error:\n" + ex.Message.ToString());
                }
            }

            return Ok("Datos actualizados");
        }

        [HttpPut]
        [Route("administrator/{cedula}")]
        public async Task<IActionResult> ActualizarUsuarioAdm(double cedula, [FromBody] ConfiguracionA newinfo)
        {
            if (cedula != newinfo.Cedula)
                return BadRequest("La cedula no coincide con la informacion del objeto enviado");

            using (var transaction = this._context.Database.BeginTransaction())
            {
                try
                {
                    //SE RECIBE INFORMACION QUE SE TIENE ACTUALMENTE EN LA BASE DE DATOS
                    Personas persona = await this._context.Personas.FindAsync(newinfo.Cedula);
                    if (persona != null)
                        this._context.Entry(persona).State = EntityState.Detached;

                    Usuarios usuario = await this._context.Usuarios.FindAsync(newinfo.Cedula);
                    if (usuario != null)
                        this._context.Entry(usuario).State = EntityState.Detached;

                    //SE VERIFICA QUE LOS OBJETOS ENVIADOS NO SEAN NULOS
                    if (persona == null && usuario == null)
                        return NotFound("Numero de cedula no registrado: " + cedula);

                    //SE CREA E INICIALIZA UN OBJETO DEL TIPO "ModificacionesUsuario" QUE SERVIRA PARA CONTENER
                    //LA INFORMACION DEL NUEVO REGISTRO DENTRO DE LA TABLA "Modificacionesusuario"
                    //----------------------------------------------------------------------------------------------
                    //NOTA: SE ENVIA EL NUMERO CERO (0) COMO ULTIMO PARAMETRO EN EL METODO NewModificacionesUsuarios
                    //DEBIDO A QUE EL USUARIO ADMINISTRATOR ES EL UNICO CON ACCESO A DICHO METODO
                    //----------------------------------------------------------------------------------------------
                    Modificacionesusuario newmodificacionesiusuario =
                        Modificacionesusuario.NewModificacionesUsuarios(persona, Personas.NewPersonaInfo(persona, newinfo),
                                                                        usuario, Usuarios.NewUsuarioInfo(usuario, newinfo),
                                                                        DateTime.Now, 0);

                    //SE ACTUALIZA LA INFORMACION DENTRO DE LOS OBJETOS PERSONAS
                    persona = Personas.NewPersonaInfo(persona, newinfo);
                    usuario = Usuarios.NewUsuarioInfo(usuario, newinfo);

                    //SE ACTUALIZA LA INFORMACION DENTRO DE LA BASE DE DATOS
                    this._context.Personas.Update(persona);
                    this._context.Usuarios.Update(usuario);

                    //SE AÑADE A LA TABLA "ModificacionesUsuarios" UN NUEVO REGISTRO
                    this._context.Modificacionesusuario.Add(newmodificacionesiusuario);
                    this._context.Entry(newmodificacionesiusuario).State = EntityState.Added;
                    //--------------------------------------------------------------------------------------------------------
                    //SE CREA E INICIALIZA UN OBJETO DEL TIPO "HistorialSolicitudesWeb" CON LA INFORMACION DEL NUEVO REGISTRO
                    //DE LA TABLA "HistorialSolicitudesWeb".
                    Historialsolicitudesweb solicitudesweb =
                        Historialsolicitudesweb.NewHistorialSolocitudesWeb(0, 12);

                    //SE ALMACENA EL REGISTRO DENTRO DE LA BASE DE DATOS
                    this._context.Historialsolicitudesweb.Add(solicitudesweb);
                    this._context.Entry(solicitudesweb).State = EntityState.Added;
                    //-------------------------------------------------------------------------------------------------------

                    //SE GUARDAN LOS CAMBIOS
                    await this._context.SaveChangesAsync();

                    //SE TERMINA LA TRANSACCION
                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    return BadRequest("\nHa ocurrico un error:\n" + ex.Message.ToString());
                }
            }

            return Ok("Datos Actualizados");
        }

        //===============================================================================================
        //===============================================================================================
    }
}