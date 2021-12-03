using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using System.Linq.Expressions;
using TinyMCE.Blazor;

namespace InputTinyMCEEditor.Blazor;

public class InputTinyEditor : Editor
{
    private string _userClassName = "tinymce-wrapper";
    private Dictionary<string, object> _userConf = new();
    [CascadingParameter] public EditContext? EditContext { get; set; }

    /**
     * Required to access EditContext's field identifier
     */

    [Parameter]
    [EditorRequired]
    public Expression<Func<string>> For { get; set; } = default!;

    [Parameter] public int? Height { get; set; }
    [Inject] public IJSRuntime Js { get; set; } = default!;

    /**
     * Params for quick configuration
     */
    [Parameter] public string? Menubar { get; set; }

    [Parameter]
    public string? Plugins { get; set; }

    [Parameter] public string? Toolbar { get; set; }

    /**
     * Options to trigger validation on input
     */

    [Parameter]
    public bool ValidateOnInput { get; set; } = false;

    private Dictionary<string, object> DefaultConf => new()
    {
        { "plugins", Plugins ?? DefaultConfiguration.DefaultPlugins },
        { "menubar", Menubar ?? DefaultConfiguration.DefaultMenubar },
        { "toolbar", Toolbar ?? DefaultConfiguration.DefaultToolbar },
        { "height", Height ?? DefaultConfiguration.DefaultHeight }
    };

    /*
     * By default, hooked up with TinyMce's onchange event;
     */

    [JSInvokable("OnChange")]
    public void OnChange()
    {
        if (!ValidateOnInput)
        {
            NotifyFieldChanged();
        }
    }

    /*
    * By default, hooked up with TinyMce's oninput event;
    */

    [JSInvokable("OnInput")]
    public void OnInput()
    {
        if (ValidateOnInput)
        {
            NotifyFieldChanged();
        }
    }

    /**
    * Store user's ClassName and Conf values
    * 1. ClassName: used as a base when add "modified, invalid, valid" on validation
    * 2. Conf: to merge with above default conf.
    */

    public override Task SetParametersAsync(ParameterView parameters)
    {
        _userClassName = parameters.GetValueOrDefault<string>("ClassName") ?? "tinymce-wrapper";
        _userConf = parameters.GetValueOrDefault<Dictionary<string, object>>("Conf") ?? new();
        return base.SetParametersAsync(parameters);
    }

    /**
     * Pass InputTinyEditor instance to JS, so JS can call OnChange/OnInput methods defined here below.
     */

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);
        if (firstRender)
        {
            await Js.InvokeVoidAsync("inputTinyEditor.setEditorInstance", DotNetObjectReference.Create(this));
        }
    }

    protected override void OnParametersSet()
    {
        EnsureContextAndParameter();
        // Set JSConfSrc to window.editorConf loaded from JS module. For hooking up JS events and
        // functions, see wwwroot/InputTinyMCEEditor.Blazor.lib.module.js
        JsConfSrc = "inputTinyEditor.editorConf";
        ClassName = GetClassName();
        // merge default conf with user conf
        Conf = Helper.Merge(DefaultConf, _userConf);
        base.OnParametersSet();
    }

    private void EnsureContextAndParameter()
    {
        if (For == null)
        {
            throw new Exception("Field accessor: \"For\" parameter is not specified.");
        }

        if (EditContext == null)
        {
            throw new Exception(
                "Edit form context is not valid, please use this component inside an \"EditForm\" component");
        }
    }

    // prepend FieldCssClass to user provided class name;
    private string GetClassName()
    {
        return $"{EditContext?.FieldCssClass(For)} {_userClassName}";
    }

    private void NotifyFieldChanged()
    {
        // Notify field changed to trigger validation
        EditContext?.NotifyFieldChanged(FieldIdentifier.Create(For));
        UpdateClassNameAndReRender();
    }

    // prepend FieldCssClass (modified, valid invalid) to TinyMce's container element
    private void UpdateClassNameAndReRender()
    {
        ClassName = GetClassName();
        StateHasChanged();
    }
}