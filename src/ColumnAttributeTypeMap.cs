namespace Belin.Dapper;

using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

/// <summary>
/// Maps a column name to a property that may be annotated with a <see cref="ColumnAttribute"/> attribute.
/// </summary>
/// <typeparam name="T">The type of entity to which this type map applies.</typeparam>
public class ColumnAttributeTypeMap<T>(): SqlMapper.ITypeMap {

	/// <summary>
	/// The custom type map used to find the properties annotated with a <see cref="ColumnAttribute"/> attribute.
	/// </summary>
	private readonly CustomPropertyTypeMap customMapper = new(typeof(T), (type, columnName) => {
		var properties = type.GetProperties(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
		return properties.FirstOrDefault(property => property.GetCustomAttribute<ColumnAttribute>()?.Name == columnName)!;
	});

	/// <summary>
	/// The default type map used when the custom type map was unable to find a matching property.
	/// </summary>
	private readonly DefaultTypeMap defaultMapper = new(typeof(T));

	/// <summary>
	/// Finds the best constructor.
	/// </summary>
	/// <param name="names">The column names.</param>
	/// <param name="types">The column types.</param>
	/// <returns>The matching constructor or the default one.</returns>
	public ConstructorInfo? FindConstructor(string[] names, Type[] types) {
		try { return customMapper.FindConstructor(names, types); }
		catch { return defaultMapper.FindConstructor(names, types); }
	}

	/// <summary>
	/// Finds the constructor which should always be used.
	/// </summary>
	/// <returns>The constructor which should always be used.</returns>
	public ConstructorInfo? FindExplicitConstructor() {
		var constructor = customMapper.FindExplicitConstructor();
		return constructor ?? defaultMapper.FindExplicitConstructor();
	}

	/// <summary>
	/// Gets the mapping for a constructor parameter.
	/// </summary>
	/// <param name="constructor">The constructor to resolve.</param>
	/// <param name="columnName">The column name.</param>
	/// <returns>The mapping implementation.</returns>
	public SqlMapper.IMemberMap? GetConstructorParameter(ConstructorInfo constructor, string columnName) {
		try { return customMapper.GetConstructorParameter(constructor, columnName); }
		catch { return defaultMapper.GetConstructorParameter(constructor, columnName); }
	}

	/// <summary>
	/// Gets the member mapping for a column.
	/// </summary>
	/// <param name="columnName">The column name.</param>
	/// <returns>The mapping implementation.</returns>
	public SqlMapper.IMemberMap? GetMember(string columnName) {
		var member = customMapper.GetMember(columnName);
		return member ?? defaultMapper.GetMember(columnName);
	}
}
