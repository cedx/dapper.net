namespace Belin.Dapper;

using System.Data;
using System.Net;

/// <summary>
/// Maps an Internet Protocol (IP) address to or from a string.
/// </summary>
/// <param name="mapToIPv6">Value indicating whether to map IPv4 addresses to IPv6.</param>
public class IPAddressTypeHandler(bool mapToIPv6 = false): SqlMapper.TypeHandler<IPAddress> {

	/// <summary>
	/// Parses a database value back to a typed value.
	/// </summary>
	/// <param name="value">The value from the database.</param>
	/// <returns>The typed value.</returns>
	public override IPAddress? Parse(object value) =>
		value is string ipString && ipString.Length > 0 ? IPAddress.Parse(ipString) : null;

	/// <summary>
	/// Assigns the value of a parameter before a command executes.
	/// </summary>
	/// <param name="parameter">The parameter to configure.</param>
	/// <param name="value">The parameter value.</param>
	public override void SetValue(IDbDataParameter parameter, IPAddress? value) =>
		parameter.Value = mapToIPv6 ? value?.MapToIPv6().ToString() : value?.ToString();
}
