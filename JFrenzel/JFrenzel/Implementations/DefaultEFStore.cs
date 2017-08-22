using DataModels;
using JFrenzel.Interfaces;
using JFrenzel.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace JFrenzel.Implementations
{
	public class DefaultEFStore<T> : IEFStore<T> where T : EFModel
	{
		readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

		private ApplicationDbContext dbContext;
		private DbSet<T> dbSet;

		public DefaultEFStore(ApplicationDbContext dbContext)
		{
			this.dbContext = dbContext;

			//Verify the context has a dbSet matching the given type
			this.dbSet = null;
			foreach (var prop in dbContext.GetType().GetProperties())
			{
				var type = prop.GetValue(dbContext).GetType();

				//dbSet of necessary type was found
				if (type == typeof(DbSet<T>))
				{
					dbSet = (DbSet<T>)prop.GetValue(dbContext);
					break;
				}
			}

			if (dbSet == null)
			{
				string errorMsg = "EFStore: Error occured when trying to create store implementation for enity object of type "
					+ typeof(T).ToString()
					+ ". Required DbSet was not included in application context.";

				logger.Error(errorMsg);
				throw new DataAccessLayerException(errorMsg);
			}


		}

		/// <summary>
		/// Searches the db for a T that has the given id
		/// </summary>
		/// <param name="Id">ID of the desired T, integer greater than or equal to 0</param>
		/// <returns>A T object whose id matches the given id, null if not found or error occured</returns>
		public T FindById(int Id)
		{
			T result = null;

			//Verify that the input is valid
			if (Id >= 0)
			{
				try
				{
					result = dbSet.Where(x => x.Id == Id).First();
				}
				catch (Exception e)
				{
					logger.Error("EFStore: Error while finding T with Id " + Id + "; Exception Message: " + e.Message);
				}
			}

			return result;
		}

		/// <summary>
		/// Retrieves all the T in the db
		/// </summary>
		/// <returns>An enumberable that contains all T in the db</returns>
		public IEnumerable<T> GetList()
		{
			try
			{
				return dbSet;
			}
			catch (Exception e)
			{
				logger.Error("EFStore: Error while returning list of T; Exception Message: " + e.Message);
				return null;
			}
		}

		/// <summary>
		/// Creates an instance of T in the db that matches the given object
		/// </summary>
		/// <param name="obj">A T object to be persisted to the db</param>
		/// <returns>A copy of the given object w/ a new id corresponding to the created db row. Null on failure</returns>
		public T Create(T obj)
		{
			//Try adding the given new entry
			try
			{
				T addedImage = dbSet.Add(obj);
				dbContext.SaveChanges();

				return addedImage;
			}
			catch (Exception e)
			{
				logger.Error("EFStore: Error while creating T " + obj.ToString() + "; Exception Message: " + e.Message);
				return null;
			}
		}

		/// <summary>
		/// Updates the db entry corresponding to the given T object
		/// </summary>
		/// <param name="obj">The T object that needs to be updated</param>
		/// <returns>A boolean that indicates the success of the db update</returns>
		public bool Update(T obj)
		{
			//Notify EF that changes have been made
			try
			{
				dbContext.Entry(obj).State = EntityState.Modified;
				dbContext.SaveChanges();

				return true;
			}
			catch (Exception e)
			{
				logger.Error("EFStore: Error while creating T " + obj.ToString() + "; Exception Message: " + e.Message);
				return false;
			}
		}

		/// <summary>
		/// Deletes the T that has the given Id from the db
		/// </summary>
		/// <param name="Id">The id of the row to delete from the db</param>
		/// <returns>A bool indicating the success of the delete</returns>
		public bool Delete(int Id)
		{
			//Try removing the image with the given Id
			try
			{
				T target = dbSet.Where(x => x.Id == Id).First();
				return Delete(target);
			}
			catch
			{
				return false;
			}
		}

		/// <summary>
		/// Deletes the T that has the given Id from the db
		/// </summary>
		/// <param name="obj">The T object to be deleted from the db</param>
		/// <returns>A bool indicating the success of the delete</returns>
		public bool Delete(T obj)
		{
			//Try removing the image with the given Id
			try
			{
				dbSet.Remove(obj);
				dbContext.SaveChanges();

				return true;
			}
			catch
			{
				return false;
			}
		}
	}
}