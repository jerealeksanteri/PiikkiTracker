using Microsoft.JSInterop;

namespace PiikkiTracker.Services;

public class ThemeService
{
    private readonly IJSRuntime _jsRuntime;
    private string _currentTheme = "light";

    public event Action<string>? ThemeChanged;

    public ThemeService(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    public string CurrentTheme => _currentTheme;

    public async Task InitializeThemeAsync()
    {
        try
        {
            var savedTheme = await _jsRuntime.InvokeAsync<string?>("localStorage.getItem", "piikki-theme");
            if (!string.IsNullOrEmpty(savedTheme) && (savedTheme == "light" || savedTheme == "dark"))
            {
                _currentTheme = savedTheme;
                await ApplyThemeAsync(_currentTheme);
            }
        }
        catch
        {
            // Fallback to light theme if localStorage is not available
            _currentTheme = "light";
        }
    }

    public async Task ToggleThemeAsync()
    {
        _currentTheme = _currentTheme == "light" ? "dark" : "light";
        await ApplyThemeAsync(_currentTheme);
        await SaveThemeAsync(_currentTheme);
        ThemeChanged?.Invoke(_currentTheme);
    }

    private async Task ApplyThemeAsync(string theme)
    {
        try
        {
            await _jsRuntime.InvokeVoidAsync("eval",
                $"document.documentElement.setAttribute('data-theme', '{theme}')");
        }
        catch
        {
            // Silently fail if JS is not available
        }
    }

    private async Task SaveThemeAsync(string theme)
    {
        try
        {
            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "piikki-theme", theme);
        }
        catch
        {
            // Silently fail if localStorage is not available
        }
    }
}