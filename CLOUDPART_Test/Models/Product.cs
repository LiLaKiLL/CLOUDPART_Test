using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace CLOUDPART_Test.Models
{
    [Table("Product")]
    public class Product
    {
        [Key()]
        public Guid Id { get; set; }
        [Required]
        [StringLength(255)]
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
    }
}
