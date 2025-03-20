# Warehouse BoxSys Notification process in Azure
## Azure Resources
- Azure Functions
  -  ReceiveShippoNotifications 
  -  UpdateShippoTrackingStatus
- Azure Storage Queues
  - shipponotification
  - processedqueue
  - failedqueue
- Hosting Environment
  - Hosted on the Azure Function App  "func-WarehouseBoxSys" in the resource group rg-appdev-dev
  - App Insights  "func-WarehouseBoxSys"
