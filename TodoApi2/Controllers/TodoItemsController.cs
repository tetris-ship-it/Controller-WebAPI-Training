using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApi2.Models;

namespace TodoApi2.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TodoItemsController : ControllerBase
{
    private readonly TodoContext _context;

    public TodoItemsController(TodoContext context)
    {
        _context = context;
    }

    // GET: api/TodoItems
    //using Task indicates it is an asynchronous operation, it allows the action method to execute asynchronously without blocking the main thread so adding it before ActionResult means that the method returns a task which will eventually produce an <ActionResult<IEnumerable<TodoItemDTO>>> result.
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TodoItemDTO>>> GetTodoItems()//ActionResult<Type of data returned> is a generic class that represents the result of an action method in an MVC or a Web API controller. IEnumberable<type> represents that it returns the sequence of items of the specified type.
    {
        return await _context.TodoItems
            .Select(x => ItemToDTO(x))//select is one of the LINQ extension methods that transform a sequence of elements(in this case the TodoItems in the inmemory db collection) to another form(in this case to TodoItemDTO elements)
            .ToListAsync();//we are changing the todo items to todoDTO items because we only want to get the data transfer objects(subset of the properties of the model class, ommitting the nullable("typeOfData?") IsSecret property)
    }
    /*A very good example of using the select and where methods for filtering:
     * 
     List<int> numbers = new List<int> {1,2,3,4,5};
     List<string> evenNumberStrings = numbers.Where(n=>n%2==0).Select(n=>n.ToString()).ToList(); 

    **ToList() is used to convert an IEnumerable<T> sequence of elements into a List<T> set of elements and it is synchronous in its making because it pulls all elements into memory and fully populates the list.
    **ToListAsync() is used to convert data to a List<T> asynchronously meaning it allows other operations to continue while it executes/while the data is being fetched. It is better suited for database queries, API calls, I/O based tasks because it utilizes the system resources good and it is responsive.

     */
    // GET: api/TodoItems/5
    // <snippet_GetByID>
    [HttpGet("{id}")]
    public async Task<ActionResult<TodoItemDTO>> GetTodoItem(long id)
    {
        var todoItem = await _context.TodoItems.FindAsync(id);

        if (todoItem == null)
        {
            return NotFound();
        }

        return ItemToDTO(todoItem);
    }
    // </snippet_GetByID>

    // PUT: api/TodoItems/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    // <snippet_Update>
    [HttpPut("{id}")]
    public async Task<IActionResult> PutTodoItem(long id, TodoItemDTO todoDTO)
    {
        if (id != todoDTO.Id)//if the route parameter ID is not the same as the updated object todoDTO's id then send a badrequest status error message.
        {
            return BadRequest();
        }

        var todoItem = await _context.TodoItems.FindAsync(id);
        if (todoItem == null)
        {
            return NotFound();
        }

        todoItem.Name = todoDTO.Name;
        todoItem.IsComplete = todoDTO.IsComplete;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException) when (!TodoItemExists(id))
        {
            return NotFound();
        }

        return NoContent();
    }
    // </snippet_Update>

    // POST: api/TodoItems
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    // <snippet_Create>
    [HttpPost]
    public async Task<ActionResult<TodoItemDTO>> PostTodoItem(TodoItemDTO todoDTO)
    {
        var todoItem = new TodoItem
        {
            IsComplete = todoDTO.IsComplete,
            Name = todoDTO.Name
        };

        _context.TodoItems.Add(todoItem);
        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetTodoItem),
            new { id = todoItem.Id },
            ItemToDTO(todoItem));//the createdAtAction method combines these paramters to appropriately make/create an http 201 response.
    }
    // </snippet_Create>

    // DELETE: api/TodoItems/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTodoItem(long id)//Task indicates the method asynchronously returns a task which may resolve to a successful deletion operation.
    {
        var todoItem = await _context.TodoItems.FindAsync(id);
        if (todoItem == null)
        {
            return NotFound();
        }

        _context.TodoItems.Remove(todoItem);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool TodoItemExists(long id)
    {
        return _context.TodoItems.Any(e => e.Id == id);//the Any() method checksand returns true if there is at least one TodoItem object in the TodoItems collection with the id parameter that was passed in else it returns false. If the collection is empty Any() returns false so we can use this behavior.
    }
    /*these methods: Select, Where, Any and the such take in lambda expressions as parameters
     lambda expressions  are basically written in the form "x=>x.methodName()" or some other form like "x=some expression containing x"*/
    private static TodoItemDTO ItemToDTO(TodoItem todoItem) =>
       new TodoItemDTO
       {
           Id = todoItem.Id,
           Name = todoItem.Name,
           IsComplete = todoItem.IsComplete
       };//this is an arrow function taking in a todoItem object then creates a new TodoItem DTO object proceeding to return the newly created object.
}
