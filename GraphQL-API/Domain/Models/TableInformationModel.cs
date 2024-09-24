public class TableInformationModel
{
    public required string TABLE_NAME { get; set; }
    public List<Table_Column>? COLUMNS { get; set; }
}
public class Table_Column
{
    public required string Name { get; set; }
    public required string Type { get; set; }
    public bool? Nullable { get; set; }
    public bool? IsFilterable { get; set; }
    public bool? IsRetrievable { get; set; }
}