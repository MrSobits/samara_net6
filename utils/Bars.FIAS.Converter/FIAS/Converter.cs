namespace Bars.FIAS.Converter
{
    using System;
    using System.IO;
    using System.Collections.Generic;
    using System.Linq;
    using System.Data.OleDb;
    using System.ComponentModel;
    using System.Text.RegularExpressions;

    public enum EnumTypeDataBase
    {
        PostgreSql,
    }

    public enum EnumTypeOLEDB
    {
        Microsoft_Jet_OLEDB_4_0,

        Microsoft_ACE_OLEDB_12_0
    }

    public class Region
    {
        public Region(string code, string aoGuid)
        {
            this.Code = code;
            this.AoGuid = aoGuid;
        }

        public string Code { get; private set; }
        public string AoGuid { get; private set; }
    }

    public class Converter
    {

        public const string MainFilePattern = "ADDROB*.DBF";

        public const string DefaultConnectionString ="Server=localhost;Database=bars_b4gkh;User ID=bars_b4gkh;Password=1";


        public readonly SortedDictionary<string, string> Regions = new SortedDictionary<string, string>();

        #region Тестирование
        public bool TestConnection(string connectionString)
        {
            return (new NpgsqlWorker(connectionString)).TestConnection();
        }
        #endregion

        /// <summary>
        /// Метод получения Записей из ФИАС DBF
        /// </summary>
        /// <param name="path"></param>
        /// <param name="worker"></param>
        /// <param name="selectedRegions"></param>
        /// <returns></returns>
        public List<FiasRecord> GetLoadFiasRecords(BackgroundWorker worker, string path, EnumTypeOLEDB typeOLEDB, List<string> selectedRegions)
        {
            worker.ReportProgress(15);

            //загружаемые записи
            var result = new List<FiasRecord>();

            var oledbProvider = "Microsoft.Jet.OLEDB.4.0";
            if (typeOLEDB == EnumTypeOLEDB.Microsoft_ACE_OLEDB_12_0)
            {
                oledbProvider = "Microsoft.ACE.OLEDB.12.0";
            }

            #region OleDB получаем список записей для загрузки по Регионам из DBF
            using (var conn = new OleDbConnection($"Provider={oledbProvider};Data Source={path};Extended Properties=dBase IV"))
            {
                conn.Open();

                #region Формируем записи ФИАС
                using (var command = conn.CreateCommand())
                {
                    foreach (var code in selectedRegions)
                    {
                        command.CommandText = $@"
                        SELECT AOGUID, FORMALNAME, REGIONCODE, AUTOCODE, AREACODE, CITYCODE,
                            CTARCODE, PLACECODE, STREETCODE, EXTRCODE, SEXTCODE, OFFNAME, POSTALCODE,
                            IFNSFL, TERRIFNSFL, IFNSUL, TERRIFNSUL, OKATO, OKTMO, UPDATEDATE, SHORTNAME,
                            AOLEVEL, PARENTGUID, AOID, PREVID, NEXTID, CODE, PLAINCODE, ACTSTATUS, CENTSTATUS,
                            OPERSTATUS, CURRSTATUS, STARTDATE, ENDDATE, NORMDOC
                        FROM addrob{code}";
                        var reader = command.ExecuteReader();

                        while (reader.Read() && !worker.CancellationPending)
                        {
                            var array = new object[35];
                            reader.GetValues(array);

                            var kladrPlainCode = (array[27] is DBNull ? string.Empty : (string)array[27]);
                            if (kladrPlainCode.Length < 15)
                            {
                                while (kladrPlainCode.Length < 15)
                                {
                                    kladrPlainCode += "0";
                                }
                            }

                            var record = new FiasRecord()
                            {
                                AOGuid = (array[0] is DBNull ? string.Empty : (string)array[0]),
                                FormalName = (array[1] is DBNull ? string.Empty : (string)array[1]),
                                CodeRegion = (array[2] is DBNull ? string.Empty : (string)array[2]),
                                CodeAuto = (array[3] is DBNull ? string.Empty : (string)array[3]),
                                CodeArea = (array[4] is DBNull ? string.Empty : (string)array[4]),
                                CodeCity = (array[5] is DBNull ? string.Empty : (string)array[5]),
                                CodeCtar = (array[6] is DBNull ? string.Empty : (string)array[6]),
                                CodePlace = (array[7] is DBNull ? string.Empty : (string)array[7]),
                                CodeStreet = (array[8] is DBNull ? string.Empty : (string)array[8]),
                                CodeExtr = (array[9] is DBNull ? string.Empty : (string)array[9]),
                                CodeSext = (array[10] is DBNull ? string.Empty : (string)array[10]),
                                OffName = (array[11] is DBNull ? string.Empty : (string)array[11]),
                                PostalCode = (array[12] is DBNull ? string.Empty : (string)array[12]),
                                IFNSFL = (array[13] is DBNull ? string.Empty : (string)array[13]),
                                TerrIFNSFL = (array[14] is DBNull ? string.Empty : (string)array[14]),
                                IFNSUL = (array[15] is DBNull ? string.Empty : (string)array[15]),
                                TerrIFNSUL = (array[16] is DBNull ? string.Empty : (string)array[16]),
                                OKATO = (array[17] is DBNull ? string.Empty : (string)array[17]),
                                OKTMO = (array[18] is DBNull ? string.Empty : (string)array[18]),
                                UpdateDate = (DateTime?)array[19],
                                ShortName = (array[20] is DBNull ? string.Empty : (string)array[20]),
                                AOLevel = (int)(double)array[21],
                                ParentGuid = (array[22] is DBNull ? string.Empty : (string)array[22]),
                                AOId = (array[23] is DBNull ? string.Empty : (string)array[23]),
                                PrevId = (array[24] is DBNull ? string.Empty : (string)array[24]),
                                NextId = (array[25] is DBNull ? string.Empty : (string)array[25]),
                                KladrCode = (array[26] is DBNull ? string.Empty : (string)array[26]),
                                KladrPlainCode = kladrPlainCode,
                                ActStatus = (int)(double)array[28],
                                CentStatus = (int)(double)array[29],
                                OperStatus = (int)(double)array[30],
                                KladrCurrStatus = (int)(double)array[31],
                                StartDate = (DateTime?)array[32],
                                EndDate = (DateTime?)array[33],
                                NormDoc = (array[34] is DBNull ? string.Empty : (string)array[34]),
                            };

                            record.CodeRecord = record.GetCodeRecord();
                            result.Add(record);
                        }
                    }
                }
                #endregion
            }
            #endregion

            return result;
        }

        /// <summary>
        /// Метод получения Записей домов из ФИАС DBF
        /// </summary>
        /// <param name="path"></param>
        /// <param name="worker"></param>
        /// <param name="selectedRegions"></param>
        /// <returns></returns>
        public List<FiasHouseRecord> GetLoadFiasHouseRecords(BackgroundWorker worker, string path, EnumTypeOLEDB typeOLEDB, List<string> selectedRegions)
        {
            worker.ReportProgress(15);

            //загружаемые записи
            var result = new List<FiasHouseRecord>();

            var oledbProvider = "Microsoft.Jet.OLEDB.4.0";
            if (typeOLEDB == EnumTypeOLEDB.Microsoft_ACE_OLEDB_12_0)
                oledbProvider = "Microsoft.ACE.OLEDB.12.0";

            #region OleDB получаем список записей для загрузки по Регионам из DBF
            using (var conn = new OleDbConnection($"Provider={oledbProvider};Data Source={path};Extended Properties=dBase IV"))
            {
                conn.Open();

                #region Формируем записи ФИАС

                foreach (var code in selectedRegions)
                {
                    var dbFileName = $"house{code}";
                    if (!File.Exists(Path.Combine(path, $"{dbFileName}.dbf")))
                    {
                        continue;
                    }

                    using (var command = conn.CreateCommand())
                    {
                        command.CommandText = $@"
                                SELECT HOUSEID, HOUSEGUID, AOGUID, POSTALCODE,
                                    OKATO, OKTMO, HOUSENUM, BUILDNUM, STRUCNUM,
                                    STATSTATUS, UPDATEDATE, STARTDATE, ENDDATE
                                FROM {dbFileName}";

                        var reader = command.ExecuteReader();

                        while (reader.Read() && !worker.CancellationPending)
                        {
                            var array = new object[13];
                            reader.GetValues(array);

                            var record = new FiasHouseRecord()
                            {
                                HouseId = (array[0] is DBNull ? string.Empty : (string) array[0]),
                                HouseGuid = (array[1] is DBNull ? string.Empty : (string) array[1]),
                                AoGuid = (array[2] is DBNull ? string.Empty : (string) array[2]),
                                PostalCode = (array[3] is DBNull ? string.Empty : (string) array[3]),
                                Okato = (array[4] is DBNull ? string.Empty : (string) array[4]),
                                Oktmo = (array[5] is DBNull ? string.Empty : (string) array[5]),
                                HouseNum = (array[6] is DBNull ? string.Empty : (string) array[6]),
                                BuildNum = (array[7] is DBNull ? string.Empty : (string) array[7]),
                                StrucNum = (array[8] is DBNull ? string.Empty : (string) array[8]),
                                ActualStatus = (int) (double) array[9],
                                UpdateDate = (DateTime?) array[10],
                                StartDate = ((DateTime?) array[11]).GetValueOrDefault(),
                                EndDate = ((DateTime?) array[12]).GetValueOrDefault(),
                            };

                            result.Add(record);
                        }
                    }
                }

                #endregion
            }
            #endregion

            return result;
        }


        #region Метод загрузки
        /// <summary>
        /// Основная точка входа для загрузки данных в ФИАС
        /// </summary>
        /// <param name="typeLoad">тип загрузки</param>
        /// <param name="connectionString">Строка подключения к БД</param>
        /// <param name="fiasRecords">Список Id регионов, которые необходимо загрузить</param>
        /// <param name="fiasHouseRecords">Список Id домов, которые необходимо загрузить</param>
        /// <returns></returns>
        public bool LoadFias(BackgroundWorker worker, int typeLoad, string connectionString, List<FiasRecord> fiasRecords, List<FiasHouseRecord> fiasHouseRecords)
        {
            worker.ReportProgress(0);

            IDbWorker dbWorker= new NpgsqlWorker(connectionString) { BackgroundWorker = worker };

            //Проверяем есть ли соединение с переданным конекшеном
            if (!dbWorker.TestConnection())
            {
                return false;
            }

            switch (typeLoad)
            {
                case 0:
                {
                    //Если тип = 0, то
                    //1. Удаляем все записи ФИАС из Базы
                    //2. Загружаем новые записи

                    worker.ReportProgress(30);
                    dbWorker.DeleteRecords();

                    worker.ReportProgress(50);
                    dbWorker.InsertRecords(fiasRecords, fiasHouseRecords);
                }
                break;

                case 1:
                {
                    //Если тип = 1, то
                    //1. Получаем список идентификаторов ФИАС существующие
                    //2. Для существующих записей делаем Update
                    //3. Для несуществующих делаем Insert

                    //1.
                    worker.ReportProgress(30);
                    var currFiasRecords = dbWorker.GetCurrentFiasRecords();

                    var updateFiasRecords = new List<FiasRecord>();
                    var insertFiasRecords = new List<FiasRecord>();

                    foreach (var rec in fiasRecords)
                    {
                        if(currFiasRecords.ContainsKey(rec.AOId))
                        {
                            //Если есть запись то добавляем ее в список на изменение
                            //Если дата обновления в БД больше чем та что загружается то не добавляем запись
                            if (currFiasRecords[rec.AOId].UpdateDate < rec.UpdateDate)
                            {
                                rec.Id = currFiasRecords[rec.AOId].Id;
                                updateFiasRecords.Add(rec);
                            }
                        }
                        else
                        {
                            insertFiasRecords.Add(rec);
                        }
                    }

                    //1.1.
                    worker.ReportProgress(40);
                    var currFiasHouseRecords = dbWorker.GetCurrentFiasHouseRecords();

                    var updateFiasHouseRecords = new List<FiasHouseRecord>();
                    var insertFiasHouseRecords = new List<FiasHouseRecord>();

                    foreach (var rec in fiasHouseRecords)
                    {
                        if (currFiasHouseRecords.ContainsKey(rec.HouseId))
                        {
                            //Если есть запись то добавляем ее в список на изменение
                            //Если дата обновления в БД больше чем та что загружается то не добавляем запись
                            if (currFiasHouseRecords[rec.HouseId].UpdateDate < rec.UpdateDate)
                            {
                                rec.Id = currFiasHouseRecords[rec.HouseId].Id;
                                updateFiasHouseRecords.Add(rec);
                            }
                        }
                        else
                        {
                            insertFiasHouseRecords.Add(rec);
                        }
                    }

                    //2.
                    worker.ReportProgress(50);
                    if(updateFiasRecords.Any() || updateFiasHouseRecords.Any())
                    {
                        dbWorker.UpdateFiasRecords(updateFiasRecords, updateFiasHouseRecords);
                    }

                    worker.ReportProgress(70);
                    //3.
                    if (insertFiasRecords.Any() || insertFiasHouseRecords.Any())
                    {
                        dbWorker.InsertRecords(insertFiasRecords, insertFiasHouseRecords);
                    }
                }
                break;
            }

            return true;
        }
        #endregion

        /// <summary>
        /// Получить список регионов, возвращает отсортированные словарь с именем и кодом региона
        /// </summary>
        /// <param name="path">Путь к файлам КЛАДР</param>
        /// <param name="worker">BackgroundWorker</param>
        /// <param name="e">DoWorkEventArgs</param>
        /// <returns></returns>
        public SortedDictionary<string, string> GetRegionList(string path, EnumTypeOLEDB typeOLEDB, BackgroundWorker worker, DoWorkEventArgs e)
        {
            this.Regions.Clear();

            if (string.IsNullOrEmpty(path))
            {
                throw new Exception("Не указан путь к исходным файлам");
            }

            var files = Directory.GetFiles(path, Converter.MainFilePattern);

            if (!files.Any())
            {
                throw new Exception($"В указанной папке нет файлов, содержащих основные адресные объекты (ADDROB*.DBF): \"{path}\"");
            }

            var regex = new Regex(@"ADDROB(\d+).DBF", RegexOptions.Compiled | RegexOptions.IgnoreCase);

            var dbTables = files.Select(x => regex.Match(x).Groups[1].Value);

            foreach (var kvp in this.regionCodeDict.Where(x => dbTables.Contains(x.Key)))
            {
                var key = kvp.Value + " [код: " + kvp.Key + "]";
                this.Regions.Add(key, kvp.Key);
            }

            return this.Regions;
        }

        private string GetValue(OleDbDataReader reader, int number)
        {
            string result = null;
            if (!reader.IsDBNull(number))
            {
                result = reader.GetString(number).TrimEnd();
            }

            return result ?? string.Empty;
        }

        private readonly IDictionary<string, string> regionCodeDict = new Dictionary<string, string>
        {
            { "01", "Республика Адыгея (Адыгея)" },
            { "02", "Республика Башкортостан" },
            { "03", "Республика Бурятия" },
            { "04", "Республика Алтай" },
            { "05", "Республика Дагестан" },
            { "06", "Республика Ингушетия" },
            { "07", "Кабардино-Балкарская Республика" },
            { "08", "Республика Калмыкия" },
            { "09", "Карачаево-Черкесская Республика" },
            { "10", "Республика Карелия" },
            { "11", "Республика Коми" },
            { "12", "Республика Марий Эл" },
            { "13", "Республика Мордовия" },
            { "14", "Республика Саха (Якутия)" },
            { "15", "Республика Северная Осетия - Алания" },
            { "16", "Республика Татарстан (Татарстан)" },
            { "17", "Республика Тыва" },
            { "18", "Удмуртская Республика" },
            { "19", "Республика Хакасия" },
            { "20", "Чеченская Республика" },
            { "21", "Чувашская Республика - Чувашия" },
            { "22", "Алтайский край" },
            { "23", "Краснодарский край" },
            { "24", "Красноярский край" },
            { "25", "Приморский край" },
            { "26", "Ставропольский край" },
            { "27", "Хабаровский край" },
            { "28", "Амурская область" },
            { "29", "Архангельская область" },
            { "30", "Астраханская область" },
            { "31", "Белгородская область" },
            { "32", "Брянская область" },
            { "33", "Владимирская область" },
            { "34", "Волгоградская область" },
            { "35", "Вологодская область" },
            { "36", "Воронежская область" },
            { "37", "Ивановская область" },
            { "38", "Иркутская область" },
            { "39", "Калининградская область" },
            { "40", "Калужская область" },
            { "41", "Камчатский край" },
            { "42", "Кемеровская область" },
            { "43", "Кировская область" },
            { "44", "Костромская область" },
            { "45", "Курганская область" },
            { "46", "Курская область" },
            { "47", "Ленинградская область" },
            { "48", "Липецкая область" },
            { "49", "Магаданская область" },
            { "50", "Московская область" },
            { "51", "Мурманская область" },
            { "52", "Нижегородская область" },
            { "53", "Новгородская область" },
            { "54", "Новосибирская область" },
            { "55", "Омская область" },
            { "56", "Оренбургская область" },
            { "57", "Орловская область" },
            { "58", "Пензенская область" },
            { "59", "Пермский край" },
            { "60", "Псковская область" },
            { "61", "Ростовская область" },
            { "62", "Рязанская область" },
            { "63", "Самарская область" },
            { "64", "Саратовская область" },
            { "65", "Сахалинская область" },
            { "66", "Свердловская область" },
            { "67", "Смоленская область" },
            { "68", "Тамбовская область" },
            { "69", "Тверская область" },
            { "70", "Томская область" },
            { "71", "Тульская область" },
            { "72", "Тюменская область" },
            { "73", "Ульяновская область" },
            { "74", "Челябинская область" },
            { "75", "Забайкальский край" },
            { "76", "Ярославская область" },
            { "77", "г. Москва" },
            { "78", "г. Санкт-Петербург" },
            { "79", "Еврейская автономная область" },
            { "83", "Ненецкий автономный округ" },
            { "86", "Ханты-Мансийский автономный округ - Югра" },
            { "87", "Чукотский автономный округ" },
            { "89", "Ямало-Ненецкий автономный округ" },
            { "91", "Республика Крым" },
            { "92", "г. Севастополь" },
            { "99", "Иные территории, включая город и космодром Байконур" }
        };
    }
}