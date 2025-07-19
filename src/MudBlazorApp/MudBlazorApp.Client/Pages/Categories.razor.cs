using System.Net.Http.Json;
using Api.Models.Dto.Categories;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using MudBlazorApp.Client.Pages.Components;
using MudBlazorApp.Client.Pages.Dialogs;

namespace MudBlazorApp.Client.Pages;

public partial class Categories
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
            var result = await _client.PutAsync($"api/v1/categories/{updatedCategory.Id}",
                JsonContent.Create(updatedCategory));

            if (result.IsSuccessStatusCode)
            {
                var receivedCategory = await result.Content.ReadFromJsonAsync<CategoryDto>();

                var index = _categories.IndexOf(oldCategory);
                _categories[index] = receivedCategory;

                StateHasChanged();
            }
        }
    }

    private async Task DeleteCategory(CategoryDto? category)
    {
        if (category == null)
        {
            return;
        }

        var parameters = new DialogParameters<YesNoDialog>
        {
            { x => x.Title, "Delete category" },
            { x => x.Message, $"Are you sure you want to delete category \"{category.Name}\"?" }
        };

        var dialog = await DialogService.ShowAsync<YesNoDialog>("Delete category", parameters, _dialogOptions);
        var dialogResult = await dialog.Result;

        if (dialogResult is { Data: true })
        {
            var result = await _client.DeleteAsync($"api/v1/categories/{category.Id}");

            if (result.IsSuccessStatusCode)
            {
                _categories.Remove(category);
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
            // Handle the new item (save to database, add to list, etc.)
            var result = await _client.PostAsync("api/v1/categories", JsonContent.Create(newItem));

            if (result.IsSuccessStatusCode)
            {
                var newCategory = await result.Content.ReadFromJsonAsync<CategoryDto>();
                if (newCategory != null) _categories.Add(newCategory);
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
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}