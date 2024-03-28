using Bars.B4;

namespace Bars.Gkh.RegOperator.Entities
{
    using System;
    using B4.Utils.Annotations;
    using DomainModelServices;
    using Gkh.Entities;

    public partial class BasePersonalAccount
    {
        public class AccountBuilder
        {
            private PersonalAccountOwner _owner;
            private Room _room;
            private DateTime? _dateOpen;
            private decimal? _areaShare;

            private IRoomAreaShareSpecification _specification;

            private AccountBuilder() {}

            public static AccountBuilder GetBuilder()
            {
                return new AccountBuilder();
            }

            /// <summary>
            /// Установить неизменяемые свойства
            /// </summary>
            /// <param name="owner"></param>
            /// <param name="room"></param>
            /// <param name="dateOpen"></param>
            /// <param name="areaShare"></param>
            /// <returns></returns>
            public AccountBuilder SetInvariantProperties(PersonalAccountOwner owner, Room room, DateTime dateOpen, decimal areaShare)
            {
                _owner = owner;
                _room = room;
                _dateOpen = dateOpen;
                _areaShare = areaShare;

                return this;
            }

            /// <summary>
            /// Установить спецификацию помещения
            /// </summary>
            /// <param name="specification"></param>
            /// <returns></returns>
            public AccountBuilder SetRoomSpecification(IRoomAreaShareSpecification specification)
            {
                _specification = specification;

                return this;
            }

            /// <summary>
            /// Собрать лицевой счет
            /// </summary>
            /// <returns></returns>
            public BasePersonalAccount Build()
            {
                ArgumentChecker.NotNull(_specification, "Спецификация");
                ArgumentChecker.NotNull(_owner, "owner");
                ArgumentChecker.NotNull(_room, "room");
                ArgumentChecker.NotNull(_dateOpen, "dateOpen");
                ArgumentChecker.NotNull(_areaShare, "areaShare");

                var areaShare = _areaShare.GetValueOrDefault();

                var account = new BasePersonalAccount
                {
                    Room = this._room,
                    OpenDate = _dateOpen.GetValueOrDefault(),
                    AreaShare = areaShare,
                    AccountOwner = _owner
                };

                if (!_specification.ValidateAreaShare(account, this._room, areaShare, this._dateOpen))
                {
                    throw new ValidationException(
                        $"Сумма долей собственности в помещении \"{this._room.RealityObject.Address}, кв. {this._room.RoomNum}\" превышает 1");
                }

                return account;
            }
        }
    }
}