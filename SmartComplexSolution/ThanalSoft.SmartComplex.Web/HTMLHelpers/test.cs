//using System;
//using System.Web;
//using System.Web.Mvc;


//public static IHtmlString GridFor<TModel>(this HtmlHelper<TModel> htmlHelper, String modelView, Type type)

//{




//    TagBuilder controlBuilder = new
//    TagBuilder(“table”);

//    controlBuilder.Attributes.Add(“style”, “border: 1px;”);


//    var properties = type.GetProperties();



//    #region Header


//    TagBuilder thead = new
//    TagBuilder(“thead”);


//    TagBuilder rowHeader = new
//    TagBuilder(“tr”);


//    foreach (var property in properties)

//    {


//        var attrHeader = property.CustomAttributes.Where(i => i.AttributeType == typeof(Common.Facade.GridColumnAttribute)).ToList();


//        if (attrHeader.Count != 0)

//        {


//            var attributeHeader = attrHeader[0];


//            if (Convert.ToBoolean(attributeHeader.ConstructorArguments[1].Value) == false)

//            {


//                TagBuilder col = new
//                TagBuilder(“td”);

//                col.InnerHtml = attributeHeader.ConstructorArguments[0].Value.ToString();

//                rowHeader.InnerHtml += col.ToString();

//            }

//        }

//    }

//    thead.InnerHtml += rowHeader.ToString();

//    controlBuilder.InnerHtml = thead.ToString();

//    #endregion



//    #region Rows and Columns




//    TagBuilder tbody = new
//    TagBuilder(“tbody”);

//    tbody.Attributes.Add(“data - bind”, “foreach: “ +modelView);

//    tbody.Attributes.Add(“style”, “width: 100”);


//    TagBuilder row = new
//    TagBuilder(“tr”);


//    foreach (var property in properties)

//    {


//        var attr = property.CustomAttributes.Where(i => i.AttributeType == typeof(Common.Facade.GridColumnAttribute)).ToList();


//        if (attr.Count != 0)

//        {


//            var attribute = attr[0];


//            if (Convert.ToBoolean(attribute.ConstructorArguments[1].Value) == false)

//            {


//                TagBuilder col = new
//                TagBuilder(“td”);

//                col.Attributes.Add(“data - bind”, “text: “ +property.Name);

//                row.InnerHtml += col.ToString();

//            }

//        }

//    }




//    TagBuilder editTd = new
//    TagBuilder(“td”);


//    TagBuilder editLink = new
//    TagBuilder(“a”);

//    editLink.Attributes.Add(“data - bind”, “attr: { href: EditLink}”);

//    editLink.InnerHtml = “Edit”;

//    editTd.InnerHtml += editLink.ToString();



//    row.InnerHtml += editTd.ToString();




//    TagBuilder deleteTd = new
//    TagBuilder(“td”);


//    TagBuilder deleteLink = new
//    TagBuilder(“a”);

//    deleteLink.Attributes.Add(“data - bind”, “attr: { href: DeleteLink}”);

//    deleteLink.InnerHtml = “Delete”;

//    deleteTd.InnerHtml += deleteLink.ToString();



//    row.InnerHtml += deleteTd.ToString();



//    tbody.InnerHtml += row.ToString();



//    controlBuilder.InnerHtml += tbody.ToString();

//    #endregion




//    return
//    MvcHtmlString.Create(controlBuilder.ToString());

//}

