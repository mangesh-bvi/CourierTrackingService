using System;
using System.Collections.Generic;
using System.Text;

namespace CourierTrackingService.Model
{
    public class CouriertrackResponce
    {
        /// <summary>
        /// statusCode
        /// </summary>
        public string statusCode { get; set; }

        /// <summary>
        /// data
        /// </summary>
        public data data { get; set; }
    }
    public class data
    {
        /// <summary>
        /// tracking_data
        /// </summary>
        public tracking_data tracking_data { get; set; }
    }
    public class tracking_data
    {
        /// <summary>
        /// track_status
        /// </summary>
        public string track_status { get; set; }

        /// <summary>
        /// shipment_status
        /// </summary>
        public string shipment_status { get; set; }

        /// <summary>
        /// shipment_track
        /// </summary>
        public shipment_Track[] shipment_track { get; set; }


        /// <summary>
        /// shipment_track_activities
        /// </summary>
        public shipment_track_activities[] shipment_track_activities { get; set; }

        /// <summary>
        /// track_url
        /// </summary>
        public string track_url { get; set; }


        /// <summary>
        /// error
        /// </summary>
        public string error { get; set; }

        /// <summary>
        /// edd
        /// </summary>
        public string edd { get; set; }
    }

    public class shipment_Track
    {
        /// <summary>
        /// id
        /// </summary>
        public int id { get; set; }

        /// <summary>
        /// awb_code
        /// </summary>
        public string awb_code { get; set; }

        /// <summary>
        /// courier_company_id
        /// </summary>
        public int courier_company_id { get; set; }

        /// <summary>
        /// shipment_id
        /// </summary>
        public string shipment_id { get; set; }


        /// <summary>
        /// order_id
        /// </summary>
        public int order_id { get; set; }


        /// <summary>
        /// pickup_date
        /// </summary>
        public string pickup_date { get; set; }

        /// <summary>
        /// delivered_date
        /// </summary>
        public string delivered_date { get; set; }

        /// <summary>
        /// weight
        /// </summary>
        public decimal weight { get; set; }


        /// <summary>
        /// packages
        /// </summary>
        public int packages { get; set; }

        /// <summary>
        /// current_status
        /// </summary>
        public string current_status { get; set; }


        /// <summary>
        /// delivered_to
        /// </summary>
        public string delivered_to { get; set; }

        /// <summary>
        /// destination
        /// </summary>
        public string destination { get; set; }


        /// <summary>
        /// consignee_name
        /// </summary>
        public string consignee_name { get; set; }


        /// <summary>
        /// origin
        /// </summary>
        public string origin { get; set; }


        /// <summary>
        /// courier_agent_details
        /// </summary>
        public string courier_agent_details { get; set; }
    }

    public class shipment_track_activities
    {
        /// <summary>
        /// date
        /// </summary>
        public string date { get; set; }


        /// <summary>
        /// activity
        /// </summary>
        public string activity { get; set; }

        /// <summary>
        /// location
        /// </summary>
        public string location { get; set; }
    }


}
