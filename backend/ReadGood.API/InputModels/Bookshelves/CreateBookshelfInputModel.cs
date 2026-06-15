using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReadGood.API.InputModels.Bookshelf
{
    public class CreateBookshelfInputModel
    {
        public required string Name { get; set; }
    }
}