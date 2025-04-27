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
		await connection.ExecuteScalarAsync<int>(GetCountQuery<T>());

	/// <summary>
	/// Deletes the entity with the specified identifier.
	/// </summary>
	/// <typeparam name="T">The entity type.</typeparam>
	/// <param name="connection">The database connection.</param>
	/// <param name="id">The entity identifier.</param>
	/// <returns><see langword="true"/> if the entity has been deleted, otherwise <see langword="false"/>.</returns>
	public static async Task<bool> DeleteAsync<T>(this IDbConnection connection, object id) where T: class =>
		await connection.ExecuteAsync(GetDeleteQuery<T>(), new { id }) > 0;

	/// <summary>
	/// Deletes the specified entity.
	/// </summary>
	/// <typeparam name="T">The entity type.</typeparam>
	/// <param name="connection">The database connection.</param>
	/// <param name="entity">The entity to delete.</param>
	/// <returns><see langword="true"/> if the entity has been deleted, otherwise <see langword="false"/>.</returns>
	public static async Task<bool> DeleteAsync<T>(this IDbConnection connection, T entity) where T: class =>
		await DeleteAsync<T>(connection, GetSingleKey<T>().GetValue(entity)!);

	/// <summary>
	/// Deletes all entities of the specified type.
	/// </summary>
	/// <typeparam name="T">The entity type.</typeparam>
	/// <param name="connection">The database connection.</param>
	/// <returns>The number of affected rows.</returns>
	public static async Task<int> DeleteAllAsync<T>(this IDbConnection connection) where T: class =>
		await connection.ExecuteAsync(GetDeleteAllQuery<T>());

	/// <summary>
	/// Fetches the entity with the specified identifier.
	/// </summary>
	/// <typeparam name="T">The entity type.</typeparam>
	/// <param name="connection">The database connection.</param>
	/// <param name="id">The entity identifier.</param>
	/// <param name="columns">The names of the columns to fetch.</param>
	/// <returns>The entity with the specified identifier, or <see langword="null"/> if not found.</returns>
	public static async Task<T?> FetchAsync<T>(this IDbConnection connection, object id, params string[] columns) where T: class =>
		await connection.QuerySingleOrDefaultAsync<T>(GetFetchQuery<T>(columns), new { id });

	/// <summary>
	/// Fetches all entities of the specified type.
	/// </summary>
	/// <typeparam name="T">The entity type.</typeparam>
	/// <param name="connection">The database connection.</param>
	/// <param name="columns">The names of the columns to fetch.</param>
	/// <returns>The entity list.</returns>
	public static async Task<IEnumerable<T>> FetchAllAsync<T>(this IDbConnection connection, params string[] columns) where T: class =>
		await connection.QueryAsync<T>(GetFetchAllQuery<T>(columns));

	/// <summary>
	/// Inserts the specified entity.
	/// </summary>
	/// <typeparam name="T">The entity type.</typeparam>
	/// <param name="connection">The database connection.</param>
	/// <param name="entity">The entity to insert.</param>
	/// <returns>Completes when the specified entity has been inserted.</returns>
	public static async Task InsertAsync<T>(this IDbConnection connection, T entity) where T: class =>
		await connection.ExecuteAsync(GetInsertQuery<T>(), entity);

	/// <summary>
	/// Truncates the table associated with the specified entity type.
	/// </summary>
	/// <param name="connection">The database connection.</param>
	/// <typeparam name="T">The entity type.</typeparam>
	public static async Task TruncateAsync<T>(this IDbConnection connection) where T: class =>
		await connection.ExecuteAsync(GetTruncateQuery<T>());

	/// <summary>
	/// Updates the specified entity.
	/// </summary>
	/// <typeparam name="T">The entity type.</typeparam>
	/// <param name="connection">The database connection.</param>
	/// <param name="entity">The entity to update.</param>
	/// <param name="columns">The names of the columns to update.</param>
	public static async Task UpdateAsync<T>(this IDbConnection connection, T entity, params string[] columns) where T: class {
		var (sql, parameters) = GetUpdateQuery(entity, columns);
		await connection.ExecuteAsync(sql, parameters);
	}
}
