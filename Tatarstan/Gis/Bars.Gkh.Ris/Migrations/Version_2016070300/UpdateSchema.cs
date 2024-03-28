namespace Bars.Gkh.Ris.Migrations.Version_2016070300
{
    using System.Collections.Generic;
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.B4.Utils;
    using Bars.Gkh.Utils;

    [Migration("2016070300")]
    [MigrationDependsOn(typeof(Version_2016070100.UpdateSchema))]
    [MigrationDependsOn(typeof(GisIntegration.Base.Migrations.Version_2016062800.UpdateSchema))]
    [MigrationDependsOn(typeof(GisIntegration.Inspection.Migrations.Version_2016062900.UpdateSchema))]
    [MigrationDependsOn(typeof(GisIntegration.CapitalRepair.Migrations.Version_2016070300.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        //private static readonly List<string> Tables = new List<string>
        //                                         {
        //                                             "ATTACHMENT",
        //                                             "INTEGR_DICT",
        //                                             "INTEGR_REF_DICT",
        //                                             "INSPECTION_EXAM_ATTACH",
        //                                             "INSPECTION_EXAMINATION",
        //                                             "INSPECTION_EXAM_PLACE",
        //                                             "INSPECTION_PLAN",
        //                                             "INSPECTION_OFFENCE_ATTACH",
        //                                             "INSPECTION_OFFENCE",
        //                                             "INSPECTION_PRECEPT_ATTACH",
        //                                             "INSPECTION_PRECEPT",
        //                                             "CONTAINER",
        //                                             "CONTRAGENT",
        //                                             "PACKAGE",
        //                                             "PACKAGE_TRIGGER",
        //                                             "TASK",
        //                                             "TASK_TRIGGER",
        //                                             "SERVICE_SETTINGS",
        //                                             "CR_ATTACHMENT_CONTRACT",
        //                                             "CR_ATTACHMENT_OUTLAY",
        //                                             "CR_CONTRACT",
        //                                             "CR_PLAN",
        //                                             "CR_PLANWORK",
        //                                             "CR_WORK"
        //                                         };

        public override void Up()
        {
            //// 1. снимаем связи между существующими GI_* таблицами
            //foreach (var table in UpdateSchema.Tables)
            //{
            //    this.RemoveForeignKeysTo($"GI_{table}");
            //}

            //// 2. дропаем существующие GI_* таблицы
            //foreach (var table in UpdateSchema.Tables)
            //{
            //    var tableName = $"GI_{table}";
            //    if (this.Database.TableExists(tableName))
            //    {
            //        this.RemoveTable(tableName);
            //    }
            //}

            //// 3. переименовываем RIS_* таблицы в GI_* таблицы
            //foreach (var table in UpdateSchema.Tables)
            //{
            //    this.ReplaceTable($"RIS_{table}", $"GI_{table}");

            //    if (this.Database.ColumnExists($"GI_{table}", "RIS_CONTRAGENT_ID"))
            //    {
            //        this.Database.RenameColumn($"GI_{table}", "RIS_CONTRAGENT_ID", "GI_CONTRAGENT_ID");
            //    }
            //}
        }

        //private void RemoveTable(string tableName)
        //{
        //    this.Database.RemoveTable(tableName);
        //    this.Database.ExecuteNonQuery($"drop sequence if exists {tableName.ToLower()}_id_seq");
        //}

        public override void Down()
        {
            //foreach (var table in UpdateSchema.Tables)
            //{
            //    var risTable = $"RIS_{table}";
            //    var giTable = $"GI_{table}";

            //    var fks = new List<RisMigrationExtensions.ForeignKey>();
            //    if (this.Database.TableExists(giTable))
            //    {
            //        if (this.Database.TableExists(risTable))
            //        {
            //            fks.AddRange(this.Database.GetForeignKeys(risTable));
            //            this.RemoveForeignKeysTo(risTable);
            //            this.RemoveTable(risTable);
            //        }

            //        fks.AddRange(this.Database.GetForeignKeys(giTable));

            //        this.Database.RenameTableAndSequence(giTable, risTable);

            //        foreach (var fk in fks)
            //        {
            //            this.Database.AddForeignKey(fk.ConstraintName, fk.TableName, fk.ColumnName, risTable, fk.ForeignColumnName);
            //        }

            //        if (this.Database.ColumnExists(risTable, "GI_CONTRAGENT_ID"))
            //        {
            //            this.Database.RenameColumn(risTable, "GI_CONTRAGENT_ID", "RIS_CONTRAGENT_ID");
            //        }
            //    }

            //    this.Database.AddPersistentObjectTable(
            //        giTable,
            //        new Column("DUMMY_COLUMN", DbType.Boolean));
            //}
        }

        //private void RemoveForeignKeysTo(string tableName)
        //{
        //    var fks = this.Database.GetForeignKeys(tableName);
        //    foreach (var fk in fks)
        //    {
        //        this.Database.RemoveConstraint(fk.TableName, fk.ConstraintName);
        //    }
        //}

        //private void ReplaceTable(string oldTableName, string newTableName)
        //{
        //    var fks = this.Database.GetForeignKeys(oldTableName);
        //    foreach (var fk in fks)
        //    {
        //        this.Database.RemoveConstraint(fk.TableName, fk.ConstraintName);
        //    }

        //    if (this.Database.TableExists(oldTableName))
        //    {
        //        this.Database.RenameTableAndSequence(oldTableName, newTableName);
        //    }

        //    foreach (var fk in fks)
        //    {
        //        this.Database.AddForeignKey(fk.ConstraintName, fk.TableName, fk.ColumnName, newTableName, fk.ForeignColumnName);
        //    }
        //}
    }
}