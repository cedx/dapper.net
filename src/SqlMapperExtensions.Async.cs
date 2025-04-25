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
	/// Deletes the entity with the specified identifier.
	/// </summary>
	/// <typeparam name="T">The entity type.</typeparam>
	/// <param name="connection">The database connection.</param>
	/// <param name="id">The entity identifier.</param>
	/// <returns><see langword="true"/> if the entity has been deleted, otherwise <see langword="false"/>.</returns>
	public static async Task<bool> DeleteAsync<T>(this IDbConnection connection, dynamic id) where T: class {
		var key = GetColumnName<T>(GetSingleKey<T>().Name);
		return await connection.ExecuteAsync(string.Format(DeleteQuery, GetTableName<T>(), key), new { id }) > 0;
	}

	/// <summary>
	/// Deletes all entities of the specified type.
	/// </summary>
	/// <typeparam name="T">The entity type.</typeparam>
	/// <param name="connection">The database connection.</param>
	/// <returns>The number of affected rows.</returns>
	public static async Task<int> DeleteAllAsync<T>(this IDbConnection connection) where T: class =>
		await connection.ExecuteAsync(string.Format(DeleteAllQuery, GetTableName<T>()));

	/// <summary>
	/// Fetches the entity with the specified identifier.
	/// </summary>
	/// <typeparam name="T">The entity type.</typeparam>
	/// <param name="connection">The database connection.</param>
	/// <param name="id">The entity identifier.</param>
	/// <param name="columns">The names of the columns to fetch.</param>
	/// <returns>The entity with the specified identifier, or <see langword="null"/> if not found.</returns>
	public static async Task<T?> FetchAsync<T>(this IDbConnection connection, dynamic id, params string[] columns) where T: class {
		var fields = columns.Length > 0 ? string.Join(", ", columns) : "*";
		var key = GetColumnName<T>(GetSingleKey<T>().Name);
		return await connection.QuerySingleOrDefaultAsync<T>(string.Format(FetchQuery, fields, GetTableName<T>(), key), new { id });
	}

	/// <summary>
	/// Fetches all entities of the specified type.
	/// </summary>
	/// <typeparam name="T">The entity type.</typeparam>
	/// <param name="connection">The database connection.</param>
	/// <param name="columns">The names of the columns to fetch.</param>
	/// <returns>The entity list.</returns>
	public static async Task<IEnumerable<T>> FetchAllAsync<T>(this IDbConnection connection, params string[] columns) where T: class {
		var fields = columns.Length > 0 ? string.Join(", ", columns) : "*";
		return await connection.QueryAsync<T>(string.Format(FetchAllQuery, fields, GetTableName<T>()));
	}

	/// <summary>
	/// Truncates the table associated with the specified entity type.
	/// </summary>
	/// <param name="connection">The database connection.</param>
	/// <typeparam name="T">The entity type.</typeparam>
	public static async Task TruncateAsync<T>(this IDbConnection connection) where T: class =>
		await connection.ExecuteAsync(string.Format(TruncateQuery, GetTableName<T>()));
}
