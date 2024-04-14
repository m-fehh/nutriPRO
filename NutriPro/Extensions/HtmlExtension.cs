using NutriPro.Application.Extensions;
using NutriPro.Application.Extensions.Exceptions;
using NutriPro.Application.Extensions.Search;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Extensions;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Text.Encodings.Web;
using System.Xml.Linq;

namespace NutriPro.Mvc.Extensions
{
    public static class HtmlExtension
    {
        public static string ToHtmlString(this IHtmlContent content)
        {
            var writer = new System.IO.StringWriter();
            content.WriteTo(writer, HtmlEncoder.Default);
            return writer.ToString();
        }

        /// <summary>
        /// Helper para label de aplicações do tipo portal
        /// </summary>
        public static IHtmlContent MLabelFor<TModel, TValue>(this IHtmlHelper<TModel> helper, Expression<Func<TModel, TValue>> expression, IDictionary<string, object> htmlAttributes = null, string labelTitle = "")
        {
            var label = new TagBuilder("label");
            label.MergeAttribute("for", helper.IdFor(expression));
            label.AddCssClass("input-label");

            var innerContent = new StringBuilder();

            if (!string.IsNullOrEmpty(labelTitle))
            {
                innerContent.Append(labelTitle);
            }
            else
            {
                innerContent.Append(helper.DisplayNameFor(expression));
            }

            if (expression.IsRequired())
            {
                innerContent.Append("<span class=\"input-label-required\"></span>");
            }

            label.InnerHtml.AppendHtml(innerContent.ToString());

            return new HtmlContentBuilder().AppendHtml(label);
        }




        /// <summary>
        /// Helper para label de aplicações do tipo portal
        /// </summary>
        public static IHtmlContent MLabel(this IHtmlHelper helper, string name, IDictionary<string, object> htmlAttributes = null)
        {
            StringBuilder classe = new StringBuilder("control-label");

            var attributes = new Dictionary<string, object>();

            attributes.Add("class", classe);

            if (htmlAttributes != null)
            {
                htmlAttributes.ToList().ForEach(dic =>
                {
                    if (attributes.ContainsKey(dic.Key))
                    {
                        attributes.Remove(dic.Key);
                    }
                });

                attributes = attributes.Union(htmlAttributes).ToDictionary(g => g.Key, g => g.Value);
            }

            return helper.Label("", name, attributes);
        }

        public static IHtmlContent MNumericBox(this IHtmlHelper helper, string name, IDictionary<string, object> htmlAttributes = null)
        {
            var attributes = new Dictionary<string, object>();

            attributes.Add("class", "form-control auto-numeric");

            if (htmlAttributes != null)
            {
                htmlAttributes.ToList().ForEach(dic =>
                {
                    if (attributes.ContainsKey(dic.Key))
                    {
                        attributes.Remove(dic.Key);
                    }
                });

                attributes = attributes.Union(htmlAttributes).ToDictionary(g => g.Key, g => g.Value);
            }

            if (htmlAttributes == null)
            {
                htmlAttributes = new Dictionary<string, object>();
            }

            return helper.MTextBox(name, attributes);
        }

        public static IHtmlContent MTextBox(this IHtmlHelper helper, string name, IDictionary<string, object> htmlAttributes = null)
        {
            var classe = new StringBuilder("form-control");

            var attributes = new Dictionary<string, object>();

            classe.Append(" toupper");

            attributes.Add("class", classe);

            attributes.Add("autocomplete", "off");

            if (htmlAttributes != null)
            {
                htmlAttributes.ToList().ForEach(dic =>
                {
                    if (attributes.ContainsKey(dic.Key))
                    {
                        attributes.Remove(dic.Key);
                    }
                });

                attributes = attributes.Union(htmlAttributes).ToDictionary(g => g.Key, g => g.Value);
            }

            return helper.TextBox(name, null, attributes);
        }

        public static Type GetObjectType<TModel, TValue>(Expression<Func<TModel, TValue>> expr)
        {
            if ((expr.Body.NodeType == ExpressionType.Convert) ||
                (expr.Body.NodeType == ExpressionType.ConvertChecked))
            {
                var unary = expr.Body as UnaryExpression;
                if (unary != null)
                    return unary.Operand.Type;
            }
            return expr.Body.Type;
        }

