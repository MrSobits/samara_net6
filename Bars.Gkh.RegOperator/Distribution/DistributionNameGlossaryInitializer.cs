namespace Bars.Gkh.RegOperator.Distribution
{
    using System.Linq;
    using Bars.B4;
    using Bars.B4.Application;
    using Bars.B4.DataAccess;
    using Bars.B4.Events;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities.Dicts.Multipurpose;

    using Castle.MicroKernel.Lifestyle;
    using Castle.Windsor;

    using NHibernate.Exceptions;

    public class DistributionNameGlossaryInitializer : EventHandlerBase<AppStartEventArgs>
    {
        private readonly IDomainService<MultipurposeGlossary> _glossaryDomainService;
        private readonly IDomainService<MultipurposeGlossaryItem> _glossaryItemDomainService;

        public IWindsorContainer Container { get; set; }

        public DistributionNameGlossaryInitializer()
        {
            _glossaryDomainService = ApplicationContext.Current.Container.ResolveDomain<MultipurposeGlossary>();
            _glossaryItemDomainService = ApplicationContext.Current.Container.ResolveDomain<MultipurposeGlossaryItem>();
        }

        private const string GlossaryCode = DistributionNameLocalizer.DistributionNameGlossaryCode;
        private const string GlossaryName = "Типы распределения счетов НВС";

        public override void OnEvent(AppStartEventArgs args)
        {
            using (this.Container.BeginScope())
            {
                try
                {
                    var glossary = _glossaryDomainService.FirstOrDefault(x => x.Code == GlossaryCode);
                    if (glossary == null)
                    {
                        CreateGlossary();
                    }
                    else
                    {
                        UpdateGlossary(glossary);
                    }
                }
                catch (GenericADOException)
                {
                    // TODO: Log
                }
            }
        }

        private void CreateGlossary()
        {
            var glossary = new MultipurposeGlossary(GlossaryCode, GlossaryName);
            ApplicationContext.Current.Container.ResolveAll<IDistribution>().ForEach(x => glossary.AddItem(x.Code, x.Name));

            _glossaryDomainService.Save(glossary);
        }

        private void UpdateGlossary(MultipurposeGlossary glossary)
        {
            if (glossary.Items.Count() == 0)
            {
                _glossaryItemDomainService.GetAll().Where(x => x.Glossary == glossary).ToList().ForEach(x =>
                {
                    glossary.AddItem(x.Key, x.Value);
                });
            }
            ApplicationContext.Current.Container.ResolveAll<IDistribution>().ForEach(x =>
            {
                if (glossary.Items.All(i => i.Key != x.Code))
                {
                    glossary.AddItem(x.Code, x.Name);
                }
            });

            _glossaryDomainService.Update(glossary);
        }
    }
}
