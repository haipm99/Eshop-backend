using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Eshop.WebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Eshop.WebApi.Controllers
{
    [Route("api/owner")]
    [ApiController]
    public class OwnerController : ControllerBase
    {
        protected EshopContext _context;
        public OwnerController(EshopContext context)
        {
            _context = context;
        }

        //url : api/owner/createShop
        //desc : create a shop
        [HttpPost("createShop")]
        [Authorize(Roles = "Shop owner")]
        public IActionResult CreateShop(Shops shop)
        {
            var result = _context.Shops.FirstOrDefault(s => s.Id == shop.Id);
            if(result != null)
            {
                return BadRequest("Shop have exist");
            }

            _context.Shops.Add(shop);
            _context.SaveChanges();
            return Ok(shop);
        }

        //url: api/owner/createProduct
        //desc: create a product
        [HttpPost("createProduct")]
        [Authorize(Roles = "Shop owner")]
        public IActionResult CreateProduct(Products product)
        {
            _context.Products.Add(product);
            _context.SaveChanges();
            return Ok(product);
        }

        //url: api/owner/getProduct
        //desc : get All product of shop
        [HttpGet("products/{id}")]
        [Authorize(Roles = "Shop owner")]
        public IActionResult GetAllProduct(int Id)
        {

            return Ok();
        }
        
        //url : api/owner/getDetailProduct
        //desc : get detail of 1 product
        [HttpGet("detailProduct")]
        [Authorize(Roles = "Shop owner")]
        public IActionResult GetDetailProduct(int Id)
        {
            var pro = _context.Products.Where(p => p.Id == Id);
            if(pro == null)
            {
                return BadRequest("Not Found Product.");
            }

            return Ok(pro);
        }

        //url : api/owner/deleteProduct
        //desc: delete 1 product
        [HttpDelete("deleteProduct")]
        [Authorize(Roles = "Shop owner")]
        public IActionResult DeleteProduct(int Id)
        {
            var pro = _context.Products.FirstOrDefault(p => p.Id == Id);
            if(pro != null)
            {
                try
                {
                    pro.Status = false;
                    _context.SaveChanges();
                    return Ok("Delete Successfull");
                }
                catch(Exception ex)
                {
                    throw;
                }
            }
            return BadRequest("Delete Failed");
        }
    }
}
