namespace Bars.Gkh.Qa.Utils
{
    using System;
    using System.Collections.Generic;

    using Bars.B4;

    using Castle.Windsor;

    using TechTalk.SpecFlow;

    [Binding]
    public class BindingBase
    {
        protected static IWindsorContainer Container { get; set; }

        protected class DomainServiceCashe<TCurrent> : IDisposable
        {
            private Dictionary<Type, IDomainService> _services = new Dictionary<Type, IDomainService>();

            public IDomainService<TCurrent> Current
            {
                get { return this.Get<TCurrent>(); }
            }
            
            public IDomainService<T> Get<T>()
            {
                IDomainService ret;
                if (!this._services.TryGetValue(typeof(T), out ret))
                {
                    ret = Container.Resolve<IDomainService<T>>();
                    this._services.Add(typeof(T), ret);
                }

                return (IDomainService<T>)ret;
            }

            public void Dispose()
            {
                this._services = null;
            }
        }
    }
}
