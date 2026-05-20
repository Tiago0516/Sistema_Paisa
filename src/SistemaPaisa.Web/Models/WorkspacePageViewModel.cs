namespace SistemaPaisa.Web.Models;

public class WorkspacePageViewModel
{
    public string ActiveModuleCode { get; init; } = string.Empty;
    public string ActiveModuleName { get; init; } = string.Empty;
    public ModuleGridViewModel Grid { get; init; } = new();
}

public record ModuleGridViewModel
{
    public string Title { get; init; } = string.Empty;
    public string ModuleCode { get; init; } = string.Empty;
    public string CreateUrl { get; init; } = string.Empty;
    public string CreateLabel { get; init; } = "Crear";
    public bool AllowCreate { get; init; }
    public bool AllowView { get; init; }
    public bool AllowEdit { get; init; }
    public bool AllowDelete { get; init; }
    public IReadOnlyList<ModuleGridColumn> Columns { get; init; } = [];
    public IReadOnlyList<ModuleGridRow> Rows { get; init; } = [];
}

public class ModuleGridColumn
{
    public string Key { get; init; } = string.Empty;
    public string Label { get; init; } = string.Empty;
}

public class ModuleGridRow
{
    public IReadOnlyDictionary<string, string> Cells { get; init; } = new Dictionary<string, string>();
    public ModuleGridRowActions? Actions { get; init; }
}

public class ModuleGridRowActions
{
    public string? DetailsUrl { get; init; }
    public string? EditUrl { get; init; }
    public string? DeleteUrl { get; init; }
}
