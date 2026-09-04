using Juribi.ViewModels;

namespace Juribi
{
    public partial class MainPageView : ContentPage
    {
        private readonly JobApplicationListViewModel _viewModel;

        public MainPageView(JobApplicationListViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = _viewModel = viewModel;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await _viewModel.LoadCommand.ExecuteAsync(null);
        }
    }
}
