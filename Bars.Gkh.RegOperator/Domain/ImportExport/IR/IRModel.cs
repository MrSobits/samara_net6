namespace Bars.Gkh.RegOperator.Domain.ImportExport.IR
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;
    using B4.Utils;

    [DebuggerDisplay("ModelName: {ModelName}, Path: {Path}, Children: {Children.Count}")]
    [Serializable]
    public class IRModel
    {
        private List<IRModelProperty> _propertyBag;
        private List<IRModel> _children;
        private string _debugView = null;

        private Dictionary<string, IRModelProperty> _internalPropertyBag; 

        public string ModelName { get; set; }

        public string Path { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<IRModelProperty> PropertyBag
        {
            get { return _propertyBag ?? (_propertyBag = new List<IRModelProperty>()); }
            set { _propertyBag = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public List<IRModel> Children
        {
            get { return _children ?? (_children = new List<IRModel>()); }
            set { _children = value; }
        }

        public IRModel()
        {
            Children = new List<IRModel>();
        }

        /// <summary>
        /// Получение значения свойства по первому совпавшему ключу
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public object GetAnyPropertyBagValue(params string[] key)
        {
            if (_internalPropertyBag.IsNull())
            {
                _internalPropertyBag = new Dictionary<string, IRModelProperty>();
                PropertyBag.ForEach(x => _internalPropertyBag[x.Name] = x);
            }

            foreach (var s in key)
            {
                IRModelProperty value;

                var valueKey = _internalPropertyBag.Keys.FirstOrDefault(x => x.EqualsIgnoreCase(s));

                if (valueKey != null && _internalPropertyBag.TryGetValue(valueKey, out value))
                    return value.Value;
            }

            return null;
        }

        /// <summary>
        /// Получение первой пары ключ-значение по первому совпавшему ключу
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public IRModelProperty GetAnyPropertyBagPair(params string[] key)
        {
            foreach (var s in key)
            {
                IRModelProperty value;
                if (_internalPropertyBag.TryGetValue(s, out value))
                    return value;
            }

            return default(IRModelProperty);
        }

        /// <summary>
        /// Отладочная информация
        /// </summary>
        public string DebugView()
        {
            if (_debugView.IsNotNull())
            {
                return _debugView;
            }

            var sb = new StringBuilder();

            foreach (var kv in PropertyBag)
            {
                sb.AppendFormat("<{0}, {1}>|", kv.Name, kv.Value);
            }

            _debugView = sb.ToString().TrimEnd('|');

            return _debugView;
        }
    }

    public class IRModelProperty
    {
        public string Name { get; set; }

        public object Value { get; set; }

        public Type Type { get; set; }

        public int NLength { get; set; }

        public int DLength { get; set; }

        public Func<object, object> ValueParser { get; set; }
    }
}