using System;

public interface IIntegerStat
{
    int Value { get; set; }
    int Max { get; set; }
    public event ChangeNotificator<int> OnChanged;
}