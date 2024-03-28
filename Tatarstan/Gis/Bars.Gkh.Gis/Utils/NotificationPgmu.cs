namespace Bars.Gkh.Gis.Utils
{
    using System;
    using System.IO;
    using System.Net;
    using System.Text;

    /// <summary>
    /// Информирование ПГМУ о новых начислениях
    /// </summary>
    public class NotificationPgmu
    {
        /// <summary>
        /// Метод шифрования в SHA1
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private string GetSha1Hash(string input)
        {
            byte[] hash;
            using (var sha1 = new System.Security.Cryptography.SHA1CryptoServiceProvider())
                hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(input));
            var sb = new StringBuilder();
            foreach (byte b in hash) sb.AppendFormat("{0:x2}", b);
            return sb.ToString();
        }

        /// <summary>
        /// Отправка данных
        /// </summary>
        /// <param name="postedData">Содержимое отправляемого сообщения (в json)</param>
        /// <returns></returns>
        private HttpWebResponse PostMethod(string postedData)
        {
            //адрес
            const string postUrl = @"http://api.tatar.ru/api/v1/services/hcs/line/integration/appointments.json";
            //секретный ключ 
            const string secretKey = "8f777bab5df951bea9627b438d74af366451a3008b7927efe37c321b8e5f2675c9eae6f8521ca6c3";
            //токеном авторизации 
            const string authorizationToken = "5396a80ec38202a49ae8b07960f045648b1d537c2995d189ac4262233777a9702845afb8543725e2";
            //ЭЦП
            var requestSignature = GetSha1Hash(GetSha1Hash(secretKey) + "/api/v1/services/hcs/line/integration/appointments.json" + "&" +
                    postedData);

            var request = (HttpWebRequest)WebRequest.Create(postUrl);
            request.Method = "POST";
            //заполняем заголовки
            request.Headers.Add("x-authorization-token", authorizationToken);
            request.Headers.Add("x-request-signature", requestSignature);
            request.Credentials = CredentialCache.DefaultCredentials;
            request.ContentType = "application/x-www-form-urlencoded";

            var bytes = new UTF8Encoding().GetBytes(postedData);
            request.ContentLength = bytes.Length;

            using (var newStream = request.GetRequestStream())
            {
                newStream.Write(bytes, 0, bytes.Length);
                newStream.Close();
            }

            return (HttpWebResponse)request.GetResponse();
        }

        /// <summary>
        /// Отправить уведомление
        /// </summary>
        /// <param name="ercCode">Код ЕРЦ</param>
        /// <param name="ercName">Наименование ЕРЦ</param>
        /// <param name="calculationDate">Расчетный месяц</param>
        /// <returns></returns>
        public string SendNotification(long ercCode, string ercName, DateTime calculationDate)
        {
            var postedData = String.Format(
                // Массив префиксов для обновления
                "{{\"prefix\": [\"{0}\"], " +

                // Дата расчета новой счет фактуры
                "\"date\": \"{1}\", " +

                // Название компании
                "\"company\": \"{2}\", " +

                // Месяц новой счет фактуры
                "\"factory_month\": \"{3}\"," +

                // Год новой счет фактуры
                "\"factory_year\": \"{4}\"" +

                "}}",
                ercCode,
                calculationDate.ToShortDateString(),
                ercName,
                calculationDate.Month,
                calculationDate.Year
                );


            var response = PostMethod(postedData);
            if (response != null)
            {
                var strreader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                return strreader.ReadToEnd();
            }

            return null;
        }


    }
}
