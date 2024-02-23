## 2й этап
Проект second_stage - 2й этап. Работает в командной строке. Точка входа - метод Main в классе Program.
Логи хранятся в директории вместе с исполняемым файлом.

## 3й этап
Проект stage3 - 3й этап. Интерфейс построен с использованием .NET MAUI. Точка входа - метод CreateMauiApp(), в классе MauiProgram. Работает с БД postgresql, с помощью Entity Framework Core.
Файл конфигурации со строкой подключения к БД - appSettings.json лежит в папке Properties. Паттерн - MVVM. Для этого используется библиотека: CommunityToolkit.Mvvm (https://www.nuget.org/packages/CommunityToolkit.Mvvm).

## Детали
Разрабатывал на windows 11, .NET 8.

TODO:
* Перепись кода 2-го этапа;
* Допил графического интерфейса;