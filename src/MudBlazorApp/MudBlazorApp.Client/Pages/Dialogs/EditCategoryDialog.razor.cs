using Api.Models.Dto.Categories;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace MudBlazorApp.Client.Pages.Dialogs;

public partial class EditCategoryDialog
{
    [CascadingParameter] private IMudDialogInstance MudDialog { get; set; }
    public string Name { get; set; } = "";
    public string? Description { get; set; }
    public string CategoryColor { get; set; } = "";
    [Parameter] public CategoryDto OldCategory { get; set; }

    protected override void OnInitialized()
    {
        Name = OldCategory.Name;
        Description = OldCategory.Description;
        CategoryColor = OldCategory.Color;
    }

    private void Cancel() => MudDialog.Cancel();

    private void Submit()
    {
        if (string.IsNullOrWhiteSpace(Name) || string.IsNullOrWhiteSpace(CategoryColor))
        {
            // Handle validation error
            return;
        }

        MudDialog.Close(DialogResult.Ok(new CategoryDto(OldCategory.Id, Name, Description, CategoryColor)));
    }
}