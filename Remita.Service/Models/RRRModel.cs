using System;
using System.Collections.Generic;
using System.Text;

namespace Remita.Service.Models
{
    public class RRRRequest
    {
        public string ServiceTypeId { get; set; }
        public string Amount { get; set; }
        public string OrderId { get; set; }
        public string PayerName { get; set; }
        public string PayerEmail { get; set; }
        public string PayerPhone { get; set; }
        public string Description { get; set; }
        public List<CustomFields> CustomFields { get; set; }
        public List<LineItems> LineItems { get; set; }
    }

    public class CustomFields
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public string Type { get; set; }
    }

    public class LineItems
    {
        public string LineItemsId { get; set; }
        public string BeneficiaryName { get; set; }
        public string BeneficiaryAccount { get; set; }
        public string BankCode { get; set; }
        public string BeneficiaryAmount { get; set; }
        public string DeductFeeeFrom { get; set; }
    }

    public class RRRResponse
    {
        public string StatusCode { get; set; }
        public string RRR { get; set; }
        public string Status { get; set; }
    }
}