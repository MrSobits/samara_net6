namespace Bars.B4.Modules.Analytics.Filters
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// 
    /// </summary>
    public class MacrosContainer : IMacrosContainer
    {
        private readonly IDictionary<string, IMacros> _container = new Dictionary<string, IMacros>();

        public void Register(IMacros macros)
        {
            // TODO normalize key
            var key = macros.Key;
            if (_container.ContainsKey(macros.Key))
            {
                // TODO warning
            }
            _container.Add(key, macros);
        }

        public IMacros Get(string key)
        {
            if (!_container.ContainsKey(key))
            {
                throw new MacrosNotRegisteredException(string.Format("Macros with {0} key not registered.", key));
            }

            // TODO normalize key
            return _container[key];
        }
    }


    public class MacrosNotRegisteredException : Exception
    {
        public MacrosNotRegisteredException(string message) : base(message) { }
    }
}
