namespace Bars.Gkh.Import.ImportOktmo
{
	using System;
	using System.Collections.Generic;
	using System.IO;

	using Bars.Gkh.Import;
    using System.Linq;

	using Bars.Gkh.Enums.Import;
	using Bars.B4;

	using Impl;

	using System.Reflection;
	using System.Text;
	using System.Text.RegularExpressions;
	using System.Threading;

	using Bars.B4.Config;
	using Bars.B4.DataAccess;
	using Bars.B4.IoC;
	using Bars.B4.Modules.FIAS;
	using Bars.B4.Modules.Tasks.Common.Service;
	using Bars.B4.Modules.Tasks.Common.Utils;
	using Bars.B4.Utils;
	using Bars.Gkh.Domain;
	using Bars.Gkh.Entities;
	using Bars.Gkh.Entities.Dicts;
	using Bars.GkhExcel;

	using ExecutionContext = Bars.B4.Modules.Tasks.Common.Contracts.ExecutionContext;

	/// <summary>
	/// Импорт ОКТМО для населенных пунктов
	/// </summary>
	public class MunicipalityFiasOktmoImport : GkhImportBase
	{
		public static string Id = MethodBase.GetCurrentMethod().DeclaringType.FullName;

		public override string Key
		{
			get { return Id; }
		}

		public override string CodeImport
		{
			get { return "MunicipalityFiasOktmoImport"; }
		}

		public override string Name
		{
			get { return "Импорт ОКТМО для населенных пунктов"; }
		}

		public override string PossibleFileExtensions
		{
			get { return "xls"; }
		}

		public override string PermissionName
		{
			get { return "Import.MunicipalityFiasOktmo"; }
		}

		public IConfigProvider ConfigProvider { get; set; }
		public IRepository<Municipality> MuRepository { get; set; }
		public IFiasRepository FiasRepository { get; set; }
		public IDomainService<MunicipalityFiasOktmo> MuFiasOktmoDomainService { get; set; }

		private string regionCode;

		private string placeReplacementPattern;
		private string[] exceptedWordsForPlaceNames =
		{
			"п ж/д рзд",
            "п ж/д ст",
			"аал",
			"аул",
			"г",
			"город",
			"д",
			"ж/д станции",
			"ж/д станция",
			"ж/д разъезда",
			"ж/д разъезд",
            "ж/д рзд",
			"ж/д",
			"м",
			"муниципальный округ",
			"нп",
			"п станция",
			"п",
			"п.ст",
			"пгт",
			"поселок",
			"поселок городского типа",
			"рабочий поселок",
			"рзд п",
			"рзд",
			"рп",
			"с",
			"село",
			"Сельское поселение",
			"сельсовет",
			"сл",
			"ст",
			"ст-ца",
			"х"
		};

		private string muReplacementPattern;
		private string[] exceptedWordsForMuNames =
		{
			"муниципальный район",
			"район",
			"р-н"
		};

		private string[] parentTypesForImport =
		{
			"муниципальный район",
			"городской округ",
			"городской округ с внутригородским делением"
		};

		private const int muNameColumnIndex = 7;
		private const int typeColumnIndex = 2;
		private const int oktmoColumnIndex = 5;
		private const int placeNameColumnIndex = 7;

		private MunicipalityRecord currentMuRecord;
		private List<MunicipalityRecord> muRecords = new List<MunicipalityRecord>();

		private Dictionary<string, long> muIdsByNameAndOktmoDict = new Dictionary<string, long>();
		private Dictionary<string, long[]> muIdsByOktmoDict = new Dictionary<string, long[]>();
		private Dictionary<string, string> fiasOffNamesDict = new Dictionary<string, string>();

		private Dictionary<string, PlaceRecordForSave> fiasOktmoForSaveDict = new Dictionary<string, PlaceRecordForSave>();

		public override bool Validate(BaseParams baseParams, out string message)
		{
			message = null;
			if (!baseParams.Files.ContainsKey("FileImport"))
			{
				message = "Не выбран файл для импорта";
				return false;
			}

			var fileData = baseParams.Files["FileImport"];
			var extention = fileData.Extention;

			var fileExtentions = PossibleFileExtensions.Contains(",")
				? PossibleFileExtensions.Split(',')
				: new[] { PossibleFileExtensions };
			if (fileExtentions.All(x => x != extention))
			{
				message = string.Format("Необходимо выбрать файл с допустимым расширением: {0}", PossibleFileExtensions);
				return false;
			}

			return true;
		}

