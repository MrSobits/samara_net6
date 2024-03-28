namespace Bars.Gkh.Overhaul.Nso.Interceptors
{
    using System.Linq;
    using B4;
    using B4.Utils;

    using Bars.Gkh.ConfigSections.Cr;
    using Bars.Gkh.ConfigSections.Cr.Enums;
    using Bars.Gkh.Utils;

    using DomainService;
    using GkhCr.Entities;
    using GkhCr.Enums;

    public class TypeWorkCrInterceptor : EmptyDomainInterceptor<TypeWorkCr>
    {
        public IDomainService<DefectList> DefectListDomain { get; set; }

        public override IDataResult BeforeCreateAction(IDomainService<TypeWorkCr> service, TypeWorkCr entity)
        {
            return CheckVolumeAndSum(service, entity);
        }

        public override IDataResult BeforeUpdateAction(IDomainService<TypeWorkCr> service, TypeWorkCr entity)
        {

            return CheckVolumeAndSum(service, entity);
        }

        private IDataResult CheckVolumeAndSum(IDomainService<TypeWorkCr> service, TypeWorkCr entity)
        {
            var typeWorkVersSt1Service = Container.Resolve<ITypeWorkStage1Service>();

            try
            {
                var typeWorkSum = service.GetAll()
                                .Where(x => x.ObjectCr.Id == entity.ObjectCr.Id && x.Work.Id == entity.Work.Id && x.Id != entity.Id)
                                .Select(x => x.Sum)
                                .Sum();

                var defectListSum = DefectListDomain.GetAll()
                            .Where(x => x.TypeDefectList.HasValue)
                            .Where(x => x.TypeDefectList.Value == TypeDefectList.Contractor || x.TypeDefectList.Value == TypeDefectList.UnexpectedExpenses)
                               .Where(x => x.ObjectCr.Id == entity.ObjectCr.Id && x.Work.Id == entity.Work.Id)
                               .Select(x => x.Sum)
                               .Sum();

                var versStage1 = typeWorkVersSt1Service.GetTypeWorkStage1(entity);

                var config = Container.GetGkhConfig<GkhCrConfig>();
                var typeCheckWork = config.DpkrConfig.TypeCheckWork;
                var typeVolumeChecking = config.General.TypeVolumeChecking;
                var useTypeDefectList = config.General.DefectListUsage == DefectListUsage.Use;

                if (useTypeDefectList && typeCheckWork == TypeCheckWork.WithDefectList
                    && typeWorkSum.ToDecimal() + entity.Sum.ToDecimal() < defectListSum.ToDecimal())
                {
                    return Failure(" Стоимость работы в краткосрочной программе не должна превышать стоимость по ведомости!");
                }

                if (typeVolumeChecking == TypeChecking.Check && (versStage1 == null || entity.Volume > versStage1.Stage1Version.StructuralElement.Volume))
                {
                    return Failure("Объем в краткосрочной программе не должен превышать объем конструктивного элемента!"
                                   + " Объем конструктивного элемента указан в паспорте жилого дома, в разделе \"Конструктивные характеристики.\"");
                }

                return this.Success();
            }
            finally
            {
                Container.Release(typeWorkVersSt1Service);
            }
        }
    }
}