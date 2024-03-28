using Bars.GkhGji.Contracts;

namespace Bars.GkhGji.Regions.Tyumen.TextValues
{
    public class DisposalText : IDisposalText
    {
        public string SubjectiveCase 
        { 
            get { return "Приказ"; } 
        }

        public string GenetiveCase 
        {
            get { return "Приказа"; } 
        }

        public string DativeCase 
        {
            get { return "Приказу"; }
        }

        public string AccusativeCase 
        {
            get { return "Приказ"; } 
        }

        public string InstrumentalCase 
        {
            get { return "Приказом"; } 
        }

        public string PrepositionalCase 
        {
            get { return "О приказе"; }
        }

        public string SubjectiveManyCase
        {
            get { return "Приказы"; }
        }

        public string GenetiveManyCase
        {
            get { return "Приказов"; }
        }
    }
}