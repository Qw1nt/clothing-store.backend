﻿using Domain.Common;

namespace Domain.Entities;

public class Order : Entity
{
    public int UserId { get; set; }
    
    public string JsonData { get; set; } = null!;
    
    public DateTime Date { get; set; }
}