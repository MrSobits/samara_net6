namespace Bars.GkhGji.Regions.Chelyabinsk.Interceptors
{
    using B4;
    using B4.Modules.States;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Regions.Chelyabinsk.Entities;
    using Entities;
    using System;
    using System.Linq;
    using System.Text;

    class AppCitAdmonVoilationInterceptor : EmptyDomainInterceptor<AppCitAdmonVoilation>
    {

        public IDomainService<AppealCitsAdmonitionLongText> AppealCitsAdmonitionLongTextDomain { get; set; }
        public IDomainService<ViolationGji> ViolationGjiDomain { get; set; }

        public override IDataResult AfterCreateAction(IDomainService<AppCitAdmonVoilation> service, AppCitAdmonVoilation entity)
        {
            try
            {
                var violation = ViolationGjiDomain.Get(entity.ViolationGji.Id);

                var violtext = $"{violation.Name} ведет к нарушению обязательных требований, установленных {violation.NormativeDocNames}.";
                var plt = AppealCitsAdmonitionLongTextDomain.GetAll().FirstOrDefault(x => x.AppealCitsAdmonition == entity.AppealCitsAdmonition);
                if (plt != null)
                {
                    plt.Violations = Encoding.UTF8.GetBytes(violtext);
                    AppealCitsAdmonitionLongTextDomain.Update(plt);
                }
                else
                {
                    AppealCitsAdmonitionLongTextDomain.Save(new AppealCitsAdmonitionLongText
                    {
                        AppealCitsAdmonition = entity.AppealCitsAdmonition,
                        Violations = Encoding.UTF8.GetBytes(violtext)
                    });
                }


            }
            catch (Exception e)
            {

            }
            return Success();
        }

    }
}
