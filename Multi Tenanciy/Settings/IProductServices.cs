namespace Multi_Tenanciy.Settings
{
    public interface IProductServices
    {
        Task CreateProduct(Products p);
        Task<IReadOnlyList<Products>> GetProducts();
        Task<Products> GetProductById(int id);
        Task EditProduct(Products p , int id);
        Task DeleteProduct(int id);
    }
}
