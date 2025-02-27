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

        public BookDto CreateOneBook(BookDtoForInsertion bookDtoforinsertion)
        {
             var entity=_mapper.Map<Book>(bookDtoforinsertion);
              
            _manager.BookRepository.CreateOneBook(entity);
            _manager.Save();
            return _mapper.Map<BookDto>(entity);
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

        public BookDto GetOneBookById(int id, bool trackChanges)
        {
            
            //return _manager.BookRepository.GetOneBookById(id, trackChanges);
            var book= _manager.BookRepository.GetOneBookById(id, trackChanges);
            if (book is null)
            {
                throw new BookNotFoundException(id);
            }
            //return (book);
            return _mapper.Map<BookDto>(book);
        }

        public (BookDtoForUpdate bookDtoForUpdate, Book book) GetOneBookForPatch(int id, bool trackChanges)
        {
            var book = _manager.BookRepository.GetOneBookById(id, trackChanges);
            if (book is null)
            {
                throw new BookNotFoundException(id);
            }
            var bookDtoForUpdate=_mapper.Map<BookDtoForUpdate>(book);
            return(bookDtoForUpdate, book);
        }

        public void SaveChangesForPatch(BookDtoForUpdate bookDtoForUpdate, Book book)
        {
            _mapper.Map(bookDtoForUpdate, book);
            _manager.Save();
            
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
