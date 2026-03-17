public class User
{
    public int Id {get; set;}
    public string Username {get; set;}
    public string Email {get; set;}
    public List<Expense>? My_Expenses {get; set;}
    public List<Category>? My_Categories {get; set;}
    public string GoogleId {get; set;}
    public DateTime CreatedAt {get; set;}

}
