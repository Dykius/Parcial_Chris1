# Sistema de Gestion de estudiantes y envio de correos.

Este proyecto es una aplicación de escritorio desarrollada en C# con Windows Forms, orientada a la gestión y envío de correos electrónicos a estudiantes. Permite cargar datos desde archivos CSV, visualizar y editar información de estudiantes, y enviar correos personalizados de manera eficiente.

## Características principales

- Carga y serialización de datos de estudiantes desde archivos CSV.
- Visualización y edición de información de estudiantes.
- Envío de correos electrónicos personalizados.
- Configuración flexible mediante archivo `App.config`.

## Requisitos previos

- Visual Studio 2022 o superior.
- .NET Framework 4.7.2 y/o .NET 8 (según el proyecto que desees ejecutar).
- Acceso a una cuenta de correo electrónico para el envío de emails (SMTP).

## Instalación

1. Clona el repositorio:
   2. Abre la solución en Visual Studio 2022.
3. Restaura los paquetes NuGet si es necesario (__Tools > NuGet Package Manager > Restore NuGet Packages__).
4. Configura las variables de entorno en el archivo `App.config` según tus credenciales y preferencias.

## Variables de entorno / configuración

En el archivo `App.config` encontrarás las siguientes variables importantes:

- **SmtpServer**: Dirección del servidor SMTP (ejemplo: `smtp.gmail.com`).
- **SmtpPort**: Puerto del servidor SMTP (ejemplo: `587`).
- **SmtpUser**: Usuario/correo electrónico remitente.
- **SmtpPassword**: Contraseña del correo electrónico remitente.
- **CsvFilePath**: Ruta al archivo CSV con los datos de los estudiantes.

Ejemplo de configuración en `App.config`:

## Uso

1. Ejecuta el proyecto desde Visual Studio (__Debug > Start Debugging__).
2. Carga el archivo CSV con los datos de los estudiantes.
3. Visualiza y edita la información según sea necesario.
4. Configura los parámetros de correo en `App.config`.
5. Utiliza la interfaz para enviar correos electrónicos a los estudiantes seleccionados.

## Autor

Nombre del autor: **[Christopher ospina]**

---

## Notas adicionales

- Asegúrate de que el archivo CSV tenga el formato correcto para evitar errores de carga.
- Si utilizas Gmail, puede que necesites habilitar el acceso de aplicaciones menos seguras o generar una contraseña de aplicación.
- El sistema está preparado para ejecutarse tanto en .NET Framework 4.7.2 como en .NET 8; selecciona el proyecto adecuado en la solución.

## Licencia

Sin licencia modelo base para el parcial de la asignatura Programcion avanzada.

