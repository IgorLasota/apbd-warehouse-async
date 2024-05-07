using Microsoft.EntityFrameworkCore;
using System.Data;
using Microsoft.Data.SqlClient;
using WarehouseAPI.DbContext;
using WarehouseAPI.DTO;
using WarehouseAPI.Interfaces;
using WarehouseAPI.Models;

namespace WarehouseAPI.Services
{
    public class WarehouseService(WarehouseDbContext context) : IWarehouseService
    {
        public async Task<WarehouseServiceResult> AddProductToWarehouseAsync(ProductWarehouseDto dto)
        {
            if (dto.Amount < 0)
            {
                return new WarehouseServiceResult
                {
                    Success = false,
                    Message = "Amount must be greater than 0",
                    Data = null
                };
            }
            
            var product = await context.Product.FindAsync(dto.IdProduct);
            if (product == null)
            {
                return new WarehouseServiceResult
                {
                    Success = false,
                    Message = "Product not found",
                    Data = null
                };
            }
            
            var warehouse = await context.Warehouse.FindAsync(dto.IdWarehouse);
            if (warehouse == null)
            {
                return new WarehouseServiceResult
                {
                    Success = false,
                    Message = "Warehouse not found",
                    Data = null
                };
            }
            
            var order = await context.Order.FirstOrDefaultAsync(o => o.IdProduct == dto.IdProduct && o.Amount >= dto.Amount && o.CreatedAt <= dto.CreatedAt && o.FulfilledAt == null);
            if (order == null)
            {
                return new WarehouseServiceResult
                {
                    Success = false,
                    Message = "No valid order, or order already fulfilled",
                    Data = null
                };
            }
            
            var existingProductWarehouse = await context.Product_Warehouse.AnyAsync(pw => pw.IdOrder == order.IdOrder);
            if (existingProductWarehouse)
            {
                return new WarehouseServiceResult
                {
                    Success = false,
                    Message = "Order has already been fulfilled",
                    Data = null
                };
            }
            
            order.FulfilledAt = DateTime.Now;
            await context.SaveChangesAsync();
            
            var productWarehouse = new ProductWarehouse
            {
                IdWarehouse = dto.IdWarehouse,
                IdProduct = dto.IdProduct,
                IdOrder = order.IdOrder,
                Amount = dto.Amount,
                Price = product.Price * dto.Amount,
                CreatedAt = DateTime.Now
            };

            context.Product_Warehouse.Add(productWarehouse);
            await context.SaveChangesAsync();
            
            return new WarehouseServiceResult
            {
                Success = true,
                Message = "Product added successfully",
                Data = new { ProductWarehouseId = productWarehouse.IdProductWarehouse }
            };
        }

        public async Task<WarehouseServiceResult> AddProductToWarehouseTransAsync(ProductWarehouseDto dto)
        {
            if (dto.Amount < 0)
            {
                return new WarehouseServiceResult
                {
                    Success = false,
                    Message = "Amount must be greater than 0",
                    Data = null
                };
            }
            
            var connection = context.Database.GetDbConnection();
            try
            {
                await connection.OpenAsync();
                using (var command = connection.CreateCommand())
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "AddProductToWarehouse";
                    command.Parameters.Add(new SqlParameter("@IdProduct", SqlDbType.Int) { Value = dto.IdProduct });
                    command.Parameters.Add(new SqlParameter("@IdWarehouse", SqlDbType.Int) { Value = dto.IdWarehouse });
                    command.Parameters.Add(new SqlParameter("@Amount", SqlDbType.Int) { Value = dto.Amount });
                    command.Parameters.Add(new SqlParameter("@CreatedAt", SqlDbType.DateTime) { Value = dto.CreatedAt });
                    
                    var result = await command.ExecuteScalarAsync();
                    if (result != null && Convert.ToInt32(result) > 0)
                    {
                        return new WarehouseServiceResult
                        {
                            Success = true,
                            Message = "Product added successfully",
                            Data = new { ProductWarehouseId = result }
                        };
                    }
                    else
                    {
                        return new WarehouseServiceResult
                        {
                            Success = false,
                            Message = "Failed to add product",
                            Data = null
                        };
                    }
                }
            }
            catch (SqlException ex)
            {
                return new WarehouseServiceResult
                {
                    Success = false,
                    Message = $"SQL error: {ex.Message}",
                    Data = null
                };
            }
            catch (Exception e)
            {
                return new WarehouseServiceResult
                {
                    Success = false,
                    Message = $"Internal server error: {e.Message}",
                    Data = null
                };
            }
            finally
            {
                await connection.CloseAsync();
            }
        }
    }
}

