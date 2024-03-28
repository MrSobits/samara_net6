namespace Bars.Gkh.TextValues
{
    /// <summary>
    /// Интерфейс для описания тектовых значений пунктов меню
    /// </summary>
    public interface IMenuItemText
    {
        /// <summary>
        /// Получить переопределенное значение
        /// </summary>
        /// <returns>Возвращает исходное значение, если оно не переопределено</returns>
        string GetText(string menuItemText);

        /// <summary>
        /// Переопределить текстовое значение
        /// </summary>
        /// <param name="baseTextValue">Исходное сообщение</param>
        /// <param name="newTextValue">Новое значение</param>
        void Override(string baseTextValue, string newTextValue);
    }
}