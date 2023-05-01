using CleanArchMvc.Domain.Entities;
using FluentAssertions;
using Xunit;

namespace CleanArchMvc.Domain.Tests
{
    public class ProductUnitTest1
    {
        [Fact(DisplayName = "Create Product With Valid State")]
        public void CreateProduct_WithValidParameters_ResultObjectValidState()
        {
            Action action = () => new Product(1, "Product Name", "Product Description", 9.99m, 00, "Product Image");

            action.Should()
                .NotThrow<Validation.DomainExceptionValidation>();
        }

        [Fact(DisplayName = "Create Product With Valid State")]
        public void CreateProduct_NegativeIdValue_DomainExceptionInvalidId()
        {
            Action action = () => new Product(-1, "Product Name", "Product Description", 9.99m, 00, "Product Image");

            action.Should()
                .Throw<Validation.DomainExceptionValidation>()
                .WithMessage("Invalid id value.");
        }

        [Fact(DisplayName = "Try To Create Product With Negative Id")]
        public void CreateProduct_ShortNameValue_DomainExceptionShortName()
        {
            Action action = () => new Product(1, "Pr", "Product Description", 9.99m, 00, "Product Image");

            action.Should()
                .Throw<Validation.DomainExceptionValidation>()
                .WithMessage("Invalid name, too short, minimum 3 charecters.");
        }

        [Fact(DisplayName = "Try To Create Product With Empty Name")]
        public void CreateProduct_MissingNameValue_DomainExceptionRequiredName()
        {
            Action action = () => new Product(1, "", "Product Description", 9.99m, 00, "Product Image");

            action.Should()
                .Throw<Validation.DomainExceptionValidation>()
                .WithMessage("Invalid name. Name is required.");
        }

        [Fact(DisplayName = "Try To Create Product Without Name")]
        public void CreateProduct_WithNullNameValue_DomainExceptionRequiredName()
        {
            Action action = () => new Product(1, null, "Product Description", 9.99m, 00, "Product Image");

            action.Should()
                .Throw<Validation.DomainExceptionValidation>();
        }

        [Fact(DisplayName = "Try To Create Product With Image Name Too Long")]
        public void CreateProduct_WithImageNameValueTooLong_DomainExceptionImageNameTooLong()
        {
            Action action = () => new Product(1, "Product Name", "Product Description", 9.99m, 00,
                "Imageeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeee " +
                "nameeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeee " +
                "tooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooo " +
                "tooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooo " +
                "loooooooooooooooooooooooooooooooooooooooooooooooooooooooooooog");

            action.Should()
                .Throw<Validation.DomainExceptionValidation>()
                .WithMessage("Invalid image name, too long, maximum 250 charecters.");
        }

        [Fact(DisplayName = "Create Product With Image Name Null")]
        public void CreateProduct_WithNullImageNameValue_NoDomainException()
        {
            Action action = () => new Product(1, "Product Name", "Product Description", 9.99m, 00, null);

            action.Should()
                .NotThrow<Validation.DomainExceptionValidation>();
        }

        [Fact(DisplayName = "Create Product With Image Name Null")]
        public void CreateProduct_WithNullImageNameValue_NoNullReferenceException()
        {
            Action action = () => new Product(1, "Product Name", "Product Description", 9.99m, 00, null);

            action.Should()
                .NotThrow<NullReferenceException>();
        }

        [Fact(DisplayName = "Create Product With Image Name Empty")]
        public void CreateProduct_WithEmptyImageNameValue_NoDomainException()
        {
            Action action = () => new Product(1, "Product Name", "Product Description", 9.99m, 00, "");

            action.Should()
                .NotThrow<Validation.DomainExceptionValidation>();
        }

        [Theory(DisplayName = "Create Product With Invalid Stock Value")]
        [InlineData(-5)]
        public void CreateProduct_WithInvalidStockValue_DomainExceptionNegativeValue(int value)
        {
            Action action = () => new Product(1, "Product Name", "Product Description", 9.99m, value, "Prduct Image Name");

            action.Should()
                .Throw<Validation.DomainExceptionValidation>()
                .WithMessage("Invalid stock value.");
        }

        [Theory(DisplayName = "Create Product With Invalid Price Value")]
        [InlineData(-5)]
        public void CreateProduct_WithInvalidPriceValue_DomainExceptionNegativeValue(int value)
        {
            Action action = () => new Product(1, "Product Name", "Product Description", value, 00, "Prduct Image Name");

            action.Should()
                .Throw<Validation.DomainExceptionValidation>()
                .WithMessage("Invalid price value.");
        }
    }
}
