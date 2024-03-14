## 2й этап
Проект cmd - 2й этап. Работает в командной строке. Точка входа - метод Main в классе Program.

# 4й этап
Проект client - клиент. Интерфейс построен с использованием .NET MAUI. Точка входа - метод CreateMauiApp(), в классе MauiProgram. Паттерн - MVVM. Для этого используется библиотека: CommunityToolkit.Mvvm (https://www.nuget.org/packages/CommunityToolkit.Mvvm).

Проект server - сервер. Работает на ASP .NET Core. Авторизация работает через систему Identity с базой данных. Файл конфигурации со строкой подключения к БД - appSettings.json.

Используются api контроллеры для авторизации, регистрации и получения данных. 

## Про БД
Проще всего создать базу данных с помошью миграции:

Для этого, в консоли диспетчера пакетов VS нужно прописать:
* Add-Migration Initial - Создаст миграцию
* Update-Database - Применит изменения в БД

Строк подключения к БД указана в appsettings.json. База данных будет создана с названием, указанным в строке подключения.
Ip и port сервера указывается в appsettings.json.

## Логи
* При запуске из VS папка logs появляется в запущенного проекта. 
* При запуске exe, папка logs появляется в папке запущенного exe.

### Логи клиента:
Папка logs

### Логи сервера:
Папка logs:
* AspNetCore - лог работы сервера ASP .NET Core
* Simulator - лог работы модели из 2-го этапа

## Не сделано
* Отображение текущего состояния подключения к серверу, только при ошибке
* Нет дополнительной формы регистрации
* В коде: нет комментариев, нет регионов, да и сам код написан на скорую руку
