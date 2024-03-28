namespace Bars.Tools.Installer.SetupActions.License
{
    using System;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Security.Cryptography.Xml;
    using System.Xml;

    public class LicenseHandler
    {
        public const string PublicKey = @"BgIAAACkAABSU0ExAAQAAAEAAQANtv8oHifZNPIKPLWAWZUPePD6sUNrhQI0JT//FCYSRebOExpm
            JTj2EhbRD+RnLnXThkeakTrbRwUP2mIVfj1OkHNRpHLL+C60QWH09gxkGQkyK7gYP9rNFjs68Cpd
            6Mg/ozDo5BshzN5Y228FviXhYo6xSupPQQv5fdeEkIo/hw==";

        public static ValidateLicenseResult ValidateLicense(string licenseText, string productVersion, DateTime buildDate)
        {
            var validationResult = new ValidateLicenseResult
            {
                IsValid = false,
                Message = ""
            };

            try
            {
                if (string.IsNullOrWhiteSpace(licenseText))
                {
                    validationResult.Message = "Для данного продукта необходимо загрузить корректный файл лицензии," +
                                               "\nлибо скопировать и вставить текст из файла в окошко \"Текст лицензии\"";
                    return validationResult;
                }

                var xmlLicense = new XmlDocument
                {
                    PreserveWhitespace = true
                };

                try
                {
                    xmlLicense.LoadXml(licenseText);
                }
                catch
                {
                    validationResult.IsValid = false;
                    validationResult.Message = "Файл лицензии имеет некорректный формат.\nПожалуйста проверьте правильный ли файл Вы загружаете.";

                    return validationResult;
                }

                var rsa = new RSACryptoServiceProvider();

                rsa.ImportCspBlob(Convert.FromBase64String(PublicKey));

                var signedXml = new SignedXml(xmlLicense);

                var nodeList = xmlLicense.GetElementsByTagName("Signature");

                signedXml.LoadXml((XmlElement)nodeList[0]);

                var isValid = signedXml.CheckSignature(rsa);

                if (!isValid)
                {
                    validationResult.Message = "Проверка ЭЦП для данной лицензии была неуспешной";
                    return validationResult;
                }

                // Инициализируем лицензию
                var records = xmlLicense.GetElementsByTagName("LicenceData");

                var record = records[0];
                var licenseValues = record.ChildNodes
                    .Cast<XmlNode>()
                    .ToDictionary(node => node.Name.ToLower(), node => node.InnerText);
                var licenseMajorVersion = licenseValues.ContainsKey("licenseMajorVersion")
                    ? licenseValues["licenseMajorVersion"]
                    : "1.";

                var licenseStartDate = DateTime.ParseExact(licenseValues["startdate"], "dd.MM.yyyy", System.Globalization.CultureInfo.InvariantCulture);
                var licenseEndDate = DateTime.ParseExact(licenseValues["enddate"], "dd.MM.yyyy", System.Globalization.CultureInfo.InvariantCulture);
                if (buildDate < licenseStartDate
                    || buildDate > licenseEndDate)
                {
                    validationResult.Message = string.Format(
                        "Срок действия лицензии не совпадает с датой сборки данного пакета" +
                        "\n\nСрок действия лицензии: {0} - {1}" +
                        "\nДата сборки пакета: {2}" +
                        "\n\nВам необходимо обратиться к поставщику пакета за корректным файлом лицензии",
                        licenseValues["startdate"], licenseValues["enddate"], buildDate.ToString("dd.MM.yyyy"));
                    return validationResult;
                }

                isValid = productVersion.StartsWith(licenseMajorVersion);

                if (!isValid)
                {
                    validationResult.Message = string.Format(
                        "Указанная лицензия предназначенна для версии {0}*,\nустанавливается версия {1}",
                        licenseMajorVersion, productVersion);
                    return validationResult;
                }

                validationResult.IsValid = true;
            }
            catch (Exception e)
            {
                validationResult.IsValid = false;
                validationResult.Message = "Проверка файла лицензии закончилось с ошибкой. Обратитесь к поставщику данного приложения.";
            }

            return validationResult;
        }
    }
}
