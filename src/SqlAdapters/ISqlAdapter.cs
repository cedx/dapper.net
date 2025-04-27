namespace Belin.Dapper;

/// <summary>
/// Represents a SQL database adapter.
/// </summary>
internal interface ISqlAdapter {

	/// <summary>
	/// The SQL query used to fetch the identifier of the last inserted row.
	/// </summary>
	string LastInsertIdQuery { get; }

	/// <summary>
	/// Escapes the specified identifier.
	/// </summary>
	/// <param name="identifier">The identifier to escape.</param>
	/// <returns>The escaped identifier.</returns>
	string EscapeIdentifier(string identifier);
}
