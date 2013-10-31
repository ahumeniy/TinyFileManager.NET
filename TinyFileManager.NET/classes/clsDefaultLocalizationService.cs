using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TinyFileManager.NET.interfaces;

namespace TinyFileManager.NET.classes
{
    internal sealed class clsDefaultLocalizationService : ILocalizationService
    {
        public string GetValue(string textId)
        {
            return textId;
        }
    }
}