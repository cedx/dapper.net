namespace Belin.Dapper;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

/// <summary>
/// A sample entity with mapping attributes.
/// </summary>
[Table("Entities")]
internal class AttributedEntity {

	/// <summary>
	/// The entity identifier.
	/// </summary>
	[Key, Column("Id")]
	public int EntityId { get; set; }

	/// <summary>
	/// The entity name.
	/// </summary>
	[Column("Name")]
	public string EntityName { get; set; } = string.Empty;

	/// <summary>
	/// Value indicating whether this entity is archived.
	/// </summary>
	[NotMapped]
	public bool IsAdmin { get; set; }
}

/// <summary>
/// A sample entity without any mapping attributes.
/// </summary>
internal class PlainEntity {

	/// <summary>
	/// The entity identifier.
	/// </summary>
	public int Id { get; set; }

	/// <summary>
	/// The entity name.
	/// </summary>
	public string Name { get; set; } = string.Empty;

	/// <summary>
	/// Value indicating whether this entity is archived.
	/// </summary>
	public bool IsArchived { get; set; }
}
