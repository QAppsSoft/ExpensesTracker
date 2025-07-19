using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace MudBlazorApp.Client.Pages.Dialogs;

public partial class YesNoDialog
{
    [CascadingParameter] private IMudDialogInstance MudDialog { get; set; }
    [Parameter] public string Title { get; set; }
    [Parameter] public string Message { get; set; }
    private void NoAction() => MudDialog.Close(DialogResult.Ok(false));
    private void YesAction() => MudDialog.Close(DialogResult.Ok(true));
}