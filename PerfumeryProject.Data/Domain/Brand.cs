namespace PerfumeryProject.Data.Domain
{
    public class Brand : BaseEntity
    {
        public Brand()
        {
            Parfums = new List<Parfum>();
        }
        public ICollection<Parfum> Parfums { get; set; }
    }
}
