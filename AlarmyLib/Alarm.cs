using System;

namespace AlarmyLib
{
    /// <summary>
    /// Describes the information required to trigger an alarm at a client.
    /// </summary>
    public class Alarm
    {
        public bool IsRtl { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }

        /// <summary>
        /// Create a new Alarm.
        /// </summary>
        /// <param name="isRtl">Whether the alarm content should be displayed as right to left.</param>
        /// <param name="title">Title of the alarm.</param>
        /// <param name="content">Content of the alarm.</param>
        public Alarm(bool isRtl, string title, string content)
        {
            IsRtl = isRtl;

            if (title.Length == 0)
            {
                throw new ArgumentException("Title must have a value.");
            }
            Title = title;

            if (content.Length == 0)
            {
                throw new ArgumentException("Content must have a value.");
            }
            Content = content;
        }
    }
}
