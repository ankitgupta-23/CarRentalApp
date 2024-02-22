using CarRentalApp.DataLayer.Entities;
using CarRentalApp.DataLayer.IRepository;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalApp.DataLayer.Repository
{
	public class CustomerRepository
	{
		private readonly IGenericRepository<Customer> _customerRepository;

		public CustomerRepository(IGenericRepository<Customer> customerRepository)
        {
			_customerRepository = customerRepository;

		}

		public IQueryable GetAll()
		{
			return _customerRepository.GetAll();
		}

		/*public bool Add(Customer customer)
		{
			return _customerRepository.
		}*/
    }
}
