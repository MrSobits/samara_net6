namespace Bars.GkhGji.Regions.Nso.TextValues
{
    using Bars.GkhGji.Contracts;

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