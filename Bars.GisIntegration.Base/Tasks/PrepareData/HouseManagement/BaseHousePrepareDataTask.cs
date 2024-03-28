namespace Bars.GisIntegration.Base.Tasks.PrepareData.HouseManagement
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using Bars.B4.Utils;
    using Bars.GisIntegration.Base.DataExtractors;
    using Bars.GisIntegration.Base.Entities.HouseManagement;
    using Bars.GisIntegration.Base.Enums;
    using Bars.GisIntegration.Base.Tasks.PrepareData;
    using Bars.Gkh.Quartz.Scheduler.Log;

    /// <summary>
    /// Базовый класс задачи подготовки данных по домам
    /// </summary>
    /// <typeparam name="TRequestType">Тип soap запроса</typeparam>
    public abstract class BaseHousePrepareDataTask<TRequestType> : BasePrepareDataTask<TRequestType>
    {
        /// <summary>
        /// Список лифтов для экспорта
        /// </summary>
        protected List<RisLift> LiftList;

        /// <summary>
        /// Список домов для экспорта
        /// </summary>
        protected List<RisHouse> HouseList;

        /// <summary>
        /// Список жилых помещений для экспорта
        /// </summary>
        protected List<ResidentialPremises> ResidentialPremisesList;

        /// <summary>
        /// Список нежилых помещений для экспорта
        /// </summary>
        protected List<NonResidentialPremises> NonResidentialPremisesList;

        /// <summary>
        /// Список подъездов для экспорта
        /// </summary>
        protected List<RisEntrance> EntranceList;

        /// <summary>
        /// Список комнат жилых помещений (многоквартирных домов)
        /// </summary>
        protected List<LivingRoom> ResidentialPremiseLivingRooms;

        /// <summary>
        /// Список комнат жилых домов
        /// </summary>
        protected List<LivingRoom> LivingHouseLivingRooms;

        /// <summary>
        /// Размер блока предаваемых данных (максимальное количество записей)
        /// </summary>
        protected const int Portion = 1;

        /// <summary>
        /// Собрать данные
        /// </summary>
        /// <param name="parameters">Параметры экспорта</param>
        protected override void ExtractData(DynamicDictionary parameters)
        {
            this.HouseList = this.ExtractHouses(parameters);

            this.extractDataPercent = 20;

            // загружаем зависимые объекты только для домов из houseList

            parameters.Add("apartmentHouses", this.HouseList.Where(x => x.HouseType == HouseType.Apartment));

            this.EntranceList = this.ExtractEntrances(parameters);

            this.ExtractLifts(parameters);

            this.extractDataPercent = 40;

            this.ResidentialPremisesList = this.ExtractResidentialPremises(parameters);

            this.extractDataPercent = 60;

            this.NonResidentialPremisesList = this.ExtractNonResidentialPremises(parameters);


            this.extractDataPercent = 80;

            this.ExtractLivingRooms(parameters);

            this.extractDataPercent = 100;
        }

        /// <summary>
        /// Собрать данные по лифтам
        /// </summary>
        /// <param name="parameters">Параметры экспорта</param>
        /// <returns>Список подъездов</returns>
        private void ExtractLifts(DynamicDictionary parameters)
        {
            var extractor = this.Container.Resolve<IDataExtractor<RisLift>>("LiftDataExtractor");

            try
            {
                parameters.Add("houses", this.HouseList);
                this.AddLogRecord(new BaseLogRecord(MessageType.Info, "Начато извлечение данных по лифтам"));

                this.LiftList = this.RunExtractor(extractor, parameters) ?? new List<RisLift>();

                this.AddLogRecord(new BaseLogRecord(MessageType.Info, "Завершено извлечение данных по лифтам"));
            }
            finally
            {
                this.Container.Release(extractor);
            }
        }

        /// <summary>
        /// Валидация данных
        /// </summary>
        /// <returns>Результат валидации</returns>
        protected override List<ValidateObjectResult> ValidateData()
        {
            var result = new List<ValidateObjectResult>();

            this.AddLogRecord(new BaseLogRecord(MessageType.Info, "Начата валидация данных по домам"));

            result.AddRange(this.ValidateObjectList(this.HouseList, this.CheckHouse));

            this.AddLogRecord(new BaseLogRecord(MessageType.Info, "Завершена валидация данных по домам"));

            this.validateDataPercent = 20;

            this.AddLogRecord(new BaseLogRecord(MessageType.Info, "Начата валидация данных по жилым помещениям"));

            result.AddRange(this.ValidateObjectList(this.ResidentialPremisesList, this.CheckResidentialPremise));

            this.AddLogRecord(new BaseLogRecord(MessageType.Info, "Завершена валидация данных по жилым помещениям"));

            this.validateDataPercent = 40;

            this.AddLogRecord(new BaseLogRecord(MessageType.Info, "Начата валидация данных по нежилым помещениям"));

            result.AddRange(this.ValidateObjectList(this.NonResidentialPremisesList, this.CheckNonResidentialPremise));

            this.AddLogRecord(new BaseLogRecord(MessageType.Info, "Завершена валидация данных по нежилым помещениям"));

            this.validateDataPercent = 60;

            this.AddLogRecord(new BaseLogRecord(MessageType.Info, "Начата валидация данных по подъездам"));

            result.AddRange(this.ValidateObjectList(this.EntranceList, this.CheckEntrance));

            this.AddLogRecord(new BaseLogRecord(MessageType.Info, "Завершена валидация данных по подъездам"));

            this.validateDataPercent = 80;

            this.AddLogRecord(new BaseLogRecord(MessageType.Info, "Начата валидация данных по лифтам"));

            result.AddRange(this.ValidateObjectList(this.LiftList, this.CheckLift));

            this.AddLogRecord(new BaseLogRecord(MessageType.Info, "Завершена валидация данных по лифтам"));

            this.validateDataPercent = 90;

            this.AddLogRecord(new BaseLogRecord(MessageType.Info, "Начата валидация данных по комнатам"));

            result.AddRange(this.ValidateObjectList(this.ResidentialPremiseLivingRooms, this.CheckLivingRoom));
            result.AddRange(this.ValidateObjectList(this.LivingHouseLivingRooms, this.CheckLivingRoom));

            this.AddLogRecord(new BaseLogRecord(MessageType.Info, "Завершена валидация данных по комнатам"));

            this.validateDataPercent = 100;

            return result;
        }

        /// <summary>
        /// Получает список порций объектов ГИС для формирования объектов для запроса.
        /// </summary>
        /// <returns>Список порций объектов ГИС</returns>
        protected List<IEnumerable<RisHouse>> GetPortions()
        {
            List<IEnumerable<RisHouse>> result = new List<IEnumerable<RisHouse>>();

            if (this.HouseList.Count > 0)
            {
                var startIndex = 0;
                do
                {
                    result.Add(this.HouseList.Skip(startIndex).Take(BaseHousePrepareDataTask<TRequestType>.Portion));
                    startIndex += BaseHousePrepareDataTask<TRequestType>.Portion;
                }
                while (startIndex < this.HouseList.Count);
            }

            return result;
        }

        /// <summary>
        /// Переопределить параметры сбора данных
        /// </summary>
        /// <param name="parameters">Параметры сбора</param>
        protected virtual void OverrideExtractingParametes(DynamicDictionary parameters)
        {
        }

        /// <summary>
        /// Проверка дома перед импортом
        /// </summary>
        /// <param name="house">Дом</param>
        /// <returns>Результат проверки</returns>
        protected abstract ValidateObjectResult CheckHouse(RisHouse house);

        /// <summary>
        /// Проверка жилого помещения перед импортом
        /// </summary>
        /// <param name="premise">Жилое помещение</param>
        /// <returns>Результат проверки</returns>
        protected abstract ValidateObjectResult CheckResidentialPremise(ResidentialPremises premise);

        /// <summary>
        /// Проверка нежилого помещения перед импортом
        /// </summary>
        /// <param name="premise">Нежилое помещение</param>
        /// <returns>Результат проверки</returns>
        protected abstract ValidateObjectResult CheckNonResidentialPremise(NonResidentialPremises premise);

        /// <summary>
        /// Проверка подъезда перед импортом
        /// </summary>
        /// <param name="entrance">Подъезд</param>
        /// <returns>Результат проверки</returns>
        protected ValidateObjectResult CheckEntrance(RisEntrance entrance)
        {
            StringBuilder messages = new StringBuilder();

            if (entrance.EntranceNum == null)
            {
                messages.Append("EntranceNum ");
            }

            return new ValidateObjectResult
            {
                Id = entrance.Id,
                State = messages.Length == 0 ? ObjectValidateState.Success : ObjectValidateState.Error,
                Message = messages.ToString(),
                Description = "Подъезд"
            };
        }

        /// <summary>
        /// Проверка комнаты в жилом доме перед импортом
        /// </summary>
        /// <param name="livingRoom">Комната в жилом доме</param>
        /// <returns>Результат проверки</returns>
        protected abstract ValidateObjectResult CheckLivingRoom(LivingRoom livingRoom);

        /// <summary>
        /// Проверка лифта перед импортом
        /// </summary>
        /// <param name="lift">Лифт</param>
        /// <returns>Результат проверки</returns>
        protected ValidateObjectResult CheckLift(RisLift lift)
        {
            StringBuilder messages = new StringBuilder();

            if (lift.EntranceNum == null)
            {
                messages.Append("EntranceNum ");
            }

            if (lift.FactoryNum == null)
            {
                messages.Append("FactoryNum ");
            }

            return new ValidateObjectResult
            {
                Id = lift.Id,
                State = messages.Length == 0 ? ObjectValidateState.Success : ObjectValidateState.Error,
                Message = messages.ToString(),
                Description = "Лифт"
            };
        }

        /// <summary>
        /// Собрать данные по домам
        /// </summary>
        /// <param name="parameters">Параметры экспорта</param>
        /// <returns>Список домов</returns>
        private List<RisHouse> ExtractHouses(DynamicDictionary parameters)
        {
            var extractor = this.Container.Resolve<IDataExtractor<RisHouse>>("RisHouseDataExtractor");

            try
            {
                this.AddLogRecord(new BaseLogRecord(MessageType.Info, "Начато извлечение данных по домам"));

                extractor.Contragent = this.Contragent;

                this.OverrideExtractingParametes(parameters);

                var result = extractor.Extract(parameters);
                this.AddLogRecord(extractor.Log);

                this.AddLogRecord(new BaseLogRecord(MessageType.Info, "Завершено извлечение данных по домам"));

                return result;
            }
            finally
            {
                this.Container.Release(extractor);
            }
        }

        /// <summary>
        /// Собрать данные по подъездам
        /// </summary>
        /// <param name="parameters">Параметры экспорта</param>
        /// <returns>Список подъездов</returns>
        private List<RisEntrance> ExtractEntrances(DynamicDictionary parameters)
        {
            var extractor = this.Container.Resolve<IDataExtractor<RisEntrance>>("EntranceDataExtractor");

            try
            {
                this.AddLogRecord(new BaseLogRecord(MessageType.Info, "Начато извлечение данных по подъездам"));
                var result = this.RunExtractor(extractor, parameters);
                this.AddLogRecord(new BaseLogRecord(MessageType.Info, "Завершено извлечение данных по подъездам"));

                return result;
            }
            finally
            {
                this.Container.Release(extractor);
            }
        }

        /// <summary>
        /// Собрать данные по жилым помещениям
        /// </summary>
        /// <param name="parameters">Параметры экспорта</param>
        /// <returns>Список жилых помещений</returns>
        private List<ResidentialPremises> ExtractResidentialPremises(DynamicDictionary parameters)
        {
            var extractor = this.Container.Resolve<IDataExtractor<ResidentialPremises>>("ResidentialPremisesDataExtractor");

            try
            {
                this.AddLogRecord(new BaseLogRecord(MessageType.Info, "Начато извлечение данных по жилым помещениям"));
                var result = this.RunExtractor(extractor, parameters);
                this.AddLogRecord(new BaseLogRecord(MessageType.Info, "Завершено извлечение данных по жилым помещениям"));

                return result;
            }
            finally
            {
                this.Container.Release(extractor);
            }
        }

        /// <summary>
        /// Собрать данные по нежилым помещениям
        /// </summary>
        /// <param name="parameters">Параметры экспорта</param>
        /// <returns>Список нежилых помещений</returns>
        private List<NonResidentialPremises> ExtractNonResidentialPremises(DynamicDictionary parameters)
        {
            var extractor = this.Container.Resolve<IDataExtractor<NonResidentialPremises>>("NonResidentialPremisesDataExtractor");

            try
            {
                this.AddLogRecord(new BaseLogRecord(MessageType.Info, "Начато извлечение данных по нежилым помещениям"));
                var result = this.RunExtractor(extractor, parameters);
                this.AddLogRecord(new BaseLogRecord(MessageType.Info, "Завершено извлечение данных по нежилым помещениям"));

                return result;
            }
            finally
            {
                this.Container.Release(extractor);
            }
        }

        /// <summary>
        /// Получить списки жилых комнат для жилых помещений (многоквартирных домов) и для жилых домов
        /// </summary>
        /// <param name="parameters">Параметры экспорта</param>
        private void ExtractLivingRooms(DynamicDictionary parameters)
        {
            var extractor = this.Container.Resolve<IDataExtractor<LivingRoom>>("LivingRoomExtractor");

            try
            {
                if (extractor != null)
                {
                    this.AddLogRecord(new BaseLogRecord(MessageType.Info, "Начато извлечение данных по комнатам"));

                    var allRooms = this.RunExtractor(extractor, parameters) ?? new List<LivingRoom>();

                    this.ResidentialPremiseLivingRooms = allRooms.Where(x => x.ResidentialPremises != null).ToList();
                    this.LivingHouseLivingRooms = allRooms.Where(x => x.House != null).ToList();

                    this.AddLogRecord(new BaseLogRecord(MessageType.Info, "Завершено извлечение данных по комнатам"));
                }
            }
            finally
            {
                this.Container.Release(extractor);
            }
        }

        /// <summary>
        /// Получить блок Item в разделе BasicCharacteristicts
        /// </summary>
        /// <param name="cadastralNumber">Кадастровый номер</param>
        /// <returns>Блок Item в разделе BasicCharacteristicts</returns>
        protected object[] GetBasicCharacteristictsItem(string cadastralNumber)
        {
            var result = new List<object>();

            if (cadastralNumber.IsNotEmpty())
            {
                result.Add(cadastralNumber);
            }
            else
            {
                result.Add(true);
            }
            return result.ToArray();
        }
    }
}