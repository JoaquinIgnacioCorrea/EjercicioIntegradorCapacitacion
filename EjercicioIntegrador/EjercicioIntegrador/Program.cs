using EjercicioIntegrador.BDD;
using EjercicioIntegrador.Dominio;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Linq;
using System.Text.Json;

namespace EjercicioIntegrador
{
    internal class Program
    {
        static void Main(string[] args)
        {
            #region Conexion BDD
            string Conexion = "Data Source=localhost;Initial Catalog=PruebasCapacitacion;Integrated Security=True;Encrypt=True;Trust Server Certificate=True";
            var Opcion = new DbContextOptionsBuilder<ConexionBDD>().UseSqlServer(Conexion).Options;
            var BDDContexto = new ConexionBDD(Opcion);
            #endregion

            #region Carga Archivo Texto
            string DireccionArchTxt = "C:\\Users\\taxrem\\OneDrive\\Escritorio\\Git Prueba\\EjercicioIntegrador\\data.txt";
            var Archivo = File.ReadAllLines(DireccionArchTxt);
            #endregion

            #region Carga de Ventas y Rechazos
            var FechaConvertida = BDDContexto.Parametria.FirstOrDefault();

            foreach (var Linea in Archivo)
            {
                var FechaLinea = DateTime.ParseExact(Linea.Substring(0, 8), "yyyyMMdd", null);
                var CodVendedorLinea = Linea.Substring(8, 3).Trim();
                var VentaLinea = decimal.Parse(Linea.Substring(11, 11), CultureInfo.InvariantCulture);
                var VentaEmpresaLinea = Linea.Substring(22, 1);

                if (FechaLinea == FechaConvertida.Fecha)
                {
                    if (CodVendedorLinea != "" && CodVendedorLinea != " ")
                    {
                        if (VentaEmpresaLinea == "S" || VentaEmpresaLinea == "N")
                        {
                            var NuevaVenta = new Ventas() { Fecha = FechaLinea, CodVendedor = int.Parse(CodVendedorLinea), Venta = VentaLinea, VentaEmpresa = VentaEmpresaLinea };
                            BDDContexto.Ventas.Add(NuevaVenta);
                        }
                        else
                        {
                            var RechazoVenta = new Rechazos() { Fecha = FechaLinea, CodVendedor = int.Parse(CodVendedorLinea), Venta = VentaLinea, VentaEmpresa = VentaEmpresaLinea };
                            BDDContexto.Rechazos.Add(RechazoVenta);
                        }
                    }
                    else
                    {
                        var RechazoVenta = new Rechazos() { Fecha = FechaLinea, CodVendedor = 000, Venta = VentaLinea, VentaEmpresa = VentaEmpresaLinea };
                        BDDContexto.Rechazos.Add(RechazoVenta);
                    }
                }
                else
                {
                    var RechazoVenta = new Rechazos() { Fecha = FechaLinea, CodVendedor = int.Parse(CodVendedorLinea), Venta = VentaLinea, VentaEmpresa = VentaEmpresaLinea };
                    BDDContexto.Rechazos.Add(RechazoVenta);
                }
            }
            BDDContexto.SaveChanges();
            #endregion

            #region Listar Act
            var ListaMasVenta = BDDContexto.Ventas.Where(v => v.Venta > 100000)
                .Select(v => new Ventas{ Id = int.Parse(v.CodVendedor.ToString()), Venta = v.Venta }).ToList();

            string ListaMsjVentaMas = string.Join(Environment.NewLine, ListaMasVenta.Select(v => $"El vendedor {v.Id} vendió {v.Venta}"));

            var ListaMenosVenta = BDDContexto.Ventas.Where(v => v.Venta < 100000)
                .Select(v => new Ventas { Id = int.Parse(v.CodVendedor.ToString()), Venta = v.Venta }).ToList();

            string ListaMsjVentaMenos = string.Join(Environment.NewLine, ListaMenosVenta.Select(v => $"El vendedor {v.Id} vendió {v.Venta}"));

            var ListaVentaEG = BDDContexto.Ventas.Where(v => v.VentaEmpresa == "S").GroupBy(v => v.CodVendedor)
                .Select(v => new Ventas { Id = int.Parse(v.Key.ToString())}).ToList();

            string ListaMsjVentaEG = string.Join(Environment.NewLine, ListaVentaEG.Select(v => $"{v.Id}"));

            var ListaRechazos = BDDContexto.Rechazos
                .Select(v => new Ventas { 
                    Fecha = v.Fecha, 
                    Id = int.Parse(v.CodVendedor.ToString()), 
                    Venta = v.Venta , 
                    VentaEmpresa = v.VentaEmpresa
                })
                .ToList();

            string ListaMsjRechazos = string.Join(Environment.NewLine, ListaRechazos.Select(v => $"{v.Fecha} | {v.Id} | {v.Venta} | {v.VentaEmpresa}"));

            Console.WriteLine($"Vendedores con ventas de mas de 100000,00:\n{ListaMsjVentaMas}\n" +
                $"\nVendedores con ventas de menos de 100000,00:\n{ListaMsjVentaMenos}\n" +
                $"\nVendedores que vendieron a empresas grandes:\n{ListaMsjVentaEG}\n" +
                $"\nLista de rechazos:\n{ListaMsjRechazos}");
            #endregion
        }
    }
}
