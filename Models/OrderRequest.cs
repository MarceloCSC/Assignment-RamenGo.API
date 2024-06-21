using System.ComponentModel.DataAnnotations;

namespace RamenGo.API.Models
{
    public class OrderRequest
    {
        [Required]
        public string BrothId { get; set; } = string.Empty;

        [Required]
        public string ProteinId { get; set; } = string.Empty;
    }
}