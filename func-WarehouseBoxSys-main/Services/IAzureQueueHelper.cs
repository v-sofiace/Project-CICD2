using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace func_WarehouseBoxSys.Services;
public interface IAzureQueueHelper
{
    Task AddMessageToProcessedQueue(string data);
    Task AddMessageToFailedQueue(string data);
    Task AddMessageToManualQueue(string data);
    Task AddMessageToNotificationsQueue(string data);

}
