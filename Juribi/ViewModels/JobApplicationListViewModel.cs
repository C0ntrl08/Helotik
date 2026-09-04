using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Juribi.Models;
using Juribi.Services;
using Juribi.Views;
using Microsoft.Maui.Controls;

namespace Juribi.ViewModels
{
    public sealed partial class JobApplicationListViewModel : ObservableObject
    {
        private readonly IJobApplicationRepository _repository;

        [ObservableProperty]
        private bool _isBusy;

        public ObservableCollection<JobApplication> Entries { get; } = new();

        public JobApplicationListViewModel(IJobApplicationRepository repository)
        {
            _repository = repository;
        }

        [RelayCommand]
        private async Task LoadAsync()
        {
            if (IsBusy)
                return;

            IsBusy = true;
            try
            {
                Entries.Clear();
                var entries = await _repository.GetAllAsync();
                foreach (var entry in entries)
                    Entries.Add(entry);
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        private static async Task OpenDetailAsync(JobApplication? entry)
        {
            if (entry is null)
                return;

            var route = $"{nameof(JobApplicationDetailPage)}?id={entry.Id}";
            await Shell.Current.GoToAsync(route);
        }
    }
}
