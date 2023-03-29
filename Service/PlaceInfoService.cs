﻿using EmployeeRegistrationService.Interface;
using EmployeeRegistrationService.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace EmployeeRegistrationService.Service
{
    public class PlaceInfoService : IPlaceInfoService
    {
        //public string sConStr = "Data Source=PEFLBELH3T;Initial Catalog=EmployeesDB;Integrated Security=True";
        public string sConStr = "Server=34.30.132.99,1433;Database=EmployeeDb;User Id=SA;Password=Sumit@mssql8796";
        public int Add(PlaceInfo placeInfo)
        {
            string sQry = "INSERT INTO [EmployeeDetails] ([Name],[Place],[About],[City],[State],[Country]) " +
                "VALUES('" + placeInfo.Name + "','" + placeInfo.Place + "','" + placeInfo.About + "','" + placeInfo.City + "','" +
                placeInfo.State + "','" + placeInfo.Country + "')";
            int retVal = ExecuteCRUDByQuery(sQry);
            return retVal;
        }

        public int AddRange(IEnumerable<PlaceInfo> places)
        {
            string sQry = "INSERT INTO [EmployeeDetails] (Name],[Place],[About],[City],[State],[Country]) VALUES";
            string sVal = "";
            foreach (var place in places)
                sVal += "('" + place.Name + "','" + place.Place + "','" + place.About + "','" + place.City + "','" + place.State + "','" + place.Country + "'),";
            sVal = sVal.TrimEnd(',');
            sQry = sQry + sVal;
            int retVal = ExecuteCRUDByQuery(sQry);
            return retVal;
        }

        public PlaceInfo Find(int id)
        {
            PlaceInfo placeInfo = null;
            string sQry = "SELECT * FROM [EmployeeDetails] WHERE [Id]=" + id;
            DataTable dtPlaceInfo = ExecuteQuery(sQry);
            if (dtPlaceInfo != null)
            {
                DataRow dr = dtPlaceInfo.Rows[0];
                placeInfo = GetPlaceInfoByRow(dr);
            }
            return placeInfo;
        }

        public IEnumerable<PlaceInfo> GetAll()
        {
            List<PlaceInfo> placeInfos = null;
            string sQry = "SELECT * FROM [EmployeeDetails]";
            DataTable dtPlaceInfo = ExecuteQuery(sQry);
            if (dtPlaceInfo != null)
            {
                placeInfos = new List<PlaceInfo>();
                foreach (DataRow dr in dtPlaceInfo.Rows)
                    placeInfos.Add(GetPlaceInfoByRow(dr));
            }
            return placeInfos;
        }

        public int Remove(int id)
        {
            string sQry = "DELETE FROM [EmployeeDetails] WHERE [Id]=" + id;
            int retVal = ExecuteCRUDByQuery(sQry);
            return retVal;
        }

        public int Update(PlaceInfo placeInfo)
        {
            string sQry = "UPDATE [EmployeeDetails] SET [Name]='" + placeInfo.Name + "',[Place]='" + placeInfo.Place + "',[About]='" + placeInfo.About + "',[City]='" + placeInfo.City + "',[State]='" + placeInfo.State + "',[Country]='" + placeInfo.Country + "' WHERE [Id]=" + placeInfo.Id;
            //string sQry = '"UPDATE [PlaceInfo] SET [EmployeeDetails]= [NAME]= "' + placeInfo.Name + ',[Place]='  + placeInfo.Place + '[About]= + placeInfo.About + [City]= + placeInfo.City + [State]= + placeInfo.State + ,[Country]= + placeInfo.Country +  WHERE [Id]= + placeInfo.Id';
            int retVal = ExecuteCRUDByQuery(sQry);
            return retVal;
        }

        private int ExecuteCRUDByQuery(string strSql)
        {
            //string sConStr = "Data Source=sqlpocbyashish.database.windows.net;Initial Catalog=EmployeesDB;Persist Security Info=True;User ID=ashish;Password=Radha@0786";
            SqlConnection conn = null;
            int iR = 0;
            try
            {
                conn = new SqlConnection(sConStr);
                SqlCommand cmd = new SqlCommand(strSql, conn);
                cmd.CommandType = CommandType.Text;
                conn.Open();
                //Execute the command
                iR = cmd.ExecuteNonQuery();
            }
            catch { iR = 0; }
            finally { if (conn.State != 0) conn.Close(); }
            return iR;
        }

        private DataTable ExecuteQuery(string strSql)
        {
            //string sConStr = "Data Source=.\\SQLExpress;Initial Catalog=BillGatesMoney;Integrated Security=True";
            SqlConnection conn = null;
            DataTable dt = null;
            try
            {
                conn = new SqlConnection(sConStr);
                SqlCommand cmd = new SqlCommand(strSql, conn);
                cmd.CommandType = CommandType.Text;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                conn.Open();
                dt = new DataTable();
                //Fill the dataset
                da.Fill(dt);
                if (!(dt.Rows.Count > 0)) dt = null;
            }
            catch { dt = null; }
            finally { if (conn.State != 0) conn.Close(); }
            return dt;
        }

        private PlaceInfo GetPlaceInfoByRow(DataRow dr)
        {
            PlaceInfo placeInfo = new PlaceInfo();
            placeInfo.Id = Convert.ToInt32(dr["Id"]);
            placeInfo.Name = dr["Name"].ToString();
            placeInfo.Place = dr["Place"].ToString();
            placeInfo.About = dr["About"].ToString();
            placeInfo.City = dr["City"].ToString();
            placeInfo.State = dr["State"].ToString();
            placeInfo.Country = dr["Country"].ToString();
            return placeInfo;
        }
    }



}


