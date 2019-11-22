namespace Structurizr
{

    /// <summary>
    /// Allows the sort order of views to be customized as follows:
    ///
    /// - Default: Views are grouped by the software system they are associated with, and then sorted by type (System Landscape, System Context, Container, Component, Dynamic and Deployment) within these groups.
    /// - Type: Views are sorted by type (System Landscape, System Context, Container, Component, Dynamic and Deployment).
    /// - Key: Views are sorted by the view key (alphabetical, ascending).
    /// </summary>
    public enum ViewSortOrder
    {
        
        Default,
        Type,
        Key
        
    }
    
}