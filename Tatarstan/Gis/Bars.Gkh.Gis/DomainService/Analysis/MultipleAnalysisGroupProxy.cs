namespace Bars.Gkh.Gis.DomainService.Analysis
{
    using System.Collections.Generic;
    using Controllers;

    public class MultipleAnalysisGroupProxy
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<MultipleAnalysisProxy> children { get; set; }

        public bool leaf
        {
            get { return false; }
        }
    }
}
