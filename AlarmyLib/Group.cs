using System;

namespace AlarmyLib
{
    public class Group
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public bool Enabled { get; set; }

        /// <summary>
        /// A special group which all Alarmy members are part of.
        /// They are unable to leave this group.
        /// </summary>
        public static readonly Group GlobalGroup = new Group();

        public Group(Guid guid = new Guid(), string name = "", bool enabled = true)
        {
            ID = guid;
            Name = name;
            Enabled = enabled;
        }

        /// <summary>
        /// Create an empty group.
        /// </summary>
        /// <returns>An empty <see cref="Group"/> instance.</returns>
        public static Group CreateEmpty()
        {
            return new Group(Guid.Empty, string.Empty);
        }

        public override string ToString()
        {
            return $"{Name} ({ID})";
        }

        public string Repr()
        {
            return $"<Group Name={Name}, ID={ID}, Enabled={Enabled}>";
        }

        public override bool Equals(object obj)
        {
            if (obj.GetType() != typeof(Group))
            {
                return false;
            }

            Group other = (Group)obj;

            // We don't consider Enable since this is a client settings.
            return ID == other.ID
                && Name == other.Name;
        }

        // TODO: override GetHashCode.
    }
}
