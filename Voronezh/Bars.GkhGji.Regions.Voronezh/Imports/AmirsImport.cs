namespace Bars.GkhGji.Regions.Voronezh.Imports
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using B4;
    using Entities;
    using Ionic.Zip;
    using Castle.Windsor;
    using B4.DataAccess;
    using Import;
    using Gkh.Enums.Import;
    using System.Xml.Serialization;
    using Dapper;
    using Gkh.Import.Impl;
    using Gkh.Import;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.Protocol;
    using Bars.GkhGji.Entities;
    using Bars.GkhExcel;
    using System.Collections.Generic;

    public class AmirsImport : GkhImportBase
    {
        #region Base
        public static string Id = MethodBase.GetCurrentMethod().DeclaringType.FullName;

        public IWindsorContainer Container { get; set; }

        public ILogImportManager LogManager { get; set; }

        public ILogImport LogImport { get; set; }

        public ILogImportManager LogImportManager { get; set; }

        public override string Key
        {
            get { return Id; }
        }

        public override string CodeImport
        {
            get { return "AmirsImport"; }
        }

        public override string Name
        {
            get { return "Импорт выписок из Росреестра"; }
        }

        public override string PossibleFileExtensions
        {
            get { return "xksx"; }
        }

        public override string PermissionName
        {
            get { return "Import.AmirsImport"; }
        }
        private void InitLog(string fileName)
        {
            LogManager = Container.Resolve<ILogImportManager>();
            if (LogManager == null)
            {
                throw new Exception("Не найдена реализация интерфейса ILogImportManager");
            }
            LogImport.ImportKey = this.Name;
            LogManager.FileNameWithoutExtention = fileName;
            LogManager.UploadDate = DateTime.Now;
        }
        #endregion
        #region Services

        public ISessionProvider SessionProvider { get; private set; }
        #endregion
        //Таймер для замера локальных операций, чтобы не создавать объект каждый раз
        private Stopwatch local_stopwatch;
        //Таймер выполнения импорта
        private Stopwatch global_stopwatch;
        public override ImportResult Import(BaseParams baseParams)
        {
           
        

            var entityToSourcesMap = Container.Resolve<IDomainService<AppealCitsSource>>();
            var entityToSubjDomain = Container.Resolve<IDomainService<AppealCitsStatSubject>>();
            var entityDomain = Container.Resolve<IDomainService<AppealCits>>();

            // Сохранение сущностей
            IDataTransaction transaction = null;
         

            var statusImport = LogImport.CountError > 0
                ? StatusImport.CompletedWithError
                : LogImport.CountWarning > 0
                    ? StatusImport.CompletedWithWarning
                    : StatusImport.CompletedWithoutError;

      

            return new ImportResult(
                statusImport,
                string.Format("Импортировано {0} записей", LogImport.CountAddedRows),
                string.Empty,
                LogImportManager.LogFileId);
        }

    }
}
