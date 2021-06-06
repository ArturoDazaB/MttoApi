using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace MttoApi
{
    //=========================================================================================================
    //=========================================================================================================
    //ASP.NET CORE APPLICATION MUST INCLUDE A "Starup" CLASS. IT IS EXECUTED FIRST WHEN THE APPLICATION
    //STARTS. THIS CLASS CAN BE CONFIGURED USING THE "UseStartup<T>()" METHOD AT THE TIME OF CONFIGURING
    //THE HOST IN THE "Main()" METHOD OF THE "Program".
    //
    //THIS CLASS INCLUDES TWO PUBLIC METHODS: "ConfigureServices" AND "Configure".
    //THE "ConfigureService" METHOD IS A PLACE WHERE YOU CAN REGISTER YOUR DEPENDENT CLASSES. AFTER REGISTERING
    //DEPENDENT CLASS, IT CAN BE USED ANYWHERE IN THE APLICATION.
    //THE CONFIGURE METHOD IS A PLACE WHERE YOU CAN CONFIGURE APPLICATION REQUEST PIPELINE FOR YOUR APPLICATION
    //USING "IApplicationBuilder" INSTANCE.
    //NOTE: ASP.NET Core REFERS DEPENDENT CLASS AS A "SERVICE". SO, WHENEVER YOU READ "SERVICE" THEN
    //UNDERSTAND IT AS A CLASS WHICH IS GOING TO BE USED IN SOME OTHER CLASS.
    //________________________________________________________________________________________________________
    //LAS APLICACIONES ASP.NET CORE INCLUYEN UNA CLASE "Statup". ESTA SE EJECUTA AL PRINCIPIO CUANDO LA APLI-
    //CACION INICIA. ESTA CLASE PUEDE SER CONFIGURADA USANDO EL METODO "UseStarup<T>()" CUANDO SE CONFIGURA
    //EL HOST EN EL METODO "Main" EN LA CLASE "Program".
    //
    //ESTA CLASE INCLUYE DOS METODOS PUBLICOS: "ConfigureServices" AND "Configure".
    //EL METODO "ConfigureServices" ES UN LUGAR EN EL CUAL PUEDE REGISTRAR LAS CLASES DEPENDIENTES. DESPUES
    //DE REGISTRAR LAS CLASES DEPENDIENTES, ESTA PUEDE SER USADA EN CUALQUIER LUGAR DE LA APLICACION.
    //EL METODO "Configure" ES UN LUGAR EN DONDE PUEDES CONFIGURAR LA CANALIZACION DE LAS SOLICITUDES PARA
    //LA APLICACION USANDO LA INSTANCIA "IApplicationBuilder".
    //NOTA> ASP.NET CORE SE REFIERE A LAS CLASES DEPENDIENTES COMO "SERVICIOS", ENTONCES, CUANDO LEA "SERVICIO"
    //ENTONCES ENTIENDA QUE TIENE UNA CLASE QUE VA A SER USADA EN OTRAS CLASES.
    //=========================================================================================================
    //=========================================================================================================
    public class Startup
    {
        //CONSTRUCTOR
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        //ATRIBUTOS/PROPIEDADES DE LA CLASE "StarUp"
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // Este metodo es llamado por el runtime. Utilice este metodo apra añadir servicios al contenedor.
        public void ConfigureServices(IServiceCollection services)
        {
            //SE CONFIGURA LA CONEXION A LA BASE DE DATOS A LA CLASE CONTEXTO
            services.AddDbContext<MttoApi.Model.Context.MTTOAPP_V6Context>(op => op.UseMySql(Configuration.GetConnectionString("MTTOAPPDB7")));
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        // Este metodo es llamado por el rubtime. Utilice este metodo para configurar la canalizacion de las solicitudes HTTP
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}