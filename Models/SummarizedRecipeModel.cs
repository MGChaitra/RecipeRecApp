using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class SummarizedRecipeModel
    {
        public string Title { get; set; } = string.Empty;
        public string Ingredients {  get; set; } = string.Empty;
        public string Summary {  get; set; } = string.Empty;
    }
}
