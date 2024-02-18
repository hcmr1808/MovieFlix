using Bogus;
using Xunit;
using HC.Codeflix.Catalog.Domain.Validation;
using FluentAssertions;
using HC.Codeflix.Catalog.Domain.Exceptions;
using System.ComponentModel;
using FluentAssertions.Specialized;

namespace HC.Codeflix.Catalog.UnitTests.Domain.Validation;
public class DomainValidationTest
{

    private Faker Faker { get ; set; } = new Faker();

    [Fact(DisplayName = nameof(NotNullOk))]
    [Trait("Domain", "DomainValidation - Validation")]
    public void NotNullOk()
    {
        var value = Faker.Commerce.ProductName();
        var fieldName = Faker.Commerce.ProductName().Replace(" ", "");
        Action action = 
            () => DomainValidation.NotNull(value, fieldName);
        action.Should().NotThrow();
        
    }

    [Fact(DisplayName = nameof(NotNullThrowWhenNull))]
    [Trait("Domain", "DomainValidation - Validation")]
    public void NotNullThrowWhenNull()
    {
        string? value = null;
        var fieldName = Faker.Commerce.ProductName().Replace(" ", "");
        Action action =
            () => DomainValidation.NotNull(value!, fieldName);
        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage($"{fieldName} should not be null.");

    }

    //nao ser null ou vazio
    [Theory(DisplayName = nameof(NotNullOrEmptyThrowWhenEmpty))]
    [Trait("Domain", "DomainValidation - Validation")]
    [InlineData("")]
    [InlineData("  ")]
    [InlineData(null)]
    public void NotNullOrEmptyThrowWhenEmpty(string? target)
    {
        var fieldName = Faker.Commerce.ProductName().Replace(" ", "");
        
        Action action =
            () => DomainValidation.NotNullOrEmpty(target, fieldName);
        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage($"{fieldName} should not be empty or null.");

    }

    [Fact(DisplayName = nameof(NotNullOrEmptyOk))]
    [Trait("Domain", "DomainValidation - Validation")]
    public void NotNullOrEmptyOk()
    {
        var target = Faker.Commerce.ProductName();
        var fieldName = Faker.Commerce.ProductName().Replace(" ", "");

        Action action =
            () => DomainValidation.NotNull(target, fieldName);
        action.Should().NotThrow();

    }
    [Theory(DisplayName = nameof(MinLengthThrowWhenMin))]
    [Trait("Domain", "DomainValidation - Validation")]
    [MemberData(nameof(GetValuesSmallerThanMin),parameters: 10)]
    
    public void MinLengthThrowWhenMin(string target, int minLength)
    {
        var fieldName = Faker.Commerce.ProductName().Replace(" ", "");
        Action action =
            () => DomainValidation.MinLength(target, minLength, fieldName); 
        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage($"{fieldName} should be at least {minLength} characters long.");
    }

    public static IEnumerable<object[]> GetValuesSmallerThanMin(int numberoftests = 5)
    {
        yield return new object[] { "123456" , 10};
        var faker = new Faker();
        for(int i = 0; i<numberoftests; i++)
        {
            var example = faker.Commerce.ProductName();
            var minLength = example.Length + (new Random()).Next(1, 20);
            yield return new object[] { example, minLength };
        }
    }

    [Theory(DisplayName = nameof(MinLengthOk))]
    [Trait("Domain", "DomainValidation - Validation")]
    [MemberData(nameof(GetValuesGreaterThanMin), parameters: 10)]

    public void MinLengthOk(string target, int minLength)
    {
        var fieldName = Faker.Commerce.ProductName().Replace(" ", "");
        Action action =
            () => DomainValidation.MinLength(target, minLength, fieldName);
        action.Should().NotThrow();
    }

    public static IEnumerable<object[]> GetValuesGreaterThanMin(int numberoftests = 5)
    {
        yield return new object[] { "123456", 6 };
        var faker = new Faker();
        for (int i = 0; i < numberoftests; i++)
        {
            var example = faker.Commerce.ProductName();
            var minLength = example.Length - (new Random()).Next(1, 5);
            yield return new object[] { example, minLength };
        }
    }

    [Theory(DisplayName = nameof(MaxLengthThrowWhenGreater))]
    [Trait("Domain", "DomainValidation - Validation")]
    [MemberData(nameof(GetValuesGreaterThanMax), parameters: 10)]

    public void MaxLengthThrowWhenGreater(string target, int maxLength)
    {
        var fieldName = Faker.Commerce.ProductName().Replace(" ", "");
        Action action =
            () => DomainValidation.MaxLength(target, maxLength, fieldName);
        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage($"{fieldName} should be less or equal {maxLength} characters long.");
    }

    public static IEnumerable<object[]> GetValuesGreaterThanMax(int numberoftests = 5)
    {
        yield return new object[] { String.Join(null, Enumerable.Range(0, 10001).Select(_ => "a").ToArray()), 10000 };
        var faker = new Faker();

        for(int i = 0; i < (numberoftests - 1); i++)
        {
            var example = faker.Commerce.ProductDescription();
            var maxLength = example.Length - (new Random()).Next(1, 5);
            yield return new object[] { example, maxLength };
        }

    }

    [Theory(DisplayName = nameof(MaxLengthOk))]
    [Trait("Domain", "DomainValidation - Validation")]
    [MemberData(nameof(GetValuesLessThanMax), parameters:10)]

    public void MaxLengthOk(string target, int maxLength)
    {
        var fieldName = Faker.Commerce.ProductName().Replace(" ", "");
        Action action =
            () => DomainValidation.MaxLength(target, maxLength, fieldName);
        action.Should().NotThrow();

    }

    public static IEnumerable<object[]> GetValuesLessThanMax(int numberoftests = 5)
    {
        yield return new object[] { "123456", 6};
        var faker = new Faker();

        for (int i = 0; i < (numberoftests - 1); i++)
        {
            var example = faker.Commerce.ProductDescription();
            var maxLength = example.Length + (new Random()).Next(0, 5);
            yield return new object[] { example, maxLength };
        }
    }

    //tam maximo



}
