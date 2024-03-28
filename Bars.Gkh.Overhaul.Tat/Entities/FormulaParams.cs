namespace Bars.Gkh.Overhaul.Tat.Entities
{
    using Gkh.Entities.CommonEstateObject;
    using Overhaul.Entities;

    public class FormulaParams
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public StructuralElementGroupAttribute Attribute { get; set; }

        public string ValueResolverCode { get; set; }

        public string ValueResolverName { get; set; }
    }
}