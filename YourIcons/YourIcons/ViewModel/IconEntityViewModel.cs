using System.Windows;
using System.Windows.Input;
using ModernUI.Presentation;
using ModernUI.Windows.Controls;
using MvvmValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YourIcons.Model;
using YourIcons.View;

namespace YourIcons.ViewModel
{
    public class IconEntityViewModel : ViewModelBase
    {
        private const string DEFAULT_DATA =
            "M 43.8489,37.2997C 43.8489,37.2997 44.2106,34.8791 45.6137,34.8791C 47.0177,34.8791 48.9456,38.1112 48.9456,38.1112C 48.9456,38.1112 44.5479,37.2997 43.8489,37.2997 Z M 51.2373,24.1416C 50.5825,23.013 47.1567,21.7126 45.3788,21.7126C 43.6029,21.7126 40.7975,21.7126 40.7975,21.7126C 40.7975,21.7126 39.3265,19.0499 35.8683,19.0499C 32.4072,19.0499 32.6401,20.5925 32.6401,21.9097L 32.6401,27.2347L 31.0714,28.8905L 23.8278,28.8905C 23.8278,28.8905 21.7871,30.2401 21.7871,33.1576C 21.7871,36.0751 22.6925,46.2416 28.7717,47.1819C 35.9643,48.2962 37.1957,44.9534 37.1957,44.5517C 37.1957,42.8588 37.2382,40.2938 37.2382,40.2938C 37.2382,40.2938 39.3446,44.318 42.5253,44.318C 45.706,44.318 47.5555,46.1452 47.5555,48.0263C 47.5555,49.9095 47.5555,51.5081 47.5555,51.5081C 47.5555,51.5081 47.4372,53.6873 45.5678,53.6873C 43.6956,53.6873 41.5761,53.6873 41.5761,53.6873C 41.5761,53.6873 40.266,52.667 40.266,51.2558C 40.266,49.8451 40.9065,49.4619 41.6544,49.4619C 42.4019,49.4619 43.0163,49.5487 43.0163,49.5487L 43.0163,46.5806C 43.0163,46.5806 36.9923,46.5414 36.9923,51.1539C 36.9923,55.7651 40.1426,56.9501 42.6685,56.9501C 45.1918,56.9501 46.7832,56.9501 46.7832,56.9501C 46.7832,56.9501 54.2129,55.9946 54.2129,41.3163C 54.2129,26.6363 51.8925,25.271 51.2373,24.1416 Z M 29.475,27.2174L 23.3318,27.2107L 31.0285,19.4668L 31.0302,25.7124L 29.475,27.2174 Z ";
        private string m_title;
        private Icon m_default;
        private Icon m_icon = null;
        private string m_name = "Icon";
        private double m_width = 32;
        private double m_height = 32;
        private string m_data = DEFAULT_DATA;
        private bool? m_isValid;
        private string m_keyword = string.Empty;
        private string m_validationErrorsString;
        private bool m_isNew = true;
        private IconEntityWindow m_window;

        public IconEntityViewModel(IconEntityWindow window)
        {
            m_title = "新建图标";
            m_window = window;
            SaveCommand = new RelayCommand(SaveCmdExcute);
            CancelCommand = new RelayCommand(CancelCmdExcute);
            ConfigureValidationRules();
            Validator.ResultChanged += OnValidationResultChanged;
        }

        private void CancelCmdExcute(object obj)
        {
            m_window.Close();
        }

        public IconEntityViewModel(Icon icon, IconEntityWindow window)
            : this(window)
        {
            ConfigureIcon(icon);
            m_icon = icon;
            m_title = "编辑图标";
        }

        private void ConfigureIcon(Icon icon)
        {
            m_isNew = false;
            m_name = icon.Name;
            m_height = icon.Height;
            m_width = icon.Width;
            m_data = icon.FilledData;
            m_keyword = icon.Keyword;
        }

        public ICommand SaveCommand { get; private set; }
        public ICommand CancelCommand { get; private set; }

        public string Title
        {
            get { return m_title; }
            set
            {
                m_title = value;
                RaisePropertyChanged("Title");
            }
        }

