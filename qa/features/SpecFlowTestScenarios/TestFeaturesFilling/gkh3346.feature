﻿
Функция: Ошибка создания лицевого счёта №3346
Региональный фонд -- Абоненты -- Реестр абонентов

Предыстория: 

Дано в реестр жилых домов добавлен новый дом
| region    | houseType       | city                                          | street             | houseNumber |
| kamchatka | Многоквартирный | Камчатский край, Алеутский р-н, с. Никольское | ул. 50 лет Октября | д. test     |
| sahalin   | Многоквартирный | Костромское                                   | Новая              | д. 1211     |

И у этого дома добавлено помещение
| RoomNum | Area | LivingArea | Type  | OwnershipType |
| 1       | 51   | 35         | Жилое | Частная       |

И добавлен абонент типа Счет физ.лица
| Surname | FirstName | secondName | BirthDate  | IdentityType | IdentitySerial | IdentityNumber |
| 1       | 1         | 1          | 12.06.1961 | 10           | 9206           | 612345         |

#И добавлен абонент типа Счет юр.лица
#| Contragent |
#| 111        |

#при привязки помещения к абоненту, создаётся ЛС который будем считать текущим
И добавлено помещение абоненту типа Счет физ.лица
| region    | OpenDate   | RealityObjectAddress                       | RoomNum | AreaShare |
| kamchatka | 01.01.2015 | с. Никольское, ул. 50 лет Октября, д. test | 1       | 0.5       |
| sahalin   | 01.01.2015 | с. Костромское, ул. Новая, д. 1211         | 1       | 0.5       |

Сценарий: Ошибка создания лицевого счёта №3346

Тогда При добавлении нового сведения о помещениях есть возможность выбора этой комнаты