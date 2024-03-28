namespace Bars.Gkh.Report
{
    using System;
    using System.Text;

    using B4.Utils;

    public class UserParamsValues
    {
        private DynamicDictionary values = new DynamicDictionary();

        public int Count
        {
            get { return values.Count; }
        }

        public DynamicDictionary Values
        {
            get { return values; }
            set { values = value; }
        }

        public object this[string name]
        {
            get
            {
                return values.ContainsKey(name) ? values[name] : null;
            }

            set
            {
                if (values.ContainsKey(name))
                {
                    values[name] = value;
                }
                else
                {
                    values.Add(name, value);
                }
            }
        }

        public object GetValue(string name)
        {
            return this[name];
        }

        public T GetValue<T>(string name, Func<object, T> valueCast)
        {
            return valueCast(this[name]);
        }

        public T GetValue<T>(string name)
        {
            return (T)this[name];
        }

        public void AddValue(string name, object value)
        {
            values.Add(name, value);
        }

        public override string ToString()
        {
            var stringBuilder = new StringBuilder();

            foreach (var keyValuePair in values)
            {
                stringBuilder.AppendFormat("{0}:{1}, ", keyValuePair.Key, keyValuePair.Value);
            }

            return stringBuilder.ToString().TrimEnd(',', ' ');
        }
    }
}