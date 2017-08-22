using DataModels;
using System.Collections.Generic;

namespace JFrenzel.Interfaces
{
	public interface IEFStore<T> where T : EFModel
	{
		/// <summary>
		/// Searches the db for a T that has the given id
		/// </summary>
		/// <param name="Id">ID of the desired T</param>
		/// <returns>A T object whose id matches the given id, null if not found</returns>
		T FindById(int Id);

		/// <summary>
		/// Retrieves all the T in the db
		/// </summary>
		/// <returns>An enumberable that contains all T in the db</returns>
		IEnumerable<T> GetList();

		/// <summary>
		/// Creates an instance of T in the db that matches the given object
		/// </summary>
		/// <param name="obj">A T object to be persisted to the db</param>
		/// <returns>A copy of the given object w/ a new id corresponding to the created db row. Null on failure</returns>
		T Create(T obj);

		/// <summary>
		/// Updates the db entry corresponding to the given T object
		/// </summary>
		/// <param name="obj">The T object that needs to be updated</param>
		/// <returns>A boolean that indicates the success of the db update</returns>
		bool Update(T obj);

		/// <summary>
		/// Deletes the T that has the given Id from the db
		/// </summary>
		/// <param name="Id">The id of the row to delete from the db</param>
		/// <returns>A bool indicating the success of the delete</returns>
		bool Delete(int Id);


		/// <summary>
		/// Deletes T from the db
		/// </summary>
		/// <param name="obj">The id of the row to delete from the db</param>
		/// <returns>A bool indicating the success of the delete</returns>
		bool Delete(T obj);
	}
}
