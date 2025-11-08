using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniInventoryManagementAPI.DTOs;
using MiniInventoryManagementAPI.Responses;
using MiniInventoryManagementAPI.Service;

using System.Net;

namespace MiniInventoryManagementAPI.Controllers.Product.API
{    
    [ApiController]
    public class ProductControllerApi : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductControllerApi(IProductService productService)
        {
            _productService = productService;
        }

        [HttpPost]
        [Route("api/product/create")]
        public IActionResult CreateProduct([FromBody] ProductDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                return Ok(new ApiResponse(_productService.CreateProduct(dto)));
            }
            catch (DbUpdateException e)
            {
                var sqlError = ExceptionHelper.ThrowSqlException(e);
                return StatusCode((int)HttpStatusCode.BadRequest, sqlError);
            }
            catch (Exception e)
            {
                var exceptionResponse = new ExceptionHelper(e);
                return StatusCode((int)HttpStatusCode.BadRequest, exceptionResponse);
            }
        }


        [HttpPost]
        [Route("api/product/update")]
        public IActionResult UpdateProduct([FromBody] ProductDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                return Ok(new ApiResponse(_productService.UpdateProduct(dto)));
            }
            catch (DbUpdateException e)
            {
                var sqlError = ExceptionHelper.ThrowSqlException(e);
                return StatusCode((int)HttpStatusCode.BadRequest, sqlError);
            }
            catch (Exception e)
            {
                var exceptionResponse = new ExceptionHelper(e);
                return StatusCode((int)HttpStatusCode.BadRequest, exceptionResponse);
            }
        }

        [HttpDelete]
        [Route("api/product/delete")]
        public IActionResult DeleteProduct(int id)
        {
            try
            {
                return Ok(new ApiResponse(_productService.DeleteProduct(id)));
            }
            catch (DbUpdateException e)
            {
                var sqlError = ExceptionHelper.ThrowSqlException(e);
                return StatusCode((int)HttpStatusCode.BadRequest, sqlError);
            }
            catch (Exception e)
            {
                var exceptionResponse = new ExceptionHelper(e);
                return StatusCode((int)HttpStatusCode.BadRequest, exceptionResponse);
            }
        }

        [HttpGet]
        [Route("api/product/get-all-products")]

        public IActionResult GetAllProducts()
        {
            try
            {
                return Ok(new ApiResponse(_productService.GetProductList()));
            }
            catch (DbUpdateException e)
            {
                var sqlError = ExceptionHelper.ThrowSqlException(e);
                return StatusCode((int)HttpStatusCode.BadRequest, sqlError);
            }
            catch (Exception e)
            {
                var exceptionResponse = new ExceptionHelper(e);
                return StatusCode((int)HttpStatusCode.BadRequest, exceptionResponse);
            }
        }

        [HttpGet]
        [Route("api/product/get-product-dropdown")]
        public IActionResult GetProductDropDown()
        {
            try
            {
                return Ok(new ApiResponse(_productService.GetProductDropDown()));

            }
            catch (DbUpdateException e)
            {
                var sqlError = ExceptionHelper.ThrowSqlException(e);
                return StatusCode((int)HttpStatusCode.BadRequest, sqlError);
            }
            catch (Exception e)
            {
                var exceptionResponse = new ExceptionHelper(e);
                return StatusCode((int)HttpStatusCode.BadRequest, exceptionResponse);
            }
        }

    }
}
