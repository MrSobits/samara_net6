namespace Bars.Gkh.Migrations._2015.Version_2015111400
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015111400")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migrations.Version_2015111000.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable("GKH_DICT_CATEGORY_POSTS",
                new Column("CODE", DbType.String, 300),
                new Column("NAME", DbType.String, 500));

            Database.AddEntityTable("GKH_DICT_MESSAGE_SUBJECT",
                new Column("CODE", DbType.String, 300),
                new Column("NAME", DbType.String, 500),
                new RefColumn("CATEGORY_POSTS_ID", ColumnProperty.NotNull, "CATEGORY_POSTS_SUBJECT", "GKH_DICT_CATEGORY_POSTS", "ID"));
        }

        public override void Down()
        {
            Database.RemoveTable("GKH_DICT_CATEGORY_POSTS");

            Database.RemoveTable("GKH_DICT_MESSAGE_SUBJECT");
        }
    }
}
