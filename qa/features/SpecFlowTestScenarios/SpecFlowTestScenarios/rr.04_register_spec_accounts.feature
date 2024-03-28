﻿
Функция: доработка отчета "04_Реестр специальных счетов"

Предыстория: 
Дано в Отчеты - Отчеты - Долгосрочная Республиканская адресная программа капитального ремонта есть отчет "04_Реестр специальных счетов"
Дано задан параметр отчета "Дата отчета" 
Дано задан параметр отчета "Муниципальные образования"
Дано в отчет тянутся данные из реестра Капитальный ремонт/Долгосрочная программа/Версии программы


Структура сценария: Описание нового параметра "Наличие в ДПКР" 
Дано доступен параметр "Наличие в ДПКР"
Дано по умолчанию параметр "Наличие в ДПКР" заполнен значением "Не задано"
Дано параметр "Наличие в ДПКР" необязателен для заполнения
Когда выбираю в параметре "Наличие в ДПКР" значение <параметр>
И нажимаю кнопку "Печать"
И после формирования отчета нажимаю кнопку "Скачать"
И открываю скаченный файл
Тогда в отчет выводятся мкд с типом <тип_мкд> с условием по версии программы <условие_версии>

Примеры: 
| параметр  | тип_мкд                | условие_версии                               |
| Да        | все                    | которые есть в основной версии программы     |
| Нет       | "Исправные" и "Ветхие" | которых нет в основной версии программы      |
| Не задано | "Исправные" и "Ветхие" | которые есть и которых нет в основной версии |                                                                                    