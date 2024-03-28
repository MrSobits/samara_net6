namespace Sobits.RosReg.Migrations._2018.Version_1
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;

    using Sobits.RosReg.Entities;

    [Migration("Version_1")]
    public class UpdateSchema : Migration
    {
        private const string Schema = "RosReg";
        private const string ExtractTable = "Extract";

        public override void Up()
        {
            this.AddSchema();
            this.AddExtract();
        }

        public override void Down()
        {
            this.Database.RemoveTable(new SchemaQualifiedObjectName { Name = UpdateSchema.ExtractTable, Schema = UpdateSchema.Schema });
            this.RemoveSchema();
        }

        private void AddSchema()
        {
            this.Database.ExecuteNonQuery($@"CREATE SCHEMA IF NOT EXISTS {UpdateSchema.Schema}");
        }

        private void RemoveSchema()
        {
            this.Database.ExecuteNonQuery($@"DROP SCHEMA IF EXISTS {UpdateSchema.Schema}");
        }

        #region Tables
        private void AddExtract()
        {
            var tableName = new SchemaQualifiedObjectName { Name = UpdateSchema.ExtractTable, Schema = UpdateSchema.Schema };
            this.Database.AddTable(
                tableName,
                new Column(nameof(Extract.Id).ToLower(), DbType.Int64, ColumnProperty.PrimaryKeyWithIdentity),
                new Column(nameof(Extract.CreateDate).ToLower(), DbType.DateTime),
                new Column(nameof(Extract.Type).ToLower(), DbType.Int32, ColumnProperty.Null),
                new Column(nameof(Extract.IsParsed).ToLower(), DbType.Boolean, false),
                new Column(nameof(Extract.IsActive).ToLower(), DbType.Boolean, false),
                new Column(nameof(Extract.File).ToLower(), DbType.Int64, ColumnProperty.Null));
            this.Database.ExecuteNonQuery($@"ALTER TABLE {UpdateSchema.Schema}.{UpdateSchema.ExtractTable} ADD xml text;");
        }
        #endregion
    }
}