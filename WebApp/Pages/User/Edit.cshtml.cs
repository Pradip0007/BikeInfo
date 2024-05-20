using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApp.Model;

namespace WebApp.Pages.User
{
    public class EditModel : PageModel
    {
        [BindProperty]
        public Users user { get; set; } = new Users();
        public string succesMessage { get; set; } = string.Empty;
        public string errorMessage { get; set; } = string.Empty;
        private readonly IConfiguration configuration;

        public EditModel(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public void OnGet()
        {
            if (int.TryParse(Request.Query["id"], out int id))
            {
                try
                {
                    DAL dal = new DAL();
                    user = dal.GetUser(id, configuration);
                }
                catch (Exception ex)
                {
                    errorMessage = ex.Message;
                }
            }
            else
            {
                errorMessage = "Invalid user ID.";
            }
        }
        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                errorMessage = "Invalid input.";
                return Page();
            }

            try
            {
                if (user.product_id == null)
                {
                    errorMessage = "Product ID cannot be null";
                    return Page();
                }

                DAL dal = new DAL();
                int i = dal.UpdateUser(user, configuration);
                if (i > 0)
                {
                    succesMessage = "User has been updated.";
                    return RedirectToPage("/User/Index");
                }
                else
                {
                    errorMessage = "Error updating user.";
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
