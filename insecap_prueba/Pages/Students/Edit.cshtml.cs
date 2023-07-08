using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Windows.Input;

namespace insecap_prueba.Pages.Students
{
    public class EditModel : PageModel
    {

        public StudentInfo student = new StudentInfo();
        public String errMessage = "";
        public String successMessage = "";

        public void OnGet()
        {

            String id = Request.Query["id"];    

            try
            {
                String connStr = "Data Source=localhost\\SQLEXPRESS;Initial Catalog=mynewdb;Integrated Security=True";

                using (SqlConnection connection = new SqlConnection(connStr))
                {
                    connection.Open();

                    String query = "SELECT * FROM students WHERE id=@id";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                student.id = "" + reader.GetInt32(0);
                                student.rut = reader.GetString(1);
                                student.name = reader.GetString(2);
                                student.birthday = reader.GetString(3);
                                student.gender = reader.GetString(4);
                                student.courses = reader.GetString(5);
                                student.createdAt = reader.GetDateTime(6).ToString();

                            }
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                
                System.Diagnostics.Debug.Write(ex.ToString());
                errMessage = "Err getting on edit: "+ ex.Message;
                throw ex;
            }

        }

        public void OnPost()
        {
            student.id = Request.Form["id"];
            student.rut = Request.Form["rut"];
            student.name = Request.Form["name"];
            student.birthday = Request.Form["birthday"];
            student.gender = Request.Form["gender"];

            try
            {
                String connStr = "Data Source=localhost\\SQLEXPRESS;Initial Catalog=mynewdb;Integrated Security=True";

                using (SqlConnection connection = new SqlConnection(connStr))
                {
                    connection.Open();

                    String query = "UPDATE students " +
                        "SET rut=@rut, name=@name, birthday=@birthday, gender=@gender " +
                        "WHERE id=@id";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@id", student.id);
                        command.Parameters.AddWithValue("@rut", student.rut);
                        command.Parameters.AddWithValue("@name", student.name);
                        command.Parameters.AddWithValue("@birthday", student.birthday);
                        command.Parameters.AddWithValue("@gender", student.gender);

                        command.ExecuteNonQuery();
                    }
                }
            }catch(Exception ex)
            {
                System.Diagnostics.Debug.Write(ex.ToString());
                errMessage = "Err posting on edit: " + ex.Message;
                throw ex;
            }

            Response.Redirect("/Students");
        }
    }
}
