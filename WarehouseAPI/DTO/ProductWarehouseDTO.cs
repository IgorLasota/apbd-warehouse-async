namespace WarehouseAPI.DTO;

public class ProductWarehouseDto
{
    public int IdWarehouse { get; set; }
    public int IdProduct { get; set; } 
    public int Amount { get; set; }
    public DateTime CreatedAt { get; set; }
}