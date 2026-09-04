using System;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Juribi.Models;
using Juribi.Services;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Controls;

namespace Juribi.ViewModels
{
    [QueryProperty(nameof(Id), "id")]
    public sealed partial class JobApplicationDetailViewModel : ObservableObject
    {
        private readonly IJobApplicationRepository _repository;

        [ObservableProperty]
        private string? _id;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(HasErrors))]
        [NotifyPropertyChangedFor(nameof(HasLink))]
        private JobApplication? _entry;

        public bool HasErrors => Entry is { IsValid: false };

        public bool HasLink => !string.IsNullOrWhiteSpace(Entry?.JobUrl);

        public JobApplicationDetailViewModel(IJobApplicationRepository repository)
        {
            _repository = repository;
        }

        partial void OnIdChanged(string? value)
        {
            _ = LoadAsync(value);
        }

        private async Task LoadAsync(string? id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return;

            Entry = await _repository.GetByIdAsync(id);
        }

        [RelayCommand]
        private async Task OpenLinkAsync()
        {
            var url = Entry?.JobUrl;
            if (string.IsNullOrWhiteSpace(url))
                return;

            if (Uri.TryCreate(url, UriKind.Absolute, out var uri))
                await Launcher.OpenAsync(uri);
        }
    }
}
