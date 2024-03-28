namespace Bars.Gkh.Reforma.Impl.DataCollectors
{
    using System;
    using System.Collections.Generic;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Reforma.ReformaService;
    using Bars.GkhDi.Entities;

    public partial class RobjectDataCollector
    {
        /// <summary>
        /// Собирает данные по жилому дому
        /// </summary>
        /// <param name="currentProfile">Текущая анкета жилого дома</param>
        /// <param name="robject">Жилой дом</param>
        /// <param name="period">Период раскрытия</param>
        /// <returns>Результат</returns>
        public IDataResult<HouseProfileData> CollectHouseProfileData(HouseProfileData currentProfile, RealityObject robject, PeriodDi period)
        {
            var useMerge = ((Dictionary<string, object>)this.syncService.GetParams().Data).Get("NullIsNotData").ToBool();
            var result = useMerge ? new HouseProfileData() : currentProfile;

            result.area_total = robject.AreaLivingNotLivingMkd ?? 0;
            result.area_residential = robject.AreaLiving;
            result.area_non_residential = result.area_total - result.area_residential;
            result.common_space = new CommonSpace { common_space_area = robject.AreaCommonUsage };
            result.cadastral_number = robject.CadastreNumber;
            result.exploitation_start_year = robject.DateCommissioning.HasValue ? robject.DateCommissioning.Value.ToString("yyyy") : null;

            // TODO: найти, что передавать
            //switch (robject.ConditionHouse)
            //{
            //    case ConditionHouse.Emergency:
            //        result.state = (int)HouseStateEnum.alarm;
            //        break;
            //    case ConditionHouse.Dilapidated:
            //        result.state = (int)HouseStateEnum.warning;
            //        break;
            //    case ConditionHouse.Serviceable:
            //        result.state = (int)HouseStateEnum.normal;
            //        break;
            //    case ConditionHouse.Razed:
            //        result.state = (int)HouseStateEnum.noinfo;
            //        break;
            //}

            result.project_type = robject.TypeProject.Return(x => x.Name);

            switch (robject.TypeHouse)
            {
                case TypeHouse.BlockedBuilding:
                    result.house_type = 3;
                    break;
                case TypeHouse.Individual:
                    result.house_type = 2;
                    break;
                case TypeHouse.ManyApartments:
                    result.house_type = 4;
                    break;
                case TypeHouse.SocialBehavior:
                    result.house_type = 1;
                    break;
            }

            result.wall_material = 1;
            if (robject.WallMaterial != null)
            {
                var material = 8;
                var name = robject.WallMaterial.Name;
                if (name.Equals("Кирпичные", StringComparison.CurrentCultureIgnoreCase))
                {
                    material = 2;
                }
                else if (name.Equals("Панельные", StringComparison.CurrentCultureIgnoreCase))
                {
                    material = 3;
                }
                else if (name.Equals("Блочные", StringComparison.CurrentCultureIgnoreCase))
                {
                    material = 4;
                }
                else if (name.Equals("Смешанные", StringComparison.CurrentCultureIgnoreCase))
                {
                    material = 5;
                }
                else if (name.Equals("Монолитные", StringComparison.CurrentCultureIgnoreCase))
                {
                    material = 6;
                }
                else if (name.Equals("Деревянные", StringComparison.CurrentCultureIgnoreCase))
                {
                    material = 7;
                }

                result.wall_material = material;
            }

            result.storeys_count = robject.MaximumFloors;
            if (result.storeys_count == 0)
            {
                result.storeys_count = null;
            }

            result.entrance_count = robject.NumberEntrances;
            if (result.entrance_count == 0)
            {
                result.entrance_count = null;
            }

            result.elevators_count = robject.NumberLifts;
            result.area_private = robject.AreaOwned;
            result.area_municipal = robject.AreaMunicipalOwned;
            result.area_national = robject.AreaGovernmentOwned;
            result.flats_count = robject.NumberApartments;
            result.residents_count = robject.NumberLiving;
            result.privatization_start_date = robject.PrivatizationDateFirstApartment;
            result.deterioration_total = robject.PhysicalWear;

            this.FillManagementContract(result, robject);
            this.FillDisclosureDependent(result, robject, period);
            var missedFields = this.CheckRequiredFields(result);
            if (missedFields.Length > 0)
            {
                return new GenericDataResult<HouseProfileData>(
                    null,
                    string.Format("Не заполнены обязательные поля: {0}", string.Join(", ", missedFields)))
                           {
                               Success =
                                   false
                           };
            }

            if (useMerge)
            {
                this.merger.Apply(currentProfile, result);
                result = currentProfile;
            }

            if (result.lifts != null && result.lifts.Length > 0)
            {
                Array.ForEach(
                    result.lifts,
                    x =>
                        {
                            if (x.capacity == 0)
                            {
                                x.capacity = null;
                            }

                            if (x.stops_count == 0)
                            {
                                x.stops_count = null;
                            }
                        });
            }

            return new GenericDataResult<HouseProfileData>(result);
        }
    }
}