﻿namespace TodoApi2.Models
{
    public class TodoItem
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        public bool IsComplete { get; set; }
        public string? IsSecret { get; set; } 
    }
}
