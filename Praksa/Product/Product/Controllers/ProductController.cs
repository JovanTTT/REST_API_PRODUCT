using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Product.Data;
using Product.DTO;
using Product.Model;
using Product.Service;
using System;
using System.ComponentModel.DataAnnotations;

namespace Product.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly AppDbContext appDbContext;

        private readonly IProductService _productService;

        private readonly IValidator<ProductDTO> validator;

        public ProductController(AppDbContext appDbContext, IProductService productService, IValidator<ProductDTO> validator)
        {
            this.appDbContext = appDbContext;
            _productService = productService;
            this.validator = validator;
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<List<ProductDTO>>> AddProduct(ProductDTO productDTO)
        {
            try
            {
                var products = await _productService.AddProductAsync(productDTO);
                return Ok(products);
            }
            catch (FluentValidation.ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet]
        [Authorize(Roles = "Admin, User")]
        public async Task<ActionResult<List<ProductDTO>>> GetAllProducts()
        {

            try
            {
                var products = await _productService.GetAllProductsAsync();
                return Ok(products);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("{id:int}")]
        [Authorize(Roles = "Admin, User")]
        public async Task<ActionResult<ProductDTO>> GetProduct(int id)
        {
            try
            {
                var product = await _productService.GetProductByIdAsync(id);
                if (product != null)
                {
                    return Ok(product);
                }
                return NotFound("Product not found");
            }
            catch(Exception ex) 
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ProductDTO>> UpdateProduct(ProductDTO productDTO)
        {
            try
            {
                var updatedProduct = await _productService.UpdateProductAsync(productDTO);
                return Ok(updatedProduct);
            }
            catch (FluentValidation.ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<List<ProductDTO>>> DeleteProduct(int id)
        {
            try
            {
                var productDeleted = await _productService.DeleteProductAsync(id);
                if (productDeleted)
                {
                    var products = await _productService.GetAllProductsAsync();
                    return Ok(products);
                }
                return NotFound("Product does not exist!");
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
