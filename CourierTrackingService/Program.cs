﻿using CourierTrackingService.Model;
using CourierTrackingService.Service;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;

namespace CourierTrackingService
{
    class Program
    {
        public static int delaytime = 0;
        private static List<WebBotHSMSetting> _hsmProgramcode;
        private static string _maxHSMURL;
        private static string _maxWeBotHSM;
        private static string _from;
        private static string _ttl;
        private static string _type;
        private static string _policy;
        private static string _code;
        private static string _xauthtoken;

        static void Main(string[] args)
        {
            IConfiguration config = new ConfigurationBuilder().AddJsonFile("appsettings.json", true, true).Build();
            delaytime = Convert.ToInt32(config.GetSection("MySettings").GetSection("IntervalInMinutes").Value);

            _maxHSMURL = config.GetSection("MySettings").GetSection("MaxHSMURL").Value;
            _maxWeBotHSM = config.GetSection("MySettings").GetSection("MaxWeBotHSM").Value;

            _from = config.GetSection("WebBotHSMSParameters:from").Value;
            _ttl = config.GetSection("WebBotHSMSParameters:ttl").Value;
            _type = config.GetSection("WebBotHSMSParameters:type").Value;
            _policy = config.GetSection("WebBotHSMSParameters:policy").Value;
            _code = config.GetSection("WebBotHSMSParameters:code").Value;
            _xauthtoken = config.GetSection("WebBotHSMSParameters:x-auth-token").Value;

            _hsmProgramcode = new List<WebBotHSMSetting>();
            var valuesSection = config.GetSection("MySettings").GetSection("WebBotHSMSParameters:HSMProgramcode");
            foreach (IConfigurationSection section in valuesSection.GetChildren())
            {
                WebBotHSMSetting webBotHSMSetting = new WebBotHSMSetting
                {
                    Programcode = config.GetSection("Programcode").Value,
                    bot = config.GetSection("bot").Value,
                    @namespace = config.GetSection("namespace").Value
                };
                _hsmProgramcode.Add(webBotHSMSetting);
            }

            Thread _Individualprocessthread = new Thread(new ThreadStart(InvokeMethod));
            _Individualprocessthread.Start();
        }
        public static void InvokeMethod()
        {
            while (true)
            {
                GetConnectionStrings();
                Thread.Sleep(delaytime);
            }
        }
        /// <summary>
        /// GetConnectionStrings
        /// </summary>
        public static void GetConnectionStrings()
        {
            string ServerName = string.Empty;
            string ServerCredentailsUsername = string.Empty;
            string ServerCredentailsPassword = string.Empty;
            string DBConnection = string.Empty;

            MySqlConnection con = null;
            try
            {
                DataTable dt = new DataTable();
                IConfiguration config = new ConfigurationBuilder().AddJsonFile("appsettings.json", true, true).Build();
                var constr = config.GetSection("ConnectionStrings").GetSection("HomeShop").Value;
                con = new MySqlConnection(constr);
                MySqlCommand cmd = new MySqlCommand("SP_HSGetAllConnectionstrings", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Connection.Open();
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                da.Fill(dt);
                cmd.Connection.Close();

                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow dr = dt.Rows[i];
                        ServerName = Convert.ToString(dr["ServerName"]);
                        ServerCredentailsUsername = Convert.ToString(dr["ServerCredentailsUsername"]);
                        ServerCredentailsPassword = Convert.ToString(dr["ServerCredentailsPassword"]);
                        DBConnection = Convert.ToString(dr["DBConnection"]);

                        string ConString = "Data Source = " + ServerName + " ; port = " + 3306 + "; Initial Catalog = " + DBConnection + " ; User Id = " + ServerCredentailsUsername + "; password = " + ServerCredentailsPassword + "";
                        GetdataFromMySQL(ConString);
                    }
                }
            }
            catch (Exception Ex)
            {

                throw Ex;
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

        /// <summary>
        /// GetdataFromMySQL
        /// </summary>
        /// <param name="ConString"></param>
        public static void GetdataFromMySQL(string ConString)
        {
            int ID = 0;
            int TenantId = 0;
            string InvoiceNo = string.Empty;
            string AWBNo = string.Empty;
            string apiResponse = string.Empty;
            string StoreCode = string.Empty;
            string ProgramCode = string.Empty;

            CouriertrackResponce couriertrackResponce = new CouriertrackResponce();


            MySqlConnection con = null;
            try
            {
                DataTable dt = new DataTable();

                IConfiguration config = new ConfigurationBuilder().AddJsonFile("appsettings.json", true, true).Build();
             
                string ClientAPIURL = config.GetSection("MySettings").GetSection("ClientAPIURL").Value;


                con = new MySqlConnection(ConString);
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
                        try
                        {
                            DataRow dr = dt.Rows[i];
                            ID = Convert.ToInt32(dr["ID"]);
                            TenantId = Convert.ToInt32(dr["TenantId"]);
                            InvoiceNo = Convert.ToString(dr["InvoiceNo"]);
                            AWBNo = Convert.ToString(dr["AWBNo"]);
                            StoreCode = Convert.ToString(dr["StoreCode"]);
                            ProgramCode = Convert.ToString(dr["ProgramCode"]);

                            CouriertrackRequest couriertrack = new CouriertrackRequest()
                            {
                                awb_no = AWBNo
                            };

                            WebBotContentRequest webBotcontentRequest = new WebBotContentRequest
                            {
                                MaxWebBotHSMURL = _maxHSMURL + _maxWeBotHSM,
                                ProgramCode = ProgramCode
                            };

                            WebBotHSMSetting webBotHSMSetting = new WebBotHSMSetting();
                            webBotHSMSetting = _hsmProgramcode.Find(x => x.Programcode.Equals(ProgramCode.ToLower()));

                            if (webBotHSMSetting != null)
                            {
                                webBotcontentRequest.webBotHSMSetting = new WebBotHSMSetting();
                                if (!string.IsNullOrEmpty(webBotHSMSetting.Programcode))
                                {
                                    webBotcontentRequest.MaxWebBotHSMURL = webBotcontentRequest.MaxWebBotHSMURL + "?bot=" + webBotHSMSetting.bot;
                                    webBotcontentRequest.webBotHSMSetting = webBotHSMSetting;
                                    MaxWebBotHSMRequest maxreq = new MaxWebBotHSMRequest();
                                    Hsm hsmReq = new Hsm()
                                    {
                                        @namespace = webBotHSMSetting.@namespace,
                                        language = new Language() { policy = _policy, code = _code }
                                    };
                                    Body body = new Body()
                                    {
                                        // from = webBotcontentRequest.WABANo,// _from,
                                        ttl = Convert.ToInt32(_ttl),
                                        type = _type,
                                        hsm = hsmReq
                                    };
                                    maxreq.body = body;

                                    webBotcontentRequest.MaxHSMRequest = maxreq;
                                }
                            }


                            string apiReq = JsonConvert.SerializeObject(couriertrack);
                            apiResponse = CommonService.SendApiRequest(ClientAPIURL + "/api/ShoppingBag/GetTracking", apiReq);
                            couriertrackResponce = JsonConvert.DeserializeObject<CouriertrackResponce>(apiResponse);
                            if (couriertrackResponce.statusCode == "200" || couriertrackResponce.statusCode == "202")
                            {
                                if (couriertrackResponce.data.tracking_data.shipment_track != null)
                                    if (couriertrackResponce.data.tracking_data.shipment_track[0].current_status != null)
                                    { 
                                        UpdateResponse(ID, TenantId, InvoiceNo, couriertrackResponce.data.tracking_data.shipment_track[0].current_status, ConString);

                                        bool IsSend = UpdateCourierStatus(ID, TenantId, InvoiceNo, couriertrackResponce.data.tracking_data.shipment_track[0].current_status, ConString);

                                        if(IsSend)
                                        {
                                            CommonService.SmsWhatsUpDataSend(TenantId, 0, ProgramCode, ID, ClientAPIURL, couriertrackResponce.data.tracking_data.shipment_track[0].current_status, ConString, webBotcontentRequest, _xauthtoken);
                                        }
                                    }
                            }
                            else
                            {
                                ExLogger(ID, InvoiceNo, Convert.ToString(DateTime.Now), StoreCode, couriertrackResponce.statusCode + " : " + couriertrackResponce.data.tracking_data.error, apiResponse, ConString);
                            }
                        }
                        catch (Exception eX)
                        {

                        }
                    }
                }
            }
            catch (Exception ex)
            {

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

        /// <summary>
        /// UpdateResponse
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="TenantId"></param>
        /// <param name="InvoiceNo"></param>
        /// <param name="CourierStatus"></param>
        /// <param name="ConString"></param>
        public static void UpdateResponse(int ID, int TenantId, string InvoiceNo, string CourierStatus, string ConString)
        {
            MySqlConnection con = null;
            try
            {
                DataTable dt = new DataTable();

                con = new MySqlConnection(ConString);
                MySqlCommand cmd = new MySqlCommand("SP_PHYUpdateCourierStatus", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@_id", ID);
                cmd.Parameters.AddWithValue("@_tenantId", TenantId);
                cmd.Parameters.AddWithValue("@_invoiceNo", InvoiceNo);
                cmd.Parameters.AddWithValue("@_courierStatus", CourierStatus);
                cmd.Connection.Open();
                cmd.ExecuteNonQuery();
                cmd.Connection.Close();
            }
            catch
            {

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


        public static bool UpdateCourierStatus(int ID, int TenantId, string InvoiceNo, string CourierStatus, string ConString)
        {
            MySqlConnection con = null;
            bool IsSend = false;
            try
            {
                DataTable dt = new DataTable();

                con = new MySqlConnection(ConString);
                MySqlCommand cmd = new MySqlCommand("SP_PHYUpdateCourierStatusForWhatsupSMS", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@_id", ID);
                cmd.Parameters.AddWithValue("@_tenantId", TenantId);
                cmd.Parameters.AddWithValue("@_invoiceNo", InvoiceNo);
                cmd.Parameters.AddWithValue("@_courierStatus", CourierStatus);
                cmd.Connection.Open();
                IsSend = Convert.ToBoolean(cmd.ExecuteScalar());
                cmd.Connection.Close();
            }
            catch
            {

            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }
                GC.Collect();
            }

            return IsSend;

        }


        /// <summary>
        /// ExLogger
        /// </summary>
        /// <param name="TransactionID"></param>
        /// <param name="BillNo"></param>
        /// <param name="BillDate"></param>
        /// <param name="StoreCode"></param>
        /// <param name="ErrorMessage"></param>
        /// <param name="ErrorDiscription"></param>
        /// <param name="ConString"></param>
        public static void ExLogger(int TransactionID, string BillNo, string BillDate, string StoreCode, string ErrorMessage, string ErrorDiscription, string ConString)
        {
            MySqlConnection con = null;
            try
            {
                DataTable dt = new DataTable();
                IConfiguration config = new ConfigurationBuilder().AddJsonFile("appsettings.json", true, true).Build();
                var constr = config.GetSection("ConnectionStrings").GetSection("HomeShop").Value;
                con = new MySqlConnection(ConString);
                MySqlCommand cmd = new MySqlCommand("SP_PHYInsertErrorLog", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@_transactionID", TransactionID);
                cmd.Parameters.AddWithValue("@_billNo", BillNo);
                cmd.Parameters.AddWithValue("@_billDate", BillDate);
                cmd.Parameters.AddWithValue("@_storeCode", StoreCode);
                cmd.Parameters.AddWithValue("@_errorMessage", ErrorMessage);
                cmd.Parameters.AddWithValue("@_errorDiscription", ErrorDiscription);
                cmd.Parameters.AddWithValue("@_repeatCount", 0);
                cmd.Parameters.AddWithValue("@_functionName", "Payment Status");
                cmd.Connection.Open();
                cmd.ExecuteNonQuery();
                cmd.Connection.Close();
            }
            catch (Exception ex)
            {
                //write code for genral exception
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
    }
}
