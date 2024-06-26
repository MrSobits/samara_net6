﻿Функция: тесткейсы для раздела "Расчетно-кассовые центры"

Предыстория:
Дано пользователь добавляет новый РКЦ

@ScenarioInTransaction
Сценарий: успешное добавление РКЦ
Дано пользователь у этого РКЦ заполняет поле Контрагент "Тестовый контрагент"
И пользователь у этого РКЦ заполняет поле Идентификатор РКЦ "1001"
Когда пользователь сохраняет этот РКЦ
Тогда запись по этому РКЦ присутствует в разделе расчетно-кассовых центров

@ScenarioInTransaction
Сценарий: успешное удаление РКЦ
Дано пользователь у этого РКЦ заполняет поле Контрагент "Тестовый контрагент"
И пользователь у этого РКЦ заполняет поле Идентификатор РКЦ "1001"
Когда пользователь сохраняет этот РКЦ
И пользователь удаляет этот РКЦ
Тогда запись по этому РКЦ отсутствует в разделе расчетно-кассовых центров

@ScenarioInTransaction
Сценарий: успешное добавление муниципального образования к РКЦ
Дано пользователь у этого РКЦ заполняет поле Контрагент "Тестовый контрагент"
И пользователь у этого РКЦ заполняет поле Идентификатор РКЦ "1001"
Когда пользователь сохраняет этот РКЦ
И пользователь к этому РКЦ добавляет муниципальное образование "Алеутский муниципальный район"
Тогда записи по этому муниципальному образованию присутствуют в списке муниципальных образований этого РКЦ

@ScenarioInTransaction
Сценарий: успешное удаление муниципального образования с РКЦ
Дано пользователь у этого РКЦ заполняет поле Контрагент "Тестовый контрагент"
И пользователь у этого РКЦ заполняет поле Идентификатор РКЦ "1001"
Когда пользователь сохраняет этот РКЦ
И пользователь к этому РКЦ добавляет муниципальное образование "Алеутский муниципальный район"
Тогда записи по этому муниципальному образованию присутствуют в списке муниципальных образований этого РКЦ
Когда пользователь удаляет это муниципальное образование у этого РКЦ
Тогда записи по этому муниципальному образованию отсутствуют в списке муниципальных образований этого РКЦ

@ScenarioInTransaction
Сценарий: проверка на уникальность контрагента в РКЦ
Дано пользователь у этого РКЦ заполняет поле Контрагент "Тестовый контрагент"
И пользователь у этого РКЦ заполняет поле Идентификатор РКЦ "1001"
Когда пользователь сохраняет этот РКЦ
Дано пользователь добавляет новый РКЦ
И пользователь у этого РКЦ заполняет поле Контрагент "Тестовый контрагент"
И пользователь у этого РКЦ заполняет поле Идентификатор РКЦ "10011"
Когда пользователь сохраняет этот РКЦ
Тогда падает ошибка с текстом "Для указанного контрагента уже существует расчетно-кассовый центр."

#проверка на клиенте
@ignore
Сценарий: проверка на уникальность идентификатора РКЦ
Дано пользователь у этого РКЦ заполняет поле Контрагент "Тестовый контрагент"
И пользователь у этого РКЦ заполняет поле Идентификатор РКЦ "1001"
Когда пользователь сохраняет этот РКЦ
Дано пользователь добавляет новый РКЦ
И пользователь у этого РКЦ заполняет поле Контрагент "тестовый контрагент_0"
И пользователь у этого РКЦ заполняет поле Идентификатор РКЦ "1001"
Когда пользователь сохраняет этот РКЦ
Тогда падает ошибка с текстом "Указанный идентификатор уже существует. Необходимо указать уникальный идентификатор."

@ignore
#тест должен запускаться без транзакции - созданные РКЦ нужно удалять
Сценарий: неудачная привязка лс к РКЦ при пересечении дат договоров
Дано пользователь у этого РКЦ заполняет поле Контрагент "Тестовый контрагент"
И пользователь у этого РКЦ заполняет поле Идентификатор РКЦ "1001"
Когда пользователь сохраняет этот РКЦ
И пользователь к этому РКЦ добавляет муниципальное образование "Алеутский муниципальный район"
Допустим пользователь в объектах РКЦ добавляет новый объект
И пользователь у этого объекта РКЦ заполняет поле Дата начала действия договора "текущая дата"
И пользователь у этого объекта РКЦ добавляет лицевой счет "140014146"
Когда пользователь сохраняет этот объект РКЦ
Дано пользователь добавляет новый РКЦ
И пользователь у этого РКЦ заполняет поле Контрагент "тестовый контрагент_0"
И пользователь у этого РКЦ заполняет поле Идентификатор РКЦ "10011"
Когда пользователь сохраняет этот РКЦ
И пользователь к этому РКЦ добавляет муниципальное образование "Алеутский муниципальный район"
Допустим пользователь в объектах РКЦ добавляет новый объект
И пользователь у этого объекта РКЦ заполняет поле Дата начала действия договора "текущая дата"
И пользователь у этого объекта РКЦ добавляет лицевой счет "140014146"
#в сохраненинии используется другой метод
Когда пользователь сохраняет этот объект РКЦ
Тогда падает ошибка с текстом "Некоторые счета имеют действующий договор. Для добавления нового договора необходимо закрыть прошлый."

@ignore
#тест должен запускаться без транзакции - созданные РКЦ нужно удалять
Сценарий: успешная привязка лс к РКЦ при непересечении дат договоров
#создание первого РКЦ
Дано пользователь у этого РКЦ заполняет поле Контрагент "Тестовый контрагент"
И пользователь у этого РКЦ заполняет поле Идентификатор РКЦ "1001"
Когда пользователь сохраняет этот РКЦ
И пользователь к этому РКЦ добавляет муниципальное образование "Алеутский муниципальный район"
Допустим пользователь в объектах РКЦ добавляет новый объект
И пользователь у этого объекта РКЦ заполняет поле Дата начала действия договора "11.01.9999"
И пользователь у этого объекта РКЦ заполняет поле Дата окончания действия договора "13.01.9999"
И пользователь у этого объекта РКЦ добавляет лицевой счет "140014146"
Когда пользователь сохраняет этот объект РКЦ
#создаем второй РКЦ
Дано пользователь добавляет новый РКЦ
И пользователь у этого РКЦ заполняет поле Контрагент "тестовый контрагент_0"
И пользователь у этого РКЦ заполняет поле Идентификатор РКЦ "10011"
Когда пользователь сохраняет этот РКЦ
И пользователь к этому РКЦ добавляет муниципальное образование "Алеутский муниципальный район"
#к второму РКЦ добавляем объект, который уже добавлен в первый РКЦ
Допустим пользователь в объектах РКЦ добавляет новый объект
И пользователь у этого объекта РКЦ заполняет поле Дата начала действия договора "14.01.9999"
И пользователь у этого объекта РКЦ добавляет лицевой счет "140014146"
Когда пользователь сохраняет этот объект РКЦ
Тогда падает ошибка с текстом "Лицевые счета сохранены успешно"