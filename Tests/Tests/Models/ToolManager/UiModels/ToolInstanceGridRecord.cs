
using System.Collections.Generic;

namespace Tests.Models.ToolManager.UiModels
{
    public class ToolInstanceGridRecord
    {
        public string Name { get; set; }

        public int Id { get; set; }

        public int Size { get; set; }

        public int Length { get; set; }

        public string Location { get; set; }

        public string ActualUsageTime { get; set; }

        public string MaximumUsageTime { get; set; }

        public string Status { get; set; }

        public override bool Equals(object obj)
        {
            var record = obj as ToolInstanceGridRecord;
            return record != null &&
                   Name == record.Name &&
                   Id == record.Id &&
                   Size == record.Size &&
                   Length == record.Length &&
                   Location == record.Location &&
                   ActualUsageTime == record.ActualUsageTime &&
                   MaximumUsageTime == record.MaximumUsageTime &&
                   Status == record.Status;
        }

        public override int GetHashCode()
        {
            var hashCode = -216210056;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
            hashCode = hashCode * -1521134295 + Id.GetHashCode();
            hashCode = hashCode * -1521134295 + Size.GetHashCode();
            hashCode = hashCode * -1521134295 + Length.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Location);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(ActualUsageTime);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(MaximumUsageTime);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Status);
            return hashCode;
        }
    }
}
