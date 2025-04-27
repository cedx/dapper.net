namespace Belin.Dapper;

using System.Data;
using System.Text;

/// <summary>
/// Provides synchronous extension methods for database connections.
/// </summary>
public static partial class SqlMapperExtensions {

	/// <summary>
	/// Counts the total number of entities.
	/// </summary>
	/// <typeparam name="T">The entity type.</typeparam>
	/// <param name="connection">The database connection.</param>
	/// <returns>The total number of entities.</returns>
	public static int Count<T>(this IDbConnection connection) where T: class =>
		connection.ExecuteScalar<int>(string.Format(CountQuery, GetTableName<T>()));

	/// <summary>
	/// Deletes the entity with the specified identifier.
	/// </summary>
	/// <typeparam name="T">The entity type.</typeparam>
	/// <param name="connection">The database connection.</param>
	/// <param name="id">The entity identifier.</param>
	/// <returns><see langword="true"/> if the entity has been deleted, otherwise <see langword="false"/>.</returns>
	public static bool Delete<T>(this IDbConnection connection, object id) where T: class {
		var singleKey = GetSingleKey<T>();
		var sql = string.Format(DeleteQuery, GetTableName<T>(), singleKey.GetColumnName());
		return connection.Execute(sql, new { id }) > 0;
	}

	/// <summary>
	/// Deletes all entities of the specified type.
	/// </summary>
	/// <typeparam name="T">The entity type.</typeparam>
	/// <param name="connection">The database connection.</param>
	/// <returns>The number of affected rows.</returns>
	public static int DeleteAll<T>(this IDbConnection connection) where T: class =>
		connection.Execute(string.Format(DeleteAllQuery, GetTableName<T>()));

	/// <summary>
	/// Fetches the entity with the specified identifier.
	/// </summary>
	/// <typeparam name="T">The entity type.</typeparam>
	/// <param name="connection">The database connection.</param>
	/// <param name="id">The entity identifier.</param>
	/// <param name="columns">The names of the columns to fetch.</param>
	/// <returns>The entity with the specified identifier, or <see langword="null"/> if not found.</returns>
	public static T? Fetch<T>(this IDbConnection connection, object id, params string[] columns) where T: class {
		var fields = columns.Length > 0 ? string.Join(", ", columns) : "*";
		var singleKey = GetSingleKey<T>();
		var sql = string.Format(FetchQuery, fields, GetTableName<T>(), singleKey.GetColumnName());
		return connection.QuerySingleOrDefault<T>(sql, new { id });
	}

	/// <summary>
	/// Fetches all entities of the specified type.
	/// </summary>
	/// <typeparam name="T">The entity type.</typeparam>
	/// <param name="connection">The database connection.</param>
	/// <param name="columns">The names of the columns to fetch.</param>
	/// <returns>The entity list.</returns>
	public static IEnumerable<T> FetchAll<T>(this IDbConnection connection, params string[] columns) where T: class {
		var fields = columns.Length > 0 ? string.Join(", ", columns) : "*";
		return connection.Query<T>(string.Format(FetchAllQuery, fields, GetTableName<T>()));
	}

	/// <summary>
	/// Inserts the specified entity.
	/// </summary>
	/// <typeparam name="T">The entity type.</typeparam>
	/// <param name="connection">The database connection.</param>
	/// <param name="entity">The entity to insert.</param>
	public static void Insert<T>(this IDbConnection connection, T entity) where T: class {
		var singleKey = GetSingleKey<T>();
		var mappedProperties = GetMappedProperties<T>().Where(property => property.Name != singleKey.Name);

		var fields = mappedProperties.Select(property => property.GetColumnName());
		var parameters = new DynamicParameters(mappedProperties.ToDictionary(property => $"@{property.Name}", property => property.GetValue(entity)));
		var sql = string.Format(InsertQuery, GetTableName<T>(), string.Join(", ", fields), string.Join(", ", parameters.ParameterNames));
		connection.Execute(sql, parameters);
	}

	/// <summary>
	/// Truncates the table associated with the specified entity type.
	/// </summary>
	/// <param name="connection">The database connection.</param>
	/// <typeparam name="T">The entity type.</typeparam>
	public static void Truncate<T>(this IDbConnection connection) where T: class =>
		connection.Execute(string.Format(TruncateQuery, GetTableName<T>()));
}
