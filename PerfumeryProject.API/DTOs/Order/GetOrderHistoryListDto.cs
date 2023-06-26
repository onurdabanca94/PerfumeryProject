namespace PerfumeryProject.API.DTOs.Order
{
    public class GetOrderHistoryListDto
    {
        public Guid OrderId { get; set; }
        public string OrderName { get; set; }
    }
}
