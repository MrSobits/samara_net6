namespace Bars.B4.Modules.FIAS.AutoUpdater.Converter.Impl
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;
    using System.Text;

    using Bars.B4.Config;
    using Bars.B4.IoC;

    using Castle.Core;
    using Castle.Windsor;

    using SocialExplorer.IO.FastDBF;

    /// <summary>
    /// Сервис конвертации данных ФИАС из *.dbf файлов
    /// </summary>
    internal class FiasDbConverter : IFiasDbConverter, IInitializable
    {
        public IWindsorContainer Container { get; set; }

        protected virtual Encoding DbfFileEncoding { get; set; } = Encoding.GetEncoding("cp866");

        /// <summary>
        /// Парсинг таблицы с кусочками фиас
        /// </summary>
        /// <param name="dbfFilePath">Путь к файлу DBF</param>
        /// <returns>IEnumerable<AddressObjectsObject></returns>
        public IEnumerable<AddressObjectsObject> GetFiasRecords(string dbfFilePath)
        {
            using (var dbfFileWrapper = new DbfFileWrapper(dbfFilePath, this.DbfFileEncoding))
            {
                var dbfFile = dbfFileWrapper.File;
                var record = new DbfRecord(dbfFile.Header);

                while (dbfFile.ReadNext(record))
                {
                    yield return ConvertDbfRecordTo<AddressObjectsObject>(record);

                }
            }
        }

        /// <summary>
        /// Парсинг таблицы с аттрибутами домов
        /// </summary>
        /// <param name="dbfFilePath">Путь к файлу DBF</param>
        /// <returns>IEnumerable<HousesHouse></returns>
        public IEnumerable<HousesHouse> GetFiasHouseRecords(string dbfFilePath)
        {
            using (var dbfFileWrapper = new DbfFileWrapper(dbfFilePath, this.DbfFileEncoding))
            {
                var dbfFile = dbfFileWrapper.File;
                var record = new DbfRecord(dbfFile.Header);

                while (dbfFile.ReadNext(record))
                {
                    yield return ConvertDbfRecordTo<HousesHouse>(record);
                }
            }
        }     

        private T ConvertDbfRecordTo<T>(DbfRecord record)
            where T : new()
        {
            var result = new T();
            var type = typeof(T);
            object newValue;

            foreach (var property in type.GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                var colId = record.FindColumn(property.Name);

                if (colId < 0)
                {
                    continue;
                }

                if (property.PropertyType == typeof(DateTime))
                {
                    newValue = record.GetDateValue(colId);
                }
                else

                {
                    var stringValue = record[colId];
                    switch (Type.GetTypeCode(property.PropertyType))
                    {
                        case TypeCode.SByte:
                            newValue = sbyte.Parse(stringValue);
                            break;
                        case TypeCode.Byte:
                            newValue = byte.Parse(stringValue);
                            break;
                        case TypeCode.Int16:
                            newValue = short.Parse(stringValue);
                            break;
                        case TypeCode.UInt16:
                            newValue = ushort.Parse(stringValue);
                            break;
                        case TypeCode.Int32:
                            newValue = int.Parse(stringValue);
                            break;
                        case TypeCode.UInt32:
                            newValue = uint.Parse(stringValue);
                            break;
                        default:
                            newValue = stringValue.Trim();
                            break;
                    }
                }

                property.SetValue(result, newValue);

            }

            return result;
        }

        private class DbfFileWrapper : IDisposable
        {
            private readonly DbfFile dbfFile;

            public DbfFile File => this.dbfFile;

            public DbfFileWrapper(string dbfFilePath, Encoding encoding)
            {
                this.dbfFile = new DbfFile(encoding);
                this.dbfFile.Open(dbfFilePath, FileMode.Open, FileAccess.Read);
            }

            /// <inheritdoc />
            public void Dispose()
            {
                this.dbfFile.Close();
            }
        }

        /// <inheritdoc />
        public void Initialize()
        {
            var configProvider = this.Container.Resolve<IConfigProvider>();

            using (this.Container.Using(configProvider))
            {
                var config = configProvider.GetConfig().GetModuleConfig("Bars.B4.Modules.FIAS.AutoUpdater");

                var dbfEncodingString = config.GetAs("DbfEncoding", default(string), true);
                var dbfEncodingInt = config.GetAs("DbfEncoding", default(int), true);

                if (!string.IsNullOrEmpty(dbfEncodingString))
                {
                    this.DbfFileEncoding = Encoding.GetEncoding(dbfEncodingString);
                }
                else if (dbfEncodingInt != 0)
                {
                    this.DbfFileEncoding = Encoding.GetEncoding(dbfEncodingInt);
                }
            }
        }
    }
}