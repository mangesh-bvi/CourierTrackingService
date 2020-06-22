using System;
using System.Collections.Generic;
using System.Text;

namespace CourierTrackingService.Model
{
    public class CouriertrackResponce
    {
        public string statusCode { get; set; }
        public data data { get; set; }
    }
    public class data
    {
        public tracking_data tracking_data { get; set; }
    }
    public class tracking_data
    {
        public string track_status { get; set; }
        public string shipment_status { get; set; }
        public shipment_Track[] shipment_track { get; set; }
        public shipment_track_activities[] shipment_track_activities { get; set; }
        public string track_url { get; set; }
        public string error { get; set; }
        public string edd { get; set; }
    }

    public class shipment_Track
    {
        public int id { get; set; }
        public string awb_code { get; set; }
        public int courier_company_id { get; set; }
        public string shipment_id { get; set; }
        public int order_id { get; set; }
        public string pickup_date { get; set; }
        public string delivered_date { get; set; }
        public decimal weight { get; set; }
        public int packages { get; set; }
        public string current_status { get; set; }
        public string delivered_to { get; set; }
        public string destination { get; set; }
        public string consignee_name { get; set; }
        public string origin { get; set; }
        public string courier_agent_details { get; set; }
    }

    public class shipment_track_activities
    {
        public string date { get; set; }
        public string activity { get; set; }
        public string location { get; set; }
    }


}
