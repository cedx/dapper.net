namespace Belin.Dapper;

using System.Data;
using System.Net.Mail;

/// <summary>
/// Maps a mail address to or from a string.
/// </summary>
public class MailAddressTypeHandler: SqlMapper.TypeHandler<MailAddress> {

	/// <summary>
	/// Parses a database value back to a typed value.
	/// </summary>
	/// <param name="value">The value from the database.</param>
	/// <returns>The typed value.</returns>
	public override MailAddress? Parse(object value) =>
		value is string address && address.Length > 0 ? new MailAddress(address) : null;

	/// <summary>
	/// Assigns the value of a parameter before a command executes.
	/// </summary>
	/// <param name="parameter">The parameter to configure.</param>
	/// <param name="value">The parameter value.</param>
	public override void SetValue(IDbDataParameter parameter, MailAddress? value) =>
		parameter.Value = value?.Address;
}
