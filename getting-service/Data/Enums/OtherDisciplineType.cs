namespace getting_service.Data.Enums;

public enum OtherDisciplineType
{
    Project = 2
    // 0 — просто дисциплина, которую какой-то пользователь добавил вручную
    // 1 — прокси дисциплина, я валяется копией реальной нагрузки из учебного плана по полю nagruzka_id, которая ссылается на таблицу nagruzka
    // 2 — проектная дисциплина
}