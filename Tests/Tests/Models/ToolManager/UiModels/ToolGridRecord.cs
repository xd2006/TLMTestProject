
using System.Collections.Generic;

namespace Tests.Models.ToolManager.UiModels
{
    public class ToolGridRecord
    {
        public string Name { get; set; }

        public int Size { get; set; }

        public int? Length { get; set; }

        public int Quantity { get; set; }

        public int QuantityInAssignedMachine { get; set; }

        public override bool Equals(object obj)
        {
            var record = obj as ToolGridRecord;
            return record != null &&
                   Name == record.Name &&
                   Size == record.Size &&
                   EqualityComparer<int?>.Default.Equals(Length, record.Length) &&
                   Quantity == record.Quantity &&
                   QuantityInAssignedMachine == record.QuantityInAssignedMachine;
        }

        public override int GetHashCode()
        {
            var hashCode = -1546502565;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
            hashCode = hashCode * -1521134295 + Size.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<int?>.Default.GetHashCode(Length);
            hashCode = hashCode * -1521134295 + Quantity.GetHashCode();
            hashCode = hashCode * -1521134295 + QuantityInAssignedMachine.GetHashCode();
            return hashCode;
        }
    }
}
