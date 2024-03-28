namespace Bars.Gkh.RegOperator.Domain.ImportExport.Serializers
{
    using B4.DataAccess;
    using B4.Utils;
    using Bars.B4.IoC;
    using Bars.Gkh.Entities;
    using Castle.Windsor;
    using Entities;
    using FastMember;
    using Mapping;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Web;
    using Wcf.Contracts.PersonalAccount;

    /// <summary>
    /// Сериализатор для FsGorodPaymentInfo
    /// </summary>
    public class FsGorodPaymentInfoSerializer : DefaultImportExportSerializer<PersonalAccountPaymentInfoIn>
	{
		/// <summary>
		/// Конструктор сериализатора
		/// </summary>
		/// <param name="container">Контейнер</param>
		public FsGorodPaymentInfoSerializer(IWindsorContainer container)
			: base(container)
		{
		}

		/// <summary>
		/// Десериализация
		/// </summary>
		/// <param name="data">Поток данных</param>
		/// <param name="format">Формат</param>
		/// <param name="fileName">Имя файла</param>
		/// <param name="extraParams">Дополнительные параметры</param>
		/// <returns>результат импорта</returns>
		public override ImportResult<PersonalAccountPaymentInfoIn> Deserialize(Stream data,
			IImportMap format,
			string fileName = null,
			DynamicDictionary extraParams = null)
		{
			var infoCode = extraParams.Return(x => x.GetAs<string>("FsGorodCode", ignoreCase: true));

			FsGorodImportInfo info;

			var infoRepo = Container.ResolveRepository<FsGorodImportInfo>();
			using (Container.Using(infoRepo))
			{
				info = infoRepo.GetAll().SingleOrDefault(x => x.Code == infoCode);
			}

			var result = new ImportResult<PersonalAccountPaymentInfoIn>();

			if (info == null)
			{
				return result;
			}

            // Тип универсального импорта
		    result.GeneralData["FsGorodName"] = info.Name;

            var listResult = new List<ImportRow<PersonalAccountPaymentInfoIn>>();

			using (var reader = new StreamReader(data, Encoding.GetEncoding(1251)))
			{
				int currentIndex;

				FsGorodMapItem[] mapItems;

				var mapItemRepo = Container.ResolveRepository<FsGorodMapItem>();
				using (Container.Using(mapItemRepo))
				{
					mapItems = mapItemRepo.GetAll().Where(x => x.ImportInfo == info).ToArray();
				}

				var metaRows = GetMeta(reader, mapItems.Where(x => x.IsMeta), fileName, out currentIndex);
				var dataRows = GetDataRows(
					reader,
					info,
					mapItems.Where(x => !x.IsMeta).ToList(),
					fileName,
					currentIndex);

				var dataAccessor = TypeAccessor.Create(typeof(PersonalAccountPaymentInfoIn));
				var dataPropertyBag = typeof(PersonalAccountPaymentInfoIn).GetProperties().ToDictionary(x => x.Name);

				var metaAccessor = TypeAccessor.Create(typeof(ImportResult<PersonalAccountPaymentInfoIn>));
				var metaMembers = typeof(ImportResult<PersonalAccountPaymentInfoIn>).GetProperties().ToDictionary(x => x.Name);

				foreach (var metaRow in metaRows)
				{
					if (metaRow.HasError)
					{
						continue;
					}

					PropertyInfo member;
					if (metaMembers.TryGetValue(metaRow.Name, out member))
					{
						metaAccessor[result, metaRow.Name] = metaRow.GetValue(member.PropertyType);
					}
					else
					{
						result.GeneralData[metaRow.Name] = metaRow.RawValue;
					}
				}

				foreach (var dataRow in dataRows)
				{
					var errorBuilder = new StringBuilder();
					var warningBuilder = new StringBuilder();

					var proxyItem = new PersonalAccountPaymentInfoIn();

					if (dataRow.HasError)
					{
						errorBuilder.AppendLine(
							string.Format(
								"Строка: {0}. Ошибка: {1}",
								dataRow.RowLine,
								dataRow.Error));
					}
					else
					{
						var allColumns = dataRow.Columns.Union(metaRows);

						foreach (var column in allColumns)
						{
							if (dataPropertyBag.ContainsKey(column.Name))
							{
                                var propertyInfo = dataPropertyBag[column.Name];

							    var value = column.GetValue(propertyInfo.PropertyType);
                                    
                                if (column.HasWarning)
								{
									warningBuilder.AppendLine(FormatWarning(dataRow, propertyInfo, column.Warning));

									continue;
								}
								if (column.HasError)
								{
									errorBuilder.AppendLine(FormatError(dataRow, propertyInfo, column.Error));

									continue;
								}

                                dataAccessor[proxyItem, column.Name] = value;
							}
						}
					}

					listResult.Add(new ImportRow<PersonalAccountPaymentInfoIn>
					{
						Value = proxyItem,
						Error = errorBuilder.ToString(),
						Warning = warningBuilder.ToString()
					});
				}
			}

			var paymentAgentDomain = Container.ResolveDomain<PaymentAgent>();
			using (Container.Using(paymentAgentDomain))
			{
                var mapItemPaymentAgent = info.MapItems.FirstOrDefault(x => x.PaymentAgent != null);

			    if (mapItemPaymentAgent != null)
			    {
			        result.GeneralData["AgentId"] = mapItemPaymentAgent.PaymentAgent.Code;
                    result.GeneralData["AgentName"] = mapItemPaymentAgent.PaymentAgent.Contragent.Name;
			    }
			    else
                {
                    var agentId = result.GeneralData["AgentId"].ToStr();

                    result.GeneralData["AgentName"] = paymentAgentDomain.GetAll()
                        .Where(x => x.Code == agentId)
                        .Select(x => x.Contragent.Name)
                        .FirstOrDefault();
                }

				
			}

			result.Rows = listResult;
			return result;
		}

		private IEnumerable<DataRow> GetDataRows(StreamReader data, FsGorodImportInfo info, List<FsGorodMapItem> mapItems, string fileName, int currentIndex)
		{
			var tmpIndex = info.DataHeadIndex;
			while (currentIndex < tmpIndex)
			{
				data.ReadLine();
				++currentIndex;
			}

			string line;
            while (!data.EndOfStream)
            {
                ++currentIndex;
                if ((line = data.ReadLine()).IsNotEmpty())
                {
                    yield return new DataRow(currentIndex, line, mapItems);
                }
            }
        }

		private List<DataColumn> GetMeta(StreamReader data, IEnumerable<FsGorodMapItem> mapInfo, string fileName, out int currentIndex)
		{
			var result = new List<DataColumn>();

			var dependsOnFileName = mapInfo.Where(x => x.UseFilename).ToList();
			foreach (var mapItem in dependsOnFileName)
			{
				if (fileName.IsEmpty())
					break;

				result.Add(new DataColumn(mapItem.PropertyName, fileName, mapItem));
			}

			var stack = mapInfo.Where(x => !x.UseFilename).OrderBy(x => x.Index);
			var localCounter = 0;

			foreach (var mapGroup in stack.GroupBy(x => x.Index))
			{
				while (localCounter != mapGroup.Key)
				{
					data.ReadLine();
					++localCounter;
				}

				var line = data.ReadLine();

			    result.AddRange(mapGroup.Select(mapItem => new DataColumn(mapItem.PropertyName, line, mapItem)));

			    ++localCounter;
			}

			while (Convert.ToChar(data.Peek()) == '#')
			{
				data.ReadLine();
				++localCounter;
			}

			currentIndex = localCounter;

			return result;
		}

		private string FormatWarning(DataRow dataRow, PropertyInfo propertyInfo, string warning)
		{
			return "Строка: {0}. Свойство: {1}. Предупреждение: {2}".FormatUsing(
				dataRow.RowLine,
				propertyInfo.GetAttribute<DisplayAttribute>(true).Return(x => x.Value),
				warning);
		}

		private string FormatError(DataRow dataRow, PropertyInfo propertyInfo, string error)
		{
			return "Строка: {0}. Свойство: {1}. Ошибка: {2}".FormatUsing(
				dataRow.RowLine,
				propertyInfo.GetAttribute<DisplayAttribute>(true).Return(x => x.Value),
				error);
		}

		private class DataRow
		{
			private readonly string _delimiter;

			/// <summary>
			/// Флаг ошибки строки данных.
			/// </summary>
			public bool HasError
			{
				get { return Error.IsNotEmpty(); }
			}

			/// <summary>
			/// Текст ошибки строки данных.
			/// </summary>
			public string Error { get; private set; }

			public int RowLine { get; private set; }

			public List<DataColumn> Columns { get; private set; }

			public DataRow(int rowLine, string line, List<FsGorodMapItem> items)
			{
				RowLine = rowLine;
				Columns = new List<DataColumn>();
				_delimiter = items.FirstOrDefault().Return(x => x.ImportInfo).Return(x => x.Delimiter);

				_delimiter = _delimiter.IsNotEmpty() ? _delimiter : ";";

				ConvertToColumns(line, items);
			}

			public DataColumn this[string name]
			{
				get { return Columns.FirstOrDefault(x => x.Name == name); }
			}

			private void ConvertToColumns(string line, List<FsGorodMapItem> items)
			{
				var splits = line.Split(new[] { _delimiter }, StringSplitOptions.None).ToArray();

				foreach (var mapItem in items)
				{
					if (splits.Length > mapItem.Index)
					{
						Columns.Add(new DataColumn(mapItem.PropertyName, splits[mapItem.Index], mapItem));
					}
					else
					{
						Error = string.Format(
							"Для свойства ({0}) отсутствуют данные в столбце ({1})",
							mapItem.PropertyName,
							mapItem.Index);
						return;
					}
				}
			}
		}

		private class DataColumn
		{
			private string _rawValue;
			private readonly FsGorodMapItem _mapItem;

			public string Name { get; private set; }

			public string RawValue
			{
				get
				{
					if (HasError)
						throw new InvalidOperationException();

					return _rawValue;
				}
			}

			public string Error { get; private set; }

			public string Warning { get; private set; }

			public bool HasError
			{
				get { return Error.IsNotEmpty(); }
			}

			public bool HasWarning
			{
				get { return Warning.IsNotEmpty(); }
			}

			public DataColumn(string name, string value, FsGorodMapItem mapItem)
			{
				Name = name;
				_mapItem = mapItem;

				ConvertValue(value);
			}

			public object GetValue(Type propertyType)
			{
			    if (HasError)
			    {
                    return Activator.CreateInstance(propertyType);
			    }

				if (propertyType.Is(typeof(DateTime)))
				{
					DateTime date = DateTime.MinValue;
                    
					if (_mapItem.Format.IsNotEmpty()
						&& DateTime.TryParseExact
                            (_rawValue, _mapItem.Format, CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
					{
						return date;
					}

				    if (!DateTime.TryParse(_rawValue, out date) || date == DateTime.MinValue)
				    {
				        this.Error = "Значение столбца не соответствует его описанию.";
                    }

				    return date;
				}

                var value = ConvertHelper.ConvertTo(_rawValue, propertyType);

			    if (propertyType.IsValueType && value == null)
			    {
                    throw new Exception("Значение столбца не соответствует его описанию.");
			    }

			    return value;
			}

			private void ConvertValue(string rawValue)
			{
				if (_mapItem.Regex.IsNotEmpty())
				{
					var regex = HttpUtility.HtmlDecode(_mapItem.Regex);

					var match = Regex.Match(rawValue, regex);

					if (!match.Success && !_mapItem.Required)
					{
						Warning = _mapItem.ErrorText.IsNotEmpty()
							? "Свойство ({0}). Предупреждение: {1}".FormatUsing(_mapItem.PropertyName, _mapItem.ErrorText)
							: "Значение свойства \"{0}\" ({1}) не соответсвует регулярному выражению \"{2}\""
								.FormatUsing(_mapItem.PropertyName, rawValue, regex);

						Warning = string.Concat(Warning, "Позиция ({0})".FormatUsing(_mapItem.Index));
						_rawValue = "";

						return;
					}

					if (_mapItem.GetValueFromRegex)
					{
						_rawValue = match.Groups[_mapItem.PropertyName].Success
							? match.Groups[_mapItem.PropertyName].Value
							: match.Groups.Count > 1 // Если в регулярке есть группа (), то она будет не первой в списке групп
								? match.Groups[1].Value
								: match.Groups[0].Value;
					}
					else
					{
						_rawValue = _mapItem.RegexSuccessValue;
					}
				}
				else
				{
					_rawValue = rawValue;
				}

				if (_mapItem.Required && _rawValue.IsEmpty())
				{
					Error = "Свойство ({0}) не может быть пустым. Позиция ({1})".FormatUsing(_mapItem.PropertyName, _mapItem.Index);
				}
			}
		}
	}
}