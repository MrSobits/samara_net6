namespace Bars.Gkh.DeloIntegration.DomainService
{
    using System.Collections.Generic;
    using Bars.Gkh.DeloIntegration.Wcf.Contracts;
    using Bars.B4;

    /// <summary>
    /// Интерфейс для интеграции с системой Дело
    /// </summary>
    public interface IDeloIntegrationService
    {
        // Метод получения ответов которые небходимо отправить в Дело
        // Берутся только те ответы, которые не отправлены, и которые находятся на конечном статуса и обращение которых загружено из Дело
        AnswerRecord[] GetAnswers(long[] ids = null);

        // Метод получения ответов которые небходимо отправить в Дело
        // Берутся только те ответы, которые не отправлены, и которые находятся на конечном статуса и обращение которых загружено из Дело
        IDataResult UpdateAnswers(List<AnswerRecord> answers);

        // метод инициализации импорта
        void InitLog(string logName, string key);

        // метод 
        void FinishLog(int success, int errors, string fileName, bool isImported = true);

    }
}
