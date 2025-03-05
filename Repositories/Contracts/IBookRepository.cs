using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Contracts
{
    public interface IBookRepository:IRepositoryBase<Book>
    {
        void CreateOneBook(Book book);
        void DeleteOneBook(Book book);
        Task<IEnumerable<Book>> GetAllBooksAsync(bool trackChanges);
        Task<Book> GetOneBookByIdAsync(int id,bool trackChanges);
        void UpdateOneBook(Book book);
    }
}