		protected override ImportResult Import(BaseParams baseParams, ExecutionContext ctx, IProgressIndicator indicator, CancellationToken ct)
		{
			var file = baseParams.Files["FileImport"];
			regionCode = ConfigProvider.GetConfig().AppSettings.GetAs<string>("Gkh_RegionCode");
			if (regionCode.IsEmpty())
			{
				return new ImportResult(StatusImport.CompletedWithError, "В конфигурации приложения не задан код региона <Gkh_RegionCode>");
			}

			var resultMessage = "";

			try
			{
				indicator.Indicate(null, 0, "Инициализация импорта");
				InitLog(file.FileName);
				InitRegexPatterns();
				InitDicts();

				indicator.Indicate(null, 10, "Подготовка к распознованию файла");
				ProcessData(file.Data, indicator);

				indicator.Indicate(null, 80, "Подготовка данных для сохранения");
				ProcessRecordsForSave();

				indicator.Indicate(null, 90, "Сохранение данных");
				SaveRecords();
			}
			catch (Exception e)
			{
				LogImport.Error(e.Message, string.Format("Произошла непредвиденная ошибка.\n{0} {1}", e.Message, e));
				resultMessage = string.Format("Произошла непредвиденная ошибка.\n {0} {1}", e.Message, e);
			}

			LogImportManager.Add(file, LogImport);
			LogImportManager.Save();

			resultMessage = resultMessage.IsEmpty() ? string.Format("Загружено строк: {0}. Ошибки: {1}", LogImport.CountAddedRows, LogImport.CountError) : resultMessage;

			return new ImportResult(
				LogImport.CountWarning > 0 ? StatusImport.CompletedWithWarning : StatusImport.CompletedWithoutError,
				resultMessage,
				string.Empty,
				LogImportManager.LogFileId);
		}

		private void InitRegexPatterns()
		{
			placeReplacementPattern = string.Format("^({0})\\s",
					string.Join("|", exceptedWordsForPlaceNames));

			muReplacementPattern = string.Format("(^(.*,\\s)|(г.\\s|город\\s))|(\\s({0})$)",
				string.Join("|", exceptedWordsForMuNames));
		}

		private void InitDicts()
		{
			var muQuery = MuRepository.GetAll()
				.Select(
					x => new
					{
						x.Id,
						x.Name,
						x.Oktmo
					});

			muIdsByNameAndOktmoDict = muQuery
				.AsEnumerable()
				.GroupBy(
					x =>
					{
						var name = Regex.Replace(x.Name, muReplacementPattern, "");
						return string.Format("{0}#{1}", name, x.Oktmo);
					})
				.ToDictionary(x => x.Key, x => x.First().Id);

			muIdsByOktmoDict = muQuery
				.AsEnumerable()
				.GroupBy(x => x.Oktmo)
				.ToDictionary(x => x.Key, x => x.Select(y => y.Id).ToArray());

			var muParentGuids = MuRepository.GetAll()
				.Select(
					x => new
					{
						FiasGuid = x.FiasId ?? x.ParentMo.FiasId
					})
				.AsEnumerable()
				.Select(
					x =>
					{
						var dynamicAddress = x.FiasGuid.IsNotEmpty() ? FiasRepository.GetDinamicAddress(x.FiasGuid) : null;
						var mirrorGuid = dynamicAddress != null ? dynamicAddress.MirrorGuid : null;
						var parentGuidId = dynamicAddress != null ? dynamicAddress.ParentGuidId : null;

						return new
						{
							x.FiasGuid,
							MirrorGuid = mirrorGuid,
							ParentGuidId = parentGuidId
						};
					})
				.ToArray();

			var muFiasGuids = muParentGuids.Where(x => x.FiasGuid != null)
				.Select(x => x.FiasGuid)
				.Distinct()
				.ToArray();

			var muMirrorGuids = muParentGuids.Where(x => x.MirrorGuid != null)
				.Select(x => x.MirrorGuid)
				.Distinct()
				.ToArray();

			var muParentGuidIds = muParentGuids.Where(x => x.ParentGuidId != null)
				.Select(x => x.ParentGuidId)
				.Distinct()
				.ToArray();

			fiasOffNamesDict = FiasRepository.GetAll()
				.Where(x => x.AOLevel == FiasLevelEnum.Place || x.AOLevel == FiasLevelEnum.City)
				.Where(x => x.ActStatus == FiasActualStatusEnum.Actual)
				.Where(x => muFiasGuids.Contains(x.ParentGuid) || muMirrorGuids.Contains(x.ParentGuid) || muParentGuidIds.Contains(x.ParentGuid))
				.Select(
					x => new
					{
						x.AOGuid,
						OffName = x.OffName.Trim().ToLower(),
                        x.OKTMO
					})
				.AsEnumerable()
				.Select(
					x =>
					{
						var offname = Regex.Replace(x.OffName, placeReplacementPattern, "");
						return new
						{
							x.AOGuid,
							OffName = $"{offname}#{x.OKTMO}"
						};
					})
				.GroupBy(x => x.OffName)
				.ToDictionary(x => x.Key, x => x.First().AOGuid);
		}

