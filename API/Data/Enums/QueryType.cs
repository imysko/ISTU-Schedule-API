namespace API.Data.Enums;

public enum QueryType
{
    Add = 0,
    Replace = 1,
    Move = 2,
    MoveFrom = 3,
    MoveTo = 4,
    WarningTeacher = 5,
    Swap = 6,
    Unknown = 100
}

//  add = ChoiceItem(0) 
//  replace = ChoiceItem(1) 
//  move = ChoiceItem(2) # означает что пара перенесена просто в другую аудиторию 
//  move_from = ChoiceItem(3) # означает что пара перенесена с этого дня 
//  move_to = ChoiceItem(4) # означает что пара перенесена на этот день 
//  warning_teacher = ChoiceItem(5) # означает предупреждение 
//  swap = ChoiceItem(6) # означает смена занятий местами
// 
//  А что такое 100 я не знаю)