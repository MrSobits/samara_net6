namespace Bars.Gkh.RegOperator.Domain.ImportExport
{
    public class ImportRow<T>
    {
        public T Value { get; set; }

        public string Title { get; set; }

        public string Info { get; set; }

        public string Warning { get; set; }

        public string Error { get; set; }
    }
}