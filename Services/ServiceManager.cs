using AutoMapper;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Repositories.Contracts;
using Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class ServiceManager : IServiceManager
    {
        private readonly Lazy<IBookService> _bookService;

        public ServiceManager(IRepositoryManager repositoryManager,ILoggerService logger,IMapper mapper)
        {
            _bookService= new Lazy<IBookService>(()=>new BookManager(repositoryManager,logger,mapper));
        }

        //ctr yerine alan tanıma yaparsam  yani IBookService gibi yaparsam class'ın geri kalnında da kullanmak zorunda kalırım.

        public IBookService BookService => _bookService.Value;
    }
}