        /// <summary>
        /// Helper para textbox de aplicações do tipo portal
        /// </summary>
        public static IHtmlContent MTextBoxFor<TModel, TValue>(this IHtmlHelper<TModel> helper, Expression<Func<TModel, TValue>> expression, IDictionary<string, object> htmlAttributes = null, bool? upperCase = true)
        {
            StringBuilder classe = new StringBuilder("form-control");

            var attributes = new Dictionary<string, object>();

            classe.Append(" toupper");

            var returnType = GetObjectType(expression);

            if (returnType.IsNumericType())
            {
                classe.Append(" form-numeric");
            }

            attributes.Add("class", classe);

            attributes.Add("autocomplete", "off");

            if (htmlAttributes != null)
            {
                htmlAttributes.ToList().ForEach(dic =>
                {
                    if (attributes.ContainsKey(dic.Key))
                    {
                        attributes.Remove(dic.Key);
                    }
                });

                attributes = attributes.Union(htmlAttributes).ToDictionary(g => g.Key, g => g.Value);
            }

            return helper.TextBoxFor(expression, attributes);
        }

        /// <summary>
        /// Helper para password de aplicações do tipo portal
        /// </summary>
        public static IHtmlContent MPasswordFor<TModel, TValue>(this IHtmlHelper<TModel> helper, Expression<Func<TModel, TValue>> expression, IDictionary<string, object> htmlAttributes = null)
        {
            StringBuilder classe = new StringBuilder("form-control col-md-12");

            var attributes = new Dictionary<string, object>();

            attributes.Add("class", classe);

            if (htmlAttributes != null)
            {
                htmlAttributes.ToList().ForEach(dic =>
                {
                    if (attributes.ContainsKey(dic.Key))
                    {
                        attributes.Remove(dic.Key);
                    }
                });

                attributes = attributes.Union(htmlAttributes).ToDictionary(g => g.Key, g => g.Value);
            }

            return helper.PasswordFor(expression, attributes);
        }

        /// <summary>
        /// Helper para textarea de aplicações do tipo portal
        /// </summary>
        public static IHtmlContent MTextArea(this IHtmlHelper helper, string name, IDictionary<string, object> htmlAttributes = null)
        {
            StringBuilder classe = new StringBuilder("form-control");

            var attributes = new Dictionary<string, object>();

            classe.Append(" toupper");

            attributes.Add("class", classe);

            if (htmlAttributes != null)
            {
                htmlAttributes.ToList().ForEach(dic =>
                {
                    if (attributes.ContainsKey(dic.Key))
                    {
                        attributes.Remove(dic.Key);
                    }
                });

                attributes = attributes.Union(htmlAttributes).ToDictionary(g => g.Key, g => g.Value);
            }

            return helper.TextArea(name, null, attributes);
        }


        /// <summary>
        /// Helper para textarea de aplicações do tipo portal
        /// </summary>
        public static IHtmlContent MTextAreaFor<TModel, TValue>(this IHtmlHelper<TModel> helper, Expression<Func<TModel, TValue>> expression, IDictionary<string, object> htmlAttributes = null)
        {
            StringBuilder classe = new StringBuilder("form-control");

            var attributes = new Dictionary<string, object>();

            classe.Append(" toupper");

            attributes.Add("class", classe);

            if (htmlAttributes != null)
            {
                htmlAttributes.ToList().ForEach(dic =>
                {
                    if (attributes.ContainsKey(dic.Key))
                    {
                        attributes.Remove(dic.Key);
                    }
                });

                attributes = attributes.Union(htmlAttributes).ToDictionary(g => g.Key, g => g.Value);
            }

            return helper.TextAreaFor(expression, attributes);
        }


