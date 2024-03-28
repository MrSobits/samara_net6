namespace Bars.Tools.Installer.SetupActions
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using System.Threading;
    using System.Windows.Forms;
    using System.Xml;
    using License;
    using Microsoft.Deployment.WindowsInstaller;

    public class CustomActions
    {
        /// <summary>
        /// Валидация загруженной лицензии
        /// </summary>
        /// <param name="session">Объект сессии</param>
        /// <returns>ActionResult</returns>
        [CustomAction]
        public static ActionResult ValidateLicense(Session session)
        {
            var validationResult = LicenseHandler.ValidateLicense(session["LICENSE_TEXT"], session["INTERNAL_PRODUCT_VERSION"],
                DateTime.ParseExact(session["BUILD_DATE"], "dd.MM.yyyy", System.Globalization.CultureInfo.InvariantCulture));
            session["LicenseValid"] = validationResult.IsValid ? "1" : "0";
            session["LastMessage"] = validationResult.Message;

            return ActionResult.Success;
        }

        /// <summary>
        /// Показывает диалог загрузки файла лицензии
        /// </summary>
        /// <param name="session">Объект сессии</param>
        /// <returns>ActionResult</returns>
        [CustomAction]
        public static ActionResult GetLicenseFile(Session session)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            try
            {
                var thread = new Thread(
                    () =>
                    {
                        using (var openDialog = new OpenFileDialog())
                        {
                            openDialog.Filter = "Config File (*.xml)|*.xml";

                            if (openDialog.ShowDialog() != DialogResult.OK)
                            {
                                return;
                            }
                            var data = File.ReadAllBytes(openDialog.FileName);
                            session["LICENSE_TEXT"] = Encoding.GetEncoding(1251).GetString(data);
                        }
                    });

                thread.SetApartmentState(ApartmentState.STA);
                thread.Start();
                thread.Join();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка файла", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return ActionResult.Success;
        }

        /// <summary>
        /// Сохраняет загруженный файл лицензии
        /// </summary>
        /// <param name="session">Объект сессии</param>
        /// <returns>ActionResult</returns>
        [CustomAction]
        public static ActionResult SaveLicense(Session session)
        {
            var licenseText = session["LICENSE_TEXT"];
            var licensePath = string.Format("{0}{1}", session["INSTALLLOCATION"], session["LICENSE_DIR"]);
            var data = Encoding.GetEncoding(1251).GetBytes(licenseText);

            if (!Directory.Exists(licensePath))
            {
                Directory.CreateDirectory(licensePath);
            }
            File.WriteAllBytes(licensePath + "\\License.xml", data);

            return ActionResult.Success;   
        }

        /// <summary>
        /// Удаляет при деинсталляции сохраненный ранее файл лицензии
        /// </summary>
        /// <param name="session">Объект сессии</param>
        /// <returns>ActionResult</returns>
        [CustomAction]
        public static ActionResult DeleteLicense(Session session)
        {
            var licensePath = string.Format("{0}{1}", session["INSTALLLOCATION"], session["LICENSE_DIR"]);

            if (File.Exists(licensePath + "\\License.xml"))
            {
                File.Delete(licensePath + "\\License.xml");

                Directory.Delete(licensePath);
            }

            return ActionResult.Success;
        }

        /// <summary>
        /// Модифицирует строку подключения в b4.config после установки
        /// </summary>
        /// <param name="session">Объект сессии</param>
        /// <returns>ActionResult</returns>
        [CustomAction]
        public static ActionResult ChangeConnectionString(Session session)
        {
            const string connectionString = "Data Source={0};Initial Catalog={1};User Id={2};Password={3};Persist Security Info=True";

            var server = session["SERVER_NAME"];
            var db = session["DB_NAME"];
            var login = session["LOGIN"];
            var password = session["PASSWORD"];

            var xmlDoc = new XmlDocument();

            // Обновляем по необходимости конфиги
            new List<string>
            {
                session["B4_CONFIG_APP_PATH"] == ""
                    ? Path.Combine(session["INSTALLLOCATION"], "b4.config")
                    : session["B4_CONFIG_APP_PATH"],
                session["B4_CONFIG_MASTER_PATH"] == ""
                    ? Path.Combine(session["INSTALLLOCATION"], "CalcServer\\master\\b4.config")
                    : session["B4_CONFIG_APP_PATH"],
                session["B4_CONFIG_EXECUTOR_PATH"] == ""
                    ? Path.Combine(session["INSTALLLOCATION"], "CalcServer\\executor\\b4.config")
                    : session["B4_CONFIG_APP_PATH"]
            }.ForEach(x =>
            {
                if (!File.Exists(x))
                {
                    return;
                }

                xmlDoc.Load(x);

                var connstring = (XmlAttribute)xmlDoc.SelectSingleNode("//configuration/b4config/dbconfig/@connstring");
                if (connstring != null)
                {
                    connstring.Value = string.Format(connectionString, server, db, login, password);
                    xmlDoc.Save(x);
                }
            });

            return ActionResult.Success;
        }
    }
}
