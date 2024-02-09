using UniRx;
using UnityEngine;
using Button = Assets.CodeBase.Calculator.Button;
using Image = UnityEngine.UI.Image;

namespace Assets.CodeBase.PlayerMove
{
    public class CalculatorInteract : MonoBehaviour
    {
        [SerializeField] private Image _interactPointImage;
        private Button _currentButton; 
        
        private void Start()
        {
            Observable.
                EveryUpdate().
                Subscribe(_ => Raycast());

            Observable.
                EveryUpdate().
                Where(_ => _currentButton != null).
                Subscribe(_ => Interact());
        }

        private void Raycast()
        {
            Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.GetComponent<Button>() != null)
                {
                    _currentButton = hit.collider.GetComponent<Button>();
                    _interactPointImage.color = Color.yellow;
                }
                else
                {
                    _currentButton = null;
                    _interactPointImage.color = Color.white;
                }
            }
        }

        private void Interact()
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (_currentButton != null)
                {
                    _currentButton.Interact();
                }
            }
        }
    }
}