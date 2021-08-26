using AutoMapper;
using DeliveryWebApp.Application.Addresses.Commands.CreateAddress;
using DeliveryWebApp.Application.Addresses.Commands.DeleteAddress;
using DeliveryWebApp.Application.Addresses.Commands.UpdateAddress;
using DeliveryWebApp.Application.BasketItems.Commands.CreateBasketItem;
using DeliveryWebApp.Application.BasketItems.Commands.DeleteBasketItem;
using DeliveryWebApp.Application.BasketItems.Commands.UpdateBasketItem;
using DeliveryWebApp.Application.Baskets.Commands.CreateBasket;
using DeliveryWebApp.Application.Baskets.Commands.DeleteBasket;
using DeliveryWebApp.Application.Baskets.Commands.PurgeBasket;
using DeliveryWebApp.Application.Baskets.Commands.UpdateBasket;
using DeliveryWebApp.Application.Customers.Commands.CreateCustomer;
using DeliveryWebApp.Application.Customers.Commands.DeleteCustomer;
using DeliveryWebApp.Application.Customers.Commands.UpdateCustomer;
using DeliveryWebApp.Application.OrderItems.Commands.CreateOrderItem;
using DeliveryWebApp.Application.OrderItems.Commands.DeleteOrderItem;
using DeliveryWebApp.Application.OrderItems.Commands.UpdateOrderItem;
using DeliveryWebApp.Application.Orders.Commands.CreateOrder;
using DeliveryWebApp.Application.Orders.Commands.DeleteOrder;
using DeliveryWebApp.Application.Orders.Commands.UpdateOrder;
using DeliveryWebApp.Application.Products.Commands.CreateProduct;
using DeliveryWebApp.Application.Products.Commands.DeleteProduct;
using DeliveryWebApp.Application.Products.Commands.UpdateProducts;
using DeliveryWebApp.Application.Requests.Commands.CreateRequest;
using DeliveryWebApp.Application.Requests.Commands.DeleteRequest;
using DeliveryWebApp.Application.Requests.Commands.UpdateRequest;
using DeliveryWebApp.Application.Restaurateurs.Commands.CreateRestaurateur;
using DeliveryWebApp.Application.Restaurateurs.Commands.DeleteRestaurateur;
using DeliveryWebApp.Application.Restaurateurs.Commands.UpdateRestaurateur;
using DeliveryWebApp.Application.Reviews.Commands.CreateReview;
using DeliveryWebApp.Application.Reviews.Commands.DeleteReview;
using DeliveryWebApp.Application.Riders.Commands.CreateRider;
using DeliveryWebApp.Application.Riders.Commands.DeleteRider;
using DeliveryWebApp.Application.Riders.Commands.UpdateRider;
using DeliveryWebApp.Domain.Entities;
using System;
using System.Linq;
using System.Reflection;

namespace DeliveryWebApp.Application.Common.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            ApplyMappingsFromAssembly(Assembly.GetExecutingAssembly());

            CreateMap<Address, CreateAddressCommand>().ReverseMap();
            CreateMap<Address, UpdateAddressCommand>().ReverseMap();
            CreateMap<Address, DeleteAddressCommand>().ReverseMap();

            CreateMap<Basket, CreateBasketCommand>().ReverseMap();
            CreateMap<Basket, PurgeBasketCommand>().ReverseMap();
            CreateMap<Basket, UpdateBasketCommand>().ReverseMap();
            CreateMap<Basket, DeleteBasketCommand>().ReverseMap();

            CreateMap<BasketItem, CreateBasketItemCommand>().ReverseMap();
            CreateMap<BasketItem, UpdateBasketItemCommand>().ReverseMap();
            CreateMap<BasketItem, DeleteBasketItemCommand>().ReverseMap();

            CreateMap<Customer, CreateCustomerCommand>().ReverseMap();
            CreateMap<Customer, DeleteCustomerCommand>().ReverseMap();
            CreateMap<Customer, UpdateCustomerCommand>().ReverseMap();

            CreateMap<Order, CreateOrderCommand>().ReverseMap();
            CreateMap<Order, DeleteOrderCommand>().ReverseMap();
            CreateMap<Order, UpdateOrderCommand>().ReverseMap();

            CreateMap<OrderItem, CreateOrderItemCommand>().ReverseMap();
            CreateMap<OrderItem, UpdateOrderItemCommand>().ReverseMap();
            CreateMap<OrderItem, DeleteOrderItemCommand>().ReverseMap();

            CreateMap<Product, CreateProductCommand>().ReverseMap();
            CreateMap<Product, DeleteProductCommand>().ReverseMap();
            CreateMap<Product, UpdateProductCommand>().ReverseMap();

            CreateMap<Request, CreateRequestCommand>().ReverseMap();
            CreateMap<Request, DeleteRequestCommand>().ReverseMap();
            CreateMap<Request, UpdateRequestCommand>().ReverseMap();

            CreateMap<Restaurateur, CreateRestaurateurCommand>().ReverseMap();
            CreateMap<Restaurateur, DeleteRestaurateurCommand>().ReverseMap();
            CreateMap<Restaurateur, UpdateRestaurateurCommand>().ReverseMap();

            CreateMap<Rider, CreateRiderCommand>().ReverseMap();
            CreateMap<Rider, DeleteRiderCommand>().ReverseMap();
            CreateMap<Rider, UpdateRiderCommand>().ReverseMap();

            CreateMap<Review, CreateReviewCommand>().ReverseMap();
            CreateMap<Review, DeleteReviewCommand>().ReverseMap();
        }

        private void ApplyMappingsFromAssembly(Assembly assembly)
        {
            var types = assembly.GetExportedTypes()
                .Where(t => t.GetInterfaces().Any(i =>
                    i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IMapFrom<>)))
                .ToList();

            foreach (var type in types)
            {
                var instance = Activator.CreateInstance(type);
                var methodInfo = type.GetMethod("Mapping");
                methodInfo?.Invoke(instance, new object[] { this });
            }
        }
    }
}