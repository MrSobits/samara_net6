﻿@ScenarioInTransaction
Функционал: тесткейсы для раздела "Реестр абонентов"
Региональный фонд - Абоненты - Реестр абонентов

Предыстория: 
Дано добавлена льготная категория
| Code | Name                    | Percent | LimitArea | DateFrom   |
| 222  | льготная категория 1111 | 13      | 132       | 01.01.2017 |

И в реестр жилых домов добавлен новый дом
| region     | houseType       | city                                          | street             | houseNumber |
| testregion | Многоквартирный | Камчатский край, Алеутский р-н, с. Никольское | ул. 50 лет Октября | 1131        |

И у этого дома добавлено помещение
| RoomNum | Area | LivingArea | Type  | OwnershipType |
| 1       | 51   | 35         | Жилое | Частная       |

Дано добавлена организационно-правовая форма
| Name | Code | OkopfCode |
| тест | тест | тест      |

Дано добавлен контрагент с организационно правовой формой
| Name |
| тест |

Сценарий: успешное добавление абонента с типом "Счет физ.лица"
Дано пользователь добавляет абонента типа Счет физ.лица
И пользователь у этого абонента типа Счет физ.лица заполняет поле Льготная категория
И пользователь у этого абонента типа Счет физ.лица заполняет поле Фамилия "Оборин"
И пользователь у этого абонента типа Счет физ.лица заполняет поле Имя "Сергей"
И пользователь у этого абонента типа Счет физ.лица заполняет поле Отчество "Иванович"
И пользователь у этого абонента типа Счет физ.лица заполняет поле Дата рождения "12.06.1961"
И пользователь у этого абонента типа Счет физ.лица заполняет поле Тип документа "Паспорт"
И пользователь у этого абонента типа Счет физ.лица заполняет поле Серия документа "9206"
И пользователь у этого абонента типа Счет физ.лица заполняет поле Номер документа "612345"
Когда пользователь сохраняет этого абонента типа Счет физ.лица
Тогда запись по этому абоненту присутствует в разделе абонентов типа Счет физ.лица

Сценарий: успешное добавление абонента с типом "Счет юр.лица"
Дано пользователь добавляет абонента типа Счет юр.лица
И пользователь у этого абонента типа Счет юр.лица заполняет поле Льготная категория
И пользователь у этого абонента типа Счет юр.лица заполняет поле Контрагент
#И пользователь у этого абонента типа Счет юр.лица заполняет поле ИНН "????????"
#И пользователь у этого абонента типа Счет юр.лица заполняет поле КПП "????????"
#GKH-2634 И пользователь у этого абонента типа Счет юр.лица заполняет поле Адрес для корреспонденции "Юридический адрес"
И пользователь у этого абонента типа Счет юр.лица заполняет поле Печатать акт при печати документов на оплату "false"
Когда пользователь сохраняет этого абонента типа Счет юр.лица
Тогда запись по этому абоненту присутствует в разделе абонентов типа Счет юр.лица

Сценарий: успешное удаление абонента с типом "Счет физ.лица"
Дано пользователь добавляет абонента типа Счет физ.лица
И пользователь у этого абонента типа Счет физ.лица заполняет поле Льготная категория
И пользователь у этого абонента типа Счет физ.лица заполняет поле Фамилия "Оборин"
И пользователь у этого абонента типа Счет физ.лица заполняет поле Имя "Сергей"
И пользователь у этого абонента типа Счет физ.лица заполняет поле Отчество "Иванович"
И пользователь у этого абонента типа Счет физ.лица заполняет поле Дата рождения "12.06.1961"
И пользователь у этого абонента типа Счет физ.лица заполняет поле Тип документа "Паспорт"
И пользователь у этого абонента типа Счет физ.лица заполняет поле Серия документа "9206"
И пользователь у этого абонента типа Счет физ.лица заполняет поле Номер документа "612345"
Когда пользователь сохраняет этого абонента типа Счет физ.лица
И пользователь удаляет этого абонента типа Счет физ.лица
Тогда запись по этому абоненту отсутствует в разделе абонентов типа Счет физ.лица

Сценарий: успешное удаление абонента с типом "Счет юр.лица"
Дано пользователь добавляет абонента типа Счет юр.лица
И пользователь у этого абонента типа Счет юр.лица заполняет поле Льготная категория
И пользователь у этого абонента типа Счет юр.лица заполняет поле Контрагент
#И пользователь у этого абонента типа Счет юр.лица заполняет поле ИНН "????????"
#И пользователь у этого абонента типа Счет юр.лица заполняет поле КПП "????????"
#GKH-2634 И пользователь у этого абонента типа Счет юр.лица заполняет поле Адрес для корреспонденции "Юридический адрес"
И пользователь у этого абонента типа Счет юр.лица заполняет поле Печатать акт при печати документов на оплату "false"
Когда пользователь сохраняет этого абонента типа Счет юр.лица
Тогда запись по этому абоненту присутствует в разделе абонентов типа Счет юр.лица
И пользователь удаляет этого абонента Тип абонента Счет юр.лица
Тогда запись по этому абоненту отсутствует в разделе абонентов Тип абонента Счет юр.лица

