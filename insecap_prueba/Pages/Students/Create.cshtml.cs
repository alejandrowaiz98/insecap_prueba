using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.Common;
using System.Data.SqlClient;

namespace insecap_prueba.Pages.Students
{
    public class CreateModel : PageModel
    {

        public StudentInfo student = new StudentInfo();
        public String errMessage = "";
        public String successMessage = "";
        public void OnGet()
        {
           
            
        }

        public void OnPost()
        {
            student.rut = Request.Form["rut"];
            student.name = Request.Form["name"];
            student.birthday = Request.Form["birthday"];
            student.gender = Request.Form["gender"];
            student.courses = "";

            try
            {
                String connStr = "Data Source=localhost\\SQLEXPRESS;Initial Catalog=mynewdb;Integrated Security=True";

                using (SqlConnection connection = new SqlConnection(connStr))
                {
                    connection.Open();

                    String query = "INSERT INTO students " +
                                   "(rut, name, birthday, gender, courses) VALUES" +
                                   "(@rut, @name, @birthday, @gender, @courses);";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@rut", student.rut);
                        command.Parameters.AddWithValue("@name", student.name);
                        command.Parameters.AddWithValue("@birthday", student.birthday);
                        command.Parameters.AddWithValue("@gender", student.gender);
                        command.Parameters.AddWithValue("@courses", student.courses);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Write(ex.ToString());
                Console.WriteLine(ex);
            }

            student.rut = ""; student.name = ""; student.birthday = ""; student.gender = "";

            successMessage = "Estudiante creado con exito!";

            Response.Redirect("/Students");

        }
    }
}
