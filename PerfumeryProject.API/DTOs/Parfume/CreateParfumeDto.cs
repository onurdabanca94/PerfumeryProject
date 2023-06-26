namespace PerfumeryProject.API.DTOs.Parfume
{
    public class CreateParfumeDto
    {
        public string Name { get; set; }
        public int BrandId { get; set; }
        public decimal Price { get; set; }
    }
}
