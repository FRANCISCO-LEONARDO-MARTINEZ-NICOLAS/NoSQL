using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace NoSQL.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SystemConfigController : ControllerBase
    {
        private readonly string _configFilePath = "system-config.json";

        [HttpGet]
        public IActionResult GetSystemConfig()
        {
            try
            {
                if (!System.IO.File.Exists(_configFilePath))
                {
                    var defaultConfig = new SystemConfig
                    {
                        Roles = new List<RoleConfig>
                        {
                            new RoleConfig { Name = "Admin", Permissions = new List<string> { "all" } },
                            new RoleConfig { Name = "Optometrista", Permissions = new List<string> { "consultas", "citas", "pacientes" } }
                        },
                        Notifications = new NotificationConfig
                        {
                            EmailSettings = new EmailSettings
                            {
                                SmtpServer = "smtp.gmail.com",
                                SmtpPort = 587,
                                SenderEmail = "noreply@opticare.com",
                                SenderName = "OptiCare System"
                            },
                            Templates = new List<EmailTemplate>
                            {
                                new EmailTemplate
                                {
                                    Name = "Cita Confirmada",
                                    Subject = "Confirmación de Cita - OptiCare",
                                    Body = "Su cita ha sido confirmada para el {fecha} a las {hora}."
                                },
                                new EmailTemplate
                                {
                                    Name = "Recordatorio de Cita",
                                    Subject = "Recordatorio de Cita - OptiCare",
                                    Body = "Le recordamos su cita programada para mañana a las {hora}."
                                }
                            }
                        },
                        Backup = new BackupConfig
                        {
                            AutoBackup = true,
                            BackupFrequency = "daily",
                            RetentionDays = 30,
                            BackupPath = "./backups"
                        }
                    };

                    SaveConfig(defaultConfig);
                    return Ok(defaultConfig);
                }

                var configJson = System.IO.File.ReadAllText(_configFilePath);
                var config = JsonSerializer.Deserialize<SystemConfig>(configJson);
                return Ok(config);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al obtener la configuración del sistema", error = ex.Message });
            }
        }

        [HttpPut]
        public IActionResult UpdateSystemConfig([FromBody] SystemConfig config)
        {
            try
            {
                if (config == null)
                    return BadRequest(new { message = "Configuración requerida" });

                SaveConfig(config);
                return Ok(new { message = "Configuración actualizada exitosamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al actualizar la configuración", error = ex.Message });
            }
        }

        [HttpPost("backup")]
        public IActionResult CreateBackup()
        {
            try
            {
                var backupPath = $"./backups/backup_{DateTime.Now:yyyyMMdd_HHmmss}.json";
                var configJson = System.IO.File.ReadAllText(_configFilePath);
                
                Directory.CreateDirectory(Path.GetDirectoryName(backupPath)!);
                System.IO.File.WriteAllText(backupPath, configJson);

                return Ok(new { message = "Respaldo creado exitosamente", path = backupPath });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al crear el respaldo", error = ex.Message });
            }
        }

        [HttpGet("backups")]
        public IActionResult GetBackups()
        {
            try
            {
                var backupDir = "./backups";
                if (!Directory.Exists(backupDir))
                    return Ok(new List<object>());

                var backups = Directory.GetFiles(backupDir, "backup_*.json")
                    .Select(file => new
                    {
                        name = Path.GetFileName(file),
                        path = file,
                        created = System.IO.File.GetCreationTime(file),
                        size = new FileInfo(file).Length
                    })
                    .OrderByDescending(b => b.created)
                    .ToList();

                return Ok(backups);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al obtener respaldos", error = ex.Message });
            }
        }

        [HttpPost("test-email")]
        public IActionResult TestEmail([FromBody] EmailTestRequest request)
        {
            try
            {
                // Aquí iría la lógica para probar el envío de correos
                // Por ahora solo simulamos el envío
                return Ok(new { message = "Correo de prueba enviado exitosamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al enviar correo de prueba", error = ex.Message });
            }
        }

        private void SaveConfig(SystemConfig config)
        {
            var configJson = JsonSerializer.Serialize(config, new JsonSerializerOptions { WriteIndented = true });
            System.IO.File.WriteAllText(_configFilePath, configJson);
        }
    }

    public class SystemConfig
    {
        public List<RoleConfig> Roles { get; set; } = new();
        public NotificationConfig Notifications { get; set; } = new();
        public BackupConfig Backup { get; set; } = new();
    }

    public class RoleConfig
    {
        public string Name { get; set; } = string.Empty;
        public List<string> Permissions { get; set; } = new();
    }

    public class NotificationConfig
    {
        public EmailSettings EmailSettings { get; set; } = new();
        public List<EmailTemplate> Templates { get; set; } = new();
    }

    public class EmailSettings
    {
        public string SmtpServer { get; set; } = string.Empty;
        public int SmtpPort { get; set; }
        public string SenderEmail { get; set; } = string.Empty;
        public string SenderName { get; set; } = string.Empty;
        public string? Username { get; set; }
        public string? Password { get; set; }
        public bool UseSsl { get; set; } = true;
    }

    public class EmailTemplate
    {
        public string Name { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
    }

    public class BackupConfig
    {
        public bool AutoBackup { get; set; } = true;
        public string BackupFrequency { get; set; } = "daily";
        public int RetentionDays { get; set; } = 30;
        public string BackupPath { get; set; } = "./backups";
    }

    public class EmailTestRequest
    {
        public string To { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
    }
} 