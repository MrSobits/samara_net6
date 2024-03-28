namespace Bars.GkhDi.Migrations.Version_2015111800
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015111800")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhDi.Migrations.Version_2015100402.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("DI_DISINFO_COM_FACILS", new Column("LESSEE_TYPE", DbType.Int32, ColumnProperty.NotNull, 10));
            Database.AddColumn("DI_DISINFO_COM_FACILS", new Column("SURNAME", DbType.String, 300));
            Database.AddColumn("DI_DISINFO_COM_FACILS", new Column("NAME", DbType.String, 300));
            Database.AddColumn("DI_DISINFO_COM_FACILS", new Column("GENDER", DbType.Int32, ColumnProperty.NotNull, 0));
            Database.AddColumn("DI_DISINFO_COM_FACILS", new Column("BIRTH_DATE", DbType.DateTime));
            Database.AddColumn("DI_DISINFO_COM_FACILS", new Column("BIRTH_PLACE", DbType.String, 300));
            Database.AddColumn("DI_DISINFO_COM_FACILS", new Column("SNILS", DbType.String, 50));
            Database.AddColumn("DI_DISINFO_COM_FACILS", new Column("OGRN", DbType.String, 50));
            Database.AddColumn("DI_DISINFO_COM_FACILS", new Column("CONTRACT_SUBJECT", DbType.String, 300));
            Database.AddColumn("DI_DISINFO_COM_FACILS",
                new RefColumn("PROTOCOL_FILE_ID", "DI_DC_FACILS_PROT_FILE", "B4_FILE_INFO", "ID"));
            Database.AddColumn("DI_DISINFO_COM_FACILS",
               new RefColumn("CONTRACT_FILE_ID", "DI_DC_FACILS_CONT_FILE", "B4_FILE_INFO", "ID"));
        }

        public override void Down()
        {
            Database.RemoveColumn("DI_DISINFO_COM_FACILS", "LESSEE_TYPE");
            Database.RemoveColumn("DI_DISINFO_COM_FACILS", "SURNAME");
            Database.RemoveColumn("DI_DISINFO_COM_FACILS", "NAME");
            Database.RemoveColumn("DI_DISINFO_COM_FACILS", "GENDER");
            Database.RemoveColumn("DI_DISINFO_COM_FACILS", "BIRTH_DATE");
            Database.RemoveColumn("DI_DISINFO_COM_FACILS", "BIRTH_PLACE");
            Database.RemoveColumn("DI_DISINFO_COM_FACILS", "SNILS");
            Database.RemoveColumn("DI_DISINFO_COM_FACILS", "OGRN");
            Database.RemoveColumn("DI_DISINFO_COM_FACILS", "CONTRACT_SUBJECT");
            Database.RemoveColumn("DI_DISINFO_COM_FACILS", "PROTOCOL_FILE_ID");
            Database.RemoveColumn("DI_DISINFO_COM_FACILS", "CONTRACT_FILE_ID");
        }
    }
}
