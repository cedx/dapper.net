namespace Belin.Dapper;

using System.Data;
using System.Threading.Tasks;

/// <summary>
/// Provides asynchronous extension methods for database connections.
/// </summary>
public static partial class SqlMapperExtensions {

	/// <summary>
	/// Counts the total number of entities.
	/// </summary>
	/// <typeparam name="T">The entity type.</typeparam>
	/// <param name="connection">The database connection.</param>
	/// <returns>The total number of entities.</returns>
	public static async Task<int> CountAsync<T>(this IDbConnection connection) where T: class =>
		await connection.ExecuteScalarAsync<int>(string.Format(CountQuery, GetTableName<T>()));

	/// <summary>
	/// Deletes all entities of the specified type.
	/// </summary>
	/// <typeparam name="T">The entity type.</typeparam>
	/// <param name="connection">The database connection.</param>
	/// <returns>The number of affected rows.</returns>
	public static async Task<int> DeleteAllAsync<T>(this IDbConnection connection) where T: class =>
		await connection.ExecuteAsync(string.Format(DeleteAllQuery, GetTableName<T>()));

	/// <summary>
	/// Fetches all entities of the specified type.
	/// </summary>
	/// <typeparam name="T">The entity type.</typeparam>
	/// <param name="connection">The database connection.</param>
	/// <param name="columns">The names of the columns to fetch.</param>
	/// <returns>The entity list.</returns>
	public static async Task<IEnumerable<T>> FetchAllAsync<T>(this IDbConnection connection, params string[] columns) where T: class =>
		await connection.QueryAsync<T>(string.Format(FetchAllQuery, columns.Length > 0 ? string.Join(", ", columns) : "*", GetTableName<T>()));

	/// <summary>
	/// Truncates the table associated with the specified entity type.
	/// </summary>
	/// <param name="connection">The database connection.</param>
	/// <typeparam name="T">The entity type.</typeparam>
	public static async Task TruncateAsync<T>(this IDbConnection connection) where T: class =>
		await connection.ExecuteAsync(string.Format(TruncateQuery, GetTableName<T>()));
}
