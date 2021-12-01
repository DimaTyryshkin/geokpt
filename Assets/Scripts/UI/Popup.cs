using SiberianWellness.NotNullValidation;
using UnityEngine;
using UnityEngine.Events;

namespace Geo.UI
{ 
    public class Popup : MonoBehaviour
    {
        public event UnityAction        Closed;
        public event UnityAction<Popup> Opened;

        public bool IsVisible { get; private set; }

        public virtual void Close()
        {
            Hide();
            Closed?.Invoke();
        }

        void Hide()
        {
            gameObject.SetActive(false);
            IsVisible = false;
        }

        public virtual void Show()
        {
            gameObject.SetActive(true);
            IsVisible = true;
            Opened?.Invoke(this);
        }
    }
}
