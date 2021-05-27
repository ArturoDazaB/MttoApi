using System;

namespace MttoApi.Model
{
    //-----------------------------------------------------------------------------------
    //--------------------------TABLAS DE LA BASE DE DATOS-------------------------------
    //===================================================================================
    //===================================================================================
    //EN ESTE ARCHIVO SE ENCUENTRAN LAS CLASES DESTINADAS A REPRESENTAR EN FORMA DE
    //OBJETO CADA UNA DE LAS TABLAS DE LA BASE DE DATOS.

    //=======================================================================================================
    //=======================================================================================================
    public partial class Historialconsultatableros
    {
        //ATRIBUTOS (PROPIEDADES) DE LA TABLA "HistorialConsultaTableros"
        public int Id { get; set; }
        public string TableroId { get; set; }
        public string TipoDeConsulta { get; set; }
        public double UsuarioId { get; set; }
        public DateTime FechaConsulta { get; set; }

        //===================================================================================
        //===================================================================================
        //FUNCION DE LA CLASE "Historalconsultatableros" QUE RECIBE CADA UNO DE LOS PARAMETROS
        //LOS ASIGNA Y RETORNA UN OBJETO DE TIPO "Historialconsultatableros".
        public static Historialconsultatableros NewHistorialConsultaTableros(string tableroid, string tipodeconsulta, double usuarioid)
        {
            return new Historialconsultatableros()
            {
                TableroId = tableroid,
                TipoDeConsulta = tipodeconsulta,
                UsuarioId = usuarioid,
                FechaConsulta = DateTime.Now,
            };
        }
    }

    //=======================================================================================================
    //=======================================================================================================
    public partial class Historialsolicitudesweb
    {
        //ATRIBUTOS (PROPIEDADES) DE LA TABLA "Historialsolicitudesweb"
        public int Id { get; set; }
        public double UsuarioId { get; set; }
        public DateTime FechaHora { get; set; }
        public string Solicitud { get; set; }

