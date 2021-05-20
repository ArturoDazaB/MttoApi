using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

using MttoApi.Model;
using MttoApi.Model.Context;
using System;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MttoApi.Controllers
{
    //===================================================================================================
    //===================================================================================================
    //SE AÑADE A LA CLASE EL ROUTING "ApiController" LA CUAL IDENTIFICARA A LA CLASE "QueryAdminContro-
    //ller" COMO UN CONTROLADOR DEL WEB API.
    [ApiController]

    //SE AÑADE A LA CLASE EL ROUTING "Route" JUNTO CON LA DIRECCION A LA CUAL SE DEBE LLAMAR PARA PODER
    //ACCESO A LA CLASE CONTROLLADOR. EJ:
    //https:/<ipadress>:<port>/mttoapp/queryadmin <=> https://192.168.1.192:8000/mttoapp/queryadmin
    [Route("mttoapp/queryadmin")]
    public class QueryAdminController : ControllerBase
    {
        //SE CREA UNA VARIABLE LOCAL DEL TIPO "Context" LA CUAL FUNCIONA COMO LA CLASE
        //QUE MAPEARA LA INFORMACION PARA LECTURA Y ESCRITURA EN LA BASE DE DATOS
        private readonly MTTOAPP_V6Context _context;

        private const string NotFoundMessage= "No ningun registro que coincida con el parametro de consulta ingresado";

        //===============================================================================================
        //===============================================================================================
        //CONSTRUCTOR
        public QueryAdminController(MTTOAPP_V6Context context)
        {
            //SE INICIALIZA LA VARIABLE LOCAL
            this._context = context;
        }

        //===============================================================================================
        //===============================================================================================
        // GET: mttoapp/queryadmin/cedula
        // GET: mttoapp/queryadmin/id
        //SE ADICIONA EL ROUTING "HttpPost" LO CUAL INDICARA QUE LA FUNCION "QueryCedula" RESPONDERA A
        //A SOLICITUDES HTTP DE TIPO POST
        [HttpPost]

        //SE ADICIONA EL ROUTING "Route" JUNTO A DIRECCION A ADICIONAR PARA REALIZAR EL LLAMADO A ESTA 
        //FUNCION MEDIANTE UNA SOLICITUD HTTP. EJ:
        //https://<ipaddress>:<port>/mttoapp/queryadmin/cedula <=> https://192.168.1.192:8000/mttoapp/queryadmin/cedula
        //https://<ipaddress>:<port>/mttoapp/queryadmin/id <=> https://192.168.1.192:8000/mttoapp/queryadmin/id
        [Route("cedula")]
        [Route("id")]

        //--------------------------------------------------------------------------------------------------
        //FUNCION QUE RETORNARA UNA LISTA DE USUARIOS, LOS CUALES DEBEN CUMPLIR CON EL PARAMETRO DE BUSQUEDA
        //ENVIADO. EL LLAMADO SE HACE DESDE LA PAGINA "PaginaQueryAdmin" DE LA APLICACION "Mtto App".
        //EN ESTA FUNCION SE RECIBEN LOS PARAMETROS: 
        // -request:  OBJETO DEL TIPO "RequestQueryAdmin" EL CUAL CONTENDRA EL PARAMETRO ENVIADO Y EL NUMERO
        // DE OPCION DE BUSQUEDA (0 => Consulta por cedula; 1=> Consulta por Ficha; 2=> Consulta por Nombr(s)
        // 3=> Consulta por Apellido(s); 4=> Consulta por Username)
        //--------------------------------------------------------------------------------------------------
        public async Task<ActionResult<List<QueryAdmin>>> QueryCedula([FromBody] RequestQueryAdmin request)
        {
            //CREACION E INICIALIZACION DE VARIABLES
            List<QueryAdmin> result = new List<QueryAdmin>();   //=> LISTA DE USUARIOS QUE COINCIDEN CON EL PARAMETRO ENVIADO

            //CREACION E INICIALIZACION DE LA LISTA DE USUARIOS REGISTRADOS EN LA PLATAFORMA
            List<Personas> registros = await this._context.Personas.ToListAsync();

            //SE RECORRE CADA UNO DE LOS REGISTROS ("Personas")
            foreach (Personas x in registros)
            {
                //SE EVALUAN TODOS LOS REGISTROS MENOS EL REGISTRO DEL USUARIO ADMINISTRATOR
                if (x.Cedula != 0) //=> true => EL REGISTRO EVALUADO NO ES EL USUARIO ADMINISTRATOR
                {
                    //SE EVALUA QUE EL DATO ENVIADO SE ENCUENTRE DENTRO DE LA LISTA REGISTROS.
                    /*------------------------------------------------------------------------
                     NOTA: LA EVALUACION SE HACE TOMANDO EN CUENTA EL TAMAÑO (CANTIDAD DE
                     CARACTERES) DEL PARAMETRO ENVIADO. PUESTO QUE SE CONSIDERA QUE NO SIEMPRE
                     SE ENVIARA TODO EL NUMERO DE CEDULA COMPLETO SE HACE UNA BUSQUEDA DE LOS
                     REGISTROS EXISTENTES ENVIANDO EL NUMERO DE CEDULA COMPLETO O PARCIAL
                     (TODO DEPENDE DE COMO SEA ENVIADO EL PARAMETRO) Y SE RETORNARAN EL
                     REGISTRO O LOS REGISTROS QUE COINCIDAN
                     ------------------------------------------------------------------------*/
                    if (request.Parametro.ToString() == x.Cedula.ToString().Substring(0, request.Parametro.ToString().Length))
                    {
                        //SE AGREGA A LA LISTA EL REGISTRO DE DICHA PERSONA
                        result?.Add(QueryAdmin.NewQueryAdmin(x));
                    }
                }
            }

            //SE EVALUA CUANTOS ELEMENTOS HAY EN LA LISTA "result"
            if (result.Count == 0)
                //CERO ELEMTENTOS: SE RETORNA EL CORIGO DE ESTATUS 404 NOTFOUND (NO HAY REGISTROS QUE CUMPLAN CON EL PARAMETRO ENVIADO)
                return NotFound(NotFoundMessage);
            else
            {
                //SE INICIA EL CICLO TRY...CATCH
                try
                {
                    //DIFERENTE DE CERO: SE RETORNA EL CODIGO DE ESTATUS 200 OK JUNTO CON LA LISTA DE USUARIOS OBTENIDOS
                    using (var transaction = this._context.Database.BeginTransaction())
                    {
                        //--------------------------------------------------------------------------------------------------------
                        //SE CREA E INICIALIZA UN OBJETO DEL TIPO "HistorialSolicitudesWeb" CON LA INFORMACION DEL NUEVO REGISTRO
                        //DE LA TABLA "HistorialSolicitudesWeb".
                        Historialsolicitudesweb solicitudesweb =
                            Historialsolicitudesweb.NewHistorialSolocitudesWeb(request.UserId, 6);

                        //--------------------------------------------------------------------------------------------------------
                        //SE ALMACENA EL REGISTRO DENTRO DE LA BASE DE DATOS
                        this._context.Historialsolicitudesweb.Add(solicitudesweb);      //=> SE CREA LA INFORMACION DE UN NUEVO REGISTRO EN LA TABLA HistorialSolicitudesWeb.
                        this._context.Entry(solicitudesweb).State = EntityState.Added;  //=> SE CAMBIA EL ESTADO DEL OBJETO CREADO COMO REFERENCIA.

                       //--------------------------------------------------------------------------------------------------------
                        //SE GUARDAN LOS CAMBIOS REALIZADOS SOBRE LA BASE DE DATOS
                        await this._context.SaveChangesAsync();
                        //SE CULMINA LA TRANSACCION CON LA BASE DE DATOS
                        await transaction.CommitAsync();
                    }

                    //SE RETORNA EL CODIGO 200 OK JUNTO CON LA LISTA DE USUARIOS QUE COINCIDEN
                    return Ok(result);
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

        //===============================================================================================
        //===============================================================================================
        // GET: mttoapp/queryadmin/ficha
        // GET: mttoapp/queryadmin/numeroficha
        //SE ADICIONA EL ROUTING "HttpPost" LO CUAL INDICARA QUE LA FUNCION "QueryFicha" RESPONDERA A
        //A SOLICITUDES HTTP DE TIPO POST
        [HttpPost]

        //SE ADICIONA EL ROUTING "Route" JUNTO A DIRECCION A ADICIONAR PARA REALIZAR EL LLAMADO A ESTA 
        //FUNCION MEDIANTE UNA SOLICITUD HTTP. EJ:
        //https://<ipaddress>:<port>/mttoapp/queryadmin/ficha <=> https://192.168.1.192:8000/mttoapp/queryadmin/ficha
        //https://<ipaddress>:<port>/mttoapp/queryadmin/numeroficha <=> https://192.168.1.192:8000/mttoapp/queryadmin/numeroficha
        [Route("ficha")]
        [Route("numeroficha")]

        //--------------------------------------------------------------------------------------------------
        //FUNCION QUE RETORNARA UNA LISTA DE USUARIOS, LOS CUALES DEBEN CUMPLIR CON EL PARAMETRO DE BUSQUEDA
        //ENVIADO. EL LLAMADO SE HACE DESDE LA PAGINA "PaginaQueryAdmin" DE LA APLICACION "Mtto App".
        //EN ESTA FUNCION SE RECIBEN LOS PARAMETROS: 
        // -request:  OBJETO DEL TIPO "RequestQueryAdmin" EL CUAL CONTENDRA EL PARAMETRO ENVIADO Y EL NUMERO
        // DE OPCION DE BUSQUEDA (0 => Consulta por cedula; 1=> Consulta por Ficha; 2=> Consulta por Nombr(s)
        // 3=> Consulta por Apellido(s); 4=> Consulta por Username)
        //--------------------------------------------------------------------------------------------------
        public async Task<ActionResult<List<QueryAdmin>>> QueryFicha([FromBody] RequestQueryAdmin request)
        {
            //CREACION E INICIALIZACION DE VARIABLES
            List<QueryAdmin> result = new List<QueryAdmin>();

            //SE LISTAN TODOS LOS REGISTROS EN LA TABLA PERSONAS
            List<Personas> registros = await this._context.Personas.ToListAsync();

            //SE RECORRE CADA UNO DE LOS REGISTROS ("Personas")
            foreach (Personas x in registros)
            {
                //SE EVALUAN TODOS LOS REGISTROS MENOS EL REGISTRO DEL USUARIO ADMINISTRATOR
                if (x.NumeroFicha != 0)
                {
                    //SE EVALUA QUE EL DATO ENVIADO SE ENCUENTRE DENTRO DE LA LISTA REGISTROS.
                    /*------------------------------------------------------------------------
                     NOTA: LA EVALUACION SE HACE TOMANDO EN CUENTA EL TAMAÑO (CANTIDAD DE
                     CARACTERES) DEL PARAMETRO ENVIADO. PUESTO QUE SE CONSIDERA QUE NO SIEMPRE
                     SE ENVIARA TODO EL NUMERO DE FICHA COMPLETO SE HACE UNA BUSQUEDA DE LOS
                     REGISTROS EXISTENTES ENVIANDO EL NUMERO DE FICHA COMPLETO O PARCIAL
                     (TODO DEPENDE DE COMO SEA ENVIADO EL PARAMETRO) Y SE RETORNARAN EL
                     REGISTRO O LOS REGISTROS QUE COINCIDAN
                     ------------------------------------------------------------------------*/
                    if (request.Parametro == x.NumeroFicha.ToString().Substring(0, request.Parametro.Length))
                    {
                        //SE AGREGA A LA LISTA EL REGISTRO DE DICHA PERSONA
                        result?.Add(QueryAdmin.NewQueryAdmin(x));
                    }
                }
            }

            //SE EVALUA CUANTOS ELEMENTOS HAY EN LA LISTA "result"
            if (result.Count == 0)
                //CERO ELEMTENTOS: SE RETORNA EL CORIGO DE ESTATUS 404 NOTFOUND (NO HAY REGISTROS QUE CUMPLAN CON EL PARAMETRO ENVIADO)
                return NotFound(NotFoundMessage);
            else
            {
                //INICIAMOS EL CICLO TRY... CATCH
                try 
                {
                    //DIFERENTE DE CERO: SE RETORNA EL CODIGO DE ESTATUS 200 OK JUNTO CON LA LISTA DE USUARIOS OBTENIDOS
                    using (var transaction = this._context.Database.BeginTransaction())
                    {
                        //--------------------------------------------------------------------------------------------------------
                        //SE CREA E INICIALIZA UN OBJETO DEL TIPO "HistorialSolicitudesWeb" CON LA INFORMACION DEL NUEVO REGISTRO
                        //DE LA TABLA "HistorialSolicitudesWeb".
                        Historialsolicitudesweb solicitudesweb =
                            Historialsolicitudesweb.NewHistorialSolocitudesWeb(request.UserId, 7);

                        //--------------------------------------------------------------------------------------------------------
                        //SE ALMACENA EL REGISTRO DENTRO DE LA BASE DE DATOS
                        this._context.Historialsolicitudesweb.Add(solicitudesweb);      //=> SE CREA LA INFORMACION DE UN NUEVO REGISTRO EN LA TABLA HistorialSolicitudesWeb.
                        this._context.Entry(solicitudesweb).State = EntityState.Added;  //=> SE CAMBIA EL ESTADO DEL OBJETO CREADO COMO REFERENCIA.

                        //--------------------------------------------------------------------------------------------------------
                        //SE GUARDAN LOS CAMBIOS REALIZADOS SOBRE LA BASE DE DATOS
                        await this._context.SaveChangesAsync();
                        //SE CULMINA LA TRANSACCION CON LA BASE DE DATOS
                        await transaction.CommitAsync();
                    }

                    return Ok(result);
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

        //===============================================================================================
        //===============================================================================================
        //SE ADICIONA EL ROUTING "HttpPost" LO CUAL INDICARA QUE LA FUNCION "QueryNombres" RESPONDERA A
        //A SOLICITUDES HTTP DE TIPO POST
        // POST: mttoapp/queryadmin/nombre
        // POST: mttoapp/queryadmin/nombres
        [HttpPost]

        //SE ADICIONA EL ROUTING "Route" JUNTO A DIRECCION A ADICIONAR PARA REALIZAR EL LLAMADO A ESTA 
        //FUNCION MEDIANTE UNA SOLICITUD HTTP. EJ:
        //https://<ipaddress>:<port>/mttoapp/queryadmin/nombre <=> https://192.168.1.192:8000/mttoapp/queryadmin/nombre
        //https://<ipaddress>:<port>/mttoapp/queryadmin/nombres <=> https://192.168.1.192:8000/mttoapp/queryadmin/nombres
        [Route("nombre")]
        [Route("nombres")]

        //--------------------------------------------------------------------------------------------------
        //FUNCION QUE RETORNARA UNA LISTA DE USUARIOS, LOS CUALES DEBEN CUMPLIR CON EL PARAMETRO DE BUSQUEDA
        //ENVIADO. EL LLAMADO SE HACE DESDE LA PAGINA "PaginaQueryAdmin" DE LA APLICACION "Mtto App".
        //EN ESTA FUNCION SE RECIBEN LOS PARAMETROS: 
        // -request:  OBJETO DEL TIPO "RequestQueryAdmin" EL CUAL CONTENDRA EL PARAMETRO ENVIADO Y EL NUMERO
        // DE OPCION DE BUSQUEDA (0 => Consulta por cedula; 1=> Consulta por Ficha; 2=> Consulta por Nombr(s)
        // 3=> Consulta por Apellido(s); 4=> Consulta por Username)
        //--------------------------------------------------------------------------------------------------
        public async Task<ActionResult<List<QueryAdmin>>> QueryNombres([FromBody] RequestQueryAdmin request)
        {
            //CREACION E INICIALIZACION DE VARIABLES
            List<QueryAdmin> result = new List<QueryAdmin>();

            //SE LISTAN TODOS LOS REGISTROS EN LA TABLA PERSONAS
            List<Personas> registros = await this._context.Personas.ToListAsync();

            //SE RECORRE CADA UNO DE LOS REGISTROS ("Personas")
            foreach (Personas x in registros)
            {
                //SE EVALUAN TODOS LOS REGISTROS MENOS EL REGISTRO DEL USUARIO ADMINISTRATOR
                if (x.Cedula != 0)
                {
                    //SE EVALUA QUE EL DATO ENVIADO SE ENCUENTRE DENTRO DE LA LISTA REGISTROS.
                    /*------------------------------------------------------------------------
                     NOTA: LA EVALUACION SE HACE TOMANDO EN CUENTA EL TAMAÑO (CANTIDAD DE
                     CARACTERES) DEL PARAMETRO ENVIADO. PUESTO QUE SE CONSIDERA QUE NO SIEMPRE
                     SE ENVIARA TODO EL NOMBRE COMPLETO SE HACE UNA BUSQUEDA DE LOS
                     REGISTROS EXISTENTES ENVIANDO EL NOMBRE COMPLETO O PARCIAL
                     (TODO DEPENDE DE COMO SEA ENVIADO EL PARAMETRO) Y SE RETORNARAN EL
                     REGISTRO O LOS REGISTROS QUE COINCIDAN
                     ------------------------------------------------------------------------*/
                    if (request.Parametro.ToLower() == x.Nombres.Substring(0, request.Parametro.Length).ToLower())
                    {
                        //SE AGREGA A LA LISTA EL REGISTRO DE DICHA PERSONA
                        result?.Add(QueryAdmin.NewQueryAdmin(x));
                    }
                }
            }

            //SE EVALUA CUANTOS ELEMENTOS HAY EN LA LISTA "result"
            if (result.Count == 0)
                //CERO ELEMTENTOS: SE RETORNA EL CORIGO DE ESTATUS 404 NOTFOUND (NO HAY REGISTROS QUE CUMPLAN CON EL PARAMETRO ENVIADO)
                return NotFound(NotFoundMessage);
            else
            {
                //INICIAMOS EL CICLO TRY... CATCH
                try
                {
                    //DIFERENTE DE CERO: SE RETORNA EL CODIGO DE ESTATUS 200 OK JUNTO CON LA LISTA DE USUARIOS OBTENIDOS
                    using (var transaction = this._context.Database.BeginTransaction())
                    {
                        //--------------------------------------------------------------------------------------------------------
                        //SE CREA E INICIALIZA UN OBJETO DEL TIPO "HistorialSolicitudesWeb" CON LA INFORMACION DEL NUEVO REGISTRO
                        //DE LA TABLA "HistorialSolicitudesWeb".
                        Historialsolicitudesweb solicitudesweb =
                            Historialsolicitudesweb.NewHistorialSolocitudesWeb(request.UserId, 8);

                        //--------------------------------------------------------------------------------------------------------
                        //SE ALMACENA EL REGISTRO DENTRO DE LA BASE DE DATOS
                        this._context.Historialsolicitudesweb.Add(solicitudesweb);      //=> SE CREA LA INFORMACION DE UN NUEVO REGISTRO EN LA TABLA HistorialSolicitudesWeb.
                        this._context.Entry(solicitudesweb).State = EntityState.Added;  //=> SE CAMBIA EL ESTADO DEL OBJETO CREADO COMO REFERENCIA.

                        //--------------------------------------------------------------------------------------------------------
                        //SE GUARDAN LOS CAMBIOS REALIZADOS SOBRE LA BASE DE DATOS
                        await this._context.SaveChangesAsync();
                        //SE CULMINA LA TRANSACCION CON LA BASE DE DATOS
                        await transaction.CommitAsync();
                    }

                    return Ok(result);
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

        //===============================================================================================
        //===============================================================================================
        //SE ADICIONA EL ROUTING "HttpPost" LO CUAL INDICARA QUE LA FUNCION "QueryApellidos" RESPONDERA A
        //A SOLICITUDES HTTP DE TIPO POST
        // POST: mttoapp/queryadmin/apellidos
        // POST: mttoapp/queryadmin/apellido
        [HttpPost]

        //SE ADICIONA EL ROUTING "Route" JUNTO A DIRECCION A ADICIONAR PARA REALIZAR EL LLAMADO A ESTA 
        //FUNCION MEDIANTE UNA SOLICITUD HTTP. EJ:
        //https://<ipaddress>:<port>/mttoapp/queryadmin/apellido <=> https://192.168.1.192:8000/mttoapp/queryadmin/apellido
        //https://<ipaddress>:<port>/mttoapp/queryadmin/apellidos <=> https://192.168.1.192:8000/mttoapp/queryadmin/apellidos
        [Route("apellidos")]
        [Route("apellido")]

        //--------------------------------------------------------------------------------------------------
        //FUNCION QUE RETORNARA UNA LISTA DE USUARIOS, LOS CUALES DEBEN CUMPLIR CON EL PARAMETRO DE BUSQUEDA
        //ENVIADO. EL LLAMADO SE HACE DESDE LA PAGINA "PaginaQueryAdmin" DE LA APLICACION "Mtto App".
        //EN ESTA FUNCION SE RECIBEN LOS PARAMETROS: 
        // -request:  OBJETO DEL TIPO "RequestQueryAdmin" EL CUAL CONTENDRA EL PARAMETRO ENVIADO Y EL NUMERO
        // DE OPCION DE BUSQUEDA (0 => Consulta por cedula; 1=> Consulta por Ficha; 2=> Consulta por Nombr(s)
        // 3=> Consulta por Apellido(s); 4=> Consulta por Username)
        //--------------------------------------------------------------------------------------------------
        public async Task<ActionResult<List<QueryAdmin>>> QueryApellidos([FromBody] RequestQueryAdmin request)
        {
            //CREACION E INICIALIZACION DE VARIABLES
            List<QueryAdmin> result = new List<QueryAdmin>();

            //SE LISTAN TODOS LOS REGISTROS EN LA TABLA PERSONAS
            List<Personas> registros = await this._context.Personas.ToListAsync();

            //SE RECORRE CADA UNO DE LOS REGISTROS ("Personas")
            foreach (Personas x in registros)
            {
                //SE EVALUAN TODOS LOS REGISTROS MENOS EL REGISTRO DEL USUARIO ADMINISTRATOR
                if (x.Cedula != 0)
                {
                    //SE EVALUA QUE EL DATO ENVIADO SE ENCUENTRE DENTRO DE LA LISTA REGISTROS.
                    /*------------------------------------------------------------------------
                     NOTA: LA EVALUACION SE HACE TOMANDO EN CUENTA EL TAMAÑO (CANTIDAD DE
                     CARACTERES) DEL PARAMETRO ENVIADO. PUESTO QUE SE CONSIDERA QUE NO SIEMPRE
                     SE ENVIARA TODO EL APELLIDO COMPLETO SE HACE UNA BUSQUEDA DE LOS
                     REGISTROS EXISTENTES ENVIANDO EL APELLIDO COMPLETO O PARCIAL
                     (TODO DEPENDE DE COMO SEA ENVIADO EL PARAMETRO) Y SE RETORNARAN EL
                     REGISTRO O LOS REGISTROS QUE COINCIDAN
                     ------------------------------------------------------------------------*/
                    if (request.Parametro.ToLower() == x.Apellidos.Substring(0, request.Parametro.Length).ToLower())
                    {
                        //SE AGREGA A LA LISTA EL REGISTRO DE DICHA PERSONA
                        result?.Add(QueryAdmin.NewQueryAdmin(x));
                    }
                }
            }

            //SE EVALUA CUANTOS ELEMENTOS HAY EN LA LISTA "result"
            if (result.Count == 0)
                //CERO ELEMTENTOS: SE RETORNA EL CORIGO DE ESTATUS 404 NOTFOUND (NO HAY REGISTROS QUE CUMPLAN CON EL PARAMETRO ENVIADO)
                return NotFound(NotFoundMessage);
            else
            {
                //INICIAMOS EL CICLO TRY... CATCH
                try
                {
                    //DIFERENTE DE CERO: SE RETORNA EL CODIGO DE ESTATUS 200 OK JUNTO CON LA LISTA DE USUARIOS OBTENIDOS
                    using (var transaction = this._context.Database.BeginTransaction())
                    {
                        //--------------------------------------------------------------------------------------------------------
                        //SE CREA E INICIALIZA UN OBJETO DEL TIPO "HistorialSolicitudesWeb" CON LA INFORMACION DEL NUEVO REGISTRO
                        //DE LA TABLA "HistorialSolicitudesWeb".
                        Historialsolicitudesweb solicitudesweb =
                            Historialsolicitudesweb.NewHistorialSolocitudesWeb(request.UserId, 9);

                        //--------------------------------------------------------------------------------------------------------
                        //SE ALMACENA EL REGISTRO DENTRO DE LA BASE DE DATOS
                        this._context.Historialsolicitudesweb.Add(solicitudesweb);      //=> SE CREA LA INFORMACION DE UN NUEVO REGISTRO EN LA TABLA HistorialSolicitudesWeb.
                        this._context.Entry(solicitudesweb).State = EntityState.Added;  //=> SE CAMBIA EL ESTADO DEL OBJETO CREADO COMO REFERENCIA.

                        //--------------------------------------------------------------------------------------------------------
                        //SE GUARDAN LOS CAMBIOS REALIZADOS SOBRE LA BASE DE DATOS
                        await this._context.SaveChangesAsync();
                        //SE CULMINA LA TRANSACCION CON LA BASE DE DATOS
                        await transaction.CommitAsync();
                    }

                    return Ok(result);
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

        //===============================================================================================
        //===============================================================================================
        //SE ADICIONA EL ROUTING "HttpPost" LO CUAL INDICARA QUE LA FUNCION "QueryUsername" RESPONDERA A
        //A SOLICITUDES HTTP DE TIPO POST
        // POST: mttoapp/queryadmin/username
        [HttpPost]

        //SE ADICIONA EL ROUTING "Route" JUNTO A DIRECCION A ADICIONAR PARA REALIZAR EL LLAMADO A ESTA 
        //FUNCION MEDIANTE UNA SOLICITUD HTTP. EJ:
        //https://<ipaddress>:<port>/mttoapp/queryadmin/username <=> https://192.168.1.192:8000/mttoapp/queryadmin/username
        [Route("username")]

        //--------------------------------------------------------------------------------------------------
        //FUNCION QUE RETORNARA UNA LISTA DE USUARIOS, LOS CUALES DEBEN CUMPLIR CON EL PARAMETRO DE BUSQUEDA
        //ENVIADO. EL LLAMADO SE HACE DESDE LA PAGINA "PaginaQueryAdmin" DE LA APLICACION "Mtto App".
        //EN ESTA FUNCION SE RECIBEN LOS PARAMETROS: 
        // -request:  OBJETO DEL TIPO "RequestQueryAdmin" EL CUAL CONTENDRA EL PARAMETRO ENVIADO Y EL NUMERO
        // DE OPCION DE BUSQUEDA (0 => Consulta por cedula; 1=> Consulta por Ficha; 2=> Consulta por Nombr(s)
        // 3=> Consulta por Apellido(s); 4=> Consulta por Username)
        //--------------------------------------------------------------------------------------------------
        public async Task<ActionResult<List<QueryAdmin>>> QueryUsername([FromBody] RequestQueryAdmin request)
        {
            //CREACION E INICIALIZACION DE VARIABLES
            QueryAdmin query = new QueryAdmin();
            List<QueryAdmin> result = new List<QueryAdmin>();

            //SE LISTAN TODOS LOS REGISTROS EN LA TABLA USUARIOS
            List<Usuarios> registros = await this._context.Usuarios.ToListAsync();

            //SE RECORRE CADA UNO DE LOS REGISTROS ("Personas")
            foreach (Usuarios x in registros)
            {
                //SE EVALUAN TODOS LOS REGISTROS MENOS EL REGISTRO DEL USUARIO ADMINISTRATOR
                if (x.Cedula != 0   &&                              //=> ID (CEDULA) DEL REGISTRO EVALUADO DIFERENTE AL ID DEL ADMINISTRATOR
                    request.Parametro.Length <= x.Username.Length)  //=> EL LARGO DEL PARAMETRO DEBE SER MENOR AL LARGO DEL USERNAME DEL REGISTRO EVALUADO
                {
                    //SE EVALUA QUE EL DATO ENVIADO SE ENCUENTRE DENTRO DE LA LISTA REGISTROS.
                    /*------------------------------------------------------------------------
                     NOTA: LA EVALUACION SE HACE TOMANDO EN CUENTA EL TAMAÑO (CANTIDAD DE
                     CARACTERES) DEL PARAMETRO ENVIADO. PUESTO QUE SE CONSIDERA QUE NO SIEMPRE
                     SE ENVIARA TODO EL NOMBRE DE USUARIO COMPLETO SE HACE UNA BUSQUEDA DE LOS
                     REGISTROS EXISTENTES ENVIANDO EL NOMBRE DE USUARIO COMPLETO O PARCIAL
                     (TODO DEPENDE DE COMO SEA ENVIADO EL PARAMETRO) Y SE RETORNARAN EL
                     REGISTRO O LOS REGISTROS QUE COINCIDAN
                     ------------------------------------------------------------------------*/

                    if (request.Parametro.ToLower() == x.Username.Substring(0, request.Parametro.Length).ToLower())
                    {
                        //SE BUSCA EL REGISTRO DENTRO DE LA TABLA PERSONAS QUE COMPARTA EL MISMO NUMERO DE CEDULA QUE EL USUARIO EVALUADO EN EL MOMENTO
                        var persona = await this._context.Personas.FindAsync(x.Cedula);

                        //SE EVALUA SI EL REGISTRO OBTENIDO ES DIFERENTE DE NULO
                        if (persona != null)
                        {
                            //SE DESECHA LA ENTIDAD RETENIDA POR LA CLASE CONTEXTO
                            this._context.Entry(persona).State = EntityState.Detached;
                        }

                        //SI LA COMPARACION COINCIDE SE PROCEDE CREAR UN OBJETO DEL TIPO "QueryAdmin" CON LA INFORMACION NECESARIA
                        query = QueryAdmin.NewQueryAdmin(persona);
                        //SE AGREGA A LA LISTA EL OBJETO "QueryAdmin" CREADO
                        result.Add(query);
                    }
                }
            }

            //SE EVALUA CUANTOS ELEMENTOS HAY EN LA LISTA "result"
            if (result.Count == 0)
                //CERO ELEMTENTOS: SE RETORNA EL CORIGO DE ESTATUS 404 NOTFOUND (NO HAY REGISTROS QUE CUMPLAN CON EL PARAMETRO ENVIADO)
                return NotFound(NotFoundMessage);
            else
            {
                //INICIAMOS EL CICLO TRY... CATCH
                try
                {
                    //DIFERENTE DE CERO: SE RETORNA EL CODIGO DE ESTATUS 200 OK JUNTO CON LA LISTA DE USUARIOS OBTENIDOS
                    using (var transaction = this._context.Database.BeginTransaction())
                    {
                        //--------------------------------------------------------------------------------------------------------
                        //SE CREA E INICIALIZA UN OBJETO DEL TIPO "HistorialSolicitudesWeb" CON LA INFORMACION DEL NUEVO REGISTRO
                        //DE LA TABLA "HistorialSolicitudesWeb".
                        Historialsolicitudesweb solicitudesweb =
                            Historialsolicitudesweb.NewHistorialSolocitudesWeb(request.UserId, 10);

                        //--------------------------------------------------------------------------------------------------------
                        //SE ALMACENA EL REGISTRO DENTRO DE LA BASE DE DATOS
                        this._context.Historialsolicitudesweb.Add(solicitudesweb);      //=> SE CREA LA INFORMACION DE UN NUEVO REGISTRO EN LA TABLA HistorialSolicitudesWeb.
                        this._context.Entry(solicitudesweb).State = EntityState.Added;  //=> SE CAMBIA EL ESTADO DEL OBJETO CREADO COMO REFERENCIA.

                        //--------------------------------------------------------------------------------------------------------
                        //SE GUARDAN LOS CAMBIOS REALIZADOS SOBRE LA BASE DE DATOS
                        await this._context.SaveChangesAsync();
                        //SE CULMINA LA TRANSACCION CON LA BASE DE DATOS
                        await transaction.CommitAsync();
                    }

                    return Ok(result);
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

        //===============================================================================================
        //===============================================================================================
        //SE ADICIONA EL ROUTING "HttpPost" LO CUAL INDICARA QUE LA FUNCION "GetUserSelectedInfo" RESPONDERA A
        //A SOLICITUDES HTTP DE TIPO POST
        [HttpPost]

        //SE ADICIONA EL ROUTING "Route" JUNTO A DIRECCION A ADICIONAR PARA REALIZAR EL LLAMADO A ESTA 
        //FUNCION MEDIANTE UNA SOLICITUD HTTP. EJ:
        //https://<ipaddress>:<port>/mttoapp/queryadmin/onuserselected <=> https://192.168.1.192:8000/mttoapp/queryadmin/onuserselected
        [Route("onuserselected")]

        //--------------------------------------------------------------------------------------------------
        //FUNCION QUE RETORNARA LA INFORMACION GENERAL DEL USUARIO SELECCIONADO EN LA LISTA DE USUARIOS
        //QUE COINCIDEN CON EL PARAMETRO DE BUSQUEDA Y LA OPCION SE CONSULTA SELECCIONADA
        //--------------------------------------------------------------------------------------------------
        public async Task<ActionResult<InformacionGeneral>> GetUserSelectedInfo([FromBody] UserSelectedRequest userselected)
        {
            //SE CREA E INICIALIZA LA VARIABLE QUE 
            var fullinfo = new InformacionGeneral();

            //SE VERIFICA QUE EL OBJETO DEL TIPO "UserSelectedRequest" ENVIADO NO SEA NULO O VACIO
            if (userselected == null)   //=> true => EL OBJETO ES NULO
            {
                //SE RETORNA LA RESPUESTA "BadRequest" JUNTO CON UN MENSAJE INFORMATIVO
                return BadRequest("Error, vuelva a intentarlo nuevamente");
            }
            else if(userselected != null)   //=> true => EL OBJETO NO ES NULO
            {
                //INICIAMOS EL CICLO TRY... CATCH
                try 
                {
                    //SE INICIA LA TRANSACCION DE DATA CON LA BASE DE DATOS
                    using (var transaction = this._context.Database.BeginTransaction())
                    {
                        //--------------------------------------------------------------------------------------------------------
                        //SE BUSCA EL REGISTRO DENTRO DE LA TABLA DE USUARIOS QUE COINDICA CON EL ID DEL OBJETO ENVIADO
                        fullinfo.Persona = await this._context.Personas.FindAsync(userselected.UserIdSelected);

                        //SE VERIFICA SI EL OBJETO QUE RECIBIO LA INFORMACION SE ENCUENTRA NULO O NO
                        if (fullinfo.Persona != null)
                        {
                            //SI NO SE ENCUENTRA NULO SE DESECHA AL OBJETO RETENIDO POR LA CLASE CONTEXTO
                            this._context.Entry(fullinfo.Persona).State = EntityState.Detached;
                        }

                        //--------------------------------------------------------------------------------------------------------
                        //SE REPITE EL MISMO PROCESO EN LA BUSQUEDA DEL REGISTRO DENTRO DE LA TABLA USUARIOS
                        fullinfo.Usuario = await this._context.Usuarios.FindAsync(userselected.UserIdSelected);

                        //SE VERIFICA SI EL OBJETO QUE RECIBIO LA INFORMACION SE ENCUENTRA NULO O NO
                        if (fullinfo.Usuario != null)
                        {
                            //SI NO SE ENCUENTRA NULO SE DESECHA AL OBJETO RETENIDO POR LA CLASE CONTEXTO
                            this._context.Entry(fullinfo.Usuario).State = EntityState.Detached;
                        }

                        //--------------------------------------------------------------------------------------------------------
                        //SE VUELVE A VERISICAR EL ESTADO DE LOS OBJTOS QUE CONTIENEN LA INFORMACION SOLICITADA DE LA BASE DE DATOS
                        if (fullinfo.Persona == null && fullinfo.Usuario == null)
                        {
                            //SE RETORNA LA RESPUESTA DE ESTATUS BAD REQUEST 400
                            return BadRequest("Error, vuelva a intentarlo nuevamente");
                        }

                        //--------------------------------------------------------------------------------------------------------
                        //SE CREA E INICIALIZA UN OBJETO DEL TIPO "HistorialSolicitudesWeb" CON LA INFORMACION DEL NUEVO REGISTRO
                        //DE LA TABLA "HistorialSolicitudesWeb".
                        Historialsolicitudesweb solicitudesweb =
                            Historialsolicitudesweb.NewHistorialSolocitudesWeb(userselected.UserIdRequested, 11);

                        //--------------------------------------------------------------------------------------------------------
                        //SE ALMACENA EL REGISTRO DENTRO DE LA BASE DE DATOS
                        this._context.Historialsolicitudesweb.Add(solicitudesweb);      //=> SE CREA LA INFORMACION DE UN NUEVO REGISTRO EN LA TABLA HistorialSolicitudesWeb.
                        this._context.Entry(solicitudesweb).State = EntityState.Added;  //=> SE CAMBIA EL ESTADO DEL OBJETO CREADO COMO REFERENCIA.

                        //--------------------------------------------------------------------------------------------------------
                        //SE GUARDAN LOS CAMBIOS REALIZADOS SOBRE LA BASE DE DATOS
                        await this._context.SaveChangesAsync();
                        //SE CULMINA LA TRANSACCION CON LA BASE DE DATOS
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
            }

            return Ok(fullinfo);
        }
    }
}