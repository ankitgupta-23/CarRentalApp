using CarRentalApp.DataLayer.AppDbContext;
using CarRentalApp.DataLayer.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

namespace CarRentalApp.DataLayer.Repository
{
	public class GenericRepository<T> :IGenericRepository<T> where T:class
	{
		private readonly ApplicationDbContext _dbContext;

		public GenericRepository(ApplicationDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<bool> AddAsync(T entity)
		{
			_dbContext.Add<T>(entity);
			int res = await _dbContext.SaveChangesAsync();
			return res>0 ? true:false;
		}

		public async Task<bool> DeleteAsync(T entity)
		{
			_dbContext.Remove<T>(entity);
			return await _dbContext.SaveChangesAsync() > 0 ? true : false;
		}

		public IQueryable<T> GetAll()
		{
			return _dbContext.Set<T>().AsQueryable<T>();
		}

		public async Task<T> GetByIdAsync(int id)
		{

			return await _dbContext.FindAsync<T>(id) ?? throw new Exception($"No record found for id = {id}");
			
		}

		public async Task<bool> UpdateAsync(T entity)
		{
			_dbContext.Update<T>(entity);

			return await _dbContext.SaveChangesAsync() > 0 ? true : false;
		}
	}
}
