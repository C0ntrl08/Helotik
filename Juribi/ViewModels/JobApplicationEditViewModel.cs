using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Juribi.Models;
using Juribi.Services;
using Juribi.Validation;
using Microsoft.Maui.Controls;

namespace Juribi.ViewModels
{
    public sealed partial class JobApplicationEditViewModel : ObservableObject
    {
        private readonly IJobApplicationRepository _repository;
        private readonly JobApplicationValidator _validator;

        [ObservableProperty]
        private string? _jobName;

        [ObservableProperty]
        private string? _company;

        [ObservableProperty]
        private string? _salary;

        [ObservableProperty]
        private string? _jobUrl;

        [ObservableProperty]
        private JobApplicationStatus _selectedStatus = JobApplicationStatus.Applied;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(HasErrors))]
        private IReadOnlyList<string> _errors = new List<string>();

        public bool HasErrors => Errors.Count > 0;

        /// <summary>Selectable statuses (excludes the <see cref="JobApplicationStatus.Unknown"/> fallback).</summary>
        public IReadOnlyList<JobApplicationStatus> Statuses { get; } =
            Enum.GetValues<JobApplicationStatus>()
                .Where(status => status != JobApplicationStatus.Unknown)
                .ToList();

        public JobApplicationEditViewModel(IJobApplicationRepository repository, JobApplicationValidator validator)
        {
            _repository = repository;
            _validator = validator;
        }

        [RelayCommand]
        private async Task SaveAsync()
        {
            var record = new JobApplicationRecord
            {
                JobName = JobName,
                Company = Company,
                Salary = Salary,
                Status = SelectedStatus.ToString(),
                JobUrl = JobUrl
            };

            var validated = _validator.Validate(record);
            if (!validated.IsValid)
            {
                Errors = validated.Errors;
                return;
            }

            Errors = new List<string>();
            await _repository.AddAsync(record);
            await Shell.Current.GoToAsync("..");
        }
    }
}
