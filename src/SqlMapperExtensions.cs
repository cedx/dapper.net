namespace Belin.Dapper;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Reflection;

/// <summary>
/// Provides extension methods for database connections.
/// </summary>
public static partial class SqlMapperExtensions {

	/// <summary>
	/// The query pattern used to count the entities.
	/// </summary>
	private const string CountQuery = "SELECT COUNT(*) FROM {0}";

	/// <summary>
	/// The query pattern used to delete an entity.
	/// </summary>
	private const string DeleteQuery = "DELETE FROM {0} WHERE {1} = @id";

	/// <summary>
	/// The query pattern used to delete all entities.
	/// </summary>
	private const string DeleteAllQuery = "DELETE FROM {0}";

	/// <summary>
	/// The query pattern used to fetch an entity.
	/// </summary>
	private const string FetchQuery = "SELECT {0} FROM {1} WHERE {2} = @id";

	/// <summary>
	/// The query pattern used to fetch all entities.
	/// </summary>
	private const string FetchAllQuery = "SELECT {0} FROM {1}";

	/// <summary>
	/// The query pattern used to insert an entity.
	/// </summary>
	private const string InsertQuery = "INSERT INTO {0} ({1}) VALUES ({2})";

	/// <summary>
	/// The query pattern used to truncate a table.
	/// </summary>
	private const string TruncateQuery = "TRUNCATE TABLE {0}";

	/// <summary>
	/// The query pattern used to update an entity.
	/// </summary>
	private const string UpdateQuery = "UPDATE {0} SET {1} WHERE {2} = @id";

	/// <summary>
	/// Resolves the column name corresponding to a given property of the specified entity type.
	/// </summary>
	/// <typeparam name="T">The entity type.</typeparam>
	/// <param name="property">The property name.</param>
	/// <returns>The resolved column name.</returns>
	/// <exception cref="DataException">TODO</exception>
	private static string GetColumnName<T>(string property) where T: class {
		var bindingFlags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;
		var member = typeof(T).GetProperty(property, bindingFlags) ?? throw new DataException("Unable to find the specified property.");
		return member.GetCustomAttribute<ColumnAttribute>()?.Name ?? member.Name;
	}

	/// <summary>
	/// TODO Resolves the column name corresponding to the single key of the specified entity type.
	/// </summary>
	/// <typeparam name="T">The entity type.</typeparam>
	/// <returns>The resolved key name.</returns>
	/// <exception cref="DataException">TODO</exception>
	private static PropertyInfo GetSingleKey<T>() where T: class {
		var bindingFlags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;
		var type = typeof(T);
		var member = type.GetProperties(bindingFlags).FirstOrDefault(property => property.IsDefined(typeof(KeyAttribute)));
		return member ?? type.GetProperty("Id", bindingFlags) ?? throw new DataException("Unable to find the single key.");
	}

	/// <summary>
	/// Gets the name of the table associated with an entity.
	/// </summary>
	/// <typeparam name="T">The entity type.</typeparam>
	/// <returns>The name of the table associated with an entity.</returns>
	private static string GetTableName<T>() where T: class {
		var type = typeof(T);
		return type.GetCustomAttribute<TableAttribute>()?.Name ?? type.Name;
	}
}
