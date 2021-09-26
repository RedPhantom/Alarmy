using System;

namespace AlarmyLib
{
    public class Group
    {
        public Guid ID { get; set; }
        public string Name { get; set; }

        /// <summary>
        /// A special group which all Alarmy members are part of.
        /// They are unable to leave this group.
        /// </summary>
        public static readonly Group GlobalGroup = new Group();

        public Group(Guid guid = new Guid(), string name = "")
        {
            ID = guid;
            Name = name;
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
    }
}
