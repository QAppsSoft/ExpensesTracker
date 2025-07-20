using Api.Models.Dto.Categories;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace MudBlazorApp.Client.Pages.Dialogs;

public partial class AddCategoryDialog
{
    [CascadingParameter] private IMudDialogInstance MudDialog { get; set; }
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
    public string CategoryColor { get; set; } = "";
    private void Cancel() => MudDialog.Cancel();

    private void Submit()
    {
        if (string.IsNullOrWhiteSpace(Name) || string.IsNullOrWhiteSpace(CategoryColor))
        {
            // Handle validation error
            return;
        }

        MudDialog.Close(DialogResult.Ok(new CreateCategoryDto(Name, Description, CategoryColor)));
    }
}