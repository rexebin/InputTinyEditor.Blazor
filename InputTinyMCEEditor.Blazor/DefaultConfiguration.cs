namespace InputTinyMCEEditor.Blazor;

internal static class DefaultConfiguration
{
    internal const string DefaultPlugins =
        @"print preview paste importcss searchreplace autolink autosave save 
directionality code visualblocks visualchars fullscreen image link media template 
codesample table charmap hr pagebreak nonbreaking anchor toc insertdatetime 
advlist lists wordcount imagetools textpattern noneditable help charmap quickbars emoticons";

    internal const string DefaultMenubar = @"file edit view insert format tools table help";

    internal const string DefaultToolbar = @"undo redo | bold italic underline strikethrough 
| fontselect fontsizeselect formatselect | alignleft aligncenter alignright alignjustify 
| outdent indent |  numlist bullist | forecolor backcolor removeformat | pagebreak 
| charmap emoticons | fullscreen  preview save print | insertfile image media template link anchor codesample 
| ltr rtl";
    
    internal const int DefaultHeight = 600;
}