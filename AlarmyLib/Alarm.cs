using System;

namespace AlarmyLib
{
    /// <summary>
    /// Describes the information required to trigger an alarm at a client.
    /// </summary>
    public class Alarm
    {
        /// <summary>
        /// Unique identifier of the message.
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// Whether the alarm content should be displayed as right to left.
        /// </summary>
        public bool IsRtl { get; set; }

        /// <summary>
        /// Title of the alarm. It will appear at the top of the window.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Content of the alarm. Rich text format.
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// Create a new Alarm.
        /// </summary>
        /// <param name="isRtl">Whether the alarm content should be displayed as right to left.</param>
        /// <param name="title">Title of the alarm. It will appear at the top of the window.</param>
        /// <param name="content">Content of the alarm. Rich text format.</param>
        public Alarm(string id, bool isRtl, string title, string content)
        {
            IsRtl = isRtl;

            if ((null == id) || (0 == id.Length))
            {
                throw new ArgumentException("ID must have a value");
            }
            ID = id;

            if ((null == title) || (0 == title.Length))
            {
                throw new ArgumentException("Title must have a value.");
            }
            Title = title;

            if ((null == content) || (0 == content.Length))
            {
                throw new ArgumentException("Content must have a value.");
            }
            Content = content;
        }

        public static string GenerateID()
        {
            Random r = new Random();
            return $"{DateTime.Now:yyyy-MM-dd_hh-mm-ss}_{r.Next():x}";
        }

        public override string ToString()
        {
            return Title;
        }
    }
}
