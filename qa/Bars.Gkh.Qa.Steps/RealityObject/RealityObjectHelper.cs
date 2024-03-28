namespace Bars.Gkh.Qa.Steps
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Enums;

    using TechTalk.SpecFlow;

    using Bars.B4.Utils;
    using Bars.B4.Modules.FIAS;

    using Bars.Gkh.Entities;
    using Bars.Gkh.Qa.Utils;

    class RealityObjectHelper : BindingBase
    {
        /// <summary>
        /// Текущий жилой дом
        /// </summary>
        public static RealityObject CurrentRealityObject
        {
            get
            {
                if (!ScenarioContext.Current.ContainsKey("currentRealityObject"))
                {
                    throw new SpecFlowException("Отсутствует текущий жилой дом");
                }

                var realityObject = ScenarioContext.Current.Get<RealityObject>("currentRealityObject");

                return realityObject;
            }

            set
            {
                if (ScenarioContext.Current.ContainsKey("currentRealityObject"))
                {
                    ScenarioContext.Current.Remove("currentRealityObject");
                }

                ScenarioContext.Current.Add("currentRealityObject", value);
            }
        }

        /// <summary>
        /// Поиск фиас гуида по населённому пункту
        /// </summary>
        /// <param name="placeName">населённый пункт</param>
        /// <returns></returns>
        public static string GetFiasGuid(string placeName)
        {
            if (placeName.IsEmpty())
            {
                throw new Exception("Не задан населённый пункт");
            }

            var fiasRep = Container.Resolve<IFiasRepository>();

            var filter = placeName.Split('.').Last().Trim();

            var list = fiasRep.GetPlacesDinamicAddress(filter).ToList();

            if (list.IsEmpty())
            {
                throw new Exception("Не найдено ни одного Фиаса по заданному населённому пункту");
            }

            return list.First().AddressGuid;
        }

        public static TypeHouse GetTypeHouse(string externalName)
        {
            switch (externalName)
            {
                case "Многоквартирный":
                    {
                        return TypeHouse.ManyApartments;
                    }

                case "Блокированной застройки":
                    {
                        return TypeHouse.BlockedBuilding;
                    }

                case "Не задано":
                    {
                        return TypeHouse.NotSet;
                    }

                case "Индивидуальный":
                    {
                        return TypeHouse.Individual;
                    }

                case "Общежитие":
                    {
                        return TypeHouse.SocialBehavior;
                    }

                default:
                    {
                        throw new SpecFlowException(string.Format("Нет типа жилого дома {0}", externalName));
                    }
            }
        }

        public static RealityObject GetRoByAddress(string address)
        {
            var rods = Container.Resolve<IDomainService<RealityObject>>();

            var ro = rods.GetAll().FirstOrDefault(x => x.Address == address);

            if (ro == null)
            {
                throw new SpecFlowException(string.Format("Отсутствует дом с адресом {0}", address));
            }

            return ro;
        }
    }
}
