namespace Bars.GkhGji.Regions.Chelyabinsk.Interceptors
{
    using Entities;
    using System;
    using System.Collections.Generic;
    using Enums;
    using Bars.GkhGji.Enums;
    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Entities;
    using System.Linq;
    using B4.Modules.States;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.Protocol197;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Overhaul.Entities;

    class ROMCategoryInterceptor : EmptyDomainInterceptor<ROMCategory>
    {
        public IGkhUserManager UserManager { get; set; }

        public IDomainService<ManOrgContractRealityObject> MorgRODomain { get; set; }

        public IDomainService<ServiceOrganization> ServiceOrganizationDomain { get; set; }

        public IDomainService<ServiceOrgRealityObjectContract> ServiceOrgRealityObjectContractDomain { get; set; }        

        public IDomainService<ROMCategoryMKD> ROMCategoryMKDDomain { get; set; }

        public IDomainService<VnResolution> VnResolutionDomain { get; set; }

        public IDomainService<VpResolution> VpResolutionDomain { get; set; }
        public IDomainService<G1Resolution> G1ResolutionDomain { get; set; }

        public IDomainService<VprResolution> VprResolutionDomain { get; set; }

        public IDomainService<Resolution> ResolutionDomain { get; set; }

        public IDomainService<Prescription> PrescriptionDomain { get; set; }

        public IDomainService<DocumentGjiChildren> DocumentGjiChildrenDomain { get; set; }

        public IDomainService<ProtocolArticleLaw> ProtocolArticleLawDomain { get; set; }

        public IDomainService<ResolProsArticleLaw> ResolProsArticleLawDomain { get; set; }

        public IDomainService<ManOrgLicense> ManOrgLicenseDomain { get; set; }

        public IDomainService<InspectionGji> InspectionGjiDomain { get; set; }

        public IDomainService<KindKNDDictArtLaw> KindKNDDictArtLawDomain { get; set; }

        public IDomainService<ROMCategory> ROMCategoryDomain { get; set; }

        public IDomainService<DocumentGji> ContragentDomain { get; set; }

        public IDomainService<VprPrescription> VprPrescriptionDomain { get; set; }

        public IDomainService<Protocol197ArticleLaw> Protocol197ArticleLawDomain { get; set; }




        public override IDataResult BeforeCreateAction(IDomainService<ROMCategory> service, ROMCategory entity)
        {
            try
            {

                var romCat = ROMCategoryDomain.GetAll()
                    .Where(x => x.KindKND == entity.KindKND && x.YearEnums == entity.YearEnums && x.Contragent == entity.Contragent).FirstOrDefault();
                if (romCat != null)
                {
                    if (entity.KindKND == KindKND.LicenseControl)
                        return Failure("В выбранном периоде рассчета уже создан расчет для данного контрагента по виду КНД лицензионный контроль");
                    else
                        return Failure("В выбранном периоде рассчета уже создан расчет для данного контрагента по виду КНД жилищный надзор");
                }

                var stateProvider = Container.Resolve<IStateProvider>();
                try
                {
                    stateProvider.SetDefaultState(entity);

                }
                catch
                {
                    return Failure("Для расчета не задан начальный статус");
                }
                finally
                {
                    Container.Release(stateProvider);
                }

                Operator thisOperator = UserManager.GetActiveOperator();
                if (thisOperator.Inspector != null)
                    entity.Inspector = thisOperator.Inspector;
                else
                    return Failure("Расчет категорий риска доступен только сотрудникам ГЖИ");

                return Success();
            }
            catch (Exception e)
            {
                return Failure("Не удалось создать задачу");
            }

        }

