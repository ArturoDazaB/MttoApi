using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
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
    //https:/<ipadress>:<port>/mttoapp/configuracion <=> https://192.168.1.192:8000/mttoapp/configuracion
    [Route("mttoapp/configuracion")]

    public class ConfiguracionController : ControllerBase
    {
        //SE CREA UNA VARIABLE LOCAL DEL TIPO "Context" LA CUAL FUNCIONA COMO LA CLASE
        //QUE MAPEARA LA INFORMACION PARA LECTURA Y ESCRITURA EN LA BASE DE DATOS
        private readonly MTTOAPP_V6Context _context;

        //===============================================================================================
        //===============================================================================================
        //CONSTRUCTOR
        public ConfiguracionController(MTTOAPP_V6Context context)
        {
            //SE INICIALIZA LA VARIABLE LOCAL
            this._context = context;
        }

        //===============================================================================================
        //===============================================================================================
        //SE ADICIONA EL ROUTING "HttpPut" LO CUAL INDICARA QUE LA FUNCION "ActualizarUsuario" RESPONDERA A
        //A SOLICITUDES HTTP DE TIPO PUT
        [HttpPut]

        //SE ADICIONA EL ROUTING "Route" JUNTO A DIRECCION A ADICIONAR PARA REALIZAR EL LLAMADO A ESTA 
        //FUNCION MEDIANTE UNA SOLICITUD HTTP. EJ:
        //https:/<ipadress>:<port>/mttoapp/configuracion/usuario/<numero_de_cedula> <=> 
        //https://192.168.1.192:8000/mttoapp/configuracion/usuario/<numero_de_cedula>
        [Route("usuario/{cedula}")]

        //--------------------------------------------------------------------------------------------------
        //FUNCION QUE ACTUALIZARA LA INFORMACION DE UN USUARIO CUANDO SE REALICE EL LLAMADO DESDE 
        //LA PAGINA "PaginaConfiguracion" DE LA APLICACION "Mtto App". EN ESTA FUNCION SE RECIBEN 
        //LOS PARAMETROS: 
        // -cedula => DATO DEFINIDO EN EL URL DE LA SOLICITUD (POR ESTA RAZON EL ROUTING "Route" 
        //CONTIENE LA PALABRA "{cedula}")
        // -newinfo => OBJETO ENVIADO EN EL BODY DE LA SOLICITUD HTTP EL CUAL CONTIENE TODA LA 
        //INFORMACION (VIEJA Y ACTUALIZADA) DEL USUARIO A ACTUALIZAR/CONFIGURAR
        //--------------------------------------------------------------------------------------------------
        public async Task<IActionResult> ActualizarUsuario(double cedula, [FromBody] ConfiguracionU newinfo)
        {
            //SE EVALUA SI PARAMETRO "cedula" Y LA PROPIEDAD "cedula" DEL OBJETO "newinfo" SON DIFERENTES
            if (cedula != newinfo.Cedula) //=> LAS PROPIEDADES SON DISTINTAS
            {
                //SE DA RETORNO A LA SOLICITUD CON LA RESPUESTA "BadRequest" JUNTO CON UN MENSAJE INFORMANDO AL USUARIO
                return BadRequest("La cedula no coincide con la informacion del objeto enviado");
            }
                
            //SI EL PARAMETRO "cedula" Y LA PROPIEDAD "cedula" DEL OBJETO "newinfo" SON SIMILARES
            //SE INICIA LA TRASACCION
            using (var transaction = this._context.Database.BeginTransaction())
            {
                //SE INICIA EL CICLO TRY... CATCH
                try
                {
                    //CON LA INFORMACION ENVIADA SE PROCEDE A BUSCAR LA INFORMACION DE USUARIO DENTRO DE LA BASE DE DATOS
                    //SE CREA EL OBJETO "persona", Y SE INICIALIZA CON EL METODO DE BUSQUEDA PROPIO DEL OBJETO "Context"
                    //DECRITO AL INICIO DE LA CLASE
                    Personas persona = await this._context.Personas.FindAsync(newinfo.Cedula);

                    //SE EVALUA SI EL OBJETO "persona" NO SE ENCUENTRA NULO
                    if (persona != null)
                    {
                        //EL OBJETO "persona" NO SE ENCUENTRA NULO.
                        //PUESTO QUE SE UTILIZO LA CLASE "Context" PARA REALIZAR LA BUSQUEDA DE INFORMACION DE UN REGISTRO 
                        //EN UNA TABLA ESPECIFICA SE DEBE "desechar" EL OBJETO QUE ACABA DE SER BUSCADO.
                        this._context.Entry(persona).State = EntityState.Detached;
                    }

                    //CON LA INFORMACION ENVIADA SE PROCEDE A BUSCAR LA INFORMACION DE USUARIO DENTRO DE LA BASE DE DATOS
                    //SE CREA EL OBJETO "usuario", Y SE INICIALIZA CON EL METODO DE BUSQUEDA PROPIO DEL OBJETO "Context"
                    //DECRITO AL INICIO DE LA CLASE
                    Usuarios usuario = await this._context.Usuarios.FindAsync(newinfo.Cedula);

                    if (usuario != null)
                    {
                        //EL OBJETO "persona" NO SE ENCUENTRA NULO.
                        //PUESTO QUE SE UTILIZO LA CLASE "Context" PARA REALIZAR LA BUSQUEDA DE INFORMACION DE UN REGISTRO 
                        //EN UNA TABLA ESPECIFICA SE DEBE "desechar" EL OBJETO QUE ACABA DE SER BUSCADO.
                        this._context.Entry(usuario).State = EntityState.Detached;
                    }

                    //SE VERIFICA SI LOS OBJETOS "personas" Y "usuarios" CREADOS E INICIALIZADOS PREVIAMENTE
                    //SE ENCUENTRAN NULOS (SE EVALUA LA "NULIDAD" DE AMBOS)
                    if (persona == null && usuario == null)
                    {
                        //SI LOS OBJETOS (AMBOS O SOLO UNO) SE ENCUENTRAN NULOS ESTO IMPLICA QUE LA CLASE "Context" NO 
                        //ENCONTRO NINGUN REGISTRO QUE RESPONDIERA AL NUMERO DE CEDULA ENVIADO COMO PARAMETRO.
                        //SE RETORNA LA RESPUESTA "NotFound" JUNTO CON UN MENSAJE INFORMANDO AL USUARIO.
                        return NotFound("Numero de cedula no registrado: " + cedula);
                    }

                    //--------------------------------------------------------------------------------------------------------
                    //SE CREA E INICIALIZA UN OBJETO DEL TIPO "ModificacionesUsuario" QUE SERVIRA PARA CONTENER
                    //LA INFORMACION DEL NUEVO REGISTRO DENTRO DE LA TABLA "Modificacionesusuario"
                    Modificacionesusuario newmodificacionesiusuario = 
                        Modificacionesusuario.NewModificacionesUsuarios(persona, Personas.NewPersonaInfo(persona, newinfo),
                                                                        usuario, Usuarios.NewUsuarioInfo(usuario, newinfo),
                                                                        DateTime.Now, newinfo.Cedula);

                    //--------------------------------------------------------------------------------------------------------
                    //SE ACTUALIZA LA INFORMACION DENTRO DE LOS OBJETOS "persona" Y "usuario"
                    persona = Personas.NewPersonaInfo(persona, newinfo);
                    usuario = Usuarios.NewUsuarioInfo(usuario, newinfo);

                    //--------------------------------------------------------------------------------------------------------
                    //SE ACTUALIZA LA INFORMACION DE LAS RESPECTIVAS TABLAS DENTRO DE LA BASE DE DATOS
                    this._context.Personas.Update(persona);                     //=> SE ACTUALIZA LA INFORMACION EN LA TABLA PERSONA
                    this._context.Entry(persona).State = EntityState.Modified;  //=> SE CAMBIA EL ESTADO DEL OBJETO CREADO COMO REFERENCIA
                    //_____________________________________________________________________________________________________________
                    //NOTA: CAMBIAR EL ESTADO DE LOS OBJETOS CREADO COMO REFERENCIA PERMITE A LA CLASE CONTEXTO SEPARAR OBJETOS
                    //MODIFICADOS, AÑADIDOS, DESECHADOS. DE ESTA MANERA CADA QUE SE EJECUTE UN CAMBIO EN LA BASE DE DATOS CON ALGUN
                    //OBJETO DE REFERENCIA LUEGO DE SU USO SE DEBE REFERENCIAR QUE SE HIZO CON DICHO OBJETO.
                    //_____________________________________________________________________________________________________________
                    this._context.Usuarios.Update(usuario);                     //=> SE ACTUALIZA LA INFORMACION EN LA TABLA USUARIO
                    this._context.Entry(usuario).State = EntityState.Modified;  //=> SE CAMBIA EL ESTADO DEL OBJETO CREADO COMO REFERENCIA

                    this._context.Modificacionesusuario.Add(newmodificacionesiusuario);         //=> SE CREA LA INFORMACION DE UN NUEVO REGISTRO EN LA TABLA MODIFICACIONESUSUARIOS
                    this._context.Entry(newmodificacionesiusuario).State = EntityState.Added;   //=> SE CAMBIA EL ESTADO DEL OBJETO CREADO COMO REFERENCIA

                    //--------------------------------------------------------------------------------------------------------
                    //SE CREA E INICIALIZA UN OBJETO DEL TIPO "HistorialSolicitudesWeb" CON LA INFORMACION DEL NUEVO REGISTRO
                    //DE LA TABLA "HistorialSolicitudesWeb".
                    Historialsolicitudesweb solicitudesweb =
                        Historialsolicitudesweb.NewHistorialSolocitudesWeb(cedula, 12);

                    //SE ALMACENA EL REGISTRO DENTRO DE LA BASE DE DATOS
                    this._context.Historialsolicitudesweb.Add(solicitudesweb);      //=> SE CREA LA INFORMACION DE UN NUEVO REGISTRO EN LA TABLA HistorialSolicitudesWeb
                    this._context.Entry(solicitudesweb).State = EntityState.Added;  //=> SE CAMBIA EL ESTADO DEL OBJETO CREADO COMO REFERENCIA
                    //-------------------------------------------------------------------------------------------------------

                    //SE GUARDAN LOS CAMBIOS
                    await this._context.SaveChangesAsync();

                    //SE TERMINA LA TRANSACCION
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

            //SI NO EXISTIERON ERRORES O EXCEPCIONES EN EL PROCESO DE LECTURA Y ESCRITURA, Y LA INFORMACION
            //FUE ACTUALIZADA SATISFACTORIAMENTE SE RETORNA LA RESPUESTA "Ok" JUNTO CON UN MENSAJE INFORMANDO 
            //SOBRE LA ACTUALIZACION EXITOSA.
            return Ok("Datos actualizados");
        }

        //===============================================================================================
        //===============================================================================================
        //SE ADICIONA EL ROUTING "HttpPut" LO CUAL INDICARA QUE LA FUNCION "ActualizarUsuarioAdmin" RESPONDERA A
        //A SOLICITUDES HTTP DE TIPO PUT
        [HttpPut]

        //SE ADICIONA EL ROUTING "Route" JUNTO A DIRECCION A ADICIONAR PARA REALIZAR EL LLAMADO A ESTA 
        //FUNCION MEDIANTE UNA SOLICITUD HTTP. EJ:
        //https:/<ipadress>:<port>/mttoapp/configuracion/administrator/<cedula> <=> 
        //https://192.168.1.192:8000/mttoapp/configuracion/administrator/<cedula>
        [Route("administrator/{cedula}")]

        //--------------------------------------------------------------------------------------------------
        //FUNCION QUE ACTUALIZARA LA INFORMACION DE UN USUARIO CUANDO SE REALICE EL LLAMADO DESDE 
        //LA PAGINA "PaginaConfiguracionAdmin" DE LA APLICACION "Mtto App". EN ESTA FUNCION SE RECIBEN 
        //LOS PARAMETROS: 
        // -cedula => DATO DEFINIDO EN EL URL DE LA SOLICITUD (POR ESTA RAZON EL ROUTING "Route" 
        //CONTIENE LA PALABRA "{cedula}")
        // -newinfo => OBJETO ENVIADO EN EL BODY DE LA SOLICITUD HTTP EL CUAL CONTIENE TODA LA 
        //INFORMACION (VIEJA Y ACTUALIZADA) DEL USUARIO A ACTUALIZAR/CONFIGURAR
        //--------------------------------------------------------------------------------------------------
        public async Task<IActionResult> ActualizarUsuarioAdm(double cedula, [FromBody] ConfiguracionA newinfo)
        {
            //SE EVALUA SI PARAMETRO "cedula" Y LA PROPIEDAD "cedula" DEL OBJETO "newinfo" SON DIFERENTES
            if (cedula != newinfo.Cedula)
            {
                //SE DA RETORNO A LA SOLICITUD CON LA RESPUESTA "BadRequest" JUNTO CON UN MENSAJE INFORMANDO AL USUARIO
                return BadRequest("La cedula no coincide con la informacion del objeto enviado");
            }

            //SI EL PARAMETRO "cedula" Y LA PROPIEDAD "cedula" DEL OBJETO "newinfo" SON SIMILARES
            //SE INICIA LA TRASACCION
            using (var transaction = this._context.Database.BeginTransaction())
            {
                //SE INICIA EL CICLO TRY... CATCH
                try
                {
                    //SE RECIBE INFORMACION QUE SE TIENE ACTUALMENTE EN LA BASE DE DATOS
                    Personas persona = await this._context.Personas.FindAsync(newinfo.Cedula);
                    if (persona != null)
                    {
                        this._context.Entry(persona).State = EntityState.Detached;
                    }

                    Usuarios usuario = await this._context.Usuarios.FindAsync(newinfo.Cedula);
                    if (usuario != null)
                    {
                        this._context.Entry(usuario).State = EntityState.Detached;
                    }

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

                    //--------------------------------------------------------------------------------------------------------
                    //SE ACTUALIZA LA INFORMACION DENTRO DE LOS OBJETOS PERSONAS
                    persona = Personas.NewPersonaInfo(persona, newinfo);
                    usuario = Usuarios.NewUsuarioInfo(usuario, newinfo);

                    //--------------------------------------------------------------------------------------------------------
                    //SE ACTUALIZA LA INFORMACION DENTRO DE LA BASE DE DATOS
                    //SE ACTUALIZA LA INFORMACION DE LAS RESPECTIVAS TABLAS DENTRO DE LA BASE DE DATOS
                    this._context.Personas.Update(persona);                     //=> SE ACTUALIZA LA INFORMACION EN LA TABLA PERSONA
                    this._context.Entry(persona).State = EntityState.Modified;  //=> SE CAMBIA EL ESTADO DEL OBJETO CREADO COMO REFERENCIA


                    this._context.Usuarios.Update(usuario);                     //=> SE ACTUALIZA LA INFORMACION EN LA TABLA USUARIO
                    this._context.Entry(usuario).State = EntityState.Modified;  //=> SE CAMBIA EL ESTADO DEL OBJETO CREADO COMO REFERENCIA

                    //SE AÑADE A LA TABLA "ModificacionesUsuarios" UN NUEVO REGISTRO
                    this._context.Modificacionesusuario.Add(newmodificacionesiusuario);         //=> SE CREA LA INFORMACION DE UN NUEVO REGISTRO EN LA TABLA MODIFICACIONESUSUARIOS
                    this._context.Entry(newmodificacionesiusuario).State = EntityState.Added;   //=> SE CAMBIA EL ESTADO DEL OBJETO CREADO COMO REFERENCIA
                    //--------------------------------------------------------------------------------------------------------
                    //SE CREA E INICIALIZA UN OBJETO DEL TIPO "HistorialSolicitudesWeb" CON LA INFORMACION DEL NUEVO REGISTRO
                    //DE LA TABLA "HistorialSolicitudesWeb".
                    Historialsolicitudesweb solicitudesweb =
                        Historialsolicitudesweb.NewHistorialSolocitudesWeb(0, 13);

                    //SE ALMACENA EL REGISTRO DENTRO DE LA BASE DE DATOS
                    this._context.Historialsolicitudesweb.Add(solicitudesweb);      //=> SE CREA LA INFORMACION DE UN NUEVO REGISTRO EN LA TABLA HistorialSolicitudesWeb
                    this._context.Entry(solicitudesweb).State = EntityState.Added;  //=> SE CAMBIA EL ESTADO DEL OBJETO CREADO COMO REFERENCIA
                    //-------------------------------------------------------------------------------------------------------

                    //SE GUARDAN LOS CAMBIOS
                    await this._context.SaveChangesAsync();

                    //SE TERMINA LA TRANSACCION
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

            //SI NO EXISTIERON ERRORES O EXCEPCIONES EN EL PROCESO DE LECTURA Y ESCRITURA, Y LA INFORMACION
            //FUE ACTUALIZADA SATISFACTORIAMENTE SE RETORNA LA RESPUESTA "Ok" JUNTO CON UN MENSAJE INFORMANDO 
            //SOBRE LA ACTUALIZACION EXITOSA.
            return Ok("Datos Actualizados");
        }

        //===============================================================================================
        //===============================================================================================
    }
}