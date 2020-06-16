﻿using CourierTrackingService.Model;
using CourierTrackingService.Service;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.Data;
using System.Threading;

namespace CourierTrackingService
{
    class Program
    {
        public static int delaytime = 0;

        static void Main(string[] args)
        {
            IConfiguration config = new ConfigurationBuilder().AddJsonFile("appsettings.json", true, true).Build();
            delaytime = Convert.ToInt32(config.GetSection("MySettings").GetSection("IntervalInMinutes").Value);

            Thread _Individualprocessthread = new Thread(new ThreadStart(InvokeMethod));
            _Individualprocessthread.Start();
        }
        public static void InvokeMethod()
        {
            while (true)
            {
                GetdataFromMySQL();
                Thread.Sleep(delaytime);
            }
        }
        public static void GetdataFromMySQL()
        {
            int ID = 0;
            int TenantId = 0;
            string InvoiceNo = string.Empty;
            string AWBNo = string.Empty;
            string apiResponse = string.Empty;

            CouriertrackResponce couriertrackResponce = new CouriertrackResponce();


            MySqlConnection con = null;
            try
            {
                DataTable dt = new DataTable();

                IConfiguration config = new ConfigurationBuilder().AddJsonFile("appsettings.json", true, true).Build();
                var constr = config.GetSection("ConnectionStrings").GetSection("HomeShop").Value;
                string ClientAPIURL = config.GetSection("MySettings").GetSection("ClientAPIURL").Value;


                con = new MySqlConnection(constr);
                MySqlCommand cmd = new MySqlCommand("SP_PHYGetCourierTrackingDetails", con)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };
                cmd.Connection.Open();
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                da.Fill(dt);
                cmd.Connection.Close();
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow dr = dt.Rows[i];
                        ID = Convert.ToInt32(dr["ID"]);
                        TenantId = Convert.ToInt32(dr["TenantId"]);
                        InvoiceNo = Convert.ToString(dr["InvoiceNo"]);
                        AWBNo= Convert.ToString(dr["AWBNo"]);


                        CouriertrackRequest couriertrack = new CouriertrackRequest()
                        {
                            awb_no = AWBNo
                        };


                        string apiReq = JsonConvert.SerializeObject(couriertrack);
                        apiResponse = CommonService.SendApiRequest(ClientAPIURL + "/api/ShoppingBag/GetTracking", apiReq);
                        couriertrackResponce = JsonConvert.DeserializeObject<CouriertrackResponce>(apiResponse);

                        if (apiResponse == "")
                        {
                            UpdateResponse(ID, TenantId, InvoiceNo);
                        }
                        else
                        {
                            //logged to Elastic
                        }
                    }

                   
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }
                GC.Collect();
            }
        }

        public static void UpdateResponse(int ID ,int TenantId,string InvoiceNo)
        {

            try
            {
                DataTable dt = new DataTable();
                IConfiguration config = new ConfigurationBuilder().AddJsonFile("appsettings.json", true, true).Build();
                var constr = config.GetSection("ConnectionStrings").GetSection("HomeShop").Value;
                MySqlConnection con = new MySqlConnection(constr);
                MySqlCommand cmd = new MySqlCommand("SP_PHYUpdateCourierStatus", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@_id", ID);
                cmd.Parameters.AddWithValue("@_tenantId", TenantId);
                cmd.Parameters.AddWithValue("@_invoiceNo", InvoiceNo);
                cmd.Connection.Open();
                cmd.ExecuteNonQuery();
                cmd.Connection.Close();
            }
            catch
            {

            }
            finally
            {
                GC.Collect();
            }

        }


    }
}
