using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Parcial_ProgAVZ1.Clases;

namespace Parcial_ProgAVZ1.Clases
{
    /// <summary>
    /// Clase de utilidades y funciones helper para el sistema
    /// </summary>
    public static class Helpers
    {
        #region Validaciones

        /// <summary>
        /// Valida si un email tiene formato válido
        /// </summary>
        /// <param name="email">Email a validar</param>
        /// <returns>True si es válido</returns>
        public static bool EsEmailValido(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                // Usar regex para validación más estricta
                string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
                return Regex.IsMatch(email, pattern);
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Valida si un dominio de email está permitido
        /// </summary>
        /// <param name="email">Email completo</param>
        /// <param name="dominiosPermitidos">Lista de dominios permitidos</param>
        /// <returns>True si el dominio está permitido</returns>
        public static bool EsDominioPermitido(string email, HashSet<string> dominiosPermitidos)
        {
            try
            {
                if (!EsEmailValido(email))
                    return false;

                string dominio = email.Split('@')[1].ToLower();
                return dominiosPermitidos.Contains(dominio);
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Valida si una nota está en el rango correcto
        /// </summary>
        /// <param name="nota">Nota a validar</param>
        /// <param name="minimo">Valor mínimo (default: 0.0)</param>
        /// <param name="maximo">Valor máximo (default: 5.0)</param>
        /// <returns>True si está en rango</returns>
        public static bool EsNotaValida(double nota, double minimo = 0.0, double maximo = 5.0)
        {
            return nota >= minimo && nota <= maximo;
        }

        /// <summary>
        /// Valida si una edad está en rango válido
        /// </summary>
        /// <param name="edad">Edad a validar</param>
        /// <param name="minimo">Edad mínima (default: 1)</param>
        /// <param name="maximo">Edad máxima (default: 120)</param>
        /// <returns>True si está en rango</returns>
        public static bool EsEdadValida(int edad, int minimo = 1, int maximo = 120)
        {
            return edad >= minimo && edad <= maximo;
        }

        #endregion

        #region Formateo y Conversiones

        /// <summary>
        /// Formatea un tamaño de archivo en bytes a formato legible
        /// </summary>
        /// <param name="bytes">Tamaño en bytes</param>
        /// <returns>String formateado (ej: "1.5 MB")</returns>
        public static string FormatearTamañoArchivo(long bytes)
        {
            string[] sizes = { "B", "KB", "MB", "GB", "TB" };
            double len = bytes;
            int order = 0;

            while (len >= 1024 && order < sizes.Length - 1)
            {
                order++;
                len = len / 1024;
            }

            return $"{len:0.##} {sizes[order]}";
        }

        /// <summary>
        /// Convierte un promedio a letra de calificación
        /// </summary>
        /// <param name="promedio">Promedio numérico</param>
        /// <returns>Letra de calificación</returns>
        public static string PromedioALetra(double promedio)
        {
            if (promedio >= 4.5) return "A";
            if (promedio >= 4.0) return "B+";
            if (promedio >= 3.5) return "B";
            if (promedio >= 3.0) return "C+";
            if (promedio >= 2.5) return "C";
            if (promedio >= 2.0) return "D";
            return "F";
        }

        /// <summary>
        /// Convierte un estado académico a emoji
        /// </summary>
        /// <param name="promedio">Promedio del estudiante</param>
        /// <returns>Emoji representativo</returns>
        public static string EstadoAEmoji(double promedio)
        {
            if (promedio >= 4.5) return "🏆"; // Excelente
            if (promedio >= 4.0) return "⭐"; // Muy bueno
            if (promedio >= 3.5) return "✅"; // Bueno
            if (promedio >= 3.0) return "👍"; // Aprobado
            return "❌"; // Reprobado
        }

        #endregion

        #region Manejo de Archivos y Rutas

        /// <summary>
        /// Crea un directorio si no existe
        /// </summary>
        /// <param name="ruta">Ruta del directorio</param>
        /// <returns>True si se creó o ya existía</returns>
        public static bool CrearDirectorioSiNoExiste(string ruta)
        {
            try
            {
                if (!Directory.Exists(ruta))
                {
                    Directory.CreateDirectory(ruta);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Obtiene una ruta segura para un archivo, evitando sobrescribir
        /// </summary>
        /// <param name="rutaBase">Ruta base del archivo</param>
        /// <returns>Ruta única</returns>
        public static string ObtenerRutaUnica(string rutaBase)
        {
            if (!File.Exists(rutaBase))
                return rutaBase;

            string directorio = Path.GetDirectoryName(rutaBase);
            string nombreArchivo = Path.GetFileNameWithoutExtension(rutaBase);
            string extension = Path.GetExtension(rutaBase);

            int contador = 1;
            string nuevaRuta;

            do
            {
                nuevaRuta = Path.Combine(directorio, $"{nombreArchivo}_{contador}{extension}");
                contador++;
            }
            while (File.Exists(nuevaRuta));

            return nuevaRuta;
        }

        /// <summary>
        /// Hace una copia de respaldo de un archivo
        /// </summary>
        /// <param name="rutaArchivo">Archivo a respaldar</param>
        /// <returns>Ruta del archivo de respaldo creado</returns>
        public static string CrearRespaldo(string rutaArchivo)
        {
            try
            {
                if (!File.Exists(rutaArchivo))
                    return null;

                string directorio = Path.GetDirectoryName(rutaArchivo);
                string nombreArchivo = Path.GetFileNameWithoutExtension(rutaArchivo);
                string extension = Path.GetExtension(rutaArchivo);
                string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");

                string rutaRespaldo = Path.Combine(directorio, "Respaldos", $"{nombreArchivo}_backup_{timestamp}{extension}");

                CrearDirectorioSiNoExiste(Path.GetDirectoryName(rutaRespaldo));
                File.Copy(rutaArchivo, rutaRespaldo);

                return rutaRespaldo;
            }
            catch
            {
                return null;
            }
        }

        #endregion

        #region Reportes y Exportación

        /// <summary>
        /// Genera un reporte HTML de estudiantes
        /// </summary>
        /// <param name="estudiantes">Lista de estudiantes</param>
        /// <param name="titulo">Título del reporte</param>
        /// <returns>HTML del reporte</returns>
        public static string GenerarReporteHTML(List<Estudiante> estudiantes, string titulo = "Reporte de Estudiantes")
        {
            var html = new StringBuilder();

            html.AppendLine("<!DOCTYPE html>");
            html.AppendLine("<html><head>");
            html.AppendLine("<meta charset='UTF-8'>");
            html.AppendLine($"<title>{titulo}</title>");
            html.AppendLine("<style>");
            html.AppendLine("body { font-family: Arial, sans-serif; margin: 20px; }");
            html.AppendLine("table { border-collapse: collapse; width: 100%; }");
            html.AppendLine("th, td { border: 1px solid #ddd; padding: 8px; text-align: left; }");
            html.AppendLine("th { background-color: #f2f2f2; }");
            html.AppendLine(".aprobado { background-color: #d4edda; }");
            html.AppendLine(".reprobado { background-color: #f8d7da; }");
            html.AppendLine(".header { text-align: center; margin-bottom: 20px; }");
            html.AppendLine("</style></head><body>");

            html.AppendLine($"<div class='header'>");
            html.AppendLine($"<h1>{titulo}</h1>");
            html.AppendLine($"<p>Generado el {DateTime.Now:dd/MM/yyyy HH:mm:ss}</p>");
            html.AppendLine($"<p>Total de estudiantes: {estudiantes.Count}</p>");
            html.AppendLine("</div>");

            if (estudiantes.Count > 0)
            {
                html.AppendLine("<table>");
                html.AppendLine("<thead>");
                html.AppendLine("<tr>");
                html.AppendLine("<th>Nombre</th><th>Apellido</th><th>Edad</th><th>Sexo</th>");
                html.AppendLine("<th>Nota 1</th><th>Nota 2</th><th>Nota 3</th><th>Promedio</th>");
                html.AppendLine("<th>Estado</th><th>Actividades</th>");
                html.AppendLine("</tr></thead><tbody>");

                foreach (var estudiante in estudiantes)
                {
                    string claseEstado = estudiante.EstaAprobado() ? "aprobado" : "reprobado";
                    string estado = estudiante.EstaAprobado() ? "APROBADO" : "REPROBADO";
                    string actividades = estudiante.ActividadesExtracurriculares.Count > 0
                        ? string.Join(", ", estudiante.ActividadesExtracurriculares)
                        : "Ninguna";

                    html.AppendLine($"<tr class='{claseEstado}'>");
                    html.AppendLine($"<td>{estudiante.Nombre}</td>");
                    html.AppendLine($"<td>{estudiante.Apellido}</td>");
                    html.AppendLine($"<td>{estudiante.Edad}</td>");
                    html.AppendLine($"<td>{estudiante.Sexo}</td>");
                    html.AppendLine($"<td>{estudiante.Notas[0]:0.0}</td>");
                    html.AppendLine($"<td>{estudiante.Notas[1]:0.0}</td>");
                    html.AppendLine($"<td>{estudiante.Notas[2]:0.0}</td>");
                    html.AppendLine($"<td>{estudiante.Promedio:0.00}</td>");
                    html.AppendLine($"<td>{estado}</td>");
                    html.AppendLine($"<td>{actividades}</td>");
                    html.AppendLine("</tr>");
                }

                html.AppendLine("</tbody></table>");

                // Estadísticas
                int aprobados = estudiantes.Count(e => e.EstaAprobado());
                int reprobados = estudiantes.Count - aprobados;
                double promedioGeneral = estudiantes.Average(e => e.Promedio);

                html.AppendLine("<div style='margin-top: 20px;'>");
                html.AppendLine("<h3>Estadísticas</h3>");
                html.AppendLine($"<p><strong>Estudiantes aprobados:</strong> {aprobados} ({(aprobados * 100.0 / estudiantes.Count):0.1}%)</p>");
                html.AppendLine($"<p><strong>Estudiantes reprobados:</strong> {reprobados} ({(reprobados * 100.0 / estudiantes.Count):0.1}%)</p>");
                html.AppendLine($"<p><strong>Promedio general:</strong> {promedioGeneral:0.00}</p>");
                html.AppendLine("</div>");
            }
            else
            {
                html.AppendLine("<p>No hay estudiantes registrados.</p>");
            }

            html.AppendLine("</body></html>");
            return html.ToString();
        }

        /// <summary>
        /// Exporta estudiantes a formato CSV personalizado
        /// </summary>
        /// <param name="estudiantes">Lista de estudiantes</param>
        /// <param name="rutaArchivo">Ruta donde guardar</param>
        /// <param name="incluirEncabezados">Si incluir encabezados</param>
        /// <returns>True si se exportó correctamente</returns>
        public static bool ExportarACSVPersonalizado(List<Estudiante> estudiantes, string rutaArchivo, bool incluirEncabezados = true)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(rutaArchivo, false, Encoding.UTF8))
                {
                    if (incluirEncabezados)
                    {
                        writer.WriteLine("Nombre Completo,Edad,Sexo,Promedio,Estado,Letra,Actividades,Fecha Registro");
                    }

                    foreach (var estudiante in estudiantes)
                    {
                        string nombreCompleto = $"{estudiante.Nombre} {estudiante.Apellido}";
                        string estado = estudiante.EstaAprobado() ? "APROBADO" : "REPROBADO";
                        string letra = PromedioALetra(estudiante.Promedio);
                        string actividades = estudiante.ActividadesExtracurriculares.Count > 0
                            ? string.Join("; ", estudiante.ActividadesExtracurriculares)
                            : "Ninguna";

                        writer.WriteLine($"\"{nombreCompleto}\",{estudiante.Edad},{estudiante.Sexo}," +
                                       $"{estudiante.Promedio:0.00},{estado},{letra},\"{actividades}\"," +
                                       $"{estudiante.FechaRegistro:yyyy-MM-dd}");
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        #endregion

        #region Configuración y Logs

        /// <summary>
        /// Registra un evento en el log del sistema
        /// </summary>
        /// <param name="mensaje">Mensaje a registrar</param>
        /// <param name="tipo">Tipo de evento (INFO, ERROR, WARNING)</param>
        public static void RegistrarLog(string mensaje, string tipo = "INFO")
        {
            try
            {
                string carpetaLogs = Path.Combine(Application.StartupPath, "Logs");
                CrearDirectorioSiNoExiste(carpetaLogs);

                string archivoLog = Path.Combine(carpetaLogs, $"sistema_{DateTime.Now:yyyyMM}.log");
                string entrada = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} [{tipo}] {mensaje}";

                File.AppendAllText(archivoLog, entrada + Environment.NewLine);
            }
            catch
            {
                // No hacer nada si falla el logging para evitar errores en cascada
            }
        }

        /// <summary>
        /// Carga configuración desde app.config
        /// </summary>
        /// <param name="clave">Clave de configuración</param>
        /// <param name="valorPorDefecto">Valor por defecto si no existe</param>
        /// <returns>Valor de configuración</returns>
      

        /// <summary>
        /// Guarda configuración (requiere app.config modificable)
        /// </summary>
        /// <param name="clave">Clave de configuración</param>
        /// <param name="valor">Valor a guardar</param>
     

        #endregion

        #region Utilidades de Interfaz

        /// <summary>
        /// Centra un formulario en la pantalla
        /// </summary>
        /// <param name="form">Formulario a centrar</param>
        public static void CentrarFormulario(Form form)
        {
            form.StartPosition = FormStartPosition.Manual;
            form.Location = new System.Drawing.Point(
                (Screen.PrimaryScreen.WorkingArea.Width - form.Width) / 2,
                (Screen.PrimaryScreen.WorkingArea.Height - form.Height) / 2);
        }

        /// <summary>
        /// Muestra un mensaje de confirmación personalizado
        /// </summary>
        /// <param name="mensaje">Mensaje a mostrar</param>
        /// <param name="titulo">Título de la ventana</param>
        /// <returns>True si el usuario confirma</returns>
        public static bool ConfirmarAccion(string mensaje, string titulo = "Confirmar")
        {
            return MessageBox.Show(mensaje, titulo, MessageBoxButtons.YesNo,
                                 MessageBoxIcon.Question) == DialogResult.Yes;
        }

        /// <summary>
        /// Muestra un mensaje de éxito
        /// </summary>
        /// <param name="mensaje">Mensaje a mostrar</param>
        /// <param name="titulo">Título de la ventana</param>
        public static void MostrarExito(string mensaje, string titulo = "Éxito")
        {
            MessageBox.Show(mensaje, titulo, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// Muestra un mensaje de error
        /// </summary>
        /// <param name="mensaje">Mensaje a mostrar</param>
        /// <param name="titulo">Título de la ventana</param>
        public static void MostrarError(string mensaje, string titulo = "Error")
        {
            MessageBox.Show(mensaje, titulo, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// Muestra un mensaje de advertencia
        /// </summary>
        /// <param name="mensaje">Mensaje a mostrar</param>
        /// <param name="titulo">Título de la ventana</param>
        public static void MostrarAdvertencia(string mensaje, string titulo = "Advertencia")
        {
            MessageBox.Show(mensaje, titulo, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        /// <summary>
        /// Valida si un archivo CSV tiene la estructura correcta de estudiantes
        /// </summary>
        /// <param name="rutaArchivo">Ruta del archivo CSV</param>
        /// <returns>Resultado de la validación con detalles</returns>
        public static ValidationResult ValidarArchivoCSV(string rutaArchivo)
        {
            var resultado = new ValidationResult();

            try
            {
                if (!File.Exists(rutaArchivo))
                {
                    resultado.EsValido = false;
                    resultado.Errores.Add("El archivo no existe");
                    return resultado;
                }

                // Validar usando la DLL
                var datos = CsvSerializer.CargarEstudiantesCSV(rutaArchivo);
                resultado.RegistrosEncontrados = datos.Count;

                // Validar cada registro
                int lineaActual = 1;
                foreach (var dato in datos)
                {
                    lineaActual++;

                    if (string.IsNullOrWhiteSpace(dato.Nombre))
                        resultado.Errores.Add($"Línea {lineaActual}: Nombre vacío");

                    if (string.IsNullOrWhiteSpace(dato.Apellido))
                        resultado.Errores.Add($"Línea {lineaActual}: Apellido vacío");

                    if (!EsEdadValida(dato.Edad))
                        resultado.Errores.Add($"Línea {lineaActual}: Edad inválida ({dato.Edad})");

                    if (!EsNotaValida(dato.Nota1))
                        resultado.Errores.Add($"Línea {lineaActual}: Nota 1 inválida ({dato.Nota1})");

                    if (!EsNotaValida(dato.Nota2))
                        resultado.Errores.Add($"Línea {lineaActual}: Nota 2 inválida ({dato.Nota2})");

                    if (!EsNotaValida(dato.Nota3))
                        resultado.Errores.Add($"Línea {lineaActual}: Nota 3 inválida ({dato.Nota3})");
                }

                resultado.EsValido = resultado.Errores.Count == 0;
            }
            catch (Exception ex)
            {
                resultado.EsValido = false;
                resultado.Errores.Add($"Error al validar archivo: {ex.Message}");
            }

            return resultado;
        }

        #endregion

        #region Búsqueda y Filtros

        /// <summary>
        /// Busca estudiantes por múltiples criterios
        /// </summary>
        /// <param name="estudiantes">Lista de estudiantes</param>
        /// <param name="criterios">Criterios de búsqueda</param>
        /// <returns>Lista filtrada</returns>
        public static List<Estudiante> BuscarPorCriterios(List<Estudiante> estudiantes, SearchCriteria criterios)
        {
            var resultado = estudiantes.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(criterios.Nombre))
            {
                resultado = resultado.Where(e =>
                    e.Nombre.ToLower().Contains(criterios.Nombre.ToLower()) ||
                    e.Apellido.ToLower().Contains(criterios.Nombre.ToLower()));
            }

            if (criterios.EdadMinima.HasValue)
            {
                resultado = resultado.Where(e => e.Edad >= criterios.EdadMinima.Value);
            }

            if (criterios.EdadMaxima.HasValue)
            {
                resultado = resultado.Where(e => e.Edad <= criterios.EdadMaxima.Value);
            }

            if (!string.IsNullOrWhiteSpace(criterios.Sexo))
            {
                resultado = resultado.Where(e =>
                    e.Sexo.Equals(criterios.Sexo, StringComparison.OrdinalIgnoreCase));
            }

            if (criterios.PromedioMinimo.HasValue)
            {
                resultado = resultado.Where(e => e.Promedio >= criterios.PromedioMinimo.Value);
            }

            if (criterios.PromedioMaximo.HasValue)
            {
                resultado = resultado.Where(e => e.Promedio <= criterios.PromedioMaximo.Value);
            }

            if (criterios.SoloAprobados.HasValue)
            {
                if (criterios.SoloAprobados.Value)
                    resultado = resultado.Where(e => e.EstaAprobado());
                else
                    resultado = resultado.Where(e => !e.EstaAprobado());
            }

            return resultado.ToList();
        }

        #endregion
    }

    #region Clases de Apoyo

    /// <summary>
    /// Resultado de validación con detalles
    /// </summary>
    public class ValidationResult
    {
        public bool EsValido { get; set; } = true;
        public List<string> Errores { get; set; } = new List<string>();
        public int RegistrosEncontrados { get; set; } = 0;

        public string ResumenErrores => string.Join("\n", Errores);
    }

    /// <summary>
    /// Criterios de búsqueda para estudiantes
    /// </summary>
    public class SearchCriteria
    {
        public string Nombre { get; set; }
        public int? EdadMinima { get; set; }
        public int? EdadMaxima { get; set; }
        public string Sexo { get; set; }
        public double? PromedioMinimo { get; set; }
        public double? PromedioMaximo { get; set; }
        public bool? SoloAprobados { get; set; }
    }

    #endregion
}