using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Globalization;



namespace Parcial_ProgAVZ1.Clases
{
    /// <summary>
    /// Estructura que representa un estudiante para la serialización CSV
    /// </summary>
    public struct EstudianteData
    {
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public int Edad { get; set; }
        public string Sexo { get; set; }
        public double Nota1 { get; set; }
        public double Nota2 { get; set; }
        public double Nota3 { get; set; }
        public double Promedio { get; set; }
        public string ActividadesExtracurriculares { get; set; }
        public DateTime FechaRegistro { get; set; }
    }

    /// <summary>
    /// Clase que representa un estudiante en el sistema
    /// </summary>
    public class Estudiante
    {
        private readonly object HashCode;
        #region Propiedades

        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public int Edad { get; set; }
        public string Sexo { get; set; }
        public double[] Notas { get; set; }
        public double Promedio { get; private set; }
        public List<string> ActividadesExtracurriculares { get; set; }
        public DateTime FechaRegistro { get; set; }

        #endregion

        #region Constructores

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public Estudiante()
        {
            Notas = new double[3];
            ActividadesExtracurriculares = new List<string>();
            FechaRegistro = DateTime.Now;
        }

        /// <summary>
        /// Constructor con parámetros básicos
        /// </summary>
        public Estudiante(string nombre, string apellido, int edad, string sexo, double[] notas, List<string> actividades = null)
        {
            Nombre = nombre;
            Apellido = apellido;
            Edad = edad;
            Sexo = sexo;
            Notas = notas ?? new double[3];
            ActividadesExtracurriculares = actividades ?? new List<string>();
            FechaRegistro = DateTime.Now;
            CalcularPromedio();
        }

        /// <summary>
        /// Constructor desde EstudianteData (para deserialización desde CSV)
        /// </summary>
        public Estudiante(EstudianteData data)
        {
            Nombre = data.Nombre;
            Apellido = data.Apellido;
            Edad = data.Edad;
            Sexo = data.Sexo;
            Notas = new double[] { data.Nota1, data.Nota2, data.Nota3 };
            ActividadesExtracurriculares = string.IsNullOrEmpty(data.ActividadesExtracurriculares)
                ? new List<string>()
                : data.ActividadesExtracurriculares.Split(',').Select(a => a.Trim()).ToList();
            FechaRegistro = data.FechaRegistro;
            CalcularPromedio();
        }

        #endregion

        #region Métodos Públicos

        /// <summary>
        /// Calcula el promedio de las notas
        /// </summary>
        public void CalcularPromedio()
        {
            if (Notas != null && Notas.Length == 3)
            {
                Promedio = (Notas[0] + Notas[1] + Notas[2]) / 3.0;
            }
            else
            {
                Promedio = 0.0;
            }
        }

        /// <summary>
        /// Actualiza una nota específica y recalcula el promedio
        /// </summary>
        /// <param name="indiceNota">Índice de la nota (0, 1 o 2)</param>
        /// <param name="valor">Nuevo valor de la nota</param>
        public void ActualizarNota(int indiceNota, double valor)
        {
            if (indiceNota >= 0 && indiceNota < 3 && valor >= 0 && valor <= 5)
            {
                Notas[indiceNota] = valor;
                CalcularPromedio();
            }
            else
            {
                throw new ArgumentOutOfRangeException("La nota debe estar entre 0 y 5, y el índice entre 0 y 2.");
            }
        }

        /// <summary>
        /// Determina si el estudiante está aprobado
        /// </summary>
        /// <returns>True si está aprobado (promedio >= 3.0)</returns>
        public bool EstaAprobado()
        {
            return Promedio >= 3.0;
        }

        /// <summary>
        /// Agrega una actividad extracurricular
        /// </summary>
        /// <param name="actividad">Nombre de la actividad</param>
        public void AgregarActividad(string actividad)
        {
            if (!string.IsNullOrWhiteSpace(actividad) && !ActividadesExtracurriculares.Contains(actividad))
            {
                ActividadesExtracurriculares.Add(actividad);
            }
        }

        /// <summary>
        /// Elimina una actividad extracurricular
        /// </summary>
        /// <param name="actividad">Nombre de la actividad</param>
        public void EliminarActividad(string actividad)
        {
            ActividadesExtracurriculares.Remove(actividad);
        }

