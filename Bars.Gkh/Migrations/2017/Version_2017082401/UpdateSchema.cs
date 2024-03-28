namespace Bars.Gkh.Migrations._2017.Version_2017082401
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.Gkh.Utils;

    [Migration("2017082401")]   
    [MigrationDependsOn(typeof(Version_2017071000.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.AddEntityTable(
                "GKH_HOUSING_INSPECTION",
                new RefColumn("CONTRAGENT_ID", "GKH_HOUSING_INSP_CONTR_ID", "GKH_CONTRAGENT", "ID"));

            this.Database.AddEntityTable(
                "GKH_HOUSING_INSPECTION_MUNICIPALITY",
                new RefColumn("HOUSING_INSPECTION_ID", "GKH_INSP_MUN_HOUSING_ID", "GKH_HOUSING_INSPECTION", "ID"),
                new RefColumn("MUNICIPALITY_ID", "GKH_HOUSING_INSP_MUN_ID", "GKH_DICT_MUNICIPALITY", "ID"));

            this.Database.AddColumn("GKH_MANORG_LIC_REQUEST", new Column("ORDER_DATE", DbType.DateTime));
            this.Database.AddColumn("GKH_MANORG_LIC_REQUEST", new Column("ORDER_NUMBER", DbType.String));
            this.Database.AddRefColumn("GKH_MANORG_LIC_REQUEST", new FileColumn("ORDER_FILE", "GKH_MNRG_LIC_RQUST_ORDER_FILE"));

            this.Database.AddColumn("GKH_MANORG_LIC_REQUEST", new Column("IDENT_TYPE", DbType.Int32));
            this.Database.AddColumn("GKH_MANORG_LIC_REQUEST", new Column("IDENT_SERIAL", DbType.String, 10));
            this.Database.AddColumn("GKH_MANORG_LIC_REQUEST", new Column("IDENT_NUMBER", DbType.String, 10));
            this.Database.AddColumn("GKH_MANORG_LIC_REQUEST", new Column("IDENT_ISSUEDBY", DbType.String, 2000));
            this.Database.AddColumn("GKH_MANORG_LIC_REQUEST", new Column("IDENT_ISSUEDDATE", DbType.DateTime));

            this.Database.AddColumn("GKH_MANORG_LICENSE", new Column("ORG_MADE_DEC_TERMINATION", DbType.String));
            this.Database.AddColumn("GKH_MANORG_LICENSE", new Column("DOCUMENT_TERMINATION", DbType.String));
            this.Database.AddColumn("GKH_MANORG_LICENSE", new Column("DOCUMENT_NUMBER_TERMINATION", DbType.String));
            this.Database.AddColumn("GKH_MANORG_LICENSE", new Column("DOCUMENT_DATE_TERMINATION", DbType.DateTime));
            this.Database.AddRefColumn("GKH_MANORG_LICENSE", new FileColumn("TERMINATION_FILE_ID", "GKH_MANORG_LICENSE_FILE_ID"));
            this.Database.AddRefColumn("GKH_MANORG_LICENSE", new RefColumn("HOUSING_INSPECTION_ID", "GKH_MNRG_LIC_HOUS_INSP_ID", "GKH_HOUSING_INSPECTION", "ID"));

            this.Database.AddColumn("GKH_MANORG_LICENSE", new Column("IDENT_TYPE", DbType.Int32));
            this.Database.AddColumn("GKH_MANORG_LICENSE", new Column("IDENT_SERIAL", DbType.String, 10));
            this.Database.AddColumn("GKH_MANORG_LICENSE", new Column("IDENT_NUMBER", DbType.String, 10));
            this.Database.AddColumn("GKH_MANORG_LICENSE", new Column("IDENT_ISSUEDBY", DbType.String, 2000));
            this.Database.AddColumn("GKH_MANORG_LICENSE", new Column("IDENT_ISSUEDDATE", DbType.DateTime));

            this.Database.AddEntityTable("GKH_MANORG_LICENSE_PERSON",
                new RefColumn("LIC_ID", ColumnProperty.NotNull, "GKH_MANORG_LIC_PERS_LIC_ID", "GKH_MANORG_LICENSE", "ID"),
                new RefColumn("PERSON_ID", ColumnProperty.NotNull, "GKH_MANORG_LIC_PERS_PERS_ID", "GKH_PERSON", "ID"));
        }

        public override void Down()
        {
            this.Database.RemoveTable("GKH_HOUSING_INSPECTION_MUNICIPALITY");
            this.Database.RemoveColumn("GKH_MANORG_LICENSE", "HOUSING_INSPECTION_ID");
            this.Database.RemoveTable("GKH_HOUSING_INSPECTION");
            this.Database.RemoveColumn("GKH_MANORG_LIC_REQUEST", "ORDER_DATE");
            this.Database.RemoveColumn("GKH_MANORG_LIC_REQUEST", "ORDER_NUMBER");
            this.Database.RemoveColumn("GKH_MANORG_LIC_REQUEST", "ORDER_FILE");

            this.Database.RemoveColumn("GKH_MANORG_LIC_REQUEST", "IDENT_TYPE");
            this.Database.RemoveColumn("GKH_MANORG_LIC_REQUEST", "IDENT_SERIAL");
            this.Database.RemoveColumn("GKH_MANORG_LIC_REQUEST", "IDENT_NUMBER");
            this.Database.RemoveColumn("GKH_MANORG_LIC_REQUEST", "IDENT_ISSUEDBY");
            this.Database.RemoveColumn("GKH_MANORG_LIC_REQUEST", "IDENT_ISSUEDDATE");

            this.Database.RemoveColumn("GKH_MANORG_LICENSE", "IDENT_TYPE");
            this.Database.RemoveColumn("GKH_MANORG_LICENSE", "IDENT_SERIAL");
            this.Database.RemoveColumn("GKH_MANORG_LICENSE", "IDENT_NUMBER");
            this.Database.RemoveColumn("GKH_MANORG_LICENSE", "IDENT_ISSUEDBY");
            this.Database.RemoveColumn("GKH_MANORG_LICENSE", "IDENT_ISSUEDDATE");

            this.Database.RemoveColumn("GKH_MANORG_LICENSE", "ORG_MADE_DEC_TERMINATION");
            this.Database.RemoveColumn("GKH_MANORG_LICENSE", "DOCUMENT_TERMINATION");
            this.Database.RemoveColumn("GKH_MANORG_LICENSE", "DOCUMENT_NUMBER_TERMINATION");
            this.Database.RemoveColumn("GKH_MANORG_LICENSE", "DOCUMENT_DATE_TERMINATION");
            this.Database.RemoveColumn("GKH_MANORG_LICENSE", "TERMINATION_FILE_ID");  
            this.Database.RemoveTable("GKH_MANORG_LICENSE_PERSON");
        }
    }
}