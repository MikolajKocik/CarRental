﻿namespace Wypożyczalnia_samochodów_online.Models;

public class Notification
{
    public string Type { get; set; }
    public string Message { get; set; } 
    public Notification(string type, string message)
    {
        Type = type;
        Message = message;
    }
}
