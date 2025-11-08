using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using MiniInventoryManagementAPI.Context;
using MiniInventoryManagementAPI.DTOs;
using MiniInventoryManagementAPI.Models;
using MiniInventoryManagementAPI.ViewModels;

namespace MiniInventoryManagementAPI.Service
{
    public interface IProductService
    {
        int CreateProduct(ProductDto dto);

        int UpdateProduct(ProductDto dto);

        int DeleteProduct(int id);

        IEnumerable<ProductDto> GetProductList();

        List<DropDownViewModel> GetProductDropDown();

    }

    public class ProductService : IProductService
    {

        private readonly MiniInventoryDbContext _db;
        private readonly IMapper _mapper;

        public ProductService(MiniInventoryDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public IEnumerable<ProductDto> GetProductList()
        {
            return _mapper.ProjectTo<ProductDto>(_db.Products).ToList();
        }

        public int CreateProduct(ProductDto dto)
        {
            var product = _mapper.Map<Product>(dto);
            product.CreatedAt = DateTime.Now;

            _db.Products.Add(product);
            var rowAffected = _db.SaveChanges();
            return rowAffected;
        }       

        public int UpdateProduct(ProductDto dto)
        {
            var product = _db.Products.FirstOrDefault(x => x.ProductId == dto.ProductId);
            if (product == null)
                throw new Exception("Product not found.");

            _mapper.Map(dto, product);
            product.UpdatedAt = DateTime.Now;

            _db.Entry(product).State = EntityState.Modified;
            return _db.SaveChanges();
        }       

        public int DeleteProduct(int id)
        {
            var product = _db.Products.FirstOrDefault(x => x.ProductId == id);

            if (product == null)
                throw new Exception("Product not found.");

            _db.Entry(product).State = EntityState.Deleted;
            return _db.SaveChanges();
        }    

        public List<DropDownViewModel> GetProductDropDown()
        {
            return _db.Products.Select(d => new DropDownViewModel { id = (int)d.ProductId, name = d.Name, price=d.Price }).ToList();
        }

    }
}