        /// <summary>
        /// Convierte el estudiante a EstudianteData para serialización CSV
        /// </summary>
        /// <returns>EstudianteData equivalente</returns>
        public EstudianteData ToEstudianteData()
        {
            return new EstudianteData
            {
                Nombre = this.Nombre,
                Apellido = this.Apellido,
                Edad = this.Edad,
                Sexo = this.Sexo,
                Nota1 = this.Notas[0],
                Nota2 = this.Notas[1],
                Nota3 = this.Notas[2],
                Promedio = this.Promedio,
                ActividadesExtracurriculares = string.Join(", ", this.ActividadesExtracurriculares),
                FechaRegistro = this.FechaRegistro
            };
        }

        /// <summary>
        /// Genera un resumen completo del estudiante
        /// </summary>
        /// <returns>String con todos los datos del estudiante</returns>
        public string GenerarResumen()
        {
            return $"Nombre: {Nombre} {Apellido}\n" +
                   $"Edad: {Edad} años\n" +
                   $"Sexo: {Sexo}\n" +
                   $"Notas: {Notas[0]:0.0}, {Notas[1]:0.0}, {Notas[2]:0.0}\n" +
                   $"Promedio: {Promedio:0.00}\n" +
                   $"Estado: {(EstaAprobado() ? "APROBADO" : "REPROBADO")}\n" +
                   $"Actividades Extracurriculares: {(ActividadesExtracurriculares.Any() ? string.Join(", ", ActividadesExtracurriculares) : "Ninguna")}\n" +
                   $"Fecha de registro: {FechaRegistro:yyyy-MM-dd HH:mm:ss}";
        }

        /// <summary>
        /// Genera datos para correo electrónico
        /// </summary>
        /// <returns>String formateado para email</returns>
        public string GenerarDatosParaCorreo()
        {
            string estado = EstaAprobado() ? "✅ APROBADO" : "❌ REPROBADO";
            string actividades = ActividadesExtracurriculares.Any()
                ? string.Join(", ", ActividadesExtracurriculares)
                : "Ninguna";

            return $"📚 REPORTE ACADÉMICO DEL ESTUDIANTE\n\n" +
                   $"👤 Estudiante: {Nombre} {Apellido}\n" +
                   $"🎂 Edad: {Edad} años\n" +
                   $"⚥ Sexo: {Sexo}\n\n" +
                   $"📊 CALIFICACIONES:\n" +
                   $"   • Nota 1: {Notas[0]:0.0}\n" +
                   $"   • Nota 2: {Notas[1]:0.0}\n" +
                   $"   • Nota 3: {Notas[2]:0.0}\n" +
                   $"   • Promedio: {Promedio:0.00}\n\n" +
                   $"🏆 Estado Académico: {estado}\n\n" +
                   $"🎯 Actividades Extracurriculares: {actividades}\n\n" +
                   $"📅 Fecha de registro: {FechaRegistro:dd/MM/yyyy HH:mm}\n\n" +
                   $"---\n" +
                   $"Sistema de Gestión de Estudiantes\n" +
                   $"Generado automáticamente el {DateTime.Now:dd/MM/yyyy HH:mm:ss}";
        }

        /// <summary>
        /// Valida si los datos del estudiante son correctos
        /// </summary>
        /// <returns>Lista de errores de validación</returns>
        public List<string> ValidarDatos()
        {
            var errores = new List<string>();

            if (string.IsNullOrWhiteSpace(Nombre))
                errores.Add("El nombre es obligatorio");

            if (string.IsNullOrWhiteSpace(Apellido))
                errores.Add("El apellido es obligatorio");

            if (Edad <= 0 || Edad > 80)
                errores.Add("La edad debe estar entre 1 y 80 años");

            if (string.IsNullOrWhiteSpace(Sexo))
                errores.Add("El sexo es obligatorio");

            if (Notas == null || Notas.Length != 3)
                errores.Add("Debe tener exactamente 3 notas");
            else
            {
                for (int i = 0; i < Notas.Length; i++)
                {
                    if (Notas[i] < 0 || Notas[i] > 5)
                        errores.Add($"La nota {i + 1} debe estar entre 0 y 5");
                }
            }

            return errores;
        }

