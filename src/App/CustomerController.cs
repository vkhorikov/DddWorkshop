using System;
using System.Linq;
using App.DataContracts;
using Domain;
using Microsoft.AspNetCore.Mvc;

namespace App
{
    [Route("api/customers")]
    public class CustomerController : Controller
    {
        private readonly MovieRepository _movieRepository;
        private readonly CustomerRepository _customerRepository;
        private readonly CustomerService _customerService;

        public CustomerController(
            MovieRepository movieRepository,
            CustomerRepository customerRepository,
            CustomerService customerService)
        {
            _customerRepository = customerRepository;
            _movieRepository = movieRepository;
            _customerService = customerService;
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult Get(long id)
        {
            Customer customer = _customerRepository.GetById(id);
            if (customer == null)
                return BadRequest("Invalid customer id: " + id);

            var dto = new GetCustomerResponse
            {
                Id = customer.Id,
                Name = customer.Name,
                Email = customer.Email,
                MoneySpent = customer.MoneySpent,
                Status = customer.Status.ToString(),
                StatusExpirationDate = customer.StatusExpirationDate,
                PurchasedMovies = customer.PurchasedMovies.Select(x => new PurchasedMovieDto
                {
                    Price = x.Price,
                    ExpirationDate = x.ExpirationDate,
                    PurchaseDate = x.PurchaseDate,
                    Movie = new MovieDto
                    {
                        Id = x.MovieId,
                        Name = _movieRepository.GetById(x.MovieId).Name
                    }
                }).ToList()
            };

            return Json(dto);
        }

        [HttpPost]
        public IActionResult Create([FromBody] CreateCustomerRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (_customerRepository.GetByEmail(request.Email) != null)
            {
                return BadRequest("Email is already in use: " + request.Email);
            }

            var customer = new Customer
            {
                Name = request.Name,
                Email = request.Email,
                MoneySpent = 0,
                Status = CustomerStatus.Regular,
                StatusExpirationDate = null
            };
            _customerRepository.Save(customer);

            return Ok();
        }

        [HttpPut]
        [Route("{id}")]
        public IActionResult Update(long id, [FromBody] UpdateCustomerRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Customer customer = _customerRepository.GetById(id);
            if (customer == null)
            {
                return BadRequest("Invalid customer id: " + id);
            }

            customer.Name = request.Name;
            _customerRepository.Save(customer);

            return Ok();
        }

        [HttpPost]
        [Route("{id}/movies")]
        public IActionResult PurchaseMovie(long id, [FromBody] long movieId)
        {
            Movie movie = _movieRepository.GetById(movieId);
            if (movie == null)
            {
                return BadRequest("Invalid movie id: " + movieId);
            }

            Customer customer = _customerRepository.GetById(id);
            if (customer == null)
            {
                return BadRequest("Invalid customer id: " + id);
            }

            if (customer.PurchasedMovies.Any(
                x => x.MovieId == movie.Id && (x.ExpirationDate == null || x.ExpirationDate.Value >= DateTime.UtcNow)))
            {
                return BadRequest("The movie is already purchased: " + movie.Name);
            }

            _customerService.PurchaseMovie(customer, movie);

            _customerRepository.Save(customer);

            return Ok();
        }

        [HttpPost]
        [Route("{id}/promotion")]
        public IActionResult PromoteCustomer(long id)
        {
            Customer customer = _customerRepository.GetById(id);
            if (customer == null)
            {
                return BadRequest("Invalid customer id: " + id);
            }

            if (customer.Status == CustomerStatus.Advanced &&
                (customer.StatusExpirationDate == null || customer.StatusExpirationDate.Value < DateTime.UtcNow))
            {
                return BadRequest("The customer already has the Advanced status");
            }

            bool success = _customerService.PromoteCustomer(customer);
            if (!success)
            {
                return BadRequest("Cannot promote the customer");
            }

            _customerRepository.Save(customer);

            return Ok();
        }
    }
}
