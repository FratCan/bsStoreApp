﻿using AutoMapper;
using Entities.DataTransferObjects;
using Entities.Exceptions;
using Entities.Models;
using Repositories.Contracts;
using Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class BookManager : IBookService
    {


        private readonly IRepositoryManager _manager;
        private readonly ILoggerService _logger;
        private readonly IMapper _mapper;


        public BookManager(IRepositoryManager manager, ILoggerService logger, IMapper mapper)
        {
            _manager = manager;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<BookDto> CreateOneBookAsync(BookDtoForInsertion bookDtoforinsertion)
        {
             var entity=_mapper.Map<Book>(bookDtoforinsertion);
              
            _manager.BookRepository.CreateOneBook(entity);
            _manager.SaveAsync();
            return _mapper.Map<BookDto>(entity);
        }

        public async Task DeleteOneBookAsync(int id, bool trackChanges)
        {
            var entity= await GetOneBookByIdAndCheckExists(id, trackChanges);
           // var entity= await _manager.BookRepository.GetOneBookByIdAsync(id,trackChanges); // // lı ifadeleri aşağıdaki bir metoda taşıdım.
           // if (entity is null) {
                /*
                string message=$"The book with id:{id} was not found";
                _logger.LogInfo(message);
                throw new Exception(message);
                */
             //   throw new BookNotFoundException(id);
            //}  
            _manager.BookRepository.DeleteOneBook(entity);
            await _manager.SaveAsync();

        }   

        public async Task<IEnumerable<BookDto>> GetAllBooksAsync(bool trackChanges)
        {
            _logger.LogInfo($"{nameof(GetAllBooksAsync)} books");

            //return _manager.BookRepository.GetAllBooks(trackChanges);
            var books =await _manager.BookRepository.GetAllBooksAsync(trackChanges);
            return _mapper.Map<IEnumerable<BookDto>>(books);
        }

        public async Task<BookDto> GetOneBookByIdAsync(int id, bool trackChanges)
        {
            
            //return _manager.BookRepository.GetOneBookById(id, trackChanges);
            var book=await GetOneBookByIdAndCheckExists(id,trackChanges);
            //var book=await _manager.BookRepository.GetOneBookByIdAsync(id, trackChanges);
            
           // if (book is null)
            //{
             //   throw new BookNotFoundException(id);
            //}
            //return (book);
            return _mapper.Map<BookDto>(book);
        }

        public async Task<(BookDtoForUpdate bookDtoForUpdate, Book book)> GetOneBookForPatchAsync(int id, bool trackChanges)
        {
            var book = await GetOneBookByIdAndCheckExists(id, trackChanges);
            /*
            var book =await _manager.BookRepository.GetOneBookByIdAsync(id, trackChanges);
            if (book is null)
            {
                throw new BookNotFoundException(id);
            }
            */
            var bookDtoForUpdate=_mapper.Map<BookDtoForUpdate>(book);
            return(bookDtoForUpdate, book);
        }

        public async Task SaveChangesForPatchAsync(BookDtoForUpdate bookDtoForUpdate, Book book)
        {
            _mapper.Map(bookDtoForUpdate, book);
            await _manager.SaveAsync();
            
        }

        public async Task UpdateOneBookAsync(int id, BookDtoForUpdate bookDto,bool trackChanges)
        {
            var entity = await GetOneBookByIdAndCheckExists(id, trackChanges);
            /*
           var entity=await _manager.BookRepository.GetOneBookByIdAsync(id,trackChanges);
            if (entity is null)
            {
                /*
                string message = $"The book with id:{id} was not found";
                _logger.LogInfo(message);
                throw new Exception(message);
                
            throw new BookNotFoundException(id);
            }
        */
            //Aşağıdaki gibi yapmak yerine automapper kullanıcam.
            // entity.Title = book.Title;
            // entity.Price = book.Price;
            entity = _mapper.Map<Book>(bookDto);

            _manager.BookRepository.UpdateOneBook(entity);
            await _manager.SaveAsync();
        }


        private async Task<Book> GetOneBookByIdAndCheckExists(int id,bool trackChanges)
        {
            var entity = await _manager.BookRepository.GetOneBookByIdAsync(id, trackChanges);
            if (entity is null)
            {
                throw new BookNotFoundException(id);
            }
            return entity;
        }
    }
}