		private void ProcessData(byte[] fileData, IProgressIndicator indicator)
		{
			muRecords.Clear();

			try
			{
			    var excelProvider = this.Container.Resolve<IGkhExcelProvider>("ExcelEngineProvider");
			    using (this.Container.Using(excelProvider))
                {
					if (excelProvider == null)
					{
						throw new ImportException("Не найдена реализация интерфейса IGkhExcelProvider");
					}

					using (var fileStream = new MemoryStream(fileData))
					{
						fileStream.Seek(0, SeekOrigin.Begin);
						excelProvider.Open(fileStream);

						var pagesCount = excelProvider.GetWorkSheetsCount(0);
                        var startIndex = 3;
						for (int i = 0; i < pagesCount; i++)
						{
							var percentage = 10 + ((float)i * 60) / pagesCount;
							indicator.Indicate(null, (uint) (percentage > 70 ? 70 : percentage), string.Format("Обработка {0} листа", i));

							ProcessPage(excelProvider, startIndex, i);
						}
					}
				}
			}
			catch (Exception ex)
			{
				throw new Exception("Некорректный файл", ex);
			}
		}

		private void ProcessPage(IGkhExcelProvider excelProvider, int startIndex, int currentPage)
		{
			for (var i = startIndex; i < excelProvider.GetRowsCount(0, currentPage); i++)
			{
				var row = excelProvider.GetRow(0, currentPage, i);
				if (!IsValidRegion(row))
				{
					continue;
				}

				ProcessRow(row);
			}
		}

		private bool IsValidRegion(GkhExcelCell[] row)
		{
			var oktmoCell = row[oktmoColumnIndex];
			if (oktmoCell == null || string.IsNullOrEmpty(oktmoCell.Value))
			{
				return false;
			}

			var oktmoRegionCode = oktmoCell.Value.Substring(0, 2);
			return oktmoRegionCode == regionCode;
		}

		private void ProcessRow(GkhExcelCell[] row)
		{
			string oktmo = row[oktmoColumnIndex].Value;

			var type = row[typeColumnIndex].Value;
			if (parentTypesForImport.Contains(type))
			{
				var muName = (row[muNameColumnIndex].Value ?? "");
				muName = Regex.Replace(muName, muReplacementPattern, "");

				currentMuRecord = new MunicipalityRecord(muName, oktmo);
				muRecords.Add(currentMuRecord);
			}
			else
			{
				//грузим только 11 значный октмо
				if (oktmo.Length != 11)
				{
					return;
				}

				var placeName = (row[placeNameColumnIndex].Value ?? "");
				placeName = Regex.Replace(placeName, placeReplacementPattern, "");

				var placeRecord = new PlaceRecord(placeName, oktmo);
				if (currentMuRecord != null)
				{
					currentMuRecord.Places.Add(placeRecord);
				}
			}
		}

