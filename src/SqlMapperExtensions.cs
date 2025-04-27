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
	/// TODO
	/// </summary>
	/// <typeparam name="T">The entity type.</typeparam>
	/// <returns>TODO</returns>
	internal static string GetCountQuery<T>() where T: class =>
		string.Format("SELECT COUNT(*) FROM {0}", GetTableName<T>());

	/// <summary>
	/// TODO
	/// </summary>
	/// <typeparam name="T">The entity type.</typeparam>
	/// <returns>TODO</returns>
	internal static string GetDeleteQuery<T>() where T: class {
		var singleKey = GetSingleKey<T>();
		return string.Format("DELETE FROM {0} WHERE {1} = @id", GetTableName<T>(), singleKey.GetColumnName());
	}

	/// <summary>
	/// TODO
	/// </summary>
	/// <typeparam name="T">The entity type.</typeparam>
	/// <returns>TODO</returns>
	internal static string GetDeleteAllQuery<T>() where T: class =>
		string.Format("DELETE FROM {0}", GetTableName<T>());

	/// <summary>
	/// TODO
	/// </summary>
	/// <typeparam name="T">The entity type.</typeparam>
	/// <param name="columns">The names of the columns to fetch.</param>
	/// <returns>TODO</returns>
	internal static string GetFetchQuery<T>(params string[] columns) where T: class {
		var fields = columns.Length > 0 ? string.Join(", ", columns) : "*";
		var singleKey = GetSingleKey<T>();
		return string.Format("SELECT {0} FROM {1} WHERE {2} = @id", fields, GetTableName<T>(), singleKey.GetColumnName());
	}

	/// <summary>
	/// TODO
	/// </summary>
	/// <typeparam name="T">The entity type.</typeparam>
	/// <param name="columns">The names of the columns to fetch.</param>
	/// <returns>TODO</returns>
	internal static string GetFetchAllQuery<T>(params string[] columns) where T: class {
		var fields = columns.Length > 0 ? string.Join(", ", columns) : "*";
		return string.Format("SELECT {0} FROM {1}", fields, GetTableName<T>());
	}

	/// <summary>
	/// TODO
	/// </summary>
	/// <typeparam name="T">The entity type.</typeparam>
	/// <param name="entity">The entity to insert.</param>
	/// <returns>TODO</returns>
	internal static (string Sql, DynamicParameters Parameters) GetInsertQuery<T>(T entity) where T: class {
		var singleKey = GetSingleKey<T>();
		var mappedProperties = GetMappedProperties<T>().Where(property => property.Name != singleKey.Name);
		var parameters = new DynamicParameters(mappedProperties.ToDictionary(property => property.Name, property => property.GetValue(entity)));

		var fields = mappedProperties.Select(property => property.GetColumnName());
		var values = parameters.ParameterNames.Select(parameter => $"@{parameter}");
		var sql = string.Format("INSERT INTO {0} ({1}) VALUES ({2})", GetTableName<T>(), string.Join(", ", fields), string.Join(", ", values));
		return (sql, parameters);
	}

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

	/// <summary>
	/// TODO
	/// </summary>
	/// <typeparam name="T">The entity type.</typeparam>
	/// <returns>TODO</returns>
	internal static string GetTruncateQuery<T>() where T: class =>
		string.Format("TRUNCATE TABLE {0}", GetTableName<T>());
}
