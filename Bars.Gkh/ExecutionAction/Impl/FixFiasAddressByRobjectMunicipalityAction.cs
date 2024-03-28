namespace Bars.Gkh.ExecutionAction.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.FIAS;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Import.FiasHelper;

    public class FixFiasAddressByRobjectMunicipalityAction : BaseExecutionAction
    {
        public ISessionProvider SessionProvider { get; set; }

        public override string Description
            => "Исправление адреса (guid и другие составляющие) согласно МО дома. Касается тех адресов, домам которых меняли МО через скрипты";

        public override string Name => "Исправление адреса согласно Мо дома";

        public override Func<IDataResult> Action => this.FixFiasAddressByRobjectMunicipality;

        public BaseDataResult FixFiasAddressByRobjectMunicipality()
        {
            var fiasHelper = this.Container.Resolve<IFiasHelper>();

            var addressesToFix = this.Container.Resolve<IDomainService<RealityObject>>().GetAll()
                .Where(x => x.Municipality != null)
                .Where(x => x.FiasAddress != null)
                .Select(x => new {x.Municipality.FiasId, x.FiasAddress})
                .AsEnumerable()
                .Where(x => !x.FiasAddress.AddressGuid.Contains(x.FiasId))
                .ToList();

            var streetKladrCodeByAoGuid = this.Container.Resolve<IRepository<Fias>>().GetAll()
                .Where(x => x.ActStatus == FiasActualStatusEnum.Actual)
                .Where(
                    x => x.AOLevel == FiasLevelEnum.Street || x.AOLevel == FiasLevelEnum.Extr || x.AOLevel == FiasLevelEnum.Sext
                        || x.AOLevel == FiasLevelEnum.PlanningStruct)
                .Where(x => x.ParentGuid != null && x.ParentGuid != string.Empty)
                .Select(
                    x => new FiasProxyWithKladr
                    {
                        AoGuid = x.AOGuid,
                        KladrCode = x.KladrCode,
                        KladrCurrStatus = x.KladrCurrStatus
                    })
                .AsEnumerable()
                .GroupBy(x => x.AoGuid)
                .ToDictionary(x => x.Key, x => x.OrderBy(y => y.KladrCurrStatus).First().KladrCode);

            var fiasAddressesToUpdateList = new List<FiasAddress>();
            var streetNotFoundErrorCount = 0;
            var findInBranchErrorCount = 0;

            foreach (var addressToFix in addressesToFix)
            {
                if (streetKladrCodeByAoGuid.ContainsKey(addressToFix.FiasAddress.StreetGuidId))
                {
                    var streetKladrCode = streetKladrCodeByAoGuid[addressToFix.FiasAddress.StreetGuidId];

                    var faultReason = string.Empty;
                    DynamicAddress dynamicAddress;

                    if (fiasHelper.FindInBranchByKladr(addressToFix.FiasId, streetKladrCode, ref faultReason, out dynamicAddress))
                    {
                        var newAddress = fiasHelper.CreateFiasAddress(
                            dynamicAddress,
                            addressToFix.FiasAddress.House,
                            addressToFix.FiasAddress.Letter,
                            addressToFix.FiasAddress.Housing,
                            addressToFix.FiasAddress.Building);

                        addressToFix.FiasAddress.AddressGuid = newAddress.AddressGuid;
                        addressToFix.FiasAddress.AddressName = newAddress.AddressName;
                        addressToFix.FiasAddress.PlaceAddressName = newAddress.PlaceAddressName;
                        addressToFix.FiasAddress.PlaceCode = newAddress.PlaceCode;
                        addressToFix.FiasAddress.PlaceGuidId = newAddress.PlaceGuidId;
                        addressToFix.FiasAddress.PlaceName = newAddress.PlaceName;
                        addressToFix.FiasAddress.ObjectEditDate = DateTime.Now;
                        addressToFix.FiasAddress.ObjectVersion++;

                        fiasAddressesToUpdateList.Add(addressToFix.FiasAddress);
                    }
                    else
                    {
                        ++findInBranchErrorCount;
                    }
                }
                else
                {
                    ++streetNotFoundErrorCount;
                }
            }

            this.SaveData(fiasAddressesToUpdateList);

            return new BaseDataResult(
                new
                {
                    UpdatedRows = fiasAddressesToUpdateList.Count,
                    StreetNotFoundErrorCount = streetNotFoundErrorCount,
                    FindInBranchErrorCount = findInBranchErrorCount
                });
        }

        private void SaveData(List<FiasAddress> fiasAddressesToUpdate)
        {
            this.SessionProvider.CloseCurrentSession();

            using (var session = this.SessionProvider.OpenStatelessSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    try
                    {
                        fiasAddressesToUpdate.ForEach(session.Update);

                        transaction.Commit();
                    }
                    catch (Exception exc)
                    {
                        try
                        {
                            transaction.Rollback();
                        }
                        catch (Exception ex)
                        {
                            throw new Exception(ex.Message, exc);
                        }
                    }
                }
            }
        }
    }
}