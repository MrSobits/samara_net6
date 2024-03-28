namespace Bars.GkhGji.Regions.Nso.Migrations.Version_2015052900
{
    using System.Data;

    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.GkhGji.Regions.Nso.Enums;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015052900")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Regions.Nso.Migrations.Version_2015052600.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("GJI_NSO_PROTOCOL", new Column("FORMAT_PLACE", DbType.String, 500, ColumnProperty.Null));
            Database.AddColumn("GJI_NSO_PROTOCOL", new Column("FORMAT_HOUR", DbType.Int32, ColumnProperty.Null));
            Database.AddColumn("GJI_NSO_PROTOCOL", new Column("FORMAT_MINUTE", DbType.Int32, ColumnProperty.Null));

            Database.RenameColumn("GJI_NSO_PROTOCOL", "PERSON_ADDRESS", "PERSON_REG_ADDRESS");
            Database.AddColumn(
                "GJI_NSO_PROTOCOL",
                new Column("PERSON_FACT_ADDRESS", DbType.String, 250, ColumnProperty.Null));

            Database.AddColumn(
                "GJI_NSO_PROTOCOL",
                new Column("TYPE_PRESENCE", DbType.Int16, ColumnProperty.NotNull, (int)TypeRepresentativePresence.None));
            Database.AddColumn(
                "GJI_NSO_PROTOCOL",
                new Column("REPRESENTATIVE", DbType.String, 500, ColumnProperty.Null));
            Database.AddColumn(
                "GJI_NSO_PROTOCOL",
                new Column("REASON_TYPE_REQ", DbType.String, 1000, ColumnProperty.Null));

            Database.AddColumn(
                "GJI_NSO_PROTOCOL",
                new Column("DELIV_THROUGH_OFFICE", DbType.Boolean, ColumnProperty.NotNull, false));
            Database.AddColumn("GJI_NSO_PROTOCOL", new Column("PROCEEDING_COPY_NUM", DbType.Int32, ColumnProperty.Null));
            Database.AddRefColumn(
                "GJI_NSO_PROTOCOL",
                new RefColumn(
                    "NORMATIVE_DOC_ID",
                    ColumnProperty.Null,
                    "NSO_PROT_NORM_DOC",
                    "GKH_DICT_NORMATIVE_DOC",
                    "ID"));

            Database.AddEntityTable(
                "GJI_NSO_PROTO_SUR_REQ",
                new RefColumn("PROTOCOL_ID", ColumnProperty.NotNull, "PROT_REQ_PROT", "GJI_NSO_PROTOCOL", "ID"),
                new RefColumn(
                    "REQUIREMENT_ID",
                    ColumnProperty.NotNull,
                    "PROT_REQ_REQ",
                    "GJI_DICT_SURVEY_SUBJ_REQ",
                    "ID"));

            Database.AddColumn(
                "GJI_NSO_PROTOCOL",
                new Column("DATE_OF_VIOLATION", DbType.DateTime, ColumnProperty.Null));
            Database.AddColumn(
                "GJI_NSO_PROTOCOL",
                new Column("HOUR_OF_VIOLATION", DbType.Int32, ColumnProperty.Null));
            Database.AddColumn(
                "GJI_NSO_PROTOCOL",
                new Column("MINUTE_OF_VIOLATION", DbType.Int32, ColumnProperty.Null));
            Database.AddRefColumn(
                "GJI_NSO_PROTOCOL",
                new RefColumn(
                    "RESOLVE_VIOL_CLAIM_ID",
                    ColumnProperty.Null,
                    "NSO_PROTO_RES_V_CL",
                    "GJI_DICT_RES_VIOL_CLAIM",
                    "ID"));

            Database.AddColumn("GJI_PROTOCOL_LTEXT", new Column("WITNESSES", DbType.Binary));
            Database.AddColumn("GJI_PROTOCOL_LTEXT", new Column("VICTIMS", DbType.Binary));

            Database.AddEntityTable(
                "GJI_PROT_ACTIV_DIRECT",
                new RefColumn("ACTIVEDIRECT_ID", ColumnProperty.NotNull, "GJI_PROT_ACTIV_DIRECT_A", "GJI_ACTIVITY_DIRECTION", "ID"),
                new RefColumn("PROTOCOL_ID", ColumnProperty.NotNull, "GJI_PROT_ACTIV_DIRECT_P", "GJI_PROTOCOL", "ID"));

            Database.AddEntityTable(
                "GJI_PROT_BASE_DOC",
                new RefColumn(
                    "KIND_BASE_DOC_ID",
                    ColumnProperty.NotNull,
                    "GJI_PROT_BASE_DOC_D",
                    "GJI_KIND_BASE_DOC",
                    "ID"),
                new RefColumn("PROTOCOL_ID", ColumnProperty.NotNull, "GJI_PROT_BASE_DOC_P", "GJI_PROTOCOL", "ID"),
                new RefColumn(
                    "REALITY_OBJECT_ID",
                    ColumnProperty.NotNull,
                    "GJI_PROT_BASE_DOC_R",
                    "GKH_REALITY_OBJECT",
                    "ID"),
                new Column("NUM_DOC", DbType.String, 300),
                new Column("DATE_DOC", DbType.DateTime));
        }       

        public override void Down()
        {
            Database.RemoveColumn("GJI_NSO_PROTOCOL", "NORMATIVE_DOC_ID");

            Database.RemoveTable("GJI_PROT_BASE_DOC");
            Database.RemoveTable("GJI_PROT_ACTIV_DIRECT");

            Database.RemoveColumn("GJI_PROTOCOL_LTEXT", "VICTIMS");
            Database.RemoveColumn("GJI_PROTOCOL_LTEXT", "WITNESSES");

            Database.RemoveColumn("GJI_NSO_PROTOCOL", "RESOLVE_VIOL_CLAIM_ID");
            Database.RemoveColumn("GJI_NSO_PROTOCOL", "MINUTE_OF_VIOLATION");
            Database.RemoveColumn("GJI_NSO_PROTOCOL", "HOUR_OF_VIOLATION");
            Database.RemoveColumn("GJI_NSO_PROTOCOL", "DATE_OF_VIOLATION");

            Database.RemoveTable("GJI_NSO_PROTO_SUR_REQ");

            Database.RemoveColumn("GJI_NSO_PROTOCOL", "FORMAT_PLACE");
            Database.RemoveColumn("GJI_NSO_PROTOCOL", "FORMAT_HOUR");
            Database.RemoveColumn("GJI_NSO_PROTOCOL", "FORMAT_MINUTE");

            Database.RenameColumn("GJI_NSO_PROTOCOL", "PERSON_REG_ADDRESS", "PERSON_ADDRESS");
            Database.RemoveColumn("GJI_NSO_PROTOCOL", "PERSON_FACT_ADDRESS");

            Database.RemoveColumn("GJI_NSO_PROTOCOL", "TYPE_PRESENCE");
            Database.RemoveColumn("GJI_NSO_PROTOCOL", "REPRESENTATIVE");
            Database.RemoveColumn("GJI_NSO_PROTOCOL", "REASON_TYPE_REQ");

            Database.RemoveColumn("GJI_NSO_PROTOCOL", "DELIV_THROUGH_OFFICE");
            Database.RemoveColumn("GJI_NSO_PROTOCOL", "PROCEEDING_COPY_NUM");
        }
    }
}