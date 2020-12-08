using System;

namespace MttoApi.Model
{
    //-----------------------------------------------------------------------------------
    //--------------------------TABLAS DE LA BASE DE DATOS-------------------------------
    //===================================================================================
    //===================================================================================
    public partial class Historialconsultatableros
    {
        public int Id { get; set; }
        public string TableroId { get; set; }
        public string TipoDeConsulta { get; set; }
        public double UsuarioId { get; set; }
        public DateTime FechaConsulta { get; set; }

        //===================================================================================
        //===================================================================================
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

    //===================================================================================
    //===================================================================================
    public partial class Historialsolicitudesweb
    {
        public int Id { get; set; }
        public double UsuarioId { get; set; }
        public DateTime FechaHora { get; set; }
        public string Solicitud { get; set; }

        //===================================================================================
        //===================================================================================

        public static Historialsolicitudesweb NewHistorialSolocitudesWeb(double userid, int solicitud)
        {
            string _solicitud = string.Empty;

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
                //REQUEST GET mttoapp/getinfo/cedula
                //        GET mttoapp/getinfo/id
                //----------------------------------------------------
                //        GET mttoapp/getinfo/ficha
                //        GET mttoapp/getinfo/numeroficha
                //----------------------------------------------------
                //        GET mttoapp/getinfo/nombre
                //        GET mttoapp/getinfo/nombres
                //----------------------------------------------------
                //        GET mttoapp/getinfo/apellidos
                //        GET mttoapp/getinfo/apellido
                //----------------------------------------------------
                //        GET mttoapp/getinfo/username
                case 6:
                    _solicitud = "Controlador=QueryAdminController;SolicitudHTTP=HTTPGET;Metodo=GetCedula;RespuestaHTTP=Ok(<List<QueryAdmin>> ObjetoLista)";
                    break;
                case 7:
                    _solicitud = "Controlador=QueryAdminController;SolicitudHTTP=HTTPGET;Metodo=GetFicha;RespuestaHTTP=Ok(<List<QueryAdmin>> ObjetoLista)";
                    break;
                case 8:
                    _solicitud = "Controlador=QueryAdminController;SolicitudHTTP=HTTPGET;Metodo=GetNombre;RespuestaHTTP=Ok(<List<QueryAdmin>> ObjetoLista)";
                    break;
                case 9:
                    _solicitud = "Controlador=QueryAdminController;SolicitudHTTP=HTTPGET;Metodo=GetApellido;RespuestaHTTP=Ok(<List<QueryAdmin>> ObjetoLista)";
                    break;
                case 10:
                    _solicitud = "Controlador=QueryAdminController;SolicitudHTTP=HTTPGET;Metodo=GetUsername;RespuestaHTTP=Ok(<List<QueryAdmin>> ObjetoLista)";
                    break;
                //--------------------------------------------------------------------------------------------------------------------------------------------------
                //CONTROLADOR = "Configuracion Controller"
                //REQUEST PUT mttoapp/configuracion/usuario/<cedula>
                //        PUT mttoapp/configuracion/usu/<cedula>
                //----------------------------------------------------
                //        PUT mttoapp/configuracion/administrator/<cedula>
                //        PUT mttoapp/configuracion/adm/<cedula>
                case 11:
                    _solicitud = "Controlador=ConfiguracionController;SolicitudHTTP=HTTPPUT;Metodo=ActualizarUsuario;RespuestaHTTP=Ok(<string> mensaje)";
                    break;
                case 12:
                    _solicitud = "Controlador=ConfiguracionController;SolicitudHTTP=HTTPPUT;Metodo=ActualizarUsuarioAdm;RespuestaHTTP=Ok(<string> mensaje)";
                    break;
            }

            return new Historialsolicitudesweb()
            {
                UsuarioId = userid,
                FechaHora = DateTime.Now,
                Solicitud = _solicitud,
            };
        }

    }

    //===================================================================================
    //===================================================================================
    public partial class Items
    {
        public int Id { get; set; }
        public string TableroId { get; set; }
        public string Descripcion { get; set; }
        public int Cantidad { get; set; }
    }

    //===================================================================================
    //===================================================================================
    public partial class Modificacionesusuario
    {
        public int Id { get; set; }
        public double IdModificado { get; set; }
        public double IdModificador { get; set; }
        public DateTime FechaHora { get; set; }
        public bool? ModificacionNombres { get; set; }
        public bool? ModificacionApellidos { get; set; }
        public bool? ModificacionFecha { get; set; }
        public bool? ModificacionUsername { get; set; }
        public bool? ModificacionCorreo { get; set; }
        public bool? ModificacionTelefono { get; set; }
        public bool? ModificacionPassword { get; set; }

