namespace Bars.Gkh.RegOperator.ExecutionAction
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Entities;
    using Bars.Gkh.ExecutionAction;
    using Bars.Gkh.RegOperator.Entities;

    public class EntityLogLightComparsionAction : BaseExecutionAction
    {
        public override string Description => "Установка истории изменений в случае расхождения с текущим значением";

        public override string Name => "Установка истории изменений в случае расхождения с текущим значением";

        public override Func<IDataResult> Action => this.Execute;

        private BaseDataResult Execute()
        {
            var entityLogLightRepo = this.Container.ResolveRepository<EntityLogLight>();
            var persAccRepo = this.Container.ResolveRepository<BasePersonalAccount>();
            var roomRepo = this.Container.ResolveRepository<Room>();

            try
            {
                var persAccEntityLogListToSave = new List<EntityLogLight>();

                var persAccLogDict =
                    entityLogLightRepo.GetAll()
                        .Where(x => x.ClassName == "BasePersonalAccount")
                        .Where(x => x.PropertyName == "AreaShare")
                        .Select(x => new {x.EntityId, x.PropertyValue})
                        .ToList()
                        .GroupBy(x => x.EntityId)
                        .ToDictionary(x => x.Key, x => x.Select(y => y.PropertyValue.ToDecimal()).ToArray());

                var persAccs = persAccRepo.GetAll()
                    .Where(x => x.State.StartState)
                    .Select(
                        x => new
                        {
                            x.Id,
                            x.AreaShare
                        })
                    .ToList();

                foreach (var acc in persAccs)
                {
                    if (!persAccLogDict[acc.Id].Contains(acc.AreaShare))
                    {
                        var persAccEntityLog = new EntityLogLight
                        {
                            EntityId = acc.Id,
                            ClassName = "BasePersonalAccount",
                            PropertyName = "AreaShare",
                            PropertyValue = decimal.Round(acc.AreaShare, 4).ToString("G29"),
                            DateActualChange = new DateTime(2014, 10, 1),
                            DateApplied = DateTime.Now,
                            ParameterName = "area_share",
                            User = "FixAction"
                        };

                        persAccEntityLogListToSave.Add(persAccEntityLog);
                    }
                }

                var roomEntityLogListToSave = new List<EntityLogLight>();

                var roomLogDict =
                    entityLogLightRepo.GetAll()
                        .Where(x => x.ClassName == "Room")
                        .Where(x => x.PropertyName == "Area")
                        .Select(x => new {x.EntityId, x.PropertyValue})
                        .ToList()
                        .GroupBy(x => x.EntityId)
                        .ToDictionary(x => x.Key, x => x.Select(y => y.PropertyValue.ToDecimal()).ToArray());

                var rooms = roomRepo.GetAll()
                    .Select(
                        x => new
                        {
                            x.Id,
                            x.Area
                        })
                    .ToList();

                foreach (var room in rooms)
                {
                    if (!roomLogDict[room.Id].Contains(room.Area))
                    {
                        var roomEntityLog = new EntityLogLight
                        {
                            EntityId = room.Id,
                            ClassName = "Room",
                            PropertyName = "Area",
                            PropertyValue = decimal.Round(room.Area, 2).ToString("G29"),
                            DateActualChange = new DateTime(2014, 10, 1),
                            DateApplied = DateTime.Now,
                            ParameterName = "room_area",
                            User = "FixAction"
                        };

                        roomEntityLogListToSave.Add(roomEntityLog);
                    }
                }

                TransactionHelper.InsertInManyTransactions(this.Container, persAccEntityLogListToSave);
                TransactionHelper.InsertInManyTransactions(this.Container, roomEntityLogListToSave);

                return new BaseDataResult();
            }
            catch (Exception e)
            {
                return new BaseDataResult(false, e.Message);
            }
            finally
            {
                this.Container.Release(entityLogLightRepo);
                this.Container.Release(persAccRepo);
                this.Container.Release(roomRepo);
            }
        }
    }
}