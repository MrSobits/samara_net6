namespace Bars.GkhGji.Import.Appeal
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Modules.States;
    using Bars.Gkh.Enums.Import;
    using Bars.Gkh.Import;
    using Bars.GkhExcel;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;

    using Castle.Windsor;
    using Gkh.Import.Impl;
    using NHibernate.Dialect.Function;

    /// <summary>
    ///     Импорт обращений в жилищную инспекцию.
    /// </summary>
    public sealed class ImportAppeal : GkhImportBase
    {
        public static string Id = MethodBase.GetCurrentMethod().DeclaringType.FullName;

        /// <summary>
        /// Ключ импорта
        /// </summary>
        public override string Key
        {
            get { return Id; }
        }

        /// <summary>
        /// Код группировки импорта (например группировка в меню)
        /// </summary>
        public override string CodeImport
        {
            get { return "ImportAppeal"; }
        }

        /// <summary>
        /// Наименование импорта
        /// </summary>
        public override string Name
        {
            get { return "Импорт обращений"; }
        }

        /// <summary>
        /// Разрешенные расширения файлов
        /// </summary>
        public override string PossibleFileExtensions
        {
            get { return "xlsx"; }
        }

        /// <summary>
        /// Права
        /// </summary>
        public override string PermissionName
        {
            get { return "Import.Appeal.View"; }
        }

        /// <summary>
        ///     IoC контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <summary>
        ///     Логгер импорта
        /// </summary>
        public ILogImport LogImport { get; set; }

        /// <summary>
        ///     Менеджер логгеров импорта
        /// </summary>
        public ILogImportManager LogImportManager { get; set; }

        private sealed class ImportAppealInfo
        {
            public AppealCits AppealCits { get; set; }

            public RevenueSourceGji RevenueSourceGji { get; set; }

            public StatSubjectGji[] Subjects { get; set; }
        }

        private StatSubjectGji ExtractSubject(string name, Dictionary<string, string> dict, StatSubjectGji[] subjects, int index)
        {
            if(!dict.ContainsKey(name) ||
               string.IsNullOrWhiteSpace(dict[name]))
            {
                return null;
            }

            var value = dict[name].Trim();
            var subject = subjects.FirstOrDefault(item => item.Name == value);
            if (subject == null)
            {
                LogImport.Warn(
                    string.Empty,
                    string.Format(
                        "Тематика не найдена для обращения {0} в строке {1}\nНазвание тематики: {2}",
                        dict[string.Empty],
                        index + 1,
                        value));
            }

            return subject;
        }

        /// <summary>
        /// Импорт данных
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        public override ImportResult Import(BaseParams baseParams)
        {
            var appeals = new List<ImportAppealInfo>();
            var file = baseParams.Files["FileImport"];

            var statementDomain = Container.Resolve<IDomainService<KindStatementGji>>();
            var revenueSourceDomain = Container.Resolve<IDomainService<RevenueSourceGji>>();
            var subjectDomain = Container.Resolve<IDomainService<StatSubjectGji>>();

            // Парсинг сущностей
            var excel = this.Container.Resolve<IGkhExcelProvider>("ExcelEngineProvider");
            using (this.Container.Using(excel))
            {
                if (excel == null)
                {
                    throw new Exception("Не найдена реализация интерфейса IGkhExcelProvider");
                }

                excel.UseVersionXlsx();

                var kindStatements = statementDomain.GetAll().ToArray();
                var revenueSources = revenueSourceDomain.GetAll().ToArray();
                var subjects = subjectDomain.GetAll().ToArray();

                using(var xlsMemoryStream = new MemoryStream(file.Data))
                {
                    excel.Open(xlsMemoryStream);
                    var data = excel.GetRows(0, 0);
                    var header = data[1].Select(x => x.Value.ToLower()).ToArray();
                    var dict = new Dictionary<string, string>();
                    for(var i = 2; i < data.Count(); i++)
                    {
                        var row = data[i];
                        dict.Clear();
                        for (var j = 0; j < row.Length; j++)
                        {
                            dict.Add(header[j],row[j].Value);
                        }

                        if(!dict.ContainsKey("execdate"))
                        {
                            LogImport.Error(
                                "Обращение не добавлено",
                                string.Format("Для обращения в строке {0} не найдена дата обращения", i + 1));
                            continue;
                        }

                        if(!dict.ContainsKey("fullname"))
                        {
                            LogImport.Error(
                                "Обращение не добавлено",
                                string.Format("Для обращения в строке {0} не найдена имя корреспондента", i + 1));
                            continue;
                        }

                        KindStatementGji kindStatement = null;
                        if(dict.ContainsKey("jalotype_name") && !string.IsNullOrWhiteSpace(dict["jalotype_name"]))
                        {
                            var type = dict["jalotype_name"].Trim();
                            kindStatement = kindStatements.FirstOrDefault(item => item.Name == type);
                            if(kindStatement == null)
                            {
                                LogImport.Warn(string.Empty, string.Format(
                                        "Не найден тип жалобы {0} для обращения {1} в строке {2}",
                                        type,
                                        dict[string.Empty],
                                        i + 1));
                            }
                        }

                        var address = dict.ContainsKey("regions_name")
                            ? dict["regions_name"] + ", "
                            : string.Empty;
                        address += dict.ContainsKey("town") ? dict["town"] + ", " : string.Empty;
                        address += dict.ContainsKey("street") ? dict["street"] + ", " : string.Empty;
                        address += dict.ContainsKey("building") ? dict["building"] : string.Empty;

                        address = address.Trim(new[] {' ', ','});

                        RevenueSourceGji source = null;
                        if (dict.ContainsKey("uporgcode") && !string.IsNullOrWhiteSpace(dict["uporgcode"]))
                        {
                            var value = dict["uporgcode"].Trim();
                            source = revenueSources.FirstOrDefault(item => item.Name == value);
                            if(source == null)
                            {
                                LogImport.Warn(
                                    string.Empty,
                                    string.Format(
                                        "Источник поступления {0} не найден для обращения {1} в строке {2}",
                                        value,
                                        dict[string.Empty],
                                        i + 1));
                            }
                        }

                        var extractedSubjects = new List<StatSubjectGji>
                        {
                            ExtractSubject("themes_themedesc", dict, subjects, i),
                            ExtractSubject("themes_1_themedesc", dict, subjects, i),
                            ExtractSubject("themes_2_themedesc", dict, subjects, i),
                            ExtractSubject("themes_3_themedesc", dict, subjects, i),
                            ExtractSubject("themes_4_themedesc", dict, subjects, i)
                        };

                        var entity = new AppealCits
                        {
                            DateFrom = DateTime.Parse(dict["obrdate"]),
                            NumberGji = dict[string.Empty],
                            Accepting = Accepting.NotSet,
                            CheckTime = DateTime.Parse(dict["execdate"]),
                            Correspondent = dict["fullname"],
                            CorrespondentAddress = address,
                            Description = dict.ContainsKey("obrcont") ? dict["obrcont"] : null,
                            DocumentNumber = dict.ContainsKey(string.Empty) ? dict[string.Empty] : null,
                            FlatNum = dict.ContainsKey("flat") ? dict["flat"] : null,
                            KindStatement = kindStatement,
                            Number = dict.ContainsKey(string.Empty) ? dict[string.Empty] : null,
                            Status = StatusAppealCitizens.New,
                            SuretyDate = dict.ContainsKey("obrdate")
                                    ? DateTime.Parse(dict["obrdate"])
                                    : (DateTime?)null
                        };

                        appeals.Add(new ImportAppealInfo
                        {
                            AppealCits = entity,
                            RevenueSourceGji = source,
                            Subjects = extractedSubjects.Where(item => item != null).ToArray()
                        });
                    }
                }
            }

            var entityToSourcesMap = Container.Resolve<IDomainService<AppealCitsSource>>();
            var entityToSubjDomain = Container.Resolve<IDomainService<AppealCitsStatSubject>>();
            var entityDomain = Container.Resolve<IDomainService<AppealCits>>();

            // Сохранение сущностей
            IDataTransaction transaction = null;
            try
            {
                var index = 0;
                const int PackSize = 1000;
                foreach(var appeal in appeals)
                {
                    if(transaction == null)
                    {
                        transaction = Container.Resolve<IDataTransaction>();
                    }

                    entityDomain.Save(appeal.AppealCits);

                    entityToSourcesMap.Save(
                        new AppealCitsSource
                        {
                            AppealCits = appeal.AppealCits,
                            RevenueSource = appeal.RevenueSourceGji
                        });

                    foreach(var subject in appeal.Subjects)
                    {
                        entityToSubjDomain.Save(
                            new AppealCitsStatSubject
                            {
                                AppealCits = appeal.AppealCits,
                                Subject = subject
                            });
                    }

                    index++;
                    if(index % PackSize == 0)
                    {
                        transaction.Commit();
                        transaction = null;
                    }

                    LogImport.CountAddedRows++;
                    LogImport.Info("Обращение добавлено", string.Format("Номер \"{0}\"", appeal.AppealCits.DocumentNumber));
                }

                if(transaction != null)
                {
                    transaction.Commit();
                    transaction = null;
                }

                //логи
                LogImport.SetFileName(file.FileName);
                LogImport.ImportKey = CodeImport;

                LogImportManager.FileNameWithoutExtention = file.FileName;
                LogImportManager.Add(file, LogImport);
                LogImportManager.Save();
            }
            catch(Exception ex)
            {
                if(transaction != null)
                {
                    transaction.Rollback();
                }

                Container.Release(statementDomain);
                Container.Release(revenueSourceDomain);
                Container.Release(subjectDomain);
                Container.Release(entityToSourcesMap);
                Container.Release(entityToSubjDomain);
                Container.Release(entityDomain);

                throw;
            }

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

        /// <summary>
        /// Первоночальная валидация файла перед импортом
        /// </summary>
        /// <param name="baseParams"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public override bool Validate(BaseParams baseParams, out string message)
        {
            message = null;
            if (!baseParams.Files.ContainsKey("FileImport"))
            {
                message = "Не выбран файл для импорта";
                return false;
            }

            var fileData = baseParams.Files["FileImport"];
            var extention = fileData.Extention;

            var fileExtentions = PossibleFileExtensions.Contains(",")
                ? PossibleFileExtensions.Split(',')
                : new[] { PossibleFileExtensions };
            if (fileExtentions.All(x => x != extention))
            {
                message = string.Format("Необходимо выбрать файл с допустимым расширением: {0}", PossibleFileExtensions);
                return false;
            }

            return true;
        }
    }
}