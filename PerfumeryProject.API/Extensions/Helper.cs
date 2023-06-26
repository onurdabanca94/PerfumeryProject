namespace PerfumeryProject.API.Extensions
{
    public static class Helper
    {
        public static bool HasValue(this IEnumerable<object> data)
        {
            return data != null && data.Count() > 0;
        }
    }
}
