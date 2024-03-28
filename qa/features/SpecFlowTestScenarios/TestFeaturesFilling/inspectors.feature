﻿
Функционал: тесткейсы для раздела "Инспекторы"
Справочники - Общие - Инспекторы


Предыстория: 
Дано пользователь добавляет нового инспектора
И пользователь у этого инспектора заполняет поле Код "тест"
И пользователь у этого инспектора заполняет поле Код ЭДО "234"
И пользователь у этого инспектора заполняет поле Должность "тест"
И пользователь у этого инспектора заполняет поле ФИО "тест"
И пользователь у этого инспектора заполняет поле Фамилия И.О. "тест"
И пользователь у этого инспектора заполняет поле Телефон "тест"
И пользователь у этого инспектора заполняет поле Электронная почта "123@mail.ru"
И пользователь у этого инспектора заполняет поле Родительный "тест"
И пользователь у этого инспектора заполняет поле Дательный "тест"
И пользователь у этого инспектора заполняет поле Винительный "тест"
И пользователь у этого инспектора заполняет поле Творительный "тест"
И пользователь у этого инспектора заполняет поле Предложный "тест"


Сценарий: успешное добавление инспектора c заполненными обязательными полями
Когда пользователь сохраняет этого инспектора
Тогда запись по этому инспектору присутствует в справочнике инспекторов

Сценарий: успешное удаление инспектора
Когда пользователь сохраняет этого инспектора
И пользователь удаляет этого инспектора
Тогда запись по этому инспектору отсутствует в справочнике инспекторов

Сценарий: удачное добавление инспектора при заполнении поля Отдел
Дано пользователь добавляет нового инспектора
И пользователь у этого инспектора заполняет поле Код "тест"
И пользователь у этого инспектора заполняет поле Код ЭДО "234"
И пользователь у этого инспектора заполняет поле ФИО "тест"
Когда пользователь сохраняет этого инспектора
И пользователь у этого инспектора заполняет поле Отдел "ГЖИ"
Тогда запись по этому инспектору присутствует в справочнике инспекторов

Сценарий: добавление инспектора с добавлением подписки на инспектора
Дано пользователь добавляет нового инспектора
И пользователь у этого инспектора заполняет поле Код "тест"
И пользователь у этого инспектора заполняет поле Код ЭДО "234"
И пользователь у этого инспектора заполняет поле ФИО "тест"
Когда пользователь сохраняет этого инспектора
И пользователь у этого инспектора добавляет подписку на инспектора с ФИО "Гречко И.Н."
Тогда запись по этому инспектору присутствует в справочнике инспекторов





