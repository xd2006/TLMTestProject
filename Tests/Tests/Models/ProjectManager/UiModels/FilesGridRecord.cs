using System.Collections.Generic;

namespace Tests.Models.ProjectManager.UiModels
{
    public class FilesGridRecord
    {
        public string Name { get; set; }
        public string Editor { get; set; }
        public string CreationDate { get; set; }

        public override bool Equals(object obj)
        {
            var record = obj as FilesGridRecord;
            var valid = record != null && Name == record.Name && Editor == record.Editor;
            if (Parameters.Parameters.Browser != "MicrosoftEdge")
            {
                valid = valid && CreationDate == record.CreationDate;
            }
            return valid;
        }

        public override int GetHashCode()
        {
            var hashCode = 1132510251;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Editor);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(CreationDate);
            return hashCode;
        }
    }
}