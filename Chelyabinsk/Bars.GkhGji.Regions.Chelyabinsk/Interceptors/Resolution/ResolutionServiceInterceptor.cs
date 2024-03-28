namespace Bars.GkhGji.Regions.Chelyabinsk.Interceptors
{
    using System.Linq;
    using System;
    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Enums;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Interceptors;
    using Bars.GkhGji.Report;
    using Bars.GkhGji.Enums;
    using Entities;
    using System.Collections.Generic;
    using Bars.B4.DataAccess;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.Protocol197;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Regions.Chelyabinsk.Enums;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.Disposal;

    public class ResolutionServiceInterceptor: ResolutionServiceInterceptor<Resolution>
    {
    }

    public class ResolutionServiceInterceptor<T> : DocumentGjiInterceptor<T>
        where T : Resolution
    {
        public override IDataResult BeforeUpdateAction(IDomainService<T> service, T entity)
        {
            var payService = Container.Resolve<IDomainService<ResolutionPayFine>>();
            var gisGmpService = Container.Resolve<IDomainService<GisGmp>>();
            var documentGjiChildrenService = Container.Resolve<IDomainService<DocumentGjiChildren>>();

            try
            {
                // Заполняем поле "Штраф оплачен (подробно)"
                    var resolutionPayFineSum = payService
                             .GetAll()
                             .Where(x => x.Resolution.Id == entity.Id)
                             .Sum(x => x.Amount)
                             .ToDecimal();
                if (entity.PenaltyAmount.HasValue && entity.Payded50Percent && resolutionPayFineSum >= entity.PenaltyAmount / 2)
                {
                    entity.Paided = YesNoNotSet.Yes;
                    entity.PayStatus = ResolutionPaymentStatus.Paid;
                }
                else if (entity.PenaltyAmount.HasValue && resolutionPayFineSum >= entity.PenaltyAmount)
                {
                    entity.Paided = YesNoNotSet.Yes;
                    entity.PayStatus = ResolutionPaymentStatus.Paid;
                }
                else if (entity.PenaltyAmount.HasValue && resolutionPayFineSum > entity.PenaltyAmount)
                {
                    entity.PayStatus = ResolutionPaymentStatus.OverPaid;
                }
                else if (entity.PenaltyAmount.HasValue && resolutionPayFineSum == entity.PenaltyAmount)
                {
                    entity.PayStatus = ResolutionPaymentStatus.Paid;
                }
                else if (entity.PenaltyAmount.HasValue && resolutionPayFineSum > 0)
                {
                    entity.PayStatus = ResolutionPaymentStatus.PartialPaid;
                }
                else
                {
                    entity.PayStatus = ResolutionPaymentStatus.NotPaid;
                }
                
                // ищем начисление по постановлению
                var gisGmp = gisGmpService.GetAll().FirstOrDefault(x => x.Protocol.Id == entity.Id);
                if (gisGmp == null)
                {
                    // ищем начисление по родитеою постановления - протоколу и т.д.
                    var resParent = documentGjiChildrenService.GetAll().Where(x => x.Children.Id == entity.Id).Select(x => x.Parent).FirstOrDefault();
                    if (resParent != null)
                    {
                        gisGmp = gisGmpService.GetAll().FirstOrDefault(x => x.Protocol.Id == resParent.Id);
                    }
                }
                if (gisGmp != null)
                {
                    gisGmp.DocNumDate = FindDocNumbers(gisGmp);
                    gisGmpService.Update(gisGmp);
                }

                return base.BeforeUpdateAction(service, entity);
            }
            finally
            {
                Container.Release(payService);
            }
        }

        public override IDataResult AfterCreateAction(IDomainService<T> service, T entity)
        {
            var protocolServ = Container.Resolve<IDomainService<Protocol>>();
            var protocolArtLaw = Container.Resolve<IDomainService<ProtocolArticleLaw>>();
            var resProsArtLaw = Container.Resolve<IDomainService<ResolProsArticleLaw>>();
            var resololArtLawDomain = Container.Resolve<IDomainService<ResolutionArtLaw>>();
            // для протоколов ГЖИ
            var doc = Container.Resolve<IDomainService<DocumentGji>>().GetAll()
                                     .Where(x => x.Inspection.Id == entity.Inspection.Id && x.TypeDocumentGji == TypeDocumentGji.Protocol).OrderByDescending(x=> x.Id)
                                     .FirstOrDefault();
            if (doc != null && doc.Id > 0)
            {
                List<ArticleLawGji> listArtLasw = protocolArtLaw.GetAll()
                    .Where(x => x.Protocol.Id == doc.Id)
                    .Select(x => x.ArticleLaw).ToList();

                try
                {
                    if (listArtLasw.Count > 0)
                    {
                        foreach (ArticleLawGji artLawGji in listArtLasw)
                        {
                            ResolutionArtLaw resolArtLaw = new ResolutionArtLaw();
                            resolArtLaw.ObjectCreateDate = DateTime.Now;
                            resolArtLaw.ObjectEditDate = DateTime.Now;
                            resolArtLaw.ObjectVersion = 1;
                            resolArtLaw.Resolution = entity;
                            resolArtLaw.ArticleLawGji = artLawGji;
                            resolArtLaw.DocumentDate = DateTime.Now;
                            resolArtLaw.Description = "Перенесена из протокола";
                            resololArtLawDomain.Save(resolArtLaw);
                        }
                        
                    }
                    else
                    {
                       
                    }
                }
                catch (Exception e)
                {
                    return Failure("Не удалось добавить статьи закона");
                }
            }
            else
            {
               // return Success();
            }

            // для постановлений прокуратуры 
            var docPros = Container.Resolve<IDomainService<ResolPros>>().GetAll()
                                     .Where(x => x.Inspection.Id == entity.Inspection.Id && x.TypeDocumentGji == TypeDocumentGji.ResolutionProsecutor).OrderByDescending(x => x.Id)
                                     .FirstOrDefault();
            if (docPros != null && docPros.Id > 0)
            {
                List<ArticleLawGji> listArtLasw = resProsArtLaw.GetAll()
                    .Where(x => x.ResolPros.Id == docPros.Id)
                    .Select(x => x.ArticleLaw).ToList();

                try
                {
                    if (listArtLasw.Count > 0)
                    {
                        foreach (ArticleLawGji artLawGji in listArtLasw)
                        {
                            ResolutionArtLaw resolArtLaw = new ResolutionArtLaw();
                            resolArtLaw.ObjectCreateDate = DateTime.Now;
                            resolArtLaw.ObjectEditDate = DateTime.Now;
                            resolArtLaw.ObjectVersion = 1;
                            resolArtLaw.Resolution = entity;
                            resolArtLaw.ArticleLawGji = artLawGji;
                            resolArtLaw.DocumentDate = DateTime.Now;
                            resolArtLaw.Description = "Перенесена из постановления прокуратуры";
                            resololArtLawDomain.Save(resolArtLaw);
                        }
                       
                    }
                    else
                    {
                       
                    }
                }
                catch (Exception e)
                {
                    return Failure("Не удалось добавить статьи закона");
                }
            }
            else
            {
                // return Success();
            }

            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            try
            {
                logEntityHistoryService.UpdateLog(entity, TypeEntityLogging.DocumentGji, entity.Id, entity.GetType(), GetPropertyValues(), entity.Id.ToString());
            }
            catch
            {

            }

            return Success();

        }


        public override IDataResult BeforeDeleteAction(IDomainService<T> service, T entity)
        {
            var result = base.BeforeDeleteAction(service, entity);

            if (!result.Success)
            {
                return Failure(result.Message);
            }

            var annexService = this.Container.Resolve<IDomainService<ResolutionAnnex>>();
            var defService = this.Container.Resolve<IDomainService<ResolutionDefinition>>();
            var disputeService = this.Container.Resolve<IDomainService<ResolutionDispute>>();
            var payService = this.Container.Resolve<IDomainService<ResolutionPayFine>>();
            var artLawService = this.Container.Resolve<IDomainService<ResolutionArtLaw>>();

            try
            {
                annexService.GetAll().Where(x => x.Resolution.Id == entity.Id)
                    .Select(x => x.Id).ForEach(x => annexService.Delete(x));

                defService.GetAll().Where(x => x.Resolution.Id == entity.Id)
                    .Select(x => x.Id).ForEach(x => defService.Delete(x));

                disputeService.GetAll().Where(x => x.Resolution.Id == entity.Id)
                    .Select(x => x.Id).ForEach(x => disputeService.Delete(x));

                payService.GetAll().Where(x => x.Resolution.Id == entity.Id)
                    .Select(x => x.Id).ForEach(x => payService.Delete(x));

                artLawService.GetAll().Where(x => x.Resolution.Id == entity.Id)
                   .Select(x => x.Id).ForEach(x => artLawService.Delete(x));

                return Success();
            }
            finally
            {
                Container.Release(annexService);
                Container.Release(defService);
                Container.Release(disputeService);
                Container.Release(payService);
                Container.Release(artLawService);
            }
        }

        public override IDataResult AfterUpdateAction(IDomainService<T> service, T entity)
        {
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            try
            {
                logEntityHistoryService.UpdateLog(entity, TypeEntityLogging.DocumentGji, entity.Id, entity.GetType(), GetPropertyValues(), entity.DocumentNumber);
            }
            catch
            {

            }
            return this.Success();
        }

        public override IDataResult AfterDeleteAction(IDomainService<T> service, T entity)
        {
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            try
            {
                logEntityHistoryService.UpdateLog(null, TypeEntityLogging.DocumentGji, entity.Id, entity.GetType(), GetPropertyValues(), entity.DocumentNumber);
            }
            catch
            {

            }
            return this.Success();
        }

        private Dictionary<string, string> GetPropertyValues()
        {
            var result = new Dictionary<string, string>
            {
                { "Inspection", "Проверка ГЖИ" },
                { "Stage", "Этап проверки" },
                { "TypeDocumentGji", "Тип документа ГЖИ" },
                { "DocumentDate", "Дата документа" },
                { "DocumentNumber", "Номер документа" },
                { "DocumentSubNum", "Дополнительный номер документа (порядковый номер если документов одного типа несколько)" },
                { "State", "Статус" },
                { "Executant", "Тип исполнителя документа" },
                { "Municipality", "Муниципальное образование" },
                { "Contragent", "Контрагент" },
                { "Sanction", "Вид санкции" },
                { "Official", "Должностное лицо" },
                { "Surname", "Фамилия" },
                { "FirstName", "Имя" },
                { "Patronymic", "Отчество" },
                { "Position", "Должность" },
                { "PhysicalPerson", "Физическое лицо" },
                { "PhysicalPersonInfo", "Реквизиты физ.лица" },
                { "PhysicalPersonDocType", "Тип документа физ.лица" },
                { "PhysicalPersonDocumentNumber", "Номер документа физлица" },
                { "PhysicalPersonDocumentSerial", "Серия документа физлица" },
                { "PhysicalPersonIsNotRF", "Не является гражданином РФ" },
                { "DeliveryDate", "Дата вручения" },
                { "TypeInitiativeOrg", "Тип инициативного органа" },
                { "SectorNumber", "Номер участка" },
                { "PenaltyAmount", "Сумма штрафов" },
                { "Paided", " Штраф оплачен" },
                { "PayStatus", "Штраф оплачен (подробно)" },
                { "DateTransferSsp", "Дата передачи в ССП" },
                { "DocumentNumSsp", "Номер документа, передача в ССП" },
                { "Description", "Примечание" },
                { "DocumentTime", "Время составления документа" },
                { "DateWriteOut", "Дата выписки из ЕГРЮЛ" },
                { "BecameLegal", "Вступило в законную силу" },
                { "FineMunicipality", "МО получателя штрафа" }
            };
            return result;
        }

        private string FindDocNumbers(GisGmp x)
        {
            // x.Protocol.DocumentNumber
            var DocumentGjiChildrenRepo = Container.Resolve<IRepository<DocumentGjiChildren>>();
            var DocumentGjiRepo = Container.Resolve<IRepository<DocumentGji>>();
            var ResolutionRepo = Container.Resolve<IRepository<Resolution>>();
            var Protocol197Repo = Container.Resolve<IRepository<Protocol197>>();
            var ResolutionProsecutorRepo = Container.Resolve<IRepository<ResolPros>>();
            var ResolutionRospotrebnadzorRepo = Container.Resolve<IRepository<ResolutionRospotrebnadzor>>();
            var ManOrgLicenseRequestRepo = Container.Resolve<IRepository<ManOrgLicenseRequest>>();
            var LicenseReissuanceRepo = Container.Resolve<IRepository<LicenseReissuance>>();


            string docNums = "";
            if (x.TypeLicenseRequest == TypeLicenseRequest.NotSet && x.Protocol != null)
            {

                var document = DocumentGjiRepo.GetAll()
                    .Where(y => y == x.Protocol)
                    .Select(y => new
                    {
                        y.Id,
                        y.TypeDocumentGji
                    })
                    .FirstOrDefault();
                if (document != null)
                {

                    if (document.TypeDocumentGji == TypeDocumentGji.Protocol ||
                        //document.TypeDocumentGji == TypeDocumentGji.ProtocolGZHI ||
                        document.TypeDocumentGji == TypeDocumentGji.ProtocolMhc ||
                        document.TypeDocumentGji == TypeDocumentGji.ProtocolMvd ||
                        //document.TypeDocumentGji == TypeDocumentGji.ProtocolProsecutor ||
                        document.TypeDocumentGji == TypeDocumentGji.ProtocolRSO ||
                        document.TypeDocumentGji == TypeDocumentGji.ResolutionProsecutor ||
                        document.TypeDocumentGji == TypeDocumentGji.ResolutionRospotrebnadzor)
                    {
                        var docChildrenParent = DocumentGjiChildrenRepo.GetAll()
                            .Where(y => y.Parent.Id == document.Id)
                            .Select(y => new
                            {
                                y.Id,
                                y.Children
                            }).FirstOrDefault();
                        if (docChildrenParent != null)
                        {
                            var resolution = ResolutionRepo.GetAll()
                                .Where(y => y == docChildrenParent.Children)
                                .Select(y => new
                                {
                                    y.DocumentNumber,
                                    y.DocumentDate,
                                    y.DecisionDate,
                                    y.DecisionNumber,
                                    y.ExecuteSSPNumber,
                                    y.DateExecuteSSP
                                }).FirstOrDefault();
                            if (resolution != null)
                            {
                                if ((resolution.DocumentNumber != null) && (resolution.DocumentNumber != ""))
                                {
                                    docNums = "Постановление " + resolution.DocumentNumber + " от " + resolution.DocumentDate.Value.ToShortDateString();
                                }
                                if ((resolution.DecisionNumber != null) && (resolution.DecisionNumber != ""))
                                {
                                    docNums = docNums + ", судебное решение " + resolution.DecisionNumber + " от " + resolution.DecisionDate.Value.ToShortDateString();
                                }
                                if ((resolution.ExecuteSSPNumber != null) && (resolution.ExecuteSSPNumber != ""))
                                {
                                    docNums = docNums + ", ИП " + resolution.ExecuteSSPNumber;// + " от " + resolution.DateExecuteSSP.Value.ToShortDateString();
                                }
                            }

                        }

                    }
                    else if (document.TypeDocumentGji == TypeDocumentGji.Resolution)
                    {
                        var resolution = ResolutionRepo.GetAll()
                            .Where(y => y.Id == document.Id)
                            .Select(y => new
                            {
                                y.DocumentNumber,
                                y.DocumentDate,
                                y.DecisionDate,
                                y.DecisionNumber,
                                y.ExecuteSSPNumber,
                                y.DateExecuteSSP
                            }).FirstOrDefault();
                        if (resolution != null)
                        {
                            if ((resolution.DocumentNumber != null) && (resolution.DocumentNumber != ""))
                            {
                                docNums = "Постановление " + resolution.DocumentNumber + " от " + resolution.DocumentDate.Value.ToShortDateString();
                            }
                            if ((resolution.DecisionNumber != null) && (resolution.DecisionNumber != ""))
                            {
                                docNums = docNums + ", судебное решение " + resolution.DecisionNumber + " от " + resolution.DecisionDate.Value.ToShortDateString();
                            }
                            if ((resolution.ExecuteSSPNumber != null) && (resolution.ExecuteSSPNumber != ""))
                            {
                                docNums = docNums + ", ИП " + resolution.ExecuteSSPNumber;// + " от " + resolution.DateExecuteSSP.Value.ToShortDateString();
                            }
                        }
                    }
                    else if (document.TypeDocumentGji == TypeDocumentGji.Protocol197)
                    {
                        var resolution = Protocol197Repo.GetAll()
                            .Where(y => y.Id == document.Id)
                            .Select(y => new
                            {
                                y.DocumentNumber,
                                y.DocumentDate
                            }).FirstOrDefault();
                        if (resolution != null)
                        {
                            if ((resolution.DocumentNumber != null) && (resolution.DocumentNumber != ""))
                            {
                                docNums = "Протокол по статье 19.7 " + resolution.DocumentNumber + " от " + resolution.DocumentDate.Value.ToShortDateString();
                            }
                        }
                    }
                }

            }
            else if (x.TypeLicenseRequest == TypeLicenseRequest.First && x.ManOrgLicenseRequest != null)
            {
                var manOrgLicReq = ManOrgLicenseRequestRepo.GetAll()
                    .Where(y => y == x.ManOrgLicenseRequest)
                    .Select(y => new
                    {
                        y.RegisterNumber,
                        y.DateRequest
                    }).FirstOrDefault();
                if (manOrgLicReq != null)
                {
                    if ((manOrgLicReq.RegisterNumber != null) && (manOrgLicReq.RegisterNumber != ""))
                    {
                        docNums = "Заявка на лицензию " + manOrgLicReq.RegisterNumber + " от " + manOrgLicReq.DateRequest.Value.ToShortDateString();
                    }
                }
            }
            else if (x.TypeLicenseRequest == TypeLicenseRequest.Reissuance && x.LicenseReissuance != null)
            {
                var licReissuance = LicenseReissuanceRepo.GetAll()
                    .Where(y => y == x.LicenseReissuance)
                    .Select(y => new
                    {
                        y.RegisterNumber,
                        y.ReissuanceDate
                    }).FirstOrDefault();
                if (licReissuance != null)
                {
                    if ((licReissuance.RegisterNumber != null) && (licReissuance.RegisterNumber != ""))
                    {
                        docNums = "Заявка на переоформление лицензии " + licReissuance.RegisterNumber + " от " + licReissuance.ReissuanceDate.Value.ToShortDateString();
                    }
                }
            }
            else if (x.TypeLicenseRequest == TypeLicenseRequest.Copy && x.LicenseReissuance != null)
            {
                var licReissuance = LicenseReissuanceRepo.GetAll()
                    .Where(y => y == x.LicenseReissuance)
                    .Select(y => new
                    {
                        y.RegisterNumber,
                        y.ReissuanceDate
                    }).FirstOrDefault();
                if (licReissuance != null)
                {
                    if ((licReissuance.RegisterNumber != null) && (licReissuance.RegisterNumber != ""))
                    {
                        docNums = "Заявка на дубликат лицензии " + licReissuance.RegisterNumber + " от " + licReissuance.ReissuanceDate.Value.ToShortDateString();
                    }
                }
            }
            return docNums;
        }
    }
}