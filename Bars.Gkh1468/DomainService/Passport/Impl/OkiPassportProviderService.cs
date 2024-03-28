namespace Bars.Gkh1468.DomainService.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.States;
    using Bars.B4.Utils;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Entities;
    using Bars.Gkh1468.DataFiller;
    using Bars.Gkh1468.Entities;
    using Bars.Gkh1468.Enums;

    using Castle.Windsor;

    using Castle.Windsor;
    using Bars.Gkh1468.ViewModel;
    using System.IO;
    using System.Security.Cryptography.X509Certificates;
    using Bars.B4.Modules.FileStorage;
    using FileInfo = Bars.B4.Modules.FileStorage.FileInfo;

    public class OkiPassportProviderService : IOkiPassportProviderService
    {
        public IWindsorContainer Container { get; set; }

        public IDomainService<OkiProviderPassport> DomainOkiProviderPassport { get; set; }

        public IBaseProviderPassportRowService<OkiProviderPassportRow> PassportRowService { get; set; }

        public IFileManager fileManager { get; set; }

        public IDataResult MakeNewPassport(BaseParams baseParams)
        {
            var muId = baseParams.Params.GetAs<long>("muId");
            var oldPaspId = baseParams.Params.GetAs<long>("oldPaspId");
            var year = baseParams.Params.GetAs<int>("year");
            var month = baseParams.Params.GetAs<int>("month");
            var paspStructId = baseParams.Params.GetAs<long>("paspStructId");
            var okipaspService = Container.Resolve<IOkiPassportService>();
            var municipality = Container.Resolve<IDomainService<Municipality>>().Get(muId);

            var currentUser = Container.Resolve<IUserIdentity>();

            if (currentUser is AnonymousUserIdentity)
            {
                return new BaseDataResult(false, "Не определен оператор");
            }

            var curOp = Container.Resolve<IDomainService<Operator>>().GetAll()
                .FirstOrDefault(x => x.User.Id == currentUser.UserId);

            if (curOp == null)
            {
                return new BaseDataResult(false, "Не определен оператор");
            }

            if (curOp.Contragent == null)
            {
                return new BaseDataResult(false, "У оператора не проставлен контрагент");
            }

            var existed = DomainOkiProviderPassport.GetAll()
                .FirstOrDefault(x => x.Municipality.Id == muId
                                     && x.ReportYear == year
                                     && x.ReportMonth == month
                                     && x.Contragent.Id == curOp.Contragent.Id
                                     && x.ContragentType == curOp.ContragentType);

            if (existed != null)
            {
                return new BaseDataResult(false, "В указанном периоде паспорт уже создан");
            }

            var stateProvider = Container.Resolve<IStateProvider>();

            if (oldPaspId > 0)
            {
                var pasp = DomainOkiProviderPassport.Get(oldPaspId);
                var okiPasp = okipaspService.GetPassport(municipality, year, month);

                if (!okiPasp.Success)
                {
                    return okiPasp;
                }

                var serv = Container.Resolve<IDomainService<OkiProviderPassportRow>>();

                var newPasp = new OkiProviderPassport
                {
                    OkiPassport = okiPasp.Data as OkiPassport,
                    Contragent = curOp.Contragent,
                    ContragentType = curOp.ContragentType,
                    Municipality = pasp.Municipality,
                    ReportYear = year,
                    ReportMonth = month,
                    PassportStruct = pasp.PassportStruct,
                    UserName = curOp.User.Name
                };

               stateProvider.SetDefaultState(newPasp);

                if (newPasp.State == null)
                {
                    return new BaseDataResult(false, "Необходимо настроить статусы для типа \"Мои паспорта ОКИ\" в разделе \"Настройка статусов\"");
                }

                DomainOkiProviderPassport.Save(newPasp);

                InTransaction(() =>
                {
                    var values = serv.GetAll()
                                     .Where(x => x.ProviderPassport.Id == oldPaspId)
                                     .ToArray();
                    // коллекционируем все самодостаточные значения
                    var allSufficientValues =
                        values.Where(x => !x.ParentValue.HasValue || x.ParentValue <= 0).OrderBy(x => x.Id).ToArray();
                    // и будем запоминать соответствие старого идентификатора новому
                    var allSufficientDict = new Dictionary<long, long>(allSufficientValues.Length);
                    foreach (var allSufficientValue in allSufficientValues)
                    {
                        var value = new OkiProviderPassportRow
                        {
                            MetaAttribute = allSufficientValue.MetaAttribute,
                            Passport = newPasp,
                            Value = allSufficientValue.Value,
                            GroupKey = allSufficientValue.GroupKey
                        };

                        serv.Save(value);
                        // хоп и запомнили. старое -> новое
                        allSufficientDict[allSufficientValue.Id] = value.Id;
                    }

                    // а теперь пройдемся по зависимым значениям
                    foreach (
                        var dependentValue in
                            values.Where(x => x.ParentValue.HasValue && x.ParentValue > 0).OrderBy(x => x.Id))
                    {
                        serv.Save(new OkiProviderPassportRow
                        {
                            MetaAttribute = dependentValue.MetaAttribute,
                            Passport = newPasp,
                            Value = dependentValue.Value,
                            GroupKey = dependentValue.GroupKey,
                            // из словаря берем показываем от чего они теперь зависят
                            ParentValue = allSufficientDict.Get(dependentValue.ParentValue.Value, (long?) null)
                        });
                    }

                    PassportRowService.UpdateFillPercent(newPasp.Id);
                });
            }
            else
            {
                var paspStruct = Container.Resolve<IDomainService<PassportStruct>>().Get(paspStructId);
                var parentPasp = okipaspService.GetPassport(municipality, year, month);

                if (!parentPasp.Success)
                {
                    return parentPasp;
                }

                var newPasp = new OkiProviderPassport
                {
                    OkiPassport = parentPasp.Data as OkiPassport,
                    Contragent = curOp.Contragent,
                    ContragentType = curOp.ContragentType,
                    Municipality = municipality,
                    ReportYear = year,
                    ReportMonth = month,
                    PassportStruct = paspStruct,
                    UserName = curOp.User.Name
                };

                stateProvider.SetDefaultState(newPasp);

                if (newPasp.State == null)
                {
                    return new BaseDataResult(false, "Необходимо настроить статусы для типа \"Мои паспорта ОКИ\" в разделе \"Настройка статусов\"");
                }

                DomainOkiProviderPassport.Save(newPasp);

                InTransaction(() => SaveMetaRows<OkiProviderPassportRow, OkiProviderPassport>(paspStructId, null, newPasp));
            }

            return new BaseDataResult();
        }

        public IDataResult GetActivePassportStruct(BaseParams baseParams)
        {
            var year = baseParams.Params.GetAs<long>("year");
            var month = baseParams.Params.GetAs<long>("month");

            if (year == 0 || month == 0)
            {
                return new BaseDataResult();
            }

            var pasportStruct = Container.Resolve<IDomainService<PassportStruct>>().GetAll()
                .Where(x => x.PassportType == PassportType.Nets)
                .Where(x => (x.ValidFromYear < year) || (x.ValidFromYear == year && x.ValidFromMonth <= month))
                .Select(x => new { x.Id, x.Name, x.ValidFromMonth, x.ValidFromYear })
                .OrderByDescending(x => x.ValidFromYear)
                .ThenByDescending(x => x.ValidFromMonth)
                .FirstOrDefault();

            if (pasportStruct == null)
            {
                return new BaseDataResult(new { Id = 0 });
            }

            var manager = Container.Resolve<IGkhUserManager>();
            var curOp = manager.GetActiveOperator();

            if (curOp == null || curOp.Contragent == null)
            {
                return new BaseDataResult(new { pasportStruct.Id, pasportStruct.Name });
            }

            var monthsDict = new Dictionary<int, string>
                             {
                                 { 1, "Январь" },
                                 { 2, "Февраль" },
                                 { 3, "Март" },
                                 { 4, "Апрель" },
                                 { 5, "Май" },
                                 { 6, "Июнь" },
                                 { 7, "Июль" },
                                 { 8, "Август" },
                                 { 9, "Сентябрь" },
                                 { 10, "Октябрь" },
                                 { 11, "Ноябрь" },
                                 { 12, "Декабрь" }
                             };

            var muId = baseParams.Params.GetAs<long>("muId");

            var existingPassports = DomainOkiProviderPassport.GetAll()
                .Where(x => x.Contragent.Id == curOp.Contragent.Id)
                .Where(x => x.Municipality.Id == muId)
                .Where(x => x.PassportStruct.Id == pasportStruct.Id)
                .Select(x => new
                {
                    x.Id,
                    x.ReportYear,
                    x.ReportMonth
                })
                .OrderByDescending(x => x.ReportYear)
                .ThenByDescending(x => x.ReportMonth)
                .AsEnumerable()
                .Select(x => new
                {
                    x.Id,
                    Period = string.Format("{0} {1}", x.ReportYear, monthsDict.ContainsKey(x.ReportMonth) ? monthsDict[x.ReportMonth] : string.Empty)
                })
                .ToList();

            return new BaseDataResult(new { pasportStruct.Id, pasportStruct.Name, existingPassports = existingPassports.Any() ? existingPassports : null });
        }

        public IDataResult ListByPassport(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();
            var okiPassport = loadParams.Filter.Get("passport", (long)0);

            var data = DomainOkiProviderPassport.GetAll()
                .Where(x => x.OkiPassport.Id == okiPassport)
                .Select(x => new
                {
                    x.Id,
                    x.State,
                    x.ObjectCreateDate,
                    Municipality = x.Municipality.Name,
                    Contragent = x.Contragent.Name,
                    x.Percent,
                    x.Certificate
                })
                .Filter(loadParams, Container)
                .Order(loadParams)
                .Paging(loadParams);
            var result = data.Select(x => new
            {
                x.Id,
                x.State,
                x.ObjectCreateDate,
                x.Municipality,
                x.Contragent,
                x.Percent,
                Signature = GetSigny(x.Certificate),
            }).ToArray();

            return new ListDataResult(result, data.Count());
        }

        public string GetSigny(FileInfo certificate)
        {

            //Container.Resolve<IFileManager>();
            if (certificate == null)
            {
                return string.Empty;
            }

            using (var certStream = new MemoryStream())
            {
                fileManager.GetFile(certificate).CopyTo(certStream);

                var certX509 = new X509Certificate2(certStream.ToArray());

                return certX509.GetNameInfo(X509NameType.SimpleName, false);
            }
        }

        public IDataResult MunicipalityForOki(BaseParams baseParams)
        {
            var loadParam = baseParams.GetLoadParam();

            var manager = Container.Resolve<IGkhUserManager>();
            var curUp = manager.GetActiveOperator();

            if (curUp == null)
            {
                return new ListDataResult(null, 0);
            }
            
            var domainServOrgMu = Container.Resolve<IDomainService<ServiceOrgMunicipality>>();
            var domainManOrgMu = Container.Resolve<IDomainService<ManagingOrgMunicipality>>();
            var domainSuppResOrgMu = Container.Resolve<IDomainService<SupplyResourceOrgMunicipality>>();
            var domainPublicServOrgMu = Container.Resolve<IDomainService<PublicServiceOrgMunicipality>>();

            var list = domainServOrgMu.GetAll().Where(x => x.ServOrg.Contragent.Id == curUp.Contragent.Id).Select(x => new { x.Municipality.Id, x.Municipality.Name }).ToList();
            list.AddRange(domainManOrgMu.GetAll().Where(x => x.ManOrg.Contragent.Id == curUp.Contragent.Id).Select(x => new { x.Municipality.Id, x.Municipality.Name }).ToList());
            list.AddRange(domainSuppResOrgMu.GetAll().Where(x => x.SupplyResourceOrg.Contragent.Id == curUp.Contragent.Id).Select(x => new { x.Municipality.Id, x.Municipality.Name }).ToList());
            list.AddRange(domainPublicServOrgMu.GetAll().Where(x => x.PublicServiceOrg.Contragent.Id == curUp.Contragent.Id).Select(x => new { x.Municipality.Id, x.Municipality.Name }).ToList());

            var result = list.Distinct(x => x.Id).AsQueryable().Filter(loadParam, Container);

            Container.Release(manager);
            Container.Release(domainManOrgMu);
            Container.Release(domainPublicServOrgMu);
            Container.Release(domainServOrgMu);
            Container.Release(domainSuppResOrgMu);

            return new ListDataResult(result.Order(loadParam).Paging(loadParam).ToArray(), result.Count());
        }

        protected void SaveMetaRows<TRow, TPassport>(long paspStructId, RealityObject realityObject, TPassport passport)
            where TRow : BaseProviderPassportRow<TPassport>
            where TPassport : BaseProviderPassport
        {
            var passportRowDomain = Container.Resolve<IDomainService<TRow>>();
            var metaAttributeDomain = Container.Resolve<IDomainService<MetaAttribute>>();

            var metaParts = Container.Resolve<IDomainService<Part>>().GetAll().Where(x => x.Struct.Id == paspStructId).Select(x => x.Id).ToArray();

            var metaAttributes = metaAttributeDomain.GetAll().Where(x => metaParts.Contains(x.ParentPart.Id)).ToList();

            var result = new List<BaseProviderPassportRow>();
            var fillers = Container.ResolveAll<IDataFiller>();
            foreach (var metaAttribute in metaAttributes)
            {
                if (Container.Kernel.HasComponent(metaAttribute.DataFillerCode))
                {
                    foreach (var dataFiller in fillers.Where(x => x.Code == metaAttribute.DataFillerCode))
                    {
                        dataFiller.RealityObject = realityObject;
                        dataFiller.Result = result;
                        dataFiller.MetaAttribute = metaAttribute;
                        dataFiller.To1468();
                    }
                }
            }

            result.ForEach(
                x =>
                {
                    var y = Activator.CreateInstance<TRow>();
                    y.Passport = passport;
                    y.MetaAttribute = x.MetaAttribute;
                    y.Value = x.Value;

                    passportRowDomain.Save(y);
                });
        }

        protected void InTransaction(Action action)
        {
            using (var transaction = Container.Resolve<IDataTransaction>())
            {
                try
                {
                    action();

                    transaction.Commit();
                }
                catch (Exception exc)
                {
                    try
                    {
                        transaction.Rollback();
                    }
                    catch (TransactionRollbackException ex)
                    {
                        throw new DataAccessException(ex.Message, exc);
                    }
                    catch (Exception e)
                    {
                        throw new DataAccessException(
                            string.Format(
                                "Произошла не известная ошибка при откате транзакции: \r\nMessage: {0}; \r\nStackTrace:{1};",
                                e.Message,
                                e.StackTrace),
                            exc);
                    }

                    throw;
                }
            }
        }
    }
}
