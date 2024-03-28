namespace Bars.GkhGji.Regions.Stavropol.TextValues
{
    using Bars.GkhGji.Contracts;

    public class DisposalText : IDisposalText
    {
        public string SubjectiveCase 
        { 
            get { return "Распоряжение"; } 
        }

        public string GenetiveCase 
        {
            get { return "Распоряжения"; } 
        }

        public string DativeCase 
        {
            get { return "Распоряжению"; }
        }

        public string AccusativeCase 
        {
            get { return "Распоряжение"; } 
        }

        public string InstrumentalCase 
        {
            get { return "Распоряжением"; } 
        }

        public string PrepositionalCase 
        {
            get { return "О распоряжении"; }
        }

        public string SubjectiveManyCase
        {
            get { return "Распоряжения"; }
        }

        public string GenetiveManyCase
        {
            get { return "Распоряжений"; }
        }
    }
}