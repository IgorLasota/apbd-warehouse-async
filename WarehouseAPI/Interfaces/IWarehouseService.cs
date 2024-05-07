using WarehouseAPI.DTO;
using WarehouseAPI.Services;

namespace WarehouseAPI.Interfaces

{
    public interface IWarehouseService
    {
        Task<WarehouseServiceResult> AddProductToWarehouseAsync(ProductWarehouseDto dto);
        Task<WarehouseServiceResult> AddProductToWarehouseTransAsync(ProductWarehouseDto dto);
    }
}