using CleanArchMvc.Domain.Entities;
using FluentAssertions;
using Xunit;

namespace CleanArchMvc.Domain.Tests
{
    public class CategoryUnitTest1
    {
        [Fact(DisplayName = "Create Category With Valid State")]
        public void CreateCategory_WithValidParameters_ResultObjectValidState()
        {
            Action action = () => new Category(1, "Category Name" );
            
            action.Should()
                .NotThrow<Validation.DomainExceptionValidation>();
        }

        [Fact(DisplayName = "Try To Create Category With Negative Id")]
        public void CreateCategory_NegativeIdValue_DomainExceptionInvalidId()
        {
            Action action = () => new Category(-1, "Category Name");

            action.Should()
                .Throw<Validation.DomainExceptionValidation>()
                .WithMessage("Invalid id value.");
        }

        [Fact(DisplayName = "Try To Create Category With Negative Id")]
        public void CreateCategory_ShortNameValue_DomainExceptionShortName()
        {
            Action action = () => new Category(1, "Ca");

            action.Should()
                .Throw<Validation.DomainExceptionValidation>()
                .WithMessage("Invalid name, too short, minimum 3 charecters.");
        }

        [Fact(DisplayName = "Try To Create Category With Empty Name")]
        public void CreateCategory_MissingNameValue_DomainExceptionRequiredName()
        {
            Action action = () => new Category(1, "");

            action.Should()
                .Throw<Validation.DomainExceptionValidation>()
                .WithMessage("Invalid name. Name is required.");
        }

        [Fact(DisplayName = "Try To Create Category Without Name")]
        public void CreateCategory_WithNullNameValue_DomainExceptionRequiredName()
        {
            Action action = () => new Category(1, null);

            action.Should()
                .Throw<Validation.DomainExceptionValidation>();
        }
    }
}