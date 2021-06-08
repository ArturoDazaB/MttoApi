using System;
using System.Collections.Generic;

namespace MttoApi.Model
{
    //=======================================================================================================
    //=======================================================================================================
    //CLASES DTO (DATA TRANSFER OBJECT). ESTAS CLASES SON CREADAS PARA FUNCIONAR COMO CLASE MODELO PARA LA
    //INFORMACION DE LOS OBJETOS QUE SON ENVIADOS O RECIBIDOS.
    //POR EJEMPLO: SI QUISIERAMOS MODIFICAR UN ATRIBUTO EN ESPECIFICO DE UN REGISTRO EN LA TABLAS PERSONAS
    //MEDIANTE LA SOLICITUD HTTP "PUT" DEBERIAMOS ENVIAR EL PRIMARY KEY O ID DE DICHO REGISTRO JUNTO CON
    //UN OBJETO JSON HOMOLOGO A EL OBJETO PERSONAS, ES DECIR CON TODOS LOS ATRIBUTOS QUE SE DESEAN MODIFICAR
    //ADEMAS DE LOS ATRIBUTOS QUE YA EXISTIAN ANTES.
    //______________________________________________________________________________________________________
    //ES DEBIDO A ESTO QUE SE CREARON LOS OBJETOS DTO LOS CUALES SOLO POSEERAN LOS ATRIBUTOS CON LOS QUE
    //REGULARMENTE SE TRABAJARAN MEDIANTE LAS SOLICITUDES HTTP.

    //=======================================================================================================
    //=======================================================================================================
    //CLASE QUE CONTENDRA LOS PARAMETROS MODIFICABLES DE LA PAGINA "PaginaConfiguracion"
    public partial class ConfiguracionU
    {
        public double Cedula { get; set; }
        public double Telefono { get; set; }
        public string Correo { get; set; }
        public string Userpassword { get; set; }

        //======================================================================
        //======================================================================
        //FUNCION QUE RETORNA UN OBJETO DE TIPO "ConfiguracionU"
        public static ConfiguracionU NewConfiguracionU(ConfiguracionU newinfo) //=> SE RECIBE UN OBJETO "ConfiguracionU" COMO PARAMETRO
        {
            //SE INICIALIZA Y RETORNA UN OBJETO DE TIPO "ConfiguracionU"
            return new ConfiguracionU
            {
                Cedula = newinfo.Cedula,
                Telefono = newinfo.Telefono,
                Correo = newinfo.Correo,
                Userpassword = newinfo.Userpassword,
            };
        }
    }

    //=======================================================================================================
    //=======================================================================================================
    //CLASE QUE CONTENDRA LOS PARAMETROS MODIFICABLES DE LA PAGINA "PaginaConfiguracionAdmin"
    //NOTA: ESTA CLASE ES DERIVADA DE LA CLASE "ConfiguracionU", ESTO DEBIDO A QUE LA CLASE "ConfiguracionA"
    //POSEE LOS MISMO ATRIBUTOS MODIFICABLES DE LA CLASE "ConfiguracionU", SIN EMBARGO, PUESTO QUE ESTA CLASE
    //ES USADA CUANDO SE REALIZAN MODIFICACIONES DESDE LA PAGINA "PaginaConfiguracionAdmin" SE ADICIONAN NUEVAS
    //PROPIEDADES MODIFICABLES.
    public partial class ConfiguracionA : ConfiguracionU
    {
        //ATRIBUTOS (PROPIEDADES) DE LA CLASE "ConfiguracionA"
        public string Nombres { get; set; }

        public string Apellidos { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string Username { get; set; }

        public int NivelUsuario { get; set; }

        //======================================================================
        //======================================================================
        //FUNCION QUE RETORNAN UN OBJETO DE TIPO "ConfiguracionA"
        public static ConfiguracionA NewConfiguracionA(ConfiguracionA newinfo)  //=> SE RECIBE UN OBJETO "ConfiguracionA" COMO PARAMETRO
        {
            //SE INICIALIZA Y RETORNA UN OBJETO DE TIPO "ConfiguracionA"
            return new ConfiguracionA
            {
                Nombres = newinfo.Nombres,
                Apellidos = newinfo.Apellidos,
                Cedula = newinfo.Cedula,
                FechaNacimiento = newinfo.FechaNacimiento,
                Telefono = newinfo.Telefono,
                Correo = newinfo.Correo,

                Username = newinfo.Username,
                Userpassword = newinfo.Userpassword,
                NivelUsuario = newinfo.NivelUsuario,
            };
        }
    }

