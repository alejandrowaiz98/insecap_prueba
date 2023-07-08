using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Data.SqlClient;

namespace insecap_prueba.Pages.Students
{
    public class IndexModel : PageModel
    {

        public List<StudentInfo> studentList = new List<StudentInfo>();

        public void OnGet()
        {

            try
            {
                String strConn = "Data Source=localhost\\SQLEXPRESS;Initial Catalog=mynewdb;Integrated Security=True";

                using SqlConnection connection = new SqlConnection(strConn);

                connection.Open();

                String query = "SELECT * FROM students";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())

                            while (reader.Read()) {
                            StudentInfo studentInfo = new StudentInfo();
                            studentInfo.id = "" + reader.GetInt32(0);
                            studentInfo.rut = "" + reader.GetString(1);
                            studentInfo.name = "" + reader.GetString(2);
                            studentInfo.birthday = "" + reader.GetString(3);
                            studentInfo.gender = "" + reader.GetString(4);
                            studentInfo.courses = "" + reader.GetString(5);
                            studentInfo.createdAt = "" + reader.GetDateTime(6).ToString();
                            
                            studentList.Add(studentInfo);
                            
                        }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }

    public class StudentInfo
    {
        public String id;
        public String rut;
        public String name;
        public String birthday;
        public String gender;
        public String createdAt;
        public String courses;
    }

}
