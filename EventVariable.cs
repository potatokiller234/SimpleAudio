using System;
namespace SimpleAudioAndSettings
{
    public class EventVariable<T>
    {
        private T value;
        public event System.EventHandler OnValueChange;

        public EventVariable(T value)
        {
            this.value = value;
        }
        public T GetValue()
        {
            return value;
        }
        public void SetValue(T value)
        {
            this.value = value;
            OnValueChange?.Invoke(this, EventArgs.Empty);
        }
    }
}
