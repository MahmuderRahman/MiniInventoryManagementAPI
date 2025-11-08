using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using MiniInventoryManagementAPI.Context;
using MiniInventoryManagementAPI.DTOs;
using MiniInventoryManagementAPI.Models;

namespace MiniInventoryManagementAPI.Service
{
    public interface IOrderService
    {
        int CreateOrder(OrderDto dto);

        IEnumerable<OrderDto> GetOrderList();

    }

    public class OrderService : IOrderService
    {
        private readonly MiniInventoryDbContext _db;
        private readonly IMapper _mapper;

        public OrderService(MiniInventoryDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public int CreateOrder(OrderDto dto)
        {
            using (IDbContextTransaction transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    var order = _mapper.Map<Order>(dto);
                    _db.Orders.Add(order);
                    _db.SaveChanges();

                    if (dto.OrderItems != null && dto.OrderItems.Count > 0)
                    {
                        foreach (var itemDto in dto.OrderItems)
                        {
                            var product = _db.Products.FirstOrDefault(p => p.ProductId == itemDto.ProductId);
                            if (product == null)
                                throw new Exception("Product not found");

                            if (product.StockQuantity < itemDto.Quantity)
                                throw new Exception($"Stock insufficient for {product.Name}");

                            product.StockQuantity -= itemDto.Quantity;

                            var orderItem = _mapper.Map<OrderItem>(itemDto);
                            orderItem.OrderId = order.OrderId;
                            _db.OrderItems.Add(orderItem);
                        }
                    }

                    int rowAff = _db.SaveChanges();
                    transaction.Commit();

                    return rowAff;
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public IEnumerable<OrderDto> GetOrderList()
        {
            var dataList = _db.Orders
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                .AsQueryable()
                .ProjectTo<OrderDto>(_mapper.ConfigurationProvider)
                .ToList();

            foreach (var order in dataList)
            {
                order.ProductName = string.Join("<br>", order.OrderItems.Select(i =>
                    $"{i.ProductName} - {i.Quantity} × {i.UnitPrice} = {i.SubTotal}"));
            }

            return dataList;
        }

    }
}
