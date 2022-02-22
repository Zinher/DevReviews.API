using System.Threading.Tasks;
using AutoMapper;
using DevReviews.API.Entities;
using DevReviews.API.Models;
using DevReviews.API.Persistence.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace DevReviews.API.Controllers
{
    [ApiController]
    [Route("api/products/{productId}/productreviews")]

    public class ProductReviewsController : ControllerBase
    {
        private readonly IProductRepository _repository;
        private readonly IMapper _mapper;

        public ProductReviewsController(IProductRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        //api/products/1/productsreviews/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int productId, int id){
            //Se não existir o id especificado, retornar NotFound();
            var productReview = await _repository.GetReviewByIdAsync(id);

            if (productReview == null)
            {
                return NotFound();
            }

            var productReviewDetails = _mapper.Map<ProductReviewDetailsViewModel>(productReview);

            return Ok(productReviewDetails);
        }
        
    //POST api/products/1/producsreviews
    [HttpPost]
    public async Task<IActionResult> Post(int productId, AddProductReviewInputModel model){
        //Se estiver com dados inválidos, retornar BadRequest()
        var productReview = new ProductReview(model.Author, model.Rating, model.Comments, productId);
        
        await _repository.AddReviewAsync(productReview);

        return CreatedAtAction(nameof(GetById), new { id = productReview.Id, productId = productId }, model);
    }
    }
}