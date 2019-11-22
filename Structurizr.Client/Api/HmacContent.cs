using System.Text;

namespace Structurizr.Api
{
    internal class HmacContent
    {

        private string[] strings;

        internal HmacContent(params string[] strings)
        {
            this.strings = strings;
        }

        public override string ToString()
        {
            StringBuilder buf = new StringBuilder();
            foreach (string s in strings)
            {
                buf.Append(s);
                buf.Append("\n");
            }

            return buf.ToString();
        }

    }
}
