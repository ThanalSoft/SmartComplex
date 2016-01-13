using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ThanalSoft.SmartComplex.Common.Attributes;

namespace ThanalSoft.SmartComplex.Web.HTMLHelpers
{
    public static class TableHelper
    {
        public static IHtmlString DisplayTableFor<TModel>(this HtmlHelper<TModel> pHtmlHelper, dynamic[] pTableValues, object pHtmlRowAttributes = null)
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
            var tableModel = modelType.CustomAttributes.Where(pI => pI.AttributeType == typeof(TableAttribute)).ToList();
            var table = tableModel[0];

            foreach (var modelProperty in modelProperties)
            {
                var attrHeader = modelProperty.CustomAttributes.Where(pI => pI.AttributeType == typeof(TableColumnAttribute)).ToList();
                if (attrHeader.Any())
                {
                    var column = attrHeader.First();
                    var isHidden = column.NamedArguments != null 
                                            && (column.NamedArguments.Any(pX => pX.MemberName.Equals("HiddenColumn")) 
                                            && Convert.ToBoolean(column.NamedArguments.FirstOrDefault(pX => pX.MemberName.Equals("HiddenColumn")).TypedValue.Value));
                    if (!isHidden)
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
                    var attrHeader = modelProperty.CustomAttributes.Where(pI => pI.AttributeType == typeof(TableColumnAttribute)).ToList();
                    if (attrHeader.Any())
                    {
                        var column = attrHeader.First();
                        var isHidden = column.NamedArguments != null
                                            && (column.NamedArguments.Any(pX => pX.MemberName.Equals("HiddenColumn"))
                                            && Convert.ToBoolean(column.NamedArguments.FirstOrDefault(pX => pX.MemberName.Equals("HiddenColumn")).TypedValue.Value));

                        var isIdCol = column.NamedArguments != null
                                            && (column.NamedArguments.Any(pX => pX.MemberName.Equals("IDColumn"))
                                            && Convert.ToBoolean(column.NamedArguments.FirstOrDefault(pX => pX.MemberName.Equals("IDColumn")).TypedValue.Value));

                        if (isHidden)
                        {
                            if(isIdCol)
                                bodyTrTag.MergeAttribute("id", Convert.ToString(modelProperty.GetValue(tableValue)));
                        }

                        if (!isHidden)
                        {
                            var bodyTdTag = new TagBuilder("td");
                            if (modelProperty.PropertyType == typeof (bool))
                            {
                                var value = Convert.ToBoolean(modelProperty.GetValue(tableValue));
                                if (value)
                                {
                                    bodyTdTag.AddCssClass("green");
                                    var boolTag = new TagBuilder("i");
                                    boolTag.AddCssClass("fa-check");
                                    boolTag.AddCssClass("fa");
                                    bodyTdTag.InnerHtml += boolTag.ToString();
                                }
                                else
                                {
                                    bodyTdTag.AddCssClass("red");
                                    var boolTag = new TagBuilder("i");
                                    boolTag.AddCssClass("fa-close");
                                    boolTag.AddCssClass("fa");
                                    bodyTdTag.InnerHtml += boolTag.ToString();
                                }
                            }
                            else
                            {
                                var value = Convert.ToString(modelProperty.GetValue(tableValue));
                                if (string.IsNullOrEmpty(value))
                                {
                                    var emptyValue = column.NamedArguments != null && column.NamedArguments.Any(pX => pX.MemberName.Equals("EmptyValue"))
                                        ? Convert.ToString(column.NamedArguments.FirstOrDefault(pX => pX.MemberName.Equals("EmptyValue")).TypedValue.Value)
                                        : String.Empty;

                                    if (!string.IsNullOrEmpty(emptyValue))
                                        value = emptyValue;
                                }

                                bodyTdTag.SetInnerText(value);
                            }
                            if (Convert.ToBoolean(table.ConstructorArguments[0].Value))
                                bodyTdTag.AddCssClass("clickable");
                            bodyTrTag.InnerHtml += bodyTdTag.ToString();
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