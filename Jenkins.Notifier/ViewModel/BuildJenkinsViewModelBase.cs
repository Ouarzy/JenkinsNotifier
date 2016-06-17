namespace Jenkins.Notifier.ViewModel
{
    using System;
    using System.Runtime.CompilerServices;

    using GalaSoft.MvvmLight;

    public abstract class BuildJenkinsViewModelBase : ViewModelBase, IDisposable
    {
        /// <summary>
        /// Affecte une valeur pour la propriété appelante si nécessaire et notifie le changement
        /// </summary>
        /// <typeparam name="T">Type de la propriété</typeparam>
        /// <param name="source">Valeur d'origine</param>
        /// <param name="value">Valeur à affecter</param>
        /// <param name="propertyName">Nom de la propriété appelante</param>
        /// <returns>Retourne vrai si la propriété à été modifiée, faux sinon</returns>
        protected bool SetProperty<T>(ref T source, T value, [CallerMemberName] string propertyName = null)
        {
            if (Equals(source, value))
            {
                return false;
            }

            source = value;

            this.RaisePropertyChanged(propertyName);
            return true;
        }

        protected override void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (propertyName != null)
            {
                // ReSharper disable once ExplicitCallerInfoArgument
                base.RaisePropertyChanged(propertyName);
            }
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool isDisposing)
        {
        }
    }
}
