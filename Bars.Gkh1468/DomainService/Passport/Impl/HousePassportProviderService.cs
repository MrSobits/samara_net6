namespace Bars.Gkh1468.DomainService.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using B4.Utils;
    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.States;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Entities;
    using Bars.Gkh1468.DataFiller;
    using Bars.Gkh1468.Domain;
    using Bars.Gkh1468.Entities;
    using Bars.Gkh1468.Entities.Passport;
    using Bars.Gkh1468.Enums;

    using Castle.Windsor;
    using Bars.Gkh1468.ViewModel;
    using System.IO;
    using System.Security.Cryptography.X509Certificates;
    using Bars.B4.Modules.FileStorage;
    using Gkh.Enums;
    using FileInfo = Bars.B4.Modules.FileStorage.FileInfo;

    public class HousePassportProviderService : IHousePassportProviderService
    {
        public IWindsorContainer Container { get; set; }

        public IDomainService<HouseProviderPassport> DomainService { get; set; }

        public IBaseProviderPassportRowService<HouseProviderPassportRow> PassportRowService { get; set; }

        public IFileManager fileManager { get; set; }

		public IUserIdentity UserIdentity { get; set; }
		public IAuthorizationService AuthorizationService { get; set; }
		public IDomainService<Operator> OperatorDomain { get; set; }

		public IDataResult MakeNewPassport(BaseParams baseParams)
        {
            var roId = baseParams.Params.GetAs<long>("roId");
            var oldPaspId = baseParams.Params.GetAs<long>("oldPaspId");
            var year = baseParams.Params.GetAs<int>("year");
            var month = baseParams.Params.GetAs<int>("month");
            var paspStructId = baseParams.Params.GetAs<long>("paspStructId");
            var housePaspService = Container.Resolve<IHousePassportService>();
            var realityObj = Container.Resolve<IDomainService<RealityObject>>().Get(roId);

            var currentUser = UserIdentity;
			var createByAllRo = AuthorizationService.Grant(UserIdentity, "Gkh1468.Passport.MyHouse.CreateByAllRo");

			Operator curOp = OperatorDomain.GetAll().FirstOrDefault(x => x.User.Id == currentUser.UserId);
			Contragent contragent = null;
			ContragentType contragentType = ContragentType.NotSet;
			HouseProviderPassport existed;
			if (!createByAllRo)
			{
				if (currentUser is AnonymousUserIdentity)
				{
					return new BaseDataResult(false, "Не определен оператор");
				}

				if (curOp == null)
				{
					return new BaseDataResult(false, "Не определен оператор");
				}

				if (curOp.Contragent == null)
				{
					return new BaseDataResult(false, "У оператора не проставлен контрагент.");
				}

				contragent = curOp.Contragent;
				contragentType = curOp.ContragentType;

				existed = DomainService.GetAll()
					.FirstOrDefault(x => x.RealityObject.Id == roId &&
					                     x.ReportYear == year &&
					                     x.ReportMonth == month &&
					                     x.Contragent.Id == contragent.Id);
			}
			else
			{
				var contragentId = curOp != null && curOp.Contragent != null ? curOp.Contragent.Id : 0;

				existed = DomainService.GetAll()
					.FirstOrDefault(x => x.RealityObject.Id == roId &&
					                     x.ReportYear == year &&
					                     x.ReportMonth == month &&
					                     (x.Contragent == null || x.Contragent.Id == contragentId));
			}

            if (existed != null)
            {
                return new BaseDataResult(false, "Уже существует паспорт в этом периоде");
            }

            var passportRowDomain = Container.Resolve<IDomainService<HouseProviderPassportRow>>();
            if (oldPaspId > 0)
            {
                var pasp = DomainService.Get(oldPaspId);
                var housePasp = housePaspService.GetPassport(realityObj, year, month).Data as HousePassport;

                var newPasp = new HouseProviderPassport
                {
                    HousePassport = housePasp,
                    Contragent = contragent,
                    ContragentType = contragentType,
                    RealityObject = pasp.RealityObject,
                    ReportYear = year,
                    ReportMonth = month,
                    PassportStruct = pasp.PassportStruct,
                    UserName = currentUser.Name
                };

                var stateProvider = Container.Resolve<IStateProvider>();
                stateProvider.SetDefaultState(newPasp);

                if (newPasp.State == null)
                {
                    return new BaseDataResult(false, "Необходимо настроить статусы для типа \"Мои паспорта домов\" в разделе \"Настройка статусов\"");
                }

                DomainService.Save(newPasp);

                InTransaction(() =>
                {
                    var values = passportRowDomain.GetAll()
                                                  .Where(x => x.ProviderPassport.Id == oldPaspId)
                                                  .ToArray();
                    // коллекционируем все самодостаточные значения
                    var allSufficientValues =
                        values.Where(x => !x.ParentValue.HasValue || x.ParentValue <= 0).OrderBy(x => x.Id).ToArray();
                    // и будем запоминать соответствие старого идентификатора новому
                    var allSufficientDict = new Dictionary<long, long>(allSufficientValues.Length);
                    foreach (var allSufficientValue in allSufficientValues)
                    {
                        var value = new HouseProviderPassportRow
                        {
                            MetaAttribute = allSufficientValue.MetaAttribute,
                            Passport = newPasp,
                            Value = allSufficientValue.Value,
                            GroupKey = allSufficientValue.GroupKey
                        };

                        passportRowDomain.Save(value);
                        // хоп и запомнили. старое -> новое
                        allSufficientDict[allSufficientValue.Id] = value.Id;
                    }

                    // а теперь пройдемся по зависимым значениям
                    foreach (
                        var dependentValue in
                            values.Where(x => x.ParentValue.HasValue && x.ParentValue > 0).OrderBy(x => x.Id))
                    {
                        passportRowDomain.Save(new HouseProviderPassportRow
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
                var parentPasp = housePaspService.GetPassport(realityObj, year, month);

                if (!parentPasp.Success)
                {
                    return parentPasp;
                }

                var newPasp = new HouseProviderPassport
                {
                    HousePassport = parentPasp.Data as HousePassport,
                    Contragent = contragent,
                    ContragentType = contragentType,
                    RealityObject = realityObj,
                    ReportYear = year,
                    ReportMonth = month,
                    PassportStruct = paspStruct,
                    UserName = currentUser.Name
				};

                var stateProvider = Container.Resolve<IStateProvider>();
                stateProvider.SetDefaultState(newPasp);

                if (newPasp.State == null)
                {
                    return new BaseDataResult(false, "Необходимо настроить статусы для типа \"Мои паспорта домов\" в разделе \"Настройка статусов\"");
                }

                DomainService.Save(newPasp);

                this.InTransaction(() => this.SaveMetaRows<HouseProviderPassportRow, HouseProviderPassport>(paspStructId, realityObj, newPasp));
            }

            return new BaseDataResult();
        }

        public IDataResult GetActivePassportStruct(BaseParams baseParams)
        {
            var roId = baseParams.Params.GetAs<long>("roId");
            var year = baseParams.Params.GetAs<long>("year");
            var month = baseParams.Params.GetAs<long>("month");

            if (year == 0 || month == 0 || roId == 0)
            {
                return new BaseDataResult();
            }

            var realtyObjTypeHouse = Container.Resolve<IDomainService<RealityObject>>().Get(roId).TypeHouse;

            var pasportStruct = Container.Resolve<IDomainService<PassportStruct>>().GetAll()
                .Where(x => x.PassportType == GetPassportType(realtyObjTypeHouse.To1468RealObjType()))
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

            var existingPassports = DomainService.GetAll()
                .Where(x => x.Contragent.Id == curOp.Contragent.Id)
                .Where(x => x.RealityObject.Id == roId)
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

        private PassportType GetPassportType(TypeRealObj typeRealObj)
        {
            return typeRealObj == TypeRealObj.Mkd ? PassportType.Mkd : PassportType.House;
        }

        public IDataResult ListByPassport(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();
            var housePassport = loadParams.Filter.GetAs<long>("passport");

            var data = DomainService.GetAll()
                .Where(x => x.HousePassport.Id == housePassport)
                .Select(x => new
                {
                    x.Id,
                    x.State,
                    x.ObjectCreateDate,
                    RealityObject = x.RealityObject.Address,
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
                x.RealityObject,
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