        /// <summary>
        /// Helper para textarea do tipo ckeditor de aplicações do tipo portal
        /// </summary>
        public static IHtmlContent MCKEditorFor<TModel, TValue>(this IHtmlHelper<TModel> helper, Expression<Func<TModel, TValue>> expression, IDictionary<string, object> htmlAttributes = null)
        {
            StringBuilder classe = new StringBuilder("ckeditor");

            var attributes = new Dictionary<string, object>();

            attributes.Add("class", classe);

            if (htmlAttributes != null)
            {
                htmlAttributes.ToList().ForEach(dic =>
                {
                    if (attributes.ContainsKey(dic.Key))
                    {
                        attributes.Remove(dic.Key);
                    }
                });

                attributes = attributes.Union(htmlAttributes).ToDictionary(g => g.Key, g => g.Value);
            }

            return helper.TextAreaFor(expression, attributes);
        }
        /// <summary>
        /// Helper para datepickers de aplicações do tipo portal
        /// </summary>
        public static IHtmlContent MDatePicker(this IHtmlHelper helper, string name, string format = "{0:dd/MM/yyyy}")
        {
            StringBuilder html = new StringBuilder("<div class=\"input-icon right date date-picker\" data-date-format=\"dd/mm/yyyy\" data-date-viewmode=\"years\">");

            html.Append("<span class=\"add-on\"></span>");

            html.Append("<i class=\"icon-calendar\"></i>");

            html.Append(helper.TextBox(name, null, format, new { @class = "form-control date date-picker", autocomplete = "off" }).ToHtmlString());

            html.Append("</div>");

            return new HtmlString(html.ToString());
        }

        /// <summary>
        /// Helper para datepickers de aplicações do tipo portal
        /// Obs.: somente um dentre os paramêtros (futureOnly, pastOnly ou futureWithOutCurrentDate) deve possuir valor igual a true
        /// para que sejam aplicados corretamente os bloqueios na datas conforme necessidade.
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="helper"></param>
        /// <param name="expression"></param>
        /// <param name="format">Permite informar em qual formato a data deve ser apresentada</param>
        /// <param name="futureOnly">Booleano para permitir que selecionem apenas a data atual ou futuras</param>
        /// <param name="pastOnly">Booleano para permitir que selecionem apenas a data atual e as datas anteriores à data de execução da aplicação</param>
        /// <param name="futureWithOutCurrentDate">Booleano para permitir que selecionem apenas datas posteriores à data de execução da aplicação</param>
        /// <returns></returns>
        public static IHtmlContent MDatePickerFor<TModel, TValue>(this IHtmlHelper<TModel> helper, Expression<Func<TModel, TValue>> expression, string format = "{0:dd/MM/yyyy}", bool futureOnly = false, bool pastOnly = false, bool futureWithOutCurrentDate = false)
        {
            StringBuilder html = new StringBuilder("<div class=\"input-icon right date date-picker"
                                                    + (futureOnly ? " future-only" : pastOnly ? " past-only" : futureWithOutCurrentDate ? " future-without-current-date" : "")
                                                    + "\" data-date-format=\"dd/mm/yyyy\" data-date-viewmode=\"years\">");

            html.Append("<span class=\"add-on\"></span>");

            html.Append("<i class=\"icon-calendar\"></i>");

            html.Append(helper.TextBoxFor(expression, format, new { @class = "form-control date date-picker", autocomplete = "off" }).ToHtmlString());

            html.Append("</div>");

            return new HtmlString(html.ToString());
        }

        /// <summary>
        /// Helper para monthpickers de aplicações do tipo portal
        /// </summary>
        public static IHtmlContent MMonthPicker(this IHtmlHelper helper, string name, bool futureOnly = false, bool pastOnly = false)
        {
            StringBuilder html = new StringBuilder("<div class=\"input-icon right date date-month-picker" + (futureOnly ? " future-only" : "") + (pastOnly ? " past-only" : "") + "\" data-date-format=\"mm/yyyy\" data-date-viewmode=\"months\">");

            html.Append("<span class=\"add-on\"></span>");

            html.Append("<i class=\"icon-calendar\"></i>");

            html.Append(helper.TextBox(name, "", new { @class = "form-control date date-month-picker", autocomplete = "off" }).ToHtmlString());

            html.Append("</div>");

            return new HtmlString(html.ToString());
        }


        /// <summary>
        /// Helper para monthpickers de aplicações do tipo portal
        /// </summary>
        public static IHtmlContent MMonthPickerFor<TModel, TValue>(this IHtmlHelper<TModel> helper, Expression<Func<TModel, TValue>> expression, bool futureOnly = false, bool pastOnly = false)
        {
            StringBuilder html = new StringBuilder("<div class=\"input-icon right date date-month-picker" + (futureOnly ? " future-only" : "") + (pastOnly ? " past-only" : "") + "\" data-date-format=\"mm/yyyy\" data-date-viewmode=\"months\">");

            html.Append("<span class=\"add-on\"></span>");

            html.Append("<i class=\"icon-calendar\"></i>");

            html.Append(helper.TextBoxFor(expression, "{0:MM/yyyy}", new { @class = "form-control date date-month-picker", autocomplete = "off" }).ToHtmlString());

            html.Append("</div>");

            return new HtmlString(html.ToString());
        }

