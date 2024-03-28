namespace Bars.Gkh.Entities.CommonEstateObject
{
    /// <summary>
    /// Параметр формулы
    /// </summary>
    public class FormulaParams
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public StructuralElementGroupAttribute Attribute { get; set; }

        public string ValueResolverCode { get; set; }

        public string ValueResolverName { get; set; }
    }
}