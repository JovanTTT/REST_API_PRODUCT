using FluentValidation;
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
        public async Task<ActionResult<List<ProductDTO>>> AddProduct(ProductDTO productdto)
        {
            FluentValidation.Results.ValidationResult result = await validator.ValidateAsync(productdto);
            if (!result.IsValid)
            {
                return BadRequest("Does not meet required format!");
            }
            if (productdto != null)
            {
                appDbContext.Products.Add(productdto);
                await appDbContext.SaveChangesAsync();
                return Ok(await appDbContext.Products.ToListAsync());
            }
            return BadRequest("Object instance not set");
        }

       // [HttpGet]
        //public async Task<ActionResult<List<ProductDTO>>> getAllProducts()
        //{
            //var products = await appDbContext.Products.ToListAsync();
           // return Ok(products);
        //}

        [HttpGet]
        public async Task<ActionResult<List<ProductDTO>>> GetAllProducts()
        {
            var products = await _productService.GetAllProductsAsync();
            return Ok(products);
        }

        //[HttpGet("{id:int}")]

        // public async Task<ActionResult<ProductDTO>> getProduct(int id)
        // {
        //   var product = await appDbContext.Products.FirstOrDefaultAsync(x => x.Id == id);
        //   if (product != null)
        //  {
        // return Ok(product);
        // }
        //return NotFound("Product not found");
        //}

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ProductDTO>> GetProduct(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product != null)
            {
                return Ok(product);
            }
            return NotFound("Product not found");
        }

        [HttpPut]

        public async Task<ActionResult<ProductDTO>> updateProduct(ProductDTO productdto)
        {
            var product = await appDbContext.Products.FirstOrDefaultAsync(x => x.Id == productdto.Id);
            FluentValidation.Results.ValidationResult result = await validator.ValidateAsync(productdto);
            if (!result.IsValid) {
                return BadRequest("Does not meet required format!");
            }
            if (product != null)
            {
                product!.Name = productdto.Name;
                product!.Description = productdto.Description;
                product!.Price = productdto.Price;
                await appDbContext.SaveChangesAsync();
                return Ok(product);
            }
            return BadRequest("Product not found");
        }

        [HttpDelete]

        public async Task<ActionResult<List<ProductDTO>>> deleteProduct(int id)
        {
            var product = await appDbContext.Products.FirstOrDefaultAsync(x => x.Id == id);
            if (product != null) { 
                appDbContext.Products.Remove(product);
                await appDbContext.SaveChangesAsync();
                return Ok(await appDbContext.Products.ToListAsync());
            }
            return NotFound("Product does not exist!");
        }
    }
}