        #endregion

        #region Overrides

        public override string ToString()
        {
            return $"{Nombre} {Apellido} - Promedio: {Promedio:0.00}";
        }

        public override bool Equals(object obj)
        {
            if (obj is Estudiante otro)
            {
                return this.Nombre == otro.Nombre &&
                       this.Apellido == otro.Apellido &&
                       this.FechaRegistro == otro.FechaRegistro;
            }
            return false;
        }

     #endregion

        #region Métodos Estáticos

        /// <summary>
        /// Convierte una lista de EstudianteData a lista de Estudiante
        /// </summary>
        /// <param name="datosEstudiantes">Lista de EstudianteData</param>
        /// <returns>Lista de Estudiante</returns>
        public static List<Estudiante> FromEstudianteDataList(List<EstudianteData> datosEstudiantes)
        {
            return datosEstudiantes.Select(data => new Estudiante(data)).ToList();
        }

        /// <summary>
        /// Convierte una lista de Estudiante a lista de EstudianteData
        /// </summary>
        /// <param name="estudiantes">Lista de Estudiante</param>
        /// <returns>Lista de EstudianteData</returns>
        public static List<EstudianteData> ToEstudianteDataList(List<Estudiante> estudiantes)
        {
            return estudiantes.Select(est => est.ToEstudianteData()).ToList();
        }

        #endregion
    }

    /// <summary>
    /// Extensiones para trabajar con listas de estudiantes
    /// </summary>
    public static class EstudianteExtensions
    {
        /// <summary>
        /// Obtiene estudiantes aprobados
        /// </summary>
        public static List<Estudiante> Aprobados(this List<Estudiante> estudiantes)
        {
            return estudiantes.Where(e => e.EstaAprobado()).ToList();
        }

        /// <summary>
        /// Obtiene estudiantes reprobados
        /// </summary>
        public static List<Estudiante> Reprobados(this List<Estudiante> estudiantes)
        {
            return estudiantes.Where(e => !e.EstaAprobado()).ToList();
        }

        /// <summary>
        /// Calcula el promedio general de todos los estudiantes
        /// </summary>
        public static double PromedioGeneral(this List<Estudiante> estudiantes)
        {
            return estudiantes.Any() ? estudiantes.Average(e => e.Promedio) : 0.0;
        }

        /// <summary>
        /// Busca estudiantes por nombre o apellido
        /// </summary>
        public static List<Estudiante> BuscarPorNombre(this List<Estudiante> estudiantes, string busqueda)
        {
            if (string.IsNullOrWhiteSpace(busqueda))
                return estudiantes;

            return estudiantes.Where(e =>
                e.Nombre.ToLower().Contains(busqueda.ToLower()) ||
                e.Apellido.ToLower().Contains(busqueda.ToLower())
            ).ToList();
        }
    }

