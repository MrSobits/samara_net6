namespace Bars.Gkh.RegOperator.Regions.Tatarstan.ViewModels
{
    using System.Linq;

    using B4;
    using B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Enums;
    using Bars.GkhRf.Entities;

    using Entities;
    using Gkh.Domain;

    public class TransferRfRecordViewModel : BaseViewModel<TransferRfRecord>
    {
        public override IDataResult List(IDomainService<TransferRfRecord> domain, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);
            var transferRfId = baseParams.Params.GetAsId("transferRfId");

            var transferRecordDomain = Container.ResolveDomain<TransferRfRecord>();
            var transferObjDomain = Container.ResolveDomain<TransferObject>();
            var transferHireDomain = Container.ResolveDomain<TransferHire>();
            var basePersonalAccountDomain = Container.ResolveDomain<BasePersonalAccount>();

            using (Container.Using(
                transferRecordDomain,
                transferObjDomain,
                transferHireDomain,
                basePersonalAccountDomain))
            {

                //Сведения о перечислениях по договору
                var transferRfRecordsId = transferRecordDomain
                    .GetAll()
                    .Where(x => x.TransferRf.Id == transferRfId)
                    .Select(x => x.Id);

                //Реестр жилых домов включенных в договор                
                var realityObjectIds = transferObjDomain
                    .GetAll()
                    .Where(x => transferRfRecordsId.Contains(x.TransferRecord.Id))
                    .Select(x => x.RealityObject.Id);

                var overhaulTypePersAccRoIds =
                    basePersonalAccountDomain.GetAll()
                        .Where(x => x.Room != null && x.Room.RealityObject != null)
                        .Where(x => realityObjectIds.Contains(x.Room.RealityObject.Id))
                        .Select(x => x.Room.RealityObject.Id);

                //словарь сведение перечисления - перечисленная сумма
                var transferObjectDict = transferObjDomain.GetAll()
                    .Where(x => transferRfRecordsId.Contains(x.TransferRecord.Id))
                    .Where(x => overhaulTypePersAccRoIds.Contains(x.RealityObject.Id))
                    .GroupBy(x => x.TransferRecord.Id)
                    .ToDictionary(x => x.Key, y => y.Sum(x => x.TransferredSum));

                //словарь перечисление по найму - перечисленная сумма
                var transferHireDict = transferHireDomain.GetAll()
                    .Where(x => transferRfRecordsId.Contains(x.TransferRecord.Id))
                    .Where(x =>
                        x.Account.ServiceType == PersAccServiceType.Recruitment
                        ||
                        x.Account.ServiceType == PersAccServiceType.OverhaulRecruitment)
                    .GroupBy(x => x.TransferRecord.Id)
                    .ToDictionary(x => x.Key, y => y.Sum(x => x.TransferredSum));

                var data = Container.Resolve<IDomainService<ViewTransferRfRecord>>().GetAll()
                    .Where(x => x.TransferRf.Id == transferRfId)
                    .Select(x => new
                    {
                        x.Id,
                        x.Description,
                        x.DateFrom,
                        x.DocumentName,
                        x.DocumentNum,
                        x.State,
                        x.TransferDate,
                        x.File,
                        x.CountRecords,
                        SumRecords = (transferObjectDict.ContainsKey(x.Id)
                            ? transferObjectDict[x.Id]
                            : 0M) +
                                     (transferHireDict.ContainsKey(x.Id)
                                         ? transferHireDict[x.Id]
                                         : 0M)
                    })
                    .Filter(loadParams, Container);

                return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
            }
        }
    }
}