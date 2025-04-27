namespace Belin.Dapper;

using System.Data;

/// <summary>
/// Maps an enumerated value to or from a string.
/// </summary>
/// <remarks>
/// Does not work until issue #259 is resolved (<see href="https://github.com/DapperLib/Dapper/issues/259"/>).
/// </remarks>
/// <param name="ignoreCase">Value indicating whether to ignore case.</param>
public class StringEnumTypeHandler<T>(bool ignoreCase = false): SqlMapper.TypeHandler<T> where T: Enum {

	/// <summary>
	/// Parses a database value back to a typed value.
	/// </summary>
	/// <param name="value">The value from the database.</param>
	/// <returns>The typed value.</returns>
	public override T? Parse(object value) =>
		value is string enumValue && enumValue.Length > 0 ? (T) Enum.Parse(typeof(T), enumValue, ignoreCase) : default;

	/// <summary>
	/// Assigns the value of a parameter before a command executes.
	/// </summary>
	/// <param name="parameter">The parameter to configure.</param>
	/// <param name="value">The parameter value.</param>
	public override void SetValue(IDbDataParameter parameter, T? value) =>
		parameter.Value = value?.ToString();
}
