using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
[ApiController]
[Route ("api/[controller]")]
public class ExpenseController : ControllerBase
{
    private readonly IExpense _iexpense;
    private readonly IMapper _mapper;
    public ExpenseController(IExpense expense, IMapper mapper)
    {
        _iexpense = expense;
        _mapper = mapper;
    }

    [HttpGet()]
   // [Authorize (Roles = "Admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task <IActionResult> GetExpense()
    {
        var allExpense = await _iexpense.GetAllExpense();
       var listOfExpense =  _mapper.Map<List<ExpenseOutputDTO>>(allExpense);
        return Ok(listOfExpense);
    }

    [HttpGet("myexpenses")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> GetExpenseForUser()
    {
       var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

        var expenses = await _iexpense.GetAllExpensesAsyncByUser(userId);
        if(expenses == null || !expenses.Any()){return Ok("No expenses found for this user");}
         var expenseMapped = _mapper.Map<List<ExpenseOutputDTO>>(expenses);
        return Ok(expenseMapped);
    }

    [HttpPost ("CreateExpense")]
    [Authorize (AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> CreateExpense(ExpenseInputDTO expense)
    {
        if(expense == null)
        {
            return BadRequest();
        }
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

        var expenseToCreate = _mapper.Map<Expense>(expense);
        expenseToCreate.UserId = userId;

         await _iexpense.CreateExpenseAsync(expenseToCreate);
         return CreatedAtAction(nameof(GetExpenseForUser), expenseToCreate);
    }

    [HttpDelete("DeleteExpense/{expenseId}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> DeleteExpense(int expenseId)
    {
        if (expenseId <= 0) return BadRequest();

        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

        var expenseExist = await _iexpense.GetExpenseByIdAsync(expenseId);
        if (expenseExist == null) return NotFound("Expense not found");

        // ownership check
        if (expenseExist.UserId != userId)
            return Unauthorized("You can only delete your own expenses");

        await _iexpense.DeleteExpenseAsync(expenseExist);
        return Ok("Request successful");
    }
    

    [HttpGet("category/{categoryId}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> GetExpensesByCategory(int categoryId)
    {
        if(categoryId <= 0)
        {
            return BadRequest("Invalid category ID");
        }
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        var expenses = await _iexpense.GetExpensesByCategoryIdAsync(categoryId, userId);
        if (expenses == null || !expenses.Any())
        {
            return Ok("No expenses found for this category");
        }
        var expenseMapped = _mapper.Map<List<ExpenseOutputDTO>>(expenses);
        return Ok(expenseMapped);
    }


    [HttpPut("UpdateExpense/{expenseId}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> UpdateExpense(int expenseId, ExpenseInputDTO expense)
    {
        if(expenseId <= 0 ) return BadRequest("Invalid Expense ID");
        if(expense == null) return BadRequest();
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

        var oldExpense =await _iexpense.GetExpenseByIdAsync(expenseId);
        if (oldExpense == null) return NotFound("Expense does not exist");

        if(oldExpense.UserId != userId)
            return Unauthorized("You can only update your own expenses");
        oldExpense.Amount = expense.Amount;
        oldExpense.CategoryId = expense.CategoryId;
        oldExpense.UpdatedAt = DateTime.UtcNow;
        oldExpense.Description = expense.Description;

        await _iexpense.UpdateExpenseAsync(oldExpense);
        return Ok("Expense Updated Successfully");
    }
}