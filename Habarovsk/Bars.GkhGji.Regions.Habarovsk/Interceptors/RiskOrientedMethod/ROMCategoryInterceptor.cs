namespace Bars.GkhGji.Regions.Habarovsk.Interceptors
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

    class ROMCategoryInterceptor : EmptyDomainInterceptor<ROMCategory>
    {
        public IGkhUserManager UserManager { get; set; }

        public IDomainService<ManOrgContractRealityObject> MorgRODomain { get; set; }

        public IDomainService<ROMCategoryMKD> ROMCategoryMKDDomain { get; set; }

        public IDomainService<VnResolution> VnResolutionDomain { get; set; }

        public IDomainService<VpResolution> VpResolutionDomain { get; set; }

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
                         .Select(x => new
                         {
                             x.Id,
                             x.RealityObject,
                             x.ManOrgContract.StartDate
                         }).ToList();


                    foreach (var rec in manOrgRO)
                    {
                        var newObj = new ROMCategoryMKD();
                        newObj.ROMCategory = romCategory;
                        newObj.RealityObject = rec.RealityObject;
                        newObj.DateStart = rec.StartDate;
                        newObj.ObjectCreateDate = DateTime.Now;
                        newObj.ObjectEditDate = DateTime.Now;
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
                         .Select(x => new
                         {
                             x.Id,
                             x.RealityObject,
                             x.ManOrgContract.StartDate
                         }).ToList();


                    foreach (var rec in manOrgRO)
                    {
                        var newObj = new ROMCategoryMKD();
                        newObj.ROMCategory = romCategory;
                        newObj.RealityObject = rec.RealityObject;
                        newObj.DateStart = rec.StartDate;
                        newObj.ObjectCreateDate = DateTime.Now;
                        newObj.ObjectEditDate = DateTime.Now;
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
                         .Select(x => new
                         {
                             x.Id,
                             x.RealityObject,
                             x.ManOrgContract.StartDate
                         }).ToList();


                    foreach (var rec in manOrgRO)
                    {
                        var newObj = new ROMCategoryMKD();
                        newObj.ROMCategory = romCategory;
                        newObj.RealityObject = rec.RealityObject;
                        newObj.DateStart = rec.StartDate;
                        newObj.ObjectCreateDate = DateTime.Now;
                        newObj.ObjectEditDate = DateTime.Now;
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

                List<long> kindKNDArtLaws = new List<long>();
                if (romCategory.KindKND == KindKND.LicenseControl)
                {
                    kindKNDArtLaws = KindKNDDictArtLawDomain.GetAll()
                       .Where(x => x.KindKNDDict.KindKND == Enums.KindKND.LicenseControl && x.Koefficients == Enums.Koefficients.Vp)
                        .Select(x => x.ArticleLawGji.Id).ToList();
                }
                else
                {
                    kindKNDArtLaws = KindKNDDictArtLawDomain.GetAll()
                        .Where(x => x.KindKNDDict.KindKND == Enums.KindKND.HousingSupervision && x.Koefficients == Enums.Koefficients.Vp)
                        .Select(x => x.ArticleLawGji.Id).ToList();
                }

                //var protocols = ProtocolArticleLawDomain.GetAll()
                //    .Where(x => x.Protocol.Inspection.Contragent == entity.Contragent && kindKNDArtLaws.Contains(x.ArticleLaw.Id))
                //    .Select(x => x.Protocol.DocumentNumber + "|" + x.Protocol.Inspection.Id
                //    ).ToList();

                var protocols = ProtocolArticleLawDomain.GetAll()
                   .Where(x => x.Protocol.Inspection.Contragent == entity.Contragent && kindKNDArtLaws.Contains(x.ArticleLaw.Id))
                   .Select(x => new
                   {
                       protId = x.Protocol.Id,
                       Document = x.Protocol.DocumentNumber + "|" + x.Protocol.Inspection.Id,
                       ArtLaw = x.ArticleLaw.Name
                   }
                   ).AsEnumerable();

                var respros = ResolProsArticleLawDomain.GetAll()
                   .Where(x => x.ResolPros.Contragent != null && x.ResolPros.Contragent == entity.Contragent && kindKNDArtLaws.Contains(x.ArticleLaw.Id))
                   .Select(x => new
                   {
                       protId = x.ResolPros.Id,
                       Document = x.ResolPros.DocumentNumber + "|" + 999,
                       ArtLaw = x.ArticleLaw.Name
                   }).AsEnumerable();

                int pc = protocols.Count();

                if (respros.Count() > 0)
                {
                    protocols = protocols.Concat(respros);
                }

                //var prot197 = Protocol197ArticleLawDomain.GetAll()
                //   .Where(x => x.Protocol197.Contragent == entity.Contragent && kindKNDArtLaws.Contains(x.ArticleLaw.Id))
                //   .Select(x => new
                //   {
                //       protId = x.Protocol197.Id,
                //       Document = x.Protocol197.DocumentNumber + "|" + 999,
                //       ArtLaw = x.ArticleLaw.Name
                //   }
                //   ).AsEnumerable();

                //if (prot197.Count() > 0)
                //{
                //    protocols = protocols.Concat(prot197);
                //}

                var protList = protocols.Select(x => x.protId).ToList();


                var childResolutions = DocumentGjiChildrenDomain.GetAll()
                    .Where(x => x.Parent != null && x.Children != null)
                    .Where(x => protList.Contains(x.Parent.Id))
                    .Where(x => x.Children.TypeDocumentGji == TypeDocumentGji.Resolution)
                      .Select(x => new
                      {
                          protId = x.Parent.Id,
                          resol = x.Children.Id,
                      }
                   ).AsEnumerable();

                //childIds = DocumentGjiChildrenDomain.GetAll();

                Dictionary<long, string> protocolDict = new Dictionary<long, string>();
                foreach (var protocol in protocols)
                {
                    if (!protocolDict.ContainsKey(protocol.protId))
                    {
                        protocolDict.Add(protocol.protId, protocol.ArtLaw);
                    }
                    else
                    {
                        protocolDict[protocol.protId] += ", " + protocol.ArtLaw;
                    }
                }

                var resolutions = ResolutionDomain.GetAll()
                    .Where(x => x.Inspection != null && x.DocumentDate.HasValue && x.Contragent != null && x.Contragent.Id == entity.Contragent.Id && x.DocumentDate.Value >= calcDate.AddYears(-2)
                     /*&& x.InLawDate.HasValue*/)
                     .Where(x => x.Sanction != null && x.Sanction.Code != "2")
                     //  .Where(x => childResolutions.Any(y => y.resol == x.Id))
                     .Where(x => x.Inspection.TypeBase != TypeBase.ProtocolMhc && x.Inspection.TypeBase != TypeBase.ProtocolMvd &&
                      x.Inspection.TypeBase != TypeBase.HeatingSeason)
                    .Select(x => new
                    {
                        x.Id,
                        x.DocumentNumber,
                        x.Inspection
                    }).ToList();
                if (resolutions.Count > 0)
                {
                    foreach (var resolution in resolutions)
                    {
                        var prots = childResolutions.Where(x => x.resol == resolution.Id).FirstOrDefault();
                        if (prots == null)
                            continue;

                        VpCount++;
                        var newObj = new VpResolution();
                        newObj.ROMCategory = romCategory;
                        newObj.ArtLaws = protocolDict[prots.protId];
                        newObj.ObjectCreateDate = DateTime.Now;
                        newObj.ObjectEditDate = DateTime.Now;
                        newObj.Resolution = ResolutionDomain.Get(resolution.Id);
                        VpResolutionDomain.Save(newObj);
                    }

                }
                entity.Vp = VpCount;

                var prot197Vp = Protocol197ArticleLawDomain.GetAll()
                   .Where(x => x.Protocol197.Contragent == entity.Contragent && kindKNDArtLaws.Contains(x.ArticleLaw.Id))
                   .Where(x => x.Protocol197.DocumentDate.HasValue && x.Protocol197.DocumentDate.Value >= calcDate.AddYears(-2))
                   .Where(x => x.Protocol197.State.FinalState)
                   .Select(x => x.Protocol197.Id).Count();




                //Предписания Vpr

                var prescrCount = PrescriptionDomain.GetAll()
                    .Where(x => x.Contragent != null && x.Contragent == entity.Contragent)
                    .Where(x => x.DocumentDate.HasValue && x.DocumentDate.Value >= calcDate.AddYears(-2))
                    .Where(x => x.State != null && !x.State.StartState)
                    .Select(x => new
                    {
                        x.Id,
                        x.DocumentNumber,
                        x.DocumentDate
                    });

                int VprCount = 0;

                kindKNDArtLaws.Clear();
                if (romCategory.KindKND == KindKND.LicenseControl)
                {
                    kindKNDArtLaws = KindKNDDictArtLawDomain.GetAll()
                       .Where(x => x.KindKNDDict.KindKND == Enums.KindKND.LicenseControl && x.Koefficients == Enums.Koefficients.Vpr)
                        .Select(x => x.ArticleLawGji.Id).ToList();
                }
                else
                {
                    kindKNDArtLaws = KindKNDDictArtLawDomain.GetAll()
                        .Where(x => x.KindKNDDict.KindKND == Enums.KindKND.HousingSupervision && x.Koefficients == Enums.Koefficients.Vpr)
                        .Select(x => x.ArticleLawGji.Id).ToList();
                }

                //var protocols = ProtocolArticleLawDomain.GetAll()
                //    .Where(x => x.Protocol.Inspection.Contragent == entity.Contragent && kindKNDArtLaws.Contains(x.ArticleLaw.Id))
                //    .Select(x => x.Protocol.DocumentNumber + "|" + x.Protocol.Inspection.Id
                //    ).ToList();

                var protocolsVpr = ProtocolArticleLawDomain.GetAll()
               .Where(x => x.Protocol.Inspection.Contragent == entity.Contragent && kindKNDArtLaws.Contains(x.ArticleLaw.Id))
               .Select(x => new
               {
                   protId = x.Protocol.Id,
                   Document = x.Protocol.DocumentNumber + "|" + x.Protocol.Inspection.Id,
                   ArtLaw = x.ArticleLaw.Name
               }
               ).AsEnumerable();

                var resprosVpr = ResolProsArticleLawDomain.GetAll()
                 .Where(x => x.ResolPros.Contragent != null && x.ResolPros.Contragent == entity.Contragent && kindKNDArtLaws.Contains(x.ArticleLaw.Id))
                 .Select(x => new
                 {
                     protId = x.ResolPros.Id,
                     Document = x.ResolPros.DocumentNumber + "|" + 999,
                     ArtLaw = x.ArticleLaw.Name
                 }).AsEnumerable();

                if (resprosVpr.Count() > 0)
                {
                    protocolsVpr = protocolsVpr.Concat(resprosVpr);
                }

                var protocolsVprList = protocolsVpr.Select(x => x.protId).ToList();



                protocolDict.Clear();

                childResolutions = DocumentGjiChildrenDomain.GetAll()
                  .Where(x => x.Parent != null && x.Children != null)
                  .Where(x => protocolsVprList.Contains(x.Parent.Id))
                  .Where(x => x.Children.TypeDocumentGji == TypeDocumentGji.Resolution)
                    .Select(x => new
                    {
                        protId = x.Parent.Id,
                        resol = x.Children.Id,
                    }
                 ).AsEnumerable();

                //childIds = DocumentGjiChildrenDomain.GetAll();

                foreach (var protocol in protocolsVpr)
                {
                    if (!protocolDict.ContainsKey(protocol.protId))
                    {
                        protocolDict.Add(protocol.protId, protocol.ArtLaw);
                    }
                    else
                    {
                        protocolDict[protocol.protId] += ", " + protocol.ArtLaw;
                    }
                }

                //если по постановлениям, сейчас формула другая, комментируем
                //if (resolutions.Count > 0)
                //{
                //    foreach (var resolution in resolutions)
                //    {
                //        var prots = childResolutions.Where(x => x.resol == resolution.Id).FirstOrDefault();
                //        if (prots == null)
                //            continue;
                //        VprCount++;
                //        var newObj = new VprResolution();
                //        newObj.ROMCategory = romCategory;
                //        newObj.ArtLaws = protocolDict[prots.protId];
                //        newObj.ObjectCreateDate = DateTime.Now;
                //        newObj.ObjectEditDate = DateTime.Now;
                //        newObj.Resolution = ResolutionDomain.Get(resolution.Id);
                //        VprResolutionDomain.Save(newObj);
                //    }

                //}
                //если по предписаниям
                if (prescrCount != null && prescrCount.Count() > 0)
                {
                    foreach (var pres in prescrCount)
                    {
                        Prescription prescr = PrescriptionDomain.Get(pres.Id);
                        VprCount++;
                        var newObj = new VprPrescription();
                        newObj.ROMCategory = romCategory;
                        newObj.StateName = prescr.State.Name;
                        newObj.ObjectCreateDate = DateTime.Now;
                        newObj.ObjectEditDate = DateTime.Now;
                        newObj.Prescription = prescr;
                        VprPrescriptionDomain.Save(newObj);
                    }

                }

                //Постановления Vn

                int VnCount = 0;

                kindKNDArtLaws.Clear();
                if (romCategory.KindKND == KindKND.LicenseControl)
                {
                    kindKNDArtLaws = KindKNDDictArtLawDomain.GetAll()
                       .Where(x => x.KindKNDDict.KindKND == Enums.KindKND.LicenseControl && x.Koefficients == Enums.Koefficients.Vn)
                        .Select(x => x.ArticleLawGji.Id).ToList();
                }
                else
                {
                    kindKNDArtLaws = KindKNDDictArtLawDomain.GetAll()
                        .Where(x => x.KindKNDDict.KindKND == Enums.KindKND.HousingSupervision && x.Koefficients == Enums.Koefficients.Vn)
                        .Select(x => x.ArticleLawGji.Id).ToList();
                }

                //var protocols = ProtocolArticleLawDomain.GetAll()
                //    .Where(x => x.Protocol.Inspection.Contragent == entity.Contragent && kindKNDArtLaws.Contains(x.ArticleLaw.Id))
                //    .Select(x => x.Protocol.DocumentNumber + "|" + x.Protocol.Inspection.Id
                //    ).ToList();

                var protocolsVn = ProtocolArticleLawDomain.GetAll()
                   .Where(x => x.Protocol.Inspection.Contragent == entity.Contragent && kindKNDArtLaws.Contains(x.ArticleLaw.Id))
                   .Select(x => new
                   {
                       protId = x.Protocol.Id,
                       Document = x.Protocol.DocumentNumber + "|" + x.Protocol.Inspection.Id,
                       ArtLaw = x.ArticleLaw.Name
                   }
                   ).AsEnumerable();

                var resprosVn = ResolProsArticleLawDomain.GetAll()
               .Where(x => x.ResolPros.Contragent != null && x.ResolPros.Contragent == entity.Contragent && kindKNDArtLaws.Contains(x.ArticleLaw.Id))
               .Select(x => new
               {
                   protId = x.ResolPros.Id,
                   Document = x.ResolPros.DocumentNumber + "|" + 999,
                   ArtLaw = x.ArticleLaw.Name
               }).AsEnumerable();

                if (resprosVn.Count() > 0)
                {
                    protocolsVn = protocolsVn.Concat(resprosVn);
                }

                var protocolsVnList = protocolsVn.Select(x => x.protId).ToList();



                childResolutions = DocumentGjiChildrenDomain.GetAll()
                .Where(x => x.Parent != null && x.Children != null)
                .Where(x => protocolsVnList.Contains(x.Parent.Id))
                .Where(x => x.Children.TypeDocumentGji == TypeDocumentGji.Resolution)
                  .Select(x => new
                  {
                      protId = x.Parent.Id,
                      resol = x.Children.Id,
                  }
               ).AsEnumerable();


                protocolDict.Clear();
                foreach (var protocol in protocolsVn)
                {
                    if (!protocolDict.ContainsKey(protocol.protId))
                    {
                        protocolDict.Add(protocol.protId, protocol.ArtLaw);
                    }
                    else
                    {
                        protocolDict[protocol.protId] += ", " + protocol.ArtLaw;
                    }
                }

                if (resolutions.Count > 0)
                {
                    foreach (var resolution in resolutions)
                    {
                        var prots = childResolutions.Where(x => x.resol == resolution.Id).FirstOrDefault();
                        if (prots == null)
                            continue;
                        VnCount++;
                        var newObj = new VnResolution();
                        newObj.ROMCategory = romCategory;
                        newObj.ArtLaws = protocolDict[prots.protId];
                        newObj.ObjectCreateDate = DateTime.Now;
                        newObj.ObjectEditDate = DateTime.Now;
                        newObj.Resolution = ResolutionDomain.Get(resolution.Id);
                        VnResolutionDomain.Save(newObj);
                    }

                }

                var prot197Vn = Protocol197ArticleLawDomain.GetAll()
              .Where(x => x.Protocol197.Contragent == entity.Contragent && kindKNDArtLaws.Contains(x.ArticleLaw.Id))
              .Where(x => x.Protocol197.DocumentDate.HasValue && x.Protocol197.DocumentDate.Value >= calcDate.AddYears(-2))
              .Where(x => x.Protocol197.State.FinalState)
              .Select(x => x.Protocol197.Id).Count();



                entity.Vp = VpCount;
                entity.Vpr = VprCount;
                entity.Vn = VnCount;
                // прибавляем утвержденные протоколы 19.7
                entity.Vp += prot197Vp;
                entity.Vn += prot197Vn;

                if (areaTotal > 0)
                {
                    decimal result = (5 * VpCount + VnCount + 0.5m * VprCount) / areaTotal;
                    entity.Result = result.RoundDecimal(2);
                    if (entity.KindKND == Enums.KindKND.HousingSupervision)
                    {
                        if (result <= 0.3m)
                        {
                            entity.RiskCategory = RiskCategory.Low;
                        }
                        if (result > 0.3m && result <= 1)
                        {
                            entity.RiskCategory = RiskCategory.Moderate;
                        }
                        if (result > 1 && result <= 3.5m)
                        {
                            entity.RiskCategory = RiskCategory.Average;
                        }
                        if (result > 3.5m)
                        {
                            entity.RiskCategory = RiskCategory.High;
                        }
                    }
                    else if (entity.KindKND == Enums.KindKND.LicenseControl)
                    {
                        if (result <= 0.04m)
                        {
                            entity.RiskCategory = RiskCategory.Low;
                        }
                        if (result > 0.04m && result <= 0.3m)
                        {
                            entity.RiskCategory = RiskCategory.Moderate;
                        }
                        if (result > 0.3m && result <= 0.6m)
                        {
                            entity.RiskCategory = RiskCategory.Average;
                        }
                        if (result > 0.6m)
                        {
                            entity.RiskCategory = RiskCategory.High;
                        }
                    }
                }
                return Success();
            }
            catch (Exception e)
            {
                return Failure("Не удалось сформировать списки для расчета коэффициентов");
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
                return Success();
            }
            catch (Exception e)
            {
                return Failure(e.ToString());
            }

        }
    }
}
