using AutoMapper;
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

        public Book CreateOneBook(Book book)
        {
            _manager.BookRepository.CreateOneBook(book);
            _manager.Save();
            return book;
        }

        public void DeleteOneBook(int id, bool trackChanges)
        {
            var entity= _manager.BookRepository.GetOneBookById(id,trackChanges);
            if (entity is null) {
                /*
                string message=$"The book with id:{id} was not found";
                _logger.LogInfo(message);
                throw new Exception(message);
                */
                throw new BookNotFoundException(id);
            }  
            _manager.BookRepository.DeleteOneBook(entity);
            _manager.Save();

        }   

        public IEnumerable<BookDto> GetAllBooks(bool trackChanges)
        {
            _logger.LogInfo($"{nameof(GetAllBooks)} books");

            //return _manager.BookRepository.GetAllBooks(trackChanges);
            var books = _manager.BookRepository.GetAllBooks(trackChanges);
            return _mapper.Map<IEnumerable<BookDto>>(books);
        }

        public Book GetOneBookById(int id, bool trackChanges)
        {
            
            //return _manager.BookRepository.GetOneBookById(id, trackChanges);
            var book= _manager.BookRepository.GetOneBookById(id, trackChanges);
            if (book is null)
            {
                throw new BookNotFoundException(id);
            }
            return (book);

        }

        public void UpdateOneBook(int id, BookDtoForUpdate bookDto,bool trackChanges)
        {
           var entity=_manager.BookRepository.GetOneBookById(id,trackChanges);
            if (entity is null)
            {
                /*
                string message = $"The book with id:{id} was not found";
                _logger.LogInfo(message);
                throw new Exception(message);
                */
                throw new BookNotFoundException(id);
            }
            //Aşağıdaki gibi yapmak yerine automapper kullanıcam.
            // entity.Title = book.Title;
            // entity.Price = book.Price;
            entity = _mapper.Map<Book>(bookDto);

            _manager.BookRepository.UpdateOneBook(entity);
            _manager.Save();
        }
    }
}
