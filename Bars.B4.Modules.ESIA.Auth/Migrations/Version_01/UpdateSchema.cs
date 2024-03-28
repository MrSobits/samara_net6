namespace Bars.B4.Modules.ESIA.Auth.Migrations.Version_01
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("1")]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddTable("B4_USER_PROFILE_SNILS",
                new Column("ID", DbType.Int64, ColumnProperty.NotNull | ColumnProperty.PrimaryKey | ColumnProperty.Identity),

                new Column("USER_ID", DbType.Int64, ColumnProperty.Null),
                new Column("SNILS", DbType.String, 50));
        }


        public override void Down()
        {
            Database.RemoveTable("B4_USER_PROFILE_SNILS");
        }
    }
}
