namespace RamenGo.API.Models
{
    public class Order
    {
        public string Id { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Image { get; set; } = string.Empty;
        public string BrothId { get; set; } = string.Empty;
        public string ProteinId { get; set; } = string.Empty;
    }
}