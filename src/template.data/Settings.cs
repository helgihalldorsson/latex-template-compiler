using System.Collections.Generic;

namespace template.data
{
    public class Settings
    {
        public bool ignoreEnabled { get; set; } = false;

        public List<string> supportedLanguages { get; set; }
    }
}
