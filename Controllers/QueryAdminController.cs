using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

using MttoApi.Model;
using MttoApi.Model.Context;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MttoApi.Controllers
{
    [Route("mttoapp/queryadmin")]
    [ApiController]
    public class QueryAdminController : ControllerBase
    {
        private readonly MTTOAPP_V6Context _context;

        public QueryAdminController(MTTOAPP_V6Context context)
        {
            this._context = context;
        }

        //===============================================================================================
        //===============================================================================================
        // GET: mttoapp/queryadmin/cedula
        // GET: mttoapp/queryadmin/id
        [HttpPost]
        [Route("cedula")]
        [Route("id")]
        public async Task<ActionResult<List<QueryAdmin>>> QueryCedula([FromBody] RequestQueryAdmin request)
        {
            //CREACION E INICIALIZACION DE VARIABLES
            QueryAdmin query = new QueryAdmin();
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
                     SE ENVIARA TODO EL NUMERO DE CEDULA COMPLETO SE HACE UNA BUSQUEDA DE LOS
                     REGISTROS EXISTENTES ENVIANDO EL NUMERO DE CEDULA COMPLETO O PARCIAL
                     (TODO DEPENDE DE COMO SEA ENVIADO EL PARAMETRO) Y SE RETORNARAN EL
                     REGISTRO O LOS REGISTROS QUE COINCIDAN
                     ------------------------------------------------------------------------*/
                    if (request.Parametro.ToString() == x.Cedula.ToString().Substring(0, request.Parametro.ToString().Length))
                    {
                        //SI LA COMPARACION COINCIDE SE PROCEDE CREAR UN OBJETO DEL TIPO "QueryAdmin" CON LA INFORMACION NECESARIA
                        query = QueryAdmin.NewQueryAdmin(x);
                        //SE AGREGA A LA LISTA EL OBJETO "QueryAdmin" CREADO
                        result?.Add(query);
                    }
                }
            }

            //SE EVALUA CUANTOS ELEMENTOS HAY EN LA LISTA "result"
            if (result.Count == 0)
                //CERO ELEMTENTOS: SE RETORNA EL CORIGO DE ESTATUS 404 NOTFOUND (NO HAY REGISTROS QUE CUMPLAN CON EL PARAMETRO ENVIADO)
                return NotFound();
            else
            {
                //DIFERENTE DE CERO: SE RETORNA EL CODIGO DE ESTATUS 200 OK JUNTO CON LA LISTA DE USUARIOS OBTENIDOS
                using(var transaction = this._context.Database.BeginTransaction())
                {
                    //--------------------------------------------------------------------------------------------------------
                    //SE CREA E INICIALIZA UN OBJETO DEL TIPO "HistorialSolicitudesWeb" CON LA INFORMACION DEL NUEVO REGISTRO
                    //DE LA TABLA "HistorialSolicitudesWeb".
                    Historialsolicitudesweb solicitudesweb =
                        Historialsolicitudesweb.NewHistorialSolocitudesWeb(request.UserId, 6);

                    //SE ALMACENA EL REGISTRO DENTRO DE LA BASE DE DATOS
                    this._context.Historialsolicitudesweb.Add(solicitudesweb);
                    this._context.Entry(solicitudesweb).State = EntityState.Added;
                    //-------------------------------------------------------------------------------------------------------
                    await this._context.SaveChangesAsync();
                    await transaction.CommitAsync();
                }

                return Ok(result);
            }
                
        }

        // GET: mttoapp/queryadmin/ficha
        // GET: mttoapp/queryadmin/numeroficha
        [HttpPost]
        [Route("ficha")]
        [Route("numeroficha")]
        public async Task<ActionResult<List<QueryAdmin>>> QueryFicha([FromBody] RequestQueryAdmin request)
        {
            //CREACION E INICIALIZACION DE VARIABLES
            QueryAdmin query = new QueryAdmin();
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
                        //SI LA COMPARACION COINCIDE SE PROCEDE CREAR UN OBJETO DEL TIPO "QueryAdmin" CON LA INFORMACION NECESARIA
                        query = QueryAdmin.NewQueryAdmin(x);
                        //SE AGREGA A LA LISTA EL OBJETO "QueryAdmin" CREADO
                        result?.Add(query);
                    }
                }
            }

            //SE EVALUA CUANTOS ELEMENTOS HAY EN LA LISTA "result"
            if (result.Count == 0)
                //CERO ELEMTENTOS: SE RETORNA EL CORIGO DE ESTATUS 404 NOTFOUND (NO HAY REGISTROS QUE CUMPLAN CON EL PARAMETRO ENVIADO)
                return NotFound();
            else
            {
                //DIFERENTE DE CERO: SE RETORNA EL CODIGO DE ESTATUS 200 OK JUNTO CON LA LISTA DE USUARIOS OBTENIDOS
                using(var transaction = this._context.Database.BeginTransaction())
                {
                    //--------------------------------------------------------------------------------------------------------
                    //SE CREA E INICIALIZA UN OBJETO DEL TIPO "HistorialSolicitudesWeb" CON LA INFORMACION DEL NUEVO REGISTRO
                    //DE LA TABLA "HistorialSolicitudesWeb".
                    Historialsolicitudesweb solicitudesweb =
                        Historialsolicitudesweb.NewHistorialSolocitudesWeb(request.UserId, 7);

                    //SE ALMACENA EL REGISTRO DENTRO DE LA BASE DE DATOS
                    this._context.Historialsolicitudesweb.Add(solicitudesweb);
                    this._context.Entry(solicitudesweb).State = EntityState.Added;
                    //-------------------------------------------------------------------------------------------------------
                    await this._context.SaveChangesAsync();
                    await transaction.CommitAsync();
                }

                return Ok(result);
            }
            
            
        }

        // POST: mttoapp/queryadmin/nombre
        // POST: mttoapp/queryadmin/nombres
        [HttpPost]
        [Route("nombre")]
        [Route("nombres")]
        public async Task<ActionResult<List<QueryAdmin>>> QueryNombres([FromBody] RequestQueryAdmin request)
        {
            //CREACION E INICIALIZACION DE VARIABLES
            QueryAdmin query = new QueryAdmin();
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
                        //SI LA COMPARACION COINCIDE SE PROCEDE CREAR UN OBJETO DEL TIPO "QueryAdmin" CON LA INFORMACION NECESARIA
                        query = QueryAdmin.NewQueryAdmin(x);
                        //SE AGREGA A LA LISTA EL OBJETO "QueryAdmin" CREADO
                        result.Add(query);
                    }
                }
            }

            //SE EVALUA CUANTOS ELEMENTOS HAY EN LA LISTA "result"
            if (result.Count == 0)
                //CERO ELEMTENTOS: SE RETORNA EL CORIGO DE ESTATUS 404 NOTFOUND (NO HAY REGISTROS QUE CUMPLAN CON EL PARAMETRO ENVIADO)
                return NotFound();
            else
            {
                //DIFERENTE DE CERO: SE RETORNA EL CODIGO DE ESTATUS 200 OK JUNTO CON LA LISTA DE USUARIOS OBTENIDOS
                using (var transaction = this._context.Database.BeginTransaction())
                {
                    //--------------------------------------------------------------------------------------------------------
                    //SE CREA E INICIALIZA UN OBJETO DEL TIPO "HistorialSolicitudesWeb" CON LA INFORMACION DEL NUEVO REGISTRO
                    //DE LA TABLA "HistorialSolicitudesWeb".
                    Historialsolicitudesweb solicitudesweb =
                        Historialsolicitudesweb.NewHistorialSolocitudesWeb(request.UserId, 8);

                    //SE ALMACENA EL REGISTRO DENTRO DE LA BASE DE DATOS
                    this._context.Historialsolicitudesweb.Add(solicitudesweb);
                    this._context.Entry(solicitudesweb).State = EntityState.Added;
                    //-------------------------------------------------------------------------------------------------------
                    await this._context.SaveChangesAsync();
                    await transaction.CommitAsync();
                }
                return Ok(result);
            }
                
        }

        // POST: mttoapp/queryadmin/apellidos
        // POST: mttoapp/queryadmin/apellido
        [HttpPost]
        [Route("apellidos")]
        [Route("apellido")]
        public async Task<ActionResult<List<QueryAdmin>>> QueryApellidos([FromBody] RequestQueryAdmin request)
        {
            //CREACION E INICIALIZACION DE VARIABLES
            QueryAdmin query = new QueryAdmin();
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
                        //SI LA COMPARACION COINCIDE SE PROCEDE CREAR UN OBJETO DEL TIPO "QueryAdmin" CON LA INFORMACION NECESARIA
                        query = QueryAdmin.NewQueryAdmin(x);
                        //SE AGREGA A LA LISTA EL OBJETO "QueryAdmin" CREADO
                        result.Add(query);
                    }
                }
            }

            //SE EVALUA CUANTOS ELEMENTOS HAY EN LA LISTA "result"
            if (result.Count == 0)
                //CERO ELEMTENTOS: SE RETORNA EL CORIGO DE ESTATUS 404 NOTFOUND (NO HAY REGISTROS QUE CUMPLAN CON EL PARAMETRO ENVIADO)
                return NotFound();
            else
            {
                using(var transaction = this._context.Database.BeginTransaction())
                {
                    //DIFERENTE DE CERO: SE RETORNA EL CODIGO DE ESTATUS 200 OK JUNTO CON LA LISTA DE USUARIOS OBTENIDOS
                    //--------------------------------------------------------------------------------------------------------
                    //SE CREA E INICIALIZA UN OBJETO DEL TIPO "HistorialSolicitudesWeb" CON LA INFORMACION DEL NUEVO REGISTRO
                    //DE LA TABLA "HistorialSolicitudesWeb".
                    Historialsolicitudesweb solicitudesweb =
                        Historialsolicitudesweb.NewHistorialSolocitudesWeb(request.UserId, 9);

                    //SE ALMACENA EL REGISTRO DENTRO DE LA BASE DE DATOS
                    this._context.Historialsolicitudesweb.Add(solicitudesweb);
                    this._context.Entry(solicitudesweb).State = EntityState.Added;
                    //-------------------------------------------------------------------------------------------------------
                    await this._context.SaveChangesAsync();
                    await transaction.CommitAsync();
                }
                return Ok(result);
            }
                
        }

        // POST: mttoapp/queryadmin/username
        [HttpPost]
        [Route("username")]
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
                if (x.Cedula != 0   &&
                    request.Parametro.Length <= x.Username.Length)
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

                        if (persona != null)
                            //SE DESECHA LA ENTIDAD RETENIDA POR LA CLASE CONTEXTO
                            this._context.Entry(persona).State = EntityState.Detached;

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
                return NotFound();
            else
            {
                using(var transaction = this._context.Database.BeginTransaction())
                {
                    //DIFERENTE DE CERO: SE RETORNA EL CODIGO DE ESTATUS 200 OK JUNTO CON LA LISTA DE USUARIOS OBTENIDOS
                    //--------------------------------------------------------------------------------------------------------
                    //SE CREA E INICIALIZA UN OBJETO DEL TIPO "HistorialSolicitudesWeb" CON LA INFORMACION DEL NUEVO REGISTRO
                    //DE LA TABLA "HistorialSolicitudesWeb".
                    Historialsolicitudesweb solicitudesweb =
                        Historialsolicitudesweb.NewHistorialSolocitudesWeb(request.UserId, 10);

                    //SE ALMACENA EL REGISTRO DENTRO DE LA BASE DE DATOS
                    this._context.Historialsolicitudesweb.Add(solicitudesweb);
                    this._context.Entry(solicitudesweb).State = EntityState.Added;

                    await this._context.SaveChangesAsync();
                    //-------------------------------------------------------------------------------------------------------
                    await transaction.CommitAsync();
                }
                return Ok(result);
            }
                
        }

        //===============================================================================================
        //===============================================================================================
        [HttpPost]
        [Route("onuserselected")]
        public async Task<ActionResult<InformacionGeneral>> GetUserSelectedInfo([FromBody] UserSelectedRequest userselected)
        {
            //SE CREA E INICIALIZA LA VARIABLE QUE 
            var fullinfo = new InformacionGeneral();

            //SE VERIFICA QUE EL OBJETO DEL TIPO "QueryAdmin" ENVIADO NO SEA NULO O VACIO
            if (userselected == null)
            {
                return BadRequest("Error, vuelva a intentarlo nuevamente");
            }
            else if(userselected != null)
            {
                //SE INICIA LA TRANSACCION DE DATA CON LA BASE DE DATOS
                using (var transaction = this._context.Database.BeginTransaction())
                {
                    //SE BUSCA EL REGISTRO DENTRO DE LA TABLA DE USUARIOS QUE COINDICA CON EL ID DEL OBJETO ENVIADO
                    fullinfo.Persona = await this._context.Personas.FindAsync(userselected.UserIdSelected);
                    //SE VERIFICA SI EL OBJETO QUE RECIBIO LA INFORMACION SE ENCUENTRA NULO O NO
                    if (fullinfo.Persona != null)
                        //SI NO SE ENCUENTRA NULO SE DESECHA AL OBJETO RETENIDO POR LA CLASE CONTEXTO
                        this._context.Entry(fullinfo.Persona).State = EntityState.Detached;

                    //SE REPITE EL MISMO PROCESO EN LA BUSQUEDA DEL REGISTRO DENTRO DE LA TABLA USUARIOS
                    fullinfo.Usuario = await this._context.Usuarios.FindAsync(userselected.UserIdSelected);
                    if (fullinfo.Usuario != null)
                        this._context.Entry(fullinfo.Usuario).State = EntityState.Detached;

                    //SE VUELVE A VERISICAR EL ESTADO DE LOS OBJTOS QUE CONTIENEN LA INFORMACION SOLICITADA DE LA BASE DE DATOS
                    if (fullinfo.Persona == null && fullinfo.Usuario == null)
                    {
                        //SE DETIENE LA TRANSACCION DE DATOS CON LA BASE DE DATOS
                        await transaction.CommitAsync();
                        //SE RETORNA LA RESPUESTA DE ESTATUS BAD REQUEST 400
                        return BadRequest("Error, vuelva a intentarlo nuevamente");
                    }
                        
                    //--------------------------------------------------------------------------------------------------------
                    //SE CREA E INICIALIZA UN OBJETO DEL TIPO "HistorialSolicitudesWeb" CON LA INFORMACION DEL NUEVO REGISTRO
                    //DE LA TABLA "HistorialSolicitudesWeb".
                    Historialsolicitudesweb solicitudesweb =
                        Historialsolicitudesweb.NewHistorialSolocitudesWeb(userselected.UserIdRequested, 11);

                    //SE ALMACENA EL REGISTRO DENTRO DE LA BASE DE DATOS
                    this._context.Historialsolicitudesweb.Add(solicitudesweb);
                    this._context.Entry(solicitudesweb).State = EntityState.Added;

                    await this._context.SaveChangesAsync();
                    //-------------------------------------------------------------------------------------------------------
                    await transaction.CommitAsync();
                }
            }

            return Ok(fullinfo);
        }
    }
}