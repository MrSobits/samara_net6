namespace Bars.Gkh.ImportExport
{
    /// <summary>
    /// Интерфейс для регистрации сущностей для экспорта
    /// </summary>
    public interface IEntityExportProvider
    {
        void FillContainer(EntityExportContainer container);
    }
}