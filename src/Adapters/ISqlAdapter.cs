namespace Belin.Dapper;

/// <summary>
/// TODO
/// </summary>
internal interface ISqlAdapter {

	/// <summary>
	/// Escapes the specified identifier.
	/// </summary>
	/// <param name="identifier">The identifier to escape.</param>
	/// <returns>The escaped identifier.</returns>
	string EscapeIdentifier(string identifier);
}
