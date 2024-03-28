namespace Bars.Gkh.DomainService
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.PassportProvider;
    using Bars.Gkh.Utils;
    using System.Reflection;

    using Castle.Windsor;

    using Microsoft.AspNetCore.Http;

    public class RealityObjectTpSyncService : IRealityObjectTpSyncService
    {
        public IPassportProvider Provider { get; set; }

        public IHttpContextAccessor HttpContextAccessor { get; set; }

        private readonly Dictionary<string, string> roMap = new Dictionary<string, string>
        {
            {"BuildYear", "Form_1#2:1"},
            {"TypeProject", "Form_1#28:1"},
            {"PhysicalWear", "Form_1#20:1"},
            {"TotalBuildingVolume", "Form_1#5:1"},
            {"AreaOwned", "Form_1#24:1"},
            {"AreaMunicipalOwned", "Form_1#25:1"},
            {"AreaGovernmentOwned", "Form_1#26:1"},
            {"AreaLivingNotLivingMkd", "Form_1#6:1"},
            {"AreaLiving", "Form_1#7:1"},
            {"AreaNotLivingFunctional", "Form_1#8:1"},
            {"AreaCommonUsage", "Form_1_3#3:1"},
            {"MaximumFloors", "Form_1#11:1"},
            {"Floors", "Form_1#10:1"},
            {"NumberEntrances", "Form_1#12:1"},
            {"NumberLiving", "Form_1#13:1"},
            {"NumberLifts", "Form_1#34:1"},
            {"NumberApartments", "Form_1#35:1"},
            {"NumberNonResidentialPremises", "Form_1#36:1"},
            {"Address", "Form_1#1:1"},
            {"CadastralHouseNumber", "Form_1#29:1"},
            {"PrivatizationDateFirstApartment", "Form_1#31:1"},
            {"IsCulturalHeritage", "Form_1#40:1"},
            {"IsEmergency", "Form_1#41:1"},
            {"HeatingSystem", "Form_3_1#1:3"},
            {"WallMaterial", "Form_5_2#1:3"},
            {"TypeRoof", "Form_5_6#1:3"},
            {"RoofingMaterial", "Form_5_6_2#29:1"},
            {"CadastreNumber", "Form_1#30:1"},
            {"ConditionHouse", "Form_1_1#1:1"},
            {"TypeHouse", "Form_1_2_3#7:1"},

            {"DateCommissioning", "Form_1#47:1" }
        };

        protected virtual Dictionary<string, string> tpMap => new Dictionary<string, string>
        {
            {"BuildYear", "Form_1#2:1"},
            {"TypeProject", "Form_1#28:1"},
            {"PhysicalWear", "Form_1#20:1"},
            {"TotalBuildingVolume", "Form_1#5:1"},
            {"AreaOwned", "Form_1#24:1"},
            {"AreaMunicipalOwned", "Form_1#25:1"},
            {"AreaGovernmentOwned", "Form_1#26:1"},
            {"AreaLivingNotLivingMkd", "Form_1#6:1"},
            {"AreaLiving", "Form_1#7:1"},
            {"AreaNotLivingFunctional", "Form_1#8:1"},
            {"AreaCommonUsage", "Form_1_3#3:1"},
            {"MaximumFloors", "Form_1#11:1"},
            {"Floors", "Form_1#10:1"},
            {"NumberEntrances", "Form_1#12:1"},
            {"NumberLiving", "Form_1#13:1"},
            {"NumberLifts", "Form_1#34:1"},
            {"NumberApartments", "Form_1#35:1"},
            {"NumberNonResidentialPremises", "Form_1#36:1"},
            {"CadastralHouseNumber", "Form_1#29:1"},
            {"PrivatizationDateFirstApartment", "Form_1#31:1"},
            {"IsCulturalHeritage", "Form_1#40:1"},
            {"IsEmergency", "Form_1#41:1"},
            {"HeatingSystem", "Form_3_1#1:3"},
            {"WallMaterial", "Form_5_2#1:3"},
            {"TypeRoof", "Form_5_6#1:3"},
            {"RoofingMaterial", "Form_5_6_2#29:1"},
            {"CadastreNumber", "Form_1#30:1"},
            {"DateCommissioning", "Form_1#47:1" }
        };

        private readonly Dictionary<string, string> roPropertyDisplayNames = new Dictionary<string, string>
        {
            {"BuildYear", "Год постройки"},
            {"TypeProject", "Серия, тип проекта"},
            {"PhysicalWear", "Физический износ %"},
            {"TotalBuildingVolume", "Общий строительный объём общая площадь мкд"},
            {"DateCommissioning", "Год ввода в эксплуатацию"},
            {"AreaOwned", "Площадь частной собственности"},
            {"AreaMunicipalOwned", "Площадь муниципальной собственности"},
            {"AreaGovernmentOwned", "Площадь государственной собственности"},
            {"AreaMkd", "Общая площадь МКД"},
            {"AreaLiving", "В т.ч. жилых всего"},
            {"AreaNotLivingFunctional", "В т.ч. нежилых помещений , функционального назначения"},
            {"AreaCommonUsage", "Площадь помещений общего пользования"},
            {"MaximumFloors", "Максимальная этажность"},
            {"Floors", "Минимальная этажность"},
            {"NumberEntrances", "Количество подъездов"},
            {"NumberLiving", "Количество проживающих"},
            {"NumberLifts", "Количество лифтов"},
            {"NumberApartments", "Количество квартир"},
            {"NumberNonResidentialPremises", "Количество нежилых помещений"},
            {"Address", "Адрес"},
            {"CadastralHouseNumber", "Кадастровый номер дома"},
            {"PrivatizationDateFirstApartment", "Дата приватизации первого жилого помещения"},
            {"IsCulturalHeritage", "Дом имеет статус объекта культурного наследия"},
            {"ConditionHouse", "Состояние дома"},
            {"HeatingSystem", "Система отопления"},
            {"WallMaterial", "Материал стен"},
            {"TypeRoof", "Тип кровли"},
            {"RoofingMaterial", "Материал кровли"}
        };

        private readonly Dictionary<string, string> tpCellDisplayNames = new Dictionary<string, string>
        {
            {"Form_1#2:1", "1 год постройки"},
            {"Form_1#28:1", "1 Серия тип проекта"},
            {"Form_1#20:1", "1 Степень износа здания"},
            {"Form_1#5:1", "1 общий строительный объем"},
            {"Form_1#24:1", "1 частная собственность"},
            {"Form_1#25:1", "1 муниципальная собственность"},
            {"Form_1#26:1", "1 государственная собственность"},
            {"Form_1#6:1", "1 площадь здания всего"},
            {"Form_1#7:1", "1 в том числе: жилой части здания"},
            {"Form_1#8:1", "1 нежилых помещений функционального назначения"},
            {"Form_1_3#3:1", "1.3 площадь помещений общего пользования"},
            {"Form_1#11:1", "1 Количество этажей наибольшая"},
            {"Form_1#10:1", "1 Количество этажей наименьшая"},
            {"Form_1#12:1", "1 Количество подъездов"},
            {"Form_1#13:1", "1 Количество проживающих или зарегестрированных"},
            {"Form_4_2#1:1", "4.1 Лифты 4.2 Количество лифтов"},
            {"Form_1#35:1", "1 Количество жилых помещений (квартир)"},
            {"Form_1#36:1", "1 Количество нежилых помещений"},
            {"Form_1#1:1", "1 Адрес многоквартирного дома"},
            {"Form_1#29:1", "1 Кадастровый номер дома"},
            {"Form_1#31:1", "1 Дата приватизации первого жилого помещения"},
            {"Form_1#40:1", "1 Наличие статуса объекта культурного наследия"},
            {"Form_1#41:1", "1 Наличие факта признания дома аварийным"},
            {"Form_3_1#1:3", "1 Отопление"},
            {"Form_5_2#1:3", "1 Тип стен"},
            {"Form_5_6#1:3", "1 Конструкция крыши"},
            {"Form_5_6_2#29:1", "1 Материал кровли"}
        };

        public RealityObjectTpSyncService(IDomainService<TehPassportValue> tpService, IRepository<RealityObject> roRepo, IWindsorContainer container)
        {
            this.TpService = tpService;
            this.RoRepo = roRepo;
            this.Container = container;
        }

        private IWindsorContainer Container { get; set; }

        private IDomainService<TehPassportValue> TpService { get; set; }

        private IRepository<RealityObject> RoRepo { get; set; }

        public IDataResult Validate(TehPassportValue value)
        {
            var code = value.FormCode + "#" + value.CellCode;
            if (!this.roMap.ContainsValue(code))
            {
                return new BaseDataResult();
            }

            var robject = value.TehPassport.RealityObject;
            var propertyName = this.roMap.FirstOrDefault(x => x.Value == code).Key;
            var correspondingValue = this.GetRoValue(propertyName, robject);
            if (!string.IsNullOrEmpty(correspondingValue))
            {
                if (correspondingValue.Contains(","))
                {
                    var temp = correspondingValue.Split(',');
                    if (temp[1][0] == '0')
                        correspondingValue = temp[0];
                    else
                        correspondingValue = temp[0] + "," + temp[1].TrimEnd('0');
                }

                if (value.Value.Contains(","))
                {
                    var temp = value.Value.Split(',');
                    if (temp[1][0] == '0')
                        value.Value = temp[0];
                    else
                        value.Value = temp[0] + "," + temp[1].TrimEnd('0');
                }

                if (correspondingValue != value.Value)
                    return new BaseDataResult(
                     false,
                     "Значение показателя \"{0}\" отличается от значения, указанного в разделе \"Общие сведения -> {1}\""
                         .FormatUsing(this.tpCellDisplayNames[code], this.roPropertyDisplayNames[propertyName]));
            }

            return new BaseDataResult();
        }

        public IDataResult Validate(RealityObject robject)
        {
            var roValues = this.GetRoValues(this.roMap.Keys, robject);
            var tpValues = this.GetTpValues(this.roMap.Values, robject);

            var differences =
                this.roMap.Select(pair => new { pair, roValue = roValues.Get(pair.Key), tpValue = tpValues.Get(pair.Value) })
                    .Where(t => !string.IsNullOrEmpty(t.tpValue) && t.tpValue != t.roValue)
                    .Select(t => t.pair)
                    .ToArray();

            if (!differences.Any())
            {
                return new BaseDataResult();
            }

            var sb = new StringBuilder();
            sb.Append("Значения следующих показателей отличаются от указанных в ТехПаспорте:<br/>");
            differences.ForEach(d => sb.Append("\"{0}\" и \"{1}\"<br/>".FormatUsing(this.roPropertyDisplayNames[d.Key], this.tpCellDisplayNames[d.Value])));
            sb.Append("Проверьте правильность введенных данных");

            return new BaseDataResult(false, sb.ToString());
        }

        public IDataResult Sync(TehPassportValue value)
        {
            try
            {
                HttpContextAccessor.HttpContext.Session.SetString("noSync", "true");

                var propertyName = this.tpMap.FirstOrDefault(x => x.Value == value.FormCode + "#" + value.CellCode).Return(x => x.Key);
                if (propertyName != null)
                {
                    var robject = value.TehPassport.RealityObject;
                    var property = typeof(RealityObject).GetProperty(propertyName);
	                var newValue = this.GetRoPropertyValue(value, property);

					property.SetValue(robject, newValue, null);

                    this.RoRepo.Update(robject);
                }

                return new BaseDataResult();
            }
            finally
            {
                HttpContextAccessor.HttpContext.Session.SetString("noSync", "false");
            }
        }

        public IDataResult Sync(RealityObject robject)
        {
            try
            {
                this.HttpContextAccessor.HttpContext.Session?.SetString("noSync", "true");

                var roValues = this.GetRoValues(this.roMap.Keys, robject);
                var cellsLookup =
                    roValues.Select(x => new { CellCode = this.roMap[x.Key], x.Value })
                        .Select(x => new { Split = x.CellCode.Split('#'), x.Value })
                        .Select(x => new SerializePassportValue { ComponentCode = x.Split[0], CellCode = x.Split[1], Value = x.Value })
                        .ToLookup(x => x.ComponentCode);
                
                cellsLookup.ForEach(cells => this.Provider.UpdateForm(robject.Id, cells.Key, cells.ToList(), fromSync: true ));

                return new BaseDataResult();
            }
            finally
            {
                this.HttpContextAccessor.HttpContext.Session?.SetString("noSync", "false");
            }
        }

        public IDataResult Sync(List<RealityObject> robjects)
        {
            try
            {
                this.HttpContextAccessor.HttpContext.Session?.SetString("noSync", "true");

                Dictionary<long, Dictionary<string, string>> roValues = new Dictionary<long, Dictionary<string, string>>();

                foreach (var robject in robjects)
                {
                    roValues[robject.Id] = this.GetRoValues(this.roMap.Keys, robject);
                }

                var cellsDict = roValues.ToDictionary(x => x.Key,
                    y => y.Value.Select(x => new { CellCode = this.roMap[x.Key], x.Value })
                        .Select(x => new { Split = x.CellCode.Split('#'), x.Value })
                        .Select(x => new SerializePassportValue { ComponentCode = x.Split[0], CellCode = x.Split[1], Value = x.Value })
                        .GroupBy(x => x.ComponentCode)
                        .ToDictionary(x => x.Key, x => x.ToList()));

                this.Provider.UpdateForms(cellsDict, true);

                return new BaseDataResult();
            }
            finally
            {
                this.HttpContextAccessor.HttpContext.Session?.SetString("noSync", "false");
            }
        }

        private Dictionary<string, string> GetTpValues(IEnumerable<string> codes, RealityObject robject)
        {
            return
                this.TpService.GetAll()
                    .Select(x => new { Code = x.FormCode + "#" + x.CellCode, x.Value, RoId = x.TehPassport.RealityObject.Id })
                    .Where(x => codes.Contains(x.Code) && x.RoId == robject.Id)
                    .ToDictionary(x => x.Code, x => x.Value);
        }

        private string GetRoValue(string propertyName, RealityObject robject)
        {
            var property = typeof(RealityObject).GetProperty(propertyName);
            if (property == null)
            {
                throw new ArgumentException(@"Поле не найдено: " + propertyName, "propertyName");
            }

            var value = property.GetValue(robject, null);

            if (value != null)
            {
                if (property.PropertyType.Namespace.Contains("Bars.Gkh.Entities"))
                {
                    if (property.PropertyType == typeof(TypeProject))
                    {
                        var nameProperty = value.GetType().GetProperty("Name");
                        if (nameProperty != null)
                        {
                            return nameProperty.GetValue(value, null).ToStr();
                        }
                    }

                    var idProperty = value.GetType().GetProperty("Id");
                    if (idProperty != null)
                    {
                        return idProperty.GetValue(value, null).ToStr();
                    }
                }
                else if (property.PropertyType == typeof(decimal) || property.PropertyType == typeof(decimal?))
                {
                    return ((decimal) value).RegopRoundDecimal(2).ToStr();
                }
                else if (property.PropertyType == typeof(DateTime) || property.PropertyType == typeof(DateTime?))
                {
                    return ((DateTime) value).ToShortDateString().ToStr();
                }
                else if (property.PropertyType == typeof(bool) || property.PropertyType == typeof(bool?))
                {
                    return ((bool) value) ? "Да" : "Нет";
                }
                else if (property.PropertyType.BaseType == typeof(Enum))
                {
                    return ((int)value).ToString();
                }
            }

            return value.ToStr();
        }

        private Dictionary<string, string> GetRoValues(IEnumerable<string> propertyNames, RealityObject robject)
        {
            return typeof(RealityObject).GetProperties()
                .Where(x => propertyNames.Contains(x.Name))
                .Select(x => new { x.Name, Value = GetRoValue(x.Name, robject).ToStr() })
                .ToDictionary(x => x.Name, x => x.Value);
        }

		private object GetRoPropertyValue(TehPassportValue value, PropertyInfo property)
	    {
			object newValue = null;
			if (property.PropertyType.Namespace.Contains("Bars.Gkh.Entities"))
			{
				var domainType = typeof(IDomainService<>).MakeGenericType(property.PropertyType);
				dynamic domain = Container.Resolve(domainType);

				try
				{
					long id;
					if (long.TryParse(value.Value, out id))
					{
						newValue = domain.Get(id);
					}
				}
				finally
				{
					Container.Release(domain);
				}
			}
			else
			{
				var to = typeof(ObjectParseExtention).GetMethod("To", new[] { typeof(object) }).MakeGenericMethod(property.PropertyType);
				newValue = to.Invoke(null, new object[] { value.Value });
			}

			return newValue;
	    }
    }
}