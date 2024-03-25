using Microsoft.JSInterop;

namespace ITEC275__Budget_App_Final_Project__
{
    public class Popups
    {
        private readonly IJSRuntime _jsRuntime;

        public Popups(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public async Task AlertPopup(string text)
        {
            string script = $"alert('{text}');";
            await _jsRuntime.InvokeVoidAsync("eval", script);
        }
    }
}
