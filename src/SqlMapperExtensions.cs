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
	private const string DeleteQuery = "DELETE FROM {0} WHERE {1} = @PrimaryKey";

	/// <summary>
	/// The SQL query used to delete all entities.
	/// </summary>
	private const string DeleteAllQuery = "DELETE FROM {0}";

	/// <summary>
	/// The SQL query used to truncate a table.
	/// </summary>
	private const string TruncateQuery = "TRUNCATE TABLE {0}";

	/// <summary>
	/// Counts the total number of entities.
	/// </summary>
	/// <typeparam name="T">The entity type.</typeparam>
	/// <returns>The total number of entities.</returns>
	public static int Count<T>(this IDbConnection connection) =>
		connection.ExecuteScalar<int>(string.Format(CountQuery, GetTableName<T>()));

	/// <summary>
	/// Deletes all entities of the specified type.
	/// </summary>
	/// <typeparam name="T">The entity type.</typeparam>
	/// <returns>The number of affected rows.</returns>
	public static int DeleteAll<T>(this IDbConnection connection) =>
		connection.Execute(string.Format(DeleteAllQuery, GetTableName<T>()));

	/// <summary>
	/// Truncates the table associated with the specified entity type.
	/// </summary>
	/// <typeparam name="T">The entity type.</typeparam>
	public static void Truncate<T>(this IDbConnection connection) =>
		connection.Execute(string.Format(TruncateQuery, GetTableName<T>()));

	private static string GetColumnName<T>(string column) {
		var bindingFlags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;
		var property = typeof(T).GetProperties(bindingFlags).FirstOrDefault(member => member.GetCustomAttribute<ColumnAttribute>()?.Name == column);
		return property is PropertyInfo info ? info.Name : column;
	}

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
