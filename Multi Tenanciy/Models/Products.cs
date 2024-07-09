namespace Multi_Tenanciy.Models
{
    public class Products : IMustHaveTenant
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Rate { get; set; }

        public string TenantId { get; set; }
    }
}