        //===================================================================================
        //===================================================================================
        //FUNCION QUE RECIBE COMO PARAMETROS EL USUARIO QUE ACABA DE REALIZAR LA SOLICITUD 
        //HTTP DESDE EL APLICATIVO Y EL NUMERO DE SOLICITUD QUE EJECUTO
        public static Historialsolicitudesweb NewHistorialSolocitudesWeb(double userid, int solicitud)
        {
            //SE CREA E INICIALIZA LA VARIABLE SOLICITUD 
            //NOTA: VARIABLE QUE CONTENDRA LA CADENA DE TEXTO CON INFORMACION SIGNIFICATIVA DE
            //SOLICITUD HECHA: Controlador = <nombre del controlador ejecutado>
            //                 SolicitudHttp = <Tipo de solicitud RESTFul ejecutada>
            //                 Metodo = <Nombre del metodo/funcion ejecutada>
            //                 RespuestaHttp = <Tipo de respuesta que retorna la solicitud hecha>
            string _solicitud = string.Empty;

            //SE EVALUA EL NUMERO DE SOLICITUD HTTP EJECUTADA PARA RETORNAR UN TIPO DE CADENA DE TEXTO
            switch (solicitud)
            {
                //--------------------------------------------------------------------------------------------------------------------------------------------------
                //CONTROLADOR = "Log In Controller"
                //REQUEST = HTTP GET mttoapp/login?username=<username>&password=<password>
                case 0:
                    _solicitud = "Controlador=LogInController;SolicitudHTTP=HTTPGET;Metodo=Get;RespuestaHTTP=Ok(<LogInResponse> Object)";
                    break;
                //--------------------------------------------------------------------------------------------------------------------------------------------------
                //CONTROLADOR = "Consulta Tableros Controller"
                case 1:
                    break;
                //--------------------------------------------------------------------------------------------------------------------------------------------------
                //CONTROLADOR = "Consulta Tableros Controller"
                //REQUEST = HTTP GET mttopp/consultatableros/consultatableroid
                //------------------------------------------------------------
                //               GET mttoapp/consultatableros/consultasapid
                case 2:
                    _solicitud = "Controlador=ConsultaTablerosController;SolicitudHTTP=HTTPGET;Metodo=ConsultaTableroId;RespuestaHTTP=Ok(<RegistroTablero> Object)";
                    break;
                case 3:
                    _solicitud = "Controlador=ConsultaTablerosController;SolicitudHTTP=HTTPGET;Metodo=ConsultaSapId;RespuestaHTTP=Ok(<RegistroTablero> Object)";
                    break;
                //--------------------------------------------------------------------------------------------------------------------------------------------------
                //CONTROLADOR = "Registro Tableros Controller"
                //REQUEST = HTTP POST mttopp/registrotableros
                case 4:
                    _solicitud = "Controlador=RegistroTablerosController;SolicitudHTTP=HTTPPOST;Metodo=NewTablero;RespuestaHTTP=Ok(<string> mensaje)";
                    break;
                //--------------------------------------------------------------------------------------------------------------------------------------------------
                //CONTROLADOR = "Registro Usuarios Controller"
                //REQUEST = HTTP POST mttopp/registrotableros
                case 5:
                    _solicitud = "Controlador=RegistroUsuariosController;SolicitudHTTP=HTTPPOST;Metodo=Post;RespuestaHTTP=Ok(<string> mensaje)";
                    break;
                //--------------------------------------------------------------------------------------------------------------------------------------------------
                //CONTROLADOR = "Query Admin Controller"
                //REQUEST POST mttoapp/getinfo/cedula
                //        POST mttoapp/getinfo/id
                //----------------------------------------------------
                //        POST mttoapp/getinfo/ficha
                //        POST mttoapp/getinfo/numeroficha
                //----------------------------------------------------
                //        POST mttoapp/getinfo/nombre
                //        POST mttoapp/getinfo/nombres
                //----------------------------------------------------
                //        POST mttoapp/getinfo/apellidos
                //        POST mttoapp/getinfo/apellido
                //----------------------------------------------------
                //        POST mttoapp/getinfo/username
                case 6:
                    _solicitud = "Controlador=QueryAdminController;SolicitudHTTP=HTTPPOST;Metodo=QueryCedula;RespuestaHTTP=Ok(<List<QueryAdmin>> ObjetoLista)";
                    break;
                case 7:
                    _solicitud = "Controlador=QueryAdminController;SolicitudHTTP=HTTPPOST;Metodo=QueryFicha;RespuestaHTTP=Ok(<List<QueryAdmin>> ObjetoLista)";
                    break;
                case 8:
                    _solicitud = "Controlador=QueryAdminController;SolicitudHTTP=HTTPPOST;Metodo=QueryNombre;RespuestaHTTP=Ok(<List<QueryAdmin>> ObjetoLista)";
                    break;
                case 9:
                    _solicitud = "Controlador=QueryAdminController;SolicitudHTTP=HTTPPOST;Metodo=QueryApellido;RespuestaHTTP=Ok(<List<QueryAdmin>> ObjetoLista)";
                    break;
                case 10:
                    _solicitud = "Controlador=QueryAdminController;SolicitudHTTP=HTTPPOST;Metodo=QueryUsername;RespuestaHTTP=Ok(<List<QueryAdmin>> ObjetoLista)";
                    break;
                case 11:
                    _solicitud = "Controlador=QueryAdminController;SolicitudHTTP=HTTPPOST;Metodo=GetUserSelectedInfo;RespuestaHTTP=Ok(<InformacionGeneral> Objeto)";
                    break;
                //--------------------------------------------------------------------------------------------------------------------------------------------------
                //CONTROLADOR = "Configuracion Controller"
                //REQUEST PUT mttoapp/configuracion/usuario/<cedula>
                //        PUT mttoapp/configuracion/usu/<cedula>
                //----------------------------------------------------
                //        PUT mttoapp/configuracion/administrator/<cedula>
                //        PUT mttoapp/configuracion/adm/<cedula>
                case 12:
                    _solicitud = "Controlador=ConfiguracionController;SolicitudHTTP=HTTPPUT;Metodo=ActualizarUsuario;RespuestaHTTP=Ok(<string> mensaje)";
                    break;
                case 13:
                    _solicitud = "Controlador=ConfiguracionController;SolicitudHTTP=HTTPPUT;Metodo=ActualizarUsuarioAdm;RespuestaHTTP=Ok(<string> mensaje)";
                    break;
            }

            //SE RETORNA UN OBJETO DE TIPO "Historialsolicitudesweb" CON TODA LA INFORMACION DE LA SOLICITUD HTTP EJECUTADA
            return new Historialsolicitudesweb()
            {
                UsuarioId = userid,         //=>ID DEL USUARIO QUE EJECUTO LA SOLITIUD
                FechaHora = DateTime.Now,   //=>FECHA Y HORA DE LA SOLICITUD
                Solicitud = _solicitud,     //=>INFORMACION SOBRE LA SOLICITUD
            };
        }

    }