    //=======================================================================================================
    //=======================================================================================================
    //CLASE USADA EN LAS PAGINAS "PaginaPrincipal", "PaginaConfiguracion", "PaginaConfiguracionAdmin".
    //NOTA: ESTA CLASE RETORNADA TODA LA INFORMACION DEL REGISTRO DE UN USUARIO EN GENERAL (INFORMACION
    //PERSONAL E INFORMACION DE USUARIO).
    public partial class InformacionGeneral
    {
        //ATRIBUTOS (PROPIEDADES) DE LA CLASE INFORMACION GENERAL
        public Personas Persona { get; set; }

        public Usuarios Usuario { get; set; }

        //======================================================================
        //======================================================================
        //FUNCION QUE RETORNA UN OBJETO "InformacionGeneral"
        public static InformacionGeneral NewInformacionGeneral(Personas persona,    //=> SE RECIBE LA INFORMACION PERSONAL DE USUARIO
                                                               Usuarios usuario)    //=> SE RECIBE LA INFORMACION DE USUARIO DEL USUARIO
        {
            return new InformacionGeneral()
            {
                Persona = persona,
                Usuario = usuario,
            };
        }
    }

    //=======================================================================================================
    //=======================================================================================================
    //CLASE USADA EN LA PAGINA "PaginaPrincipal" PARA RETORNAR TODA LA INFORMACION DEL USUARIO QUE DESEA
    //INGRESAR A LA PLATAFORMA
    public partial class LogInResponse
    {
        //ATRIBUTOS (PROPIEDADES) DE LA CLASE "LogInResponse"
        public InformacionGeneral UserInfo { get; set; }    //=> PROPIEDAD QUE CONTENDRA TODA LA INFORMACION

                                                            //   DEL USUARIO QUE DESEA INGRESAR A LA PLATAFORMA
        public DateTime UltimaFechaIngreso { get; set; }

        //==========================================================================================
        //==========================================================================================
        //FUNCION QUE RETORNA UN OBJETO DE TIPO "LogInResponse"
        public static LogInResponse NewLogInResponse(InformacionGeneral info,   //SE RECIBE UN OBJETO "InformacionGeneral"
                                                     DateTime ultimoingreso)    //SE RECIBE UN OBJETO "Datetime"
        {
            //SE RETORNA UN OBJETO DE TIPO "LogInResponse"
            return new LogInResponse()
            {
                UserInfo = info,
                UltimaFechaIngreso = ultimoingreso,
            };
        }
    }

    //=======================================================================================================
    //=======================================================================================================
    public class LogInRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    //=======================================================================================================
    //=======================================================================================================
    //CLASE USADA EN LA PAGINA "PaginaRegistro" PARA RECIBIR LA INFORMACION DEL USUARIO QUE SE VA A REGISTRAR
    //Y EL ID DEL USUARIO QUE SE ENCUENTRA REALIZANDO EL REGISTRO
    public partial class RequestRegistroUsuario
    {
        //ATRIBUTOS (PROPIEDADES) DE LA CLASE "RequestRegistroUsuario"
        public InformacionGeneral NewUser { get; set; }

        public double UserId { get; set; }
    }

    //=======================================================================================================
    //=======================================================================================================
    //CLASE USADA EN LA PAGINA "PaginaQueryAdmin" PARA USARSE COMO MODELO PARA LA LISTA DE USUARIOS SOLICITADOS
    //EN DICHA PAGINA
    public partial class QueryAdmin
    {
        //ATRIBUTOS (PROPIEDADES) DE LA CLASE "QueryAdmin"
        public string Nombres { get; set; }

        public string Apellidos { get; set; }
        public double Cedula { get; set; }

        //======================================================================
        //======================================================================
        //FUNCION QUE RETORNA UNA OBJETO DE TIPO "QueryAdmin"
        public static QueryAdmin NewQueryAdmin(Personas persona)
        {
            return new QueryAdmin()
            {
                Nombres = persona.Nombres,
                Apellidos = persona.Apellidos,
                Cedula = persona.Cedula,
            };
        }
    }

