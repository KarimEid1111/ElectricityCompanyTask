namespace WebPortalDomain.Dtos;

public class CuttingDownSearchRequest
{
    public int? SourceOfCuttingDown { get; set; } // Channel_Key
    public int? ProblemTypeKey { get; set; } // Problem_Type_Key
    public bool? IsClosed { get; set; } // Filter by ActualEndDate presence
    public int? SearchCriteria { get; set; } // Network_Element_Type_Key
    public string SearchValue { get; set; } // City Name, etc.
}
