namespace Bars.Gkh.RegOperator.Domain.RealtyObjectPayment
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Reflection.Emit;

    using Bars.B4.Utils;
    using Bars.Gkh.Domain.DatabaseMutex;
    using Bars.Gkh.DomainEvent.Infrastructure;
    using Bars.Gkh.Entities;
    using Bars.Gkh.RegOperator.Domain.AggregationRoots;
    using Bars.Gkh.RegOperator.Domain.Repository.RealityObjectAccount;
    using Bars.Gkh.RegOperator.DomainEvent.Infrastructure;

    using Castle.Windsor;

    /// <summary>
    /// Сессия обновления счетов дома. Аггрегирует оплаты по разным ЛС
    /// </summary>
    public class RealtyObjectPaymentSession : IRealtyObjectPaymentSession
    {
        private readonly RealtyObjectEventContainer eventsContainer;
        private readonly IWindsorContainer container;
        private readonly IDatabaseMutexManager mutexManager;
        private readonly IRealtyObjectPaymentRootRepository rootRepo;
        private readonly Guid sessionId;

        public RealtyObjectPaymentSession(
            RealtyObjectEventContainer eventsContainer,
            IWindsorContainer container,
            IDatabaseMutexManager mutexManager,
            IRealtyObjectPaymentRootRepository rootRepo)
        {
            this.eventsContainer = eventsContainer;
            this.container = container;
            this.mutexManager = mutexManager;
            this.rootRepo = rootRepo;
            this.sessionId = Guid.NewGuid();
        }

        /// <inheritdoc />
        public void FireEvent(RealityObject realityObject, IDomainEvent @event)
        {
            this.eventsContainer.AddEvent(realityObject, this, @event);
        }

        /// <inheritdoc />
        public void Complete()
        {
            var sessionItems = this.eventsContainer.GetSnapshot(this);

            var roots = this.rootRepo.GetByRealityObjects(sessionItems.Keys).ToDictionary(x => x.PaymentAccount.RealityObject);

            var uniqueEvents = sessionItems.Values.SelectMany(x => x).Select(x => x.GetType()).Distinct().ToList();

            var typeHandlerDict = new Dictionary<Type, Tuple<Func<object, object[], object>, object>>();

            foreach (var uniqueEvent in uniqueEvents)
            {
                var handler = this.container.Resolve(typeof(IRealtyObjectPaymentEventHandler<>).MakeGenericType(uniqueEvent));

                var method = handler.GetType().GetMethod("Execute", new[] { typeof(RealtyObjectPaymentRoot), uniqueEvent });

                if (method.IsNotNull())
                {
                    var wrapped = RealtyObjectPaymentSession.Wrap(method);

                    typeHandlerDict[uniqueEvent] = new Tuple<Func<object, object[], object>, object>(wrapped, handler);
                }
            }

            var hashSet = new HashSet<RealtyObjectPaymentRoot>();

            foreach (var sessionItem in sessionItems)
            {
                RealtyObjectPaymentRoot root;
                var changes = false;

                if (roots.TryGetValue(sessionItem.Key, out root))
                {
                    foreach (var domainEvent in sessionItem.Value)
                    {
                        Tuple<Func<object, object[], object>, object> handler;

                        if (typeHandlerDict.TryGetValue(domainEvent.GetType(), out handler))
                        {
                            handler.Item1(handler.Item2, new object[] { root, domainEvent });
                            changes = true;
                        }
                    }

                    if (changes)
                    {
                        hashSet.Add(root);
                    }
                }
            }

            hashSet.ForEach(this.rootRepo.Update);
        }

        /// <summary>
        /// The rollback.
        /// </summary>
        public void Rollback()
        {
            this.eventsContainer.ClearSessionItems(this);
        }

        private static Func<object, object[], object> Wrap(MethodInfo method)
        {
            var dm = new DynamicMethod(
                method.Name,
                typeof(object),
                new[]
                {
                    typeof(object),
                    typeof(object[])
                },
                method.DeclaringType,
                true);
            var il = dm.GetILGenerator();

            if (!method.IsStatic)
            {
                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Unbox_Any, method.DeclaringType);
            }
            var parameters = method.GetParameters();
            for (var i = 0; i < parameters.Length; i++)
            {
                il.Emit(OpCodes.Ldarg_1);
                il.Emit(OpCodes.Ldc_I4, i);
                il.Emit(OpCodes.Ldelem_Ref);
                il.Emit(OpCodes.Unbox_Any, parameters[i].ParameterType);
            }
            il.EmitCall(
                method.IsStatic || method.DeclaringType.IsValueType
                    ? OpCodes.Call
                    : OpCodes.Callvirt,
                method,
                null);
            if (method.ReturnType == null || method.ReturnType == typeof(void))
            {
                il.Emit(OpCodes.Ldnull);
            }
            else if (method.ReturnType.IsValueType)
            {
                il.Emit(OpCodes.Box, method.ReturnType);
            }
            il.Emit(OpCodes.Ret);
            return (Func<object, object[], object>)dm.CreateDelegate(typeof(Func<object, object[], object>));
        }

        /// <summary>
        /// Determines whether the specified <see cref="T:System.Object" /> is equal to the current <see cref="T:System.Object" />.
        /// </summary>
        /// <returns>
        /// true if the specified <see cref="T:System.Object" /> is equal to the current <see cref="T:System.Object" />; otherwise, false.
        /// </returns>
        /// <param name="obj">The object to compare with the current object. </param>
        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            if (object.ReferenceEquals(this, obj))
            {
                return true;
            }

            return this.Equals(obj as RealtyObjectPaymentSession);
        }

        protected bool Equals(RealtyObjectPaymentSession other)
        {
            if (other == null)
            {
                return false;
            }

            return this.sessionId.Equals(other.sessionId);
        }

        /// <summary>
        /// Serves as a hash function for a particular type.
        /// </summary>
        /// <returns>
        /// A hash code for the current <see cref="T:System.Object" />.
        /// </returns>
        public override int GetHashCode()
        {
            return this.sessionId.GetHashCode();
        }
    }
}