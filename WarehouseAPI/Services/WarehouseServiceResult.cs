using WarehouseAPI.DTO;

namespace WarehouseAPI.Services

{
    public class WarehouseServiceResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }
    }
}