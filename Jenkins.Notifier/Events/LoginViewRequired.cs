namespace Jenkins.Notifier.Events
{
    public class LoginViewRequired : IDomainEvent
    {
        private readonly object sender;

        public LoginViewRequired(object sender)
        {
            this.sender = sender;
        }

        public override bool Equals(object obj)
        {
            return obj is LoginViewRequired && this.Equals((LoginViewRequired)obj);
        }

        private bool Equals(LoginViewRequired other)
        {
            return other.sender.Equals(this.sender);
        }

        public override int GetHashCode()
        {
            return this.sender != null ? this.sender.GetHashCode() : 0;
        }
    }
}
