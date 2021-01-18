using System;
using System.Linq;
using App.DataContracts;
using CSharpFunctionalExtensions;
using Domain;
using Microsoft.AspNetCore.Mvc;

namespace App
{
    [Route("api/customers")]
    public class CustomerController : BaseController
    {
        private readonly MovieRepository _movieRepository;
        private readonly CustomerRepository _customerRepository;

        public CustomerController(MovieRepository movieRepository, CustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
            _movieRepository = movieRepository;
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult Get(long id)
        {
            Customer customer = _customerRepository.GetById(id);
            if (customer == null)
                return Error("Invalid customer id: " + id);

            var dto = new GetCustomerResponse
            {
                Id = customer.Id,
                Name = customer.Name.Value,
                Email = customer.Email.Value,
                MoneySpent = customer.MoneySpent.Value,
                Status = customer.Status.Type.ToString(),
                StatusExpirationDate = customer.Status.ExpirationDate.Date,
                PurchasedMovies = customer.PurchasedMovies.Select(x => new PurchasedMovieDto
                {
                    Price = x.Price.Value,
                    ExpirationDate = x.ExpirationDate.Date,
                    PurchaseDate = x.PurchaseDate,
                    Movie = new MovieDto
                    {
                        Id = x.MovieId,
                        Name = _movieRepository.GetById(x.MovieId).Name
                    }
                }).ToList()
            };

            return Ok(dto);
        }

        [HttpPost]
        public IActionResult Create([FromBody] CreateCustomerRequest request)
        {
            Result<CustomerName> customerName = CustomerName.Create(request.Name);
            Result<Email> email = Email.Create(request.Email);

            Result result = Result.Combine(customerName, email);
            if (result.IsFailure)
                return Error(result.Error);

            if (_customerRepository.GetByEmail(email.Value) != null)
                return Error("Email is already in use: " + request.Email);

            var customer = new Customer(customerName.Value, email.Value);
            _customerRepository.Save(customer);

            return Ok();
        }

        [HttpPut]
        [Route("{id}")]
        public IActionResult Update(long id, [FromBody] UpdateCustomerRequest request)
        {
            Customer customer = _customerRepository.GetById(id);
            if (customer == null)
                return Error("Invalid customer id: " + id);

            Result<CustomerName> customerName = CustomerName.Create(request.Name);
            if (customerName.IsFailure)
                return Error(customerName.Error);

            customer.Name = customerName.Value;
            _customerRepository.Save(customer);

            return Ok();
        }

        [HttpPost]
        [Route("{id}/movies")]
        public IActionResult PurchaseMovie(long id, [FromBody] long movieId)
        {
            Movie movie = _movieRepository.GetById(movieId);
            if (movie == null)
                return Error("Invalid movie id: " + movieId);

            Customer customer = _customerRepository.GetById(id);
            if (customer == null)
                return Error("Invalid customer id: " + id);

            if (customer.PurchasedMovies.Any(x => x.MovieId == movie.Id && x.ExpirationDate.IsExpired(DateTime.UtcNow) == false))
                return Error("The movie is already purchased: " + movie.Name);

            customer.PurchaseMovie(movie, DateTime.UtcNow);

            _customerRepository.Save(customer);

            return Ok();
        }

        [HttpPost]
        [Route("{id}/promotion")]
        public IActionResult PromoteCustomer(long id)
        {
            Customer customer = _customerRepository.GetById(id);
            if (customer == null)
                return Error("Invalid customer id: " + id);

            if (customer.Status.IsAdvanced(DateTime.UtcNow))
                return Error("The customer already has the Advanced status");

            bool success = customer.Promote(DateTime.UtcNow);
            if (!success)
                return Error("Cannot promote the customer");

            _customerRepository.Save(customer);

            return Ok();
        }
    }
}
