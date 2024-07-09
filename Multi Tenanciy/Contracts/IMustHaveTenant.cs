namespace Multi_Tenanciy.Contracts
{
    public interface IMustHaveTenant
    {
        public string TenantId { get; set; } 
    }
}