        public override IDataResult AfterCreateAction(IDomainService<ROMCategory> service, ROMCategory entity)
        {
            try
            {
                ROMCategory romCategory = ROMCategoryDomain.Get(entity.Id);
                // МКД в управлении
                decimal areaTotal = 0;
                DateTime startDate = DateTime.Now;
                bool isLicense = false;
                var licenseList = ManOrgLicenseDomain.GetAll()
                    .Where(x => x.Contragent == entity.Contragent)
                    .Select(x => x.Id).ToList();
                if (licenseList.Count > 0)
                {
                    isLicense = true;
                }
                if (romCategory.KindKND == KindKND.LicenseControl && isLicense)
                {
                    var manOrgRO = MorgRODomain.GetAll()
                        .Where(x => x.ManOrgContract.EndDate == null && x.ManOrgContract.ManagingOrganization.Contragent == entity.Contragent)
                        .Where(x => x.ManOrgContract.TypeContractManOrgRealObj == Gkh.Enums.TypeContractManOrg.ManagingOrgOwners)
                        .Where(x => x.RealityObject.ConditionHouse != ConditionHouse.Emergency && x.RealityObject.ConditionHouse != ConditionHouse.Razed)
                         .Select(x => new
                         {
                             x.Id,
                             x.RealityObject,
                             x.ManOrgContract.StartDate
                         }).ToList();


                    foreach (var rec in manOrgRO)
                    {
                        var hasVdgo = GetVDGOInfo(rec.RealityObject.Id);
                        if (hasVdgo == YesNoNotSet.Yes || rec.RealityObject.Floors > 5)
                        {
                            hasVdgo = YesNoNotSet.Yes;
                            entity.SeverityGroup = SeverityGroup.AGroup;
                        }
                        var newObj = new ROMCategoryMKD();
                        newObj.ROMCategory = romCategory;
                        newObj.RealityObject = rec.RealityObject;
                        newObj.DateStart = rec.StartDate;
                        newObj.ObjectCreateDate = DateTime.Now;
                        newObj.ObjectEditDate = DateTime.Now;
                        newObj.HasGasOrLift = hasVdgo;
                        ROMCategoryMKDDomain.Save(newObj);
                        areaTotal += rec.RealityObject.AreaMkd.HasValue ? rec.RealityObject.AreaMkd.Value : 0;
                        if (rec.StartDate.HasValue && startDate > rec.StartDate)
                        {
                            startDate = rec.StartDate.Value;
                        }

                    }
                }
                else if (romCategory.KindKND == KindKND.HousingSupervision && isLicense)
                {
                    var manOrgRO = MorgRODomain.GetAll()
                        .Where(x => x.ManOrgContract.EndDate == null && x.ManOrgContract.ManagingOrganization.Contragent == entity.Contragent)
                        .Where(x => x.ManOrgContract.TypeContractManOrgRealObj != Gkh.Enums.TypeContractManOrg.ManagingOrgOwners)
                          .Where(x => x.RealityObject.ConditionHouse != ConditionHouse.Emergency && x.RealityObject.ConditionHouse != ConditionHouse.Razed)
                         .Select(x => new
                         {
                             x.Id,
                             x.RealityObject,
                             x.ManOrgContract.StartDate
                         }).ToList();

                    var roids = manOrgRO.Select(x => x.RealityObject.Id).Distinct().ToList();

                    foreach (var rec in manOrgRO)
                    {
                        var hasVdgo = GetVDGOInfo(rec.RealityObject.Id);
                        if (hasVdgo == YesNoNotSet.Yes || rec.RealityObject.Floors > 5)
                        {
                            hasVdgo = YesNoNotSet.Yes;
                            entity.SeverityGroup = SeverityGroup.AGroup;
                        }
                        var newObj = new ROMCategoryMKD();
                        newObj.ROMCategory = romCategory;
                        newObj.RealityObject = rec.RealityObject;
                        newObj.DateStart = rec.StartDate;
                        newObj.ObjectCreateDate = DateTime.Now;
                        newObj.ObjectEditDate = DateTime.Now;
                        newObj.HasGasOrLift = hasVdgo;
                        ROMCategoryMKDDomain.Save(newObj);
                        areaTotal += rec.RealityObject.AreaMkd.HasValue ? rec.RealityObject.AreaMkd.Value : 0;
                        if (rec.StartDate.HasValue && startDate > rec.StartDate)
                        {
                            startDate = rec.StartDate.Value;
                        }

                    }
                }
                else if (romCategory.KindKND == KindKND.HousingSupervision && !isLicense)
                {

                    var manOrgRO = MorgRODomain.GetAll()
                        .Where(x => x.ManOrgContract.EndDate == null && x.ManOrgContract.ManagingOrganization.Contragent == entity.Contragent)
                          .Where(x => x.RealityObject.ConditionHouse != ConditionHouse.Emergency && x.RealityObject.ConditionHouse != ConditionHouse.Razed)
                         .Select(x => new
                         {
                             x.Id,
                             x.RealityObject,
                             x.ManOrgContract.StartDate
                         }).ToList();

                    var roids = manOrgRO.Select(x => x.RealityObject.Id).Distinct().ToList();


                    foreach (var rec in manOrgRO)
                    {
                        var hasVdgo = GetVDGOInfo(rec.RealityObject.Id);
                        if (hasVdgo == YesNoNotSet.Yes || rec.RealityObject.Floors > 5)
                        {
                            hasVdgo = YesNoNotSet.Yes;
                            entity.SeverityGroup = SeverityGroup.AGroup;
                        }
                        var newObj = new ROMCategoryMKD();
                        newObj.ROMCategory = romCategory;
                        newObj.RealityObject = rec.RealityObject;
                        newObj.DateStart = rec.StartDate;
                        newObj.ObjectCreateDate = DateTime.Now;
                        newObj.ObjectEditDate = DateTime.Now;
                        newObj.HasGasOrLift = hasVdgo;
                        ROMCategoryMKDDomain.Save(newObj);
                        areaTotal += rec.RealityObject.AreaMkd.HasValue ? rec.RealityObject.AreaMkd.Value : 0;
                        if (rec.StartDate.HasValue && startDate > rec.StartDate)
                        {
                            startDate = rec.StartDate.Value;
                        }

                    }
                }

                int year = entity.YearEnums.GetDisplayName().ToInt();
                if (romCategory.KindKND == KindKND.LicenseControl)
                {
                    if (startDate < DateTime.Parse("01.05.2015"))
                    {
                        startDate = DateTime.Parse("01.05.2015");
                    }
                }
                int R = (year - startDate.Year) * 12 + (1 - startDate.Month);
                if (R < 0 && entity.CalcDate.HasValue)
                {
                    R = (entity.CalcDate.Value.Month - startDate.Month);
                }
                if (R > 24)
                {
                    R = 24;
                }
                areaTotal = areaTotal / 1000;
                entity.MkdAreaTotal = areaTotal;
                entity.MonthCount = R;

                //Постановления Vp
                DateTime calcDate = DateTime.Parse("01.01." + year);
                int VpCount = 0;

                List<long> g1ArtLaws = new List<long>();

                g1ArtLaws = KindKNDDictArtLawDomain.GetAll()
                    .Where(x=> !x.KindKNDDict.DateTo.HasValue || (x.KindKNDDict.DateTo.HasValue && x.KindKNDDict.DateTo.Value>entity.CalcDate.Value))
                    .Where(x => x.KindKNDDict.KindKND == romCategory.KindKND && x.Koefficients == Enums.Koefficients.G1)
                    .Select(x => x.ArticleLawGji.Id).Distinct().ToList();

                //var protocols = ProtocolArticleLawDomain.GetAll()
                //    .Where(x => x.Protocol.Inspection.Contragent == entity.Contragent && kindKNDArtLaws.Contains(x.ArticleLaw.Id))
                //    .Select(x => x.Protocol.DocumentNumber + "|" + x.Protocol.Inspection.Id
                //    ).ToList();

                var protg1 = ProtocolArticleLawDomain.GetAll()
                    .Where(x => x.Protocol.DocumentDate.HasValue && x.Protocol.DocumentDate.Value >= calcDate.AddMonths(-40))
                   .Where(x => x.Protocol.Inspection.Contragent == entity.Contragent && g1ArtLaws.Contains(x.ArticleLaw.Id))
                   .Select(x => new
                   {
                       protId = x.Protocol.Id,
                       Document = x.Protocol.DocumentNumber + "|" + x.Protocol.Inspection.Id,
                       ArtLaw = x.ArticleLaw.Name
                   }
                   ).AsEnumerable();

                var protgzhi1 = Protocol197ArticleLawDomain.GetAll()
                .Where(x => x.Protocol197.DocumentDate.HasValue && x.Protocol197.DocumentDate.Value >= calcDate.AddMonths(-40))
               .Where(x => x.Protocol197.Contragent == entity.Contragent && g1ArtLaws.Contains(x.ArticleLaw.Id))
               .Select(x => new
               {
                   protId = x.Protocol197.Id,
                   Document = x.Protocol197.DocumentNumber + "|" + x.Protocol197.Inspection.Id,
                   ArtLaw = x.ArticleLaw.Name
               }
               ).AsEnumerable();

                var resprosg1 = ResolProsArticleLawDomain.GetAll()
                 .Where(x => x.ResolPros.DocumentDate.HasValue && x.ResolPros.DocumentDate.Value > calcDate.AddMonths(-40))
                .Where(x => x.ResolPros.Contragent != null && x.ResolPros.Contragent == entity.Contragent && g1ArtLaws.Contains(x.ArticleLaw.Id))
                .Select(x => new
                {
                    protId = x.ResolPros.Id,
                    Document = x.ResolPros.DocumentNumber + "|" + 999,
                    ArtLaw = x.ArticleLaw.Name
                }).AsEnumerable();


                Dictionary<long, string> protocolg1Dict = new Dictionary<long, string>();

                if (resprosg1.Count() > 0)
                {
                    protg1 = protg1.Concat(resprosg1);
                }
                if (protgzhi1.Count() > 0)
                {
                    protg1 = protg1.Concat(protgzhi1);
                }
                foreach (var protocol in protg1)
                {
                    if (!protocolg1Dict.ContainsKey(protocol.protId))
                    {
                        protocolg1Dict.Add(protocol.protId, protocol.ArtLaw);
                    }
                    else
                    {
                        protocolg1Dict[protocol.protId] += ", " + protocol.ArtLaw;
                    }
                }
                entity.ProbabilityGroup = ProbabilityGroup.Group2;
                if (protocolg1Dict.Count() > 0)
                {
                    List<long> idsList = protocolg1Dict.Select(x => x.Key).ToList();

                    if (romCategory.KindKND == KindKND.LicenseControl)
                    {
                        var resList = ResolutionDomain.GetAll()
                            .Where(x=> x.Contragent == entity.Contragent && x.State.Name != "Оспаривание")
                            .Where(x=> x.PenaltyAmount.HasValue && x.PenaltyAmount.Value>0 && !x.WrittenOff)
                            .Where(x => x.DocumentDate > calcDate.AddMonths(-24) && x.DocumentDate<= calcDate
                            && (x.PayStatus == ResolutionPaymentStatus.NotPaid || x.PayStatus == ResolutionPaymentStatus.NotSet) && x.InLawDate.HasValue)
                            .Select(x => x.Id).ToList();

                        DocumentGjiChildrenDomain.GetAll()
                     .Where(x => x.Parent != null && x.Children != null)
                       .Where(x => idsList.Contains(x.Parent.Id))
                       .Where(x => x.Children.TypeDocumentGji == TypeDocumentGji.Resolution)
                       .WhereIf(resList.Count>0,x => resList.Contains(x.Children.Id))
                     .Select(x => new
                     {
                         prId = x.Parent.Id,
                         Resol = x.Children.Id
                     }).ToList().ForEach(x =>
                     {
                         if (resList.Count > 0 && resList.Contains(x.Resol))
                         {
                             var newObj = new G1Resolution();
                             newObj.ROMCategory = romCategory;
                             newObj.ArtLaws = protocolg1Dict.ContainsKey(x.prId) ? protocolg1Dict[x.prId] : "";
                             newObj.ObjectCreateDate = DateTime.Now;
                             newObj.ObjectEditDate = DateTime.Now;
                             newObj.Resolution = ResolutionDomain.Get(x.Resol);
                             G1ResolutionDomain.Save(newObj);
                             entity.ProbabilityGroup = ProbabilityGroup.Group1;
                         }
                     });
                    }
                    else
                    {
                        DocumentGjiChildrenDomain.GetAll()
                     .Where(x => x.Parent != null && x.Children != null)
                       .Where(x => idsList.Contains(x.Parent.Id))
                        .Where(x => x.Children.TypeDocumentGji == TypeDocumentGji.Resolution && x.Children.DocumentDate > calcDate.AddMonths(-36))
                     .Select(x => new
                     {
                         prId = x.Parent.Id,
                         Resol = x.Children.Id
                     }).ToList().ForEach(x =>
                     {
                         var newObj = new G1Resolution();
                         newObj.ROMCategory = romCategory;
                         newObj.ArtLaws = protocolg1Dict.ContainsKey(x.prId) ? protocolg1Dict[x.prId] : "";
                         newObj.ObjectCreateDate = DateTime.Now;
                         newObj.ObjectEditDate = DateTime.Now;
                         newObj.Resolution = ResolutionDomain.Get(x.Resol);
                         G1ResolutionDomain.Save(newObj);
                         entity.ProbabilityGroup = ProbabilityGroup.Group1;
                     });
                    }

                    
                }
                else
                {
                    entity.ProbabilityGroup = ProbabilityGroup.Group2;
                }

                entity.Vp = 0;
                int VprCount = 0;
                int VnCount = 0;




                entity.Vp = VpCount;
                entity.Vpr = VprCount;
                entity.Vn = VnCount;

                if (areaTotal > 0)
                {
                    decimal result = (5 * VpCount + VnCount + 0.5m * VprCount) / areaTotal;
                    entity.Result = result.RoundDecimal(2);

                    if (entity.SeverityGroup == SeverityGroup.AGroup)
                    {
                        if (entity.ProbabilityGroup == ProbabilityGroup.Group1)
                        {
                            entity.RiskCategory = RiskCategory.High;
                        }
                        else
                        {
                            entity.RiskCategory = RiskCategory.Average;
                        }
                    }
                    else
                    {
                        entity.SeverityGroup = SeverityGroup.BGroup;
                        if (entity.ProbabilityGroup == ProbabilityGroup.Group1)
                        {
                            entity.RiskCategory = RiskCategory.Moderate;
                        }
                        else
                        {
                            entity.ProbabilityGroup = ProbabilityGroup.Group2;
                            entity.RiskCategory = RiskCategory.Low;
                        }

                    }


                }
                return Success();
            }
            catch (Exception e)
            {
                return Failure("Не удалось сформировать списки для расчета коэффициентов " + e.Message + " " + e.StackTrace);
            }

        }

