namespace PerfumeryProject.API.DTOs.CartItems
{
    public class CreateCartItemsDto
    {
        public Guid UserId { get; set; }
        public Guid? OrderId { get; set; }
        public string BrandName { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public bool IsOrdered { get; set; }
        public int CartNumber { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string Name { get; set; }
    }
}
