namespace Bars.Gkh.Overhaul.Tat.Interceptors
{
    using Bars.B4;
    using Bars.Gkh.Overhaul.Tat.ConfigSections;
    using Bars.Gkh.Utils;
    using Bars.GkhCr.Entities;
    using Bars.GkhCr.Enums;

    public class TypeWorkCrRemovalInterceptor : Bars.GkhCr.Interceptors.TypeWorkCrRemovalInterceptor
    {
        public override IDataResult BeforeCreateAction(
            IDomainService<TypeWorkCrRemoval> service,
            TypeWorkCrRemoval entity)
        {
            if (entity.TypeReason == TypeWorkCrReason.NewYear
                || entity.TypeReason == TypeWorkCrReason.NotRequiredShortProgram)
            {
                var config = Container.GetGkhConfig<OverhaulTatConfig>();
                var periodEnd = config.ProgrammPeriodEnd;

                if (entity.NewYearRepair.HasValue && entity.NewYearRepair.Value > periodEnd)
                {
                    return
                        Failure(
                            string.Format(
                                "Новый год выполнения работы не может превышать срок окончания долгосрочной программы: {0}",
                                periodEnd));
                }
            }

            return this.Success();
        }
    }
}