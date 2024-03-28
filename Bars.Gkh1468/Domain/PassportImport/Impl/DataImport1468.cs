namespace Bars.Gkh1468.Domain.PassportImport.Impl
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using B4;
    using Castle.Windsor;
    using Gkh.Enums.Import;
    using Gkh.Import;
    using Gkh.Import.Impl;
    using Interfaces;
	using System.Threading;
	using Bars.B4.Modules.Tasks.Common.Service;
	using ExecutionContext = Bars.B4.Modules.Tasks.Common.Contracts.ExecutionContext;

    public class DataImport1468 : GkhImportBase
    {
        public static string Id = "1468Import";

        public ILogImportManager LogManager { get; set; }
        public IWindsorContainer Container { get; set; }
        public ILogImport LogImport { get; set; }
        public ILogImportManager LogImportManager { get; set; }
        public IAuthorizationService AuthService { get; set; }
        public IUserIdentity UserIdentity { get; set; }

        public IDataProcessorFactory DataProcessorFactory { get; set; }

        public override string Key { get { return Id; } }

        public override string CodeImport { get { return "1468Import"; } }

        public override string Name { get { return "Импорт паспортов для 1468"; } }

        public override string PossibleFileExtensions { get { return "zip,rar"; } }

        public override string PermissionName { get { return "Gkh1468.Passport.ImportFrom1468.Import"; } }

        public const string ImportWoDigitSignPermission = "Gkh1468.Passport.ImportFrom1468.ImportWoDigitSign";

        public bool AllowImportWoDigitSign = false;
		private long _userId;

		protected override ImportResult Import(BaseParams @params, ExecutionContext ctx, IProgressIndicator indicator, CancellationToken ct)
		{
			_userId = ctx.UserId;
			return base.Import(@params, ctx, indicator, ct);
		}

        public override ImportResult Import(BaseParams baseParams)
        {
			AllowImportWoDigitSign = AuthService.Grant(new ProxyUserIdentity(_userId), ImportWoDigitSignPermission);

            var provider = DataProcessorFactory.CreateDataProcessor(baseParams, new Dictionary<string, object>
            {
                { "logger", LogImport },
                { "allowImportWoDigitSign", AllowImportWoDigitSign }
            });

            try
            {
                ImportInternal(provider);
            }
            catch (Exception exc)
            {
                LogImport.Error("Ошибка", exc.Message);
            }

            LogImport.ImportKey = this.CodeImport;
            LogImport.SetFileName("import1468");
            LogManager.FileNameWithoutExtention = GetFileName(baseParams);

            var statusImport = LogImport.CountError > 0
                ? StatusImport.CompletedWithError
                : LogImport.CountWarning > 0
                    ? StatusImport.CompletedWithWarning
                    : StatusImport.CompletedWithoutError;

            LogManager.Add(GetFileStream(baseParams), GetFileName(baseParams), LogImport);
            LogManager.Save();

            return new ImportResult(statusImport,
                statusImport == StatusImport.CompletedWithError
                    ? "Ошибка. Подробное описание см. в логе загрузок (Администрирование/Логи/Логи загрузок)"
                    : "Импорт завершен",
                    string.Empty,
                    LogManager.LogFileId);
        }

        private Stream GetFileStream(BaseParams baseParams)
        {
            var file = GetFileFromParams(baseParams);

            if (file == null)
            {
                return new MemoryStream();
            }

            return new MemoryStream(file.Data);
        }

        private string GetFileName(BaseParams baseParams)
        {
            FileData file = GetFileFromParams(baseParams);

            if (file == null)
            {
                return "empty";
            }

            return file.FileName;
        }

        private FileData GetFileFromParams(BaseParams baseParams)
        {
            return baseParams.Files.ContainsKey("FileImport") ? baseParams.Files["FileImport"] : null;
        }

        public override bool Validate(BaseParams baseParams, out string message)
        {
            AllowImportWoDigitSign = AuthService.Grant(UserIdentity, ImportWoDigitSignPermission);

            if (!AuthService.Grant(UserIdentity, PermissionName) && !AllowImportWoDigitSign)
            {
                message = "У вас нет разрешения на импорт паспортов!";
                return false;
            }

            var importType = baseParams.Params.GetAs<ImportType1468>("import_type");

            if (importType == ImportType1468.ONLINE)
            {
                message = string.Empty;
                return true;
            }

            message = null;

            foreach (var file in baseParams.Files)
            {
                var fileData = file.Value;
                var extention = fileData.Extention;

                var fileExtentions = PossibleFileExtensions.Split(',');
                if (fileExtentions.All(x => x != extention))
                {
                    message = string.Format("Необходимо выбрать файл с допустимым расширением: {0}", PossibleFileExtensions);
                    return false;
                }
            }

            return true;
        }

        private void ImportInternal(IDataProcessor provider)
        {
            provider.ProcessData();
        }
    }
}