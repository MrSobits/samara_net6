namespace Bars.Gkh.Overhaul.Hmao.DomainService.Impl
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;
    using System.Xml;
    using System.Xml.Serialization;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Modules.Analytics.Reports.Enums;
    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Modules.States;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Overhaul.Hmao.Entities;
    using Bars.Gkh.Overhaul.Hmao.Reports;

    using Castle.Windsor;

    using Ionic.Zip;
    using Ionic.Zlib;

    using NHibernate;


    using FileInfo = Bars.B4.Modules.FileStorage.FileInfo;

    /// <summary>
    /// Сервис для работы с Опубликованной программой
    /// </summary>
    public class PublishedProgramService : IPublishProgramService
    {
        /// <summary>
        /// IoC контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Получить записи опубликованной программы
        /// </summary>
        /// <param name="program">Программа</param>
        /// <returns>Результат выполнения запроса</returns>
        public IQueryable<PublishedProgramRecord> GetPublishedProgramRecords(PublishedProgram program)
        {
            var service = this.Container.Resolve<IDomainService<PublishedProgramRecord>>();
            try
            {
                return service.GetAll()
                    .Where(x => x.PublishedProgram.Id == program.Id)
                    .OrderBy(x => x.IndexNumber)
                    .AsQueryable();
            }
            finally
            {
                this.Container.Release(service);
            }
        }

        /// <summary>
        /// Удалить опубликованную программу
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns>Результат выполнения запроса</returns>
        public IDataResult DeletePublishedProgram(BaseParams baseParams)
        {
            var publishDomain = this.Container.Resolve<IDomainService<PublishedProgram>>();

            var programId = baseParams.Params.GetAsId("programId");

            PublishedProgram program;

            using (this.Container.Using(publishDomain))
            {
                program = publishDomain.Get(programId);
            }

            if (program == null)
            {
                return BaseDataResult.Error("Программа не найдена");
            }

            if (program.EcpSigned)
            {
                return BaseDataResult.Error("Программа не может быть удалена, т.к. ее данные подписаны");
            }

            if (program.State.FinalState)
            {
                return BaseDataResult.Error("Программа не может быть удалена, т.к. у нее конечный статус");
            }

            using (var provider = this.Container.Resolve<ISessionProvider>())
            {
                var session = provider.OpenStatelessSession();
                using (var transaction = session.BeginTransaction())
                {
                    session.CreateSQLQuery($"delete from OVRHL_PUBLISH_PRG_REC where PUBLISH_PRG_ID = {programId};")
                        .ExecuteUpdate();

                    session.CreateSQLQuery($"delete from OVRHL_PUBLISH_PRG where id = {programId};")
                        .ExecuteUpdate();

                    transaction.Commit();

                    return new BaseDataResult();
                }
            }
        }

        /// <summary>
        /// Получить опубликованную программу
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns>Результат выполнения запроса</returns>
        public IDataResult GetPublishedProgram(BaseParams baseParams)
        {
            var programVersionDomain = this.Container.Resolve<IDomainService<ProgramVersion>>();
            var publishedPrgDomain = this.Container.Resolve<IDomainService<PublishedProgram>>();

            try
            {
                var muId = baseParams.Params.GetAs<long>("muId");

                var version = programVersionDomain.GetAll().FirstOrDefault(x => x.IsMain && x.Municipality.Id == muId);

                if (version == null)
                {
                    return new BaseDataResult(false, "Для выбранного муниципального образования не задана основная версия");
                }

                var publish = publishedPrgDomain.GetAll().FirstOrDefault(x => x.ProgramVersion.Id == version.Id);

                if (publish == null)
                {
                    return new BaseDataResult(false, "Для основной версии не существует опубликованной программы");
                }

                return new BaseDataResult(publish);
            }
            finally
            {
                this.Container.Release(programVersionDomain);
                this.Container.Release(publishedPrgDomain);
            }
        }

        /// <summary>
        /// Валидация создания опубликованной программы
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns>Результат выполнения запроса</returns>
        public IDataResult GetValidationForCreatePublishProgram(BaseParams baseParams)
        {
            var programVersionDomain = this.Container.Resolve<IDomainService<ProgramVersion>>();
            var publishedPrgDomain = this.Container.Resolve<IDomainService<PublishedProgram>>();

            try
            {
                var moId = baseParams.Params.GetAs<long>("mo_id");

                if (moId <= 0)
                {
                    return new BaseDataResult(false, "Не удалось получить муниципальное образование");
                }

                var version = programVersionDomain.GetAll().FirstOrDefault(x => x.IsMain && x.Municipality.Id == moId);

                if (version == null)
                {
                    return new BaseDataResult(false, "Не задана основная версия");
                }

                var publish = publishedPrgDomain.GetAll().FirstOrDefault(x => x.ProgramVersion.Id == version.Id);

                if (publish != null && publish.State.FinalState)
                {
                    return new BaseDataResult(false, "Опубликованная программа уже утверждена");
                }

                if (publish != null && publish.EcpSigned)
                {
                    return new BaseDataResult(
                        true,
                        "Опубликованная программа уже подписана ЭЦП, в случае продолжения, существующие данные будут перезаписаны и необходимо будет заново подписывать ЭЦП. Продолжить?");
                }

                return new BaseDataResult(true, "Вы уверены, что хотите опубликовать программу?");
            }
            finally
            {
                this.Container.Release(programVersionDomain);
                this.Container.Release(publishedPrgDomain);
            }
        }

        /// <summary>
        /// Валидация ЭЦП
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns>Результат выполнения запроса</returns>
        public IDataResult GetValidationForSignEcp(BaseParams baseParams)
        {
            var programVersionDomain = this.Container.Resolve<IDomainService<ProgramVersion>>();
            var publishedPrgDomain = this.Container.Resolve<IDomainService<PublishedProgram>>();

            try
            {
                var muId = baseParams.Params.GetAs<long>("muId");

                var version = programVersionDomain.GetAll().FirstOrDefault(x => x.IsMain && x.Municipality.Id == muId);

                if (version == null)
                {
                    return new BaseDataResult(false, "Не задана основная версия");
                }

                var publish = publishedPrgDomain.GetAll().FirstOrDefault(x => x.ProgramVersion.Id == version.Id);

                if (publish == null)
                {
                    return new BaseDataResult(false, "Отсутствует опубликованная программа");
                }

                if (publish.State.FinalState)
                {
                    return new BaseDataResult(false, "Опубликованная программа утверждена");
                }

                if (publish.EcpSigned)
                {
                    return new BaseDataResult(
                        true,
                        "Опубликованная программа уже подписана ЭЦП, в случае продолжения, существующая подпись будет перезаписана новой подписью. Продолжить подписание?");
                }

                return new BaseDataResult(true, "Вы уверены, что хотите подписать опубликованную программу?");
            }
            finally
            {
                this.Container.Release(programVersionDomain);
                this.Container.Release(publishedPrgDomain);
            }
        }

        /// <summary>
        /// Получить данные для ЭЦП
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns>Результат выполнения запроса</returns>
        public IDataResult GetDataToSignEcp(BaseParams baseParams)
        {
            var programVersionDomain = this.Container.Resolve<IDomainService<ProgramVersion>>();
            var publishedPrgDomain = this.Container.Resolve<IDomainService<PublishedProgram>>();
            var publishedPrgRecDomain = this.Container.Resolve<IDomainService<PublishedProgramRecord>>();
            var fileManager = this.Container.Resolve<IFileManager>();

            try
            {
                var muId = baseParams.Params.GetAs<long>("muId");

                var version = programVersionDomain.GetAll().FirstOrDefault(x => x.IsMain && x.Municipality.Id == muId);

                if (version == null)
                {
                    return new BaseDataResult(false, "Не задана основная версия");
                }

                // елси нету данных то создаем их сами (только для Тестирования )
                if (!publishedPrgRecDomain.GetAll().Any())
                {
                    this.CreateTestRecords(version);
                }

                var publish = publishedPrgDomain.GetAll().FirstOrDefault(x => x.ProgramVersion.Id == version.Id);

                if (publish == null)
                {
                    return new BaseDataResult(false, "Отсутствует опубликованная программа");
                }

                if (publish.State.FinalState)
                {
                    return new BaseDataResult(false, "Опубликованная программа утверждена");
                }

                var dataRecords = this.GetPublishedProgramRecords(publish)
                    .Select(
                        x => new SignedRecord
                        {
                            Municipality = x.RealityObject.Municipality.Name,
                            PublishedYear = x.PublishedYear,
                            IndexNumber = x.IndexNumber,
                            Locality = x.Locality,
                            Street = x.Street,
                            House = x.House,
                            Housing = x.Housing,
                            Address = x.Address,
                            CommonEstateobject = x.CommonEstateobject,
                            CommissioningYear = x.CommissioningYear,
                            LastOverhaulYear = x.LastOverhaulYear,
                            Wear = x.Wear,
                            Sum = x.Sum
                        })
                    .ToArray();

                // Тут будет Base 64 от xml которая формируется ниже
                var strBase64 = string.Empty;

                var xmlId = 0L;
                var pdfId = 0L;

                var proxy = new PublishedProgramForSignEcp() {Records = dataRecords};

                using (MemoryStream xmlStream = this.GetStreamXml(proxy))
                {
                    var xmlBytes = xmlStream.ToArray();

                    // Поскольку xml получается огромная ~100 МБ
                    // То беру Base64 Не от xml а от его md5.Hash
                    // в итоге получается примерно такая строка для подписания
                    // r0gyQ2ZcmMulETnzijcaCQ==
                    using (MD5 md5 = MD5.Create())
                    {
                        byte[] bytes = md5.ComputeHash(xmlBytes);
                        strBase64 = Convert.ToBase64String(bytes);
                    }

                    using (var zipXml = new MemoryStream())
                    {
                        using (var fileZip = new ZipFile(Encoding.UTF8)
                        {
                            CompressionLevel = CompressionLevel.Level3,
                            ProvisionalAlternateEncoding = Encoding.GetEncoding("cp866")
                        })
                        {
                            fileZip.AddEntry("xml_for_signed_ecp.xml", xmlBytes);

                            fileZip.Save(zipXml);

                            // теперь уже сохраняем zip файл в котором лежит xml
                            var fileXml = fileManager.SaveFile(zipXml, $"EcpSigned_{DateTime.Now.ToShortDateString()}.zip");
                            xmlId = fileXml.Id;
                        }
                    }
                }

                if (xmlId == 0)
                {
                    return new BaseDataResult(false, "Не удалось сформировать Xml");
                }

                var p = new PublishedDpkrForSignedReport {Container = this.Container, ExportFormat = StiExportFormat.Pdf};

                using (var msPdf = p.GetGeneratedReport())
                {
                    // теперь уже сохраняем zip файл в котором лежит xml
                    var filePdf = fileManager.SaveFile(msPdf, "EcpSigned.pdf");
                    pdfId = filePdf.Id;
                }

                /*
                // ToDo пока сделал прост ополучение PDF любой для теста, потом над опеределать чтобы именна та PDF получалась сгенерированная
                var filePdf = FileInfoDomain.GetAll().FirstOrDefault(x => x.Extention.ToUpper() == "PDF");
                var pdfId = 0;
                if (filePdf != null)
                {
                    pdfId = filePdf.Id;

                    using (var file = FileManager.GetFile(filePdf))
                    {
                        file.CopyTo(msPdf);
                    }    
                }
                */

                var result = new BaseDataResult();
                result.Data = new {dataToSign = strBase64, xmlId = xmlId, pdfId = pdfId};

                return result;
            }
            finally
            {
                this.Container.Release(programVersionDomain);
                this.Container.Release(publishedPrgDomain);
                this.Container.Release(publishedPrgRecDomain);
                this.Container.Release(fileManager);
            }
        }

        /// <summary>
        /// Сохранить результат подписи
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns>Результат выполнения запроса</returns>
        public IDataResult SaveSignedResult(BaseParams baseParams)
        {
            var programVersionDomain = this.Container.Resolve<IDomainService<ProgramVersion>>();
            var publishedPrgDomain = this.Container.Resolve<IDomainService<PublishedProgram>>();
            var fileManager = this.Container.Resolve<IFileManager>();

            try
            {
                var muId = baseParams.Params.GetAs<long>("muId");

                var version = programVersionDomain.GetAll().FirstOrDefault(x => x.IsMain && x.Municipality.Id == muId);

                if (version == null)
                {
                    return new BaseDataResult(false, "Не задана основная версия");
                }

                var publish = publishedPrgDomain.GetAll().FirstOrDefault(x => x.ProgramVersion.Id == version.Id);

                if (publish == null)
                {
                    return new BaseDataResult(false, "Отсутствует опубликованная программа");
                }

                if (publish.State.FinalState)
                {
                    return new BaseDataResult(false, "Опубликованная программа утверждена");
                }

                var xmlId = baseParams.Params.GetAs<long>("xmlid", 0);
                var pdfId = baseParams.Params.GetAs<long>("pdfid", 0);
                var sign = baseParams.Params.GetAs<string>("sign");
                var certificate = baseParams.Params.GetAs<string>("certificate");

                if (xmlId == 0 || string.IsNullOrEmpty(sign) || string.IsNullOrEmpty(certificate))
                {
                    return new BaseDataResult(false, "Отсутствует одна из составляющих подписи");
                }

                publish.EcpSigned = true;
                publish.FileXml = new FileInfo {Id = xmlId};
                publish.FilePdf = pdfId > 0 ? new FileInfo {Id = pdfId} : null;
                publish.FileSign = fileManager.SaveFile("signature", "sig", Encoding.UTF8.GetBytes(sign));
                publish.SignDate = DateTime.Now;
                publish.FileCertificate = fileManager.SaveFile(
                    "certificate",
                    "cer",
                    Encoding.UTF8.GetBytes("-----BEGIN CERTIFICATE-----" + certificate + "-----END CERTIFICATE-----"));

                publishedPrgDomain.Update(publish);

                return new BaseDataResult(true, message: "Программа подписана успешно");
            }
            finally
            {
                this.Container.Release(programVersionDomain);
                this.Container.Release(publishedPrgDomain);
                this.Container.Release(fileManager);
            }
        }

        private void CreateTestRecords(ProgramVersion version)
        {
            var stateDomain = this.Container.Resolve<IDomainService<State>>();
            var sessionProvider = this.Container.Resolve<ISessionProvider>();
            var publishedPrgDomain = this.Container.Resolve<IDomainService<PublishedProgram>>();
            var stage2Domain = this.Container.Resolve<IDomainService<VersionRecordStage2>>();

            try
            {
                // Проверяем Существует ли нужный статус и если нет то создаем новый
                var firstState = stateDomain.GetAll().FirstOrDefault(x => x.TypeId == "ovrhl_published_program" && x.StartState);

                if (firstState == null)
                {
                    firstState = new State
                    {
                        Name = "Черновик",
                        Code = "Черновик",
                        StartState = true,
                        TypeId = "ovrhl_published_program"
                    };

                    stateDomain.Save(firstState);
                }

                PublishedProgram publish = publishedPrgDomain.GetAll().FirstOrDefault(x => x.ProgramVersion.Id == version.Id);

                if (publish == null)
                {
                    publish = new PublishedProgram
                    {
                        State = firstState,
                        ProgramVersion = version
                    };
                }

                var listRecordsToSave = new List<PublishedProgramRecord>();

                // Получаем записи корректировки и поним создаем опубликованную программу
                var data =
                    stage2Domain.GetAll()
                        .Where(x => x.Stage3Version.ProgramVersion.Id == version.Id)
                        .Select(
                            x => new
                            {
                                st2Id = x.Id,
                                RoId = x.Stage3Version.RealityObject.Id,
                                x.Stage3Version.Year,
                                x.Stage3Version.IndexNumber,
                                Locality = x.Stage3Version.RealityObject.FiasAddress.PlaceName,
                                Street = x.Stage3Version.RealityObject.FiasAddress.StreetName,
                                x.Stage3Version.RealityObject.FiasAddress.House,
                                x.Stage3Version.RealityObject.FiasAddress.Housing,
                                Address = x.Stage3Version.RealityObject.FiasAddress.AddressName,
                                CommonEstateobject = x.CommonEstateObject.Name,
                                CommissioningYear =
                                    x.Stage3Version.RealityObject.BuildYear.HasValue
                                        ? x.Stage3Version.RealityObject.BuildYear.Value
                                        : 0,
                                x.Sum
                            })
                        .ToList();

                int i = 1;
                foreach (var rec in data)
                {
                    if (i == 100)
                    {
                        break;
                    }

                    var newRec = new PublishedProgramRecord
                    {
                        PublishedProgram = publish,
                        Stage2 = new VersionRecordStage2 {Id = rec.st2Id},
                        RealityObject = new RealityObject {Id = rec.RoId},
                        PublishedYear = rec.Year,
                        IndexNumber = rec.IndexNumber,
                        Locality = rec.Locality,
                        Street = rec.Street,
                        House = rec.House,
                        Housing = rec.Housing,
                        Address = rec.Address,
                        CommonEstateobject = rec.CommonEstateobject,
                        CommissioningYear = rec.CommissioningYear,
                        Sum = rec.Sum,
                    };

                    listRecordsToSave.Add(newRec);
                    i++;
                }

                var session = sessionProvider.GetCurrentSession();
                var lastMode = session.FlushMode;

                session.FlushMode = FlushMode.Commit;

                try
                {
                    using (var transaction = session.BeginTransaction())
                    {
                        try
                        {
                            if (publish.Id <= 0)
                            {
                                session.Save(publish);
                            }

                            listRecordsToSave.ForEach(x => session.Save(x));

                            transaction.Commit();
                        }
                        catch (Exception)
                        {
                            transaction.Rollback();
                            throw;
                        }
                    }
                }
                finally
                {
                    session.FlushMode = lastMode;
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                }
            }
            finally
            {
                this.Container.Release(stateDomain);
                this.Container.Release(publishedPrgDomain);
                this.Container.Release(stage2Domain);
            }
        }

        private MemoryStream GetStreamXml(PublishedProgramForSignEcp proxy)
        {
            var serializer = new XmlSerializer(typeof(PublishedProgramForSignEcp));

            var result = new MemoryStream();
            var writer = XmlWriter.Create(result);

            serializer.Serialize(writer, proxy);

            result.Seek(0, SeekOrigin.Begin);

            return result;
        }
    }

    /// <summary>
    /// Опубликованная программа для ЭЦП
    /// </summary>
    public class PublishedProgramForSignEcp
    {
        /// <summary>
        /// Записи
        /// </summary>
        public SignedRecord[] Records { get; set; }
    }

    /// <summary>
    /// в данном классе будут только значимые поля для сериализации в xml и дальнейшее подписание ЭЦП
    /// </summary>
    public class SignedRecord
    {
        /// <summary>
        /// Порядковый номер
        /// </summary>
        public virtual int IndexNumber { get; set; }

        /// <summary>
        /// МО
        /// </summary>
        public virtual string Municipality { get; set; }

        /// <summary>
        /// Населенный пункт
        /// </summary>
        public virtual string Locality { get; set; }

        /// <summary>
        /// Улица
        /// </summary>
        public virtual string Street { get; set; }

        /// <summary>
        /// Дом
        /// </summary>
        public virtual string House { get; set; }

        /// <summary>
        /// Корпус
        /// </summary>
        public virtual string Housing { get; set; }

        /// <summary>
        /// Адрес
        /// </summary>
        public virtual string Address { get; set; }

        /// <summary>
        /// Год ввода в эксплуатацию
        /// </summary>
        public virtual int CommissioningYear { get; set; }

        /// <summary>
        /// Объект общего имущества
        /// </summary>
        public virtual string CommonEstateobject { get; set; }

        /// <summary>
        /// Стоимость
        /// </summary>
        public virtual decimal Sum { get; set; }

        /// <summary>
        /// Износ
        /// </summary>
        public virtual decimal Wear { get; set; }

        /// <summary>
        /// Дата последнего капитального ремонта
        /// </summary>
        public virtual int LastOverhaulYear { get; set; }

        /// <summary>
        /// Плановый год проведения капитального ремонта
        /// </summary>
        public virtual int PublishedYear { get; set; }
    }
}