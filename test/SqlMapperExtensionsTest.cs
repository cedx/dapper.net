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
	public void GetCountQuery() {
		AreEqual("SELECT COUNT(*) FROM Entities", GetCountQuery<AttributedEntity>());
		AreEqual("SELECT COUNT(*) FROM PlainEntity", GetCountQuery<PlainEntity>());
	}

	[TestMethod]
	public void GetDeleteQuery() {
		AreEqual("DELETE FROM Entities WHERE Id = @id", GetDeleteQuery<AttributedEntity>());
		AreEqual("DELETE FROM PlainEntity WHERE Id = @id", GetDeleteQuery<PlainEntity>());
	}

	[TestMethod]
	public void GetDeleteAllQuery() {
		AreEqual("DELETE FROM Entities", GetDeleteAllQuery<AttributedEntity>());
		AreEqual("DELETE FROM PlainEntity", GetDeleteAllQuery<PlainEntity>());
	}

	[TestMethod]
	public void GetFetchQuery() {
		AreEqual("SELECT * FROM Entities WHERE Id = @id", GetFetchQuery<AttributedEntity>());
		AreEqual("SELECT * FROM PlainEntity WHERE Id = @id", GetFetchQuery<PlainEntity>());
		AreEqual("SELECT Id AS Alias, Name FROM Entities WHERE Id = @id", GetFetchQuery<AttributedEntity>("Id AS Alias", "Name"));
		AreEqual("SELECT Id AS Alias, Name FROM PlainEntity WHERE Id = @id", GetFetchQuery<PlainEntity>("Id AS Alias", "Name"));
	}

	[TestMethod]
	public void GetFetchAllQuery() {
		AreEqual("SELECT * FROM Entities", GetFetchAllQuery<AttributedEntity>());
		AreEqual("SELECT * FROM PlainEntity", GetFetchAllQuery<PlainEntity>());
		AreEqual("SELECT Id AS Alias, Name FROM Entities", GetFetchAllQuery<AttributedEntity>("Id AS Alias", "Name"));
		AreEqual("SELECT Id AS Alias, Name FROM PlainEntity", GetFetchAllQuery<PlainEntity>("Id AS Alias", "Name"));
	}

	[TestMethod]
	public void GetInsertQuery() {
		AreEqual("INSERT INTO Entities (Name) VALUES (@EntityName)", GetInsertQuery<AttributedEntity>());
		AreEqual("INSERT INTO PlainEntity (Id, Name, IsMapped) VALUES (@Id, @Name, @IsMapped)", GetInsertQuery<PlainEntity>());
	}

	[TestMethod]
	public void GetMappedProperties() {
		var properties = GetMappedProperties<AttributedEntity>().Select(property => property.Name);
		CollectionAssert.AreEqual(new[] { "EntityId", "EntityName" }, properties.ToArray());
		properties = GetMappedProperties<PlainEntity>().Select(property => property.Name);
		CollectionAssert.AreEqual(new[] { "Id", "Name", "IsMapped" }, properties.ToArray());
	}

	[TestMethod]
	public void GetSingleKey() {
		AreEqual("EntityId", GetSingleKey<AttributedEntity>()?.Name);
		AreEqual("Id", GetSingleKey<PlainEntity>()?.Name);
	}

	[TestMethod]
	public void GetTableName() {
		AreEqual("Entities", GetTableName<AttributedEntity>());
		AreEqual("PlainEntity", GetTableName<PlainEntity>());
	}

	[TestMethod]
	public void GetTruncateQuery() {
		AreEqual("TRUNCATE TABLE Entities", GetTruncateQuery<AttributedEntity>());
		AreEqual("TRUNCATE TABLE PlainEntity", GetTruncateQuery<PlainEntity>());
	}

	[TestMethod]
	public void GetUpdateQuery() {
		AreEqual("UPDATE Entities SET Name = @EntityName WHERE Id = @EntityId", GetUpdateQuery<AttributedEntity>());
		AreEqual("UPDATE PlainEntity SET Name = @Name, IsMapped = @IsMapped WHERE Id = @Id", GetUpdateQuery<PlainEntity>());
	}

	[TestMethod]
	public void IsDatabaseGenerated() {
		IsTrue(typeof(AttributedEntity).GetProperty("EntityId")!.IsDatabaseGenerated());
		IsFalse(typeof(PlainEntity).GetProperty("Id")!.IsDatabaseGenerated());
	}
}
