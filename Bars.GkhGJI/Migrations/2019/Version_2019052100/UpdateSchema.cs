namespace Bars.GkhGji.Migrations._2019.Version_2019052100
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2019052100")]
    [MigrationDependsOn(typeof(Bars.GkhGji.Migrations._2019.Version_2019041000.UpdateSchema))]

    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddColumn("GJI_APPEAL_CITIZENS", new Column("SSTU", DbType.Int32, 4, ColumnProperty.NotNull, 10));
            Database.AddColumn("GJI_APPEAL_CITIZENS", new Column("QUESTION_STATE", DbType.Int32, 4, ColumnProperty.NotNull, 0));
            Database.AddColumn("GJI_APPEAL_CITIZENS", new Column("TRANSFER_ORG_ID", DbType.Int64));
            Database.AddEntityTable("GJI_DICT_SSTU_TRANSFER",
                 new Column("DESCRIPTION", DbType.String, 2000),
                 new Column("NAME", DbType.String, 500),
                 new Column("SSTU_GUID", DbType.String, 300));
        }

        public override void Down()
        {
            Database.RemoveColumn("GJI_APPEAL_CITIZENS", "QUESTION_STATE");
            Database.RemoveColumn("GJI_APPEAL_CITIZENS", "SSTU");
            Database.RemoveColumn("GJI_APPEAL_CITIZENS", "TRANSFER_ORG_ID");
            Database.RemoveTable("GJI_DICT_SSTU_TRANSFER");
        }
    }
}