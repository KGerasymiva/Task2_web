using System.Linq;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Task2.Views.Shared
{
    public static class CustomHelperMethods
    {
        public static IHtmlContent CteateForm(this IHtmlHelper helper, int count)
        {
            var res = helper.BeginForm("Index", "Query6", FormMethod.Post,
                new {enctype = "multipart/form-data"});
            helper.ValidationSummary(true);

            var divtaTagBuilder = new TagBuilder("div");
            var dictDiv = new AttributeDictionary();
            dictDiv["class"] = "form-group col-md-2";

            divtaTagBuilder.MergeAttributes(dictDiv);

            var selecttaTagBuilder = new TagBuilder("select");

            var dict1 = new AttributeDictionary();
            dict1["class"] = "form-control";
            dict1["id"] = "id";

            selecttaTagBuilder.MergeAttributes(dict1);

            foreach (var id in Enumerable.Range(1, count))
            {
                var optionTagBuilder = new TagBuilder("option");
                optionTagBuilder.InnerHtml.Append(id.ToString());
                selecttaTagBuilder.InnerHtml.AppendHtml(optionTagBuilder);
            }

            divtaTagBuilder.InnerHtml.AppendHtml(selecttaTagBuilder);
            divtaTagBuilder.InnerHtml.AppendHtml(new TagBuilder("br"));
            var inputtag = new TagBuilder("input");

            var dict2 = new AttributeDictionary();
            dict2["type"] = "submit";
            dict2["value"] = "Submit";

            inputtag.MergeAttributes(dict2);

            divtaTagBuilder.InnerHtml.AppendHtml(inputtag);

            res.EndForm();
            return divtaTagBuilder;
        }
    }
}
