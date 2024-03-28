namespace Bars.GkhEdoInteg.Serialization
{
    using System.Collections.Generic;

    public sealed class SendEdo
    {
        public SendEdo()
        {
            Documents = new Dictionary<string, Document>();
            Answers = new Dictionary<string, Answer>();
        }

        public int Id { get; set; }

        public string Number { get; set; }

        public int SuretyId { get; set; }

        public string Surety { get; set; }

        public int VerifierId { get; set; }

        public string Verifier { get; set; }

        public Dictionary<string, Document> Documents { get; set; }

        public Dictionary<string, Answer> Answers { get; set; }
    }
}
