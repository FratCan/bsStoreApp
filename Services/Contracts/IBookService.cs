﻿using Entities.DataTransferObjects;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Contracts
{
    public interface IBookService
    {
        Task<IEnumerable<BookDto>> GetAllBooksAsync(bool trackChanges);
        Task<BookDto> GetOneBookByIdAsync(int id,bool trackChanges);

        Task<BookDto> CreateOneBookAsync(BookDtoForInsertion book);

        Task DeleteOneBookAsync(int id,bool trackChanges);
        Task UpdateOneBookAsync(int id,BookDtoForUpdate bookDto,bool trackChanges);
        Task<(BookDtoForUpdate bookDtoForUpdate,Book book)> GetOneBookForPatchAsync(int id,bool trackChanges); // burdan sonuçları result. diyerek alıcam

        Task SaveChangesForPatchAsync(BookDtoForUpdate bookDtoForUpdate,Book book);
    }
}
