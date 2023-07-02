using CleanArchMvc.Application.DTOs;
using CleanArchMvc.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchMvc.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetAll()
        {
            var products = await _productService.GetProducts();

            if (products is null)
            {
                return NotFound("Products not found");
            }

            return Ok(products);
        }

        [HttpGet("{id}", Name = "GetProduct")]
        public async Task<ActionResult<ProductDTO>> GetById(int id)
        {
            var product = await _productService.GetById(id);

            if (product is null)
            {
                return NotFound("Product not found");
            }

            return Ok(product);
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] ProductDTO productDTO)
        {
            if (productDTO == null)
            {
                return BadRequest("Invalid Data");
            }

            await _productService.Add(productDTO);

            return new CreatedAtRouteResult("GetProduct", new { id = productDTO.Id }, productDTO);
        }

        [HttpPut]
        public async Task<ActionResult> Edit(int id, [FromBody] ProductDTO productDTO)
        {
            if (id != productDTO.Id)
                return BadRequest();

            if (productDTO is null)
                return BadRequest();

            await _productService.Update(productDTO);

            return Ok(productDTO);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ProductDTO>> Delete(int id)
        {
            var product = await _productService.GetById(id);

            if (product is null)
                return NotFound("Product not found");

            await _productService.Remove(id);

            return Ok(product);
        }
    }
}
