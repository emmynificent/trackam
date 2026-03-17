using AutoMapper;   

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Expense, ExpenseInputDTO>().ReverseMap();
        CreateMap<Expense, ExpenseOutputDTO>().ReverseMap();
        CreateMap<Category, CategoryInputDTO>().ReverseMap();
        CreateMap<Category, CategoryOutputDTO>().ReverseMap();
        CreateMap<User, UserInputDTO>().ReverseMap();   
        CreateMap<User, UserOutputDTO>().ReverseMap();  
        
        
    }
}   