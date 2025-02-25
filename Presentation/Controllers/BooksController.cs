﻿using Entities.DataTransferObjects;
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
        public IActionResult GetAllBooks()
        {
            //var books = manager.BookRepository.GetAllBooks(false);
            var books = _manager.BookService.GetAllBooks(false);
            return Ok(books);
        }

        [HttpGet("{id:int}")]
        public IActionResult GetOneBook([FromRoute(Name = "id")] int id)
        {
          
                //throw new Exception("!!!!");
                //var book = manager.BookRepository.GetOneBookById(id, false);//context.Books.Where(b => b.Id.Equals(id)).SingleOrDefault();
                var book = _manager.BookService.GetOneBookById(id, false);
               /*
                if (book is null)
                    //return NotFound(); //404
                    throw new BookNotFoundException(id);  Hata fırlatma işini serviste yapıcam.
               */
                return Ok(book);
            
            
        }

        [HttpPost]
        public IActionResult AddOneBook([FromBody] Book book)
        {
            
                if (book is null)
                    return BadRequest();
                _manager.BookService.CreateOneBook(book);
                //manager.BookRepository.CreateOneBook(book);
                //manager.Save();
                // context.Books.Add(book);
                //context.SaveChanges(); //Değişikliği kalıcı olarak onaylamış oluruz.
                return StatusCode(201, book);
            
        }

        [HttpPut("{id:int}")]
        public IActionResult UpdateOneBook([FromRoute(Name = "id")] int id, [FromBody] BookDtoForUpdate bookDto)
        {
            
                if (bookDto is null)
                    return BadRequest(); //400
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
                _manager.BookService.UpdateOneBook(id, bookDto, false);
                //return Ok(book);
                return NoContent(); //204
            
        }

        [HttpDelete("{id:int}")]
        public IActionResult DeleteOneBook([FromRoute(Name = "id")] int id)
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
                _manager.BookService.DeleteOneBook(id, false);
                return NoContent();

        }

        [HttpPatch("{id:int}")]

        public IActionResult PartiallyUpdateOneBook([FromRoute(Name = "id")] int id, [FromBody] JsonPatchDocument<Book> book)
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
                var entity = _manager.BookService.GetOneBookById(id, true);
            /*
                if (entity is null)
                    return NotFound();
            */
                book.ApplyTo(entity);
                _manager.BookService.UpdateOneBook(id, new BookDtoForUpdate(entity.Id,entity.Title,entity.Price), false);
                return NoContent();
            
        }
    }
}
