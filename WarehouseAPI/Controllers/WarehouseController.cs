using Microsoft.AspNetCore.Mvc;
using WarehouseAPI.DTO;
using System.Threading.Tasks;
using WarehouseAPI.DbContext;
using WarehouseAPI.Services;
using IWarehouseService = WarehouseAPI.Interfaces.IWarehouseService;

namespace WarehouseAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WarehouseController : ControllerBase
    {
        private readonly IWarehouseService _warehouseService;

        public WarehouseController(IWarehouseService warehouseService)
        {
            _warehouseService = warehouseService;
        }

        [HttpPost("addProduct")]
        public async Task<IActionResult> AddProductToWarehouse([FromBody] ProductWarehouseDto dto)
        {
            WarehouseServiceResult result = await _warehouseService.AddProductToWarehouseAsync(dto);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok(result.Data);
        }

        [HttpPost("addProductProcedure")]
        public async Task<IActionResult> AddProductToWarehouseTrans([FromBody] ProductWarehouseDto dto)
        {
            WarehouseServiceResult result = await _warehouseService.AddProductToWarehouseTransAsync(dto);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok(result.Data);
        }
    }
}