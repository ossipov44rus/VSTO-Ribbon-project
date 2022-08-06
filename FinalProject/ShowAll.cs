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
            var pj = Globals.ThisAddIn.Application.ActiveProject;
            string connection = "Server=DESKTOP-DPGMQGD;Database=Task_Final;Trusted_Connection=True";
            var dict = new Dictionary<int, string>();
            dict.Add(1, "SELECT * FROM dbo.First_Task");
            dict.Add(2, "SELECT * FROM dbo.Second_Task");
            string value = string.Empty;
            string queryMain = "SELECT * FROM dbo.Main_Task";
            string queryEmpty = string.Empty;
            using (SqlConnection con = new SqlConnection(connection))
            {
                con.Open();
                SqlCommand cmdMain = new SqlCommand(queryMain, con);
                SqlDataReader reader = cmdMain.ExecuteReader();
                List<CustomTask> mainTasks = new List<CustomTask>();
               while (reader.Read())
                {
                    var myNewTask = CustomTaskCreation();
                    mainTasks.Add(myNewTask);
                }
               reader.Close();
                int counter = 1;
                int counter2 = 1;
                foreach (var task in mainTasks)
                {
                    
                    Microsoft.Office.Interop.MSProject.Task newTask = pj.Tasks.Add
                            (task.Name);
                    newTask.OutlineLevel=1;
                    newTask.Duration = task.Duration;
                    newTask.Start=task.Start;
                    newTask.Finish=task.Finish;
                    int key = task.SubTasks;
                    value = dict[key];
                    cmdMain = new SqlCommand(value, con);
                    SqlDataReader reader2 = cmdMain.ExecuteReader();
                    while (reader2.Read())
                    {
                        counter2++;
                        Microsoft.Office.Interop.MSProject.Task newTask2 = pj.Tasks.Add
                           (reader2.GetString(1));
                        newTask2.OutlineLevel = 2;
                        newTask2.Start = reader2.GetValue(3);
                        newTask2.Finish = reader2.GetDateTime(4);
                        //newTask2.Duration = reader2.GetValue(2);
                        if (reader2.GetValue(5).ToString() != string.Empty)
                        {
                            int pred = reader2.GetInt32(5)+counter;
                            newTask2.Predecessors = pred.ToString();
                        }
                    }
                    reader2.Close();
                    counter += counter2;
                }

               CustomTask CustomTaskCreation()
                {
                    CustomTask myNewTask = new CustomTask();
                    myNewTask.Name = reader.GetString(1);
                    myNewTask.Duration = reader.GetString(2);
                    myNewTask.Start = reader.GetDateTime(3);
                    myNewTask.Finish = reader.GetDateTime(4);
                    if (reader.GetValue(5).ToString() != string.Empty)
                    {
                        int pred = reader.GetInt32(4);
                        myNewTask.Predecessors = pred;
                    }
                    if (reader.GetValue(6).ToString() != string.Empty)
                    {
                        int key = reader.GetInt32(6);
                        myNewTask.SubTasks = key;
                    }
                    return myNewTask;
                }


                
            }


        }
    }
}