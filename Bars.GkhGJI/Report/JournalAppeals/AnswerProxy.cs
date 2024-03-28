namespace Bars.GkhGji.Report
{
    public class AnswerProxy
    {
        public long AppealCitsId { get; set; }

        public string DocumentNumber { get; set; }

        public bool IsDocumentNumber
        {
            get
            {
                return !string.IsNullOrEmpty(DocumentNumber);
            }
        }

        public string DocumentDate { get; set; }

        public int Count { get; set; }
    }
}
