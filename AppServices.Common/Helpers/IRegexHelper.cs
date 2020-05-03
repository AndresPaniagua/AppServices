using System;
using System.Collections.Generic;
using System.Text;

namespace AppServices.Common.Helpers
{
    public interface IRegexHelper
    {
        bool IsValidEmail(string emailaddress);
    }
}