    //=======================================================================================================
    //=======================================================================================================
    public partial class Items
    {
        //ATRIBUTOS (PROPIEDADES) DE LA TABLA "Items"
        //NOTA: ACTUALMENTE ESTA CLASE ES USADA PARA REPRESENTAR LOS ITEMS DENTRO DE LOS TABLEROS
        public int Id { get; set; }
        public string TableroId { get; set; }
        public string Descripcion { get; set; }
        public int Cantidad { get; set; }
    }

    //=======================================================================================================
    //=======================================================================================================
    public partial class Modificacionesusuario
    {
        //ATRIBUTOS (PROPIEDADES) DE LA TABLA "Modificacionesusuario"
        public int Id { get; set; }
        public double IdModificado { get; set; }
        public double IdModificador { get; set; }
        public DateTime FechaHora { get; set; }
        //NOTA: LAS PROPIEDADES QUE FIGUREN EL CARACTER '?' SON PROPIEDADES
        //QUE DENTRO DE LA BASE DE DATO PUEDEN SER NULAS
        public bool? ModificacionNombres { get; set; }
        public bool? ModificacionApellidos { get; set; }
        public bool? ModificacionFecha { get; set; }
        public bool? ModificacionUsername { get; set; }
        public bool? ModificacionCorreo { get; set; }
        public bool? ModificacionTelefono { get; set; }
        public bool? ModificacionPassword { get; set; }

        public bool? ModificacionNivelUsuario { get; set; }

        //===================================================================================
        //===================================================================================
        //CONSTRUCTOR
        public Modificacionesusuario()
        {
            //SE INICIALIZAN LAS PROPIEDADES QUE IDENFITICARAN LOS CAMBIOS HECHOS ANTES DE
            //REALIZAR LA MODIFICACION
            ModificacionNombres = ModificacionApellidos = ModificacionFecha = ModificacionUsername
                = ModificacionCorreo = ModificacionTelefono = ModificacionPassword = ModificacionNivelUsuario = false;
        }

