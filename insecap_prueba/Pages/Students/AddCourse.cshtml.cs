using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace insecap_prueba.Pages.Students
{
    public class AddCourseModel : PageModel
    {
        public List<CourseInfo> courseList = new List<CourseInfo>();
        public CourseInfo choosenCourse = new CourseInfo();
        public StudentInfo student = new StudentInfo();
        String ErrMessage = "";

        public void OnGet()
        {

            student.id = Request.Query["id"];

            try
            {
                String strConn = "Data Source=localhost\\SQLEXPRESS;Initial Catalog=mynewdb;Integrated Security=True";

                using SqlConnection connection = new SqlConnection(strConn);

                connection.Open();

                String coursesQuery = "SELECT * FROM courses";

                using (SqlCommand command = new SqlCommand(coursesQuery, connection))
                {
                   
                    using (SqlDataReader reader = command.ExecuteReader())

                        while (reader.Read())
                        {
                            CourseInfo courseInfo = new CourseInfo();
                            courseInfo.id = "" + reader.GetInt32(0);
                            courseInfo.code = "" + reader.GetString(1);
                            courseInfo.name = "" + reader.GetString(2);
                            courseInfo.classroom = "" + reader.GetString(3);
                            courseInfo.teacher = "" + reader.GetString(4);
                            courseInfo.bimester = "" + reader.GetInt32(5);
                            courseInfo.year = "" + reader.GetInt32(6);
 
                            courseList.Add(courseInfo);
                        }
                }

                String studentQuery = "SELECT * FROM students WHERE id=@id";

                using (SqlCommand command = new SqlCommand(studentQuery, connection))
                {
                    command.Parameters.AddWithValue("@id", student.id);
                    using (SqlDataReader reader = command.ExecuteReader())

                        while (reader.Read())
                        {
                            
                            student.id = "" + reader.GetInt32(0);
                            student.rut = "" + reader.GetString(1);
                            student.name = "" + reader.GetString(2);
                            student.birthday = "" + reader.GetString(3);
                            student.gender = "" + reader.GetString(4);
                            student.courses = "" + reader.GetString(5);
                            student.createdAt = "" + reader.GetDateTime(6).ToString();
                            
                        }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void OnPost()
        {           

            try
            {

                choosenCourse.code = Request.Form["code"];
                student.id = Request.Form["id"];    

                String connStr = "Data Source=localhost\\SQLEXPRESS;Initial Catalog=mynewdb;Integrated Security=True";

                using (SqlConnection connection = new SqlConnection(connStr))
                {
                    connection.Open();

                    String query = "UPDATE students " +
                        "SET courses=@courses " +
                        "WHERE id=@id";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@courses", choosenCourse.code);
                        command.Parameters.AddWithValue("@id", student.id);
                        

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Write(ex.ToString());
                ErrMessage = "Err posting on edit: " + ex.Message;
                throw ex;
            }

            Response.Redirect("/Students");

        }
    }

    public class CourseInfo
    {
        public String id;
        public String code;
        public String name;
        public String classroom;
        public String teacher;
        public String bimester;
        public String year;
    }

}
