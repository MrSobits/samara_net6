namespace Bars.B4.Modules.ESIA.Auth.Migrations.Version_1
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    /// <summary>
    /// Migration
    /// </summary>
    [global::Bars.B4.Modules.Ecm7.Framework.Migration("1")]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        /// <summary>
        /// Up
        /// </summary>
        public override void Up()
        {
            Database.AddEntityTable("ESIA_OPERATOR",
                new RefColumn("OPERATOR_ID", "ESIA_GKH_OPERATOR", "GKH_OPERATOR", "ID"),
                new Column("USERID", DbType.String, 50),
                new Column("USERNAME", DbType.String, 50),
                new Column("GENDER", DbType.String, 10),
                new Column("LASTNAME", DbType.String, 50),
                new Column("FIRSTNAME", DbType.String, 50),
                new Column("MIDDLENAME", DbType.String, 50),
                new Column("PERSON_SNILS", DbType.String, 50),
                new Column("PERSON_EMAIL", DbType.String, 50),
                new Column("BIRTHDATE", DbType.String, 50),
                new Column("ORG_POSITION", DbType.String, 50),
                new Column("ORG_NAME", DbType.String, 100),
                new Column("ORG_SHORTNAME", DbType.String, 50),
                new Column("ORG_TYPE", DbType.String, 50),
                new Column("ORG_OGRN", DbType.String, 50),
                new Column("ORG_INN", DbType.String, 50),
                new Column("ORG_KPP", DbType.String, 50),
                new Column("ORG_ADDRESSES", DbType.String, 100),
                new Column("ORG_LEGALFORM", DbType.String, 50));
        }

        /// <summary>
        /// Down
        /// </summary>
        public override void Down()
        {
            Database.RemoveTable("ESIA_OPERATOR");
        }
    }
}
