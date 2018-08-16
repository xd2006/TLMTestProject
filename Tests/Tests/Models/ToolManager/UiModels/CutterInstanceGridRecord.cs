
using System.Collections.Generic;

namespace Tests.Models.ToolManager.UiModels
{
    public class CutterInstanceGridRecord
    {
        
        public string Id { get; set; }
        
        public string Location { get; set; }

        public string Quantity { get; set; }
        
        public string Status { get; set; }

        public override bool Equals(object obj)
        {
            var record = obj as CutterInstanceGridRecord;
            return record != null &&
                   Id == record.Id &&
                   Location == record.Location &&
                   Quantity == record.Quantity &&
                   Status == record.Status;
        }

        public override int GetHashCode()
        {
            var hashCode = 486371821;
            hashCode = hashCode * -1521134295 + Id.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Location);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Quantity);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Status);
            return hashCode;
        }
    }
}
