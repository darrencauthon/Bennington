﻿namespace Bennington.Content
{
    public class ContentAction
    {
        public string Action { get; set; }
        public string DisplayName { get; set; }

        public override string ToString()
        {
            return DisplayName;
        }
    }
}