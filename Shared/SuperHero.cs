namespace BlazorCrudApp.Shared
{
    public class SuperHero
    {
       public int Id { get; set; } 
       public string FirstName { get; set; } = String.Empty;
       public string LastName { get; set; } = String.Empty;
       public string HeroName { get; set; } = String.Empty;
       public Comic Comic {get; set;}
       public int ComicId {get; set;}

    }
}