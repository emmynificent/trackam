using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

[Route("api/[controller]")]
[ApiController]
public class CategoryController : ControllerBase
{
    private readonly ICategory _icategory;
    private readonly IMapper _imapper ; 
    public CategoryController(ICategory category, IMapper mapper)
    {
        _icategory = category;
        _imapper = mapper;
    }

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpGet]
    public async Task<IActionResult> AllCategories()
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        var category = await _icategory.GetCategoriesByUserId(userId);
        var categoryMapped = _imapper.Map<List<CategoryOutputDTO>>(category);
        return Ok(categoryMapped);
    }

    [HttpPost("CreateCategory")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> CreateCategory(CategoryInputDTO category)
    {
        if(category == null)
        {
            return BadRequest();
        }
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        
        var newCategoryMapped = _imapper.Map<Category>(category);
        newCategoryMapped.UserId = userId;

        if(await _icategory.GetCategoryByName(category.Name) != null)
        {
            return BadRequest("Category with the same name already exists");
        }
        var newCategory = await _icategory.CreateCategory(newCategoryMapped);
        
        return Ok("success! Category has been created");
    
    }

    [HttpDelete("{categoryId}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme )]

    public async Task<IActionResult> DeleteCategory(int categoryId)
    {
        if(categoryId <= 0) 
        {
            return BadRequest("Invalid Input");
        }
        var category = await _icategory.GetCategoryById(categoryId);
        if(category == null)
        {
            return NotFound("Category Not Found");
        }
        await _icategory.DeleteCategory(category);
        return Ok();
    }

    [HttpPut ("UpdateCategories/{categoryId}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> UpdateCategory (int categoryId, CategoryInputDTO category)
    {
        if(categoryId <= 0 || category == null) return BadRequest("Category does not exist");
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        var existingCategory = await _icategory.GetCategoryById(categoryId);
        if(existingCategory == null) return NotFound("Category not found");
        if(existingCategory.UserId != userId) return Unauthorized("You can only touch what you created");
        existingCategory.Description = category.Description;
        existingCategory.Name = category.Name;
        await _icategory.UpdateCategory(existingCategory);
        return Ok("Category has been updated");

    }

    [HttpGet("GetCategoriesByUserId")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> GetCategoriesByUserId()
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        var categories = await _icategory.GetCategoriesByUserId(userId);
        var categoriesMapped = _imapper.Map<List<CategoryOutputDTO>>(categories);
        return Ok(categoriesMapped);
    }


    // [HttpGet]
    // public async Task<IActionResult> AllCategoriesAdmin()
    // {
    //     var category = await _icategory.GetAllCategory();
    //     var categoryMapped = _imapper.Map<List<CategoryOutputDTO>>(category);
    //     return Ok(categoryMapped);
    // }

}