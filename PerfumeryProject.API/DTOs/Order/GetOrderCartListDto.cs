namespace PerfumeryProject.API.DTOs.Order
{
    public class GetOrderCartListDto
    {
        public Guid UserId { get; set; }
        public Guid? OrderId { get; set; }
        public string BrandName { get; set; }
        public string Fullname { get; set; }
        public decimal TotalPrice { get; set; }
        public int Quantity { get; set; }
        public string PerfumeName { get; set; }
        public DateTime OrderDate { get; set; }
    }
}
