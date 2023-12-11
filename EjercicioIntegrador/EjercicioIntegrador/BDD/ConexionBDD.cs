using EjercicioIntegrador.Dominio;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace EjercicioIntegrador.BDD
{
    public class ConexionBDD : DbContext
    {
        public ConexionBDD(DbContextOptions<ConexionBDD> OpConexion) : base(OpConexion) { }
        public DbSet<Ventas> Ventas { get; set; }
        public DbSet<Rechazos> Rechazos { get; set; }
        public DbSet<Parametria> Parametria { get; set; }

        protected override void OnModelCreating(ModelBuilder ModeloConstructor)
        {
            ModeloConstructor.Entity<Ventas>(entity =>
            {
                entity.ToTable("ventas_mensuales");
                entity.Property(prop => prop.Fecha)
                       .HasColumnName("FechaInforme");
            });

            ModeloConstructor.Entity<Rechazos>(entity =>
            {
                entity.ToTable("rechazos");
                entity.Property(prop => prop.Fecha)
                       .HasColumnName("FechaInforme");
            });

            ModeloConstructor.Entity<Parametria>(entity =>
            {
                entity.HasNoKey();
                entity.ToTable("parametria");
                entity.Property(prop => prop.Fecha)
                                .HasColumnName("fecha_proceso");
            });

            base.OnModelCreating(ModeloConstructor);
        }
    }
}
