using Microsoft.Win32;

namespace Jenkins.Notifier.Services
{
    public class UiService : IUiService
    {
        public string SelectFile()
        {
            var fileDialog = new OpenFileDialog();
            return fileDialog.ShowDialog() != null ? fileDialog.FileName : string.Empty;
        }
    }
}
