using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using System.Text.Json;

namespace JsonFormatterBlazor.Pages;
public partial class Index
{
    IBrowserFile? currFile;

    string currValue = "";
    string formattedJson = "";
    string minifiedJson = "";

    async Task OnFilePicked(InputFileChangeEventArgs e)
    {
        this.currFile = e.File;

        using var s = currFile.OpenReadStream();
        using var reader = new StreamReader(s);
        this.currValue = await reader.ReadToEndAsync();

        var obj = JsonSerializer.Deserialize<JsonElement>(this.currValue);
        this.formattedJson = JsonSerializer.Serialize(obj, new JsonSerializerOptions()
        {
            WriteIndented = true,
        });
        this.minifiedJson = JsonSerializer.Serialize(obj, new JsonSerializerOptions()
        {
            WriteIndented = false,
        });
    }

    async Task Download(string content)
    {
        var s = new MemoryStream();
        using (var writer = new StreamWriter(s, leaveOpen: true))
        {
            await writer.WriteAsync(content);
        }

        s.Position = 0;

        using var streamRef = new DotNetStreamReference(s);
        await this.js.InvokeVoidAsync("downloadFile", "file.json", streamRef);
    }

    protected override Task OnAfterRenderAsync(bool firstRender)
    {
        return base.OnAfterRenderAsync(firstRender);
    }

}
