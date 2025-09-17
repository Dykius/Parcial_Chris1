using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using Parcial_ProgAVZ1.Clases;

namespace Parcial_ProgAVZ1
{
    public partial class Frm_correos : Form
    {
        #region Variables Privadas

        private Estudiante estudianteSeleccionado;
        private List<string> archivosAdjuntos;
        private Dictionary<string, string> configuracionServidor;

        // Lista blanca de dominios permitidos
        private readonly HashSet<string> dominiosPermitidos = new HashSet<string>
        {
            "gmail.com", "outlook.com", "hotmail.com", "yahoo.com", "yahoo.es",
            "institutotecnologico.edu.co", "sena.edu.co", "unal.edu.co",
            "javeriana.edu.co", "andes.edu.co", "unisabana.edu.co"
        };

        #endregion

        #region Constructor

        public Frm_correos()
        {
            InitializeComponent();
            InicializarFormulario();
        }

        public Frm_correos(Estudiante estudiante)
        {
            InitializeComponent();
            this.estudianteSeleccionado = estudiante;
            InicializarFormulario();
            CargarDatosEstudiante();
        }

        #endregion

        #region Inicialización

        private void InicializarFormulario()
        {
            archivosAdjuntos = new List<string>();
            configuracionServidor = new Dictionary<string, string>();

            // Configuración por defecto (Gmail)
            ConfigurarGmail();

            // Configurar eventos
            txtDestinatario.Leave += ValidarEmail;
            txtAsunto.Text = "Reporte Académico - Sistema de Gestión de Estudiantes";


            // Configurar la lista de adjuntos
            listAdjuntos.View = View.Details;
            listAdjuntos.Columns.Add("Archivo", 200);
            listAdjuntos.Columns.Add("Tamaño", 100);
            listAdjuntos.FullRowSelect = true;
            listAdjuntos.GridLines = true;
        }

        private void ConfigurarGmail()
        {
            configuracionServidor["Servidor"] = "smtp.gmail.com";
            configuracionServidor["Puerto"] = "587";
            configuracionServidor["SSL"] = "true";
            configuracionServidor["Usuario"] = "tu_correo@gmail.com"; // Cambiar por tu correo
            configuracionServidor["Password"] = "tu_app_password"; // Cambiar por tu App Password
        }

      

        #endregion

        #region Carga de Datos

        private void CargarDatosEstudiante()
        {
            if (estudianteSeleccionado != null)
            {
                // Cargar datos del estudiante en el cuerpo del mensaje
                string datosEstudiante = estudianteSeleccionado.GenerarDatosParaCorreo();
                txtCuerpoMensaje.Text = datosEstudiante;

                // Actualizar el asunto con el nombre del estudiante
                txtAsunto.Text = $"Reporte Académico - {estudianteSeleccionado.Nombre} {estudianteSeleccionado.Apellido}";

                // Mostrar información del estudiante en la interfaz
                lblEstudianteInfo.Text = $"Enviando datos de: {estudianteSeleccionado.Nombre} {estudianteSeleccionado.Apellido} " +
                                       $"(Promedio: {estudianteSeleccionado.Promedio:0.00})";
            }
            else
            {
                lblEstudianteInfo.Text = "No hay estudiante seleccionado";
                txtCuerpoMensaje.Text = "Mensaje desde el Sistema de Gestión de Estudiantes";
            }
        }

        #endregion

        #region Validaciones

        private void ValidarEmail(object sender, EventArgs e)
        {
            string email = txtDestinatario.Text.Trim();

            if (string.IsNullOrWhiteSpace(email))
            {
                lblValidacionEmail.Text = "";
                lblValidacionEmail.ForeColor = Color.Black;
                return;
            }

            if (EsEmailValido(email))
            {
                if (EsDominioPermitido(email))
                {
                    lblValidacionEmail.Text = "✓ Email válido";
                    lblValidacionEmail.ForeColor = Color.Green;
                }
                else
                {
                    lblValidacionEmail.Text = "⚠ Dominio no permitido";
                    lblValidacionEmail.ForeColor = Color.Orange;
                }
            }
            else
            {
                lblValidacionEmail.Text = "✗ Formato de email inválido";
                lblValidacionEmail.ForeColor = Color.Red;
            }
        }

