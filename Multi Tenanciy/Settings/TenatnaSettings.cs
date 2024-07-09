namespace Multi_Tenanciy.Settings
{
    public class TenatnaSettings
    {
        public Configruation Defaluts { get; set; } = default!;
        public List<Tenant> Tenants { get; set; } = new();
    }
}