        public string Name
        {
            get { return m_name; }
            set
            {
                m_name = value;
                RaisePropertyChanged("Name");
                Validator.ValidateAsync(() => Name);
            }
        }

        public double Width
        {
            get { return m_width; }
            set
            {
                m_width = value;
                RaisePropertyChanged("Width");
                Validator.Validate(() => Width);
            }
        }

        public double Height
        {
            get { return m_height; }
            set
            {
                m_height = value;
                RaisePropertyChanged("Height");
                Validator.Validate(() => Height);
            }
        }

        public string Data
        {
            get { return m_data; }
            set
            {
                m_data = value;
                RaisePropertyChanged("Data");
                Validator.Validate(() => Data);
            }
        }

        public string Keyword
        {
            get { return m_keyword; }
            set
            {
                m_keyword = value;
                RaisePropertyChanged("Keyword");
                Validator.Validate(() => Keyword);
            }
        }

        public string ValidationErrorsString
        {
            get { return m_validationErrorsString; }
            private set
            {
                m_validationErrorsString = value;
                RaisePropertyChanged("ValidationErrorsString");
            }
        }

        public bool? IsValid
        {
            get { return m_isValid; }
            private set
            {
                m_isValid = value;
                RaisePropertyChanged("IsValid");
            }
        }

        private void ConfigureValidationRules()
        {
            Validator.AddRequiredRule(() => Name, "Name is required");

            Validator.AddRequiredRule(() => Width, "Width is required");

            Validator.AddRequiredRule(() => Height, "Height is required");

            Validator.AddRequiredRule(() => Data, "Data is required");
            Validator.AddRule(() => Name,
                              () => RuleResult.Assert(DataRetrieved.Instance.ValidateIconName(Name),
                                  "Name is duplicated"));
            //Validator.AddRule(() => Data,
            //                  () =>
            //                  {
            //                      const string regexPattern =
            //                          @"^[_a-z0-9-]+(\.[_a-z0-9-]+)*@[a-z0-9-]+(\.[a-z0-9-]+)*(\.[a-z]{2,4})$";
            //                      return RuleResult.Assert(Regex.IsMatch(Data, regexPattern),
            //                                               "Email must by a valid email address");
            //                  });
        }

        private void SaveCmdExcute(object obj)
        {
            Validate();
        }

        private void Validate()
        {
            var uiThread = TaskScheduler.FromCurrentSynchronizationContext();

            Validator.ValidateAllAsync().ContinueWith(r => OnValidateAllCompleted(r.Result), uiThread);
        }

        private void OnValidateAllCompleted(ValidationResult validationResult)
        {
            UpdateValidationSummary(validationResult);
            if (validationResult.IsValid)
            {
                var icon = new Icon()
                {
                    Name = Name,
                    Width = Width,
                    Height = Height,
                    FilledData = Data,
                    Keyword = Keyword
                };
                bool result = false;
                //SaveIcon
                if (m_isNew)
                {
                    icon.CreatedTime = DateTime.Now;
                    result = DataRetrieved.Instance.AddIcon(icon);
                }
                else
                {
                    icon.ModifiedTime = DateTime.Now;
                    result = DataRetrieved.Instance.ModifiedIcon(icon);
                }
                if (!result)
                {
                    ModernDialog.ShowMessage("Save Icon Failed.\r\nThe reason maybe you have not enought file write permition", "Operation Failed", MessageBoxButton.OK, m_window);
                    return;
                }
                m_window.Close();
            }
            else
            {
                ModernDialog.ShowMessage(validationResult.ToString(), "Operation Failed", MessageBoxButton.OK, m_window);
            }
        }

        private void OnValidationResultChanged(object sender, ValidationResultChangedEventArgs e)
        {
            if (!IsValid.GetValueOrDefault(true))
            {
                ValidationResult validationResult = Validator.GetResult();

                UpdateValidationSummary(validationResult);
            }
        }

        private void UpdateValidationSummary(ValidationResult validationResult)
        {
            IsValid = validationResult.IsValid;
            ValidationErrorsString = validationResult.ToString();
        }
    }
}
