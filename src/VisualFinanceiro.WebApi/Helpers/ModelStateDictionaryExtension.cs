using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;
using System.Linq;

namespace VisualFinanceiro.Common.Extensions
{
    public static class ModelStateDictionaryExtension
    {
        public static List<KeyValuePair<string, string>> GetModelErrors(this ModelStateDictionary modelState)
        {
            var list = modelState.SelectMany(m => m.Value.Errors.Select(e => new KeyValuePair<string, string>(m.Key, e.ErrorMessage))).ToList();
            return list;
        }
    }
}
