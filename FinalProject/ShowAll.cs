using Microsoft.Office.Tools.Ribbon;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace FinalProject
{
    public partial class ShowAll
    {
        private void ShowAll_Load(object sender, RibbonUIEventArgs e)
        {

        }

        private void ShowAllButton_Click(object sender, RibbonControlEventArgs e)
        {
            


            
        }



        private void DateTimePicker_Click(object sender, RibbonControlEventArgs e)
        {
            Form1 form = new Form1();
            form.ShowDialog();
            
        }

       

        private void Allin_Click(object sender, RibbonControlEventArgs e)
        {
            HashSet<Microsoft.Office.Interop.MSProject.Resource> uniqueResources 
                = new HashSet<Microsoft.Office.Interop.MSProject.Resource>();
            Dictionary<Microsoft.Office.Interop.MSProject.Task,
                List<Microsoft.Office.Interop.MSProject.Resource>> taskResources 
                = new Dictionary<Microsoft.Office.Interop.MSProject.Task, List<Microsoft.Office.Interop.MSProject.Resource>>();

            DateTime inputDateTime = SelectDate();
            GetAllTasksAndResources(inputDateTime);
            SendTasksToSQLServer();
            SendResourcesToSQLServer();
            SendAssignmentsToSQLServer();
            DeleteTasksFromProject();
            //DeleteResourcesFromProject(); Не понятно, нужно ли удалять ресурс из проекта после архивации
            ClearAll();

            DateTime SelectDate()
            {
                DateTime correctDate = Form1.currentSelection;
                return correctDate;
            }

            void GetAllTasksAndResources(DateTime givenDateTime)
            {
                var pj = Globals.ThisAddIn.Application.ActiveProject;
                HashSet<string> uniqueGuid = new HashSet<string>();
                foreach (Microsoft.Office.Interop.MSProject.Task task in pj.Tasks)
                {
                    if (DateTime.Compare(task.Finish,givenDateTime)<=0)
                    {
                        List<Microsoft.Office.Interop.MSProject.Resource> resourcesForTask = new List<Microsoft.Office.Interop.MSProject.Resource>();
                        foreach (Microsoft.Office.Interop.MSProject.Resource resource in task.Resources)
                        {

                            if (!uniqueGuid.Contains(resource.Guid))
                            {
                                uniqueGuid.Add(resource.Guid);
                                uniqueResources.Add(resource);
                            }

                            resourcesForTask.Add(resource);
                        }
                        taskResources.Add(task, resourcesForTask);
                    }
                }
                uniqueGuid.Clear();

            }

            void SendTasksToSQLServer()
            {
                var tasks = taskResources.Keys;
                string connection = "Server=DESKTOP-DPGMQGD;Database=Task_Final;Trusted_Connection=True";
                using (SqlConnection con = new SqlConnection(connection))
                {
                    con.Open();
                    foreach (var task in tasks)
                    {
                        string query = "INSERT INTO dbo.ArchieveTasks " +
                        "VALUES" +
                        $"('{task.Guid}','{task.Name}','{task.Duration}','{task.Start}'," +
                        $"'{task.Finish}','{task.Predecessors}',{task.OutlineLevel});";
                        SqlCommand cmdMain = new SqlCommand(query, con);
                        cmdMain.ExecuteNonQuery();
                    }
                }
            }

            void SendResourcesToSQLServer()
            {
                string connection = "Server=DESKTOP-DPGMQGD;Database=Task_Final;Trusted_Connection=True";
                using (SqlConnection con = new SqlConnection(connection))
                {
                    con.Open();
                    foreach (var resource in uniqueResources)
                    {
                        string query = "INSERT INTO dbo.ArchieveResources " +
                        "VALUES" +
                        $"('{resource.Guid}','{resource.Name}','{resource.Type}','{resource.Initials}'," +
                        $"'{resource.MaxUnits}');";
                        SqlCommand cmdMain = new SqlCommand(query, con);
                        cmdMain.ExecuteNonQuery();
                    }
                }
            }
            void SendAssignmentsToSQLServer()
            {
                var pj = Globals.ThisAddIn.Application.ActiveProject;
                string connection = "Server=DESKTOP-DPGMQGD;Database=Task_Final;Trusted_Connection=True";
                using (SqlConnection con = new SqlConnection(connection))
                {
                    con.Open();
                    foreach (var taskResource in taskResources)
                    {
                        var task = taskResource.Key;
                        var assignmets = taskResource.Value;
                        foreach(var assignment in assignmets)
                        {
                            string query = "INSERT INTO dbo.ArchieveAssignments " +
                        "VALUES" +
                        $"('{pj.GetServerProjectGuid()}','{task.Guid}','{assignment.Guid}');";
                            SqlCommand cmdMain = new SqlCommand(query, con);
                            cmdMain.ExecuteNonQuery();
                        }
                    }
                }
            }
            void DeleteTasksFromProject()
            {
                int counter = 0;
                var pj = Globals.ThisAddIn.Application.ActiveProject;
                foreach (var task in taskResources.Keys)
                {
                    counter++;
                    var thisTask = pj.Tasks[task.ID];
                    thisTask.Delete();
                }
            }

            void DeleteResourcesFromProject()
            {
                int counter = 0;
                var pj = Globals.ThisAddIn.Application.ActiveProject;
                foreach (var resource in uniqueResources)
                {
                    counter++;
                    var thisResource = pj.Resources[resource.ID];
                    thisResource.Delete();
                }
            }
            void ClearAll()
            {
                uniqueResources.Clear();
                taskResources.Clear();
                System.Windows.Forms.MessageBox.Show("Данные успешно заархивированы.");
            }

        }

        private void Show_All_Click(object sender, RibbonControlEventArgs e)
        {
            var pj = Globals.ThisAddIn.Application.ActiveProject;
            string connection = "Server=DESKTOP-DPGMQGD;Database=Task_Final;Trusted_Connection=True;MultipleActiveResultSets=True";
            using(SqlConnection con = new SqlConnection(connection))
            {
                con.Open();
                string projectId = GetProjectID.Text;
                string queryRes = $"SELECT DISTINCT ArchieveResources.Name, ArchieveResources.Type, ArchieveResources.Initials, ArchieveResources.Max_Units, ArchieveAssignments.Project_Id FROM ArchieveAssignments INNER JOIN ArchieveResources ON ArchieveAssignments.Resource_Id = ArchieveResources.GUID WHERE(ArchieveAssignments.Project_Id LIKE '{projectId}')";
                SqlCommand cmd = new SqlCommand(queryRes, con);
                SqlDataReader readerRes = cmd.ExecuteReader();
                while (readerRes.Read())
                {
                    Microsoft.Office.Interop.MSProject.Resource newRes = pj.Resources.Add(readerRes.GetString(0));
                    newRes.Type = Microsoft.Office.Interop.MSProject.PjResourceTypes.pjResourceTypeWork;//TODO: сделать проверку типа в таблице и вставить нужный тип
                    newRes.MaxUnits = Convert.ToInt32(readerRes.GetString(3));
                    newRes.Initials = readerRes.GetString(2);
                }
                readerRes.Close();
                cmd.Cancel();
                string query = "SELECT * FROM dbo.ArchieveTasks";
                SqlCommand command= new SqlCommand(query,con);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Microsoft.Office.Interop.MSProject.Task newTask=pj.Tasks.Add(reader.GetString(2));
                    int durationInt = Convert.ToInt32(reader.GetString(3))/480;
                    string durationString = durationInt.ToString() + "dys";
                    newTask.Duration=durationString;
                    DateTime start = DateTime.Parse(reader.GetString(4));
                    newTask.Start= start;
                    DateTime finish = DateTime.Parse(reader.GetString(5));
                    newTask.Start = finish;
                    newTask.Predecessors=reader.GetString(6);
                    newTask.OutlineLevel = (short)reader.GetInt32(7);
                   


                    string query1 = $"SELECT ArchieveResources.Name, ArchieveAssignments.Task_Id FROM ArchieveAssignments INNER JOIN ArchieveTasks ON ArchieveAssignments.Task_Id = ArchieveTasks.GUID INNER JOIN ArchieveResources ON ArchieveAssignments.Resource_Id = ArchieveResources.GUID WHERE(ArchieveAssignments.Task_Id LIKE '{reader.GetGuid(1).ToString()}')";
                    SqlCommand command1= new SqlCommand(query1, con);
                    SqlDataReader reader1 = command1.ExecuteReader();
                    StringBuilder thisResources = new StringBuilder();
                    while (reader1.Read())
                    {
                        thisResources.Append($"{reader1.GetString(0)};");
                    }
                    newTask.ResourceNames=thisResources.ToString();
                    
                }
                reader.Close();
                con.Close();
            }
        }

        private void editBox1_TextChanged(object sender, RibbonControlEventArgs e)
        {

        }
    }
}