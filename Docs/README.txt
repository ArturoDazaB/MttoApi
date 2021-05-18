=================================================================================
=================================================================================
HABILITAR Y CONFIGURAR IIS Y APLICACIONES ASP.NET CORE - Windows Server 2012 R2
=================================================================================
=================================================================================

>Si su aplicacion o servicio web utiliza ASP.NET en versiones desde la 2 hasta
la 3.5 se debe instalar dicho framework en su version 3.5 antes de activar el 
modulo de IIS y el propio modulo ASP.NET

NOTA: SE DEBEN REALIZAR ESTOS INSTRUCTIVOS SI EL SERVIDOR NO TIENE INSTALADO
.NET Framework

1) Ejecute el cmd como administrador (run as administrator).

2) Ejecute el siguiente comando en la aplicacion de lineas de comando
	"dism /online /enable-feature /featurename:netfx3".

3) Espere hasta que se complete el proceso (puede tomar varios minutos).

=================================================================================
>Habilitar los modulos de IIS y ASP.NET

1) Ejecute el Administrador del Servidor (Server Manager).

2) Desde la seccion "Panel" (dashboard) seleccione la opcion "Agregar roles y
   caracteristicas" (add roles and features).

3) Aparecera una ventana emergente que lleva por nombre "Asistente para agregar
   roles y caracteristicas".

4) En la seccion "Tipo de Instalacion" seleccione la opcion "Instalacion Basada
   en Caracteristicas o en roles" (role-based or feature-based instalation).
5) En la seccion "Seleccion del Servidor" seleccione el servidor en el cual 
   desea activar IIS de la lista de opciones.

6) En la seccion "Roles del Servidor" busque la opcion Web Server (IIS) y haga
   clic sobre esta. Aparecerá otra ventana emergente de confirmacion, en ella 
   se observa un checkmark seleccionado con la opcion "Incluir heramientas de 
   administracion (si es aplicable)". Cuando esto ocurra haga clic en el boton
   de confirmacion "Agregar Caracteristicas".

7) En las seccione "Caracteristicas" y "Rol del servidor web (IIS)" puede hacer
   el clic en "siguiente" sin ninguna opcion adicional (opcional).

8) La configuracion de la seccion "Servicios del rol" dependera de las caracte-
   risticas que desee agregar a su servidor. Puede dejar los servidores prede-
   terminados si no esta seguro, de lo contrario debera PERSONALIZAR las carac-
   teristicas segun los requerimientos del usuario.

   Expanda la categoria "Application Development" y seleccione la casilla 
   "ASP.NET 4.5" (Si se tomaron los primeros tres pasos de esta guia para
   instalar .NET 3.5 tambien debe seleccionar la casilla "ASP.NET 3.5").

9) En la ultima seccion del "Asistente para agregar roles y caracteristicas"
   podrá confirmar todos los elementos correspondientes a la configuracion
   dispuesta anteriormente.

>En este punto tendra instalado un servidor web con IIS listo para usar y
 publicar su sitio web.

=================================================================================

>Si usted planifico el uso de aplicaciones ASP.NET en su sito web con IIS y ha 
desarrollado una aplicacion asociada, debe agregar dicha aplicacion a la confi-
guracion, para hacerlo:

1) Vuelva a abrir el "Administrador del Servidor" y luego haga clic en "Herrami-
   entas" -> "Asministrador del Internet Information Service (IIS)".

2) Expanda la pestaña "Sitios" y haga clic derecho sobre el sitio web en cuestion.
   Luego selecione "Agregar aplicacion".

3) Aparecera una ventana emergente titulada "Agregar Aplicacion". En el cuadro de 
   texto "Alias" escriba un nombre para el URL de la aplicacion (esto permitirá
   acceder a la aplicacion con una direccion URL).

4) Haga clic en "Seleccionar" si desea elegir una aplicacion distinta a la 
   mostrada.

5) En el cuadro "Ruta de acceso fisica" escriba la ruta al directorio de la 
   aplicacion o haga clic en el boton (...) para navegar en el explorador de 
   archivos. Para terminar haga clic en Aceptar.