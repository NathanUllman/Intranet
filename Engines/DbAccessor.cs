using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using IntranetApplication.Models;
using IntranetApplication.Models.DashboardItem;
using Microsoft.AspNetCore.Mvc;

namespace IntranetApplication.Engines
{
    public class DbAccessor
    {
        private string CONNECTION_STRING =
            "Server = PLXLicensingDB ; Database = IntranetDB; User Id=test; Password = Penlink123;";

        private SqlConnection _conn { get; }

        public DbAccessor()
        {
            _conn = new SqlConnection(CONNECTION_STRING);
        }

        public List<DashboardItem> GetActiveDashboardItems()
        {
            List<DashboardItem> result = new List<DashboardItem>();
            SqlCommand cmd = new SqlCommand();
            SqlDataReader reader;

            using (_conn)
            {              
                cmd.CommandText = "exec GetActiveDashboardItems";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = _conn;

                _conn.Open();
                reader = cmd.ExecuteReader();
                
                while (reader.Read())
                {
                    try
                    {
                        result.Add(new DashboardItem().Create(reader));
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Data);
                    }
                }
                _conn.Close();
            }
            return result;
        }

        public List<Dashboard> GetActiveDashboards()
        {
            List<Dashboard> result = new List<Dashboard>();
            SqlCommand cmd = new SqlCommand();
            SqlDataReader reader;

            using (_conn)
            {
                cmd.CommandText = "exec GetActiveDashboards";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = _conn;

                _conn.Open();
                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    try
                    {
                        result.Add(new Dashboard().Create(reader));
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Data);
                    }
                }
                _conn.Close();
            }
            return result;
        }
        public DashboardItem GetDashItem(string id)
        {
            SqlDataReader reader;
            DashboardItem result = new DashboardItem();

            using (_conn)
            {
                SqlCommand cmd = new SqlCommand("dbo.GetDashItem", _conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@DashboardItemID", SqlDbType.Int).Value = id;

                _conn.Open();
                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    try
                    {
                        result = new DashboardItem().Create(reader);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Data);
                    }
                }
                _conn.Close();
            }
            return result;
        }
        public Dashboard GetDashboard(string id)
        {
            SqlDataReader reader;
            Dashboard result = new Dashboard();

            using (_conn)
            {
                SqlCommand cmd = new SqlCommand("dbo.GetDashboard", _conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@DashboardID", SqlDbType.Int).Value = id;

                _conn.Open();
                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    try
                    {
                        result = new Dashboard().Create(reader);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Data);
                    }
                }
                _conn.Close();
            }
            return result;
        }



        public void InsertDashboard(Dashboard newDashboard)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("dbo.InsertDashboard", _conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@SortOrder", SqlDbType.Int).Value = newDashboard.SortOrder;
                cmd.Parameters.Add("@DashboardTitle", SqlDbType.VarChar).Value = newDashboard.DashboardTitle;
                _conn.Open();
                cmd.ExecuteNonQuery();
                _conn.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Data);
            }
           
        }

        public void UpdateDashboard(Dashboard dashboardToUpdate)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("dbo.UpdateDashboard", _conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@DashboardID", SqlDbType.Int).Value = dashboardToUpdate.DashboardID;
                cmd.Parameters.Add("@DashboardStatusID", SqlDbType.Int).Value = dashboardToUpdate.DashboardStatusID;
                cmd.Parameters.Add("@SortOrder", SqlDbType.Int).Value = dashboardToUpdate.SortOrder;
                cmd.Parameters.Add("@DashboardTitle", SqlDbType.VarChar).Value = dashboardToUpdate.DashboardTitle;
                _conn.Open();
                cmd.ExecuteNonQuery();
                _conn.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Data);
            }
        }
        // InsertDashItem can handle multiple types
        public void InsertDashItem(DashItemUpload newItem)
        {
            try
            {

                SqlCommand cmd = new SqlCommand("dbo.InsertDashItemUpload", _conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@DashboardID", SqlDbType.Int).Value = newItem.DashboardID;
                cmd.Parameters.Add("@Title", SqlDbType.VarChar).Value = newItem.Title;
                cmd.Parameters.Add("@StartDateTime", SqlDbType.DateTime).Value = newItem.StartDateTime;
                cmd.Parameters.Add("@EndDateTime", SqlDbType.DateTime).Value = newItem.EndDateTime;
                cmd.Parameters.Add("@SortOrder", SqlDbType.Int).Value = newItem.SortOrder;

                cmd.Parameters.Add("@ImageURI", SqlDbType.VarChar).Value = newItem.ImageURI;
                _conn.Open();
                cmd.ExecuteNonQuery();
                _conn.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Data);
            }

        }

        public void InsertDashItem(DashItemScrap newItem)
        {
            try
            {

                SqlCommand cmd = new SqlCommand("dbo.InsertDashItemScrap", _conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@DashboardID", SqlDbType.Int).Value = newItem.DashboardID;
                cmd.Parameters.Add("@Title", SqlDbType.VarChar).Value = newItem.Title;
                cmd.Parameters.Add("@StartDateTime", SqlDbType.DateTime).Value = newItem.StartDateTime;
                cmd.Parameters.Add("@EndDateTime", SqlDbType.DateTime).Value = newItem.EndDateTime;
                cmd.Parameters.Add("@SortOrder", SqlDbType.Int).Value = newItem.SortOrder;

                cmd.Parameters.Add("@SourceURL", SqlDbType.VarChar).Value = newItem.SourceURL;
                cmd.Parameters.Add("@LogonUser", SqlDbType.VarChar).Value = newItem.LogonUser;
                cmd.Parameters.Add("@LogonPwd", SqlDbType.VarChar).Value = newItem.LogonPwd;
                cmd.Parameters.Add("@ClickThruUrl", SqlDbType.VarChar).Value = newItem.ClickThruURL;
                cmd.Parameters.Add("@CssSelector", SqlDbType.VarChar).Value = newItem.CssSelector;
                cmd.Parameters.Add("@ImageURI", SqlDbType.VarChar).Value = newItem.ImageURI;

                _conn.Open();
                cmd.ExecuteNonQuery();
                _conn.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Data);
            }
        }
        public void InsertDashItem(DashItemText newItem)
        {
            try
            {

                SqlCommand cmd = new SqlCommand("dbo.InsertDashItemText", _conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@DashboardID", SqlDbType.Int).Value = newItem.DashboardID;
                cmd.Parameters.Add("@Title", SqlDbType.VarChar).Value = newItem.Title;
                cmd.Parameters.Add("@StartDateTime", SqlDbType.DateTime).Value = newItem.StartDateTime;
                cmd.Parameters.Add("@EndDateTime", SqlDbType.DateTime).Value = newItem.EndDateTime;
                cmd.Parameters.Add("@SortOrder", SqlDbType.Int).Value = newItem.SortOrder;

                cmd.Parameters.Add("@DisplayText", SqlDbType.VarChar).Value = newItem.DisplayText;

                _conn.Open();
                cmd.ExecuteNonQuery();
                _conn.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Data);
            }
        }

        public void UpdateDashItem(DashboardItem itemToUpdate)
        {
            try
            {

                SqlCommand cmd = new SqlCommand("dbo.UpdateDashItem", _conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@DashboardItemID", SqlDbType.Int).Value = itemToUpdate.DashboardItemID;
                cmd.Parameters.Add("@DashboardTypeID", SqlDbType.Int).Value = itemToUpdate.DashboardTypeID;
                cmd.Parameters.Add("@DashboardItemStatusID", SqlDbType.Int).Value = itemToUpdate.DashboardItemStatusID;
                cmd.Parameters.Add("@DashboardID", SqlDbType.Int).Value = itemToUpdate.DashboardID;
                cmd.Parameters.Add("@Title", SqlDbType.VarChar).Value = itemToUpdate.Title;
                cmd.Parameters.Add("@StartDateTime", SqlDbType.DateTime).Value = itemToUpdate.StartDateTime;
                cmd.Parameters.Add("@EndDateTime", SqlDbType.DateTime).Value = itemToUpdate.EndDateTime;
                cmd.Parameters.Add("@SortOrder", SqlDbType.Int).Value = itemToUpdate.SortOrder;

                cmd.Parameters.Add("@SourceURL", SqlDbType.VarChar).Value = itemToUpdate.SourceURL ?? "";
                cmd.Parameters.Add("@LogonUser", SqlDbType.VarChar).Value = itemToUpdate.LogonUser ?? "";
                cmd.Parameters.Add("@LogonPwd", SqlDbType.VarChar).Value = itemToUpdate.LogonPwd ?? "";
                cmd.Parameters.Add("@ClickThruUrl", SqlDbType.VarChar).Value = itemToUpdate.ClickThruURL ?? "";
                cmd.Parameters.Add("@CssSelector", SqlDbType.VarChar).Value = itemToUpdate.CssSelector ?? "";
                cmd.Parameters.Add("@ImageURI", SqlDbType.VarChar).Value = itemToUpdate.ImageURI ?? "";

                cmd.Parameters.Add("@DisplayText", SqlDbType.VarChar).Value = itemToUpdate.DisplayText ?? "";


                _conn.Open();
                cmd.ExecuteNonQuery();
                _conn.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Data);
            }
        }

        public void DeleteDashItem(int id)
        {
            try
            {

                SqlCommand cmd = new SqlCommand("dbo.DeleteDashItem", _conn);

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@DashboardItemID", SqlDbType.Int).Value = id;
               
                _conn.Open();
                cmd.ExecuteNonQuery();
                _conn.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Data);
            }
        }

        public void DeleteDashboard(int id)
        {
            try
            {

                SqlCommand cmd = new SqlCommand("dbo.DeleteDashboard", _conn);

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@DashboardID", SqlDbType.Int).Value = id;

                _conn.Open();
                cmd.ExecuteNonQuery();
                _conn.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Data);
            }
        }


    }
}
