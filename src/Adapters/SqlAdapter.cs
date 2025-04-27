namespace Belin.Dapper;

/// <summary>
/// The SQL Server database adapter.
/// </summary>
internal class SqlAdapter: ISqlAdapter {

	/// <summary>
	/// The SQL query used to fetch the identifier of the last inserted row.
	/// </summary>
	public string LastInsertIdQuery => "SELECT SCOPE_IDENTITY()";

	/// <summary>
	/// Escapes the specified identifier.
	/// </summary>
	/// <param name="identifier">The identifier to escape.</param>
	/// <returns>The escaped identifier.</returns>
	public string EscapeIdentifier(string identifier) => $"[{identifier}]";
}
