namespace Hms.UI.ViewModels
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Runtime.CompilerServices;

    using Hms.UI.Annotations;
    using Hms.UI.Infrastructure.Providers;

    public class CreateProfileViewModel : INotifyPropertyChanged, INotifyDataErrorInfo
    {
        public IBuildingsSuggestionProvider BuildingsSuggestionProvider { get; }

        private readonly Dictionary<string, List<string>> errors = new Dictionary<string, List<string>>();

        private GeoObject city;

        private GeoObject street;

        private GeoObject building;

        public CreateProfileViewModel(IBuildingsSuggestionProvider buildingsProvider)
        {
            this.BuildingsSuggestionProvider = buildingsProvider;
        }

        public GeoObject City
        {
            get
            {
                return this.city;
            }

            set
            {
                string key = nameof(this.City);

                if (value != null)
                {
                    this.city = value;

                    List<string> propertyErrors;

                    if (this.errors.TryGetValue(key, out propertyErrors))
                    {
                        propertyErrors.Clear();    
                    }

                    this.BuildingsSuggestionProvider.StreetsSuggestionProvider.CitiesSuggestionProvider.SelectedCity = value;

                    this.OnPropertyChanged();
                    this.OnRaiseErrorsChanged();
                }
                else
                {

                    if (!this.errors.ContainsKey(key))
                    {
                        this.errors.Add(key, new List<string>());
                    }

                    this.errors[key].Add("Bla-bla");

                    this.OnRaiseErrorsChanged();
                }
            }
        }

        public GeoObject Street
        {
            get
            {
                return this.street;
            }

            set
            {
                string key = nameof(this.Street);

                if (value != null)
                {
                    this.street = value;

                    List<string> propertyErrors;

                    if (this.errors.TryGetValue(key, out propertyErrors))
                    {
                        propertyErrors.Clear();
                    }

                    this.BuildingsSuggestionProvider.StreetsSuggestionProvider.SelectedStreet = value;

                    this.OnPropertyChanged();
                    this.OnRaiseErrorsChanged();
                }
                else
                {
                    if (!this.errors.ContainsKey(key))
                    {
                        this.errors.Add(key, new List<string>());
                    }

                    this.errors[key].Add("Bla-bla");

                    this.OnRaiseErrorsChanged();
                }
            }
        }

        public GeoObject Building
        {
            get
            {
                return this.building;
            }

            set
            {
                string key = nameof(this.Building);

                if (value != null)
                {
                    this.building = value;

                    List<string> propertyErrors;

                    if (this.errors.TryGetValue(key, out propertyErrors))
                    {
                        propertyErrors.Clear();
                    }

                    this.BuildingsSuggestionProvider.SelectedBuilding = value;

                    this.OnPropertyChanged();
                    this.OnRaiseErrorsChanged();
                }
                else
                {
                    if (!this.errors.ContainsKey(key))
                    {
                        this.errors.Add(key, new List<string>());
                    }

                    this.errors[key].Add("Bla-bla");

                    this.OnRaiseErrorsChanged();
                }
            }
        }

        public IEnumerable GetErrors(string propertyName)
        {
            return this.errors.FirstOrDefault(error => error.Key == propertyName).Value;
        }

        public bool HasErrors => this.errors.Values.FirstOrDefault(l => l.Count > 0) != null;

        public event PropertyChangedEventHandler PropertyChanged;

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected virtual void OnRaiseErrorsChanged([CallerMemberName] string propertyName = null)
        {
            this.ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }
    }
}
