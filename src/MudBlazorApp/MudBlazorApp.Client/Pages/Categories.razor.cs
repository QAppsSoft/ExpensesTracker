using System.Net.Http.Json;
using Api.Models.Dto.Categories;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using MudBlazorApp.Client.Pages.Components;
using MudBlazorApp.Client.Pages.Dialogs;

namespace MudBlazorApp.Client.Pages;

public partial class Categories : IDisposable
{
    [CascadingParameter] private PreRenderState PreRenderState { get; set; } = new(IsPreRender: false);
    private readonly HttpClient _client = new() { BaseAddress = new Uri("http://localhost:5079") };
    private List<CategoryDto> _categories = [];
    private bool _loading = true;

    private readonly DialogOptions _dialogOptions = new()
    {
        CloseOnEscapeKey = true,
        FullWidth = true,
        MaxWidth = MaxWidth.Small
    };

    [Inject] private IDialogService DialogService { get; set; } = null!;
    [Inject] private ISnackbar Snackbar { get; set; } = null!;

    private async Task EditCategory(CategoryDto? oldCategory)
    {
        if (oldCategory == null)
        {
            return;
        }

        var parameters = new DialogParameters<EditCategoryDialog> { { x => x.OldCategory, oldCategory } };

        var dialog = await DialogService.ShowAsync<EditCategoryDialog>("Edit category", parameters, _dialogOptions);
        var dialogResult = await dialog.Result;

        if (dialogResult is { Canceled: false, Data: CategoryDto updatedCategory })
        {
            try
            {
                var result = await _client.PutAsJsonAsync($"api/v1/categories/{updatedCategory.Id}", updatedCategory);

                if (result.IsSuccessStatusCode)
                {
                    var receivedCategory = await result.Content.ReadFromJsonAsync<CategoryDto>();

                    var index = _categories.IndexOf(oldCategory);
                    _categories[index] = receivedCategory;

                    StateHasChanged();

                    Snackbar.Add("Category updated successfully", Severity.Success);
                }
                else
                {
                    Snackbar.Add($"Error updating category.\n\nStatus code: {result.StatusCode} \nReason: {result.ReasonPhrase}", Severity.Error);
                }
            }
            catch (Exception)
            {
                Snackbar.Add("Error updating category", Severity.Error);
            }
        }
    }

    private async Task DeleteCategory(CategoryDto? category)
    {
        if (category == null)
        {
            return;
        }
        
        var dialogResult = await DialogService.ShowMessageBox(
            "Delete category",
            $"Are you sure you want to delete category \"{category.Name}\"?", 
            yesText:"Delete!", cancelText:"Cancel");
        
        if (dialogResult != null)
        {
            try
            {
                var result = await _client.DeleteAsync($"api/v1/categories/{category.Id}");

                if (result.IsSuccessStatusCode)
                {
                    _categories.Remove(category);

                    Snackbar.Add("Category deleted successfully", Severity.Success);
                }
                else
                {
                    Snackbar.Add($"Error deleting category.\n\nStatus code: {result.StatusCode} \nReason: {result.ReasonPhrase}", Severity.Error);
                }
            }
            catch (Exception)
            {
                Snackbar.Add("Error deleting category", Severity.Error);
            }
        }
    }

    private async Task OpenAddDialog()
    {
        var dialog = await DialogService.ShowAsync<AddCategoryDialog>("Add new category", _dialogOptions);
        var dialogResult = await dialog.Result;

        if (dialogResult is { Canceled: false })
        {
            var newItem = dialogResult.Data as CreateCategoryDto;

            try
            {
                var result = await _client.PostAsJsonAsync("api/v1/categories", newItem);

                if (result.IsSuccessStatusCode)
                {
                    var newCategory = await result.Content.ReadFromJsonAsync<CategoryDto>();
                    if (newCategory != null)
                    {
                        _categories.Add(newCategory);
                    }

                    Snackbar.Add("Category added successfully", Severity.Success);
                }
                else
                {
                    Snackbar.Add($"Error adding category.\n\nStatus code: {result.StatusCode} \nReason: {result.ReasonPhrase}", Severity.Error);
                }
            }
            catch (Exception)
            {
                Snackbar.Add("Error adding category", Severity.Error);
            }
        }
    }

    protected override async Task OnInitializedAsync()
    {
        if (PreRenderState.IsPreRender)
        {
            return;
        }

        await LoadCategories();
    }

    private async Task LoadCategories()
    {
        try
        {
            var result = await _client.GetAsync("api/v1/categories");
            if (result.IsSuccessStatusCode)
            {
                _categories = await result.Content.ReadFromJsonAsync<List<CategoryDto>>() ?? [];
                _loading = false;
            }
            else
            {
                Snackbar.Add(
                    $"Error loading categories. Retry in a few minutes.\n\nStatus code: {result.StatusCode} \nReason: {result.ReasonPhrase}",
                    Severity.Error);
            }
        }
        catch (Exception)
        {
            Snackbar.Add("Error loading categories. Retry in a few minutes.", Severity.Error);
        }
    }

    public void Dispose()
    {
        _client.Dispose();
    }
}