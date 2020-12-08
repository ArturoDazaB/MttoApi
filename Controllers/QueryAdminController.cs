using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

using MttoApi.Model;
using MttoApi.Model.Context;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MttoApi.Controllers
{
    [Route("mttoapp ")]
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
        [HttpGet]
        [Route("cedula")]
        [Route("id")]
        public async Task<ActionResult<List<QueryAdmin>>> GetCedula([FromBody] RequestQueryAdmin request)
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
        [HttpGet]
        [Route("ficha")]
        [Route("numeroficha")]
        public async Task<ActionResult<List<QueryAdmin>>> GetFicha([FromBody] RequestQueryAdmin request)
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

        // GET: mttoapp/queryadmin/nombre
        // GET: mttoapp/queryadmin/nombres
        [HttpGet]
        [Route("nombre")]
        [Route("nombres")]
        public async Task<ActionResult<List<QueryAdmin>>> GetNombres([FromBody] RequestQueryAdmin request)
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

        // GET: mttoapp/queryadmin/apellidos
        // GET: mttoapp/queryadmin/apellido
        [HttpGet]
        [Route("apellidos")]
        [Route("apellido")]
        public async Task<ActionResult<List<QueryAdmin>>> GetApellidos([FromBody] RequestQueryAdmin request)
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

        // GET: mttoapp/queryadmin/username
        [HttpGet]
        [Route("username")]
        public async Task<ActionResult<List<QueryAdmin>>> GetUsername([FromBody] RequestQueryAdmin request)
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
        


    }
}