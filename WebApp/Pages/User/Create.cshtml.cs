using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApp.Model;

namespace WebApp.Pages.User
{
	public class CreateModel : PageModel
    {
        public Users user = new Users();
        public string succesMessage = string.Empty;
        public string errorMessage = string.Empty;

        private readonly IConfiguration configuration;

        public CreateModel(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public void OnGet()
        {
        }
        public void OnPost()
        {
            user.product_name = Request.Form["product_name"];

            if (!int.TryParse(Request.Form["model_year"], out int modelYear))
            {
                errorMessage = "Model year must be a valid number.";
                return;
            }
            user.model_year = modelYear;

            if (!int.TryParse(Request.Form["brand_id"], out int brandId))
            {
                errorMessage = "Brand ID must be a valid number.";
                return;
            }
            user.brand_id = brandId;

            if (decimal.TryParse(Request.Form["list_price"], out decimal listPrice))
            {
                user.list_price = listPrice;
            }
            else
            {
                errorMessage = "List price must be a valid number.";
                return;
            }

            if (string.IsNullOrEmpty(user.product_name) || user.list_price == 0 || user.model_year == 0 || user.brand_id == 0)
            {
                errorMessage = "All fields are required.";
                return;
            }

            try
            {
                DAL dal = new DAL();
                int i = dal.AddUser(user, configuration);

                if (i > 0)
                {
                    succesMessage = "User has been added.";
                    Response.Redirect("/User/Index");
                }
                else
                {
                    errorMessage = "Error adding user.";
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }
        }



    }
}
