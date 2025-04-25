namespace Belin.Dapper;

using System.Data;

/// <summary>
/// Provides asynchronous extension methods for database connections.
/// </summary>
public static partial class SqlMapperExtensions {

	/// <summary>
	/// Counts the total number of entities.
	/// </summary>
	/// <typeparam name="T">The entity type.</typeparam>
	/// <returns>The total number of entities.</returns>
	public static async Task<int> CountAsync<T>(this IDbConnection connection) =>
		await connection.ExecuteScalarAsync<int>(string.Format(CountQuery, GetTableName<T>()));

	/// <summary>
	/// Deletes all entities of the specified type.
	/// </summary>
	/// <typeparam name="T">The entity type.</typeparam>
	/// <returns>The number of affected rows.</returns>
	public static async Task<int> DeleteAllAsync<T>(this IDbConnection connection) =>
		await connection.ExecuteAsync(string.Format(DeleteAllQuery, GetTableName<T>()));

	/// <summary>
	/// Truncates the table associated with the specified entity type.
	/// </summary>
	/// <typeparam name="T">The entity type.</typeparam>
	public static async Task TruncateAsync<T>(this IDbConnection connection) =>
		await connection.ExecuteAsync(string.Format(TruncateQuery, GetTableName<T>()));
}
