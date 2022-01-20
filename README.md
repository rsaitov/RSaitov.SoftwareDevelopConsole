# Расчет зарплаты ООО “Разработка софта”
Моя версия реализации задания, опубликованного на youtube-канале Степана Берегового: https://www.youtube.com/c/SBeregovoyRU/

Описание задания:
https://docs.google.com/document/d/1kZz1ozAwNTVkIxWoyPYI_zTw6mos3CI03MyXnCxNbeM/edit#heading=h.jolz5iaffihg

## Реализация ##

**Проект Data**
- слой работы с БД IRepository (реализация TextFileDB)
- слой бизнес-логики IService

**Проект Domain**
- объекты с поведением (сотрудники, таблицы учета рабочего времени, отчеты)

Объекты реализующие IWorker - виды сотрудников по ролям:
- менеджер (Manager)
- сотрудник на зарплате (Employee)
- фрилансер (Freelancer)

## Что можно улучшить ##

- IoC-контейнер для глобального управления зависимостями (NInject, Unity, Autofac)
- custom-responce object для упаковки результата выполнения команды, текста ошибки, возвращаемого объекта
