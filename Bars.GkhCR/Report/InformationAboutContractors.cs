namespace Bars.GkhCr.Report
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Bars.B4;
    
    using Bars.B4.Modules.States;
    using B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.GkhCr.Entities;
    using Bars.GkhCr.Enums;

    using Castle.Windsor;

    public class InformationAboutContractors : BasePrintForm
    {
        public IWindsorContainer Container { get; set; }

        private long[] municipalityIds;
        private long programCrId;

        public InformationAboutContractors()
            : base(new ReportTemplateBinary(Properties.Resources.InformationAboutContractors))
        {
        }

        public override string Name
        {
            get
            {
                return "Сведения по подрядчикам";
            }
        }

        public override string Desciption
        {
            get
            {
                return "Сведения по подрядчикам";
            }
        }

        public override string GroupName
        {
            get
            {
                return "Формы программы";
            }
        }

        public override string ParamsController
        {
            get
            {
                return "B4.controller.report.InformationAboutContractors";
            }
        }

        public override string RequiredPermission
        {
            get
            {
                return "Reports.CR.InformationAboutContractors";
            }
        }

        public override void SetUserParams(BaseParams baseParams)
        {
            this.programCrId = baseParams.Params["programCrId"].ToInt();

            var m = baseParams.Params["municipalityIds"].ToStr();
            this.municipalityIds = !string.IsNullOrEmpty(m) ? m.Split(',').Select(x => x.ToLong()).ToArray() : new long[0];
        }

        public override void PrepareReport(ReportParams reportParams)
        {
            var serviceBuildContract = Container.Resolve<IDomainService<BuildContract>>();
            var serviceTypeWorkCr = Container.Resolve<IDomainService<TypeWorkCr>>();
            var serviceBuilderFeedback = Container.Resolve<IDomainService<BuilderFeedback>>();
            var serviceContragentContact = Container.Resolve<IDomainService<ContragentContact>>();
            var serviceContragentBank = Container.Resolve<IDomainService<ContragentBank>>();
            var serviceBuilderDocument = Container.Resolve<IDomainService<BuilderDocument>>();
            var serviceBuilderTechnique = Container.Resolve<IDomainService<BuilderTechnique>>();
            var serviceBuilderWorkforce = Container.Resolve<IDomainService<BuilderWorkforce>>();
            var serviceBuilderProductionBase = Container.Resolve<IDomainService<BuilderProductionBase>>();
            var serviceVoiceMember = Container.Resolve<IDomainService<VoiceMember>>();
            var serviceFinanceSource = Container.Resolve<IDomainService<FinanceSource>>();

            var financeSource1 = serviceFinanceSource.GetAll().FirstOrDefault(x => x.Code == "1");
            var financeSource2 = serviceFinanceSource.GetAll().FirstOrDefault(x => x.Code == "3");
            var financeSourceId1 = financeSource1 != null ? financeSource1.Id : -1;
            var financeSourceId2 = financeSource2 != null ? financeSource2.Id : -1;
            
            var buildContractQuery = serviceBuildContract.GetAll()
                .WhereIf(this.municipalityIds.Length > 0, x => this.municipalityIds.Contains(x.ObjectCr.RealityObject.Municipality.Id))
                .Where(x => x.ObjectCr.ProgramCr.Id == programCrId)
                .Select(x => new
                    {
                        builderContractId = x.Id,
                        crId = x.ObjectCr.Id,
                        MuId = x.ObjectCr.RealityObject.Municipality.Id,
                        MuName = x.ObjectCr.RealityObject.Municipality.Name,
                        RoId = x.ObjectCr.RealityObject.Id,
                        BuilderId = (long?)x.Builder.Id,
                        BuilderName = x.Builder.Contragent.Name,
                        RoAddress = x.ObjectCr.RealityObject.Address,
                        x.ObjectCr.ProgramCr.TypeProgramStateCr,
                        x.Builder.WorkWithoutContractor,
                        JuridicalAddress = x.Builder.Contragent.FiasJuridicalAddress.AddressName,
                        x.Builder.Contragent.MailingAddress,
                        ContragentId = (long?)x.Builder.Contragent.Id,
                        x.Builder.Contragent.Phone,
                        x.Builder.Contragent.Inn,
                        x.Builder.Contragent.Kpp,
                        x.Builder.ConsentInfo,
                        x.Builder.TaxInfoAddress,
                        x.Builder.TaxInfoPhone,
                        x.ObjectCr.RealityObject.ObjectEditDate,
                        x.Builder.AdvancedTechnologies
                    });

            var roIds = buildContractQuery.Select(x => x.RoId);

            var buildersIds = buildContractQuery
                .Where(x => x.BuilderId.HasValue)
                .Select(x => x.BuilderId.Value);

            var contragentIds = buildContractQuery
                .Where(x => x.ContragentId.HasValue)
                .Select(x => x.ContragentId.Value);

            var builderProductionBaseDict = serviceBuilderProductionBase.GetAll()
                .Where(x => buildersIds.Contains(x.Builder.Id))
                .Where(x => contragentIds.Contains(x.Builder.Contragent.Id))
                .Select(x => new
                {
                    BuilderId = x.Builder.Id,
                    ContragentId = x.Builder.Contragent.Id,
                    x.KindEquipment.Name
                })
                .ToList()
                .GroupBy(x => x.BuilderId)
                .ToDictionary(x => x.Key, x => x.Select(z => z.Name)
                        .ToList());

            var builderWorkforceDict = serviceBuilderWorkforce.GetAll()
                .Where(x => buildersIds.Contains(x.Builder.Id))
                .Where(x => contragentIds.Contains(x.Builder.Contragent.Id))
                .Select(x => new
                {
                    BuilderId = x.Builder.Id,
                    ContragentId = x.Builder.Contragent.Id,
                    x.Specialty.Name
                })
                .ToList()
                .GroupBy(x => x.BuilderId)
                .ToDictionary(x => x.Key, x => x.Select(z => z.Name)
                        .ToList());

            var builderTechniqueDict = serviceBuilderTechnique.GetAll()
                .Where(x => buildersIds.Contains(x.Builder.Id))
                .Where(x => contragentIds.Contains(x.Builder.Contragent.Id))
                .Select(x => new
                {
                    BuilderId = x.Builder.Id,
                    x.Name
                })
                .ToList()
                .GroupBy(x => x.BuilderId)
                .ToDictionary(x => x.Key, x => x.Select(y => y.Name).ToList());
            
            var builderDocumentDict = serviceBuilderDocument.GetAll()
                .Where(x => buildersIds.Contains(x.Builder.Id))
                .Where(x => contragentIds.Contains(x.Builder.Contragent.Id))
                .Select(x => new
                    {
                        BuilderId = x.Builder.Id,
                        x.BuilderDocumentType,
                        x.DocumentNum,
                        x.DocumentDate
                    })
                .ToList()
                .GroupBy(x => x.BuilderId)
                .ToDictionary(x => x.Key, x => x.Distinct().ToList());
            
            var contragentBanks = serviceContragentBank.GetAll()
                .Where(x => contragentIds.Contains(x.Contragent.Id))
                .Select(x => new
                                 {
                                     x.Name,
                                     ContragentId = x.Contragent.Id,
                                     x.SettlementAccount,
                                     x.CorrAccount,
                                     x.Bik,
                                     x.Okonh,
                                     x.Okpo
                                 })
                .ToList()
                .GroupBy(x => x.ContragentId)
                .ToDictionary(x => x.Key, x => x.ToList());

            var contragentFIO = serviceContragentContact.GetAll()
                .Where(x => contragentIds.Contains(x.Contragent.Id))
                .Where(x => x.Position.Code == "1")
                .Select(x => new
                                 {
                                     x.FullName,
                                     ContragentId = x.Contragent.Id
                                 })
                .ToList()
                .GroupBy(x => x.ContragentId)
                .ToDictionary(x => x.Key, x => x.Select(z => z.FullName).First());

            var buildContractDict = buildContractQuery
                .Where(x => x.BuilderId.HasValue)
                .ToList()
                .GroupBy(x => new { x.BuilderId, x.BuilderName })
                .ToDictionary(x => x.Key, x =>
                    x.GroupBy(y => new { y.MuId, y.MuName })
                    .ToDictionary(y => y.Key, y =>
                        y.GroupBy(z => new { z.RoId, z.RoAddress })
                        .ToDictionary(z => z.Key, z =>
                            z.Select(q =>
                                {
                                    var builderName = q.BuilderName;
                                    var roAddress = q.RoAddress;
                                    var workWithoutContractor = string.Empty;
                                    var consentInfo = string.Empty;
                                    var AdvancedTechnologies = string.Empty;

                                    switch (q.WorkWithoutContractor)
                                    {
                                        case YesNoNotSet.Yes:
                                            workWithoutContractor = "Да";
                                            break;
                                        case YesNoNotSet.No:
                                            workWithoutContractor = "Нет";
                                            break;
                                    }

                                    switch (q.ConsentInfo)
                                    {
                                        case YesNoNotSet.Yes:
                                            consentInfo = "Да";
                                            break;
                                        case YesNoNotSet.No:
                                            consentInfo = "Нет";
                                            break;
                                    }

                                    switch (q.AdvancedTechnologies)
                                    {
                                        case YesNoNotSet.Yes:
                                            AdvancedTechnologies = "Да";
                                            break;
                                        case YesNoNotSet.No:
                                            AdvancedTechnologies = "Нет";
                                            break;
                                    }

                                    return new
                                               {
                                                   builderName,
                                                   roAddress,
                                                   workWithoutContractor,
                                                   AdvancedTechnologies,
                                                   consentInfo,
                                                   builderData = q
                                               };
                                }).First())));
            
            var voiceMemberDict = serviceVoiceMember.GetAll()
                .Where(x => roIds.Contains(x.Qualification.ObjectCr.RealityObject.Id))
                .Where(x => buildersIds.Contains(x.Qualification.Builder.Id))
                .Where(x => x.TypeAcceptQualification == TypeAcceptQualification.Yes)
                .Select(x => new
                    {
                        roId = x.Qualification.ObjectCr.RealityObject.Id,
                        builderId = x.Qualification.Builder.Id,
                        qualificationMemberName = x.QualificationMember.Name,
                        x.TypeAcceptQualification,
                        x.DocumentDate
                    })
                .ToList()
                .GroupBy(x => x.builderId)
                .ToDictionary(x => x.Key, x =>
                    x.GroupBy(y => y.roId)
                    .ToDictionary(y => y.Key, y => 
                        y.Select(z => new
                        {
                            documentDate = z.DocumentDate.HasValue ? z.DocumentDate.Value.ToShortDateString() : string.Empty,
                            memberName = z.qualificationMemberName
                        })
                        .ToList()));

            var voiceMemberList = voiceMemberDict.Values.SelectMany(x => x.Values.SelectMany(z => z.Select(y => y.memberName).ToList())).Distinct().ToList();
            
            var roWorkSumDict =
                Container.Resolve<IDomainService<TypeWorkCr>>()
                         .GetAll()
                         .Where(x => roIds.Contains(x.ObjectCr.RealityObject.Id))
                         .Where(x => x.ObjectCr.ProgramCr.Id == this.programCrId)
                         .GroupBy(x => x.ObjectCr.RealityObject.Id)
                         .Select(x => new { x.Key, sum = x.Sum(y => y.Sum) })
                         .ToDictionary(x => x.Key, x => x.sum.HasValue ? x.sum.Value: 0);
               
            var housesValuesByBuilderIdDict = buildContractQuery
                 .Where(x => x.TypeProgramStateCr == TypeProgramStateCr.Complete)
                 .Select(x => new
                 {
                     x.BuilderId,
                     x.RoId
                 })
                 .ToList()
                 .GroupBy(x => x.BuilderId)
                .ToDictionary(x => x.Key, x => new
                                                   {
                                                       count = x.Count(),
                                                       housesSum = x.Where(y => roWorkSumDict.ContainsKey(y.RoId)).Sum(y => roWorkSumDict[y.RoId])
                                                   });
            
            var houseLimitSumDict = serviceTypeWorkCr.GetAll()
                .Where(x => roIds.Contains(x.ObjectCr.RealityObject.Id))
                .Where(x => x.ObjectCr.ProgramCr.Id == this.programCrId)
                .Where(x => x.FinanceSource.Id == financeSourceId1 || x.FinanceSource.Id == financeSourceId2)
                .GroupBy(x => x.ObjectCr.RealityObject.Id)
                .Select(x => new
                {
                    x.Key,
                    sum = x.Sum(y => y.Sum)
                })
                .ToDictionary(x => x.Key, x => x.sum);

            var BuilderFeedbackDict = serviceBuilderFeedback.GetAll()
                .Where(x => buildersIds.Contains(x.Builder.Id))
                .Where(x => contragentIds.Contains(x.Builder.Contragent.Id))
                .Where(x => x.TypeAssessment == TypeAssessment.Positive)
                .Where(x => x.TypeAuthor == TypeAuthor.Customer || x.TypeAuthor == TypeAuthor.Other)
                .Select(x => new
                    {
                        x.OrganizationName,
                        BuilderId = x.Builder.Id
                    })
                .ToList()
                .GroupBy(x => x.BuilderId)
                .ToDictionary(x => x.Key, x => x.Select(y => y.OrganizationName).ToList());
            
            var count = 0;
            var membersDict = voiceMemberList.ToDictionary(voicemember => voicemember, voicemember => ++count);

            if (voiceMemberList.Count > 0)
            {
                var columnReturn = reportParams.ComplexReportParams.ДобавитьСекцию("columnAccept");

                foreach (var member in voiceMemberList)
                {
                    columnReturn.ДобавитьСтроку();
                    columnReturn["DynamicName"] = string.Format("$DynamicName{0}$", membersDict[member]);
                    columnReturn["CombNameContragent"] = string.Format("$CombNameContragent{0}$", membersDict[member]);
                }
            }

            var section = reportParams.ComplexReportParams.ДобавитьСекцию("section");

            voiceMemberList.ForEach(x => reportParams.SimpleReportParams["DynamicName" + membersDict[x]] = x);

            foreach (var builder in buildContractDict.OrderBy(x => x.Key.BuilderName))
            {
                var builderId = builder.Key.BuilderId.HasValue ? builder.Key.BuilderId.Value : -1;
                var builderValue = builder.Value;
                
                foreach (var municipality in builderValue.OrderBy(x => x.Key.MuName))
                {
                    var municipalityValue = municipality.Value;
                    
                    foreach (var realtyObject in municipalityValue.OrderBy(x => x.Key.RoAddress))
                    {
                        var realtyObjectId = realtyObject.Key.RoId;
                        var roValue = realtyObject.Value;
                        var builderData = roValue.builderData;

                        count = 1;

                        if (builderDocumentDict.ContainsKey(builderId))
                        {
                            var docDatas = builderDocumentDict[builderId];
                            count = docDatas.GroupBy(x => x.BuilderDocumentType.Code).Select(x => x.Count()).Max();
                        }

                        for (int i = 0; i < count; i++)
                        {
                            section.ДобавитьСтроку();

                            var contragentId = builderData.ContragentId.HasValue ? builderData.ContragentId.Value : -1;

                            section["ContragentName"] = builder.Key.BuilderName;
                            section["MO"] = municipality.Key.MuName;

                            section["Address"] = realtyObject.Key.RoAddress;

                            if (voiceMemberDict.ContainsKey(builderId))
                            {
                                var voicemember = voiceMemberDict[builderId];

                                foreach (var member in voiceMemberList)
                                {
                                    section["CombNameContragent" + membersDict[member]] = voicemember.ContainsKey(realtyObjectId) ? voicemember[realtyObjectId].Where(x => x.memberName == member).Select(x => x.documentDate).FirstOrDefault() : string.Empty;
                                }
                            }

                            section["SumLimitHouse"] = houseLimitSumDict.ContainsKey(realtyObjectId) ? houseLimitSumDict[realtyObjectId] : decimal.Zero;
                            section["HouseCount"] = housesValuesByBuilderIdDict.ContainsKey(builderId) ? housesValuesByBuilderIdDict[builderId].count : decimal.Zero;
                            section["Sum"] = housesValuesByBuilderIdDict.ContainsKey(builderId) ? housesValuesByBuilderIdDict[builderId].housesSum : decimal.Zero;
                            section["PositivReview"] = BuilderFeedbackDict.ContainsKey(builderId) ? BuilderFeedbackDict[builderId].Aggregate((cur, next) => cur + "\n" + next) : string.Empty;
                            section["ChanceWork"] = roValue.workWithoutContractor;
                            section["JurAddress"] = builderData.JuridicalAddress;
                            section["Mail"] = builderData.MailingAddress;
                            section["FIOContragent"] = contragentFIO.ContainsKey(contragentId) ? contragentFIO[contragentId] : string.Empty;
                            section["Phone"] = builderData.Phone;
                            section["INN"] = builderData.Inn;
                            section["KPP"] = builderData.Kpp;

                            if (contragentBanks.ContainsKey(contragentId))
                            {
                                var bankData = contragentBanks[contragentId];

                                section["NameServicingBank"] = bankData.Select(x => x.Name).Aggregate((cur, next) => cur + "\n" + next);
                                section["PaymentAccount"] = bankData.Select(x => x.SettlementAccount).Aggregate((cur, next) => cur + "\n" + next);
                                section["CorrespondentAccount"] = bankData.Select(x => x.CorrAccount).Aggregate((cur, next) => cur + "\n" + next);
                                section["BIK"] = bankData.Select(x => x.Bik).Aggregate((cur, next) => cur + "\n" + next);
                                section["OKONH"] = bankData.Select(x => x.Okonh).Aggregate((cur, next) => cur + "\n" + next);
                                section["OKPO"] = bankData.Select(x => x.Okpo).Aggregate((cur, next) => cur + "\n" + next);
                            }

                            section["Agreement"] = builderData.ConsentInfo.GetEnumMeta().Display;
                            section["TaxAddress"] = builderData.TaxInfoAddress;
                            section["TaxPhone"] = builderData.TaxInfoPhone;

                            if (builderDocumentDict.ContainsKey(builderId))
                            {
                                var docDatas = builderDocumentDict[builderId];

                                if (docDatas.Any(x => x.BuilderDocumentType.Code == (int)TypeDocument.CertificateConformance))
                                {
                                    var docdataList = docDatas.Where(x => x.BuilderDocumentType.Code == (int)TypeDocument.CertificateConformance).ToList();

                                    if (docdataList.Count >= (i + 1))
                                    {
                                        var docdata = docdataList[i];

                                    section["DocYesNo"] = "Да";
                                    section["DocNum"] = docdata.DocumentNum;
                                    section["DocDate"] = docdata.DocumentDate.HasValue ? docdata.DocumentDate.Value.ToShortDateString() : string.Empty;
                                    }
                                }

                                if (docDatas.Any(x => x.BuilderDocumentType.Code == (int)TypeDocument.LackOfTaxDebts))
                                {
                                    var docdataList = docDatas.Where(x => x.BuilderDocumentType.Code == (int)TypeDocument.LackOfTaxDebts).ToList();

                                    if (docdataList.Count >= (i + 1))
                                    {
                                        var docdata = docdataList[i];

                                        section["DebtDocYesNo"] = "Да";
                                        section["DebtDocNum"] = docdata.DocumentNum;
                                        section["DebtDocDate"] = docdata.DocumentDate.HasValue ? docdata.DocumentDate.Value.ToShortDateString() : string.Empty;
                                    }
                                }

                                if (docDatas.Any(x => x.BuilderDocumentType.Code == (int)TypeDocument.LackOfCreditorDebts))
                                {
                                    var docdataList = docDatas.Where(x => x.BuilderDocumentType.Code == (int)TypeDocument.LackOfCreditorDebts).ToList();

                                    if (docdataList.Count >= (i + 1))
                                    {
                                        var docdata = docdataList[i];

                                        section["CreditDocYesNo"] = "Да";
                                        section["CreditDocNum"] = docdata.DocumentNum;
                                        section["CreditDocDate"] = docdata.DocumentDate.HasValue ? docdata.DocumentDate.Value.ToShortDateString() : string.Empty;
                                    }
                                }

                                if (docDatas.Any(x => x.BuilderDocumentType.Code == (int)TypeDocument.NoLiquidationNoActivity))
                                {
                                    var docdataList = docDatas.Where(x => x.BuilderDocumentType.Code == (int)TypeDocument.NoLiquidationNoActivity).ToList();

                                    if (docdataList.Count >= (i + 1))
                                    {
                                        var docdata = docdataList[i];

                                        section["LiqDocYesNo"] = "Да";
                                        section["LiqDocNum"] = docdata.DocumentNum;
                                        section["LiqDocDate"] = docdata.DocumentDate.HasValue ? docdata.DocumentDate.Value.ToShortDateString() : string.Empty;
                                    }
                                }

                                if (docDatas.Any(x => x.BuilderDocumentType.Code == (int)TypeDocument.LackOfRegulationsGji))
                                {
                                    var docdataList = docDatas.Where(x => x.BuilderDocumentType.Code == (int)TypeDocument.LackOfRegulationsGji).ToList();

                                    if (docdataList.Count >= (i + 1))
                                    {
                                        var docdata = docdataList[i];

                                        section["PresDocYesNo"] = "Да";
                                        section["PresDocNum"] = docdata.DocumentNum;
                                        section["PresDocDate"] = docdata.DocumentDate.HasValue ? docdata.DocumentDate.Value.ToShortDateString() : string.Empty;
                                    }
                                }
                            }

                            section["UseAdvanced"] = builderData.AdvancedTechnologies.GetEnumMeta().Display;
                            section["AvailEquip"] = builderTechniqueDict.ContainsKey(builderId) ? builderTechniqueDict[builderId].Aggregate((cur, next) => cur + "\n" + next) : string.Empty;
                            section["LaborForce"] = builderWorkforceDict.ContainsKey(builderId) ? builderWorkforceDict[builderId].Aggregate((cur, next) => cur + "\n" + next) : string.Empty;
                            section["ManufacturingBase"] = builderProductionBaseDict.ContainsKey(builderId) ? builderProductionBaseDict[builderId].Aggregate((cur, next) => cur + "\n" + next) : string.Empty;
                            section["DateLast"] = builderData.ObjectEditDate;
                        }
                    }
                }
            }
        }

        public override string ReportGenerator
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }
    }

    sealed class PassportData
    {
        public string Name { get; set; }

        public DateTime ChangeDate { get; set; }
    }
}