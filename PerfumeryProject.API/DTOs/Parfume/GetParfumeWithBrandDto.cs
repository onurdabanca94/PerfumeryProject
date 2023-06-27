namespace PerfumeryProject.API.DTOs.Parfume
{
    public class GetParfumeWithBrandDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int BrandId { get; set; }
        public decimal Price { get; set; }
        public string BrandName { get; set; }
    }
}
