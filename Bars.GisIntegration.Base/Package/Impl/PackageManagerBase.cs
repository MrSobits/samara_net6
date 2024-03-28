namespace Bars.GisIntegration.Base.Package.Impl
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Text;
    using System.Xml;
    using System.Xml.Serialization;

    using Bars.B4.Utils;
    using Bars.GisIntegration.Base.Extensions;
    using Bars.GisIntegration.Base.Utils;

    public abstract class PackageManagerBase<TPackageInfo, TPackageId> : IPackageManager<TPackageInfo, TPackageId>
        where TPackageInfo : IPackageInfo
    {
        /// <summary>
        /// Создать пакет
        /// </summary>
        /// <param name="packageName">Имя пакета</param>
        /// <param name="notSignedData">Неподписанные данные</param>
        /// <param name="transportGuidDictionary">Словарь транспортных идентификаторов</param>
        /// <returns>Описание пакета</returns>
        public abstract TPackageInfo CreatePackage(
            string packageName,
            object notSignedData,
            Dictionary<Type, Dictionary<string, long>> transportGuidDictionary = null);

        /// <summary>
        /// Сохранить неподписанные данные
        /// </summary>
        /// <param name="packageInfo">Описание пакета</param>
        /// <param name="notSignedData">Неподписанные данные</param>
        /// <param name="transportGuidDictionary">Словарь транспортных идентификаторов</param>
        void IPackageManager.SaveNotSignedData(
            IPackageInfo packageInfo,
            object notSignedData,
            Dictionary<Type, Dictionary<string, long>> transportGuidDictionary)
        {
            this.SaveNotSignedData((TPackageInfo)packageInfo, notSignedData, transportGuidDictionary);
        }

        /// <summary>
        /// Сохранить подписанные данные
        /// </summary>
        /// <param name="packageId">Идентификатор пакета</param>
        /// <param name="signedData">Подписанные данные</param>
        void IPackageManager.SaveSignedData(object packageId, string signedData)
        {
            this.SaveSignedData(ConvertHelper.ConvertTo<TPackageId>(packageId), signedData);
        }

        /// <summary>
        /// Сохранить подписанные данные
        /// </summary>
        /// <param name="packageInfo">Описание пакета</param>
        /// <param name="signedData">Подписанные данные</param>
        void IPackageManager.SaveSignedData(IPackageInfo packageInfo, string signedData)
        {
            this.SaveSignedData((TPackageInfo)packageInfo, signedData);
        }

        /// <summary>
        /// Получить данные пакета
        /// </summary>
        /// <param name="packageId">Идентификатор пакета</param>
        /// <param name="signed">Признак - вернуть подписанные/неподписанные данные данные</param>
        /// <param name="formatted">Признак - вернуть форматированную/неформатированную xml строку</param>
        /// <returns>Данные xml строкой</returns>
        string IPackageManager.GetData(object packageId, bool signed, bool formatted)
        {
            return this.GetData(ConvertHelper.ConvertTo<TPackageId>(packageId), signed, formatted);
        }

        /// <summary>
        /// Получить данные пакета
        /// </summary>
        /// <param name="packageInfo">Описание пакета</param>
        /// <param name="signed">Признак - вернуть подписанные/неподписанные данные данные</param>
        /// <param name="formatted">Признак - вернуть форматированную/неформатированную xml строку</param>
        /// <returns>Данные xml строкой</returns>
        string IPackageManager.GetData(IPackageInfo packageInfo, bool signed, bool formatted)
        {
            return this.GetData((TPackageInfo)packageInfo, signed, formatted);
        }

        /// <summary>
        /// Получить данные пакета
        /// </summary>
        /// <typeparam name="TDataType">Тип данных пакета</typeparam>
        /// <param name="packageId">Идентификатор пакета</param>
        /// <param name="signed">Признак - вернуть подписанные/неподписанные данные данные</param>
        /// <returns>Данные пакета</returns>
        TDataType IPackageManager.GetData<TDataType>(object packageId, bool signed)
        {
            return this.GetData<TDataType>(ConvertHelper.ConvertTo<TPackageId>(packageId), signed);
        }

        /// <summary>
        /// Получить данные пакета
        /// </summary>
        /// <typeparam name="TDataType">Описание пакета</typeparam>
        /// <param name="packageInfo">Идентификатор пакета</param>
        /// <param name="signed">Признак - вернуть подписанные/неподписанные данные данные</param>
        /// <returns>Данные пакета</returns>
        TDataType IPackageManager.GetData<TDataType>(IPackageInfo packageInfo, bool signed)
        {
            return this.GetData<TDataType>((TPackageInfo)packageInfo, signed);
        }

        /// <summary>
        /// Получить словать транспортных идентификаторов
        /// </summary>
        /// <param name="packageInfo">Описание пакета</param>
        /// <returns>Словарь транспортных идентификаторов</returns>
        Dictionary<Type, Dictionary<string, long>> IPackageManager.GetTransportGuidDictionary(IPackageInfo packageInfo)
        {
            return this.GetTransportGuidDictionary((TPackageInfo)packageInfo);
        }

        /// <summary>
        /// Сохранить неподписанные данные
        /// </summary>
        /// <param name="packageInfo">Описание пакета</param>
        /// <param name="notSignedData">Неподписанные данные</param>
        /// <param name="transportGuidDictionary">Словарь транспортных идентификаторов</param>
        public abstract void SaveNotSignedData(
            TPackageInfo packageInfo,
            object notSignedData,
            Dictionary<Type, Dictionary<string, long>> transportGuidDictionary = null);

        /// <summary>
        /// Сохранить подписанные данные
        /// </summary>
        /// <param name="packageId">Идентификатор пакета</param>
        /// <param name="signedData">Подписанные данные</param>
        public abstract void SaveSignedData(TPackageId packageId, string signedData);

        /// <summary>
        /// Сохранить подписанные данные
        /// </summary>
        /// <param name="packageInfo">Описание пакета</param>
        /// <param name="signedData">Подписанные данные</param>
        public abstract void SaveSignedData(TPackageInfo packageInfo, string signedData);

        /// <summary>
        /// Получить данные пакета
        /// </summary>
        /// <param name="packageId">Идентификатор пакета</param>
        /// <param name="signed">Признак - вернуть подписанные/неподписанные данные данные</param>
        /// <param name="formatted">Признак - вернуть форматированную/неформатированную xml строку</param>
        /// <returns>Данные xml строкой</returns>
        public abstract string GetData(TPackageId packageId, bool signed, bool formatted);

        /// <summary>
        /// Получить данные пакета
        /// </summary>
        /// <param name="packageInfo">Описание пакета</param>
        /// <param name="signed">Признак - вернуть подписанные/неподписанные данные данные</param>
        /// <param name="formatted">Признак - вернуть форматированную/неформатированную xml строку</param>
        /// <returns>Данные xml строкой</returns>
        public abstract string GetData(TPackageInfo packageInfo, bool signed, bool formatted);

        /// <summary>
        /// Получить словать транспортных идентификаторов
        /// </summary>
        /// <param name="packageInfo">Описание пакета</param>
        /// <returns>Словарь транспортных идентификаторов</returns>
        public abstract Dictionary<Type, Dictionary<string, long>> GetTransportGuidDictionary(TPackageInfo packageInfo);

        /// <summary>
        /// Получить данные пакета
        /// </summary>
        /// <typeparam name="TDataType">Тип данных пакета</typeparam>
        /// <param name="packageId">Идентификатор пакета</param>
        /// <param name="signed">Признак - вернуть подписанные/неподписанные данные данные</param>
        /// <returns>Данные пакета</returns>
        public TDataType GetData<TDataType>(TPackageId packageId, bool signed)
        {
            var dataString = this.GetData(packageId, signed, false);

            return this.DeserializeData<TDataType>(dataString);
        }

        /// <summary>
        /// Получить данные пакета
        /// </summary>
        /// <typeparam name="TDataType">Описание пакета</typeparam>
        /// <param name="packageInfo">Идентификатор пакета</param>
        /// <param name="signed">Признак - вернуть подписанные/неподписанные данные данные</param>
        /// <returns>Данные пакета</returns>
        public TDataType GetData<TDataType>(TPackageInfo packageInfo, bool signed)
        {
            var dataString = this.GetData(packageInfo, signed, false);

            return this.DeserializeData<TDataType>(dataString);
        }

        /// <summary>
        /// Создать пакет
        /// </summary>
        /// <param name="packageName">Имя пакета</param>
        /// <param name="notSignedData">Неподписанные данные</param>
        /// <param name="transportGuidDictionary">Словарь транспортных идентификаторов</param>
        /// <returns>Описание пакета</returns>
        IPackageInfo IPackageManager.CreatePackage(
            string packageName,
            object notSignedData,
            Dictionary<Type, Dictionary<string, long>> transportGuidDictionary)
        {
            return this.CreatePackage(packageName, notSignedData, transportGuidDictionary);
        }

        /// <summary>
        /// Получание данных XML-строкой
        /// </summary>
        /// <param name="data">>Данные</param>
        /// <returns>Неформатированная XML-строка</returns>
        protected virtual string GetXmlDataString(byte[] data)
        {
            return Encoding.UTF8.GetString(data);
        }

        /// <summary>
        /// Получание данных отформатированной XML-строкой
        /// </summary>
        /// <param name="data">Данные</param>
        /// <returns>Форматированная XML-строка</returns>
        protected virtual string GetFormattedXmlDataString(byte[] data)
        {
            return XmlExtensions.GetFormattedXmlString(data);
        }

        /// <summary>
        /// Сериализовать XML-документ
        /// </summary>
        /// <param name="document">XML-документ</param>
        protected virtual byte[] XmlToBytes(XmlDocument document)
        {
            return Encoding.UTF8.GetBytes(document.OuterXml);
        }

        /// <summary>
        /// Десериализовать словарь сущностей, содержащихся в данных
        /// </summary>
        /// <returns>Словать сущностей, содержащихся в данных</returns>
        protected virtual Dictionary<Type, Dictionary<string, long>> DeserializeTransportGuidDictionary(byte[] transportGuidDictionary)
        {
            var result = new Dictionary<Type, Dictionary<string, long>>();
            var formatter = new BinaryFormatter();

            if (transportGuidDictionary != null)
            {
                using (var memoryStream = new MemoryStream(transportGuidDictionary))
                {
                    result = (Dictionary<Type, Dictionary<string, long>>)formatter.Deserialize(memoryStream);
                }
            }

            return result;
        }

        /// <summary>
        /// Сериализовать словарь сущностей данных
        /// </summary>
        /// <param name="dictionary">Словарь сущностей данных</param>
        protected virtual byte[] SerializeTransportGuidDictionary(Dictionary<Type, Dictionary<string, long>> dictionary)
        {
            using (var memoryStream = new MemoryStream())
            {
                if (dictionary != null)
                {
                    var formatter = new BinaryFormatter();
                    formatter.Serialize(memoryStream, dictionary);
                }

                return memoryStream.ToArray();
            }
        }

        /// <summary>
        /// Сериаилазация запроса
        /// </summary>
        /// <param name="data">Запрос</param>
        /// <returns>Xml-документ</returns>
        protected XmlDocument SerializeRequest(object data)
        {
            var type = data.GetType();
            XmlDocument result;

            var attr = (XmlTypeAttribute)type.GetCustomAttribute(typeof(XmlTypeAttribute));
            var xmlSerializer = new XmlSerializer(type, attr.Namespace);

            using (var stringWriter = new StringWriter())
            {
                using (var xmlWriter = XmlWriter.Create(stringWriter, new XmlWriterSettings { OmitXmlDeclaration = true }))
                {
                    xmlSerializer.Serialize(xmlWriter, data);

                    result = new XmlDocument();
                    result.LoadXml(stringWriter.ToString());
                }
            }

            var prefixer = new XmlNsPrefixer();
            prefixer.Process(result);

            return result;
        }

        protected byte[] StreamToByteArray(Stream input)
        {
            var buffer = new byte[16 * 1024];
            using (var ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }

                return ms.ToArray();
            }
        }

        protected TDataType DeserializeData<TDataType>(string data)
        {
            TDataType result;

            var attr = (XmlTypeAttribute)typeof(TDataType).GetCustomAttribute(typeof(XmlTypeAttribute));
            var xmlSerializer = new XmlSerializer(typeof(TDataType), attr.Namespace);

            using (var reader = XmlReader.Create(new StringReader(data)))
            {
                result = (TDataType)xmlSerializer.Deserialize(reader);
            }

            return result;
        }
    }
}