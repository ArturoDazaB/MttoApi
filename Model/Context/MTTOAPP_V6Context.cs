﻿using Microsoft.EntityFrameworkCore;

namespace MttoApi.Model.Context
{
    //========================================================================================
    //========================================================================================
    /*________________________________________________________________________________________
     The DbContext class is an integral part of Entity Framework. An instance of DbContext
     represents a session with the database which can be used to query and save instances of
     your entities to a database.
    __________________________________________________________________________________________
     La clase DbContext es una parte integral del Entity Framework. Una instancia de tipo
     DbContext representa una sesion con la base de datos la cual puede ser usada para consultar
     y guardar instancias de tu entidad a la base de datos.*/

    /*NOTA: ESTA CLASE, EN CONJUNTO CON LAS CLASES QUE SE ENCUENTRAN DESCRITAS DENTRO DEL ARCHIVO
     "Tablas" (ARCHIVO UBICADO EN LA CARPETA MODEL DEL PROYECTO) FUERON CREADOS MEDIANTE EL USO
     DE LA "Consola Del Administrador De Paquetes (PACKAGE MANAGER CONSOLE)" Y EL PAQUETE
     (NIGET PACKAGE) "Pomelo.EntityFrameworkCore.MySql"

     PARA MAS INFORMACION:
        https://github.com/PomeloFoundation/Pomelo.EntityFrameworkCore.MySql
        https://docs.microsoft.com/en-us/ef/core/cli/powershell
    */

    //========================================================================================
    //========================================================================================
    public partial class MTTOAPP_V6Context : DbContext
    {
        public MTTOAPP_V6Context()
        {
        }

        public MTTOAPP_V6Context(DbContextOptions<MTTOAPP_V6Context> options)
            : base(options)
        {
        }

        //========================================================================================
        //========================================================================================
        //PROPIEDADES DE LA CLASE "MTTOAPP_V6Context"
        //NOTA: LOS OBJETOS DE TIPO "DbSet" REPRESENTA LA RECOLECCION DE TODAS LAS ENTIDADES
        //DEL CONTEXTO.
        public virtual DbSet<Historialconsultatableros> Historialconsultatableros { get; set; }

        public virtual DbSet<Historialsolicitudesweb> Historialsolicitudesweb { get; set; }
        public virtual DbSet<Items> Items { get; set; }
        public virtual DbSet<Modificacionesusuario> Modificacionesusuario { get; set; }
        public virtual DbSet<Personas> Personas { get; set; }
        public virtual DbSet<Tableros> Tableros { get; set; }
        public virtual DbSet<Ultimaconexion> Ultimaconexion { get; set; }
        public virtual DbSet<Usuarios> Usuarios { get; set; }

        //---------------------------------------------------------------------------
        //---------------------------------------------------------------------------
        public virtual DbSet<TablaBorrador> TablaBorrador { get; set; }

        //---------------------------------------------------------------------------
        //---------------------------------------------------------------------------

