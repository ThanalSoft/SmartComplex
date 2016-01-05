using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ThanalSoft.SmartComplex.Common.Attributes;

namespace ThanalSoft.SmartComplex.Web.HTMLHelpers
{
    public static class TableHelper
    {
        public static IHtmlString DisplayTableFor<TModel>(this HtmlHelper<TModel> pHtmlHelper, dynamic[] pTableValues, object pHtmlRowAttributes)
        {
            if (pTableValues == null || !pTableValues.Any())
            {
                var emptyTag = new TagBuilder("div");
                emptyTag.AddCssClass("col-md-12 col-sm-12 col-xs-12");
                var h4Tag = new TagBuilder("h4");
                h4Tag.MergeAttribute("style", "color: #CBB1B1; text-align: center;");
                var iTag = new TagBuilder("i");
                iTag.AddCssClass("fa fa-bell");
                h4Tag.InnerHtml += iTag.ToString();
                var emptyTextTag = new TagBuilder("label");
                emptyTextTag.MergeAttribute("style", "margin-left:5px;");
                emptyTextTag.SetInnerText("No Items found here.");
                h4Tag.InnerHtml += emptyTextTag.ToString();
                emptyTag.InnerHtml += h4Tag.ToString();

                return MvcHtmlString.Create(emptyTag.ToString());
            }

            var tableTag = new TagBuilder("table");
            tableTag.GenerateId("displayTable");
            tableTag.AddCssClass("table table-striped responsive-utilities jambo_table bulk_action");
            var headerTag = new TagBuilder("thead");
            var headerTrTag = new TagBuilder("tr");
            headerTrTag.AddCssClass("headings");

            Type modelType = pTableValues.First().GetType();
            var modelProperties = modelType.GetProperties();
            var tableModel = modelType.CustomAttributes.Where(i => i.AttributeType == typeof(TableAttribute)).ToList();
            var table = tableModel[0];

            foreach (var modelProperty in modelProperties)
            {
                var attrHeader = modelProperty.CustomAttributes.Where(i => i.AttributeType == typeof(TableColumnAttribute)).ToList();
                if (attrHeader.Count > 0)
                {
                    var column = attrHeader[0];
                    if (column.ConstructorArguments.Count == 1 || Convert.ToBoolean(column.ConstructorArguments[2].Value) == false)
                    {
                        var headerThTag = new TagBuilder("th");
                        headerThTag.AddCssClass("column-title");
                        headerThTag.SetInnerText(column.ConstructorArguments[0].Value.ToString());
                        headerTrTag.InnerHtml += headerThTag.ToString();
                    }
                }
            }
            headerTag.InnerHtml += headerTrTag.ToString();
            tableTag.InnerHtml += headerTag.ToString();

            var bodyTag = new TagBuilder("tbody");
            foreach (var tableValue in pTableValues)
            {
                var bodyTrTag = new TagBuilder("tr");
                bodyTrTag.AddCssClass("even pointer body");
                bodyTrTag.MergeAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(pHtmlRowAttributes));

                foreach (var modelProperty in modelProperties)
                {
                    var attrHeader = modelProperty.CustomAttributes.Where(i => i.AttributeType == typeof(TableColumnAttribute)).ToList();
                    if (attrHeader.Count > 0)
                    {
                        var column = attrHeader[0];
                        if (column.ConstructorArguments.Count >= 1 || Convert.ToBoolean(column.ConstructorArguments[2].Value) == false)
                        {
                            var bodyTdTag = new TagBuilder("td");
                            if (column.ConstructorArguments.Count > 1)
                            {
                                if (column.ConstructorArguments.Count > 1)
                                {
                                    if (Convert.ToBoolean(column.ConstructorArguments[1].Value))
                                        bodyTrTag.MergeAttribute("id", Convert.ToString(modelProperty.GetValue(tableValue)));
                                }
                            }
                            else
                            {
                                bodyTdTag.SetInnerText(Convert.ToString(modelProperty.GetValue(tableValue)));
                                if (Convert.ToBoolean(table.ConstructorArguments[0].Value))
                                {
                                    bodyTdTag.AddCssClass("clickable");
                                }
                                bodyTrTag.InnerHtml += bodyTdTag.ToString();
                            }
                        }
                    }
                }
                bodyTag.InnerHtml += bodyTrTag.ToString();
            }
            tableTag.InnerHtml += bodyTag.ToString();
            
            return MvcHtmlString.Create(tableTag.ToString());
        }
    }
}