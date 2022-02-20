using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DevReviews.API.Entities;
using DevReviews.API.Models;
using DevReviews.API.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DevReviews.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly DevReviewsDbContext _dbContext;
        private readonly IMapper _mapper;

        public ProductsController(DevReviewsDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        [HttpGet]
        public IActionResult GetAll() {
            var products = _dbContext.Products;

            // var productsViewModel = products
                // .Select(p => new ProductViewModel(p.Id, p.Title, p.Price));
            var productsViewModel = _mapper.Map<IEnumerable<ProductViewModel>>(products);

            return Ok(productsViewModel);
        }

        //GET para api/products/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id){
            //Se não achar retornar NotFound
            var product = await _dbContext
                .Products
                .Include(p => p.Reviews)
                .SingleOrDefaultAsync(p => p.Id == id);
            
            if(product == null){
                return NotFound();
            }

            //Without AutoMapper
            // var reviewsViewModel = product
            //     .Reviews
            //     .Select(r => new ProductReviewViewModel(r.Id, r.Author, r.Rating, r.Comments, r.RegisteredAt))
            //     .ToList();

            // var productDetailsViewModel = new ProductDetailsViewModel(
            //     product.Id,
            //     product.Title,
            //     product.Description,
            //     product.Price,
            //     product.RegisteredAt,
            //     reviewsViewModel
            // );

            var productDetailsViewModel = _mapper.Map<ProductDetailsViewModel>(product);

            return Ok(productDetailsViewModel);
        }

        //POST para api/products
        [HttpPost]
        public async Task<IActionResult> Post(AddModelInputModel model) {
            //Se tiver erros de validação, retornar BadRequest()
            var product = new Product(model.Title, model.Description, model.Price);
            await _dbContext.Products.AddAsync(product);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = product.Id }, model);
        }
        
        //PUT para api/products/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PUT(int id, UpdateProductInputModel model) {
            //Se tiver erros de validação, retornar BadRequest()
            //Se não existir produto com o id especificado, retornar NotFound()
            
            if(model.Description.Length > 50){
                return BadRequest();
            }

            var product = await _dbContext.Products.SingleOrDefaultAsync(p => p.Id == id);

            if(product == null){
                return NotFound();
            }

            product.Update(model.Description, model.Price);
            await _dbContext.SaveChangesAsync();

            return NoContent();
        }
    }
}