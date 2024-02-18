using HC.Codeflix.Catalog.Domain.Exceptions;
using Xunit;
using DomainEntity = HC.Codeflix.Catalog.Domain.Entity;
using System.Linq;
using FluentAssertions;

namespace HC.Codeflix.Catalog.UnitTests.Domain.Entity.Category;

[Collection(nameof(CategoryTestFixture))]
public class CategoryTest
{
    private readonly CategoryTestFixture _categoryTestFixture;

    public CategoryTest(CategoryTestFixture categoryTestFixture)
        => _categoryTestFixture = categoryTestFixture;
    

    [Fact(DisplayName = nameof(Instantiate))]
    [Trait("Domain", "Category - Aggregates")]
    public void Instantiate()
    {
        //Arrange
        var validCategory = _categoryTestFixture.GetValidCategory();
        //Act
        var datetimeBefore = DateTime.Now;
        var category = new DomainEntity.Category(validCategory.Name, validCategory.Description);
        var datetimeAfter = DateTime.Now;
        //Assert

        //Assert.NotNull(category);
        category.Should().NotBeNull();
        category.Name.Should().Be(validCategory.Name);
        category.Description.Should().Be(validCategory.Description);
        category.Id.Should().NotBe(default(Guid));
        //Assert.NotEqual(default(DateTime), category.CreatedAt);
        category.CreatedAt.Should().NotBeSameDateAs(default(DateTime));

        (category.CreatedAt > datetimeBefore).Should().BeTrue();
        //Assert.True(category.CreatedAt > datetimeBefore);
        (category.CreatedAt < datetimeAfter).Should().BeTrue();
        //Assert.True(category.CreatedAt < datetimeAfter);
        category.IsActive.Should().BeTrue();
        //Assert.True(category.IsActive);

    }

    [Theory(DisplayName = nameof(InstantiateWithIsActive))]
    [Trait("Domain", "Category - Aggregates")]
    [InlineData(true)]
    [InlineData(false)]
    public void InstantiateWithIsActive(bool isActive)
    {
        //Arrange
        var validCategory = _categoryTestFixture.GetValidCategory();
        //Act
        var datetimeBefore = DateTime.Now;
        var category = new DomainEntity.Category(validCategory.Name, validCategory.Description, isActive);
        var datetimeAfter = DateTime.Now.AddSeconds(1);
        //Assert

        category.Should().NotBeNull();
        category.Name.Should().Be(validCategory.Name);
        category.Description.Should().Be(validCategory.Description);
        category.Id.Should().NotBe(default(Guid));
        //Assert.NotEqual(default(DateTime), category.CreatedAt);
        category.CreatedAt.Should().NotBeSameDateAs(default(DateTime));

        (category.CreatedAt >= datetimeBefore).Should().BeTrue();
        //Assert.True(category.CreatedAt > datetimeBefore);
        (category.CreatedAt <= datetimeAfter).Should().BeTrue();
        //Assert.True(category.CreatedAt < datetimeAfter);
        category.IsActive.Should().Be(isActive);

    }

