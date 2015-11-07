//original license: MIT
//
//See the file license.txt for copying permission.

using screenium.Compare;
using System;

namespace screenium.Reports.Html
{
    class HtmlSupport
    {
        private static int _targetId = 0;

        internal string GetHtmlColoredForResult(CompareResult compareResult, string text)
        {
            string color;
            const string green = "#00FF00";
            const string red = "#FF0000";
            switch (compareResult)
            {
                case Compare.CompareResult.Similar:
                    color = green;
                    break;
                case Compare.CompareResult.Different:
                    color = red;
                    break;
                default:
                    throw new ArgumentException("Not a recognised Report Result: " + compareResult);
            }
            return GetTagWithAttributesAndChildText("div", "style='background-color:" + color + "'", text);
        }

        internal string GetSeparator()
        {
            return GetTagStart("hr") + GetTagEnd("hr");
        }

        internal string GetLink(string text, string url)
        {
            return GetTagStartWithAttributes("a", "href='" + url + "' target='" + GetNextTargetId() + "'") + text + GetTagEnd("a");
        }

        private string GetNextTargetId()
        {
            return "_screenium_window_" + _targetId++;
        }

        private string GetTagWithAttributesAndChildText(string tag, string attributes, string text)
        {
            return GetTagStartWithAttributes(tag, attributes) + text + GetTagEnd(tag);
        }

        internal string GetTagWithChildText(string tag, string text)
        {
            return GetTagStart(tag) + text + GetTagEnd(tag);
        }

        internal string GetEmphasisedText(string text)
        {
            return GetTagStart("b") + text + GetTagEnd("b");
        }

        internal string CreateImageHtml(string imageFilePath, string altText)
        {
            return "<img src='" + imageFilePath + "' alt='" + altText + "' />";
        }
        internal string GetTagStart(string text)
        {
            return GetTagStartWithAttributes(text, "");
        }

        internal string GetTagStartWithAttributes(string text, string attributes)
        {
            return "<" + text + " " + attributes + ">" + Environment.NewLine;
        }

        internal string GetTagEnd(string text)
        {
            return "</" + text + ">" + Environment.NewLine;
        }
    }
}
