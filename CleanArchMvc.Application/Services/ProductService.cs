using AutoMapper;
using CleanArchMvc.Application.DTOs;
using CleanArchMvc.Application.Interfaces;
using CleanArchMvc.Application.Mappings;
using CleanArchMvc.Application.Products.Commands;
using CleanArchMvc.Application.Products.Queries;
using CleanArchMvc.Domain.Entities;
using CleanArchMvc.Domain.Interfaces;
using MediatR;

namespace CleanArchMvc.Application.Services
{
    public class ProductService : IProductService
    {
        //private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private IProductRepository _productRepository;

        public ProductService(/*IMediator mediator*/IMapper mapper, IProductRepository productRepository)
        {
            //_mediator = mediator;
            _mapper = mapper;
            _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
        }

        public async Task<ProductDTO> GetById(int? id)
        {
            // var productByIdQuery = new GetProductByIdQuery(id.Value);

            // if (productByIdQuery is null)
            //     throw new Exception($"Entity could not be loaded.");

            // var result = await _mediator.Send(productByIdQuery);

            // return _mapper.Map<ProductDTO>(result);
            var productsEntity = await _productRepository.GetByIdAsync(id);

            return _mapper.Map<ProductDTO>(productsEntity);
        }

        public async Task<IEnumerable<ProductDTO>> GetProducts()
        {
            // var productsQuery = new GetProductsQuery();

            // if (productsQuery is null)
            //     throw new Exception($"Entity could not be loaded.");

            // var result = await _mediator.Send(productsQuery);

            // return _mapper.Map<IEnumerable<ProductDTO>>(result);
            var productsEntity = await _productRepository.GetProductsAsync();

            return _mapper.Map<IEnumerable<ProductDTO>>(productsEntity);
        }

        public async Task Add(ProductDTO productDTO)
        {
            // var productCreateCommand = _mapper.Map<DTOToCommandMappingProfile>(productDTO);

            // await _mediator.Send(productCreateCommand);

            var productEntity = _mapper.Map<Product>(productDTO);

            await _productRepository.CreateAsync(productEntity);
        }

        public async Task Remove(int? id)
        {
            // var productRemoveCommand = new ProductRemoveCommand(id.Value);

            // if (productRemoveCommand is null)
            //     throw new Exception($"Entity could not be loaded.");

            // await _mediator.Send(productRemoveCommand);
            var productEntity = _productRepository.GetByIdAsync(id).Result;

            await _productRepository.RemoveAsync(productEntity);
        }

        public async Task Update(ProductDTO productDTO)
        {
            // var productUpdateCommand = _mapper.Map<DTOToCommandMappingProfile>(productDTO);

            // await _mediator.Send(productUpdateCommand);

            var productEntity = _mapper.Map<Product>(productDTO);

            await _productRepository.UpdateAsync(productEntity);
        }

        public async Task<ProductDTO> GetProductCategory(int? id)
        {
            var productsEntity = await _productRepository.GetProductCategoryAsync(id);

            return _mapper.Map<ProductDTO>(productsEntity);
        }
    }
}
