namespace Belin.Dapper;

using Microsoft.Data.Sqlite;
using System.Net.Mail;

/// <summary>
/// Tests the features of the <see cref="MailAddressTypeHandler"/> class.
/// </summary>
[TestClass]
public sealed class MailAddressTypeHandlerTest {

	[TestMethod]
	public void Parse() {
		var typeHandler = new MailAddressTypeHandler();

		// It should return `null` if the value is invalid.
		IsNull(typeHandler.Parse(123));
		IsNull(typeHandler.Parse(string.Empty));

		// It should return a mail address if the value is valid.
		var value = typeHandler.Parse("cedric@belin.io");
		IsNotNull(value);
		AreEqual("cedric@belin.io", value.Address);
	}

	[TestMethod]
	public void SetValue() {
		var parameter = new SqliteParameter();
		var typeHandler = new MailAddressTypeHandler();

		// It should set the parameter to `null` if the value is `null`.
		typeHandler.SetValue(parameter, null);
		IsNull(parameter.Value);

		// It should set the parameter to the string representation if the value is not `null`.
		typeHandler.SetValue(parameter, new MailAddress("cedric@belin.io"));
		AreEqual("cedric@belin.io", parameter.Value);
	}
}