        public override IDataResult BeforeUpdateAction(IDomainService<ROMCategory> service, ROMCategory entity)
        {
            try
            {

                Operator thisOperator = UserManager.GetActiveOperator();
                if (thisOperator.Inspector != null)
                    entity.Inspector = thisOperator.Inspector;
                else
                    return Failure("Расчет категорий риска доступен только сотрудникам ГЖИ");

                return Success();
            }
            catch (Exception e)
            {
                return Failure("Не удалось сохранить задачу");
            }
        }

        public override IDataResult BeforeDeleteAction(IDomainService<ROMCategory> service, ROMCategory entity)
        {
            try
            {
                var MKDList = ROMCategoryMKDDomain.GetAll()
               .Where(x => x.ROMCategory.Id == entity.Id)
               .Select(x => x.Id).ToList();
                foreach (var id in MKDList)
                {
                    ROMCategoryMKDDomain.Delete(id);
                }
                var VpList = VpResolutionDomain.GetAll()
            .Where(x => x.ROMCategory.Id == entity.Id)
            .Select(x => x.Id).ToList();
                foreach (var id in VpList)
                {
                    VpResolutionDomain.Delete(id);
                }
                var VprList = VprResolutionDomain.GetAll()
               .Where(x => x.ROMCategory.Id == entity.Id)
               .Select(x => x.Id).ToList();
                foreach (var id in VprList)
                {
                    VprResolutionDomain.Delete(id);
                }
                var VprprList = VprPrescriptionDomain.GetAll()
              .Where(x => x.ROMCategory.Id == entity.Id)
              .Select(x => x.Id).ToList();
                foreach (var id in VprprList)
                {
                    VprPrescriptionDomain.Delete(id);
                }
                var VnList = VnResolutionDomain.GetAll()
               .Where(x => x.ROMCategory.Id == entity.Id)
               .Select(x => x.Id).ToList();
                foreach (var id in VnList)
                {
                    VnResolutionDomain.Delete(id);
                }
                var G1List = G1ResolutionDomain.GetAll()
           .Where(x => x.ROMCategory.Id == entity.Id)
           .Select(x => x.Id).ToList();
                foreach (var id in G1List)
                {
                    G1ResolutionDomain.Delete(id);
                }
                return Success();
            }
            catch (Exception e)
            {
                return Failure(e.ToString());
            }

        }

        private YesNoNotSet GetVDGOInfo(long roId)
        {
            var structElService = Container.Resolve<IDomainService<RealityObjectStructuralElement>>();
            try
            {
                var existRo = structElService.GetAll()
                     .Where(x => x.RealityObject.Id == roId)
                     .Where(x => x.State.StartState && (x.StructuralElement.Name.ToLower().Contains("газо") || x.StructuralElement.Name.Contains("лифто")))
                     .Select(x => x.Id).FirstOrDefault();
                if (existRo > 0)
                {
                    return YesNoNotSet.Yes;
                }


            }
            finally
            {
                Container.Release(structElService);
            }
            return YesNoNotSet.No;
        }
    }
}
