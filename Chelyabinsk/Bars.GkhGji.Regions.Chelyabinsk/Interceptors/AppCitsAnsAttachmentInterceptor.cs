namespace Bars.GkhGji.Regions.Chelyabinsk.Interceptors
{
    using Bars.B4;
    using Bars.B4.Config;
    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Regions.Chelyabinsk.Services.Impl;

    /// <summary>
    /// Загружает вложение на сторонний ftp ресурс
    /// </summary>
    /// <remarks>Требуется для интеграции с ОГ Челябинск</remarks>
    public class AppCitsAnsAttachmentInterceptor : EmptyDomainInterceptor<AppealCitsAnswerAttachment>
    {
        public IConfigProvider ConfigProvider { get; set; }
        public IFileManager FileManager { get; set; }

        public override IDataResult BeforeCreateAction(IDomainService<AppealCitsAnswerAttachment> service, AppealCitsAnswerAttachment entity)
        {
            return this.UploadFile(entity);
        }

        public override IDataResult BeforeUpdateAction(IDomainService<AppealCitsAnswerAttachment> service, AppealCitsAnswerAttachment entity)
        {
            return this.UploadFile(entity);
        }

        private IDataResult UploadFile(AppealCitsAnswerAttachment entity)
        {
            if (entity.FileInfo != null && (entity.AppealCitsAnswer?.AppealCits?.AppealUid.HasValue ?? false))
            {
                var fullName = this.GetUniqueName(entity);

                var appSettings = this.ConfigProvider.GetConfig().AppSettings;
                var ftpServer = appSettings.GetAs<string>("ftpServer");
                var ftpUser = appSettings.GetAs<string>("ftpUser");
                var ftpPassword = appSettings.GetAs<string>("ftpPassword");
                var ftpUtility = new Bars.Gkh.Utils.FtpUtility(ftpServer, ftpUser, ftpPassword);

                entity.UniqueName = ftpUtility.UploadFile(this.FileManager.GetFile(entity.FileInfo), fullName, true);
            }
            
            return this.Success();
        }

        private string GetUniqueName(AppealCitsAnswerAttachment entity)
        {
            var appCitDir = entity.AppealCitsAnswer.AppealCits.AppealUid;
            var answerDir = entity.AppealCitsAnswer.Id;
            var fileName = $"{entity.FileInfo.Id}.{entity.FileInfo.Extention}";
            return $"{appCitDir}/{answerDir}/{fileName}";
          //  return $"{fileName}";
        }
    }
}