    [Theory(DisplayName = nameof(InstatiateErrorWhenNameIsEmpty))]
    [Trait("Domain", "Category - Aggregates")]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("  ")]

    public void InstatiateErrorWhenNameIsEmpty(string? name) 
    {
        //Action action =
        //() => new DomainEntity.Category(name!, "Category Description");
        //var  exception = Assert.Throws<EntityValidationException>(action);
        //Assert.Equal("Name should not be empty or null.", exception.Message);

        var validCategory = _categoryTestFixture.GetValidCategory();

        Action action =
            () => new DomainEntity.Category(name!, validCategory.Description);

        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage("Name should not be empty or null.");

    }

    //nome deve ter no minimo 3 caracteres
    //nome deve ter no máx 255 caracteres
    //A descricao deve ter no max 10_000 caracteres

    [Fact(DisplayName = nameof(InstantiateErrorWhenDescriptionIsNull))]
    [Trait("Domain", "Category - Aggregates")]

    public void InstantiateErrorWhenDescriptionIsNull()
    {
        var validCategory = _categoryTestFixture.GetValidCategory();

        Action action =
            () => new DomainEntity.Category(validCategory.Name, null!);

        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage("Description should not be null.");
    }

    [Theory(DisplayName = nameof(InstantiateErrorWhenNameLessThan3Characters))]
    [Trait("Domain", "Category - Aggregates")]
    [MemberData(nameof(GetNamesWithLessThan3Characters), parameters: 10)]
    

    public void InstantiateErrorWhenNameLessThan3Characters(string invalidName)
    {
        var validCategory = _categoryTestFixture.GetValidCategory();

        Action action =
            () => new DomainEntity.Category(invalidName, validCategory.Description);
       action.Should()
            .Throw<EntityValidationException>()
            .WithMessage("Name should be at least 3 characters long.");
    }

    public static IEnumerable<object[]> GetNamesWithLessThan3Characters(int numberOfTests = 6)
    {
        var fixture = new CategoryTestFixture();
        for(int i = 0; i<6; i++)
        {
            var isOdd = i % 2 == 1;
            yield return new object[] {
                fixture.GetValidCategoryName()[..(isOdd ? 1 : 2)] };
        }
    }

    [Fact(DisplayName = nameof(InstantiateErrorWhenNameLessThan3Characters))]
    [Trait("Domain", "Category - Aggregates")]

    public void InstantiateErrorWhenNameGreaterThan255Characters()
    {
        var validCategory = _categoryTestFixture.GetValidCategory();

        var invalidName = String.Join(null, Enumerable.Range(0, 256).Select(_ => "a").ToArray());
        Action action =
            () => new DomainEntity.Category(invalidName, validCategory.Description);
        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage("Name should be less or equal 255 characters long.");
       
    }

    [Fact(DisplayName = nameof(InstantiateErrorWhenDescriptionGreaterThan10_000Characters))]
    [Trait("Domain", "Category - Aggregates")]

    public void InstantiateErrorWhenDescriptionGreaterThan10_000Characters()
    {
        var validCategory = _categoryTestFixture.GetValidCategory();

        var invalidDescription = String.Join(null, Enumerable.Range(0, 10001).Select(_ => "a").ToArray());
        Action action =
            () => new DomainEntity.Category(validCategory.Name, invalidDescription);
        action.Should()
           .Throw<EntityValidationException>()
           .WithMessage("Description should be less or equal 10000 characters long.");
        
    }

    [Fact(DisplayName = nameof(Activate))]
    [Trait("Domain", "Category - Aggregates")]
    
    public void Activate()
    {

        var validCategory = _categoryTestFixture.GetValidCategory();

        var category = new DomainEntity.Category(validCategory.Name, validCategory.Description, false);
        category.Activate();

        category.IsActive.Should().BeTrue();
    }

    [Fact(DisplayName = nameof(Deactivate))]
    [Trait("Domain", "Category - Aggregates")]

    public void Deactivate()
    {

        var validCategory = _categoryTestFixture.GetValidCategory();

        var category = new DomainEntity.Category(validCategory.Name, validCategory.Description, true);
        category.Deactivate();

        category.IsActive.Should().BeFalse();
    }

    [Fact(DisplayName = nameof(Update))]
    [Trait("Domain", "Category - Aggregates")]

    public void Update()
    { 

        var category = _categoryTestFixture.GetValidCategory();
        var categoryWithNewValues = _categoryTestFixture.GetValidCategory();

        category.Update(categoryWithNewValues.Name, categoryWithNewValues.Description);


        //Assert.Equal(newValues.Name, category.Name);
        categoryWithNewValues.Name.Should().Be(category.Name);
        //Assert.Equal(newValues.Description, category.Description);
        categoryWithNewValues.Description.Should().Be(category.Description);
    }

    [Fact(DisplayName = nameof(UpdateOnlyName))]
    [Trait("Domain", "Category - Aggregates")]

    public void UpdateOnlyName()
    {
        var category = _categoryTestFixture.GetValidCategory();
        var newName = _categoryTestFixture.GetValidCategoryName();
        var currentDescription = category.Description;

        category.Update(newName);


        //Assert.Equal(newValues.Name, category.Name);
        category.Name.Should().Be(newName);
        //Assert.Equal(category.Description, currentDescription);
        category.Description.Should().Be(currentDescription);
    }

    [Theory(DisplayName = nameof(UpdateErrorWhenNameIsEmpty))]
    [Trait("Domain", "Category - Aggregates")]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("  ")]

    public void UpdateErrorWhenNameIsEmpty(string? name)
    {
        var category = _categoryTestFixture.GetValidCategory();
        Action action =
            () => category.Update(name!);
        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage("Name should not be empty or null.");
        

    }

    [Theory(DisplayName = nameof(UpdateErrorWhenNameLessThan3Characters))]
    [Trait("Domain", "Category - Aggregates")]
    [InlineData("Um")]
    [InlineData("ca")]
    [InlineData("a")]

    public void UpdateErrorWhenNameLessThan3Characters(string invalidName)
    {
        var category = _categoryTestFixture.GetValidCategory();
        Action action =
            () => category.Update(invalidName);
        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage("Name should be at least 3 characters long.");
       
    }

    [Fact(DisplayName = nameof(UpdateErrorWhenNameLessThan3Characters))]
    [Trait("Domain", "Category - Aggregates")]

    public void UpdateErrorWhenNameGreaterThan255Characters()
    {
        var category = _categoryTestFixture.GetValidCategory();
        var invalidName = _categoryTestFixture.Faker.Lorem.Letter(256);
        Action action =
            () => category.Update(invalidName);
        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage("Name should be less or equal 255 characters long.");
        
    }

    [Fact(DisplayName = nameof(UpdateErrorWhenDescriptionGreaterThan10_000Characters))]
    [Trait("Domain", "Category - Aggregates")]

    public void UpdateErrorWhenDescriptionGreaterThan10_000Characters()
    {
        var category = _categoryTestFixture.GetValidCategory();
        var invalidDescription = 
            _categoryTestFixture.Faker.Commerce.ProductDescription();
        while (invalidDescription.Length <= 10_000)
            invalidDescription = $"{invalidDescription} {_categoryTestFixture.Faker.Commerce.ProductDescription()}";
        Action action =
            () => category.Update("Category New Name", invalidDescription);
        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage("Description should be less or equal 10000 characters long.");
        
    }

}
