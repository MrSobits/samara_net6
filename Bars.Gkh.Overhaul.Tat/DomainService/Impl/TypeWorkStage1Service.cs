namespace Bars.Gkh.Overhaul.Tat.DomainService.Impl
{
    using Bars.Gkh.ConfigSections.Cr;
    using Bars.Gkh.ConfigSections.Cr.Enums;
    using Bars.Gkh.Utils;
    using System.Linq;
    using B4;
    using Castle.Windsor;
    using Entities;
    using Gkh.Entities.Dicts;
    using GkhCr.Entities;

    public class TypeWorkStage1Service : ITypeWorkStage1Service
    {
        public  IWindsorContainer Container { get; set; }

        public IDomainService<TypeWorkCrVersionStage1> TypeWorkSt1Domain { get; set; }

        public TypeWorkCrVersionStage1 GetTypeWorkStage1(TypeWorkCr typeWork)
        {
            var versRecs = TypeWorkSt1Domain.GetAll().Where(x => x.TypeWorkCr.Id == typeWork.Id).ToArray();
            var dpkrType = Container.GetGkhConfig<GkhCrConfig>().DpkrConfig.DisplayDataByDpkr;
            return versRecs.Any()
                       ? new TypeWorkCrVersionStage1
                             {
                                 TypeWorkCr = versRecs.First().TypeWorkCr,
                                 Stage1Version = versRecs.First().Stage1Version,
                                 CalcBy = versRecs.First().CalcBy,
                                 UnitMeasure = versRecs.First().UnitMeasure,
                                 Volume = dpkrType == DisplayDataByDpkr.Previous ? versRecs.Sum(x => x.Volume) : versRecs.Sum(x => x.Stage1Version.Volume),
                                 Sum = dpkrType == DisplayDataByDpkr.Previous ? versRecs.Sum(x => x.Sum) : versRecs.Sum(x => x.Stage1Version.Sum)
                             }
                       : null;
        }

        public TypeWorkCrVersionStage1 GetTypeWorkStage1(Work work, ObjectCr objectCr)
        {
            var typeWorkCrService = Container.Resolve<IDomainService<TypeWorkCr>>();

            // получаем вид работ по типу работ дефектоной ведомости
            var typeWorkCrQuery =
                typeWorkCrService.GetAll().Where(x => x.Work.Id == work.Id && x.ObjectCr.Id == objectCr.Id);

            var versRecs = TypeWorkSt1Domain.GetAll().Where(x => typeWorkCrQuery.Any(y => y.Id == x.TypeWorkCr.Id)).ToArray();
            var dpkrType = Container.GetGkhConfig<GkhCrConfig>().DpkrConfig.DisplayDataByDpkr;
            return versRecs.Any()
                       ? new TypeWorkCrVersionStage1
                       {
                           TypeWorkCr = versRecs.First().TypeWorkCr,
                           Stage1Version = versRecs.First().Stage1Version,
                           CalcBy = versRecs.First().CalcBy,
                           UnitMeasure = versRecs.First().UnitMeasure,
                           Volume = dpkrType == DisplayDataByDpkr.Previous ? versRecs.Sum(x => x.Volume) : versRecs.Sum(x => x.Stage1Version.Volume),
                           Sum = dpkrType == DisplayDataByDpkr.Previous ? versRecs.Sum(x => x.Sum) : versRecs.Sum(x => x.Stage1Version.Sum)
                       } : null;
        }
    }
}