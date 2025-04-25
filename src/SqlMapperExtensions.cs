namespace Belin.Dapper;

using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Reflection;

/// <summary>
/// Provides extension methods for database connections.
/// </summary>
public static partial class SqlMapperExtensions {

	/// <summary>
	/// The SQL query used to count the entities.
	/// </summary>
	private const string CountQuery = "SELECT COUNT(*) FROM {0}";

	/// <summary>
	/// The SQL query used to delete an entity.
	/// </summary>
	// private const string DeleteQuery = "DELETE FROM {0} WHERE {1} = @Key";

	/// <summary>
	/// The SQL query used to delete all entities.
	/// </summary>
	private const string DeleteAllQuery = "DELETE FROM {0}";

	/// <summary>
	/// The SQL query used to truncate a table.
	/// </summary>
	private const string TruncateQuery = "TRUNCATE TABLE {0}";

	/// <summary>
	/// Resolves the name of the property corresponding to the specified column name for a given entity type.
	/// </summary>
	/// <typeparam name="T">The entity type.</typeparam>
	/// <param name="columnName">The column name.</param>
	/// <returns>The resolved property name.</returns>
	// private static string GetColumnName<T>(string columnName) {
	// 	var properties = typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
	// 	return properties.FirstOrDefault(property => property.GetCustomAttribute<ColumnAttribute>()?.Name == columnName)?.Name ?? columnName;
	// }

	// private static string GetPrimaryKey<T>() {
	// 	var bindingFlags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;
	// 	return type.GetProperties(bindingFlags).FirstOrDefault(property => property.GetCustomAttribute<ColumnAttribute>()?.Name == column)!;
	// }

	/// <summary>
	/// Gets the name of the table associated with an entity.
	/// </summary>
	/// <typeparam name="T">The entity type.</typeparam>
	/// <returns>The name of the table associated with an entity.</returns>
	private static string GetTableName<T>() {
		var type = typeof(T);
		return type.GetCustomAttribute<TableAttribute>()?.Name ?? type.Name;
	}
}
