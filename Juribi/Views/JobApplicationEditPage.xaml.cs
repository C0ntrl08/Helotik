using Juribi.ViewModels;

namespace Juribi.Views
{
    public partial class JobApplicationEditPage : ContentPage
    {
        public JobApplicationEditPage(JobApplicationEditViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}
