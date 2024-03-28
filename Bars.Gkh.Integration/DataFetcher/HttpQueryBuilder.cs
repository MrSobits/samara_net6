namespace Bars.Gkh.Integration.DataFetcher
{
    using System.Text;
    using B4.Utils;

    public class HttpQueryBuilder
    {
        private bool _noParameters;
        private readonly StringBuilder _builder;

        public string Address { get; set; }

        public HttpQueryBuilder(string baseUrl)
        {
            _noParameters = true;
            _builder = new StringBuilder(baseUrl);
        }

        public HttpQueryBuilder AddParameter(string key, object value)
        {
            AddPlaceHolder();

            _builder.AppendFormat("{0}={1}", key, value);

            return this;
        }

        public HttpQueryBuilder AddDictionary(string key, DynamicDictionary args)
        {
            AddPlaceHolder();

            _builder.AppendFormat("{0}=", key);
            _builder.Append("{");

            foreach (var kv in args)
            {
                _builder.Append("'{0}':'{1}',".FormatUsing(kv.Key, kv.Value));
            }

            _builder.Append("}");

            return this;
        }

        private void AddPlaceHolder()
        {
            if (_noParameters)
            {
                _builder.Append("?");
                _noParameters = false;
                return;
            }

            _builder.Append("&");
        }

        public override string ToString()
        {
            return _builder.ToString();
        }
    }
}