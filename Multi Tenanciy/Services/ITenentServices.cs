namespace Multi_Tenanciy.Services
{
    public interface ITenentServices
    {
        string? GetConnectionString();
        string? GetDataBaseProvider();
        Tenant? GetCurrentTenant();
    }
}
