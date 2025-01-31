namespace Models
{
	public class RecipeModel
	{
		public int Id { get; set; }
		public string Name { get; set; } = string.Empty;
		public string Description { get; set; } = string.Empty;
		public List<IngredientModel> RequiredIngredients { get; set; } = [];
		public bool IsVeg { get; set; } = true;
		public List<IngredientModel> ExtraSelectedIngredients { get; set; } = []; //selected in pantry but not in recipe
		public List<IngredientModel> AdditionalRequiredIngredients { get; set; } = []; //required in recipe but not present in pantry
	}
}
