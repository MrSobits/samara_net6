namespace Bars.Gkh.RegOperator.DomainEvent.Handlers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.Gkh.DomainEvent.Infrastructure;
    using Bars.Gkh.Enums;
    using Bars.Gkh.RegOperator.DomainEvent.Events.PersonalAccountDto;
    using Bars.Gkh.RegOperator.DomainModelServices.MassUpdater;
    using Bars.Gkh.RegOperator.DomainService.PersonalAccount;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.Utils;

    using NHibernate;

    /// <summary>
    /// Перехватчик событий изменения данных связанных с лицевым счётом для актуализации денормализованной сущности ЛС
    /// </summary>
    public class PersonalAccountDtoChangeHandler :
        DefaultMassOperationExecutor<BasePersonalAccount>,
        IDomainEventHandler<PersonalAccountChangeOwnerDtoEvent>,
        IDomainEventHandler<RoomChangeEvent>,
        IDomainEventHandler<RealityObjectForDtoChangeEvent>,
        IDomainEventHandler<MunicipalityChangeEvent>,
        IDomainEventHandler<PersonalAccountOwnerUpdateEvent>,
        IDomainEventHandler<BasePersonalAccountDtoEvent>
    {
        private readonly ISessionProvider sessionProvider;
        private readonly IMassPersonalAccountDtoService dtoChangeService;

        private ISession CurrentSession => this.sessionProvider.GetCurrentSession();

        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="sessionProvider">Фабрика сессий</param>
        /// <param name="dtoChangeService">Интерфес сервиса для массовой работы с BasePersonalAccountDto</param>
        public PersonalAccountDtoChangeHandler(
            ISessionProvider sessionProvider,
            IMassPersonalAccountDtoService dtoChangeService)
        {
            this.sessionProvider = sessionProvider;
            this.dtoChangeService = dtoChangeService;
        }

        /// <inheritdoc />
        public void Handle(BasePersonalAccountDtoEvent args)
        {
            if (MassUpdateContext.CurrentContext.IsNotNull())
            {
                this.AddEntity(args.Account);
                return;
            }

            var account = args.Account;
            var owner = account.AccountOwner;

            var changes = this.ApplyAccountChanges(
               account.PersAccNumExternalSystems,
               account.OpenDate,
               account.CloseDate,
               account.AreaShare,
               account.State.Id,
               account.State.StartState && owner.ActiveAccountsCount == 1);

            this.GetQuery(changes, "ID", args.Account.Id).ExecuteUpdate();
        }

        /// <inheritdoc />
        public void Handle(PersonalAccountChangeOwnerDtoEvent args)
        {
            var newOwnerId = args.NewOwner.Id;
            var ownerName = (args.NewOwner as LegalAccountOwner)?.Contragent.Name ?? args.NewOwner.Name;
            var ownerType = (int)args.NewOwner.OwnerType;

            var changes = this.ApplyOwnerChange(
                newOwnerId,
                ownerName,
                ownerType,
                args.NewOwner.PrivilegedCategory?.Id,
                args.NewOwner.ActiveAccountsCount,
                args.Account.State.StartState);

            this.GetQuery(changes, "ID", args.Account.Id).ExecuteUpdate();
        }

        /// <inheritdoc />
        public void Handle(PersonalAccountOwnerUpdateEvent args)
        {
            var owner = args.Owner;
            var changes = this.ApplyOwnerChange(
                owner.Id,
                (owner as LegalAccountOwner)?.Contragent.Name ?? owner.Name,
                (int)owner.OwnerType,
                owner.PrivilegedCategory?.Id,
                owner.ActiveAccountsCount);

            this.GetQuery(changes, "ACC_OWNER_ID", owner.Id).ExecuteUpdate();
        }

        /// <inheritdoc />
        public void Handle(RoomChangeEvent args)
        {
            var room = args.Room;
            var changes = this.ApplyRoomChangeParams(
                room.RealityObject.Address,
                room.RoomNum,
                room.ChamberNum,
                room.Area);

            this.GetQuery(changes, "ROOM_ID", room.Id).ExecuteUpdate();
        }

        /// <inheritdoc />
        public void Handle(RealityObjectForDtoChangeEvent args)
        {
            var ro = args.RealityObject;
            var changes = this.ApplyRealityObjectChangeParams(
                ro.Address,
                ro.Municipality.Id,
                ro.MoSettlement?.Id,
                ro.Municipality.Name,
                ro.MoSettlement?.Name,
                (int) (ro.AccountFormationVariant ?? CrFundFormationType.Unknown),
                ro.AreaMkd);

            this.GetQuery(changes, "RO_ID", ro.Id, "dto").ExecuteUpdate();
        }

        /// <inheritdoc />
        public void Handle(MunicipalityChangeEvent args)
        {
            var municipality = args.Municipality;

            // сначала пробуем обработать изменения как МР
            var changes = this.ApplyMunicipalityChangeParams(municipality.Name, false);
            this.GetQuery(changes, "MO_ID", municipality.Id).ExecuteUpdate();

            // потом пробуем обработать изменения как МО
            changes = this.ApplyMunicipalityChangeParams(municipality.Name, true);

            this.GetQuery(changes, "STL_ID", municipality.Id).ExecuteUpdate();
        }

        private IQuery GetQuery(
            IDictionary<string, object> data,
            string filterField,
            long value,
            string alias = "")
        {
            var query = this.CurrentSession.CreateSQLQuery(this.GetResultSql(data, filterField, value, alias));

            foreach (var dataValue in data.Where(x => x.Value.IsNotNull() && !(x.Value is SqlWrapper)))
            {
                query.SetParameter(dataValue.Key, dataValue.Value);
            }

            return query;
        }

        private string GetResultSql(
            IDictionary<string, object> data, 
            string filterField, 
            long value, 
            string alias = "")
        {
            return $@"
                UPDATE 
                    REGOP_PERS_ACC_DTO {alias}
                SET
                    {this.AggregateParams(data)}
                WHERE
                    {filterField} = {value}";
        }

        private string AggregateParams(IDictionary<string, object> data)
        {
            return data
                .Where(x => x.Value.IsNotNull())
                .Select(x => $"{x.Key} = {this.GetParamNameOrQuery(x)}")
                .AggregateWithSeparator(", ");
        }

        private string GetParamNameOrQuery(KeyValuePair<string, object> kvp)
        {
            var sql = kvp.Value as SqlWrapper;
            return sql.IsNotNull() ? $"({sql.Command})" : $":{kvp.Key}";
        } 

        private IDictionary<string, object> ApplyOwnerChange(
            long newOwnerId,
            string ownerName,
            int ownerType,
            long? privelegedCategoryId,
            int activeAccountsCount,
            bool? accStartState = null)
        {
            // если прошла смена абонента ЛС, то мы сразу можем определить имеет абонент только 1 открытый ЛС
            // в противном случае, добавляем подзапрос
            var onlyRoomExpression = accStartState.HasValue
                ? (object)(activeAccountsCount == 1 && accStartState.Value)
                : new SqlWrapper($@"SELECT count(*) = 1
                        FROM regop_pers_acc ac
                          JOIN b4_state st ON st.id = ac.state_id
                        WHERE acc_owner_id = {newOwnerId} AND st.start_state");

            var result = new Dictionary<string, object>
            {
                { "ACC_OWNER_ID", newOwnerId },
                { "OWNER_TYPE", ownerType },
                { "NAME", ownerName },
                { "PRIVILEGED_CATEGORY_ID", privelegedCategoryId },
                { "HAS_ONLY_ROOM_WITH_OPEN_STATE", onlyRoomExpression }
            };

            return result;
        }

        private IDictionary<string, object> ApplyAccountChanges(
            string persAccNumExternalSystems, 
            DateTime openDate, 
            DateTime? closeDate, 
            decimal areaShare,
            long stateId,
            bool hasOnlyOneRoomWithOpenState)
        {
            var result = new Dictionary<string, object>
            {
                { "REGOP_PERS_ACC_EXTSYST", persAccNumExternalSystems },
                { "OPEN_DATE", openDate },
                { "CLOSE_DATE", closeDate },
                { "AREA_SHARE", areaShare },
                { "STATE_ID", stateId },
                { "HAS_ONLY_ROOM_WITH_OPEN_STATE", hasOnlyOneRoomWithOpenState }
            };

            return result;
        }

        private IDictionary<string, object> ApplyRoomChangeParams(
            string realityObjectAddress,
            string roomNum,
            string chamberNum,
            decimal area)
        {
            var roomAddress = realityObjectAddress + ", кв. " + roomNum
                + (!string.IsNullOrEmpty(chamberNum) ? ", ком. " + chamberNum : string.Empty);

            var result = new Dictionary<string, object>
            {
                { "CAREA", area },
                { "CROOM_NUM", roomNum },
                { "ROOM_ADRESS", roomAddress }
            };

            return result;
        }

        private IDictionary<string, object> ApplyRealityObjectChangeParams(
            string realityObjectAddress,
            long muId,
            long? settlId,
            string municipality,
            string settlement,
            int accFormVariant,
            decimal? areaMkd)
        {
            var roomAddressExpr = new SqlWrapper($"'{realityObjectAddress}, кв. '||dto.CROOM_NUM||" +
                "(CASE WHEN COALESCE(dto.CHAMBER_NUM, '') = '' " +
                "THEN '' else ', ком. ' || dto.CHAMBER_NUM END)");

            var result = new Dictionary<string, object>
            {
                { "ADDRESS", realityObjectAddress },
                { "AREA_MKD", areaMkd },
                { "ACC_FORM_VARIANT", accFormVariant },
                { "MO_NAME", municipality },
                { "MO_ID", muId },
                { "STL_NAME", settlement },
                { "STL_ID", settlId },
                { "ROOM_ADRESS", roomAddressExpr },
            };

            return result;
        }

        private IDictionary<string, object> ApplyMunicipalityChangeParams(
            string name,
            bool forSettlement)
        {
           var result = new Dictionary<string, object>
            {
                { forSettlement ? "STL_NAME" : "MO_NAME", name },
            };

            return result;
        }

        private class SqlWrapper
        {
            public string Command { get; }

            public SqlWrapper(string command)
            {
                this.Command = command;
            }
        }

        protected override IEnumerable<BasePersonalAccount> ProcessChangesInternal(IEnumerable<BasePersonalAccount> entities, bool useStatless)
        {
            entities.ForEach(this.dtoChangeService.AddPersonalAccount);
            this.dtoChangeService.UseStatelessSession(useStatless);
            this.dtoChangeService.ApplyChanges();

            return Enumerable.Empty<BasePersonalAccount>();
        }
    }
}