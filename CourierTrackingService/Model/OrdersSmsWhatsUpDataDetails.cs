using System;
using System.Collections.Generic;
using System.Text;

namespace CourierTrackingService.Model
{
    public class OrdersSmsWhatsUpDataDetails
    {
        /// <summary>
        /// Order ID
        /// </summary>
        public int OderID { get; set; }

        /// <summary>
        /// Alert Communication via Whtsup
        /// </summary>
        public bool AlertCommunicationviaWhtsup { get; set; }

        /// <summary>
        /// Alert Communication via SMS
        /// </summary>
        public bool AlertCommunicationviaSMS { get; set; }

        /// <summary>
        /// SMS Sender Name
        /// </summary>
        public string SMSSenderName { get; set; }

        /// <summary>
        /// Is Send
        /// </summary>
        public bool IsSend { get; set; }

        /// <summary>
        /// Message Text
        /// </summary>
        public string MessageText { get; set; }

        /// <summary>
        /// Invoice No
        /// </summary>
        public string InvoiceNo { get; set; }

        /// <summary>
        /// Additional Info
        /// </summary>
        public string AdditionalInfo { get; set; }

        /// <summary>
        /// Mobile Number
        /// </summary>
        public string MobileNumber { get; set; }

        /// <summary>
        /// WabaNumber
        /// </summary>
        public string WabaNumber { get; set; }
        /// <summary>
        /// ShoppingBagNo
        /// </summary>
        public string ShoppingBagNo { get; set; }
    }

    public class GetWhatsappMessageDetailsResponse
    {

        /// <summary>
        /// ProgramCode
        /// </summary>
        public string ProgramCode { get; set; }
        /// <summary>
        /// TemplateName
        /// </summary>
        public string TemplateName { get; set; } = "";
        /// <summary>
        /// TemplateNamespace
        /// </summary>
        public string TemplateNamespace { get; set; }
        /// <summary>
        /// TemplateText
        /// </summary>
        public string TemplateText { get; set; }
        /// <summary>
        /// TemplateLanguage
        /// </summary>
        public string TemplateLanguage { get; set; }
        /// <summary>
        /// Remarks
        /// </summary>
        public string Remarks { get; set; }

    }

    public class GetWhatsappMessageDetailsModal
    {

        /// <summary>
        /// ProgramCode
        /// </summary>
        public string ProgramCode { get; set; }

    }

    public class SendFreeTextRequest
    {
        /// <summary>
        /// To
        /// </summary>
        public string To { get; set; }
        /// <summary>
        /// ProgramCode
        /// </summary>
        public string ProgramCode { get; set; }
        /// <summary>
        /// TemplateName
        /// </summary>
        public string TemplateName { get; set; }
        /// <summary>
        /// TemplateNamespace
        /// </summary>
        //public string TemplateNamespace { get; set; }
        /// <summary>
        /// TemplateName
        /// </summary>
        public List<string> AdditionalInfo { get; set; }
    }

    public class ChatSendSMS
    {
        /// <summary>
        /// MobileNumber
        /// </summary>
        public string MobileNumber { get; set; }
        /// <summary>
        /// SenderId
        /// </summary>
        public string SenderId { get; set; }
        /// <summary>
        /// SmsText
        /// </summary>
        public string SmsText { get; set; }
    }

    public class ChatSendSMSResponse
    {
        /// <summary>
        /// Guid
        /// </summary>
        public string Guid { get; set; }
        /// <summary>
        /// SubmitDate
        /// </summary>
        public string SubmitDate { get; set; }
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// ErrorSEQ
        /// </summary>
        public string ErrorSEQ { get; set; }
        /// <summary>
        /// ErrorCODE
        /// </summary>
        public string ErrorCODE { get; set; }
    }
}
