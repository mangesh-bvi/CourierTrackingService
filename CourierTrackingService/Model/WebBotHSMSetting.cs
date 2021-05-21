using System;
using System.Collections.Generic;
using System.Text;

namespace CourierTrackingService.Model
{
    public class WebBotHSMSetting
    {
        public string Programcode { get; set; }
        public string bot { get; set; }
        public string @namespace { get; set; }
    }

    public class WebBotContentRequest
    {
        public int TenantID { get; set; }
        public string ProgramCode { get; set; }
        public int OptionID { get; set; }
        public int UserID { get; set; }
        public string CustomerName { get; set; }
        public string MobileNo { get; set; }
        public int ShopingBagNo { get; set; }
        public int? OrderID { get; set; }
        public string MakeBellActiveUrl { get; set; }
        public string ClientAPIUrl { get; set; }
        public string WeBBotGenerationLink { get; set; }
        public string MaxWebBotHSMURL { get; set; }
        public string WebBotLink { get; set; }
        public string WABANo { get; set; }

        public MaxWebBotHSMRequest MaxHSMRequest { get; set; }

        public WebBotHSMSetting webBotHSMSetting { get; set; }
    }

    public class MaxWebBotHSMRequest
    {
        public Body body { get; set; }
    }

    public class Body
    {
        public string to { get; set; }
        public string from { get; set; }
        public int ttl { get; set; }
        public string type { get; set; }
        public Hsm hsm { get; set; }
    }

    public class Hsm
    {
        public string @namespace { get; set; }
        public string element_name { get; set; }
        public Language language { get; set; }
        public List<LocalizableParam> localizable_params { get; set; }
    }

    public class Language
    {
        public string policy { get; set; }
        public string code { get; set; }
    }

    public class LocalizableParam
    {
        public string @default { get; set; }
    }

    public class MaxWebBotHSMResponse
    {
        public bool success { get; set; }
        public string message { get; set; }
        public object data { get; set; }
    }
}
