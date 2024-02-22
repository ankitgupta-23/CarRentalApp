using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalApp.DataLayer.IRepository
{
	public interface IGenericRepository<T> where T : class
	{
		IQueryable<T> GetAll();

		Task<T> GetByIdAsync(int id);

		Task<bool> AddAsync(T entity);

		Task<bool> UpdateAsync(T entity);

		Task<bool> DeleteAsync(T entity);
	}
}
