using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using DevReviews.API.Entities;
using DevReviews.API.Models;
using DevReviews.API.Persistence.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace DevReviews.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _repository;
        private readonly IMapper _mapper;

        public ProductsController(IProductRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll() {
            var products = await _repository.GetAllAsync();

            // var productsViewModel = products
                // .Select(p => new ProductViewModel(p.Id, p.Title, p.Price));
            var productsViewModel = _mapper.Map<IEnumerable<ProductViewModel>>(products);

            return Ok(productsViewModel);
        }

        //GET para api/products/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id){
            //Se não achar retornar NotFound
            var product = await _repository.GetDetailsByIdAsync(id);
            
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
            
            await _repository.AddAsync(product);

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

            var product = await _repository.GetByIdAsync(id);

            if(product == null){
                return NotFound();
            }

            product.Update(model.Description, model.Price);
            await _repository.UpdateAsync(product);

            return NoContent();
        }
    }
}