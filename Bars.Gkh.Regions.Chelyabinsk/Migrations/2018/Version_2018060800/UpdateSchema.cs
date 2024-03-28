namespace Bars.Gkh.Regions.Chelyabinsk.Migrations._2018.Version_2018060800
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2018060800")]
    [MigrationDependsOn(typeof(Version_2018050800.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        SchemaQualifiedObjectName table = new SchemaQualifiedObjectName { Name = "ROSREGEXTRACTBIG", Schema = "IMPORT" };
        public override void Up()
        {
            Database.AddTable(table,
                new Column("ID", DbType.Int64, ColumnProperty.PrimaryKeyWithIdentity),
                new Column("CadastralNumber",DbType.String),
                new Column("Address", DbType.String),
                new Column("ExtractDate", DbType.String),
                new Column("ExtractNumber", DbType.String));
            //Нет поддержки xml в нхибернейт, приклеиваем руками
            this.Database.ExecuteNonQuery(@"ALTER TABLE import.rosregextractbig ADD xml xml;");
        }

        public override void Down()
        {
            Database.RemoveTable(table);
        }
    }
}