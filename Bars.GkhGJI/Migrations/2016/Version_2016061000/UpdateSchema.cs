namespace Bars.GkhGji.Migrations.Version_2016061000
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2016061000")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Migrations.Version_2016020800.UpdateSchema))]

    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            this.Database.AddEntityTable("GJI_ORGAN_MVD",
                 new Column("NAME", DbType.String, 200),
                 new Column("COD", DbType.Int32),
                 new RefColumn("MUNICIPALITY_ID", "GJI_ORGAN_MVD_AND_GKH_DICT_MUNICIPALITY", "GKH_DICT_MUNICIPALITY", "ID"));

            this.Database.AddRefColumn("GJI_PROTOCOL_MVD", new RefColumn("ORGAN_MVD_ID", "PROTOCOL_MVD_GKH_ORGAN_MVD", "GJI_ORGAN_MVD", "ID"));
            this.Database.AddColumn("GJI_PROTOCOL_MVD", new Column("DATE_OFFENSE", DbType.DateTime));
            this.Database.AddColumn("GJI_PROTOCOL_MVD", new Column("SERIAL_AND_NUMBER", DbType.String, 50));
            this.Database.AddColumn("GJI_PROTOCOL_MVD", new Column("BIRTH_DATE", DbType.DateTime));
            this.Database.AddColumn("GJI_PROTOCOL_MVD", new Column("ISSUE_DATE", DbType.DateTime));
            this.Database.AddColumn("GJI_PROTOCOL_MVD", new Column("BIRTH_PLACE", DbType.String,255));
            this.Database.AddColumn("GJI_PROTOCOL_MVD", new Column("ISSUING_AUTHORITY", DbType.String, 150));
            this.Database.AddColumn("GJI_PROTOCOL_MVD", new Column("COMPANY", DbType.String, 255));

            this.Database.AddColumn("GJI_RESOLUTION", new Column("RULING_NUMBER", DbType.Int64));
            this.Database.AddColumn("GJI_RESOLUTION", new Column("OFFENDER_WAS", DbType.Int16));
            this.Database.AddColumn("GJI_RESOLUTION", new Column("RULING_DATE", DbType.DateTime));
            this.Database.AddColumn("GJI_RESOLUTION", new Column("RULIN_FIO", DbType.String, 150));

            this.Database.AddRefColumn("GJI_PROTOCOL_MVD", new RefColumn("MUNICIPALITY_ID_RT", "GJI_ORGAN_MVD_AND_GKH_DICT_MUNICIPALITY_RT", "GKH_DICT_MUNICIPALITY", "ID"));

            this.Database.ExecuteNonQuery("update GJI_RESOLUTION set OFFENDER_WAS=30");
            this.Database.ExecuteNonQuery("update GJI_PROTOCOL_MVD set MUNICIPALITY_ID_RT =MUNICIPALITY_ID");

          }

        public override void Down()
        {
            this.Database.RemoveColumn("GJI_PROTOCOL_MVD", "ORGAN_MVD_ID");
            this.Database.RemoveTable("GJI_ORGAN_MVD");

            this.Database.RemoveColumn("GJI_PROTOCOL_MVD", "DATE_OFFENSE");
            this.Database.RemoveColumn("GJI_PROTOCOL_MVD", "TIME_OFFENSE");
            this.Database.RemoveColumn("GJI_PROTOCOL_MVD", "SERIAL_AND_NUMBER");
            this.Database.RemoveColumn("GJI_PROTOCOL_MVD", "BIRTH_DATE");
            this.Database.RemoveColumn("GJI_PROTOCOL_MVD", "ISSUE_DATE");
            this.Database.RemoveColumn("GJI_PROTOCOL_MVD", "BIRTH_PLACE");
            this.Database.RemoveColumn("GJI_PROTOCOL_MVD", "ISSUING_AUTHORITY");
            this.Database.RemoveColumn("GJI_PROTOCOL_MVD", "COMPANY");

            this.Database.RemoveColumn("GJI_RESOLUTION", "RULING_NUMBER");
            this.Database.RemoveColumn("GJI_RESOLUTION", "OFFENDER_WAS");
            this.Database.RemoveColumn("GJI_RESOLUTION", "RULING_DATE");
            this.Database.RemoveColumn("GJI_RESOLUTION", "RULIN_FIO");

            this.Database.ExecuteNonQuery("update GJI_PROTOCOL_MVD set MUNICIPALITY_ID = MUNICIPALITY_ID_RT");
            this.Database.RemoveColumn("GJI_PROTOCOL_MVD", "MUNICIPALITY_ID_RT");
        }

    }
}
