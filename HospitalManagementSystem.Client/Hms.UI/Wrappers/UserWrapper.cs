namespace Hms.UI.Wrappers
{
    using Hms.Common.Interface.Domain;

    public class UserWrapper : ModelWrapper<User>
    {
        public UserWrapper(User model)
            : base(model)
        {
        }

        public int Id
        {
            get
            {
                if (this.Model == null)
                {
                    return -1;
                }

                return this.GetValue<int>();
            }

            set
            {
                this.SetValue(value);
            }
        }

        public string Login
        {
            get
            {
                return this.GetValue<string>();
            }

            set
            {
                this.SetValue(value);
            }
        }

        public string PasswordHash
        {
            get
            {
                return this.GetValue<string>();
            }

            set
            {
                this.SetValue(value);
            }
        }
    }
}
