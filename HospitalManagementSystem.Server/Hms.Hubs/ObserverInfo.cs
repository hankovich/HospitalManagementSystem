namespace Hms.Hubs
{
    using System;

    public struct ObserverInfo : IEquatable<ObserverInfo>
    {
        public string ObserverIdentifier { get; }

        public DateTime Date { get; }

        public ObserverInfo(string observerIdentifier, DateTime date)
        {
            this.ObserverIdentifier = observerIdentifier;
            this.Date = date;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((this.ObserverIdentifier != null ? this.ObserverIdentifier.GetHashCode() : 0) * 397) ^ this.Date.GetHashCode();
            }
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public bool Equals(ObserverInfo other)
        {
            return string.Equals(this.ObserverIdentifier, other.ObserverIdentifier) && this.Date.Equals(other.Date);
        }
    }
}