namespace Multi_Tenanciy.Settings
{
    public class ProductServices : IProductServices
    {

        private readonly ApplicationDbContext _context;

        public ProductServices(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task CreateProduct(Products p)
        {
            await _context.Products.AddAsync(p);
        }

        public async Task DeleteProduct(int id)
        {
            _context.Products.Remove(await GetProductById(id));
            await _context.SaveChangesAsync();
        }

        public async Task EditProduct(Products p, int id)
        {
            _context.Entry(p).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task<Products> GetProductById(int id)
        {
            return await _context.Products.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IReadOnlyList<Products>> GetProducts()
        {
            return await _context.Products.ToListAsync();
        }
    }
}