    //=======================================================================================================
    //=======================================================================================================
    //CLASE USADA EN LA PAGINA "PaginaRegistroTablero" ENVIADA DESDE LA APLICACION AL SERVIDOR PARA REGISTRAR
    //UN NUEVO TALERO EN LA BASE DE DATOS
    public partial class RegistroTablero
    {
        //ATRIBUTOS (PROPIEDADES) DE LA CLASE
        public Tableros tableroInfo { get; set; }

        public List<Items> itemsTablero { get; set; }

        //======================================================================
        //======================================================================
        //FUNCION QUE RETORNA UNA OBJETO DE TIPO "QueryAdmin"
        public static RegistroTablero NewRegistroTablero(Tableros tableroinfo,      //=> INFORMACION DEL TABLERO
                                                        List<Items> itemstablero)   //=> LISTA DE ITEMS QUE FORMAN PARTE DEL TABLERO
        {
            return new RegistroTablero()
            {
                tableroInfo = tableroinfo,
                itemsTablero = itemstablero,
            };
        }
    }

    //=======================================================================================================
    //=======================================================================================================
    //CLASE USADA EN LA PAGINA "PaginaConsultaTablero" ENVIADA DESDE LA APLICACION AL SERVIDOR PARA SOLICITAR
    //LA INFORMACION DE UN TABLERO A CONSULTAR POR ESCANER O POR CONSULTA DE ID
    public partial class RequestConsultaTablero
    {
        //ATRIBUTOS (PROPIEDADES) DE LA CLASE "RequestConsultaTablero"
        public double UserId { get; set; }      //=> ID DEL USUARIO QUE REALIZA LA SOLICITUD

        public string TableroId { get; set; }   //=> ID DEL TABLERO A CONSULTAR
        public string SapId { get; set; }       //=> ID DE SAP DEL TABLERO A CONSULTAR

        //NOTA: LAS PROPIEDADES "TableroId" Y "SapId" SON MUTUAMENTE EXCLUYENTES. ESTO IMPLICA
        //QUE AL REALIZAR LA CONSULTA, AL RECIBIR UN OBJETO DE TIPO "ResquestConsultaTablero"
        //SI LA PROPIEDAD "TableroId" CONTIENE INFORMCION ENTONCES LA PROPIEDAD "SapId" DEBE SER
        //NULA O VACIA. LO MISMO OCURRE SI LA PROPIEDAD "SapId" CONTIENE INFORMACION.
    }

    //=======================================================================================================
    //=======================================================================================================
    //CLASE USADA EN LA PAGINA "PaginaQueryAdmin" ENVIADA DESDE LA APLICACION AL SERVIDOR PARA SOLICITAR
    //LA LISTA DE USUARIOS QUE COINCIDAN CON EL PARAMETRO ENVIADO Y LA OPCION DE CONSULTA SELECCIONADA.
    public partial class RequestQueryAdmin
    {
        //ATRIBUTOS (PARAMETROS) DE LA CLASE "RequestQueryAdmin".
        public string Parametro { get; set; }   //=> PARAMETRO DE CONSULTA ENVIADO COMO REFERENCIA

        public double UserId { get; set; }      //=> Id (CEDULA) DEL USUARIO QUE REALIZO LA SOLICITUD
    }

    //=======================================================================================================
    //=======================================================================================================
    //CLASE USADA EN LA PAGINA "PaginaQueryAdmin" ENVIADA DESDE LA APLICACION AL SERVIDOR PARA SOLICITAR
    //LA INFORMACION DEL USUARIO SELECCIONADO POR LA LISTA DE USUARIOS QUE COINCIDEN CON EL PARAMETRO
    //ENVIADO.
    public partial class UserSelectedRequest
    {
        //ATRIBUTOS (PROPIEDADES) DE LA CLASE
        public double UserIdSelected { get; set; }  //=> ID DEL USUARIO QUE SE VA A RETORNAR

        public double UserIdRequested { get; set; } //=> ID DEL USUARIO QUE REALIZA LA SOLICITUD
    }
}