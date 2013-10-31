using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TinyFileManager.NET.interfaces
{
    /// <summary>
    /// Returns a localized text, given the text id 
    /// The text id of a text is the text you see in the default implementation
    /// </summary>
    public interface ILocalizationService
    {
        string GetValue(string textId);
    }
}
