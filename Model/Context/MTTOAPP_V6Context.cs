using Microsoft.EntityFrameworkCore;

namespace MttoApi.Model.Context
{
    public partial class MTTOAPP_V6Context : DbContext
    {
        public MTTOAPP_V6Context()
        {
        }

        public MTTOAPP_V6Context(DbContextOptions<MTTOAPP_V6Context> options)
            : base(options)
        {
        }

        public virtual DbSet<Historialconsultatableros> Historialconsultatableros { get; set; }
        public virtual DbSet<Historialsolicitudesweb> Historialsolicitudesweb { get; set; }
        public virtual DbSet<Items> Items { get; set; }
        public virtual DbSet<Modificacionesusuario> Modificacionesusuario { get; set; }
        public virtual DbSet<Personas> Personas { get; set; }
        public virtual DbSet<Tableros> Tableros { get; set; }
        public virtual DbSet<Ultimaconexion> Ultimaconexion { get; set; }
        public virtual DbSet<Usuarios> Usuarios { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                //#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseMySql("server=Localhost;port=3306;database=MTTOAPP_V6;uid=root;pwd=Ca06Db0*", x => x.ServerVersion("8.0.21-mysql"));
            }
        }

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

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}