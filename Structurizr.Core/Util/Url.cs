using System;

namespace Structurizr.Util
{

    public class Url
    {

        public static bool IsUrl(string urlAsString)
        {
            if (urlAsString != null && urlAsString.Trim().Length > 0)
            {
                Uri uri;
                return Uri.TryCreate(urlAsString, UriKind.Absolute, out uri);
            }

            return false;
        }

    }

}
