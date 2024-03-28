namespace Bars.Gkh.Overhaul.Nso.Interceptors
{
    using Bars.B4;
    using Bars.Gkh.Overhaul.Nso.ConfigSections;
    using Bars.GkhCr.Entities;
    using Bars.GkhCr.Enums;
    
    using Gkh.Utils;

    public class TypeWorkCrRemovalInterceptor : GkhCr.Interceptors.TypeWorkCrRemovalInterceptor
    {

        public override IDataResult BeforeCreateAction(IDomainService<TypeWorkCrRemoval> service, TypeWorkCrRemoval entity)
        {
            if (entity.TypeReason == TypeWorkCrReason.NewYear || entity.TypeReason == TypeWorkCrReason.NotRequiredShortProgram)
            {
                var periodEnd = Container.GetGkhConfig<OverhaulNsoConfig>().ProgrammPeriodEnd;

                if (entity.NewYearRepair.HasValue && entity.NewYearRepair.Value > periodEnd)
                {
                    return Failure(string.Format("Новый год выполнения работы не может превышать срок окончания долгосрочной программы: {0}", periodEnd));
                }
            }

            return this.Success();
        }
    }
}