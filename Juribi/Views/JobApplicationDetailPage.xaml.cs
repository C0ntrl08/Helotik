using Juribi.ViewModels;

namespace Juribi.Views
{
    public partial class JobApplicationDetailPage : ContentPage
    {
        public JobApplicationDetailPage(JobApplicationDetailViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}
