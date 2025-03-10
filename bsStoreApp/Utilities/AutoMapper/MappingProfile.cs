﻿using AutoMapper;
using Entities.DataTransferObjects;
using Entities.Models;

namespace bsStoreApp.Utilities.AutoMapper
{
    public class MappingProfile : Profile //Automapperden kalıtır.
    {
        public MappingProfile() {
            CreateMap<BookDtoForUpdate, Book>().ReverseMap();
            CreateMap<Book, BookDto>(); //ilk ifade source ifadem ikinci ifadem destination ifadem.
            CreateMap<BookDtoForInsertion, Book>();
        }
    }
}
