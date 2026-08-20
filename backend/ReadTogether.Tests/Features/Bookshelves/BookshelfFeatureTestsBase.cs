using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using ReadTogether.Infrastructure.Interfaces;

namespace ReadTogether.Tests.Features.Bookshelves
{
    public class BookshelfFeatureTestsBase
    {
        protected static readonly string[] UserIds = { "user-1", "user-2", "user-3" };
        protected static readonly string[] BookshelfNames = { "Want to Read", "Currently Reading", "Read" };

        protected readonly Mock<IBookshelfRepository> BookshelfRepositoryMock = new Mock<IBookshelfRepository>();
    }
}