        private bool EsEmailValido(string email)
        {
            try
            {
                var mailAddress = new System.Net.Mail.MailAddress(email);
                return mailAddress.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private bool EsDominioPermitido(string email)
        {
            try
            {
                string dominio = email.Split('@')[1].ToLower();
                return dominiosPermitidos.Contains(dominio);
            }
            catch
            {
                return false;
            }
        }

        #endregion

        #region Manejo de Adjuntos

       

        private void AgregarArchivoALista(string rutaArchivo)
        {
            try
            {
                FileInfo fileInfo = new FileInfo(rutaArchivo);
                ListViewItem item = new ListViewItem(fileInfo.Name);
                item.SubItems.Add(FormatearTamañoArchivo(fileInfo.Length));
                item.Tag = rutaArchivo;

                listAdjuntos.Items.Add(item);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al procesar el archivo {rutaArchivo}: {ex.Message}",
                              "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private string FormatearTamañoArchivo(long bytes)
        {
            string[] sizes = { "B", "KB", "MB", "GB" };
            double len = bytes;
            int order = 0;

            while (len >= 1024 && order < sizes.Length - 1)
            {
                order++;
                len = len / 1024;
            }

            return $"{len:0.##} {sizes[order]}";
        }

        private void btnEliminarAdjunto_Click(object sender, EventArgs e)
        {
            if (listAdjuntos.SelectedItems.Count > 0)
            {
                foreach (ListViewItem item in listAdjuntos.SelectedItems)
                {
                    string rutaArchivo = item.Tag.ToString();
                    archivosAdjuntos.Remove(rutaArchivo);
                    listAdjuntos.Items.Remove(item);
                }

                ActualizarInfoAdjuntos();
            }
        }

        private void ActualizarInfoAdjuntos()
        {
            lblInfoAdjuntos.Text = $"Adjuntos: {archivosAdjuntos.Count} archivo(s)";
            btnEliminarAdjunto.Enabled = archivosAdjuntos.Count > 0;
        }

        #endregion

        #region Envío de Correo

        private async void btnEnviar_Click(object sender, EventArgs e)
        {
            if (!ValidarFormulario())
                return;

            // Deshabilitar el botón para evitar múltiples envíos
            btnEnviar.Enabled = false;
            btnEnviar.Text = "Enviando...";

          

            try
            {
                bool resultado = await EnviarCorreoAsync();

                if (resultado)
                {
                    MessageBox.Show("Correo enviado exitosamente!", "Éxito",
                                  MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Preguntar si desea cerrar el formulario
                    if (MessageBox.Show("¿Desea cerrar la ventana de correos?", "Confirmar",
                                      MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        this.Close();
                    }
                    else
                    {
                        LimpiarFormulario();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al enviar el correo: {ex.Message}", "Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // Rehabilitar el botón
                btnEnviar.Enabled = true;
                btnEnviar.Text = "Enviar Correo";

              
            }
        }

        private bool ValidarFormulario()
        {
            // Validar destinatario
            if (string.IsNullOrWhiteSpace(txtDestinatario.Text))
            {
                MessageBox.Show("Por favor, ingrese el correo del destinatario.", "Campo requerido",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtDestinatario.Focus();
                return false;
            }

            if (!EsEmailValido(txtDestinatario.Text))
            {
                MessageBox.Show("Por favor, ingrese un correo electrónico válido.", "Email inválido",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtDestinatario.Focus();
                return false;
            }

            if (!EsDominioPermitido(txtDestinatario.Text))
            {
                DialogResult resultado = MessageBox.Show(
                    "El dominio del correo no está en la lista de dominios permitidos. ¿Desea continuar de todos modos?",
                    "Dominio no permitido", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (resultado == DialogResult.No)
                    return false;
            }

            // Validar asunto
            if (string.IsNullOrWhiteSpace(txtAsunto.Text))
            {
                MessageBox.Show("Por favor, ingrese el asunto del correo.", "Campo requerido",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtAsunto.Focus();
                return false;
            }

            // Validar cuerpo del mensaje
            if (string.IsNullOrWhiteSpace(txtCuerpoMensaje.Text))
            {
                MessageBox.Show("Por favor, ingrese el contenido del correo.", "Campo requerido",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtCuerpoMensaje.Focus();
                return false;
            }

            return true;
        }

        private async Task<bool> EnviarCorreoAsync()
        {
            try
            {
                // Crear el mensaje
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("Sistema Gestión Estudiantes", configuracionServidor["Usuario"]));
                message.To.Add(new MailboxAddress("", txtDestinatario.Text.Trim()));
                message.Subject = txtAsunto.Text.Trim();

                // Crear el cuerpo del mensaje
                var builder = new BodyBuilder();
                builder.TextBody = txtCuerpoMensaje.Text;

                // Agregar adjuntos
                foreach (string rutaArchivo in archivosAdjuntos)
                {
                    if (File.Exists(rutaArchivo))
                    {
                        object value = message.Attachments.A(rutaArchivo);
                    }
                }

                message.Body = builder.ToMessageBody();

                // Configurar el cliente SMTP
                using (var client = new SmtpClient())
                {
                    // Configurar para Gmail
                    await client.ConnectAsync(
                        configuracionServidor["Servidor"],
                        int.Parse(configuracionServidor["Puerto"]),
                        SecureSocketOptions.StartTls);

                    // Autenticación
                    await client.AuthenticateAsync(
                        configuracionServidor["Usuario"],
                        configuracionServidor["Password"]);

                    // Enviar el mensaje
                    await client.SendAsync(message);
                    await client.DisconnectAsync(true);
                }

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error en el envío: {ex.Message}");
            }
        }

        #endregion

        #region Métodos de Utilidad

        private void LimpiarFormulario()
        {
            txtDestinatario.Clear();
            txtAsunto.Text = "Reporte Académico - Sistema de Gestión de Estudiantes";
            txtCuerpoMensaje.Clear();

            // Limpiar adjuntos
            archivosAdjuntos.Clear();
            listAdjuntos.Items.Clear();
            ActualizarInfoAdjuntos();

            lblValidacionEmail.Text = "";

            // Volver a cargar datos del estudiante si existe
            if (estudianteSeleccionado != null)
            {
                CargarDatosEstudiante();
            }
        }

        public void SetEstudiante(Estudiante estudiante)
        {
            this.estudianteSeleccionado = estudiante;
            CargarDatosEstudiante();
        }

        #endregion

        #region Configuración de Servidor

        private void btnConfiguracion_Click(object sender, EventArgs e)
        {
            // Formulario simple para configuración rápida
            string servidor = Microsoft.VisualBasic.Interaction.InputBox(
                "Servidor SMTP:", "Configuración", configuracionServidor["Servidor"]);

            if (!string.IsNullOrWhiteSpace(servidor))
            {
                configuracionServidor["Servidor"] = servidor;

                string puerto = Microsoft.VisualBasic.Interaction.InputBox(
                    "Puerto:", "Configuración", configuracionServidor["Puerto"]);

                if (!string.IsNullOrWhiteSpace(puerto) && int.TryParse(puerto, out _))
                {
                    configuracionServidor["Puerto"] = puerto;
                }
            }
        }

        #endregion

        #region Eventos del Formulario

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Frm_correos_Load(object sender, EventArgs e)
        {
            // Cargar configuración guardada si existe
            CargarConfiguracion();
        }

        private void CargarConfiguracion()
        {
            // Aquí podrías cargar configuración desde app.config o archivo
            // Por ahora mantiene la configuración por defecto
        }

        private void btnPlantilla_Click(object sender, EventArgs e)
        {
            // Generar plantilla automática
            if (estudianteSeleccionado != null)
            {
                txtCuerpoMensaje.Text = estudianteSeleccionado.GenerarDatosParaCorreo();
            }
            else
            {
                txtCuerpoMensaje.Text = "Estimado/a destinatario/a,\n\n" +
                                      "Le enviamos información del Sistema de Gestión de Estudiantes.\n\n" +
                                      "Saludos cordiales,\n" +
                                      "Sistema de Gestión de Estudiantes";
            }
        }

        #endregion
    }
}