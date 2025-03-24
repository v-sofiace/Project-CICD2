using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace func_WarehouseBoxSys;
public class AppOptions
{
    public string KeyVaultUri { get; set; }
    public string StorageQueueUri { get; set; }
    public string WarehouseBoxSysApiUri { get; set; }
    public string NotificationsQueueName { get; set; }
    public string ProcessedQueueName { get; set; }
    public string FailedQueueName { get; set; }
}