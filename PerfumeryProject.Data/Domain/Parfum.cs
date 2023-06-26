namespace PerfumeryProject.Data.Domain
{
    public class Parfum : BaseEntity
    {
        public Parfum()
        {
            BrandName = new Brand();
        }
        public int BrandId { get; set; }
        public Brand BrandName { get; set; }
        public decimal Price { get; set; }

    }
}