        /// <summary>
        /// Helper para datetimepickers de aplicações do tipo portal
        /// </summary>
        public static IHtmlContent MDateTimePickerFor<TModel, TValue>(this IHtmlHelper<TModel> helper, Expression<Func<TModel, TValue>> expression, string format = "{0:dd/MM/yyyy HH:mm}")
        {
            StringBuilder html = new StringBuilder("<div class=\"input-icon right\">");

            html.Append("<span class=\"add-on\"></span>");

            html.Append("<i class=\"icon-clock\"></i>");

            html.Append(helper.TextBoxFor(expression, format, new { @class = "form-control date datetime-picker", autocomplete = "off" }).ToHtmlString());

            html.Append("</div>");

            return new HtmlString(html.ToString());
        }

        /// <summary>
        /// Helper para datetimepickers de aplicações do tipo portal
        /// </summary>
        public static IHtmlContent MTimePickerFor<TModel, TValue>(this IHtmlHelper<TModel> helper, Expression<Func<TModel, TValue>> expression)
        {
            StringBuilder html = new StringBuilder("<div class=\"input-icon right\">");

            html.Append("<span class=\"add-on\"></span>");

            html.Append("<i class=\"icon-clock\"></i>");

            html.Append(helper.TextBoxFor(expression, new { @class = "form-control time-picker", autocomplete = "off" }).ToHtmlString());

            html.Append("</div>");

            return new HtmlString(html.ToString());
        }

        /// <summary>
        /// Helper para checkbox de aplicações do tipo portal
        /// </summary>
        public static IHtmlContent MCheckBox(this IHtmlHelper helper, string name, bool isChecked = false, IDictionary<string, object> htmlAttributes = null)
        {
            StringBuilder html = new StringBuilder();

            var attributes = new Dictionary<string, object>();

            StringBuilder classe = new StringBuilder("switch");

            attributes.Add("class", classe);

            if (isChecked)
            {
                attributes.Add("checked", isChecked);
            }

            if (htmlAttributes != null)
            {
                htmlAttributes.ToList().ForEach(dic =>
                {
                    if (attributes.ContainsKey(dic.Key))
                    {
                        attributes.Remove(dic.Key);
                    }
                });

                attributes = attributes.Union(htmlAttributes).ToDictionary(g => g.Key, g => g.Value);
            }

            html.Append(helper.CheckBox(name, attributes).ToHtmlString());

            return new HtmlString(html.ToString());
        }

