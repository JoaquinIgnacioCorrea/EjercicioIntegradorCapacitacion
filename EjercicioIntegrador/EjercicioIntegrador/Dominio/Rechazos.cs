using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EjercicioIntegrador.Dominio
{
    public class Rechazos
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public int CodVendedor { get; set; }
        public decimal Venta { get; set; }
        public string? VentaEmpresa { get; set; }
    }
}
