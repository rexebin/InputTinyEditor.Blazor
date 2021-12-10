using System.Linq.Expressions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using TinyMCE.Blazor;

namespace InputTinyMCEEditor.Blazor;

public class InputTinyEditor : Editor
{
    private Dictionary<string, object> _userConf = new();
    

    /**
     * Required to access EditContext's field identifier
     */

    [Parameter]
    [EditorRequired]
    public Expression<Func<string>> For { get; set; } = default!;

    [Parameter] public int? Height { get; set; }

    /**
     * Params for quick configuration
     */
    [Parameter]
    public string? Menubar { get; set; }

    [Parameter] public string? Plugins { get; set; }

    [Parameter] public string? Toolbar { get; set; }


    private Dictionary<string, object> DefaultConf => new()
    {
        { "plugins", Plugins ?? DefaultConfiguration.DefaultPlugins },
        { "menubar", Menubar ?? DefaultConfiguration.DefaultMenubar },
        { "toolbar", Toolbar ?? DefaultConfiguration.DefaultToolbar },
        { "height", Height ?? DefaultConfiguration.DefaultHeight }
    };

    protected override void OnParametersSet()
    {
        EnsureContextAndParameter();
        if (Field == null)
        {
            Field = For;
        }

        JsConfSrc = "inputTinyEditor.editorConf";
        // merge default conf with user conf
        Conf = Helper.Merge(DefaultConf, _userConf);
        base.OnParametersSet();
    }

    private void EnsureContextAndParameter()
    {
        if (EditContext != null && (For == null && Field == null))
        {
            throw new Exception("Field accessor: \"For\" parameter is not specified.");
        }
    }

    /**
    * Store user's ClassName and Conf values
    * 1. ClassName: used as a base when add "modified, invalid, valid" on validation
    * 2. Conf: to merge with above default conf.
    */
    public override Task SetParametersAsync(ParameterView parameters)
    {
        _userConf = parameters.GetValueOrDefault<Dictionary<string, object>>("Conf") ?? new();
        return base.SetParametersAsync(parameters);
    }
}