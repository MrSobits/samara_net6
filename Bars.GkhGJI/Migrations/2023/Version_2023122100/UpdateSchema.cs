namespace Bars.GkhGji.Migrations._2023.Version_2023122210
{
    using B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.GkhGji.Enums;
    using System.Data;

    [Migration("2023122210")]
    [MigrationDependsOn(typeof(Version_2023122200.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddColumn("GJI_RESOLUTION", new Column("PERSON_REG_ADDRESS", DbType.String, 500));
            Database.AddColumn("GJI_RESOLUTION", new Column("PERSON_FACT_ADDRESS", DbType.String, 500));
            Database.AddColumn("GJI_RESOLUTION", new Column("TYPE_PRESENCE", DbType.Int16, ColumnProperty.NotNull, (int)TypeRepresentativePresence.None));
            Database.AddColumn("GJI_RESOLUTION", new Column("REPRESENTATIVE", DbType.String, 500));
            Database.AddColumn("GJI_RESOLUTION", new Column("REASON_TYPE_REQ", DbType.String, 1000));
            Database.AddColumn("GJI_RESOLUTION", new Column("PERSON_BIRTH_DATE", DbType.DateTime, ColumnProperty.None));
            Database.AddColumn("GJI_RESOLUTION", new Column("PERSON_BIRTH_PLACE", DbType.String, 500));

            Database.AddColumn("GJI_RESOLPROS", new Column("SURNAME", DbType.String, 50));
            Database.AddColumn("GJI_RESOLPROS", new Column("FIRSTNAME", DbType.String, 50));
            Database.AddColumn("GJI_RESOLPROS", new Column("PATRONYMIC", DbType.String, 50));
            Database.AddColumn("GJI_RESOLPROS", new Column("PERSON_REG_ADDRESS", DbType.String, 500));
            Database.AddColumn("GJI_RESOLPROS", new Column("PERSON_FACT_ADDRESS", DbType.String, 500));
            Database.AddColumn("GJI_RESOLPROS", new Column("TYPE_PRESENCE", DbType.Int16, ColumnProperty.NotNull, (int)TypeRepresentativePresence.None));
            Database.AddColumn("GJI_RESOLPROS", new Column("REPRESENTATIVE", DbType.String, 500));
            Database.AddColumn("GJI_RESOLPROS", new Column("REASON_TYPE_REQ", DbType.String, 1000));
            Database.AddColumn("GJI_RESOLPROS", new Column("PERSON_BIRTH_DATE", DbType.DateTime, ColumnProperty.None));
            Database.AddColumn("GJI_RESOLPROS", new Column("PERSON_BIRTH_PLACE", DbType.String, 500));
            Database.AddColumn("GJI_RESOLPROS", new RefColumn("PHYSICALPERSON_DOCTYPE_ID", "GJI_RESOLPROS_PHYSICALPERSONDOCTYPE_ID", "GJI_DICT_PHYSICAL_PERSON_DOC_TYPE", "ID"));
            Database.AddColumn("GJI_RESOLPROS", new Column("PHYSICALPERSON_DOC_NUM", DbType.String, 10));
            Database.AddColumn("GJI_RESOLPROS", new Column("PHYSICALPERSON_DOC_SERIAL", DbType.String, 4));
            Database.AddColumn("GJI_RESOLPROS", new Column("PHYSICALPERSON_NOT_CITIZENSHIP", DbType.Boolean, false));
        }

        public override void Down()
        {
            this.Database.RemoveColumn("GJI_RESOLPROS", "PHYSICALPERSON_DOC_NUM");
            this.Database.RemoveColumn("GJI_RESOLPROS", "PHYSICALPERSON_DOC_SERIAL");
            this.Database.RemoveColumn("GJI_RESOLPROS", "PHYSICALPERSON_NOT_CITIZENSHIP");
            this.Database.RemoveColumn("GJI_RESOLPROS", "PHYSICALPERSON_DOCTYPE_ID");
            this.Database.RemoveColumn("GJI_RESOLPROS", "PERSON_BIRTH_PLACE");
            this.Database.RemoveColumn("GJI_RESOLPROS", "PERSON_BIRTH_DATE");
            this.Database.RemoveColumn("GJI_RESOLPROS", "REASON_TYPE_REQ");
            this.Database.RemoveColumn("GJI_RESOLPROS", "REPRESENTATIVE");
            this.Database.RemoveColumn("GJI_RESOLPROS", "TYPE_PRESENCE");
            this.Database.RemoveColumn("GJI_RESOLPROS", "PERSON_FACT_ADDRESS");
            this.Database.RemoveColumn("GJI_RESOLPROS", "PERSON_REG_ADDRESS");
            this.Database.RemoveColumn("GJI_RESOLPROS", "PATRONYMIC");
            this.Database.RemoveColumn("GJI_RESOLPROS", "FIRSTNAME");
            this.Database.RemoveColumn("GJI_RESOLPROS", "SURNAME");

            this.Database.RemoveColumn("GJI_RESOLUTION", "PERSON_BIRTH_PLACE");
            this.Database.RemoveColumn("GJI_RESOLUTION", "PERSON_BIRTH_DATE");
            this.Database.RemoveColumn("GJI_RESOLUTION", "REASON_TYPE_REQ");
            this.Database.RemoveColumn("GJI_RESOLUTION", "REPRESENTATIVE");
            this.Database.RemoveColumn("GJI_RESOLUTION", "TYPE_PRESENCE");
            this.Database.RemoveColumn("GJI_RESOLUTION", "PERSON_FACT_ADDRESS");
            this.Database.RemoveColumn("GJI_RESOLUTION", "PERSON_REG_ADDRESS");

        }
    }
}