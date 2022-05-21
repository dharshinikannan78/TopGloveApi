using System;

namespace topGlove.Extension
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class DisplayNameAttribute : Attribute
    {
        public string DisplayName { get; set; }

        public DisplayNameAttribute(string displayName)
        {
            DisplayName = displayName;
        }
    }
}
