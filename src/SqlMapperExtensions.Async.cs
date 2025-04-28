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
	/// <exception cref="DataException">The single key could not be determined.</exception>
	public static async Task<bool> DeleteAsync<T>(this IDbConnection connection, T entity) where T: class {
		var singleKey = GetSingleKey<T>() ?? throw new DataException("Unable to find the single key.");
		return await DeleteAsync<T>(connection, singleKey.GetValue(entity)!);
	}

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
	/// <returns>The identifier of the inserted row.</returns>
	public static async Task<long> InsertAsync<T>(this IDbConnection connection, T entity) where T: class {
		var sql = $"{GetInsertQuery<T>()}; {connection.GetSqlAdapter().LastInsertIdQuery};";
		var first = await (await connection.QueryMultipleAsync(sql, entity)).ReadFirstOrDefaultAsync();
		if (first is null || first.Id is null) return 0;

		var singleKey = GetSingleKey<T>();
		singleKey?.SetValue(entity, Convert.ChangeType(first.Id, singleKey.PropertyType));
		return (long) first.Id;
	}

	/// <summary>
	/// Truncates the table associated with the specified entity type.
	/// </summary>
	/// <param name="connection">The database connection.</param>
	/// <typeparam name="T">The entity type.</typeparam>
	/// <returns>Completes when the table has been truncated.</returns>
	public static async Task TruncateAsync<T>(this IDbConnection connection) where T: class =>
		await connection.ExecuteAsync(GetTruncateQuery<T>());

	/// <summary>
	/// Updates the specified entity.
	/// </summary>
	/// <typeparam name="T">The entity type.</typeparam>
	/// <param name="connection">The database connection.</param>
	/// <param name="entity">The entity to update.</param>
	/// <param name="columns">The names of the columns to update.</param>
	/// <returns><see langword="true"/> if the entity has been deleted, otherwise <see langword="false"/>.</returns>
	public static async Task<bool> UpdateAsync<T>(this IDbConnection connection, T entity, params string[] columns) where T: class =>
		(await connection.ExecuteAsync(GetUpdateQuery<T>(columns), entity)) > 0;
}
