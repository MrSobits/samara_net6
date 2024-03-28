namespace Bars.Gkh.Overhaul.Nso.DomainService.Impl
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
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
    using Bars.Gkh.Overhaul.Nso.Entities;
    using Bars.Gkh.Overhaul.Nso.Reports;
    using Castle.Windsor;
    using NHibernate;

    using Ionic.Zip;
    using Ionic.Zlib;

    using FileInfo = Bars.B4.Modules.FileStorage.FileInfo;

    public class PublishedProgramService: IPublishProgramService
    {
        public IWindsorContainer Container { get; set; }

        public IFileManager FileManager { get; set; }

        public IDomainService<FileInfo> FileInfoDomain { get; set; }

        public IDomainService<VersionRecordStage2> Stage2Domain { get; set; }

        public IDomainService<PublishedProgram> PublishedPrgDomain { get; set; }

        public IDomainService<PublishedProgramRecord> PublishedPrgRecDomain { get; set; }

        public IDomainService<ProgramVersion> ProgramVersionDomain { get; set; }

        public IDomainService<State> StateDomain { get; set; }

        public ISessionProvider SessionDomain { get; set; }

        public IQueryable<PublishedProgramRecord> GetPublishedProgramRecords(PublishedProgram program)
        {
            return this.PublishedPrgRecDomain.GetAll()
                                     .Where(x => x.PublishedProgram.Id == program.Id)
                                     .OrderBy(x => x.IndexNumber)
                                     .AsQueryable();
        }

        public IDataResult GetPublishedProgram(BaseParams baseParams)
        {
            var version = this.ProgramVersionDomain.GetAll().FirstOrDefault(x => x.IsMain);

            if (version == null)
            {
                return new BaseDataResult(false, "Не задана основная версия");
            }

            var publish = this.PublishedPrgDomain.GetAll().FirstOrDefault(x => x.ProgramVersion.Id == version.Id);

            if (publish == null)
            {
                return new BaseDataResult(false, "Для основной версии не существует опубликованной программы");
            }

            return new BaseDataResult(publish);
        }

        public IDataResult GetValidationForCreatePublishProgram(BaseParams baseParams)
        {
            var version = this.ProgramVersionDomain.GetAll().FirstOrDefault(x => x.IsMain);

            if (version == null)
            {
                return new BaseDataResult(false, "Не задана основная версия");
            }

            var publish = this.PublishedPrgDomain.GetAll().FirstOrDefault(x => x.ProgramVersion.Id == version.Id);

            if (publish != null && publish.State.FinalState)
            {
                return new BaseDataResult(false, "Опубликованная программа уже утверждена");
            }

            if (publish != null && publish.EcpSigned)
            {
                return new BaseDataResult(true, "Опубликованная программа уже подписана ЭЦП, в случае продолжения, существующие данные будут перезаписаны и необходимо будет заново подписывать ЭЦП. Продолжить?");
            }

            return new BaseDataResult(true, "Вы уверены, что хотите опубликовать программу?");
        }

        public IDataResult GetValidationForSignEcp(BaseParams baseParams)
        {
            var version = this.ProgramVersionDomain.GetAll().FirstOrDefault(x => x.IsMain);

            if (version == null)
            {
                return new BaseDataResult(false, "Не задана основная версия");
            }

            var publish = this.PublishedPrgDomain.GetAll().FirstOrDefault(x => x.ProgramVersion.Id == version.Id);

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
                return new BaseDataResult(true, message: "Опубликованная программа уже подписана ЭЦП, в случае продолжения, существующая подпись будет перезаписана новой подписью. Продолжить подписание?");
            }

            return new BaseDataResult(true, message: "Вы уверены, что хотите подписать опубликованную программу?");
        }

        public IDataResult GetDataToSignEcp(BaseParams baseParams)
        {
            var version = this.ProgramVersionDomain.GetAll().FirstOrDefault(x => x.IsMain);

            if (version == null)
            {
                return new BaseDataResult(false, "Не задана основная версия");
            }

            // елси нету данных то создаем их сами (только для Тестирования )
            if(!this.PublishedPrgRecDomain.GetAll().Any())
                this.CreateTestRecords(version);

            var publish = this.PublishedPrgDomain.GetAll().FirstOrDefault(x => x.ProgramVersion.Id == version.Id);

            if (publish == null)
            {
                return new BaseDataResult(false, "Отсутствует опубликованная программа");
            } 
            
            if (publish.State.FinalState)
            {
                return new BaseDataResult(false, "Опубликованная программа утверждена");
            }

            var dataRecords = this.GetPublishedProgramRecords(publish)
                .Select(x => new SignedRecord
                {
                    Municipality =
                        x.Stage2.Stage3Version.RealityObject.Municipality.Name,
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

            var proxy = new PublishedProgramForSignEcp { records = dataRecords };

            using (MemoryStream xmlStream = this.GetStreamXml(proxy))
            {

                var xmlBytes = xmlStream.ToArray();

                // Поскольку xml получается огромная ~100 МБ
                // То беру Base64 Не от xml а от его md5.Hash
                // в итоге получается примерно такая строка для подписания
                // r0gyQ2ZcmMulETnzijcaCQ==
                using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
                {
                    byte[] bytes = md5.ComputeHash(xmlBytes);
                    strBase64 = Convert.ToBase64String(bytes);
                }

                using (var zipXml = new MemoryStream())
                {
                    using (
                        var fileZip = new ZipFile(Encoding.UTF8)
                        {
                            CompressionLevel = CompressionLevel.Level3,
                            ProvisionalAlternateEncoding =
                                Encoding.GetEncoding("cp866")
                        })
                    {

                        fileZip.AddEntry("xml_for_signed_ecp.xml", xmlBytes);

                        fileZip.Save(zipXml);

                        // теперь уже сохраняем zip файл в котором лежит xml
                        var fileXml = this.FileManager.SaveFile(zipXml,
                            string.Format("EcpSigned_{0}.zip", DateTime.Now.ToShortDateString()));
                        xmlId = fileXml.Id;
                    }
                }
            }

            if (xmlId == 0)
            {
                return new BaseDataResult(false, "Не удалось сформирвоать Xml");
            }

            var p = new PublishedDpkrForSignedReport { Container = this.Container, ExportFormat = StiExportFormat.Pdf };

            using (var msPdf = p.GetGeneratedReport())
            {
                // теперь уже сохраняем zip файл в котором лежит xml
                var filePdf = this.FileManager.SaveFile(msPdf, "EcpSigned.pdf");
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
            result.Data = new { dataToSign = strBase64, xmlId = xmlId, pdfId = pdfId };

            return result;
        }

        public IDataResult SaveSignedResult(BaseParams baseParams)
        {
            var version = this.ProgramVersionDomain.GetAll().FirstOrDefault(x => x.IsMain);

            if (version == null)
            {
                return new BaseDataResult(false, "Не задана основная версия");
            }

            var publish = this.PublishedPrgDomain.GetAll().FirstOrDefault(x => x.ProgramVersion.Id == version.Id);

            if (publish == null)
            {
                return new BaseDataResult(false, "Отсутствует опубликованная программа");
            }
            
            if (publish.State.FinalState)
            {
                return new BaseDataResult(false, "Опубликованная программа утверждена");
            }

            var xmlId = baseParams.Params.GetAs<long>("xmlId");
            var pdfId = baseParams.Params.GetAs<long>("pdfId");
            var sign = baseParams.Params.GetAs<string>("sign");
            var certificate = baseParams.Params.GetAs<string>("certificate");

            if (xmlId == 0 || string.IsNullOrEmpty(sign) || string.IsNullOrEmpty(certificate))
            {
                return new BaseDataResult(false, "Отсутствует одна из составляющих подписи");
            }

            publish.EcpSigned = true;
            publish.FileXml = new FileInfo { Id = xmlId };
            publish.FilePdf = pdfId >0 ? new FileInfo { Id = pdfId } : null;
            publish.FileSign = this.FileManager.SaveFile("signature", "sig", Encoding.UTF8.GetBytes(sign));
            publish.SignDate = DateTime.Now;
            publish.FileCertificate = this.FileManager.SaveFile("certificate", "cer",
                    Encoding.UTF8.GetBytes("-----BEGIN CERTIFICATE-----" + certificate + "-----END CERTIFICATE-----"));

            this.PublishedPrgDomain.Update(publish);

            return new BaseDataResult(true, message: "Программа подписана успешно");
        }


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
                return new BaseDataResult(false, "Программа не может быть удалена, т.к. ее данные подписаны.");
            }

            if (program.State.FinalState)
            {
                return new BaseDataResult(false, "Программа не может быть удалена, т.к. у нее конечный статус.");
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

        private void CreateTestRecords(ProgramVersion version)
        {
            var session = this.SessionDomain.OpenStatelessSession();

            // Проверяем Существует ли нужный статус и если нет то создаем новый
            var firstState = this.StateDomain.GetAll().FirstOrDefault(x => x.TypeId == "ovrhl_published_program" && x.StartState);

            if (firstState == null)
            {
                firstState = new State
                {
                    Name = "Черновик",
                    Code = "Черновик",
                    StartState = true,
                    TypeId = "ovrhl_published_program"
                };

                this.StateDomain.Save(firstState);
            }

            PublishedProgram publish = this.PublishedPrgDomain.GetAll().FirstOrDefault(x => x.ProgramVersion.Id == version.Id);

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
            var data = this.Stage2Domain.GetAll()
                    .Where(x => x.Stage3Version.ProgramVersion.Id == version.Id)
                    .Select(x => new
                    {
                        st2Id = x.Id,
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
                if (i ==100)
                    break;

                var newRec = new PublishedProgramRecord
                {
                    PublishedProgram = publish,
                    Stage2 = new VersionRecordStage2 { Id = rec.st2Id },
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

            try
            {
                using (var transaction = session.BeginTransaction())
                {
                    try
                    {

                        if (publish.Id <= 0)
                        {
                            session.Insert(publish);
                        }

                        listRecordsToSave.ForEach(x => session.Insert(x));

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
                GC.Collect();
                GC.WaitForPendingFinalizers();
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

    public class PublishedProgramForSignEcp
    {
        public SignedRecord[] records { get; set; }
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
