
using System.Collections.Generic;

namespace Tests.Models.ProjectManager.UiModels
{
    public class TaskGridRecord
    {
        public string Name { get; set; }

        public string DurationPerWorkpiece { get; set; }

        public string DurationInTotal { get; set; }

        public string Start { get; set; }

        public string End { get; set; }

        public string Machine { get; set; }

        public string CamFile { get; set; }

        public override bool Equals(object obj)
        {
            var record = obj as TaskGridRecord;
            return record != null &&
                   Name == record.Name &&
                   DurationPerWorkpiece == record.DurationPerWorkpiece &&
                   DurationInTotal == record.DurationInTotal &&
                   Start == record.Start &&
                   End == record.End &&
                   Machine == record.Machine &&
                   CamFile == record.CamFile;
        }

        public override int GetHashCode()
        {
            var hashCode = -1170254112;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(DurationPerWorkpiece);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(DurationInTotal);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Start);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(End);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Machine);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(CamFile);
            return hashCode;
        }
    }
}
