# Расчет зарплаты ООО “Разработка софта”
Моя версия реализации задания, опубликованного на youtube-канале Степана Берегового: https://www.youtube.com/c/SBeregovoyRU/

Описание задания:
https://docs.google.com/document/d/1kZz1ozAwNTVkIxWoyPYI_zTw6mos3CI03MyXnCxNbeM/edit#heading=h.jolz5iaffihg

## Реализация ##

**Проект Data**
- слой работы с БД IRepository (реализация TextFileDB)
- объекты хранения данных (Person, Worker, TimeRecord)

**Проект Domain**
- объекты с поведением (сотрудники, таблицы учета рабочего времени, отчеты)
- слой бизнес-логики IService

Объекты реализующие IWorker - виды сотрудников по ролям:
- менеджер (Manager)
- сотрудник на зарплате (Employee)
- фрилансер (Freelancer)

## Что можно улучшить ##

- IoC-контейнер для глобального управления зависимостями (https://github.com/thangchung/awesome-dotnet-core#ioc);
- custom-responce object для упаковки результата выполнения команды, текста ошибки, возвращаемого объекта;
- устранить избыточную вычитку из БД. Производить фильтрацию на уровне запросов к БД, а не на уровне IService. Пример: GetTimeRecords(IWorker worker) выбирает из БД все рабочие часы сотрудников с той же ролью. Можно добавить аналогичный метод в IRepository.

## Что можно добавить ##

- логирование для хранения истории выполнения команд. Эдакая аналитика каким функционалом чаще пользуются (https://github.com/thangchung/awesome-dotnet-core#logging)
