using System;
using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Threading;

namespace Infrastructure
{
    public interface ILockService
    {
        bool TryGet(string key, out IDisposable disposable);
    }
    
    internal class LockService : ILockService
    {
        private readonly Dictionary<string, SemaphoreSlim> _semaphores = new Dictionary<string, SemaphoreSlim>();
        private readonly SemaphoreSlim _semaphoreSlim = new SemaphoreSlim(1, 1);

        // Заменить на нормальное решение при расширении, с текущими задачами справляется
        public bool TryGet(string key, out IDisposable disposable)
        {
            _semaphoreSlim.Wait();

            if (!_semaphores.ContainsKey(key))
            {
                _semaphores.Add(key, new SemaphoreSlim(1, 1));
            }

            var keySemaphoreSlim = _semaphores[key];
            var locked = keySemaphoreSlim.TryLock();
            
            _semaphoreSlim.Release();
            
            if (locked)
            {
                disposable = Disposable.Create(keySemaphoreSlim, x => x.Release());
                return true;
            }

            disposable = null;
            return false;
        }
    }
}