using System;
using System.Security.Cryptography;
using System.Text;

namespace Structurizr.Api
{
    internal class Md5Digest
    {

        internal string Generate(string content)
        {
            if (content == null)
            {
                content = "";
            }

            MD5 md5 = MD5.Create();
            byte[] textToHash = Encoding.UTF8.GetBytes(content);
            byte[] result = md5.ComputeHash(textToHash);

            return BitConverter.ToString(result).Replace("-", "").ToLower();
        }

    }
}
