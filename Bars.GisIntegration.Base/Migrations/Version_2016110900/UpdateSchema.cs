namespace Bars.GisIntegration.Base.Migrations.Version_2016110900
{
    using System.Data;
    using System.Linq;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.GisIntegration.Base.Extensions;

    /// <summary>
    /// Миграция перенесена из RegOp (от 10.03) за исключением тех таблиц, которые уже были ранее добавлены в этом модуле
    /// </summary>
    [Migration("2016110900")]
    [MigrationDependsOn(typeof(Version_2016102800.UpdateSchema))]
    public partial class UpdateSchema : Migration
    {
        public override void Up()
        {
            var ris_trustdocattachment = new SchemaQualifiedObjectName { Name = "ris_trustdocattachment", Schema = "public" };
            if (Database.TableExists(ris_trustdocattachment) && !Database.ColumnExists(ris_trustdocattachment, "attachment_id"))
            {
                Database.AddColumn(ris_trustdocattachment, new Column("attachment_id", DbType.Int64));
            }

            // контейнеры больше не используем
            if (this.Database.TableExists("GI_CONTAINER"))
            {
                var fks = this.Database.GetForeignKeys("GI_CONTAINER");
                foreach (var fk in fks)
                {
                    this.Database.RemoveColumn(fk.TableName, fk.ColumnName);
                }

                this.Database.RemoveTable("GI_CONTAINER");
            }

            //Добавление РИСовых сущностей
            foreach (var table in this.tables)
            {
                if (!this.Database.TableExists(table.Name))
                {
                    this.Database.ExecuteNonQuery(table.CreationScript);
                }
                foreach (var index in table.IndexList)
                {
                    if (!this.Database.IndexExists(index.Name, table.Name))
                    {
                        this.Database.ExecuteNonQuery(index.CreationScript);
                    }
                }
            }
        }

        /// <summary>
        /// Метод миграции на версию назад
        /// </summary>
        public override void Down()
        {
            var reverseTables = this.tables.ToList();
            reverseTables.Reverse();

            foreach (var table in reverseTables)
            {
                if (this.Database.TableExists(table.Name))
                {
                    this.Database.RemoveTable(table.Name);
                }
            }
        }
    }
}