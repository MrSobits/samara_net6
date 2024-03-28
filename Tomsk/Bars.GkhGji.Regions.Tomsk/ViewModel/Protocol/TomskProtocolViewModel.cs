namespace Bars.GkhGji.Regions.Tomsk.ViewModel
{
    using B4;
    using B4.Utils;
    using Controller;
    using DomainService;
    using Enums;
    using Gkh.Domain;
    using GkhGji.ViewModel;

    public class TomskProtocolViewModel : ProtocolViewModel<TomskProtocol>
    {
        public override IDataResult Get(IDomainService<TomskProtocol> domainService, BaseParams baseParams)
        {
            var obj = domainService.Get(baseParams.Params.GetAsId());

            if (obj == null)
            {
                return new BaseDataResult();
            }

            var physInfo = Container.Resolve<IDocumentPhysInfoService>().GetByDocument(obj);

            return new BaseDataResult(new
            {
                obj.Id,
                obj.Executant,
                obj.DocumentDate,
                obj.DocumentNum,
                obj.DocumentNumber,
                obj.LiteralNum,
                obj.DocumentSubNum,
                obj.DocumentYear,
                obj.Contragent,
                obj.DateOfProceedings,
                obj.DateToCourt,
                obj.Description,
                obj.HourOfProceedings,
                obj.MinuteOfProceedings,
                obj.Inspection,
                obj.PersonFollowConversion,
                obj.PhysicalPerson,
                obj.PhysicalPersonInfo,
                obj.Stage,
                obj.State,
                obj.ToCourt,
                obj.TypeDocumentGji,
                obj.DateOfViolation,
                obj.HourOfViolation,
                obj.MinuteOfViolation,
                PhysInfoId = physInfo.ReturnSafe(x => x.Id),
                PhysAddress = physInfo.ReturnSafe(x => x.PhysAddress),
                PhysJob = physInfo.ReturnSafe(x => x.PhysJob),
                PhysPosition = physInfo.ReturnSafe(x => x.PhysPosition),
                PhysBirthdayAndPlace = physInfo.ReturnSafe(x => x.PhysBirthdayAndPlace),
                PhysIdentityDoc = physInfo.ReturnSafe(x => x.PhysIdentityDoc),
                PhysSalary = physInfo.ReturnSafe(x => x.PhysSalary),
                TypeGender = physInfo.ReturnSafe(x => x.TypeGender)
            });
        }
    }
}