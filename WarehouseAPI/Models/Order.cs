using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WarehouseAPI.Models;

public class Order
{
    [Key]
    public int IdOrder { get; set; }
    
    [ForeignKey("Product")]
    public int IdProduct { get; set; }
    public int Amount { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? FulfilledAt { get; set; }
    
    public Product Product { get; set; }
}
