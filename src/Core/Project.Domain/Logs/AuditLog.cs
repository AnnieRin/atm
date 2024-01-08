using Project.Domain.Common;

namespace Project.Domain.Logs;
public class AuditLog : BaseEntity
{
    public DateTime Timestamp { get; set; }
    public string PersonalN { get; set; }
    public string Action { get; set; }
    public int StatusCode { get; set; }
    public string Description { get; set; }
    public string ServiceName { get; set; }
}
