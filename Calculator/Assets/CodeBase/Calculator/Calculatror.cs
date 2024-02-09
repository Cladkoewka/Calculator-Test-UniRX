using System;
using System.Linq;
using TMPro;
using UniRx;
using UnityEngine;

namespace Assets.CodeBase.Calculator
{
    public class Calculatror : MonoBehaviour
    {
        [SerializeField] private Button[] _buttons;
        [SerializeField] private TMP_Text _calculatorDisplay;

        private string _currentNumber = "";
        private string _additionalNumber = "";
        private bool _inOperation = false;
        private char _currentOperation;
        
        private void Start()
        {
            IObservable<char> combinedStream = Observable.Merge(_buttons.Select(button => button.OnInteract));

            combinedStream.Subscribe(HandleInput);
        }

        private void HandleInput(char nextChar)
        {
            if (Char.IsDigit(nextChar))
                AddDigit(nextChar);
            else if (nextChar == '.')
                AddDot();
            else
                HandleOperation(nextChar);
        }

        private void AddDot()
        {
            if (_inOperation && !_additionalNumber.Contains('.') && _additionalNumber.Length > 0)
                _additionalNumber += ',';
            else if (!_inOperation && !_currentNumber.Contains('.') && _currentNumber.Length > 0)
                _currentNumber += ',';
            UpdateDisplay();
        }

        private void HandleOperation(char operation)
        {
            switch (operation)
            {
                case '=':
                    Equal();
                    break;
                case 'A':
                    ClearNumber();
                    break;
                case 'C':
                    RemoveLastDigit();
                    break;
                case '+':
                case '-':
                case '*':
                case '/':
                    SetOperation(operation);
                    break;
            }
        }

        private void SetOperation(char operation)
        {
            if (!_inOperation)
            {
                UpdateDisplay();
                _currentOperation = operation;
                _inOperation = true;
            }
        }

        private void Equal()
        {
            if (_inOperation)
            {
                float result = Calculate();
                _currentNumber = result.ToString();
                _additionalNumber = "";
                _inOperation = false;
                UpdateDisplay();
            }
        }

        private float Calculate()
        {
            float digitalCurrentNumber = float.Parse(_currentNumber);
            float digitalAdditionalNumber = float.Parse(_additionalNumber);
            float result = 0;
            switch (_currentOperation)
            {
                case '+':
                    result = digitalCurrentNumber + digitalAdditionalNumber;
                    break;
                case '-':
                    result = digitalCurrentNumber - digitalAdditionalNumber;
                    break;
                case '*':
                    result = digitalCurrentNumber * digitalAdditionalNumber;
                    break;
                case '/':
                    result = digitalCurrentNumber / digitalAdditionalNumber;
                    break;
            }

            return result;
        }

        private void RemoveLastDigit()
        {
            if (_inOperation && _additionalNumber.Length > 0)
                _additionalNumber = _additionalNumber.Substring(0, _additionalNumber.Length - 1);
            else if (!_inOperation && _currentNumber.Length > 0)
                _currentNumber = _currentNumber.Substring(0, _currentNumber.Length - 1);

            UpdateDisplay();
        }

        private void ClearNumber()
        {
            if (_inOperation)
                _additionalNumber = String.Empty;
            else
                _currentNumber = String.Empty;

            UpdateDisplay();
        }

        private void AddDigit(char nextChar)
        {
            if (_inOperation)
                _additionalNumber += nextChar;
            else
                _currentNumber += nextChar;
            UpdateDisplay();
        }

        private void UpdateDisplay()
        {
            if (_inOperation && _additionalNumber.Length > 0)
                _calculatorDisplay.text = _additionalNumber;
            else
                _calculatorDisplay.text = _currentNumber;
        }
    }
}