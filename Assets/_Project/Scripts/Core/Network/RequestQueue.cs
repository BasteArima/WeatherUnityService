using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts.Core.Network
{
    public class RequestQueue : IRequestQueue
    {
        private readonly Queue<IRequest> _requests = new Queue<IRequest>();
        private IRequest _currentRequest;
        private bool _isProcessing;

        public void Enqueue(IRequest request)
        {
            _requests.Enqueue(request);
            TryProcessNext();
        }

        public void Remove(IRequest request)
        {
            if (_requests.Contains(request))
            {
                var newQueue = new Queue<IRequest>();
                while (_requests.Count > 0)
                {
                    var r = _requests.Dequeue();
                    if (r != request)
                    {
                        newQueue.Enqueue(r);
                    }
                }

                while (newQueue.Count > 0)
                {
                    _requests.Enqueue(newQueue.Dequeue());
                }
            }
        }

        public void CancelCurrent()
        {
            if (_currentRequest != null)
            {
                _currentRequest.Cancel();
            }
        }

        private async void TryProcessNext()
        {
            if (_isProcessing) return;
            if (_requests.Count == 0) return;

            _isProcessing = true;
            _currentRequest = _requests.Dequeue();

            try
            {
                await _currentRequest.ExecuteAsync();
            }
            catch (Exception e)
            {
                Debug.LogError($"Request failed: {e.Message}");
            }
            finally
            {
                _currentRequest = null;
                _isProcessing = false;
                if (_requests.Count > 0)
                {
                    TryProcessNext();
                }
            }
        }
    }
}