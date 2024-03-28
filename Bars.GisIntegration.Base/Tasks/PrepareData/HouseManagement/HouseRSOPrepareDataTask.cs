namespace Bars.GisIntegration.Base.Tasks.PrepareData.HouseManagement
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Bars.B4.Utils;
    using Bars.GisIntegration.Base.Entities.HouseManagement;
    using Bars.GisIntegration.Base.Enums;
    using Bars.GisIntegration.Base.HouseManagementAsync;
    using Bars.GisIntegration.Base.Tasks.PrepareData;

    /// <summary>
    /// Задача подготовки данных по домам для ресурсоснабжающих организаций
    /// </summary>
    public partial class HouseRSOPrepareDataTask : BaseHousePrepareDataTask<importHouseRSORequest>
    {
        /// <summary>
        /// Переопределить параметры сбора данных
        /// </summary>
        /// <param name="parameters">Параметры сбора</param>
        protected override void OverrideExtractingParametes(DynamicDictionary parameters)
        {
            parameters.Add("rsoId", this.Contragent.GkhId);
        }

        /// <summary>
        /// Проверка дома перед импортом
        /// </summary>
        /// <param name="house">Дом</param>
        /// <returns>Результат проверки</returns>
        protected override ValidateObjectResult CheckHouse(RisHouse house)
        {
            StringBuilder messages = new StringBuilder();

            if (house.FiasHouseGuid.IsEmpty())
            {
                messages.Append("FIASHouseGuid ");
            }

            if (house.OlsonTZCode.IsEmpty() || house.OlsonTZGuid.IsEmpty())
            {
                messages.Append("OlsonTZ ");
            }

            return new ValidateObjectResult
            {
                Id = house.Id,
                State = messages.Length == 0 ? ObjectValidateState.Success : ObjectValidateState.Error,
                Message = messages.ToString(),
                Description = "Дом"
            };
        }

        /// <summary>
        /// Проверка жилого помещения перед импортом
        /// </summary>
        /// <param name="premise">Жилое помещение</param>
        /// <returns>Результат проверки</returns>
        protected override ValidateObjectResult CheckResidentialPremise(ResidentialPremises premise)
        {
            StringBuilder messages = new StringBuilder();

            if (premise.PremisesNum.IsEmpty())
            {
                messages.Append("PremisesNum ");
            }

            if (premise.EntranceNum == null)
            {
                messages.Append("EntranceNum ");
            }

            return new ValidateObjectResult
            {
                Id = premise.Id,
                State = messages.Length == 0 ? ObjectValidateState.Success : ObjectValidateState.Error,
                Message = messages.ToString(),
                Description = "Жилое помещение"
            };
        }

        /// <summary>
        /// Проверка нежилого помещения перед импортом
        /// </summary>
        /// <param name="premise">Нежилое помещение</param>
        /// <returns>Результат проверки</returns>
        protected override ValidateObjectResult CheckNonResidentialPremise(NonResidentialPremises premise)
        {
            StringBuilder messages = new StringBuilder();

            if (string.IsNullOrEmpty(premise.PremisesNum))
            {
                messages.Append("PremisesNum ");
            }

            return new ValidateObjectResult
            {
                Id = premise.Id,
                State = messages.Length == 0 ? ObjectValidateState.Success : ObjectValidateState.Error,
                Message = messages.ToString(),
                Description = "Нежилое помещение"
            };
        }

        /// <summary>
        /// Проверка комнаты в жилом доме перед импортом
        /// </summary>
        /// <param name="livingRoom">Комната в жилом доме</param>
        /// <returns>Результат проверки</returns>
        protected override ValidateObjectResult CheckLivingRoom(LivingRoom livingRoom)
        {
            StringBuilder messages = new StringBuilder();

            if (livingRoom.RoomNumber.IsEmpty())
            {
                messages.Append("RoomNumber ");
            }

            return new ValidateObjectResult
            {
                Id = livingRoom.Id,
                State = messages.Length == 0 ? ObjectValidateState.Success : ObjectValidateState.Error,
                Message = messages.ToString(),
                Description = "Комната"
            };
        }

        /// <summary>
        /// Сформировать объекты запросов к асинхронному сервису ГИС
        /// </summary>
        /// <returns>Словарь Объект запроса - Словарь Транспортных идентификаторов: Тип обектов - Словарь: Транспортный идентификатор - Идентификатор объекта</returns>
        protected override Dictionary<importHouseRSORequest, Dictionary<Type, Dictionary<string, long>>> GetRequestData()
        {
            var result = new Dictionary<importHouseRSORequest, Dictionary<Type, Dictionary<string, long>>>();

            foreach (var iterationList in this.GetPortions())
            {
                var transportGuidDictionary = new Dictionary<Type, Dictionary<string, long>>();
                var request = this.GetRequestObject(iterationList, transportGuidDictionary);
                request.Id = Guid.NewGuid().ToString();

                result.Add(request, transportGuidDictionary);
            }

            return result;
        }

        /// <summary>
        /// Получить объект запроса
        /// </summary>
        /// <param name="listForImport">Список объектов для импорта (в текущем методе в списке 1 объект)</param>
        /// <param name="transportGuidDictionary">Словарь транспортных идентификаторов</param>
        /// <returns>Объект запроса</returns>
        private importHouseRSORequest GetRequestObject(IEnumerable<RisHouse> listForImport, Dictionary<Type, Dictionary<string, long>> transportGuidDictionary)
        {
            var house = listForImport.First();
            object item = null;

            switch (house.HouseType)
            {
                case HouseType.Apartment:
                    item = this.CreateApartmentHouseRequest(house, transportGuidDictionary);
                    break;
                case HouseType.Living:
                    //case HouseType.Blocks:
                    item = this.CreateLivingHouseRequest(house, transportGuidDictionary);
                    break;
            }

            return new importHouseRSORequest { Item = item };
        }

        /// <summary>
        /// Получить BasicCharacteristicts для раздела Create
        /// </summary>
        /// <param name="house">Дом</param>
        /// <returns>BasicCharacteristicts для раздела Create</returns>
        private HouseBasicRSOType GetBasicCharacteristictsToCreate(RisHouse house)
        {
            var itemsField = new List<object>();
            var itemsElementNameField = new List<ItemsChoiceType10>();

            if (house.CadastralNumber.IsNotEmpty())
            {
                itemsElementNameField.Add(ItemsChoiceType10.CadastralNumber);
                itemsField.Add(house.CadastralNumber);
            }
            else {
                itemsElementNameField.Add(ItemsChoiceType10.No_RSO_GKN_EGRP_Registered);
                itemsField.Add(true);
            }

            return new HouseBasicRSOType
            {
                Items = itemsField.ToArray(),
                ItemsElementName = itemsElementNameField.ToArray(),
                FIASHouseGuid = house.FiasHouseGuid,
                OKTMO = !house.OktmoCode.IsEmpty() ?
                new OKTMORefType
                {
                    code = house.OktmoCode
                } : null,
                OlsonTZ = new nsiRef
                {
                    Code = house.OlsonTZCode,
                    GUID = house.OlsonTZGuid
                }
            };
        }

        /// <summary>
        /// Получить BasicCharacteristicts для раздела Update
        /// </summary>
        /// <param name="house">Дом</param>
        /// <returns>BasicCharacteristicts для раздела Update</returns>
        private HouseBasicUpdateRSOType GetBasicCharacteristictsToUpdate(RisHouse house)
        {
            var itemsField = new List<object>();
            var itemsElementNameField = new List<ItemsChoiceType10>();

            if (house.CadastralNumber.IsNotEmpty())
            {
                itemsElementNameField.Add(ItemsChoiceType10.CadastralNumber);
                itemsField.Add(house.CadastralNumber);
            }
            else
            {
                itemsElementNameField.Add(ItemsChoiceType10.No_RSO_GKN_EGRP_Registered);
                itemsField.Add(true);
            }

            return new HouseBasicUpdateRSOType
            {
                Items = itemsField.ToArray(),
                ItemsElementName = itemsElementNameField.ToArray(),
                FIASHouseGuid = house.FiasHouseGuid,
                OlsonTZ = !house.OlsonTZCode.IsEmpty() && !house.OlsonTZGuid.IsEmpty() ? new nsiRef
                {
                    Code = house.OlsonTZCode,
                    GUID = house.OlsonTZGuid
                } : null,
                OKTMO = !house.OktmoCode.IsEmpty() ? new OKTMORefType
                {
                    code = house.OktmoCode
                } : null
            };
        }
    }
}
