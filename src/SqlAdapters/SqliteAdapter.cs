namespace Belin.Dapper;

/// <summary>
/// The SQLite database adapter.
/// </summary>
internal class SqliteAdapter: ISqlAdapter {

	/// <summary>
	/// The SQL query used to fetch the identifier of the last inserted row.
	/// </summary>
	public string LastInsertIdQuery => $"SELECT last_insert_rowid() AS {EscapeIdentifier("Id")}";

	/// <summary>
	/// Escapes the specified identifier.
	/// </summary>
	/// <param name="identifier">The identifier to escape.</param>
	/// <returns>The escaped identifier.</returns>
	public string EscapeIdentifier(string identifier) => $"\"{identifier}\"";
}
