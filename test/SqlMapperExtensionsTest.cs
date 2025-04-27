namespace Belin.Dapper;

using static SqlMapperExtensions;

/// <summary>
/// Tests the features of the <see cref="SqlMapperExtensions"/> class.
/// </summary>
[TestClass]
public sealed class SqlMapperExtensionsTest {

	[TestMethod]
	public void GetColumnName() {
		AreEqual("Name", typeof(AttributedEntity).GetProperty("EntityName")!.GetColumnName());
		AreEqual("Name", typeof(PlainEntity).GetProperty("Name")!.GetColumnName());
	}

	[TestMethod]
	public void GetMappedProperties() {
		var properties = GetMappedProperties<AttributedEntity>().Select(property => property.Name);
		CollectionAssert.AreEquivalent(new[] { "EntityId", "EntityName" }, properties.ToArray());
		properties = GetMappedProperties<PlainEntity>().Select(property => property.Name);
		CollectionAssert.AreEquivalent(new[] { "Id", "Name", "IsMapped" }, properties.ToArray());
	}

	[TestMethod]
	public void GetSingleKey() {
		AreEqual("EntityId", GetSingleKey<AttributedEntity>().Name);
		AreEqual("Id", GetSingleKey<PlainEntity>().Name);
		// TODO test exception!!!!
	}

	[TestMethod]
	public void GetTableName() {
		AreEqual("Entities", GetTableName<AttributedEntity>());
		AreEqual("PlainEntity", GetTableName<PlainEntity>());
	}
}
