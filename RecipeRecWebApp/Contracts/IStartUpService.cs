namespace RecipeRecWebApp.Contracts
{
    public interface IStartUpService
    {
        public Task InitializeIngredientsAsync();
    }
}