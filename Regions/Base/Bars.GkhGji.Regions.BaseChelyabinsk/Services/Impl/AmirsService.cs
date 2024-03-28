namespace Bars.GkhGji.Regions.BaseChelyabinsk.Services.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Services.ServiceContracts;
    using Bars.GkhGji.Services.DataContracts;
    using Castle.Windsor;
    using Bars.B4;
    using Entities.Protocol197;
    using Bars.Gkh.Import;
    using System.ServiceModel;
    using System.IO;

    /// <summary>
    /// Сервис импорта из АМИРС
    /// </summary>
    public partial class AmirsService : IAmirsService
    {
        public IDomainService<Protocol> ProtocolDomain { get; set; }

        public IDomainService<Protocol197> Protocol197Domain { get; set; }

        protected IWindsorContainer Container { get; }

        public ILogImport LogImport { get; set; }
        public ILogImportManager LogManager { get; set; }

        public string Name
        {
            get { return "Импорт постановлений из АМИРС (сервис)"; }
        }

        /// <summary>
        /// Импорт сведений об обращении граждан
        /// </summary>
        /// <returns></returns>
        public AmirsResult[] ImportAmirs(AmirsData[] records, string token)
        {
            string xmlString = OperationContext.Current.RequestContext.RequestMessage.ToString();
            InitLog($"Импорт постановлений {DateTime.Now.ToString("yyyy.MM.dd hh-mm-ss")}");
            var result = new LinkedList<AmirsResult>();
            if (!records?.Any() ?? true)
            {
                LogImport.Error("Постановления не добавлены", "Прислан пустой запрос");
                result.AddLast(
                    new AmirsResult
                    {
                        IsUploaded = false,
                        AdditionalInformation = "Прислан пустой запрос"
                    });
            }
            else
            {

                var validationResult = this.ValidateAmirs(records);

                //обрабатываем прошедшие валидацию
                foreach (var record in validationResult.Where(x => x.Value.Success))
                {
                    using (var tr = this.Container.Resolve<IDataTransaction>())
                    {
                        using (this.Container.Using(tr))
                        {
                            var replaceResultOnRollback = false;
                            try
                            {
                                var savingResult = this.SaveAmirs(record.Key);

                                if (savingResult.Success)
                                {
                                    result.AddLast(
                                        new AmirsResult
                                        {
                                            NumDate = $"{record.Key.resolution_num} от {record.Key.resolution_date.ToShortDateString()}",
                                            IsUploaded = true
                                        });
                                    replaceResultOnRollback = true;
                                    tr.Commit();
                                    LogImport.CountAddedRows += 1;
                                }
                                else
                                {
                                    result.AddLast(
                                        new AmirsResult
                                        {
                                            NumDate = $"{record.Key.resolution_num} от {record.Key.resolution_date.ToShortDateString()}",
                                            IsUploaded = false,
                                            AdditionalInformation = savingResult.Message
                                        });

                                    tr.Rollback();
                                    LogImport.Error("Постановление не добавлено", $"Постановление {record.Key.resolution_num} от {record.Key.resolution_date.ToShortDateString()}: {savingResult.Message}");
                                }
                            }
                            catch (Exception ex)
                            {
                                if (replaceResultOnRollback)
                                {
                                    result.RemoveLast();
                                    result.AddLast(
                                        new AmirsResult
                                        {
                                            NumDate = $"{record.Key.resolution_num} от {record.Key.resolution_date.ToShortDateString()}",
                                            IsUploaded = false,
                                            AdditionalInformation = $"Ошибка во время сохранения транзакции: {(ex.InnerException ?? ex).Message}"
                                        });
                                }
                                tr.Rollback();
                                LogImport.Error("Постановление не добавлено", $"Постановление {record.Key.resolution_num} от {record.Key.resolution_date.ToShortDateString()}: ошибка во время сохранения транзакции: {(ex.InnerException ?? ex).Message}");
                            }
                        }
                    }
                }

                //обрабатываем НЕ прошедшие валидацию
                foreach (var record in validationResult.Where(x => !x.Value.Success))
                {
                    result.AddLast(
                        new AmirsResult
                        {
                            NumDate = $"{record.Key.resolution_num} от {record.Key.resolution_date.ToShortDateString()}",
                            IsUploaded = false,
                            AdditionalInformation = record.Value.Message
                        });
                    LogImport.Error("Постановление не добавлено", $"Постановление {record.Key.resolution_num} от {record.Key.resolution_date.ToShortDateString()}: {record.Value.Message}");
                }
            }
            var fileStream = new MemoryStream();
            var writer = new StreamWriter(fileStream);
            writer.Write(xmlString);
            writer.Flush();
            LogManager.Add(fileStream, $"Запрос из АМИРС от {DateTime.Now.ToString("yyyy.MM.dd hh-mm-ss")}.xml", this.LogImport);
            LogManager.Save();
            return result.ToArray();
        }

        /// <summary>
        /// Сервис импорта из АМИРС
        /// </summary>
        /// <param name="container"></param>
        public AmirsService(IWindsorContainer container)
        {
            this.Container = container;
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
    }
}
