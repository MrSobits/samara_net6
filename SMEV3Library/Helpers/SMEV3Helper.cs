using SMEV3Library.Entities.GetResponseResponse;
using SMEV3Library.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SMEV3Library.Helpers
{
    public static class Smev3Helper
    {
        /// <summary>
        /// Кеш результатов, которые еще не запрашивали, но их вернул СМЭВ
        /// </summary>
        static Dictionary<string, GetResponseResponse> responses = new Dictionary<string, GetResponseResponse>();

        /// <summary>
        /// Проверяет наличие ответа в СМЭВ стеке
        /// </summary>
        /// <param name="NamespaceURI">target namespace схемы</param>
        /// <param name="RootElementLocalName">имя (local name) элемента</param>
        /// 
        public static async Task<GetResponseResponse> TryGetResponseAsync(ISMEV3Service service, string NamespaceURI, string RootElementLocalName, string MessageId, bool saveLog = false)
        {
            //ответ есть в кеше?
            if (responses.ContainsKey(MessageId))
            {
                //читаем из кеша
                var response = responses[MessageId];
                //удаляем из кеша
                responses.Remove(MessageId);
                //удаляем с сервера
                await service.GetAckAsync(MessageId, true).ConfigureAwait(false);
                //
                return response;
            }

            GetResponseResponse responseResult;
            do
            {
                responseResult = await service.GetResponseAsync(NamespaceURI, RootElementLocalName, saveLog).ConfigureAwait(false);
                //Если сервер прислал ошибку, возвращаем как есть
                if (responseResult.FaultXML != null)
                    return responseResult;

                //если результатов пока нет, возврат
                if (!responseResult.isAnswerPresent)
                    return null;

                //сохранение в кеш результатов с другими ID
                if (responseResult.OriginalMessageId != MessageId)
                    if (!responses.ContainsKey(responseResult.OriginalMessageId))
                        responses.Add(responseResult.OriginalMessageId, responseResult);
            }
            while (responseResult.OriginalMessageId != MessageId);

            //чистка кеша от тех, которые уже забрали
            foreach (var pair in responses)
            {
                if (DateTime.Now - pair.Value.CreatedTime > TimeSpan.FromMinutes(26))
                    responses.Remove(pair.Key);
            }

            await service.GetAckAsync(responseResult.MessageId, true).ConfigureAwait(false);

            return responseResult;
        }

        /// <summary>
        /// Ожидает очищения СМЭВ стека. Сам не помечает на удаление
        /// </summary>
        /// <param name="NamespaceURI">target namespace схемы</param>
        /// <param name="RootElementLocalName">имя (local name) элемента</param>
        public static async Task WaitRequestsIsClearAsync(ISMEV3Service service, string NamespaceURI, string RootElementLocalName)
        {
            while (true)
            {
                //запрос ответа
                GetResponseResponse responseResult = await service.GetResponseAsync(NamespaceURI, RootElementLocalName).ConfigureAwait(false);

                if (responseResult == null)
                    break;
            }
        }

        /// <summary>
        /// Очищает все уже пришедшие запросы в СМЭВ стеке
        /// </summary>
        /// <param name="NamespaceURI">target namespace схемы</param>
        /// <param name="RootElementLocalName">имя (local name) элемента</param>
        /// <returns>Количество удаленных</returns>
        public static async Task<int> ClearAllRequestsAsync(ISMEV3Service service, string NamespaceURI, string RootElementLocalName)
        {
            int count = 0;

            while (true)
            {
                //запрос ответа
                GetResponseResponse responseResult = await service.GetResponseAsync(NamespaceURI, RootElementLocalName).ConfigureAwait(false);
                if (responseResult == null || !responseResult.isAnswerPresent)
                    break;

                count++;

                //запрос удаления
                await service.GetAckAsync(responseResult.MessageId, true).ConfigureAwait(false);
            }

            return count;
        }
    }
}
