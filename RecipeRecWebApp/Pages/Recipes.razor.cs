namespace RecipeRecWebApp.Pages
{
    public partial class Recipes
    {
        private string searchTerm = string.Empty;
        private List<Category> Categories = new();
        private List<Category> FilteredCategories = new();

        protected override async Task OnInitializedAsync()
        {
            try
            {

                var jsonData = await Http.GetStringAsync("data/ingredients.json");
                Categories = System.Text.Json.JsonSerializer.Deserialize<List<Category>>(jsonData) ?? new List<Category>();


                foreach (var category in Categories)
                {
                    category.IsExpanded = true;
                    foreach (var ingredient in category.Ingredients)
                    {
                        ingredient.Visible = true;
                        ingredient.Selected = false;
                    }
                }

                FilteredCategories = Categories;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading categories: {ex.Message}");
            }
        }

        private void FilterIngredients()
        {
            foreach (var category in Categories)
            {
                foreach (var ingredient in category.Ingredients)
                {
                    ingredient.Visible = string.IsNullOrWhiteSpace(searchTerm) || ingredient.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase);
                }
            }
        }

        private void ToggleCategory(string categoryName)
        {
            var category = Categories.FirstOrDefault(c => c.Name == categoryName);
            if (category != null)
            {
                category.IsExpanded = !category.IsExpanded;
            }
        }

        private void ToggleIngredientSelection(Ingredient ingredient) => ingredient.Selected = !ingredient.Selected;

        private class Ingredient
        {
            public string Name { get; set; } = string.Empty;
            public string ImageUrl { get; set; } = string.Empty;
            public bool Visible { get; set; } = true;
            public bool Selected { get; set; } = false;
        }

        private class Category
        {
            public string Name { get; set; } = string.Empty;
            public string Icon { get; set; } = string.Empty;
            public bool IsExpanded { get; set; } = true;
            public List<Ingredient> Ingredients { get; set; } = new();
        }
    }
}