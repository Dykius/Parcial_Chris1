using Parcial_ProgAVZ1.Clases;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Parcial_ProgAVZ1
{
    public partial class Form1 : Form
    {
        #region Variables Privadas

        // Lista principal de estudiantes
        private List<Estudiante> estudiantes;
        private int estudianteSeleccionado = -1;

        // Ruta del archivo CSV
        private string rutaArchivoCSV;

        // CheckBoxes para actividades extracurriculares (asumiendo que existen en el diseño)
        private CheckBox chkArte, chkDeporte, chkMusica, chkNoAplica;

        #endregion

        #region Constructor y Inicialización

        public Form1()
        {
            InitializeComponent();
            InicializarComponentes();
            CargarDatosDesdeCSV();
        }

        private void InicializarComponentes()
        {
            estudiantes = new List<Estudiante>();

            // Definir la ruta del archivo CSV
            string carpetaArchivos = Path.Combine(Application.StartupPath, "Archivos");
            if (!Directory.Exists(carpetaArchivos))
            {
                Directory.CreateDirectory(carpetaArchivos);
            }
            rutaArchivoCSV = Path.Combine(carpetaArchivos, "estudiantes.csv");

            // Configurar el ListBox
            studentslist.DisplayMember = "ToString";
            studentslist.DoubleClick += StudentslistDoubleClick;

            // Configurar eventos para cálculo automático de promedio
            not1txt.TextChanged += CalcularPromedioEnTiempoReal;
            not2txt.TextChanged += CalcularPromedioEnTiempoReal;
            not3txt.TextChanged += CalcularPromedioEnTiempoReal;

            // Inicializar CheckBoxes si existen en el diseño
            InicializarCheckBoxes();

            // Limpiar campos al inicio
            LimpiarCampos();

            // Configurar el formulario para que se cierre correctamente
            this.FormClosing += Form1_FormClosing;
        }

        private void InicializarCheckBoxes()
        {
            // Buscar CheckBoxes en el formulario
            // Si no existen en el diseño, los crear dinámicamente o manejar de otra manera
            chkArte = this.Controls.Find("chkArte", true).FirstOrDefault() as CheckBox;
            chkDeporte = this.Controls.Find("chkDeporte", true).FirstOrDefault() as CheckBox;
            chkMusica = this.Controls.Find("chkMusica", true).FirstOrDefault() as CheckBox;
            chkNoAplica = this.Controls.Find("chkNoAplica", true).FirstOrDefault() as CheckBox;

            // Si no se encuentran, crear temporalmente (deberías agregarlos al diseño)
            if (chkArte == null || chkDeporte == null || chkMusica == null || chkNoAplica == null)
            {
                // Aquí podrías crear los CheckBoxes dinámicamente si no están en el diseño
                // O simplemente manejar las actividades extracurriculares de otra manera
            }
        }

        #endregion

        #region Carga y Guardado de Datos CSV

        private void CargarDatosDesdeCSV()
        {
            try
            {
                var datosCSV = CsvSerializer.CargarEstudiantesCSV(rutaArchivoCSV);
                estudiantes = Estudiante.FromEstudianteDataList(datosCSV);
                ActualizarListaEstudiantes();

                if (estudiantes.Count > 0)
                {
                    this.Text = $"Sistema Gestión de Estudiantes - {estudiantes.Count} estudiante(s) cargado(s)";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar datos desde CSV: {ex.Message}", "Error de Carga",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private bool GuardarDatosEnCSV()
        {
            try
            {
                var datosCSV = Estudiante.ToEstudianteDataList(estudiantes);
                bool resultado = CsvSerializer.GuardarEstudiantesCSV(datosCSV, rutaArchivoCSV);

                if (resultado)
                {
                    this.Text = $"Sistema Gestión de Estudiantes - {estudiantes.Count} estudiante(s) - Guardado ✓";
                }

                return resultado;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar datos en CSV: {ex.Message}", "Error de Guardado",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Guardar automáticamente al cerrar
            if (estudiantes.Count > 0)
            {
                GuardarDatosEnCSV();
            }
        }

        #endregion

        #region Eventos de Controles Básicos

        private void btnminimaze_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void btnclose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnsendmail_Click(object sender, EventArgs e)
        {
            // Verificar si hay un estudiante seleccionado
            Estudiante estudianteParaCorreo = null;

            if (estudianteSeleccionado >= 0 && estudianteSeleccionado < estudiantes.Count)
            {
                estudianteParaCorreo = estudiantes[estudianteSeleccionado];
            }

            // Abrir formulario de correos
            Frm_correos frmCorreos = new Frm_correos(estudianteParaCorreo);
            frmCorreos.ShowDialog(); // Usar ShowDialog para modal, o Show() para no modal
        }

        #endregion

        #region Eventos de TextBoxes

        private void not1txt_TextChanged(object sender, EventArgs e)
        {
            // El cálculo se maneja en CalcularPromedioEnTiempoReal
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Ya se carga en el constructor, pero si necesitas algo adicional aquí
        }

        private void not2txt_TextChanged(object sender, EventArgs e)
        {
            // El cálculo se maneja en CalcularPromedioEnTiempoReal
        }

        private void not3txt_TextChanged(object sender, EventArgs e)
        {
            // El cálculo se maneja en CalcularPromedioEnTiempoReal
        }

        private void Averagetxt_TextChanged(object sender, EventArgs e)
        {
            // Campo de solo lectura
        }

        private void nametxt_TextChanged(object sender, EventArgs e)
        {
            // Campo de entrada - sin lógica específica
        }

        private void lastnametxt_TextChanged(object sender, EventArgs e)
        {
            // Campo de entrada - sin lógica específica
        }

        private void agetxt_TextChanged(object sender, EventArgs e)
        {
            // Campo de entrada - sin lógica específica
        }

        #endregion

        #region Eventos de Botones CRUD

        private void btnadd_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValidarCampos())
                {
                    var nuevoEstudiante = new Estudiante();

                    // Obtener datos básicos
                    nuevoEstudiante.Nombre = nametxt.Text.Trim();
                    nuevoEstudiante.Apellido = lastnametxt.Text.Trim();
                    nuevoEstudiante.Edad = int.Parse(agetxt.Text);

                    // Obtener sexo
                    nuevoEstudiante.Sexo = rdbtnM.Checked ? "Masculino" : "Femenino";

                    // Obtener notas
                    nuevoEstudiante.Notas = new double[3];
                    nuevoEstudiante.Notas[0] = double.Parse(not1txt.Text);
                    nuevoEstudiante.Notas[1] = double.Parse(not2txt.Text);
                    nuevoEstudiante.Notas[2] = double.Parse(not3txt.Text);

                    // Calcular promedio
                    nuevoEstudiante.CalcularPromedio();

                    // Obtener actividades extracurriculares
                    nuevoEstudiante.ActividadesExtracurriculares = ObtenerActividadesSeleccionadas();

                    // Validar datos del estudiante
                    var errores = nuevoEstudiante.ValidarDatos();
                    if (errores.Count > 0)
                    {
                        MessageBox.Show($"Errores de validación:\n{string.Join("\n", errores)}",
                                      "Error de Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // Agregar estudiante a la lista
                    estudiantes.Add(nuevoEstudiante);

                    // Guardar en CSV
                    if (CsvSerializer.AgregarEstudianteCSV(nuevoEstudiante.ToEstudianteData(), rutaArchivoCSV))
                    {
                        // Actualizar interfaz
                        ActualizarListaEstudiantes();
                        LimpiarCampos();

                        MessageBox.Show($"Estudiante {nuevoEstudiante.Nombre} {nuevoEstudiante.Apellido} registrado exitosamente!",
                                      "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        // Remover de la lista si no se pudo guardar en CSV
                        estudiantes.Remove(nuevoEstudiante);
                        MessageBox.Show("Error al guardar el estudiante en el archivo CSV.", "Error",
                                      MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al registrar estudiante: {ex.Message}", "Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnedit_Click(object sender, EventArgs e)
        {
            try
            {
                if (estudianteSeleccionado >= 0 && estudianteSeleccionado < estudiantes.Count)
                {
                    if (ValidarCampos())
                    {
                        var estudiante = estudiantes[estudianteSeleccionado];

                        // Actualizar datos básicos
                        estudiante.Nombre = nametxt.Text.Trim();
                        estudiante.Apellido = lastnametxt.Text.Trim();
                        estudiante.Edad = int.Parse(agetxt.Text);
                        estudiante.Sexo = rdbtnM.Checked ? "Masculino" : "Femenino";

                        // Actualizar notas
                        estudiante.Notas[0] = double.Parse(not1txt.Text);
                        estudiante.Notas[1] = double.Parse(not2txt.Text);
                        estudiante.Notas[2] = double.Parse(not3txt.Text);

                        // Recalcular promedio
                        estudiante.CalcularPromedio();

                        // Actualizar actividades extracurriculares
                        estudiante.ActividadesExtracurriculares = ObtenerActividadesSeleccionadas();

                        // Validar datos
                        var errores = estudiante.ValidarDatos();
                        if (errores.Count > 0)
                        {
                            MessageBox.Show($"Errores de validación:\n{string.Join("\n", errores)}",
                                          "Error de Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        // Guardar todos los datos en CSV
                        if (GuardarDatosEnCSV())
                        {
                            // Actualizar interfaz
                            ActualizarListaEstudiantes();
                            LimpiarCampos();
                            estudianteSeleccionado = -1;

                            MessageBox.Show($"Estudiante {estudiante.Nombre} {estudiante.Apellido} actualizado exitosamente!",
                                          "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Por favor, seleccione un estudiante para editar.", "Advertencia",
                                  MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al editar estudiante: {ex.Message}", "Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btndelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (estudianteSeleccionado >= 0 && estudianteSeleccionado < estudiantes.Count)
                {
                    var estudiante = estudiantes[estudianteSeleccionado];

                    DialogResult resultado = MessageBox.Show(
                        $"¿Está seguro de eliminar al estudiante {estudiante.Nombre} {estudiante.Apellido}?\n\n" +
                        $"Esta acción no se puede deshacer y se actualizará el archivo CSV.",
                        "Confirmar eliminación",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question);

                    if (resultado == DialogResult.Yes)
                    {
                        // Eliminar de la lista
                        estudiantes.RemoveAt(estudianteSeleccionado);

                        // Guardar cambios en CSV
                        if (GuardarDatosEnCSV())
                        {
                            // Actualizar interfaz
                            ActualizarListaEstudiantes();
                            LimpiarCampos();
                            estudianteSeleccionado = -1;

                            MessageBox.Show($"Estudiante {estudiante.Nombre} {estudiante.Apellido} eliminado exitosamente!",
                                          "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            // Si falla el guardado, restaurar el estudiante
                            estudiantes.Insert(estudianteSeleccionado, estudiante);
                            MessageBox.Show("Error al actualizar el archivo CSV. El estudiante no fue eliminado.",
                                          "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Por favor, seleccione un estudiante para eliminar.", "Advertencia",
                                  MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al eliminar estudiante: {ex.Message}", "Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region Eventos de ListBox

        private void studentslist_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (studentslist.SelectedIndex >= 0)
            {
                estudianteSeleccionado = studentslist.SelectedIndex;
                CargarDatosEstudiante(estudianteSeleccionado);
            }
        }

        private void StudentslistDoubleClick(object sender, EventArgs e)
        {
            if (studentslist.SelectedIndex >= 0)
            {
                MostrarDetallesEstudiante(studentslist.SelectedIndex);
            }
        }

        #endregion

        #region Eventos de RadioButtons

        private void rdbtnF_CheckedChanged(object sender, EventArgs e)
        {
            // RadioButton femenino - no necesita lógica específica
        }

        private void rdbtnM_CheckedChanged(object sender, EventArgs e)
        {
            // RadioButton masculino - no necesita lógica específica
        }

        #endregion

        #region Eventos del PictureBox

        private void Approved_Disapproved_Click(object sender, EventArgs e)
        {
            // PictureBox - podría mostrar información adicional del estado académico
            if (estudianteSeleccionado >= 0 && estudianteSeleccionado < estudiantes.Count)
            {
                var estudiante = estudiantes[estudianteSeleccionado];
                string estado = estudiante.EstaAprobado() ? "APROBADO" : "REPROBADO";
                MessageBox.Show($"Estado académico: {estado}\nPromedio: {estudiante.Promedio:0.00}",
                              "Estado Académico", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        #endregion

        #region Métodos de Cálculo y Validación

        private void CalcularPromedioEnTiempoReal(object sender, EventArgs e)
        {
            try
            {
                if (double.TryParse(not1txt.Text, out double nota1) &&
                    double.TryParse(not2txt.Text, out double nota2) &&
                    double.TryParse(not3txt.Text, out double nota3))
                {
                    double promedio = (nota1 + nota2 + nota3) / 3.0;
                    Averagetxt.Text = promedio.ToString("0.00");

                    // Mostrar imagen según el promedio
                    MostrarImagenPromedio(promedio);
                }
                else
                {
                    Averagetxt.Clear();
                    Approved_Disapproved.Image = null;
                }
            }
            catch (Exception)
            {
                // En caso de error, limpiar campos
                Averagetxt.Clear();
                Approved_Disapproved.Image = null;
            }
        }

        private bool ValidarCampos()
        {
            // Validar nombre
            if (string.IsNullOrWhiteSpace(nametxt.Text))
            {
                MessageBox.Show("Por favor, ingrese el nombre del estudiante.", "Campo requerido",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                nametxt.Focus();
                return false;
            }

            // Validar apellido
            if (string.IsNullOrWhiteSpace(lastnametxt.Text))
            {
                MessageBox.Show("Por favor, ingrese el apellido del estudiante.", "Campo requerido",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                lastnametxt.Focus();
                return false;
            }

            // Validar edad
            if (!int.TryParse(agetxt.Text, out int edad) || edad <= 0 || edad > 120)
            {
                MessageBox.Show("Por favor, ingrese una edad válida (1-120 años).", "Campo inválido",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                agetxt.Focus();
                return false;
            }

            // Validar sexo
            if (!rdbtnM.Checked && !rdbtnF.Checked)
            {
                MessageBox.Show("Por favor, seleccione el sexo del estudiante.", "Campo requerido",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            // Validar notas
            if (!double.TryParse(not1txt.Text, out double nota1) || nota1 < 0 || nota1 > 5)
            {
                MessageBox.Show("Por favor, ingrese una nota válida para N1 (0.0 - 5.0).", "Campo inválido",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                not1txt.Focus();
                return false;
            }

            if (!double.TryParse(not2txt.Text, out double nota2) || nota2 < 0 || nota2 > 5)
            {
                MessageBox.Show("Por favor, ingrese una nota válida para N2 (0.0 - 5.0).", "Campo inválido",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                not2txt.Focus();
                return false;
            }

            if (!double.TryParse(not3txt.Text, out double nota3) || nota3 < 0 || nota3 > 5)
            {
                MessageBox.Show("Por favor, ingrese una nota válida para N3 (0.0 - 5.0).", "Campo inválido",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                not3txt.Focus();
                return false;
            }

            return true;
        }

        #endregion

        #region Métodos de Interfaz

        private void LimpiarCampos()
        {
            nametxt.Clear();
            lastnametxt.Clear();
            agetxt.Clear();
            not1txt.Clear();
            not2txt.Clear();
            not3txt.Clear();
            Averagetxt.Clear();

            rdbtnM.Checked = false;
            rdbtnF.Checked = false;

            // Limpiar actividades extracurriculares
            LimpiarActividadesExtracurriculares();

            // Limpiar imagen
            Approved_Disapproved.Image = null;

            // Limpiar selección
            studentslist.ClearSelected();
            estudianteSeleccionado = -1;
        }

        private void LimpiarActividadesExtracurriculares()
        {
            if (chkArte != null) chkArte.Checked = false;
            if (chkDeporte != null) chkDeporte.Checked = false;
            if (chkMusica != null) chkMusica.Checked = false;
            if (chkNoAplica != null) chkNoAplica.Checked = false;

            // Buscar otros CheckBoxes dinámicamente
            foreach (Control control in this.Controls)
            {
                if (control is CheckBox cb)
                {
                    if (cb.Name.ToLower().Contains("arte") || cb.Text.ToLower().Contains("arte") ||
                        cb.Name.ToLower().Contains("deporte") || cb.Text.ToLower().Contains("deporte") ||
                        cb.Name.ToLower().Contains("musica") || cb.Text.ToLower().Contains("música") ||
                        cb.Name.ToLower().Contains("aplica") || cb.Text.ToLower().Contains("aplica"))
                    {
                        cb.Checked = false;
                    }
                }
            }
        }

        private List<string> ObtenerActividadesSeleccionadas()
        {
            var actividades = new List<string>();

            if (chkArte != null && chkArte.Checked) actividades.Add("Arte");
            if (chkDeporte != null && chkDeporte.Checked) actividades.Add("Deporte");
            if (chkMusica != null && chkMusica.Checked) actividades.Add("Música");
            if (chkNoAplica != null && chkNoAplica.Checked) actividades.Add("No aplica");

            // Buscar otros CheckBoxes dinámicamente
            foreach (Control control in this.Controls)
            {
                if (control is CheckBox cb && cb.Checked)
                {
                    if ((cb.Name.ToLower().Contains("arte") || cb.Text.ToLower().Contains("arte")) && !actividades.Contains("Arte"))
                        actividades.Add("Arte");
                    else if ((cb.Name.ToLower().Contains("deporte") || cb.Text.ToLower().Contains("deporte")) && !actividades.Contains("Deporte"))
                        actividades.Add("Deporte");
                    else if ((cb.Name.ToLower().Contains("musica") || cb.Text.ToLower().Contains("música")) && !actividades.Contains("Música"))
                        actividades.Add("Música");
                    else if ((cb.Name.ToLower().Contains("aplica") || cb.Text.ToLower().Contains("aplica")) && !actividades.Contains("No aplica"))
                        actividades.Add("No aplica");
                }
            }

            return actividades;
        }

        private void ActualizarListaEstudiantes()
        {
            studentslist.Items.Clear();
            foreach (var estudiante in estudiantes)
            {
                studentslist.Items.Add(estudiante); // Usa el ToString() de la clase Estudiante
            }
        }

        private void CargarDatosEstudiante(int indice)
        {
            if (indice >= 0 && indice < estudiantes.Count)
            {
                var estudiante = estudiantes[indice];

                nametxt.Text = estudiante.Nombre;
                lastnametxt.Text = estudiante.Apellido;
                agetxt.Text = estudiante.Edad.ToString();

                if (estudiante.Sexo == "Masculino")
                    rdbtnM.Checked = true;
                else if (estudiante.Sexo == "Femenino")
                    rdbtnF.Checked = true;

                not1txt.Text = estudiante.Notas[0].ToString("0.0");
                not2txt.Text = estudiante.Notas[1].ToString("0.0");
                not3txt.Text = estudiante.Notas[2].ToString("0.0");

                Averagetxt.Text = estudiante.Promedio.ToString("0.00");

                // Marcar actividades extracurriculares
                MarcarActividades(estudiante.ActividadesExtracurriculares);

                // Mostrar imagen según el promedio
                MostrarImagenPromedio(estudiante.Promedio);
            }
        }

        private void MarcarActividades(List<string> actividades)
        {
            // Primero limpiar todas las actividades
            LimpiarActividadesExtracurriculares();

            // Luego marcar las actividades del estudiante
            foreach (string actividad in actividades)
            {
                switch (actividad.ToLower())
                {
                    case "arte":
                        if (chkArte != null) chkArte.Checked = true;
                        break;
                    case "deporte":
                        if (chkDeporte != null) chkDeporte.Checked = true;
                        break;
                    case "música":
                    case "musica":
                        if (chkMusica != null) chkMusica.Checked = true;
                        break;
                    case "no aplica":
                        if (chkNoAplica != null) chkNoAplica.Checked = true;
                        break;
                }
            }

            // Buscar y marcar CheckBoxes dinámicamente
            foreach (Control control in this.Controls)
            {
                if (control is CheckBox cb)
                {
                    foreach (string actividad in actividades)
                    {
                        if ((cb.Name.ToLower().Contains("arte") || cb.Text.ToLower().Contains("arte")) && actividad.ToLower() == "arte")
                            cb.Checked = true;
                        else if ((cb.Name.ToLower().Contains("deporte") || cb.Text.ToLower().Contains("deporte")) && actividad.ToLower() == "deporte")
                            cb.Checked = true;
                        else if ((cb.Name.ToLower().Contains("musica") || cb.Text.ToLower().Contains("música")) && actividad.ToLower().Contains("música"))
                            cb.Checked = true;
                        else if ((cb.Name.ToLower().Contains("aplica") || cb.Text.ToLower().Contains("aplica")) && actividad.ToLower().Contains("aplica"))
                            cb.Checked = true;
                    }
                }
            }
        }

        private void MostrarDetallesEstudiante(int indice)
        {
            if (indice >= 0 && indice < estudiantes.Count)
            {
                var estudiante = estudiantes[indice];
                string detalles = estudiante.GenerarResumen();

                MessageBox.Show(detalles, $"Detalles - {estudiante.Nombre} {estudiante.Apellido}",
                              MessageBoxButtons.OK, MessageBoxIcon.Information);

                // También mostrar la imagen correspondiente
                MostrarImagenPromedio(estudiante.Promedio);
            }
        }

        private void MostrarImagenPromedio(double promedio)
        {
            try
            {
                string rutaImagen = "";

                if (promedio >= 3.0)
                {
                    rutaImagen = Path.Combine(Application.StartupPath, "Aprobado.png");
                }
                else
                {
                    rutaImagen = Path.Combine(Application.StartupPath, "Reprobado.png");
                }

                if (File.Exists(rutaImagen))
                {
                    Approved_Disapproved.Image = Image.FromFile(rutaImagen);
                    Approved_Disapproved.SizeMode = PictureBoxSizeMode.StretchImage;
                }
                else
                {
                    // Si no se encuentra la imagen, crear una imagen temporal con texto
                    CrearImagenTemporal(promedio >= 3.0 ? "APROBADO" : "REPROBADO",
                                      promedio >= 3.0 ? Color.Green : Color.Red);
                }
            }
            catch (Exception)
            {
                // En caso de error con las imágenes, mostrar texto
                CrearImagenTemporal(promedio >= 3.0 ? "APROBADO" : "REPROBADO",
                                  promedio >= 3.0 ? Color.Green : Color.Red);
            }
        }

        private void CrearImagenTemporal(string texto, Color color)
        {
            try
            {
                Bitmap bitmap = new Bitmap(200, 100);
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.Clear(Color.White);
                    using (Font font = new Font("Arial", 12, FontStyle.Bold))
                    {
                        using (Brush brush = new SolidBrush(color))
                        {
                            StringFormat sf = new StringFormat();
                            sf.Alignment = StringAlignment.Center;
                            sf.LineAlignment = StringAlignment.Center;

                            g.DrawString(texto, font, brush,
                                       new RectangleF(0, 0, bitmap.Width, bitmap.Height), sf);
                        }
                    }
                }
                Approved_Disapproved.Image = bitmap;
                Approved_Disapproved.SizeMode = PictureBoxSizeMode.StretchImage;
            }
            catch (Exception)
            {
                Approved_Disapproved.Image = null;
            }
        }

        #endregion
    }
}