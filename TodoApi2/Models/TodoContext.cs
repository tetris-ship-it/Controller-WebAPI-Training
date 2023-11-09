using Microsoft.EntityFrameworkCore;

namespace TodoApi2.Models;

public class TodoContext : DbContext
{
    public TodoContext(DbContextOptions<TodoContext> options)
        : base(options)//this passes options to the parent's contructor class initializing the DbContext.
    {
    }

    public DbSet<TodoItem> TodoItems { get; set; } = null!;//the DbSet<TodoItem> represents the fact that there will be a set of todoitem entities called TodoItems.
}
//the get; and set; indicate that the property has both getters and setters allowing for read and write actions to it.
//the null! known as the null forgiving operator is used for indicating that the property can be null but the code guarantees that it wont be so it's there just for satisfying the compiler.