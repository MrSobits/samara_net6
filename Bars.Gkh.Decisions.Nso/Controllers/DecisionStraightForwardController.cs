namespace Bars.Gkh.Decisions.Nso.Controllers
{
	using B4;
	using B4.DataAccess;
	using B4.Modules.FileStorage;
	using B4.Modules.States;
	using B4.Utils;
	using Bars.B4.IoC;
	using Bars.Gkh.Enums.Decisions;
	using Domain;
	using Entities;
	using Entities.Proxies;
	using Gkh.Domain;
	using Gkh.Entities;
	using System;
	using System.Collections;
	using System.Linq;
	using Microsoft.AspNetCore.Mvc;

    public class DecisionStraightForwardController : FileStorageDataController<RealityObjectDecisionProtocol>
    {
        public ActionResult SaveOrUpdateDecisions(BaseParams baseParams)
        {
            var result = Container.Resolve<IRobjectDecisionService>().SaveOrUpdateDecisions(baseParams);

            return new JsonNetResult(result.Data) { ContentType = "text/html; charset=utf-8" };
        }

        public ActionResult SaveConfirm(BaseParams baseParams)
        {
            var notification = baseParams.Params.ReadClass<DecisionNotification>();

            var notificationDomain = Container.ResolveDomain<DecisionNotification>();
            notification.Protocol = new RealityObjectDecisionProtocol {Id = baseParams.Params.GetAs<long>("Protocol")};

            notificationDomain.Update(notification);

            return JsSuccess();
        }

        public ActionResult GetOwnerType(BaseParams baseParams)
        {
            var roId = baseParams.Params.GetAs<long>("roId");

            var contract = GetCurrentManOrgContract(new RealityObject {Id = roId});

            return JsSuccess(
                contract != null && contract.ManOrgContract.ManagingOrganization != null
                    ? contract.ManOrgContract.ManagingOrganization.Contragent.Name
                    : "Не задано");
        }

		public ActionResult GetConfirm(BaseParams baseParams)
		{
			var decisionService = Container.Resolve<IDecisionStraightForwardService>();
			using (Container.Using(decisionService))
			{
				var result = decisionService.GetConfirm(baseParams);
				return JsSuccess(result.Data);
			}
		}

        public override ActionResult Get(BaseParams baseParams)
        {
            var result = Container.Resolve<IRobjectDecisionService>().Get(baseParams);

            return JsSuccess(result.Data);
        }

        public ActionResult GetLatest(BaseParams baseParams)
        {
            var roId = baseParams.Params.GetAs<long>("roId");
            var decisionService = Container.Resolve<IUltimateDecisionService>();
            using(Container.Using(decisionService))
            {
                var proxy = Activator.CreateInstance<UltimateDecisionProxy>();
                proxy.MinFundAmountDecision = decisionService.GetActualDecision<MinFundAmountDecision>(roId);
                proxy.CrFundFormationDecision = decisionService.GetActualDecision<CrFundFormationDecision>(roId);
                return JsSuccess(proxy);
            }
        }

        public ActionResult CreateNotfication(BaseParams baseParams)
        {
            var proxy = baseParams.Params.ReadClass<UltimateDecisionProxy>();

            if(proxy == null)
            {
                return JsFailure("Нет данных по решениям!");
            }

            SaveOrUpdateInternal(baseParams, proxy);

            var validation = ValidateBeforeNotification(proxy);

            if(!validation.Success)
            {
                return JsFailure(validation.Message);
            }

            return CreateNotfication(proxy);
        }

        private ActionResult CreateNotfication(UltimateDecisionProxy proxy)
        {
            if(proxy == null
               || proxy.CrFundFormationDecision == null
               || proxy.CrFundFormationDecision.Decision != CrFundFormationDecisionType.SpecialAccount
               || proxy.CreditOrgDecision == null
               || proxy.CreditOrgDecision.Decision == null)
            {
                return JsFailure("Ошибка при формировании уведомления");
            }

            var notif = new CreditOrgDecisionNotification
            {
                BankAccountNumber = proxy.CreditOrgDecision.BankAccountNumber,
                BankFile = proxy.CreditOrgDecision.BankFile,
                CreditOrg = proxy.CreditOrgDecision.Decision,
                OwnerType = proxy.AccountOwnerDecision.DecisionType
            };

            return JsSuccess(notif);
        }

        private ManOrgContractRealityObject GetCurrentManOrgContract(RealityObject ro)
        {
            return Container.ResolveDomain<ManOrgContractRealityObject>().GetAll()
                .Where(x => x.RealityObject.Id == ro.Id)
                .Where(x => x.ManOrgContract.StartDate <= DateTime.Now)
                .Where(x => !x.ManOrgContract.EndDate.HasValue || x.ManOrgContract.EndDate >= DateTime.Now)
                .OrderByDescending(x => x.ManOrgContract.StartDate)
                .FirstOrDefault();
        }

        private IDataResult ValidateBeforeNotification(UltimateDecisionProxy proxy)
        {
            if(proxy.CrFundFormationDecision == null
               ||
               proxy.CrFundFormationDecision.Decision != CrFundFormationDecisionType.SpecialAccount)
            {
                return new BaseDataResult(false, "Уведомление формируется при принятии решения о специальном счете");
            }

            if(proxy.CreditOrgDecision == null ||
               proxy.CreditOrgDecision.Decision == null)
            {
                return new BaseDataResult(false, "Уведомление формируется при принятии решения о выборе кредитной организации");
            }

            return new BaseDataResult(true);
        }

        private void SaveOrUpdateInternal(BaseParams baseParams, UltimateDecisionProxy proxy)
        {
            var properties = proxy.GetType().GetProperties();

            dynamic protocol = null;
            var protocolProperty = properties.FirstOrDefault(x => typeof(RealityObjectDecisionProtocol) == x.PropertyType);

            if(protocolProperty != null)
            {
                properties = properties.Except(new[] {protocolProperty}).ToArray();
                protocol = protocolProperty.GetValue(proxy, new object[0]);

                if(protocol != null)
                {
                    var domain = Container.ResolveDomain<RealityObjectDecisionProtocol>();

                    using(Container.Using(domain))
                    {
                        var file = baseParams.Files.FirstOrDefault(x => x.Key == "File").Value;
                        if(file != null)
                        {
                            var fileManager = Container.Resolve<IFileManager>();

                            using(Container.Using(fileManager))
                            {
                                protocol.File = fileManager.SaveFile(file);
                            }
                        }

                        if(protocol.Id > 0)
                        {
                            domain.Update(protocol);
                        }
                        else
                        {
                            domain.Save(protocol);
                        }
                    }
                }
            }

            foreach(var property in properties)
            {
                var value = property.GetValue(proxy, new object[0]);

                if(value == null)
                {
                    continue;
                }

                var protocolProp =
                    value.GetType()
                        .GetProperties()
                        .FirstOrDefault(x => typeof(RealityObjectDecisionProtocol) == x.PropertyType);

                if(protocolProp != null)
                {
                    protocolProp.SetValue(value, protocol, new object[0]);
                }

                var id = value.GetType().GetProperty("Id").GetValue(value, new object[0]).To<long>();

                if(typeof(IEntity).IsAssignableFrom(property.PropertyType))
                {
                    var domainType = typeof(IDomainService<>).MakeGenericType(property.PropertyType);
                    var domain = Container.Resolve(domainType);

                    using(Container.Using(domain))
                    {
                        var fileProp =
                            value.GetType().GetProperties().FirstOrDefault(x => x.PropertyType == typeof(FileInfo));

                        var file = baseParams.Files.FirstOrDefault(x => x.Key == fileProp.Return(p => p.Name)).Value;

                        if(fileProp != null &&
                           file != null)
                        {
                            fileProp.SetValue(value, Container.Resolve<IFileManager>().SaveFile(file), new object[0]);
                        }

                        var isCheckedVal =
                            value.GetType().GetProperty("IsChecked").GetValue(value, new object[0]).ToStr();

                        var isChecked = isCheckedVal.ToBool() || isCheckedVal.ToUpperInvariant() == "ON";

                        if(!isChecked)
                        {
                            if(id > 0)
                            {
                                var delete = domain.GetType().GetMethod("Delete", new[] {typeof(long)});
                                delete.Invoke(domain, new object[] {id});
                            }

                            continue;
                        }

                        if(id > 0)
                        {
                            var update = domain.GetType().GetMethod("Update", new[] {property.PropertyType});
                            update.Invoke(domain, new[] {value});
                        }
                        else
                        {
                            var save = domain.GetType().GetMethod("Save", new[] {property.PropertyType});
                            save.Invoke(domain, new[] {value});
                        }
                    }
                }
            }
        }
        
        public ActionResult RealityObjectsOnSpecialAccount()
        {
            var service = Container.Resolve<IRealityObjectDecisionsService>();

            using(Container.Using(service))
            {
                return new JsonListResult((IEnumerable)service.RealityObjectsOnSpecialAccount().Data);
            }
        }

        public ActionResult RealityObjectCreditOrgDecisions()
        {
            var service = Container.Resolve<IRealityObjectDecisionsService>();

            using(Container.Using(service))
            {
                return new JsonListResult((IEnumerable)service.RealityObjectCreditOrgDecisions().Data);
            }
        }
    }
}