        public static Modificacionesusuario NewModificacionesUsuarios(Personas Persona, Personas PrevPersona,
                                                                    Usuarios Usuario, Usuarios PrevUsuario, DateTime fechahora, double userid)
        {
            //SE CREA UN NUEVO OBJETO ModificacionesUsuarios
            Modificacionesusuario Modificaciones = new Modificacionesusuario();
            Modificaciones.FechaHora = DateTime.Now;

            //SE VERIFICA SI SE ACCEDIO DESDE LA PAGINA DE ADMINISTRATOR 
            Modificaciones.IdModificador = userid;
            Modificaciones.IdModificado = Persona.Cedula;

            //SE EVALUA CUAL FUE EL CAMBIO REALIZADO
            if (Persona.Nombres != PrevPersona.Nombres)
                Modificaciones.ModificacionNombres = true;
            if (Persona.Apellidos != PrevPersona.Apellidos)
                Modificaciones.ModificacionApellidos = true;
            if (Persona.FechaNacimiento != PrevPersona.FechaNacimiento)
                Modificaciones.ModificacionFecha = true;
            if (Persona.Telefono != PrevPersona.Telefono)
                Modificaciones.ModificacionTelefono = true;
            if (Persona.Correo != PrevPersona.Correo)
                Modificaciones.ModificacionCorreo = true;
            if (Usuario.Username != PrevUsuario.Username)
                Modificaciones.ModificacionUsername = true;
            if (Usuario.Password != PrevUsuario.Password)
                Modificaciones.ModificacionPassword = true;

            return Modificaciones;
        }
    }

    //===================================================================================
    //===================================================================================
    public partial class Personas
    {
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
        public static Personas NewPersonas(Personas persona)
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

        public static Personas NewPersonas(string nombres, string apellidos, double cedula, int numeroficha, double telefono, string correo,
            DateTime fechanacimiento, DateTime fechacreacion)
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
        public static Personas NewPersonaInfo(Personas persona, ConfiguracionU newinfo)
        {
            //SE LLENAN LOS VALORES DEL REGISTRO
            return new Personas()
            {
                Nombres = persona.Nombres,
                Apellidos = persona.Apellidos,
                Cedula = persona.Cedula,
                NumeroFicha = persona.NumeroFicha,
                FechaCreacion = persona.FechaCreacion,
                FechaNacimiento = persona.FechaNacimiento,

                Telefono = newinfo.Telefono,
                Correo = newinfo.Correo,
            };
        }

        public static Personas NewPersonaInfo(Personas persona, ConfiguracionA newinfo)
        {
            //SE LLENAN LOS VALORES DEL REGISTRO
            return new Personas()
            {
                Cedula = persona.Cedula,
                NumeroFicha = persona.NumeroFicha,
                FechaCreacion = persona.FechaCreacion,

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
        public string Username { get; set; }
        public string Password { get; set; }
        public double Cedula { get; set; }
        public int NivelUsuario { get; set; }
        public DateTime FechaCreacion { get; set; }

        //==================================================================================================
        //==================================================================================================

        public static Usuarios NewUsuarios(Usuarios usuario)
        {
            return new Usuarios()
            {
                Username = usuario.Username,
                Password = usuario.Password,
                Cedula = usuario.Cedula,
                NivelUsuario = usuario.NivelUsuario,
                FechaCreacion = usuario.FechaCreacion
            };
        }

        public static Usuarios NewUsuarios(string username, string password, double cedula, int nivelusuario, DateTime fechacreacion)
        {
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

        public static Usuarios NewUsuarioInfo(Usuarios usuario, ConfiguracionU newinfo)
        {
            return new Usuarios
            {
                Username = usuario.Username,
                Cedula = usuario.Cedula,
                NivelUsuario = usuario.NivelUsuario,
                FechaCreacion = usuario.FechaCreacion,
                Password = newinfo.Userpassword,
            };
        }

        public static Usuarios NewUsuarioInfo(Usuarios usuario, ConfiguracionA newinfo)
        {
            return new Usuarios()
            {
                Cedula = usuario.Cedula,
                NivelUsuario = usuario.NivelUsuario,
                FechaCreacion = usuario.FechaCreacion,
                Username = newinfo.Username,
                Password = newinfo.Userpassword,
            };
        }
    }
}