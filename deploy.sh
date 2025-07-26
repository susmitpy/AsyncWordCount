az functionapp create --resource-group RealTimeWordCountRG --consumption-plan-location "eastus" --runtime dotnet-isolated --functions-version 4 --name awcworker --storage-account realtimewordcount
func azure functionapp publish awcworker

az functionapp create --resource-group RealTimeWordCountRG --consumption-plan-location "eastus" --runtime dotnet-isolated --functions-version 4 --name awcweb --storage-account realtimewordcount
func azure functionapp publish awcweb