        //===================================================================================
        //===================================================================================
        //FUNCION QUE RECIBE MEDIANTE PARAMETROS TODA LA INFORMACION PARA GENERAR
        //UN OBJETO DE TIPO "NewModificacionesUsuarios".
        public static Modificacionesusuario NewModificacionesUsuarios(Personas Persona,     //INFORMACION PERSONAL NUEVA A REGISTRAR
                                                                      Personas PrevPersona, //INFORMACION PERSONAL QUE SE ENCUENTRA REGISTRADA
                                                                      Usuarios Usuario,     //INFORMACION DE USUARIO NUEVA A REGISTRAR
                                                                      Usuarios PrevUsuario, //INFORMACION DE USUARIO QUE SE ENCUENTRA REGISTRADA
                                                                      DateTime fechahora,   //FECHA Y HORA DE LA CREACION DEL NUEVO REGISTRO
                                                                      double userid)        //ID DEL USUARIO QUE ACABA DE REALIZAR LA SOLICITUD DE MODIFICACION
        {
            //SE CREA UN NUEVO OBJETO ModificacionesUsuarios
            Modificacionesusuario Modificaciones = new Modificacionesusuario();

            //SE LE ASIGNA A LA PROPIEDAD "FechaHora" DEL OBJETO CREADO LA FECHA Y HORA RECIBIDA COMO PARAMETRO
            Modificaciones.FechaHora = fechahora;
            //SE LE ASIGNA A LA PROPIEDAD "IdModificador" EL ID DEL USUARIO QUE SE ENCUENTRA REALIZANDO LA SOLICITUD DE MODIFICACION DE DATOS
            Modificaciones.IdModificador = userid;
            //SE LE ASIGNA A LA PROPIEDAD "IdModificado" EL ID DEL REGISTRO DE USUARIO A MODIFICAR
            Modificaciones.IdModificado = Persona.Cedula;

            //SE EVALUA CUAL FUE EL CAMBIO REALIZADO
            if (Persona.Nombres != PrevPersona.Nombres) //=> true => SE DETECTARON CAMBIOS EN LA PROPIEDAD "Nombre(s)"
                Modificaciones.ModificacionNombres = true;

            if (Persona.Apellidos != PrevPersona.Apellidos) //=> true => SE DETECTARON CAMBIOS EN LA PROPIEDAD "Apellido(s)"
                Modificaciones.ModificacionApellidos = true;

            if (Persona.FechaNacimiento != PrevPersona.FechaNacimiento) //=> true => SE DETECTARON CAMBIOS EN LA PROPIEDAD "FechaNacimiento"
                Modificaciones.ModificacionFecha = true;
                
            if (Persona.Telefono != PrevPersona.Telefono)   //=> true => SE DETECTARON CAMBIOS EN LA PROPIEDAD "Telefono"
                Modificaciones.ModificacionTelefono = true;

            if (Persona.Correo != PrevPersona.Correo)   //=> true => SE DETECTARON CAMBIOS EN LA PROPIEDAD "Correo"
                Modificaciones.ModificacionCorreo = true;

            if (Usuario.Username != PrevUsuario.Username)   //=> true => SE DETECTARON CAMBIOS EN LA PROPIEDAD "Username"
                Modificaciones.ModificacionUsername = true;

            if (Usuario.Password != PrevUsuario.Password)   //=> true => SE DETECTARON CAMBIOS EN LA PROPIEDAD "Password"
                Modificaciones.ModificacionPassword = true;

            if (Usuario.NivelUsuario != PrevUsuario.NivelUsuario)   //=> true => SE DETECTARON CAMBIOS EN LA PROPIEDAD "Nivel de Usuario"
                Modificaciones.ModificacionNivelUsuario = true;

            //SE RETORNA EL OBJETO CREADO
            return Modificaciones;
        }
    }

