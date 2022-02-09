using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevReviews.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace DevReviews.API.Controllers
{
    [ApiController]
    [Route("api/products/{productId}/productreviews")]
    public class ProductReviewsController : ControllerBase
    {
        //api/products/1/productsreviews/5
        [HttpGet("{id}")]
        public IActionResult GetById(int productId, int id){
            //Se não existir o id especificado, retornar NotFound();
            return Ok();
        }
        
    //POST api/products/1/producsreviews
    [HttpPost]
    public IActionResult Post(int productId, AddProductReviewInputModel model){
        //Se estiver com dados inválidos, retornar BadRequest()
        return CreatedAtAction(nameof(GetById), new { id = 1, productId = 2 }, model);
    }
    }
}