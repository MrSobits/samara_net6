namespace Bars.Gkh.Domain.TableLocker
{
	using System;

	using Bars.B4.DataAccess;

	/// <summary>
	///     Массовый блокировщик таблиц
	/// </summary>
	public interface IBatchTableLocker : IDisposable
	{
		/// <summary>
		///     Автоматически сбрасывать блокировку при <see cref="IDisposable.Dispose" /> компонента
		/// </summary>
		/// <param name="auto"></param>
		/// <returns></returns>
		IBatchTableLocker AutoUnlock(bool auto);

		/// <summary>
		///     Проверить блокировку таблиц
		/// </summary>
		/// <returns></returns>
		bool CheckLocked();

		/// <summary>
		///     Очистка списка таблиц
		/// </summary>
		void Clear();

		/// <summary>
		///     Заблокировать таблицы
		/// </summary>
		IBatchTableLocker Lock();

		/// <summary>
		///     Бросать исключение при попытке заблокировать
		///     уже заблокированную таблицу
		/// </summary>
		/// <param name="throwOn"></param>
		/// <returns></returns>
		IBatchTableLocker ThrowOnAlreadyLocked(bool throwOn = true);

		/// <summary>
		///     Разблокировать таблицы
		/// </summary>
		IBatchTableLocker Unlock();

		/// <summary>
		///     Включить таблицу по имени
		/// </summary>
		/// <param name="tableName"></param>
		/// <param name="action"></param>
		/// <returns></returns>
		IBatchTableLocker With(string tableName, string action);

		/// <summary>
		///     Включить таблицу по сущности
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		IBatchTableLocker With<T>(string action) where T : PersistentObject;

		/// <summary>
		///     Включить таблицу по сущности
		/// </summary>
		/// <returns></returns>
		IBatchTableLocker With(Type type, string action);
	}
}