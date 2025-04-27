namespace Belin.Dapper;

/// <summary>
/// TODO
/// </summary>
internal class SqlServerAdapter {

	/// <summary>
	/// Escapes the specified identifier.
	/// </summary>
	/// <param name="identifier">The identifier to escape.</param>
	/// <returns>The escaped identifier.</returns>
	public string EscapeIdentifier(string identifier) => $"[{identifier}]";
}
