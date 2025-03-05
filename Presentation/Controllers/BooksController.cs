using Entities.DataTransferObjects;
using Entities.Exceptions;
using Entities.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/books")]
    public class BooksController : ControllerBase
    {

        private readonly IServiceManager _manager;
        public BooksController(IServiceManager manager)
        {

            _manager = manager;
        }



        [HttpGet]
        public async Task<IActionResult> GetAllBooksAsync()
        {
            //var books = manager.BookRepository.GetAllBooks(false);
            var books =await _manager.BookService.GetAllBooksAsync(false);
            return Ok(books);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetOneBookAsync([FromRoute(Name = "id")] int id)
        {
          
                //throw new Exception("!!!!");
                //var book = manager.BookRepository.GetOneBookById(id, false);//context.Books.Where(b => b.Id.Equals(id)).SingleOrDefault();
                var book =await _manager.BookService.GetOneBookByIdAsync(id, false);
               /*
                if (book is null)
                    //return NotFound(); //404
                    throw new BookNotFoundException(id);  Hata fırlatma işini serviste yapıcam.
               */
                return Ok(book);
            
            
        }

        [HttpPost]
        public async Task<IActionResult> AddOneBookAsync([FromBody] BookDtoForInsertion bookDtoforinsertion)
        {
            
                if (bookDtoforinsertion is null)
                    return BadRequest();
                if (!ModelState.IsValid)
                {
                    return UnprocessableEntity(ModelState);
                }
                await _manager.BookService.CreateOneBookAsync(bookDtoforinsertion);
                //manager.BookRepository.CreateOneBook(book);
                //manager.Save();
                // context.Books.Add(book);
                //context.SaveChanges(); //Değişikliği kalıcı olarak onaylamış oluruz.
                return StatusCode(201, bookDtoforinsertion);
            
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateOneBookAsync([FromRoute(Name = "id")] int id, [FromBody] BookDtoForUpdate bookDto)
        {
            
                if (bookDto is null)
                    return BadRequest(); //400
                if(!ModelState.IsValid)
                    return UnprocessableEntity(ModelState); //422
                /*
                //var entity = context.Books.Where(b => b.Id.Equals(id)).SingleOrDefault();
                var entity=manager.BookRepository.GetOneBookById(id,true);

                // check book
                if (entity is null)   
                    return NotFound();   //404

                //check id
                if (id != book.Id)
                    return BadRequest();   //400

                entity.Title = book.Title;
                entity.Price = book.Price;
                //context.SaveChanges();
                manager.Save();
                */
                await _manager.BookService.UpdateOneBookAsync(id, bookDto, false);
                //return Ok(book);
                return NoContent(); //204
            
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteOneBookAsync([FromRoute(Name = "id")] int id)
        {
            
                //var entity = context.Books.Where(b => b.Id.Equals(id)).SingleOrDefault();
                /*
                 var entity = manager.BookRepository.GetOneBookById(id, false);
                 if (entity is null)
                     return NotFound();
                 //context.Books.Remove(entity);
                 //context.SaveChanges();
                 manager.BookRepository.DeleteOneBook(entity);
                 manager.Save();
                */
                await _manager.BookService.DeleteOneBookAsync(id, false);
                return NoContent();

        }

        [HttpPatch("{id:int}")]

        public async Task<IActionResult> PartiallyUpdateOneBookAsync([FromRoute(Name = "id")] int id, [FromBody] JsonPatchDocument<BookDtoForUpdate> bookPach)
        {
            
                //var entity = context.Books.Where(b => b.Id.Equals(id)).SingleOrDefault();
                /*
                var entity=manager.BookRepository.GetOneBookById(id, true);
                if (entity is null)
                    return NotFound();

                book.ApplyTo(entity);
                //context.SaveChanges();
                manager.BookRepository.Update(entity);
                 */
                if(bookPach is null)
                return BadRequest();
              //  var bookDto = _manager.BookService.GetOneBookById(id, true);
              var result=await _manager.BookService.GetOneBookForPatchAsync(id,false);
            /*
                if (entity is null)
                    return NotFound();
            */
                bookPach.ApplyTo(result.bookDtoForUpdate,ModelState);
            /*
                _manager.BookService.UpdateOneBook(id, 
                    new BookDtoForUpdate()
                    { 
                        Id=bookDto.Id,
                        Title = bookDto.Title,
                        Price =bookDto.Price,
                        
                    }, true);
            */
            TryValidateModel(result.bookDtoForUpdate);
            if (!ModelState.IsValid)
                return UnprocessableEntity(ModelState);
            
            await _manager.BookService.SaveChangesForPatchAsync(result.bookDtoForUpdate,result.book);
            return NoContent();
            
        }
    }
}
