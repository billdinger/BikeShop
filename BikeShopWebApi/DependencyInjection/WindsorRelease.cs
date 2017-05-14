using System;

namespace BikeShopWebApi.DependencyInjection
{
    public class WindsorRelease : IDisposable
    {
        private Action Release { get; }

        /// <summary>
        /// Used to construct a <see cref="T:BikeShopWebApi.DependencyInjection.WindsorRelease"/> class. The action passed to the constructor
        /// is invoked when Dispose is called on the method.
        /// </summary>
        /// <param name="release">A <see cref="T:System.Action"/> that is aclled when Dispose is called on this class. Cannot be null.</param>
        public WindsorRelease(Action release)
        {
            if (release == null)
            {
                throw new ArgumentNullException(nameof(release));
            }
            Release = release;
        }


        public void Dispose()
        {
            Dispose(true);
        }

        private bool _disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                Release();
            }
            _disposed = true;

        }
    }
}