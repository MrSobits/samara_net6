namespace Bars.GisIntegration.Base.Migrations.Version_2016101100
{
    using System;
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.GisIntegration.Base.Enums;

    [Migration("2016101100")]
    [MigrationDependsOn(typeof(Version_2016090100.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.AddEntityTable("DELEGACY",
                new Column("END_DATE", DbType.DateTime, ColumnProperty.NotNull),
                new Column("START_DATE", DbType.DateTime, ColumnProperty.NotNull),
                new RefColumn("OPERATOR_IS_ID", "DELEGACY_OPERATOR", "GI_CONTRAGENT", "ID"),
                new RefColumn("INFORMATION_PROVIDER_ID", "DELEGACY_INFO_PROV", "GI_CONTRAGENT", "ID"));

            this.Database.AddEntityTable("GIS_ROLE",
                new Column("NAME", DbType.String));

            this.Database.AddEntityTable("GIS_ROLE_METHOD",
                new Column("METHOD_ID", DbType.String, ColumnProperty.NotNull),
                new Column("METHOD_NAME", DbType.String, ColumnProperty.NotNull),
                new RefColumn("GIS_ROLE_ID", "GIS_ROLE_METHOD_ROLE", "GIS_ROLE", "ID"));

            this.Database.AddEntityTable("GIS_OPERATOR",
                new RefColumn("RIS_CONTRAGENT_ID", "GIS_OPERATOR_CONTR", "GI_CONTRAGENT", "ID"));

            this.Database.AddEntityTable("GIS_ROLE_CONTRAGENT",
                new RefColumn("GIS_OPERATOR_ID", "GIS_ROLE_CONTR_OPER", "GIS_OPERATOR", "ID"),
                new RefColumn("GIS_ROLE_ID", "GIS_ROLE_CONTR_ROLE", "GIS_ROLE", "ID"));
        }

        public override void Down()
        {
            this.Database.RemoveTable("GIS_ROLE_CONTRAGENT");
            this.Database.RemoveTable("GIS_OPERATOR");
            this.Database.RemoveTable("GIS_ROLE_METHOD");
            this.Database.RemoveTable("GIS_ROLE");
            this.Database.RemoveTable("DELEGACY");
        }
    }
}