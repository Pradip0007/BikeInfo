using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApp.Model;

namespace WebApp.Pages.User
{
    public class DeleteModel : PageModel
    {
        public string errorMessage { get; set; } = string.Empty;
        private readonly IConfiguration configuration;

        public DeleteModel(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        public IActionResult OnPost(int id)
        {
            try
            {
                DAL dal = new DAL();
                int i = dal.DeleteUser(id, configuration);
                if (i > 0)
                {
                    return RedirectToPage("/User/Index");
                }
                else
                {
                    errorMessage = "Error deleting user.";
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }
            return Page();
        }
    }
}
