namespace Belin.Dapper;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

/// <summary>
/// A sample entity with mapping attributes.
/// </summary>
[Table("Entities")]
internal class AttributedEntity {

	[Key, Column("Id")]
	public int EntityId { get; set; }

	[Column("Name")]
	public string EntityName { get; set; } = string.Empty;

	[NotMapped]
	public bool IsMapped => false;
}

/// <summary>
/// A sample entity without any mapping attributes.
/// </summary>
internal class PlainEntity {
	public int Id { get; set; }
	public string Name { get; set; } = string.Empty;
	public bool IsMapped => true;
}
