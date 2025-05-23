namespace Belin.Dapper;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Reflection;
using System.Text;

/// <summary>
/// Provides extension methods for database connections.
/// </summary>
public static partial class SqlMapperExtensions {

	/// <summary>
	/// Resolves the column name corresponding to the specified property.
	/// </summary>
	/// <param name="property">The property.</param>
	/// <returns>The resolved column name.</returns>
	internal static string GetColumnName(this PropertyInfo property) =>
		property.GetCustomAttribute<ColumnAttribute>()?.Name ?? property.Name;

	/// <summary>
	/// Gets the SQL query used to count the entities of the specified type.
	/// </summary>
	/// <typeparam name="T">The entity type.</typeparam>
	/// <returns>The SQL query used to count the entities of the specified type.</returns>
	internal static string GetCountQuery<T>() where T: class =>
		string.Format("SELECT COUNT(*) FROM {0}", GetTableName<T>());

	/// <summary>
	/// Gets the SQL query used to delete an entity of the specified type.
	/// </summary>
	/// <typeparam name="T">The entity type.</typeparam>
	/// <returns>The SQL query used to delete an entity of the specified type.</returns>
	/// <exception cref="DataException">The single key could not be determined.</exception>
	internal static string GetDeleteQuery<T>() where T: class {
		var singleKey = GetSingleKey<T>() ?? throw new DataException("Unable to find the single key.");
		return string.Format("DELETE FROM {0} WHERE {1} = @id", GetTableName<T>(), singleKey.GetColumnName());
	}

	/// <summary>
	/// Gets the SQL query used to delete all entities of the specified type.
	/// </summary>
	/// <typeparam name="T">The entity type.</typeparam>
	/// <returns>The SQL query used to delete all entities of the specified type.</returns>
	internal static string GetDeleteAllQuery<T>() where T: class =>
		string.Format("DELETE FROM {0}", GetTableName<T>());

	/// <summary>
	/// Gets the SQL query used to fetch an entity of the specified type.
	/// </summary>
	/// <typeparam name="T">The entity type.</typeparam>
	/// <param name="columns">The names of the columns to fetch.</param>
	/// <returns>The SQL query used to fetch an entity of the specified type.</returns>
	/// <exception cref="DataException">The single key could not be determined.</exception>
	internal static string GetFetchQuery<T>(params string[] columns) where T: class {
		var fields = columns.Length > 0 ? string.Join(", ", columns) : "*";
		var singleKey = GetSingleKey<T>() ?? throw new DataException("Unable to find the single key.");
		return string.Format("SELECT {0} FROM {1} WHERE {2} = @id", fields, GetTableName<T>(), singleKey.GetColumnName());
	}

	/// <summary>
	/// Gets the SQL query used to fetch all entities of the specified type.
	/// </summary>
	/// <typeparam name="T">The entity type.</typeparam>
	/// <param name="columns">The names of the columns to fetch.</param>
	/// <returns>The SQL query used to fetch all entities of the specified type.</returns>
	internal static string GetFetchAllQuery<T>(params string[] columns) where T: class {
		var fields = columns.Length > 0 ? string.Join(", ", columns) : "*";
		return string.Format("SELECT {0} FROM {1}", fields, GetTableName<T>());
	}

	/// <summary>
	/// Gets the SQL query used to insert an entity of the specified type.
	/// </summary>
	/// <typeparam name="T">The entity type.</typeparam>
	/// <returns>The SQL query used to insert an entity of the specified type.</returns>
	internal static string GetInsertQuery<T>() where T: class {
		var singleKey = GetSingleKey<T>();
		var mappedProperties = GetMappedProperties<T>();
		if (singleKey?.IsDatabaseGenerated() ?? false) mappedProperties = mappedProperties.Where(property => property.Name != singleKey.Name);

		var fields = mappedProperties.Select(property => property.GetColumnName());
		var values = mappedProperties.Select(property => $"@{property.Name}");
		return string.Format("INSERT INTO {0} ({1}) VALUES ({2})", GetTableName<T>(), string.Join(", ", fields), string.Join(", ", values));
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
	/// <returns>The resolved property, or <see langword="null"/> if not found.</returns>
	internal static PropertyInfo? GetSingleKey<T>() where T: class {
		var bindingFlags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;
		var type = typeof(T);
		return type.GetProperties(bindingFlags).SingleOrDefault(property => property.IsDefined(typeof(KeyAttribute))) ?? type.GetProperty("Id", bindingFlags);
	}

	/// <summary>
	/// Gets the SQL adapter corresponding to the specified database connection.
	/// </summary>
	/// <param name="connection">The database connection.</param>
	/// <returns>The SQL adapter corresponding to the specified database connection.</returns>
	internal static ISqlAdapter GetSqlAdapter(this IDbConnection connection) => connection.GetType().Name switch {
		"MySqlConnection" => new MySqlAdapter(),
		"SqliteConnection" => new SqliteAdapter(),
		_ => new SqlAdapter()
	};

	/// <summary>
	/// Resolves the name of the table corresponding to the specified entity type.
	/// </summary>
	/// <typeparam name="T">The entity type.</typeparam>
	/// <returns>The resolved table name.</returns>
	internal static string GetTableName<T>() where T: class {
		var type = typeof(T);
		return type.GetCustomAttribute<TableAttribute>()?.Name ?? type.Name;
	}

	/// <summary>
	/// Gets the SQL query used to truncate the table associated with the specified entity type.
	/// </summary>
	/// <typeparam name="T">The entity type.</typeparam>
	/// <returns>The SQL query used to truncate the table associated with the specified entity type.</returns>
	internal static string GetTruncateQuery<T>() where T: class =>
		string.Format("TRUNCATE TABLE {0}", GetTableName<T>());

	/// <summary>
	/// Gets the SQL query used to update an entity of the specified type.
	/// </summary>
	/// <typeparam name="T">The entity type.</typeparam>
	/// <param name="columns">The names of the columns to update.</param>
	/// <returns>The SQL query used to update an entity of the specified type.</returns>
	/// <exception cref="DataException">The single key could not be determined.</exception>
	internal static string GetUpdateQuery<T>(params string[] columns) where T: class {
		var singleKey = GetSingleKey<T>() ?? throw new DataException("Unable to find the single key.");
		var mappedProperties = GetMappedProperties<T>().Where(property => property.Name != singleKey.Name);

		var builder = new StringBuilder();
		var fields = columns.Length > 0 ? columns : [.. mappedProperties.Select(property => property.GetColumnName())];
		for (var i = 0; i < fields.Length; i++) {
			builder.AppendFormat("{0} = @{1}", fields[i], mappedProperties.Single(property => property.GetColumnName() == fields[i]).Name);
			if (i < fields.Length - 1) builder.Append(", ");
		}

		return string.Format("UPDATE {0} SET {1} WHERE {2} = @{3}", GetTableName<T>(), builder.ToString(), singleKey.GetColumnName(), singleKey.Name);
	}

	/// <summary>
	/// Gets a value indicating whether the specified property is generated by the database.
	/// </summary>
	/// <param name="property">The property.</param>
	/// <returns><see langword="true"/> if the specified property is generated by the database, otherwise <see langword="false"/>.</returns>
	internal static bool IsDatabaseGenerated(this PropertyInfo property) =>
		property.GetCustomAttribute<DatabaseGeneratedAttribute>() is DatabaseGeneratedAttribute attribute
			? attribute.DatabaseGeneratedOption != DatabaseGeneratedOption.None
			: property.IsDefined(typeof(KeyAttribute)) || property.Name == "Id";
}
