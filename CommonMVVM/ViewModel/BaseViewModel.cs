namespace CommonMVVM.ViewModel
{
    using Common;

    public class BaseViewModel : BasePropertyChangedNotifier
    {
        public virtual bool SetValue<T>(ref T field, T newValue)
        {
            if (Equals(field, newValue)) return false;

            field = newValue;
            return true;
        }
    }
}
