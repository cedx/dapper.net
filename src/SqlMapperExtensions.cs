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
	/// Resolves the column name corresponding to the specified property.
	/// </summary>
	/// <param name="property">The property.</param>
	/// <returns>The resolved column name.</returns>
	internal static string GetColumnName(this PropertyInfo property) =>
		property.GetCustomAttribute<ColumnAttribute>()?.Name ?? property.Name;

	/// <summary>
	/// Gets the properties of the specified entity type that are mapped to a database column.
	/// </summary>
	/// <typeparam name="T">The entity type.</typeparam>
	/// <returns>The mapped properties.</returns>
	internal static IEnumerable<PropertyInfo> GetMappedProperties<T>() where T: class {
		var bindingFlags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;
		return typeof(T).GetProperties(bindingFlags).Where(property => !property.IsDefined(typeof(NotMappedAttribute)));
	}

	/// <summary>
	/// Resolves the property corresponding to the single key of the specified entity type.
	/// </summary>
	/// <typeparam name="T">The entity type.</typeparam>
	/// <returns>The resolved property.</returns>
	/// <exception cref="DataException">The single key is not found.</exception>
	internal static PropertyInfo GetSingleKey<T>() where T: class {
		var bindingFlags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;
		var type = typeof(T);
		var member = type.GetProperties(bindingFlags).SingleOrDefault(property => property.IsDefined(typeof(KeyAttribute)));
		return member ?? type.GetProperty("Id", bindingFlags) ?? throw new DataException("Unable to find the single key.");
	}

	/// <summary>
	/// Resolves the table name of the table corresponding the specified entity type.
	/// </summary>
	/// <typeparam name="T">The entity type.</typeparam>
	/// <returns>The resolved table name.</returns>
	internal static string GetTableName<T>() where T: class {
		var type = typeof(T);
		return type.GetCustomAttribute<TableAttribute>()?.Name ?? type.Name;
	}
}
