using Moq;
using UseCases = HC.Codeflix.Catalog.Application.UseCases.CreateCategory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Xunit;
using FluentAssertions;
using HC.Codeflix.Catalog.Domain.Entity;
using HC.Codeflix.Catalog.Domain.Repository;
using HC.Codeflix.Catalog.Application.Interfaces;

namespace HC.Codeflix.Catalog.UnitTests.Application.CreateCategory;
public class CreateCategoryTest
{
    public void CreateCategory()
    {



        [Fact(DisplayName = nameof(CreateCategory))]
        [Trait("Application", "CreateCategory - Use Cases")]
        public async void CreateCategory()
        {
            var repositoryMock = new Mock<ICategoryRepository>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var useCase = new UseCases.CreateCategory(
                repositoryMock.Object,
                unitOfWorkMock.Object
                );

            var input = new CreateCategoryInput(
                "Category Name",
                "Category Description",
                true
                );

            var output = await useCase.Handle(input, CancellationToken.None);

            repositoryMock.Verify(
                repositoryMock => repositoryMock.Insert(
                    It.IsAny<Category>(),
                    It.IsAny<CancellationToken>()
                    ),
                Times.Once
                );
            unitOfWorkMock.Verify(
                uow => uow.Commit(It.IsAny<CancellationToken>()),
                Times.Once
                );

            output.ShouldNotBeNull();
            output.Name.Should().Be("Category Name");
            output.Description.Should().Be("Category Description");
            output.IsActive.Should().Be(true);
            (output.Id != null && output.Id != Guid.Empty).Should().BeTrue();
            (output.CreatedAt != null && output.CreatedAt != default(DateTime)).Should().BeTrue();


        }
    }

}
