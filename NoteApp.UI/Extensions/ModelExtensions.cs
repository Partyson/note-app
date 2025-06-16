﻿using Microsoft.AspNetCore.Mvc.RazorPages;

namespace NoteApp.UI.Extensions;

public static class ModelExtensions
{
    public static string GetImportanceText(this PageModel model, string importance) => importance switch
    {
        "High" => "Высокий",
        "VeryHigh" => "Очень высокий",
        _ => "Обычный"
    };
}