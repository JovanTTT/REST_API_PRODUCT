using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Product.BusinessLayer.DTO;
using Product.BusinessLayer.Service;
using Product.Data;
using Product.DTO;
using Product.Model;
using Product.Service;
using System;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace Product.Controllers
{
    [Route("api/[controller]")]
    //[Authorize]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService productsService;
        private readonly IValidator<ProductDTO> validator;
        public ProductController(IProductService productsService, IValidator<ProductDTO> validator)
        {
            this.productsService = productsService;
            this.validator = validator;
        }

        [HttpPost]
        public async Task<ActionResult<ProductDTO>> AddProduct(ProductDTO newProduct)
        {
            if (newProduct == null)
            {
                return BadRequest("Object instance not set1");
            }

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User ID not found in the token");
            }

            FluentValidation.Results.ValidationResult result = await validator.ValidateAsync(newProduct);
            if (!result.IsValid)
            {
                return BadRequest("Product data isn't valid");
            }
            try
            {
                return Ok(await productsService.AddProduct(newProduct, int.Parse(userId)));
            }
            catch (DbUpdateException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request");
            }
        }

        [HttpGet]
        public async Task<ActionResult<List<ProductDTO>>> GetAllProducts()
        {
            try
            {
                var allProducts = await productsService.GetAllProductsAsync();
                return Ok(allProducts);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request");
            }
        }

        [HttpGet("user")]
        public async Task<ActionResult<List<ProductDTO>>> getProductsForUser()
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized("User ID not found in the token");
                }
                var products = await productsService.GetProductsForUserAsync(int.Parse(userId));
                return Ok(products);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request");
            }

        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ProductDTO>> getProduct(int id)
        {
            try
            {
                ProductDTO product = await productsService.GetProduct(id);
                return Ok(product);
            }
            catch (DbUpdateException)
            {
                return BadRequest("Product not found");
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request");
            }

        }

        [HttpPut]
        public async Task<ActionResult<ProductDTO>> UpdateProduct(ProductDTO updatedProduct)
        {
            if (updatedProduct == null)
            {
                return BadRequest("Product not found");
            }

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User ID not found in the token");
            }

            FluentValidation.Results.ValidationResult result = await validator.ValidateAsync(updatedProduct);
            if (!result.IsValid)
            {
                return BadRequest("Object not updated");
            }

            try
            {
                ProductDTO product = await productsService.UpdateProduct(updatedProduct, int.Parse(userId));
                return Ok(product);
            }
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest("Not your product");
            }
            catch (DbUpdateException)
            {
                return BadRequest("Object not found");
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request");
            }

        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<ProductDTO>> DeleteProduct(int id)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized("User ID not found in the token");
                }

                ProductDTO product = await productsService.DeleteProductById(id, int.Parse(userId));
                return Ok(product);
            }
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest("Product not deleted");

            }
            catch (DbUpdateException)
            {
                return BadRequest("Product not found");
            }
            catch (ArgumentException)
            {
                return BadRequest("Product not yours");
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request");
            }

        }

        [HttpPut("assign-user/{productId:int}/{userId:int}")]
        public async Task<ActionResult<UsetDTO>> AssignProductToUser(int productId, int userId)
        {
            try
            {
                var user = await productsService.AssignProductToUser(productId, userId);
                return Ok(user);
            }
            catch (Exception ex)
            {
                if (ex.Message == ("Product not found")) return BadRequest("Product not found");
                if (ex.Message == ("User not found")) return BadRequest("User not found");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request");
            }
        }

        [HttpGet("statistics")]
        public async Task<ActionResult<Dictionary<string, object>>> GetProductStatistics()
        {
            var statistics = await productsService.GetProductStatisticsAsync();
            return Ok(statistics);
        }

        [HttpGet("popular")]
        public async Task<ActionResult<List<ProductPopularityDTO>>> GetMostPopularProducts([FromQuery] int topN = 10)
        {
            var popularProducts = await productsService.GetMostPopularProductsAsync(topN);
            return Ok(popularProducts);
        }
    }
}
