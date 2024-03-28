namespace Bars.GkhGji.Regions.Voronezh.Import
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.States;
    using Bars.Gkh.Entities;
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
    public sealed class TarifImport : GkhImportBase
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
            get { return "TarifImport"; }
        }

        /// <summary>
        /// Наименование импорта
        /// </summary>
        public override string Name
        {
            get { return "Импорт тарифов и нормативов"; }
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
            get { return "Import.Tarif.View"; }
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

        /// <summary>
        /// Импорт данных
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        public override ImportResult Import(BaseParams baseParams)
        {
            var tarifs = new List<TarifNormative>();
            var file = baseParams.Files["FileImport"];

            var MunicipalityDomain = Container.Resolve<IDomainService<Municipality>>();
            var TarifNormativeDomain = Container.Resolve<IDomainService<TarifNormative>>();
            var CategoryCSMKDDomain = Container.Resolve<IDomainService<CategoryCSMKD>>();

            // Парсинг сущностей
            using (var excel = Container.Resolve<IGkhExcelProvider>("ExcelEngineProvider"))
            {
                if (excel == null)
                {
                    throw new Exception("Не найдена реализация интерфейса IGkhExcelProvider");
                }

                excel.UseVersionXlsx();           

                using(var xlsMemoryStream = new MemoryStream(file.Data))
                {
                    excel.Open(xlsMemoryStream);
                    var data = excel.GetRows(0, 0);
                    var header = data[0].Select(x => x.Value.ToLower()).ToArray();
                    var dict = new Dictionary<string, string>();
                    for(var i = 1; i < data.Count(); i++)
                    {
                        var row = data[i];
                        dict.Clear();
                        for (var j = 0; j < row.Length; j++)
                        {
                            dict.Add(header[j],row[j].Value);
                        }

                        if(!dict.ContainsKey("name"))
                        {
                            LogImport.Error(
                                "Тариф/норматив не добавлен",
                                string.Format("Для записи в строке {0} не найдено наименование элемента", i + 1));
                            continue;
                        }
                        if (!dict.ContainsKey("code"))
                        {
                            LogImport.Error(
                                "Тариф/норматив не добавлен",
                                string.Format("Для записи в строке {0} не найден код элемента", i + 1));
                            continue;
                        }
                        if (!dict.ContainsKey("value"))
                        {
                            LogImport.Error(
                                "Тариф/норматив не добавлен",
                                string.Format("Для записи в строке {0} не найдено значение элемента", i + 1));
                            continue;
                        }

                        Municipality municipality = null;
                        if(dict.ContainsKey("municipality") && !string.IsNullOrWhiteSpace(dict["municipality"]))
                        {
                            var muname = dict["municipality"].Trim();
                            municipality = MunicipalityDomain.GetAll().FirstOrDefault(x => x.Name == muname);
                        }
                        DateTime dateFrom = DateTime.Now;
                        if (dict.ContainsKey("datefrom") && !string.IsNullOrWhiteSpace(dict["datefrom"]))
                        {
                            try
                            {
                                dateFrom = Convert.ToDateTime(dict["datefrom"].Trim());                              
                            }
                            catch
                            {
                                
                            }
                        }
                        DateTime? dateTo = null;
                        if (dict.ContainsKey("dateto") && !string.IsNullOrWhiteSpace(dict["dateto"]))
                        {
                            try
                            {
                                dateTo = Convert.ToDateTime(dict["dateto"].Trim());
                            }
                            catch
                            {

                            }
                        }
                        if (dict.ContainsKey("code") && string.IsNullOrWhiteSpace(dict["code"]))
                        {
                            continue;
                        }
                        if (dict.ContainsKey("code") && string.IsNullOrWhiteSpace(dict["name"]))
                        {
                            continue;
                        }
                        decimal value = 0;
                        if (dict.ContainsKey("value") && !string.IsNullOrWhiteSpace(dict["value"]))
                        {
                            value = Convert.ToDecimal(dict["value"].Trim());
                        }
                        CategoryCSMKD category = null;
                        if (dict.ContainsKey("categorycsmkd") && !string.IsNullOrWhiteSpace(dict["categorycsmkd"]))
                        {
                            var categorycsmkd = dict["categorycsmkd"].Trim();
                            category = CategoryCSMKDDomain.GetAll().FirstOrDefault(x => x.Code == categorycsmkd);

                        }


                        var entity = new TarifNormative
                        {
                            DateFrom = dateFrom,
                            DateTo =dateTo,
                            Value = value,
                            CategoryCSMKD = category,
                            Code = dict["code"].Trim(),
                            Municipality = municipality,
                            Name = dict["name"].Trim(),
                            UnitMeasure = dict["unitmeasure"].Trim(),
                        };
                        tarifs.Add(entity);



                    }
                }
            }


            // Сохранение сущностей
            IDataTransaction transaction = null;
            try
            {
                var index = 0;
                const int PackSize = 1000;
                foreach(var tarif in tarifs)
                {
                    if(transaction == null)
                    {
                        transaction = Container.Resolve<IDataTransaction>();
                    }

                    TarifNormativeDomain.Save(tarif);


                    index++;
                    if(index % PackSize == 0)
                    {
                        transaction.Commit();
                        transaction = null;
                    }

                    LogImport.CountAddedRows++;
                    LogImport.Info("Тариф добавлен", string.Format("Номер \"{0}\"", tarif.Name));
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

                Container.Release(TarifNormativeDomain);
                Container.Release(CategoryCSMKDDomain);
                Container.Release(MunicipalityDomain);

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