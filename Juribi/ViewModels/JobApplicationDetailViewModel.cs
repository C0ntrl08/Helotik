using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Juribi.Models;
using Juribi.Services;
using Juribi.Validation;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Controls;

namespace Juribi.ViewModels
{
    [QueryProperty(nameof(Id), "id")]
    public sealed partial class JobApplicationDetailViewModel : ObservableObject
    {
        private readonly IJobApplicationRepository _repository;
        private readonly JobApplicationValidator _validator;

        [ObservableProperty]
        private string? _id;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(HasErrors))]
        [NotifyPropertyChangedFor(nameof(HasLink))]
        private JobApplication? _entry;

        [ObservableProperty]
        private bool _isEditing;

        [ObservableProperty]
        private string? _editJobName;

        [ObservableProperty]
        private string? _editCompany;

        [ObservableProperty]
        private string? _editSalary;

        [ObservableProperty]
        private string? _editJobUrl;

        [ObservableProperty]
        private JobApplicationStatus _selectedStatus = JobApplicationStatus.Applied;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(HasEditErrors))]
        private IReadOnlyList<string> _editErrors = new List<string>();

        public bool HasEditErrors => EditErrors.Count > 0;

        /// <summary>Selectable statuses (excludes the <see cref="JobApplicationStatus.Unknown"/> fallback).</summary>
        public IReadOnlyList<JobApplicationStatus> Statuses { get; } =
            Enum.GetValues<JobApplicationStatus>()
                .Where(status => status != JobApplicationStatus.Unknown)
                .ToList();

        public bool HasErrors => Entry is { IsValid: false };

        public bool HasLink => !string.IsNullOrWhiteSpace(Entry?.JobUrl);

        public JobApplicationDetailViewModel(IJobApplicationRepository repository, JobApplicationValidator validator)
        {
            _repository = repository;
            _validator = validator;
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

        [RelayCommand]
        private static Task GoBackAsync()
            => Shell.Current.GoToAsync("..");

        [RelayCommand]
        private void Edit()
        {
            if (Entry is null)
                return;

            EditJobName = Entry.JobName;
            EditCompany = Entry.Company;
            EditSalary = Entry.Salary;
            EditJobUrl = Entry.JobUrl;
            SelectedStatus = Entry.Status == JobApplicationStatus.Unknown
                ? JobApplicationStatus.Applied
                : Entry.Status;

            EditErrors = new List<string>();
            IsEditing = true;
        }

        [RelayCommand]
        private void Cancel()
        {
            EditErrors = new List<string>();
            IsEditing = false;
        }

        [RelayCommand]
        private async Task SaveAsync()
        {
            if (Entry is null)
                return;

            var record = new JobApplicationRecord
            {
                Id = Entry.Id,
                JobName = EditJobName,
                Company = EditCompany,
                Salary = EditSalary,
                Status = SelectedStatus.ToString(),
                JobUrl = EditJobUrl
            };

            var validated = _validator.Validate(record);
            if (!validated.IsValid)
            {
                EditErrors = validated.Errors;
                return;
            }

            await _repository.UpdateAsync(record);
            Entry = await _repository.GetByIdAsync(record.Id);
            EditErrors = new List<string>();
            IsEditing = false;
        }
    }
}
