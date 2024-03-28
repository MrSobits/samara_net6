namespace Bars.Gkh.RegOperator.ExecutionAction
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Entities;
    using Bars.Gkh.ExecutionAction;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.Utils;

    public class RestoreLogLightAction : BaseExecutionAction
    {
        /// <summary>
        /// Код для регистрации
        /// </summary>
        public string Code => "RestoreLogLightAction";

        /// <summary>
        /// Описание действия
        /// </summary>
        public override string Description
            => "Восстановление версионируемых параметров для объектов, созданных после 1 февраля 2015 года (помещения и лицевые счета)";

        /// <summary>
        /// Название для отображения
        /// </summary>
        public override string Name => "Восстановление версионируемых параметров для объектов";

        /// <summary>
        /// Действие
        /// </summary>
        public override Func<IDataResult> Action => this.Execute;

        public BaseDataResult Execute()
        {
            var accountRepo = this.Container.ResolveRepository<BasePersonalAccount>();
            var roomDomain = this.Container.ResolveRepository<Room>();
            var logDomain = this.Container.ResolveRepository<EntityLogLight>();

            var roomDate = new DateTime(2014, 6, 1);
            var date = new DateTime(2015, 2, 1);

            var accounts = accountRepo.GetAll()
                .Where(x => x.ObjectCreateDate >= date)
                .Where(
                    z => !logDomain.GetAll()
                        .Where(x => x.EntityId == z.Id)
                        .Where(x => x.ClassName == "BasePersonalAccount")
                        .Any(x => x.DateActualChange == z.OpenDate))
                .Select(x => new {x.Id, x.AreaShare, x.OpenDate, x.ObjectCreateDate})
                .ToList();

            var logs = new List<EntityLogLight>();

            foreach (var account in accounts)
            {
                var areaShare = new EntityLogLight
                {
                    EntityId = account.Id,
                    ClassName = "BasePersonalAccount",
                    PropertyName = "AreaShare",
                    PropertyValue = account.AreaShare.RegopRoundDecimal(2).ToString(),
                    DateApplied = account.ObjectCreateDate,
                    DateActualChange = account.OpenDate,
                    ParameterName = "area_share",
                    User = "admin"
                };

                var openDate = new EntityLogLight
                {
                    EntityId = account.Id,
                    ClassName = "BasePersonalAccount",
                    PropertyName = "OpenDate",
                    PropertyValue = account.OpenDate.ToShortDateString(),
                    DateApplied = account.ObjectCreateDate,
                    DateActualChange = account.OpenDate,
                    ParameterName = "account_open_date",
                    User = "admin"
                };

                logs.Add(areaShare);
                logs.Add(openDate);
            }

            var rooms = roomDomain.GetAll()
                .Where(x => x.ObjectCreateDate >= date)
                .Where(
                    z => !logDomain.GetAll()
                        .Where(x => x.EntityId == z.Id)
                        .Where(x => x.ClassName == "Room")
                        .Any(x => x.DateActualChange == roomDate))
                .Select(
                    x => new
                    {
                        x.Id,
                        x.Area,
                        x.OwnershipType,
                        x.RoomNum,
                        x.ObjectCreateDate
                    })
                .ToList();

            foreach (var room in rooms)
            {
                var roomNum = new EntityLogLight
                {
                    EntityId = room.Id,
                    ClassName = "Room",
                    PropertyName = "RoomNum",
                    PropertyValue = room.RoomNum,
                    DateApplied = room.ObjectCreateDate,
                    DateActualChange = roomDate,
                    ParameterName = "room_num",
                    User = "admin"
                };

                var ownership = new EntityLogLight
                {
                    EntityId = room.Id,
                    ClassName = "Room",
                    PropertyName = "OwnershipType",
                    PropertyValue = room.OwnershipType.ToString(),
                    DateApplied = room.ObjectCreateDate,
                    DateActualChange = roomDate,
                    ParameterName = "room_ownership_type",
                    User = "admin"
                };

                var area = new EntityLogLight
                {
                    EntityId = room.Id,
                    ClassName = "Room",
                    PropertyName = "Area",
                    PropertyValue = room.Area.RegopRoundDecimal(2).ToString(),
                    DateApplied = room.ObjectCreateDate,
                    DateActualChange = roomDate,
                    ParameterName = "room_area",
                    User = "admin"
                };

                logs.Add(roomNum);
                logs.Add(ownership);
                logs.Add(area);
            }

            TransactionHelper.InsertInManyTransactions(this.Container, logs, 1000, true, true);

            return new BaseDataResult();
        }
    }
}