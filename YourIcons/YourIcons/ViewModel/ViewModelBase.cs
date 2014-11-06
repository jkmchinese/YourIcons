using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MvvmValidation;

namespace YourIcons.ViewModel
{
    /// <summary>
    /// Base class for all ViewModel classes.
    /// It provides support for property change notifications and has a DisplayName property.
    /// This class is abstract.
    /// </summary>
    public abstract class ViewModelBase : INotifyPropertyChanged, IDisposable, IValidatable, IDataErrorInfo
    {
        protected ValidationHelper Validator { get; private set; }
        private DataErrorInfoAdapter DataErrorInfoAdapter { get; set; }


        /// <summary>
        /// Initializes a new instance of the <see cref="ViewModelBase"/> class.
        /// </summary>
        protected ViewModelBase()
        {
            Validator = new ValidationHelper();
            DataErrorInfoAdapter = new DataErrorInfoAdapter(Validator);
            OnCreated();
        }

        void OnCreated()
        {
            HookUpValidationNotification();
        }

        public string this[string columnName]
        {
            get { return DataErrorInfoAdapter[columnName]; }
        }

        public string Error
        {
            get { return DataErrorInfoAdapter.Error; }
        }

        private void HookUpValidationNotification()
        {
            // Due to limitation of IDataErrorInfo, in WPF we need to explicitly indicated that something has changed
            // about the property in order for the framework to look for errors for the property.
            Validator.ResultChanged += (o, e) =>
            {
                var propertyName = e.Target as string;

                if (!string.IsNullOrEmpty(propertyName))
                {
                    RaisePropertyChanged(propertyName);
                }
            };
        }

        Task<ValidationResult> IValidatable.Validate()
        {
            return Validator.ValidateAllAsync();
        }

        /// <summary>
        /// Warns the developer if this object does not have
        /// a public property with the specified name. This 
        /// method does not exist in a Release build.
        /// </summary>
        [Conditional("DEBUG")]
        [DebuggerStepThrough]
        protected void VerifyPropertyName(string propertyName)
        {
            // Verify that the property name matches a real,  
            // public, instance property on this object.
            if (this.GetType().GetProperty(propertyName) == null)
            {
                Debug.Assert(false, "Invalid property name: " + propertyName);
            }
        }

        /// <summary>
        /// Raised when a property on this object has a new value.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Invokes the specified action on the UI thread.
        /// </summary>
        /// <param name="action">An Action to be invoked on the UI thread.</param>
        public static void InvokeOnUIThread(Action action)
        {
#if SILVERLIGHT
			var dispatcher = System.Windows.Deployment.Current.Dispatcher;
#else
            var dispatcher = System.Windows.Threading.Dispatcher.CurrentDispatcher;
#endif
            if (dispatcher.CheckAccess())
            {
                action();
            }
            else
            {
                dispatcher.BeginInvoke(action);
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing,
        /// or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        internal void RaisePropertyChanged(string propertyName)
        {
            this.OnPropertyChanged(propertyName);
        }

        /// <summary>
        /// Raises this object's <see cref="PropertyChanged"/> event.
        /// </summary>
        /// <param name="propertyName">The property that has a new value.</param>
        protected virtual void OnPropertyChanged(string propertyName)
        {
            this.VerifyPropertyName(propertyName);

            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                var e = new PropertyChangedEventArgs(propertyName);
                handler(this, e);
            }
        }

        /// <summary>
        /// Raises this object's <see cref="PropertyChanged"/> event. 
        /// </summary>
        /// <remarks>
        /// Use the following syntax:
        /// this.OnPropertyChanged(() => this.MyProperty);
        /// instead of:
        /// this.OnPropertyChanged("MyProperty");.
        /// </remarks>
        /// <param name="propertyExpression">A MemberExpression, containing the property that value changed.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        protected virtual void OnPropertyChanged<T>(Expression<Func<T>> propertyExpression)
        {
            this.OnPropertyChanged(((MemberExpression)propertyExpression.Body).Member.Name);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources.
        /// <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // Dispose managed resources.
            }

            // There are no unmanaged resources to release, but
            // if we add them, they need to be released here.
        }
    }
}
