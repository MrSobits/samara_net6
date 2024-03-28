namespace Bars.Gkh.Qa.Steps
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC.Lifestyles.SessionLifestyle;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Domain.ParameterVersioning;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Qa.Steps.CommonSteps;
    using Bars.Gkh.Qa.Utils;

    using FluentAssertions;

    using TechTalk.SpecFlow;

    [Binding]
    internal class RoomSteps : BindingBase
    {
        private DomainServiceCashe<Room> _cashe = new DomainServiceCashe<Room>();

        [Given(@"у этого дома добавлено помещение")]
        public void ДопустимУЭтогоДомаДобавленоПомещение(Table table)
        {
            var room = new Room
                           {
                               RoomNum = table.Rows[0]["RoomNum"],
                               Area = decimal.Parse(table.Rows[0]["Area"]),
                               LivingArea = decimal.Parse(table.Rows[0]["LivingArea"]),
                               Type = EnumHelper.GetFromDisplayValue<RoomType>(table.Rows[0]["Type"]),
                               OwnershipType = EnumHelper.GetFromDisplayValue<RoomOwnershipType>(table.Rows[0]["OwnershipType"])
                           };

            RoomHelper.Current = room;

            RoomHelper.Current.RealityObject = RealityObjectHelper.CurrentRealityObject;

            this._cashe.Current.SaveOrUpdate(room);

        }

        [Given(@"пользователь выбирает помещение текущего лс")]
        public void ДопустимПользовательВыбираетПомещениеТекущегоЛс()
        {
            RoomHelper.Current = BasePersonalAccountHelper.Current.Room;
        }

        [Given(@"пользователь у этого помещения вызывает смену общей доли собственности")]
        public void ДопустимПользовательУЭтогоПомещенияВызываетСменуОбщейДолиСобственности()
        {
            var changeRoomArea = new ChangeRoomArea
                                     {
                                         Room = RoomHelper.Current
                                     };

            ScenarioContext.Current["ChangeRoomArea"] = changeRoomArea;
        }

        [Given(@"пользователь в смене общей площади комнаты заполняет поле Дата вступления значения в силу ""(.*)""")]
        public void ДопустимПользовательВСменеОбщейПлощадиКомнатыЗаполняетПолеДатаВступленияЗначенияВСилу(string dateActual)
        {
            var changeRoomArea = ScenarioContext.Current.Get<ChangeRoomArea>("ChangeRoomArea");

            changeRoomArea.DateActual = dateActual.DateParse().Value;
        }

        [Given(@"пользователь в смене общей площади комнаты заполняет поле Новое значение ""(.*)""")]
        public void ДопустимПользовательВСменеОбщейПлощадиКомнатыЗаполняетПолеНовоеЗначение(decimal newArea)
        {
            var changeRoomArea = ScenarioContext.Current.Get<ChangeRoomArea>("ChangeRoomArea");

            changeRoomArea.NewArea = newArea;
        }

        [Given(@"пользователь у этого дома выбирает помещение под № ""(.*)""")]
        public void ДопустимПользовательУЭтогоДомаВыбираетПомещениеПод(string roomNumber)
        {
            var room = this._cashe.Current.GetAll()
                .FirstOrDefault(
                    x => x.RealityObject.Id == RealityObjectHelper.CurrentRealityObject.Id && x.RoomNum == roomNumber);

            if (room == null)
            {
                throw new SpecFlowException(
                    string.Format(
                        "у дома по адресу {0} отсутствует комната с номером {1}",
                        RealityObjectHelper.CurrentRealityObject.Address,
                        roomNumber));
            }

            RoomHelper.Current = room;
        }

        [When(@"пользователь в смене общей площади комнаты сохраняет изменения")]
        public void ЕслиПользовательВСменеОбщейПлощадиКомнатыСохраняетИзменения()
        {
            var changeRoomArea = ScenarioContext.Current.Get<ChangeRoomArea>("ChangeRoomArea");

            var service = Container.Resolve<IVersionedEntityService>();

            var baseParams = new BaseParams
                                 {
                                     Params =
                                         new DynamicDictionary
                                             {
                                                 { "factDate", changeRoomArea.DateActual },
                                                 { "value", changeRoomArea.NewArea },
                                                 { "className", "Room" },
                                                 { "propertyName", "Area" },
                                                 { "entityId", changeRoomArea.Room.Id }
                                             }
                                 };

            try
            {
                service.SaveParameterVersion(baseParams);
            }
            catch (Exception ex)
            {
                ExceptionHelper.AddException("IVersionedEntityService.SaveParameterVersion(ChangeRoomArea)", ex.Message);

                Container.Resolve<ISessionProvider>().GetCurrentSession().Clear();
            }
        }

        [Then(@"у этого помещения заполнено поле Общая площадь ""(.*)""")]
        public void ТоУЭтогоПомещенияЗаполненоПолеОбщаяПлощадь(decimal expectedArea)
        {
            var room = Container.Resolve<IDomainService<Room>>().Get(RoomHelper.Current.Id);

            var actualArea = room.Area;

            actualArea.Should()
                .Be(expectedArea, string.Format("У текущего помещения поле Общая площадь должно быть {0}", expectedArea));
        }


        private class ChangeRoomArea
        {
            public Room Room { get; set; }

            public decimal NewArea { get; set; }

            public DateTime DateActual { get; set; }
        }
    }
}
