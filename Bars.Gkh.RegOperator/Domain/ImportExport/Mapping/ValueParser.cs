namespace Bars.Gkh.RegOperator.Domain.ImportExport.Mapping
{
    using System;

    public class ValueParser
    {
        public object Parse(object obj)
        {
            return _converter(obj);
        }

        private readonly Func<object, object> _converter;

        public ValueParser(Func<object, object> func)
        {
            _converter = func;
        }
    }
}