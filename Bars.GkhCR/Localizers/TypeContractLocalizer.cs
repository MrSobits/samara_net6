namespace Bars.GkhCr.Localizers
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.Application;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.Gkh.Entities.Dicts.Multipurpose;
    using Castle.Windsor;

    public static class TypeContractLocalizer
    {
        public const string GlossaryCode = "cr_contract_type";

        private static readonly IWindsorContainer Container = ApplicationContext.Current.Container;

        private static readonly Dictionary<string, string> _items = new Dictionary<string, string>
        {
            { "Psd", "ПСД" },
            { "Expertise", "Экспертиза" },
            { "TechSepervision", "Технический надзор" },
            { "Insurance", "Страхование" },
            { "RoMoAggreement", "Договор о функции заказчика между РО и МО" }
        };

        public static Dictionary<string, string> GetDefaultItems()
        {
            return _items;
        }

        public static bool IsDefault(string code)
        {
            return _items.ContainsKey(code);
        }

        public static Dictionary<string, string> GetAllItems()
        {
            var glossaryDomain = Container.ResolveDomain<MultipurposeGlossary>();
            using(Container.Using(glossaryDomain))
            {
                var glossary = glossaryDomain.FirstOrDefault(x => x.Code == GlossaryCode);
                if (glossary == null)
                {
                    return _items;
                }

                return glossary.Items.ToDictionary(x => x.Key, x => x.Value);
            }
        }
    }
}