        //========================================================================================
        //========================================================================================
        //FUNCION OnConfiguring DONDE SE ESPECIFICARA LA BASE DE DATOS QUE SE VA A USAR
        //ADEMAS DE LA CADENA DE CONEXION CON LA BASE DE DATOS
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                //#warning To protect potentially sensitive information in your connection string, you should move it out of source code.
                //See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseMySql("Server=127.0.0.1;Port=3306;Database=MTTOAPP_V6;Uid=CarlosDaza;Pwd=*122900900625*", x => x.ServerVersion("8.0.23-mysql"));
            }
        }

        //========================================================================================
        //========================================================================================
        //FUNCION "OnModelCreating" ENCARGADA DE MODELAR Y RELACIONAR CADA TABLA DE LA BASE
        //DE DATOS CON EL OBJETO QUE LO REPRESENTARA COMO ENTIDAD
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Historialconsultatableros>(entity =>
            {
                entity.ToTable("historialconsultatableros");

                entity.HasIndex(e => e.FechaConsulta)
                    .HasName("FechaConsulta")
                    .IsUnique();

                entity.HasIndex(e => e.Id)
                    .HasName("Id")
                    .IsUnique();

                entity.Property(e => e.FechaConsulta).HasColumnType("datetime");

                entity.Property(e => e.TableroId)
                    .IsRequired()
                    .HasColumnType("text")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.TipoDeConsulta)
                    .IsRequired()
                    .HasColumnType("text")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");
            });

            modelBuilder.Entity<Historialsolicitudesweb>(entity =>
            {
                entity.ToTable("historialsolicitudesweb");

                entity.HasIndex(e => e.FechaHora)
                    .HasName("FechaHora")
                    .IsUnique();

                entity.HasIndex(e => e.Id)
                    .HasName("Id")
                    .IsUnique();

                entity.Property(e => e.FechaHora).HasColumnType("datetime");

                entity.Property(e => e.Solicitud)
                    .IsRequired()
                    .HasColumnType("text")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");
            });

            modelBuilder.Entity<Items>(entity =>
            {
                entity.ToTable("items");

                entity.Property(e => e.Descripcion)
                    .HasColumnType("text")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.TableroId)
                    .IsRequired()
                    .HasColumnType("varchar(20)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");
            });

            modelBuilder.Entity<Modificacionesusuario>(entity =>
            {
                entity.ToTable("modificacionesusuario");

                entity.HasIndex(e => e.FechaHora)
                    .HasName("FechaHora")
                    .IsUnique();

                entity.Property(e => e.FechaHora).HasColumnType("datetime");
            });

            modelBuilder.Entity<Personas>(entity =>
            {
                entity.HasKey(e => e.Cedula)
                    .HasName("PRIMARY");

                entity.ToTable("personas");

                entity.Property(e => e.Apellidos)
                    .IsRequired()
                    .HasColumnType("text")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.Correo)
                    .HasColumnType("text")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

                entity.Property(e => e.FechaNacimiento).HasColumnType("datetime");

                entity.Property(e => e.Nombres)
                    .IsRequired()
                    .HasColumnType("text")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");
            });

            modelBuilder.Entity<Tableros>(entity =>
            {
                entity.HasKey(e => e.SapId)
                    .HasName("PRIMARY");

                entity.ToTable("tableros");

                entity.HasIndex(e => e.TableroId)
                    .HasName("TableroId")
                    .IsUnique();

                entity.Property(e => e.SapId)
                    .HasColumnType("varchar(20)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.AreaFilial)
                    .IsRequired()
                    .HasColumnType("text")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.CodigoQrdata)
                    .IsRequired()
                    .HasColumnName("CodigoQRData")
                    .HasColumnType("text")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.CodigoQrfilename)
                    .IsRequired()
                    .HasColumnName("CodigoQRFilename")
                    .HasColumnType("text")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.FechaRegistro).HasColumnType("datetime");

                entity.Property(e => e.Filial)
                    .IsRequired()
                    .HasColumnType("text")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.Idcreador).HasColumnName("IDCreador");

                entity.Property(e => e.TableroId)
                    .IsRequired()
                    .HasColumnType("varchar(20)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");
            });

            modelBuilder.Entity<Ultimaconexion>(entity =>
            {
                entity.ToTable("ultimaconexion");

                entity.HasIndex(e => e.UltimaConexion1)
                    .HasName("UltimaConexion")
                    .IsUnique();

                entity.Property(e => e.UltimaConexion1)
                    .HasColumnName("UltimaConexion")
                    .HasColumnType("datetime");
            });

            modelBuilder.Entity<Usuarios>(entity =>
            {
                entity.HasKey(e => e.Cedula)
                    .HasName("PRIMARY");

                entity.ToTable("usuarios");

                entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasColumnType("tinytext")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasColumnType("tinytext")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");
            });

            //---------------------------------------------------------------------------
            //---------------------------------------------------------------------------
            //POR MOTIVOS DOCUMENTALES (5/27/2021) SE ADICIONO LA TABLA "tabla_borrador"
            //A LA BASE DE DATOS "MTTOAPP_V6" (SERVIDOR DE BASE DE DATOS ALOJADA EN EL
            //EQUIPO CORPORATIVO "10.10.4.104") CON EL FIN DE DOCUMENTAR LA MODIFICACION/
            //INTERVENCION DE LA BASE DE DATOS Y EL CODIGO FUENTE DE LOS PROYECTOS
            //"MttoApp" y "MttoApi".
            modelBuilder.Entity<TablaBorrador>(entity =>
            {
                entity.ToTable("tabla_borrador");

                entity.Property(e => e.Columna1)
                    .IsRequired()
                    .HasColumnType("text")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");
            });
            //---------------------------------------------------------------------------
            //---------------------------------------------------------------------------

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}