namespace Bars.Gkh.Overhaul.Tat.DomainService.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities.Dicts;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Overhaul.Tat.Entities;
    using Bars.Gkh.Overhaul.Tat.Enum;

    using Castle.Windsor;

    public class PropertyOwnerDecisionWorkService : IPropertyOwnerDecisionWorkService
    {
        public IWindsorContainer Container { get; set; }

        public IDataResult AddWorks(BaseParams baseParams)
        {
            try
            {
                var propertyOwnerDecisionId = baseParams.Params["propertyOwnerDecisionId"].ToLong();

                if (!string.IsNullOrEmpty(baseParams.Params["objectIds"].ToStr()))
                {
                    var objectIds = baseParams.Params["objectIds"].ToStr().Split(',');
                    var service = Container.Resolve<IDomainService<PropertyOwnerDecisionWork>>();

                    // получаем у контроллера работы что бы не добавлять их повторно
                    var existingPropertyOwnerDecisionWorks =
                        service.GetAll()
                            .Where(x => x.Decision.Id == propertyOwnerDecisionId)
                            .Select(x => x.Work.Id)
                            .Distinct()
                            .ToList();

                    foreach (var id in objectIds.Select(x => x.ToLong()))
                    {
                        if (existingPropertyOwnerDecisionWorks.Contains(id))
                        {
                            continue;
                        }

                        var newFinanceSourceWork = new PropertyOwnerDecisionWork
                        {
                            Work = new Work { Id = id },
                            Decision = new SpecialAccountDecision { Id = propertyOwnerDecisionId }
                        };

                        service.Save(newFinanceSourceWork);
                    }
                }

                return new BaseDataResult { Success = true };
            }
            catch (ValidationException exc)
            {
                return new BaseDataResult
                {
                    Success = false,
                    Message = exc.Message
                };
            }
        }

        public IDataResult PropertyOwnerDecisionTypeList(BaseParams baseParams)
        {

            var service = Container.ResolveAll<IAuthorizationService>().FirstOrDefault();

            if (service == null)
            {
                throw new ArgumentNullException();
            }

            var userIdentity = Container.Resolve<IUserIdentity>();

            var permissionList = new List<PropertyOwnerDecisionTypeProxy>();
            const string prefix = "Gkh.RealityObject.Register.OwnerProtocol.PropertyOwnerDecision.PropertyOwnerDecisionType.";

            foreach (PropertyOwnerDecisionType propertyOwnerDecisionType in System.Enum.GetValues(typeof(PropertyOwnerDecisionType)))
            {
                permissionList.Add(new PropertyOwnerDecisionTypeProxy
                    {
                        Id = (int)propertyOwnerDecisionType,
                        Name = propertyOwnerDecisionType.GetEnumMeta().Display,
                        PermissionName = prefix + propertyOwnerDecisionType.ToString()
                    });
            }

            var data = permissionList
                .Where(x => service.Grant(userIdentity, x.PermissionName))
                .Select(x => new { x.Id, x.Name })
                .ToList();

            return new ListDataResult(data, data.Count());
        }

        public IDataResult MethodFormFundCrTypeList(BaseParams baseParams)
        {

            var service = Container.ResolveAll<IAuthorizationService>().FirstOrDefault();

            if (service == null)
            {
                throw new ArgumentNullException();
            }

            var userIdentity = Container.Resolve<IUserIdentity>();

            var permissionList = new List<MethodFormFundCrTypeProxy>();
            const string permission = "Gkh.RealityObject.Register.OwnerProtocol.PropertyOwnerDecision.FundFormType.SpecAcc";

            foreach (MethodFormFundCr fundFormType in Enum.GetValues(typeof(MethodFormFundCr)))
            {
                if (fundFormType == MethodFormFundCr.NotSet)
                {
                    continue;
                }

                if ((int)fundFormType != 20 || service.Grant(userIdentity, permission))
                {
                    permissionList.Add(new MethodFormFundCrTypeProxy
                    {
                        Value = (int)fundFormType,
                        Display = fundFormType.GetEnumMeta().Display
                    });
                }
            }

            var data = permissionList
                .Select(x => new { x.Value, x.Display})
                .ToList();

            return new ListDataResult(data, data.Count());
        }
    }

    internal class PropertyOwnerDecisionTypeProxy
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PermissionName { get; set; }
    }

    internal class MethodFormFundCrTypeProxy
    {
        public int Value { get; set; }
        public string Display { get; set; }
        public string PermissionName { get; set; }
    }
}