using HC.Codeflix.Catalog.Domain.Exceptions;
using HC.Codeflix.Catalog.Domain.SeedWork;
using HC.Codeflix.Catalog.Domain.Validation;
using System.ComponentModel.DataAnnotations;

namespace HC.Codeflix.Catalog.Domain.Entity;
public class Category : AggregateRoot
{
    public string Name { get; private set; }
    public string Description { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }


    public Category(string name, string description, bool isActive = true) : base()
    {
     
        Name = name;
        Description = description;
        IsActive = true;
        CreatedAt = DateTime.Now;
        IsActive = isActive;

        Validate();
    }

    public void Update(string name, string? description = null)
    {
        Name = name;
        Description = description ?? Description;
        Validate();
    }

    public void Deactivate()
    {
        IsActive = false;
        Validate();
    }

    public void Activate()
    {
        IsActive = true;
        Validate();
    }
    private void Validate()
    {

        DomainValidation.NotNullOrEmpty(Name, nameof(Name));
        DomainValidation.MinLength(Name, 3, nameof(Name));
        DomainValidation.MaxLength(Name, 255, nameof(Name));

        DomainValidation.NotNull(Description, nameof(Description));
        DomainValidation.MaxLength(Description, 10_000, nameof(Description));    

        //if (String.IsNullOrWhiteSpace(Name))
        //{
            //throw new EntityValidationException($"{nameof(Name)} should not be empty or null.");
        //}

        /*if (Name.Length < 3)
        {
            throw new EntityValidationException($"{nameof(Name)} should be at least 3 characters.");
        }
        if (Name.Length > 255)
        {
            throw new EntityValidationException($"{nameof(Name)} should be less or equal 255 characters long.");
        }*/

        /*if (Description == null)
        {
            throw new EntityValidationException($"{nameof(Description)} should not be null.");
        }

        if (Description.Length > 10000)
        {
            throw new EntityValidationException($"{nameof(Description)} should be less or equal 10_000 characters long.");
        }*/

    }

}
