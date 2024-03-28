namespace Bars.Gkh.RegOperator.Domain.ImportExport.Mapping
{
    using System;
    using B4.Utils.Annotations;

    public class ProviderMapper
    {
        private Lookuper _lookuper;

        public void SetLookuper(Lookuper lookuper)
        {
            ArgumentChecker.NotNull(lookuper, "lookuper");

            _lookuper = lookuper;
        }

        public void SetLookuper(string path)
        {
            ArgumentChecker.NotNull(path, "path");

            _lookuper = new Lookuper(path);
        }

        public void SetValueParser(Func<object, object> parser)
        {
            Parser = new ValueParser(parser);
        }

        public Lookuper Lookuper
        {
            get { return _lookuper; }
            set { _lookuper = value; }
        }

        public ValueParser Parser { get; private set; }

        public int NLength { get; set; }

        public int DLength { get; set; }
    }
}