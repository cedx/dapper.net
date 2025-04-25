namespace Belin.Dapper;

using System.Data;

/// <summary>
/// Maps a uniform resource identifier (URI) to or from a string.
/// </summary>
/// <param name="uriKind">Specifies whether the URI string is a relative URI, an absolute URI, or is indeterminate.</param>
public class UriTypeHandler(UriKind uriKind = UriKind.Absolute): SqlMapper.TypeHandler<Uri> {

	/// <summary>
	/// Assigns the value of a parameter before a command executes.
	/// </summary>
	/// <param name="parameter">The parameter to configure.</param>
	/// <param name="value">The parameter value.</param>
	public override void SetValue(IDbDataParameter parameter, Uri? value) =>
		parameter.Value = value?.ToString();

	/// <summary>
	/// Parses a database value back to a typed value.
	/// </summary>
	/// <param name="value">The value from the database.</param>
	/// <returns>The typed value.</returns>
	public override Uri? Parse(object value) =>
		value is string uriString && uriString.Length > 0 ? new Uri(uriString, uriKind) : null;
}