Сценарий: успешное добавление помещения к абоненту типа Счет физ.лица
Дано пользователь добавляет абонента типа Счет физ.лица
И пользователь у этого абонента типа Счет физ.лица заполняет поле Льготная категория
И пользователь у этого абонента типа Счет физ.лица заполняет поле Фамилия "Оборин"
И пользователь у этого абонента типа Счет физ.лица заполняет поле Имя "Сергей"
И пользователь у этого абонента типа Счет физ.лица заполняет поле Отчество "Иванович"
И пользователь у этого абонента типа Счет физ.лица заполняет поле Дата рождения "12.06.1961"
И пользователь у этого абонента типа Счет физ.лица заполняет поле Тип документа "Паспорт"
И пользователь у этого абонента типа Счет физ.лица заполняет поле Серия документа "9206"
И пользователь у этого абонента типа Счет физ.лица заполняет поле Номер документа "612345"
Когда пользователь сохраняет этого абонента типа Счет физ.лица
Допустим пользователь добавляет запись по сведению о помещении абоненту типа Счет физ.лица
И пользователь у этой записи по сведению о помещении заполняет поле Дата открытия ЛС "01.01.2015" абоненту типа Счет физ.лица
И пользователь у этой записи по сведению о помещении заполняет поле Жилой дом абоненту типа Счет физ.лица
И пользователь у этой записи по сведению о помещении заполняет поле № квартиры/помещения абоненту типа Счет физ.лица
И у этого № квартиры/помещения заполняет поле Доля собственности "1" абоненту типа Счет физ.лица
Когда пользователь сохраняет эту запись по сведению о помещении абоненту типа Счет физ.лица
Тогда запись по этому сведению о помещении присутствует в сведениях о помещении по этому абоненту типа Счет физ.лица
И Не выпало не одной ошибки

Сценарий: успешное добавление дубля абонента с типом "Счет физ.лица"
Дано пользователь добавляет абонента типа Счет физ.лица
И пользователь у этого абонента типа Счет физ.лица заполняет поле Льготная категория 
И пользователь у этого абонента типа Счет физ.лица заполняет поле Фамилия "Оборин"
И пользователь у этого абонента типа Счет физ.лица заполняет поле Имя "Сергей"
И пользователь у этого абонента типа Счет физ.лица заполняет поле Отчество "Иванович"
И пользователь у этого абонента типа Счет физ.лица заполняет поле Дата рождения "12.06.1961"
И пользователь у этого абонента типа Счет физ.лица заполняет поле Тип документа "Паспорт"
И пользователь у этого абонента типа Счет физ.лица заполняет поле Серия документа "9206"
И пользователь у этого абонента типа Счет физ.лица заполняет поле Номер документа "612345"
Когда пользователь сохраняет этого абонента типа Счет физ.лица 
Дано пользователь добавляет абонента типа Счет физ.лица
И пользователь у этого абонента типа Счет физ.лица заполняет поле Льготная категория 
И пользователь у этого абонента типа Счет физ.лица заполняет поле Фамилия "Оборин"
И пользователь у этого абонента типа Счет физ.лица заполняет поле Имя "Сергей"
И пользователь у этого абонента типа Счет физ.лица заполняет поле Отчество "Иванович"
И пользователь у этого абонента типа Счет физ.лица заполняет поле Дата рождения "12.06.1961"
И пользователь у этого абонента типа Счет физ.лица заполняет поле Тип документа "Паспорт"
И пользователь у этого абонента типа Счет физ.лица заполняет поле Серия документа "9206"
И пользователь у этого абонента типа Счет физ.лица заполняет поле Номер документа "612345"
Когда пользователь сохраняет этого абонента типа Счет физ.лица 
Тогда запись по этому абоненту присутствует в разделе абонентов типа Счет физ.лица

Сценарий: успешное добавление дубля абонента с типом "Счет юр.лица"
Дано пользователь добавляет абонента типа Счет юр.лица
И пользователь у этого абонента типа Счет юр.лица заполняет поле Льготная категория
И пользователь у этого абонента типа Счет юр.лица заполняет поле Контрагент
#И пользователь у этого абонента типа Счет юр.лица заполняет поле ИНН "????????"
#И пользователь у этого абонента типа Счет юр.лица заполняет поле КПП "????????"
#GKH-2634 И пользователь у этого абонента типа Счет юр.лица заполняет поле Адрес для корреспонденции "Юридический адрес"
И пользователь у этого абонента типа Счет юр.лица заполняет поле Печатать акт при печати документов на оплату "false"
Когда пользователь сохраняет этого абонента типа Счет юр.лица
Дано пользователь добавляет абонента типа Счет юр.лица
И пользователь у этого абонента типа Счет юр.лица заполняет поле Контрагент
#И пользователь у этого абонента типа Счет юр.лица заполняет поле ИНН "????????"
#И пользователь у этого абонента типа Счет юр.лица заполняет поле КПП "????????"
#И пользователь у этого абонента типа Счет юр.лица заполняет поле Адрес для корреспонденции "Юридический адрес"
Когда пользователь сохраняет этого абонента типа Счет юр.лица 
Тогда запись по этому абоненту присутствует в разделе абонентов типа Счет юр.лица 