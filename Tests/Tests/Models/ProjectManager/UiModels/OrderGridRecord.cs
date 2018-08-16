
using System.Collections.Generic;

namespace Tests.Models.ProjectManager.UiModels
{
    public class OrderGridRecord
    {
        public string OrderId { get; set; }

        public string Customer { get; set; }
        
        public string DeliveryDate { get; set; }

        public string Editor { get; set; }

        public string CreationDate { get; set; }

        public override bool Equals(object obj)
        {
           var record = obj as OrderGridRecord;
            bool valid = record != null && OrderId == record.OrderId && Customer == record.Customer
                         && Editor == record.Editor;
                   

            if (Parameters.Parameters.Browser != "MicrosoftEdge")
            {
                valid = valid && DeliveryDate == record.DeliveryDate && CreationDate == record.CreationDate;
            }

            return valid;
        }

        public override int GetHashCode()
        {
            var hashCode = 1132510251;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(OrderId);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Customer);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(DeliveryDate);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Editor);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(CreationDate);
            return hashCode;
        }
    }
}
