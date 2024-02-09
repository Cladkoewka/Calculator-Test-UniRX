using System;
using UniRx;
using UnityEngine;

namespace Assets.CodeBase.Calculator
{
    public class Button : MonoBehaviour
    {
        [SerializeField] private char _operator;

        private Subject<char> _interactionSubject = new Subject<char>();

        public IObservable<char> OnInteract => _interactionSubject;

        public void Interact()
        {
            _interactionSubject.OnNext(_operator);
            //Debug.Log(_operator);
        }

        private void OnDestroy()
        {
            _interactionSubject.Dispose();
        }
    }
}