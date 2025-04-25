namespace Belin.Dapper;

using System.Data;

/// <summary>
/// Provides synchronous extension methods for database connections.
/// </summary>
public static partial class SqlMapperExtensions {

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
}
