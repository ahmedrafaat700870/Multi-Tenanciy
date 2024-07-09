
using Microsoft.Extensions.Options;

namespace Multi_Tenanciy.Services
{
    public class TenentServices : ITenentServices
    {
        private HttpContext? _httpContext;
        private Tenant _currentTenant;
        private TenatnaSettings _tenatnaSettings;
        public TenentServices(IHttpContextAccessor httpContextAccessor , IOptions<TenatnaSettings> tentantOptions)
        {
            _httpContext = httpContextAccessor.HttpContext;
            _tenatnaSettings = tentantOptions.Value;
            if(_httpContext is not null)
            {
                if(_httpContext.Request.Headers.TryGetValue("tenant" , out var tenantId))
                    SetCurrentTenant(tenantId!);
                 else
                    throw new Exception("request must have tenant configurations.");
            } 
        }
        public string? GetConnectionString()
        {
            var connectionString = !string.IsNullOrEmpty(_currentTenant?.ConnectionString)
                ? _currentTenant?.ConnectionString
                : _tenatnaSettings.Defaluts.ConnectionString;

            return connectionString;
        }

        public Tenant? GetCurrentTenant()
        {
            return _currentTenant;
        }

        public string? GetDataBaseProvider()
        {
            return _tenatnaSettings.Defaluts.DbProvider;
        }


        private void SetCurrentTenant(string tenantId)
        {
            _currentTenant = _tenatnaSettings.Tenants.FirstOrDefault(t => t.TId == tenantId);
            if (_currentTenant is null)
            {
                throw new Exception("invalid request!");
            }

            if (string.IsNullOrEmpty(_currentTenant.ConnectionString))
            {
                // add defalut connection string.
                _currentTenant.ConnectionString = _tenatnaSettings.Defaluts.ConnectionString;
            }
        }
    }
}
