using System.Threading.Tasks;
using OptiRiskAI.Authors;
using Microsoft.AspNetCore.Mvc;
namespace OptiRiskAI.Web.Pages.Authors
{
    public class CreateModalModel : OptiRiskAIPageModel
    {
        [BindProperty]
        public CreateUpdateAuthorDto Author { get; set; }
        private readonly IAuthorAppService _authorAppService;
        public CreateModalModel(IAuthorAppService authorAppService)
        {
            _authorAppService = authorAppService;
        }
        public void OnGet()
        {
            Author = new CreateUpdateAuthorDto();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            await _authorAppService.CreateAsync(Author);
            return NoContent();
        }
    }
}
