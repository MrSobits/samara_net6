﻿Функция: Ошибка печати физиков

Предыстория: 
#Дано в реестр жилых домов добавлен новый дом
#| region    | houseType       | city                                          | street             | houseNumber |
#| kamchatka | Многоквартирный | Камчатский край, Алеутский р-н, с. Никольское | ул. 50 лет Октября | д. test777  |
#| sahalin   | Многоквартирный | Костромское                                   | Новая              | д. 1211777  |

#И у этого дома добавлено помещение
#| RoomNum | Area | LivingArea | Type  | OwnershipType |
#| 1       | 51   | 35         | Жилое | Частная       |

#И добавлен абонент типа Счет физ.лица
#| Surname | FirstName | secondName | BirthDate  | IdentityType | IdentitySerial | IdentityNumber |
#| 1777    | 1777      | 1777       | 12.06.1961 | 10           | 0000           | 000000         |

#И добавлен абонент типа Счет юр.лица
#| Contragent |
#| 111        |

#при привязки помещения к абоненту, создаётся ЛС который будем считать текущим
#И добавлено помещение абоненту типа Счет физ.лица
#| region    | OpenDate   | RealityObjectAddress                          | RoomNum | AreaShare |
#| kamchatka | 01.01.2015 | с. Никольское, ул. 50 лет Октября, д. test777 | 1       | 1         |
#| sahalin   | 01.01.2015 | с. Костромское, ул. Новая, д. 1211777         | 1       | 1         |

Допустим пользователь в реестре ЛС выбирает лицевой счет "100000377"

Сценарий: Ошибка печати физиков

Допустим пользователь выбирает Период "2014 Июль"
И пользователь в реестре ЛС выбирает текущий ЛС
Когда пользователь в реестре ЛС выбирает действие Выгрузка - Предпросмотр документов на оплату 
И пользователь в реестре ЛС выбирает предпросмотр текущего ЛС
И Пользователь в реестре ЛС выбирает действие Выгрузка - Документы на оплату
Тогда Не выпало не одной ошибки
И в реестре задач появилась задача с Наименованием "Формирование документов на оплату"
И в течении 1 мин статус задачи стал "Успешно выполнена"
И в течении 1 мин процент выполнения задачи стал "100"
И в течении 1 мин ход выполнения задачи стал "Завершено"