    //===================================================================================
    //===================================================================================
    public partial class Personas
    {
        //ATRIBUTOS (PROPIEDADES) DE LA TABLA "Personas"
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public double Cedula { get; set; }
        public int NumeroFicha { get; set; }
        public double? Telefono { get; set; }
        public string Correo { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public DateTime FechaCreacion { get; set; }

        //==================================================================================================
        //==================================================================================================
        //FUNCIONES QUE RETORNAN UN OBJETO DE TIPO "Personas"
        public static Personas NewPersonas(Personas persona) //=> SE PASA UN OBJETO PERSONA COMO PARAMETRO
        {
            return new Personas()
            {
                Nombres = persona.Nombres,
                Apellidos = persona.Apellidos,
                Cedula = persona.Cedula,
                NumeroFicha = persona.NumeroFicha,
                FechaNacimiento = persona.FechaNacimiento,
                Telefono = persona.Telefono,
                Correo = persona.Correo,
                FechaCreacion = persona.FechaCreacion,
            };
        }

        public static Personas NewPersonas(string nombres,
                                           string apellidos,
                                           double cedula,
                                           int numeroficha,
                                           double telefono,
                                           string correo,
                                           DateTime fechanacimiento,
                                           DateTime fechacreacion) //=> SE PASAN CADA UNO DE LOS DATOS A INSERTAR EN EL OBJETO PERSONA
        {
            return new Personas()
            {
                Nombres = nombres,
                Apellidos = apellidos,
                Cedula = cedula,
                NumeroFicha = numeroficha,
                FechaNacimiento = fechanacimiento,
                Telefono = telefono,
                Correo = correo,
                FechaCreacion = fechacreacion,
            };
        }

        //==================================================================================================
        //==================================================================================================
        //FUNCIONES QUE RETORNAN UN OBJETO PERSONA CON LA NUEVA INFORMACION MODIFICADA
        //NOTA: PUESTO QUE EXISTEN DOS NIVELES DE ACCESO PARA MODIFICAR UN REGISTRO DE USUARIO, SE DECIDIO
        //CREAR DOS FUNCIONES DENTRO DE LA CLASE PERSONAS QUE MODIFIQUEN ESPECIFICAMENTE LOS CAMPOS "MODIFICABLES"
        //DE CADA PAGINA ("PaginaConfiguracion" y "PaginaConfiguracionAdmin")
        public static Personas NewPersonaInfo(Personas persona,         //=> INFORMACION REGISTRADA DEL USUARIO 
                                              ConfiguracionU newinfo)   //=> CAMPOS POSIBLEMENTE MODIFICADOS
        {
            //SE LLENAN LOS VALORES DEL REGISTRO
            return new Personas()
            {
                //INFORMACION SIN MODIFICAR
                Nombres = persona.Nombres,
                Apellidos = persona.Apellidos,
                Cedula = persona.Cedula,
                NumeroFicha = persona.NumeroFicha,
                FechaCreacion = persona.FechaCreacion,
                FechaNacimiento = persona.FechaNacimiento,
                //POSIBLE INFORMACION MODIFICADA
                //NOTA: SE DESCRIBE COMO POSIBLE DEBIDO A QUE NO ES NECESARIO MODIFICAR
                //LOS DOS CAMPOS PARA GENERAR UN REGISTRO
                Telefono = newinfo.Telefono,
                Correo = newinfo.Correo,
            };
        }

        public static Personas NewPersonaInfo(Personas persona,         //=> INFORMACION REGISTRADA DEL USUARIO
                                              ConfiguracionA newinfo)   //=> CAMPOS POSIBLEMENTE MODIFICADOS
        {
            //SE LLENAN LOS VALORES DEL REGISTRO
            return new Personas()
            {
                //INFORMACION SIN MODIFICAR
                Cedula = persona.Cedula,
                NumeroFicha = persona.NumeroFicha,
                FechaCreacion = persona.FechaCreacion,
                //POSIBLE INFORMACION MODIFICADA
                //NOTA: SE DESCRIBE COMO POSIBLE DEBIDO A QUE NO ES NECESARIO MODIFICAR
                //LOS CINCO CAMPOS PARA GENERAR UN REGISTRO
                Nombres = newinfo.Nombres,
                Apellidos = newinfo.Apellidos,
                Telefono = newinfo.Telefono,
                Correo = newinfo.Correo,
                FechaNacimiento = newinfo.FechaNacimiento,
            };
        }
    }

    //===================================================================================
    //===================================================================================
    public partial class Tableros
    {
        //ATRIBUTOS (PROPIEDADES) DE LA TABLA "Tableros"
        public string TableroId { get; set; }
        public string SapId { get; set; }
        public double Idcreador { get; set; }
        public string Filial { get; set; }
        public string AreaFilial { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string CodigoQrdata { get; set; }
        public string CodigoQrfilename { get; set; }
    }

    //===================================================================================
    //===================================================================================
    public partial class Ultimaconexion
    {
        //ATRIBUTOS (PROPIEDADES) DE LA TABLA "UltimaConexion"
        public int Id { get; set; }
        public DateTime UltimaConexion1 { get; set; }
        public double UserId { get; set; }

        //===============================================================================================
        //===============================================================================================
        //SE MANDAN LOS DATOS NECESARIOS PARA PODER LLENAR TODOS LOS ATRIBUTOS
        //CADA QUE SE GENERE UN NUEVO REGISTRO EN LA TABLA UltimaConexion

        //FUNCION CON PARAMETROS Personas Y Usuarios
        public Ultimaconexion NewUltimaConexion(Personas Persona, Usuarios Usuario)
        {
            return new Ultimaconexion
            {
                UltimaConexion1 = DateTime.Now,
                UserId = Usuario.Cedula
            };
        }

        //FUNCION CON PARAMETRO UltimaConexion
        public Ultimaconexion NewUltimaConexion(Ultimaconexion UltimaConexion)
        {
            return new Ultimaconexion
            {
                UltimaConexion1 = UltimaConexion.UltimaConexion1,
                UserId = UltimaConexion.UserId,
            };
        }
    }

