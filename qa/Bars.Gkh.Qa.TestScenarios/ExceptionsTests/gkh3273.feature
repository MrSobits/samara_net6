﻿@ScenarioInTransaction
Функция: Ошибка возврата средств
Региональный фонл - Счета - Банковские операции

Предыстория:
Дано в реестр жилых домов добавлен новый дом
| region     | houseType       | city                                          | street             | houseNumber |
| testregion | Многоквартирный | Камчатский край, Алеутский р-н, с. Никольское | ул. 50 лет Октября | д. test     |

И выполненно действие "Создание операций по счету начислений дома (если отсутствуют)"

И у этого дома добавлено помещение
| RoomNum | Area | LivingArea | Type  | OwnershipType |
| 1       | 51   | 35         | Жилое | Частная       |

И добавлен абонент типа Счет физ.лица
| Surname | FirstName | secondName | BirthDate  | IdentityType | IdentitySerial | IdentityNumber |
| 1       | 1         | 1          | 12.06.1961 | 10           | 9206           | 612345         |

#при привязки помещения к абоненту, создаётся ЛС который будем считать текущим
И добавлено помещение абоненту типа Счет физ.лица
| region     | OpenDate   | RealityObjectAddress                       | RoomNum | AreaShare |
| testregion | 01.01.2015 | с. Никольское, ул. 50 лет Октября, д. test | 1       | 1         |

И добавлена банковская операция
| DateReceipt | Sum | MoneyDirection |
| текущая дата | 100 | Приход         |
| текущая дата | 100 | Расход         |

#Надо переделывать полностью
@ignore
Сценарий: возврат средств
Когда пользователь производит действие зачисления по кнопке Распределить для банковской операции с типом Приход
И пользователь у этой банковской операции для распределения Тип распределения "Платеж КР"
#И пользователь у банковской операции с типом Приход для распределения Вид распределения "Равномерное"
И пользователь выбирает этот лицевой счет
#И пользователь формирует распределение
И пользователь применяет распределение
И пользователь производит действие зачисления по кнопке Распределить для банковской операции с типом Расход
И пользователь у этой банковской операции для распределения Тип распределения "Возврат средств"
#И пользователь у банковской операции с типом Расход для распределения Вид распределения "Равномерное"
И пользователь выбирает этот лицевой счет
#И пользователь формирует распределение
И пользователь применяет распределение
Тогда в списке банковских операций у этой банковской операции статус = Распределена
И в карточке этого лицевого счета, в операциях за текущий период, присутствует запись "Оплата по базовому тарифу" с датой операции из этой банковской операции и Изменение сальдо = 100
#И выходит сообщение с тектом "Распределение выполнено успешно!"