using System.Collections.Generic;

namespace Structurizr.Core.Util
{
    
    public class DictionaryUtils
    {
    
        public static Dictionary<string,string> Create(params string[] nameValuePairs)
        {
            Dictionary<string,string> map = new Dictionary<string, string>();

            if (nameValuePairs != null) {
                foreach (string nameValuePair in nameValuePairs)
                {
                    string[] tokens = nameValuePair.Split('=');
                    if (tokens.Length == 2)
                    {
                        map[tokens[0]] = tokens[1];
                    }
                }
            }

            return map;
        }
        
    }
    
}