    /// <summary>
    /// Clase simple para manejo de archivos CSV (versión integrada)
    /// </summary>
    public static class CsvSerializer
    {
        /// <summary>
        /// Guarda una lista de estudiantes en un archivo CSV
        /// </summary>
        public static bool GuardarEstudiantesCSV(List<EstudianteData> estudiantes, string rutaArchivo)
        {
            try
            {
                string directorio = Path.GetDirectoryName(rutaArchivo);
                if (!Directory.Exists(directorio))
                {
                    Directory.CreateDirectory(directorio);
                }

                using (StreamWriter writer = new StreamWriter(rutaArchivo, false, Encoding.UTF8))
                {
                    writer.WriteLine("Nombre,Apellido,Edad,Sexo,Nota1,Nota2,Nota3,Promedio,ActividadesExtracurriculares,FechaRegistro");

                    foreach (var estudiante in estudiantes)
                    {
                        string linea = $"{EscaparCampoCSV(estudiante.Nombre)}," +
                                      $"{EscaparCampoCSV(estudiante.Apellido)}," +
                                      $"{estudiante.Edad}," +
                                      $"{EscaparCampoCSV(estudiante.Sexo)}," +
                                      $"{estudiante.Nota1.ToString("0.00", CultureInfo.InvariantCulture)}," +
                                      $"{estudiante.Nota2.ToString("0.00", CultureInfo.InvariantCulture)}," +
                                      $"{estudiante.Nota3.ToString("0.00", CultureInfo.InvariantCulture)}," +
                                      $"{estudiante.Promedio.ToString("0.00", CultureInfo.InvariantCulture)}," +
                                      $"{EscaparCampoCSV(estudiante.ActividadesExtracurriculares)}," +
                                      $"{estudiante.FechaRegistro.ToString("yyyy-MM-dd HH:mm:ss")}";

                        writer.WriteLine(linea);
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Carga una lista de estudiantes desde un archivo CSV
        /// </summary>
        public static List<EstudianteData> CargarEstudiantesCSV(string rutaArchivo)
        {
            List<EstudianteData> estudiantes = new List<EstudianteData>();

            try
            {
                if (!File.Exists(rutaArchivo))
                    return estudiantes;

                using (StreamReader reader = new StreamReader(rutaArchivo, Encoding.UTF8))
                {
                    reader.ReadLine(); // Saltar encabezados

                    string linea;
                    while ((linea = reader.ReadLine()) != null)
                    {
                        if (string.IsNullOrWhiteSpace(linea))
                            continue;

                        try
                        {
                            var campos = linea.Split(',');
                            if (campos.Length >= 10)
                            {
                                estudiantes.Add(new EstudianteData
                                {
                                    Nombre = campos[0].Trim('"'),
                                    Apellido = campos[1].Trim('"'),
                                    Edad = int.Parse(campos[2]),
                                    Sexo = campos[3].Trim('"'),
                                    Nota1 = double.Parse(campos[4], CultureInfo.InvariantCulture),
                                    Nota2 = double.Parse(campos[5], CultureInfo.InvariantCulture),
                                    Nota3 = double.Parse(campos[6], CultureInfo.InvariantCulture),
                                    Promedio = double.Parse(campos[7], CultureInfo.InvariantCulture),
                                    ActividadesExtracurriculares = campos[8].Trim('"'),
                                    FechaRegistro = DateTime.ParseExact(campos[9], "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)
                                });
                            }
                        }
                        catch
                        {
                            // Ignorar líneas con errores
                        }
                    }
                }
            }
            catch
            {
                // Fallar silenciosamente
            }

            return estudiantes;
        }

        /// <summary>
        /// Agrega un estudiante al final del archivo CSV
        /// </summary>
        public static bool AgregarEstudianteCSV(EstudianteData estudiante, string rutaArchivo)
        {
            try
            {
                string directorio = Path.GetDirectoryName(rutaArchivo);
                if (!Directory.Exists(directorio))
                {
                    Directory.CreateDirectory(directorio);
                }

                bool archivoExiste = File.Exists(rutaArchivo);

                using (StreamWriter writer = new StreamWriter(rutaArchivo, true, Encoding.UTF8))
                {
                    if (!archivoExiste)
                    {
                        writer.WriteLine("Nombre,Apellido,Edad,Sexo,Nota1,Nota2,Nota3,Promedio,ActividadesExtracurriculares,FechaRegistro");
                    }

                    string linea = $"{EscaparCampoCSV(estudiante.Nombre)}," +
                                  $"{EscaparCampoCSV(estudiante.Apellido)}," +
                                  $"{estudiante.Edad}," +
                                  $"{EscaparCampoCSV(estudiante.Sexo)}," +
                                  $"{estudiante.Nota1.ToString("0.00", CultureInfo.InvariantCulture)}," +
                                  $"{estudiante.Nota2.ToString("0.00", CultureInfo.InvariantCulture)}," +
                                  $"{estudiante.Nota3.ToString("0.00", CultureInfo.InvariantCulture)}," +
                                  $"{estudiante.Promedio.ToString("0.00", CultureInfo.InvariantCulture)}," +
                                  $"{EscaparCampoCSV(estudiante.ActividadesExtracurriculares)}," +
                                  $"{estudiante.FechaRegistro.ToString("yyyy-MM-dd HH:mm:ss")}";

                    writer.WriteLine(linea);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        private static string EscaparCampoCSV(string campo)
        {
            if (string.IsNullOrEmpty(campo))
                return "";

            if (campo.Contains(",") || campo.Contains("\""))
            {
                campo = campo.Replace("\"", "\"\"");
                return $"\"{campo}\"";
            }
            return campo;
        }
    }
}