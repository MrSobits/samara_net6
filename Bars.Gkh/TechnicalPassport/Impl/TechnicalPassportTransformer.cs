namespace Bars.Gkh.TechnicalPassport.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Data;

    using Bars.B4.DataAccess;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities.TechnicalPassport;
    using Bars.Gkh.Enums.TechnicalPassport;
    using Bars.Gkh.Utils;

    using Castle.Windsor;

    /// <summary>
    /// Сервис изменения структуры технического паспорта в базе данных
    /// </summary>
    public class TechnicalPassportTransformer : ITechnicalPassportTransformer
    {
        /// <summary>
        /// Название таблицы технического паспорта
        /// </summary>
        public const string TechnicalPassportTableName = "TP_TECHNICAL_PASSPORT";

        private const string SchemaName = "TECHNICAL_PASSPORT";

        private static readonly Dictionary<EditorType, DbType> ColumnTypeMapper = new Dictionary<EditorType, DbType>
        {
            {EditorType.Text, DbType.String},
            {EditorType.Date, DbType.DateTime},
            {EditorType.Int, DbType.Int32},
            {EditorType.Double, DbType.Double},
            {EditorType.Decimal, DbType.Decimal},
            {EditorType.Bool, DbType.Boolean}
        };

        public IWindsorContainer Container { get; set; }

        public void CreateForm(Form form)
        {
            var transformProvider = this.GetTransformationProvider();

            this.CheckSchema(transformProvider);

            transformProvider.AddTable(this.GetTableName(form), this.GetPropertyColumns(form));
        }

        public void UpdateForm(Form form)
        {
            throw new NotImplementedException();
        }

        public void DeleteForm(Form form)
        {
            var transformProvider = this.GetTransformationProvider();

            this.CheckSchema(transformProvider);

            transformProvider.RemoveTable(this.GetTableName(form));
        }

        private ITransformationProvider GetTransformationProvider()
        {
            var session = this.Container.Resolve<ISessionProvider>().GetCurrentSession();
            var transformProvider = MigratorUtils.GetTransformProvider(session.Connection);
            return transformProvider;
        }

        private SchemaQualifiedObjectName GetTableName(Form form)
        {
            return new SchemaQualifiedObjectName
            {
                Name = form.TableName,
                Schema = TechnicalPassportTransformer.SchemaName
            };
        }

        private Column[] GetPropertyColumns(Form form)
        {
            var columns = new List<Column>();

            var refColumn = new RefColumn(
                "TECHNICAL_PASSPORT_ID",
                ColumnProperty.NotNull,
                $"{form.TableName}_TECHNICAL_PASSPORT_ID",
                TechnicalPassportTransformer.TechnicalPassportTableName,
                "ID");

            columns.Add(refColumn);

            foreach (var attribute in form.Attributes)
            {
                var columnProperty = attribute.Required ? ColumnProperty.NotNull : ColumnProperty.Null;

                if (attribute.Editor.EditorType.In(EditorType.Dictionary, EditorType.FormReference, EditorType.TableReference))
                {
                    columns.Add(
                        new RefColumn(
                            attribute.ColumnName,
                            columnProperty,
                            $"{form.TableName}_{attribute.ColumnName}",
                            attribute.Editor.ReferenceTableName,
                            "ID"));
                }
                else
                {
                    columns.Add(
                        new Column(
                            attribute.ColumnName,
                            TechnicalPassportTransformer.ColumnTypeMapper[attribute.Editor.EditorType],
                            columnProperty));
                }
            }

            return columns.ToArray();
        }

        private void CheckSchema(ITransformationProvider transformProvider)
        {
            transformProvider.ExecuteNonQuery($"CREATE SCHEMA IF NOT EXISTS {TechnicalPassportTransformer.SchemaName}");
        }
    }
}