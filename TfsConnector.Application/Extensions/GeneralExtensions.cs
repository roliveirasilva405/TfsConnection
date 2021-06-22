using TfsConnector.Application.Model;

namespace TfsConnector.Application.Extensions
{
    public static class GeneralExtensions
    {
        public static void SplitObject(this MergeSheet self)
        {
            var s = self.PathTfsFull.Split('/');
            self.ObjectName = s[s.Length - 1];
            if (!self.ObjectName.Contains(".")) self.ObjectName = "";
            if (self.ObjectName != "")
                self.PathTfs = self.PathTfs.Replace(self.ObjectName, "");
        }
    }
}
