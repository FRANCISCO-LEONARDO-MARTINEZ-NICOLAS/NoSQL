using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace NoSQL.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SystemMonitoringController : ControllerBase
    {
        private readonly string _logFilePath = "system-logs.json";

        [HttpGet("activities")]
        public IActionResult GetActivities([FromQuery] string? user, [FromQuery] string? action, [FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
        {
            try
            {
                var activities = LoadActivities();

                // Filtrar por usuario
                if (!string.IsNullOrEmpty(user))
                {
                    activities = activities.Where(a => a.UserEmail.ToLower().Contains(user.ToLower())).ToList();
                }

                // Filtrar por acción
                if (!string.IsNullOrEmpty(action))
                {
                    activities = activities.Where(a => a.Action.ToLower().Contains(action.ToLower())).ToList();
                }

                // Filtrar por fecha
                if (startDate.HasValue)
                {
                    activities = activities.Where(a => a.Timestamp >= startDate.Value).ToList();
                }

                if (endDate.HasValue)
                {
                    activities = activities.Where(a => a.Timestamp <= endDate.Value).ToList();
                }

                return Ok(activities.OrderByDescending(a => a.Timestamp));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al obtener actividades", error = ex.Message });
            }
        }

        [HttpGet("stats")]
        public IActionResult GetSystemStats()
        {
            try
            {
                var activities = LoadActivities();
                var now = DateTime.UtcNow;
                var last24Hours = now.AddHours(-24);
                var last7Days = now.AddDays(-7);
                var last30Days = now.AddDays(-30);

                var stats = new SystemStats
                {
                    TotalActivities = activities.Count,
                    ActivitiesLast24Hours = activities.Count(a => a.Timestamp >= last24Hours),
                    ActivitiesLast7Days = activities.Count(a => a.Timestamp >= last7Days),
                    ActivitiesLast30Days = activities.Count(a => a.Timestamp >= last30Days),
                    TopUsers = activities
                        .GroupBy(a => a.UserEmail)
                        .Select(g => new TopUserStats { User = g.Key, Count = g.Count() })
                        .OrderByDescending(x => x.Count)
                        .Take(5)
                        .ToList(),
                    TopActions = activities
                        .GroupBy(a => a.Action)
                        .Select(g => new TopActionStats { Action = g.Key, Count = g.Count() })
                        .OrderByDescending(x => x.Count)
                        .Take(5)
                        .ToList(),
                    RecentActivities = activities
                        .OrderByDescending(a => a.Timestamp)
                        .Take(10)
                        .ToList()
                };

                return Ok(stats);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al obtener estadísticas", error = ex.Message });
            }
        }

        [HttpPost("log")]
        public IActionResult LogActivity([FromBody] ActivityLog activity)
        {
            try
            {
                if (activity == null)
                    return BadRequest(new { message = "Actividad requerida" });

                activity.Timestamp = DateTime.UtcNow;
                activity.Id = Guid.NewGuid().ToString();

                var activities = LoadActivities();
                activities.Add(activity);

                // Mantener solo los últimos 1000 logs para evitar archivos muy grandes
                if (activities.Count > 1000)
                {
                    activities = activities.OrderByDescending(a => a.Timestamp).Take(1000).ToList();
                }

                SaveActivities(activities);

                return Ok(new { message = "Actividad registrada exitosamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al registrar actividad", error = ex.Message });
            }
        }

        [HttpGet("health")]
        public IActionResult GetSystemHealth()
        {
            try
            {
                var health = new SystemHealth
                {
                    Status = "Healthy",
                    Timestamp = DateTime.UtcNow,
                    Uptime = TimeSpan.FromMilliseconds(Environment.TickCount64),
                    MemoryUsage = GC.GetTotalMemory(false),
                    CpuUsage = GetCpuUsage(),
                    DiskSpace = GetDiskSpace(),
                    DatabaseStatus = "Connected",
                    Services = new List<ServiceStatus>
                    {
                        new ServiceStatus { Name = "API", Status = "Running" },
                        new ServiceStatus { Name = "Database", Status = "Connected" },
                        new ServiceStatus { Name = "Email Service", Status = "Available" }
                    }
                };

                return Ok(health);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al obtener estado del sistema", error = ex.Message });
            }
        }

        [HttpDelete("logs")]
        public IActionResult ClearLogs([FromQuery] int? daysToKeep)
        {
            try
            {
                var activities = LoadActivities();

                if (daysToKeep.HasValue)
                {
                    var cutoffDate = DateTime.UtcNow.AddDays(-daysToKeep.Value);
                    activities = activities.Where(a => a.Timestamp >= cutoffDate).ToList();
                }
                else
                {
                    activities.Clear();
                }

                SaveActivities(activities);

                return Ok(new { message = "Logs limpiados exitosamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al limpiar logs", error = ex.Message });
            }
        }

        private List<ActivityLog> LoadActivities()
        {
            if (!System.IO.File.Exists(_logFilePath))
                return new List<ActivityLog>();

            var json = System.IO.File.ReadAllText(_logFilePath);
            return JsonSerializer.Deserialize<List<ActivityLog>>(json) ?? new List<ActivityLog>();
        }

        private void SaveActivities(List<ActivityLog> activities)
        {
            var json = JsonSerializer.Serialize(activities, new JsonSerializerOptions { WriteIndented = true });
            System.IO.File.WriteAllText(_logFilePath, json);
        }

        private double GetCpuUsage()
        {
            // Simulación de uso de CPU
            return new Random().NextDouble() * 100;
        }

        private DiskInfo GetDiskSpace()
        {
            try
            {
                var drive = new DriveInfo(Directory.GetCurrentDirectory());
                return new DiskInfo
                {
                    TotalSpace = drive.TotalSize,
                    FreeSpace = drive.AvailableFreeSpace,
                    UsedSpace = drive.TotalSize - drive.AvailableFreeSpace
                };
            }
            catch
            {
                return new DiskInfo { TotalSpace = 0, FreeSpace = 0, UsedSpace = 0 };
            }
        }
    }

    public class ActivityLog
    {
        public string Id { get; set; } = string.Empty;
        public string UserEmail { get; set; } = string.Empty;
        public string UserRole { get; set; } = string.Empty;
        public string Action { get; set; } = string.Empty;
        public string Details { get; set; } = string.Empty;
        public string IpAddress { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
        public string? Module { get; set; }
        public bool Success { get; set; } = true;
    }

    public class SystemStats
    {
        public int TotalActivities { get; set; }
        public int ActivitiesLast24Hours { get; set; }
        public int ActivitiesLast7Days { get; set; }
        public int ActivitiesLast30Days { get; set; }
        public List<TopUserStats> TopUsers { get; set; } = new();
        public List<TopActionStats> TopActions { get; set; } = new();
        public List<ActivityLog> RecentActivities { get; set; } = new();
    }

    public class TopUserStats
    {
        public string User { get; set; } = string.Empty;
        public int Count { get; set; }
    }

    public class TopActionStats
    {
        public string Action { get; set; } = string.Empty;
        public int Count { get; set; }
    }

    public class SystemHealth
    {
        public string Status { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
        public TimeSpan Uptime { get; set; }
        public long MemoryUsage { get; set; }
        public double CpuUsage { get; set; }
        public DiskInfo DiskSpace { get; set; } = new();
        public string DatabaseStatus { get; set; } = string.Empty;
        public List<ServiceStatus> Services { get; set; } = new();
    }

    public class ServiceStatus
    {
        public string Name { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
    }

    public class DiskInfo
    {
        public long TotalSpace { get; set; }
        public long FreeSpace { get; set; }
        public long UsedSpace { get; set; }
    }
} 