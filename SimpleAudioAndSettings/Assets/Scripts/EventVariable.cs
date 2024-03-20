using System;
namespace SimpleAudioAndSettings
{
    public class EventVariable<T>
    {
        private T value;
        public event System.EventHandler OnValueChange;
        public SimpleAudioType type;

        public EventVariable(T value, SimpleAudioType type)
        {
            this.value = value;
            this.type = type;
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
