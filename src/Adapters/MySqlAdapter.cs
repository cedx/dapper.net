namespace Belin.Dapper;

/// <summary>
/// The MySQL database adapter.
/// </summary>
internal class MySqlAdapter: ISqlAdapter {

	/// <summary>
	/// The SQL query used to fetch the identifier of the last inserted row.
	/// </summary>
	public string LastInsertIdQuery => "SELECT LAST_INSERT_ID()";

	/// <summary>
	/// Escapes the specified identifier.
	/// </summary>
	/// <param name="identifier">The identifier to escape.</param>
	/// <returns>The escaped identifier.</returns>
	public string EscapeIdentifier(string identifier) => $"`{identifier}`";
}
