namespace CLOUDPART_MVC.Models.ViewModels
{
    public class ProductViewModel
    {
        public Guid id { get; set; }
        public string name { get; set; } = string.Empty;
        public string? description { get; set; }
    }
}
