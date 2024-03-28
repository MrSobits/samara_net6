**<h3 align="center">Bars Esia OAuth20 App</h3>**
<h3 align="center">Модуль авторизации ЕСИА</h3>

---

<p align="center"> Краткое руководство.
    <br> 
</p>

## 📝 **Разделы**

- [About](#🏁-about)
- [Windows service](#🏁-windows-service)
- [Console application](#🏁-console-application)
- [B4.CONFIG](#🏁-b4config)
- [Supported settings](#🏁-supported-settings)
- [Additional info](#🏁-additional-info)

---

## 🏁 **About**

Модуль авторизации ЕСИА представляет собой отдельное приложение с поддержкой развертывания в виде сервиса-службы Windows. Реализовано в рамках [задачи](https://jira.bars.group/browse/GKH-20552).

## 🏁 **Windows service**

Варианты развертывание модуля авторизации в виде службы Windows:
- Регистрация исполняемого файла модуля в службах сторонними решенями. К примеру, встроенным в Windows SC.EXE
```
sc create MyService binPath=C:\MyService\MyService.exe DisplayName=″My New Service″ type=own start=auto
```
- Использование встроенного в модуль _installer'а_ (для удаления вместо -i указать параметр -u)
```
Bars.Esia.OAuth20.App.exe -i
```

## 🏁 **Console application**

Для запуска в виде консольного приложения необходимо указать параметр -s
```
Bars.Esia.OAuth20.App.exe -s
```

## 🏁 **B4.CONFIG**

Конфиг настроен под локальный запуск и ориентирован на тестовую среду ЕСИА, необходимо лишь установить сертификат с закрытым ключом. См. [дополнительную информацию](#🏁-additional-info) и описание параметра _**CertificateThumbPrint**_ среди [параметров клиента ЕСИА](#🏁-supported-settings).
<br>
Для стендов необходимо указать соответствующий _CallbackUri_ и ориентир на среду ЕСИА - параметры _RedirectUri, TokenUri и RestUri_.

## 🏁 **Supported settings**

Модуль построен на основе компонентов B4, поэтому конфигурационным файлом является _**b4.config**_ или _**b4.user.config**_, который будет перекрывать первый. Настройка logger'ов осуществляется в этих же файлах.

Реализованные параметры приложения (секция _appsettings_):
- **Параметры приложения**:
  - SocketListeningAddress (опционально) - адрес для прослушивания сообщений, указать если приложение неправильно определяет ip-шник сервака
  - SocketListeningPort - порт для прослушивания сообщений
  - SocketConnectionsLength (опционально, по умолчанию 1) - макс. количество соединений, которое будет обрабатывать модуль
  - SocketReceiveBufferLength (опционально, по умолчанию 1024) - размер буффера для считывания сообщений
- **Параметры клиента ЕСИА**:
  - RequestTimeout (опционально, по умолчанию 60 сек) - timeout запроса к ЕСИА
  - MaxResponseContentBufferSize (опционально, по умолчанию 10Мб) - размер буффера запроса к ЕСИА
  - CertificateThumbPrint - отпечаток сертификата, который должен находиться в My-хранилище на LocalMachine (локалка -> личное)
  - ClientId - идентификатор ИС (мнемоника)
  - CallbackUri - адрес, на который должна вернуть ЕСИА после авторизации
  - RedirectUri - адрес для получения кода доступа из ЕСИА
  - TokenUri - адрес для получения маркера доступа из ЕСИА
  - RestUri - адрес для получения данных из ЕСИА
  - Scope - область доступа (для получения ФИО и организаций пользователя указывать "fullname usr_org")
  - RequestType (опционально, по умолчанию code) - Тип запроса (code - для получения маркера доступа, т.е. пользователь всегда должен быть)
  - PrnsRef, CttsRef, AddrsRef, DocsRef, OrgsRef (опциональны) - ссылки на контейнеры с соответствующими данными в ЕСИА

## 🏁 **Additional info**

Учетки ЕСИА можно найти [тут](https://conf.bars.group/pages/viewpage.action?pageId=145322579).
<br>
Все необходимые для запуска файлы с описанием можно найти в комментариях [этой задачи](https://jira.bars.group/browse/GKH-20552).
<br>
