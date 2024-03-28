namespace Bars.Gkh.Map
{
    using Bars.Gkh.FormatDataExport.ExportableEntities;
    using NHibernate.Mapping.ByCode.Conformist;

    /// <summary>
    /// Базовая реализация маппинга сущностей <see cref="IHaveExportId"/>
    /// <para>Запрещает обновлять поле <see cref="IHaveExportId.ExportId"/></para>
    /// </summary>
    public abstract class BaseHaveExportIdMapping<T> : ClassMapping<T>
        where T : class, IHaveExportId
    {
        protected BaseHaveExportIdMapping()
        {
            this.Property(x => x.ExportId, m =>
            {
                m.Column("EXPORT_ID");
                m.NotNullable(true);
                m.Insert(false);
                m.Update(false);
            });
        }
    }

    /// <summary>
    /// Базовая реализация маппинга сущностей <see cref="IHaveExportId"/>
    /// <para>Запрещает обновлять поле <see cref="IHaveExportId.ExportId"/></para>
    /// <para>Использовать при наследовании сущности (<see cref="JoinedSubclassMapping{T}"/>)</para>
    /// </summary>
    public abstract class BaseHaveExportIdJoinedMapping<T> : JoinedSubclassMapping<T>
        where T : class, IHaveExportId
    {
        protected BaseHaveExportIdJoinedMapping()
        {
            this.Property(x => x.ExportId, m =>
            {
                m.Column("EXPORT_ID");
                m.NotNullable(true);
                m.Insert(false);
                m.Update(false);
            });
        }
    }
}