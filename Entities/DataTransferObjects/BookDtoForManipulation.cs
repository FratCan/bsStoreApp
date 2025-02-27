using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DataTransferObjects
{
    public abstract record BookDtoForManipulation
    {

        protected BookDtoForManipulation() { }
        protected BookDtoForManipulation(string title, decimal price)
        {
            Title = title;
            Price = price;
        }

        // Burada bir nevi kural tanımı yaptım.
        [Required(ErrorMessage ="Title is a required field")]
        [MinLength(2, ErrorMessage ="Title must contains of at least 2 characters")]
        [MaxLength(50,ErrorMessage = "Title must contains of at max 2 characters")]
        public string Title { get; init; }

        [Required(ErrorMessage = "Price is a required field")]
        [Range(10, 1000)]
        public decimal Price { get; init; }
    }
}
