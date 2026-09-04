using Juribi.Views;

namespace Juribi
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(JobApplicationDetailPage), typeof(JobApplicationDetailPage));
        }
    }
}