        public static IHtmlContent MLegendIcon(this IHtmlHelper helper, string legendText, string placement = "top")
        {
            StringBuilder html = new StringBuilder($"<span style=\"white-space: normal\"><i class=\"fa fa-info-circle legend-tooltip\" data-html=\"true\" data-placement=\"{placement.Replace("\"", "")}\" data-original-title=\"{legendText.Replace("\"", "")}\"></i></span>");

            return new HtmlString(html.ToString());
        }

        /// <summary>
        /// Helper para checkbox de aplicações do tipo portal
        /// </summary>
        public static IHtmlContent MCheckBoxFor<TModel>(this IHtmlHelper<TModel> helper, Expression<Func<TModel, bool>> expression)
        {
            StringBuilder html = new StringBuilder(helper.CheckBoxFor(expression, new { @class = "switch" }).ToHtmlString());

            return new HtmlString(html.ToString());
        }

        /// <summary>
        /// Helper para buscadores de aplicações do tipo portal
        /// </summary>
        public static IHtmlContent MSearch(this IHtmlHelper helper, string fieldName, string searchName, string extraCondition = null, string referenceNameInput = null, bool registration = true, IDictionary<string, object> htmlAttributes = null)
        {
            var item = FindInSearchCollection(searchName);

            var classes = "form-control col-md-12 Voeit-search";

            var attributes = new Dictionary<string, object>();

            attributes.Add("searchName", searchName);
            attributes.Add("extraCondition", extraCondition);
            attributes.Add("referenceNameInput", referenceNameInput);

            var noFreeSearch = item.GetType().GetCustomAttributes(typeof(NoFreeSearchAttribute)).Count() > 0;

            if (noFreeSearch)
            {
                classes += " noFreeSearch";
            }

            if (item is ISearchItemRegistration && registration == true)
            {
                classes += " Voeit-search-register";

                attributes.Add("registerName", (item as ISearchItemRegistration).RegistrationName);
                attributes.Add("registerAddress", (item as ISearchItemRegistration).RegistrationAddress);
            }

            attributes.Add("class", classes);

            if (htmlAttributes != null)
            {
                htmlAttributes.ToList().ForEach(dic =>
                {
                    if (attributes.ContainsKey(dic.Key))
                    {
                        attributes.Remove(dic.Key);
                    }
                });

                attributes = attributes.Union(htmlAttributes).ToDictionary(g => g.Key, g => g.Value);
            }

            return helper.Hidden(fieldName, null, attributes);
        }

        /// <summary>
        /// Helper para buscadores de aplicações do tipo portal
        /// </summary>
        public static IHtmlContent MSearchFor<TModel, TValue>(this IHtmlHelper<TModel> helper, Expression<Func<TModel, TValue>> expression, string searchName, string extraCondition = null, string referenceNameInput = null)
        {
            var item = FindInSearchCollection(searchName);

            var classes = "form-control col-md-12 Voeit-search";

            var attributes = new Dictionary<string, object>();

            attributes.Add("searchName", searchName);
            attributes.Add("extraCondition", extraCondition);
            attributes.Add("referenceNameInput", referenceNameInput);

            var noFreeSearch = item.GetType().GetCustomAttributes(typeof(NoFreeSearchAttribute)).Count() > 0;

            if (noFreeSearch)
            {
                classes += " noFreeSearch";
            }

            if (item is ISearchItemRegistration)
            {
                classes += " Voeit-search-register";

                attributes.Add("registerName", (item as ISearchItemRegistration).RegistrationName);
                attributes.Add("registerAddress", (item as ISearchItemRegistration).RegistrationAddress);
            }

            attributes.Add("class", classes);

            return helper.HiddenFor(expression, attributes);
        }

        /// <summary>
        /// Helper para combobox com valores estáticos
        /// </summary>
        public static IHtmlContent MCombo(this IHtmlHelper helper, string name, IEnumerable<SelectListItem> values, bool multiple = false, IDictionary<string, object> htmlAttributes = null)
        {
            StringBuilder classe = new StringBuilder("form-control col-md-12 voeit-combobox");

            var attributes = new Dictionary<string, object>();

            attributes.Add("class", classe);

            if (htmlAttributes != null)
            {
                htmlAttributes.ToList().ForEach(dic =>
                {
                    if (attributes.ContainsKey(dic.Key))
                    {
                        attributes.Remove(dic.Key);
                    }
                });

                attributes = attributes.Union(htmlAttributes).ToDictionary(g => g.Key, g => g.Value);
            }

            if (multiple)
            {
                attributes.Add("multiple", "");

                return helper.ListBox(name, values, attributes);
            }
            else
            {
                return helper.DropDownList(name, values, attributes);
            }
        }

        public static IHtmlContent MEnumComboFor<TModel, TValue>(this IHtmlHelper<TModel> helper, Expression<Func<TModel, TValue>> expression, string optionLabel = null)
            where TModel : struct
        {
            StringBuilder classe = new StringBuilder("form-control col-md-12 voeit-combobox");

            var attributes = new Dictionary<string, object>();

            attributes.Add("class", classe);

            if (!string.IsNullOrEmpty(optionLabel))
            {
                return helper.DropDownListFor(expression, helper.GetEnumSelectList<TModel>(), optionLabel, attributes);
            }
            else
            {
                return helper.DropDownListFor(expression, helper.GetEnumSelectList<TModel>(), attributes);
            }
        }

        public static List<SelectListItem> GetEnumSelectList<TEnum>() where TEnum : Enum
        {
            return Enum.GetValues(typeof(TEnum))
                .Cast<TEnum>()
                .Select(e => new SelectListItem { Text = e.GetDisplayName(), Value = e.ToString() })
                .ToList();
        }


        /// <summary>
        /// Helper para combobox com valores estáticos
        /// </summary>
        public static IHtmlContent MComboFor<TModel, TValue>(this IHtmlHelper<TModel> helper, Expression<Func<TModel, TValue>> expression, IEnumerable<SelectListItem> values, bool multiple = false, string optionLabel = null, IDictionary<string, object> htmlAttributes = null)
        {
            StringBuilder classe = new StringBuilder("form-control");

            var attributes = new Dictionary<string, object>();

            attributes.Add("class", classe);

            if (htmlAttributes != null)
            {
                htmlAttributes.ToList().ForEach(dic =>
                {
                    if (attributes.ContainsKey(dic.Key))
                    {
                        attributes.Remove(dic.Key);
                    }
                });

                attributes = attributes.Union(htmlAttributes).ToDictionary(g => g.Key, g => g.Value);
            }

            if (multiple)
            {
                attributes.Add("multiple", "");

                return helper.ListBoxFor(expression, values, attributes);
            }
            else if (!string.IsNullOrEmpty(optionLabel))
            {
                return helper.DropDownListFor(expression, values, optionLabel, attributes);
            }
            else
            {
                return helper.DropDownListFor(expression, values, attributes);
            }
        }

        public static IHtmlContent MSubmitButton<TModel>(this IHtmlHelper<TModel> helper, string buttonText, string cssClass = "btn btn-success", bool isFloatRight = false)
        {
            TagBuilder button = new TagBuilder("button");
            button.AddCssClass(cssClass);
            button.MergeAttribute("type", "submit");
            button.InnerHtml.Append(buttonText);

            TagBuilder div = new TagBuilder("div");
            div.AddCssClass("col-md-12");

            if (isFloatRight)
            {
                div.AddCssClass("float-right");
            }

            div.InnerHtml.AppendHtml(button);

            return div;
        }


        public static IHtmlContent MComboFor<TModel, TValue>(this IHtmlHelper<TModel> helper, Expression<Func<TModel, TValue>> expression, string searchName, bool multiple = false, string extraCondition = null)
        {
            var item = FindInSearchCollection(searchName);

            var list = item.Get("", int.MaxValue, 1, extraCondition).Cast<SearchResult>().Select(i => new SelectListItem() { Text = i.text, Value = i.id.ToString() }).ToList();

            list.Insert(0, new SelectListItem());

            return MComboFor(helper, expression, list, multiple);
        }

        private static ISearchItem FindInSearchCollection(string searchName)
        {
            var searchItemRepository = new SearchItemRepository();
            var item = searchItemRepository.FindSearchItemByName(searchName);

            if (item == null) throw new BusinessException("Regra de busca não implementada");

            return item;
        }


        /// <summary>
        /// Cria um validation summary customizado para as aplicações que usam o modelo-portal.
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="excludePropertyErrors"></param>
        /// <returns></returns>
        public static IHtmlContent MValidationSummary(this IHtmlHelper helper, bool excludePropertyErrors)
        {
            var element = helper.ValidationSummary(excludePropertyErrors, (string)null, new { @class = "note note-warning" });

            var htmlString = element.ToHtmlString();

            if (!string.IsNullOrEmpty(htmlString))
            {
                XElement xEl = XElement.Parse(htmlString);

                var lis = xEl.Element("ul").Elements("li");

                if (lis.Count() == 1 && lis.First().Value == "")
                    return null;
            }

            return element;
        }

        public static IHtmlContent MMultiSelect(this IHtmlHelper helper, string name, string dataSourceUrl, bool startWithSelectedOnly = false)
        {
            var id = $"{name}{Guid.NewGuid()}";
            return helper.Hidden(name, new { @id = id, @Name = name, @class = "NutriPro-multiSelect", data_Source_Url = dataSourceUrl, data_start_selected_only = startWithSelectedOnly });
        }

        /// <summary>
        /// Helper para criação de campo para seleção de domínios virtuais
        /// </summary>
        public static IHtmlContent MMultiSelectFor<TModel, TValue>(this IHtmlHelper<TModel> helper, Expression<Func<TModel, TValue>> expression, string dataSourceUrl, bool startWithSelectedOnly = false)
        {
            var name = ((expression.Body as MemberExpression).Member as PropertyInfo).Name;

            var id = $"{name}{Guid.NewGuid()}";
            return helper.HiddenFor(expression, new { @id = id, @Name = name, @class = "NutriPro-multiSelect", data_Source_Url = dataSourceUrl, data_start_selected_only = startWithSelectedOnly });
        }


        /// <summary>
        /// Helper para criar uma div-container de controles de aplicações do tipo portal
        /// </summary>
        public static IDisposable MControlGroup<TModel, TValue>(this IHtmlHelper<TModel> helper, Expression<Func<TModel, TValue>> expression, bool createLabel = true, string extra_class = "")
        {
            return new MControlGroup<TModel, TValue>(helper, expression, createLabel, extra_class);
        }

        /// <summary>
        /// Helper para criar uma div-container de controles de aplicações do tipo portal
        /// </summary>
        public static IDisposable MControlGroup(this IHtmlHelper helper, string name, bool createLabel = true, string extra_class = "")
        {
            return new MControlGroup(helper, name, createLabel, extra_class);
        }

        /// <summary>
        /// retorna o htmlstring utilizado para criação do cabeçalho de um controle de grupo
        /// </summary>
        public static IHtmlContent MControlGroupHeader<TModel, TValue>(this IHtmlHelper<TModel> helper, Expression<Func<TModel, TValue>> expression, bool createLabel = true, string labelTitle = "", string extra_class = "")
        {
            var name = helper.NameFor(expression).ToString();

            var errorsCount = helper.ViewData.ModelState[name]?.Errors?.Count ?? 0;

            StringBuilder html = new StringBuilder("<div class=\"form-group " + extra_class + (errorsCount == 0 ? null : " has-error") + "\">");

            if (createLabel)
            {
                if (string.IsNullOrEmpty(labelTitle))
                {
                    html.Append(helper.MLabelFor(expression).ToHtmlString());
                }
                else
                {
                    html.Append(helper.MLabelFor(expression, null, labelTitle).ToHtmlString());
                }
            }

            return new HtmlString(html.ToString());
        }

        public static IHtmlContent MControlGroupHeader(this IHtmlHelper helper, string name, bool createLabel = true, string labelTitle = "", string extra_class = "")
        {
            StringBuilder html = new StringBuilder("<div class=\"form-group " + extra_class + "\">");

            if (createLabel)
            {
                if (string.IsNullOrEmpty(labelTitle))
                    html.Append(helper.MLabel(name).ToHtmlString());
            }

            return new HtmlString(html.ToString());
        }

        /// <summary>
        /// retorna o htmlstring utilizado para criação do rodapé de um controle de grupo
        /// </summary>
        public static IHtmlContent MControlGroupFooter<TModel, TValue>(this IHtmlHelper<TModel> helper, Expression<Func<TModel, TValue>> expression)
        {
            StringBuilder html = new StringBuilder(helper.ValidationMessageFor(expression, null, new { @class = "help-block" }).ToHtmlString());

            html.Append("</div>");

            return new HtmlString(html.ToString());
        }

        /// <summary>
        /// retorna o htmlstring utilizado para criação do rodapé de um controle de grupo
        /// </summary>
        public static IHtmlContent MControlGroupFooter(this IHtmlHelper helper)
        {
            return new HtmlString("</div>");
        }

        /// <summary>
        /// Helper para radiobuttons de aplicações do tipo portal
        /// </summary>
        public static IHtmlContent MRadioButtonFor<TModel, TValue>(this IHtmlHelper<TModel> helper, Expression<Func<TModel, TValue>> expression, object value)
        {
            StringBuilder html = new StringBuilder();

            html.Append(helper.RadioButtonFor(expression, value).ToHtmlString());

            return new HtmlString(html.ToString());
        }
    }

    public class MControlGroup<TModel, TValue> : IDisposable
    {
        private readonly IHtmlHelper<TModel> _helper;
        private readonly Expression<Func<TModel, TValue>> _expression;

        public MControlGroup(IHtmlHelper<TModel> helper, Expression<Func<TModel, TValue>> expression, bool createLabel = true, string extra_class = "")
        {
            _helper = helper;

            _expression = expression;

            _helper.ViewContext.Writer.WriteLine(_helper.MControlGroupHeader(expression, createLabel, "", extra_class));
        }

        public void Dispose()
        {
            _helper.ViewContext.Writer.WriteLine(_helper.MControlGroupFooter(_expression));
        }
    }

    public class MControlGroup : IDisposable
    {
        private readonly IHtmlHelper _helper;

        public MControlGroup(IHtmlHelper helper, string name, bool createLabel = true, string extra_class = "")
        {
            _helper = helper;

            _helper.ViewContext.Writer.WriteLine(_helper.MControlGroupHeader(name, createLabel, "", extra_class));
        }

        public void Dispose()
        {
            _helper.ViewContext.Writer.WriteLine(_helper.MControlGroupFooter());
        }
    }
}
