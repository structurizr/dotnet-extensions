namespace Structurizr.Documentation
{
    
    internal class FormattedContent
    {

        internal string Content { get; }
        internal Format Format { get; }

        internal FormattedContent(string content, Format format)
        {
            Content = content;
            Format = format;
        }
 
    }   
    
}