    //===================================================================================
    //===================================================================================
    public partial class Usuarios
    {
        //ATRIBUTOS (PROPIEDADES) DE LA TABLA USUARIOS
        public string Username { get; set; }
        public string Password { get; set; }
        public double Cedula { get; set; }
        public int NivelUsuario { get; set; }
        public DateTime FechaCreacion { get; set; }

        //==================================================================================================
        //==================================================================================================
        //FUNCIONES PARA RETORNAR UN NUEVO OBJETO USUARIOS
        public static Usuarios NewUsuarios(Usuarios usuario) //=> SE ENVIA UN OBJETO USUARIOS COMO PARAMETRO
        {
            //SE RETORNA UN OBJETO DE TIPO "Usuarios"
            return new Usuarios()
            {
                Username = usuario.Username,
                Password = usuario.Password,
                Cedula = usuario.Cedula,
                NivelUsuario = usuario.NivelUsuario,
                FechaCreacion = usuario.FechaCreacion
            };
        }

        public static Usuarios NewUsuarios(string username,     //=> SE ENVIAN LOS DATOS QUE SERAN INSERTADOS
                                           string password,     //   EN CADA UNA DE LAS PROPIEDADES
                                           double cedula,
                                           int nivelusuario,
                                           DateTime fechacreacion)
        {
            //SE RETORNA UN OBJETO DE TIPO "Usuarios"
            return new Usuarios()
            {
                Username = username,
                Password = password,
                Cedula = cedula,
                NivelUsuario = nivelusuario,
                FechaCreacion = fechacreacion,
            };
        }

        //==================================================================================================
        //==================================================================================================
        //FUNCIONES QUE RETORNAN UN OBJETO USUARIO CON LA NUEVA INFORMACION MODIFICADA
        //NOTA: PUESTO QUE EXISTEN DOS NIVELES DE ACCESO PARA MODIFICAR UN REGISTRO DE USUARIO, SE DECIDIO
        //CREAR DOS FUNCIONES DENTRO DE LA CLASE PERSONAS QUE MODIFIQUEN ESPECIFICAMENTE LOS CAMPOS "MODIFICABLES"
        //DE CADA PAGINA ("PaginaConfiguracion" y "PaginaConfiguracionAdmin")
        public static Usuarios NewUsuarioInfo(Usuarios usuario,         //=> INFORMACION REGISTRADA DEL USUARIO
                                              ConfiguracionU newinfo)   //=> CAMPOS POSIBLEMENTE MODIFICADOS
        {
            //SE LLENAN LOS VALORES DEL REGISTRO
            return new Usuarios
            {
                Username = usuario.Username,
                Cedula = usuario.Cedula,
                NivelUsuario = usuario.NivelUsuario,
                FechaCreacion = usuario.FechaCreacion,
                Password = newinfo.Userpassword,
            };
        }

        public static Usuarios NewUsuarioInfo(Usuarios usuario,         //=> INFORMACION REGISTRADA DEL USUARIO
                                              ConfiguracionA newinfo)   //=> CAMPOS POSIBLEMENTE MODIFICADOS
        {
            //SE LLENAN LOS VALORES DEL REGISTRO
            return new Usuarios()
            {
                Cedula = usuario.Cedula,
                FechaCreacion = usuario.FechaCreacion,
                Username = newinfo.Username,
                NivelUsuario = newinfo.NivelUsuario,
                Password = newinfo.Userpassword,
            };
        }
    }

    //===================================================================================
    //===================================================================================
    //POR MOTIVOS DOCUMENTALES (5/27/2021) SE ADICIONO LA TABLA "tabla_borrador" A LA
    //BASE DE DATOS "MTTOAPP_V6" (SERVIDOR DE BASE DE DATOS ALOJADA EN EL EQUIPO
    //CORPORATIVO "10.10.4.104") CON EL FIN DE DOCUMENTAR LA MODIFICACION/INTERVENCION DE
    //LA BASE DE DATOS Y EL CODIGO FUENTE DE LOS PROYECTOS "MttoApp" y "MttoApi".
    public partial class Tabla_Borrador
    {
        public string Columna1 { get; set; }
        public double Columna2 { get; set; }
    }
}