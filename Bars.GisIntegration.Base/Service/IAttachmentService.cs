namespace Bars.GisIntegration.Base.Service
{
    using Bars.B4.Modules.FileStorage;
    using Bars.GisIntegration.Base.Entities;
    using Bars.GisIntegration.Base.Enums;

    /// <summary>
    /// Интерфейс сервиса для работы с вложениями
    /// </summary>
    public interface IAttachmentService
    {
        /// <summary>
        /// Создать вложение
        /// </summary>
        /// <param name="file">Файл</param>
        /// <param name="description">Описание</param>
        /// <returns>Вложение</returns>
        Attachment CreateAttachment(FileInfo file, string description);

        /// <summary>
        /// Загрузить вложение в ГИС ЖКХ
        /// </summary>
        /// <param name="attachment">Вложение</param>
        /// <param name="orgPpaGuid">Идентификатор зарегистрированной организации</param>
        void UploadAttachment(Attachment attachment, string orgPpaGuid);
    }
}