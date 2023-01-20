namespace TrafficControlService.Repositories;

public interface IVehicleStateRepository
{
    Task SaveVehicleStateAsync(VehicleState vehicleState);
    Task<VehicleState?> GetVehicleStateAsync(string licenseNumber);
}

public class DaprVehicleStateRepository : IVehicleStateRepository
{
    private const string DAPR_STORE_NAME = "statestore";
    private readonly DaprClient _daprClient;

    public DaprVehicleStateRepository(DaprClient daprClient)
    {
        _daprClient = daprClient;
    }

    public async Task SaveVehicleStateAsync(VehicleState vehicleState)
    {
        await _daprClient.SaveStateAsync<VehicleState>(
            DAPR_STORE_NAME, vehicleState.LicenseNumber, vehicleState);
    }

    public async Task<VehicleState?> GetVehicleStateAsync(string licenseNumber)
    {
        var stateEntry = await _daprClient.GetStateEntryAsync<VehicleState>(
            DAPR_STORE_NAME, licenseNumber);
        return stateEntry.Value;
    }
}
