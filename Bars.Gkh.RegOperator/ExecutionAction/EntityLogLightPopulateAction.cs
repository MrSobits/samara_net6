namespace Bars.Gkh.RegOperator.ExecutionAction
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.Security;
    using Bars.B4.Utils;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Domain.ParameterVersioning;
    using Bars.Gkh.Entities;
    using Bars.Gkh.ExecutionAction;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.Repositories.ChargePeriod;

    public class EntityLogLightPopulateAction : BaseExecutionAction
    {
        private string _login;

        public override string Description => "Проставить историю версионируемым сущностям, которые не имеют истории. Только Лицевые Счета и Помещения!";

        public override string Name => "Проставить историю версионируемым сущностям";

        public override Func<IDataResult> Action => this.PopulateEntityLogLight;

        public IRepository<User> UserRepository { get; set; }

        public IRepository<EntityLogLight> EntityLogLightRepository { get; set; }

        public ISessionProvider SessionProvider { get; set; }

        public IUserIdentity Identity { get; set; }

        private BaseDataResult PopulateEntityLogLight()
        {
            var newEntityLogLight = new List<EntityLogLight>();

            var user = this.Container.Resolve<IGkhUserManager>().GetActiveUser();

            this._login = user == null ? "anonymous" : user.Login;

            newEntityLogLight.AddRange(this.GenerateEntityLogLight<BasePersonalAccount>());
            newEntityLogLight.AddRange(this.GenerateEntityLogLight<Room>());

            this.Save(newEntityLogLight);

            return new BaseDataResult();
        }

        private List<EntityLogLight> GenerateEntityLogLight<T>() where T : IEntity, new()
        {
            var result = new List<EntityLogLight>();
            var currentPeriod = this.Container.Resolve<IChargePeriodRepository>().GetCurrentPeriod();

            var fakeEntity = new T();

            if (!VersionedEntityHelper.IsUnderVersioning(fakeEntity))
            {
                return result;
            }

            var parameters = VersionedEntityHelper.GetCreator(fakeEntity)
                .CreateParameters()
                .Where(x => !VersionedEntityHelper.ShouldSkip(fakeEntity, x.ParameterName))
                .ToList();

            var parameterNames = parameters.Select(x => x.ParameterName).ToList();

            var entityLastLogDict = this.EntityLogLightRepository.GetAll()
                .Where(x => x.ClassName == fakeEntity.GetType().Name)
                .Where(x => parameterNames.Contains(x.ParameterName))
                .Select(x => new {x.EntityId, x.Id, x.PropertyValue, x.ParameterName})
                .AsEnumerable()
                .GroupBy(x => x.EntityId)
                .ToDictionary(
                    x => x.Key,
                    x => x.GroupBy(y => y.ParameterName)
                        .ToDictionary(y => y.Key, y => y.OrderByDescending(z => z.Id).First().PropertyValue));

            var entityData = this.Container.ResolveRepository<T>().GetAll().ToList();

            var props =
                typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public)
                    .Where(x => x.CanRead && x.CanWrite)
                    .ToList();

            var className = typeof(T).Name;

            Func<string, string, bool, bool, bool> needNewLog = (v1, v2, v1HasComa, v2HasComa) =>
            {
                if (v1.Length > v2.Length)
                {
                    var diff = v1.Length - v2.Length;

                    var qwe =
                        v2.Append(
                            (v2HasComa ? "" : ",") +
                                string.Join("", Enumerable.Range(1, diff - (v2HasComa ? 0 : 1)).Select(x => "0")));

                    return v1 != qwe;
                }

                if (v1.Length < v2.Length)
                {
                    var diff = v2.Length - v1.Length;

                    var qwe =
                        v1.Append(
                            (v1HasComa ? "" : ",") +
                                string.Join("", Enumerable.Range(1, diff - (v1HasComa ? 0 : 1)).Select(x => "0")));

                    return v2 != qwe;
                }

                return true;
            };

            foreach (var entity in entityData)
            {
                T _entity;

                if (className == entity.GetType().Name)
                {
                    _entity = entity;
                }
                else
                {
                    _entity = new T();
                    foreach (var property in props)
                    {
                        property.SetValue(_entity, property.GetValue(entity, null), null);
                    }
                }

                var newlogs = this.LogEntity(_entity, currentPeriod);

                if (!newlogs.Any())
                {
                    continue;
                }

                var entityId = (long) _entity.Id;

                if (entityLastLogDict.ContainsKey(entityId))
                {
                    var lastLogDict = entityLastLogDict[entityId];

                    foreach (var newlog in newlogs)
                    {
                        if (lastLogDict.ContainsKey(newlog.ParameterName))
                        {
                            var lastLog = lastLogDict[newlog.ParameterName];

                            if (lastLog != newlog.PropertyValue)
                            {
                                var createNewLog = true;

                                var lastLogHasComa = lastLog.Contains(",");
                                var newLogHasComa = newlog.PropertyValue.Contains(",");
                                if (lastLogHasComa || newLogHasComa)
                                {
                                    createNewLog = needNewLog(newlog.PropertyValue, lastLog, newLogHasComa, lastLogHasComa);
                                }

                                if (createNewLog)
                                {
                                    result.Add(newlog);
                                }
                            }
                        }
                        else
                        {
                            result.Add(newlog);
                        }
                    }
                }
                else
                {
                    result.AddRange(newlogs);
                }
            }

            return result;
        }

        private List<EntityLogLight> LogEntity(IEntity entity, ChargePeriod period)
        {
            var parameters =
                VersionedEntityHelper.GetCreator(entity)
                    .CreateParameters()
                    .Where(x => !VersionedEntityHelper.ShouldSkip(entity, x.ParameterName))
                    .ToList();

            var hasDate = entity as IHasDateActualChange;

            var now = DateTime.Now;

            var logParams = parameters
                .Select(
                    x => new EntityLogLight
                    {
                        EntityId = entity.Id.To<long>(),
                        ClassName = x.ClassName,
                        PropertyName = x.PropertyName,
                        PropertyValue = x.PropertyValue,
                        ParameterName = x.ParameterName,
                        DateApplied = now,
                        DateActualChange = hasDate.Return(
                            y => y.ActualChangeDate,
                            new DateTime(
                                now.Year,
                                period.StartDate.Month,
                                1,
                                now.Hour,
                                now.Minute,
                                now.Second,
                                now.Millisecond)),
                        User = this._login,
                        ObjectCreateDate = now,
                        ObjectEditDate = now
                    })
                .ToList();

            return logParams;
        }

        private void Save(List<EntityLogLight> listToSave)
        {
            TransactionHelper.InsertInManyTransactions(this.Container, listToSave, 10000, true, true);
        }
    }
}