		private void ProcessRecordsForSave()
		{
			foreach (var muRecord in muRecords)
			{
				long[] muIds;

				var muRecordkey = string.Format("{0}#{1}", muRecord.Name, muRecord.Oktmo);
				if (muIdsByNameAndOktmoDict.ContainsKey(muRecordkey))
				{
					muIds = new[] {muIdsByNameAndOktmoDict[muRecordkey]};
				}
				else if (muIdsByOktmoDict.ContainsKey(muRecord.Oktmo))
				{
					muIds = muIdsByOktmoDict[muRecord.Oktmo];
				}
				else
				{
					LogImport.Warn(
						muRecord.Name,
						string.Format("Муниципальный район {0} с ОКТМО {1} не найден в дереве муниципальных образований", muRecord.Name, muRecord.Oktmo));

					continue;
				}

				foreach (var placeRecord in muRecord.Places)
				{
					var placeName = placeRecord.Name.ToLower();
				    var key = $"{placeName}#{placeRecord.Oktmo.Substring(0,8)}";
					if (!fiasOffNamesDict.ContainsKey(key))
					{
						LogImport.Warn(
							placeRecord.Name,
							string.Format("Населенный пункт {0} не найден в справочнике ФИАС", placeRecord.Name));

						continue;
					}

					var aoGuid = fiasOffNamesDict[key];
					foreach (var muId in muIds)
					{
						var muFiasKey = string.Format("{0}#{1}", muId, aoGuid);
						if (!fiasOktmoForSaveDict.ContainsKey(muFiasKey))
						{
							fiasOktmoForSaveDict.Add(
								muFiasKey,
								new PlaceRecordForSave(placeRecord.Name, muRecord.Name, aoGuid, muId, placeRecord.Oktmo));
						}
					}
				}
			}
		}

		private void SaveRecords()
		{
			var muIds = fiasOktmoForSaveDict.Values
				.Select(x => x.MunicipalityId)
				.Distinct()
				.ToArray();

			var fiasGuids = fiasOktmoForSaveDict.Values
				.Select(x => x.FiasGuid)
				.Distinct()
				.ToArray();

			var existingEntities = MuFiasOktmoDomainService.GetAll()
				.Where(x => muIds.Contains(x.Municipality.Id))
				.Where(x => fiasGuids.Contains(x.FiasGuid))
				.AsEnumerable()
				.GroupBy(x => string.Format("{0}#{1}", x.Municipality.Id, x.FiasGuid))
				.ToDictionary(x => x.Key, x => x.First());

			Container.InTransaction(() =>
			{
				foreach (var forSave in fiasOktmoForSaveDict)
				{
					var existingRecord = existingEntities.Get(forSave.Key);
					if (existingRecord == null)
					{
						var newRecord = new MunicipalityFiasOktmo
						{
							FiasGuid = forSave.Value.FiasGuid,
							Municipality = new Municipality {Id = forSave.Value.MunicipalityId},
							Oktmo = forSave.Value.Oktmo
						};

						MuFiasOktmoDomainService.Save(newRecord);

						LogImport.CountAddedRows++;
						LogImport.Info(
							forSave.Value.MunicipalityName,
							string.Format("Добавлено ОКТМО {0} и населенный пункт {1} с FiasGuid {2}", forSave.Value.Oktmo, forSave.Value.Name, forSave.Value.FiasGuid));
					}
					else
					{
						existingRecord.Oktmo = forSave.Value.Oktmo;
						MuFiasOktmoDomainService.Update(existingRecord);

						LogImport.CountChangedRows++;
						LogImport.Info(
							forSave.Value.MunicipalityName,
							string.Format("Обновлено ОКТМО {0} для населенного пункта {1} с FiasGuid {2}", forSave.Value.Oktmo, forSave.Value.Name, forSave.Value.FiasGuid));
					}
				}
			});
		}
		
		private class MunicipalityRecord
		{
			public MunicipalityRecord(string name, string oktmo)
			{
				Name = name;
				Oktmo = oktmo;
				Places = new List<PlaceRecord>();
			}

			public string Name { get; }
			public string Oktmo { get; }
			public List<PlaceRecord> Places { get; }
		}

		private class PlaceRecord
		{
			public PlaceRecord(string name, string oktmo)
			{
				Name = name;
				Oktmo = oktmo;
			}

			public string Name { get; }
			public string Oktmo { get; }
		}

		private class PlaceRecordForSave
		{
			public PlaceRecordForSave(string name, string municipalityName, string fiasGuid, long municipalityId, string oktmo)
			{
				Name = name;
				MunicipalityName = municipalityName;
				FiasGuid = fiasGuid;
				MunicipalityId = municipalityId;
				Oktmo = oktmo;
			}

			public string Name { get; }
			public string MunicipalityName { get; }
			public string FiasGuid { get; }
			public long MunicipalityId { get; }
			public string Oktmo { get; }
        }
	}
}