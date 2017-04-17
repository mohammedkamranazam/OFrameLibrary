using OFrameLibrary.Helpers;
using OFrameLibrary.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace OFrameLibrary.Util
{
    public static class Extensions
    {
        public static string Base64Decode(this string encodedText)
        {
            return Encoding.UTF8.GetString(Convert.FromBase64String(encodedText));
        }

        public static string Base64Encode(this string text)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(text));
        }

        public static void DeleteFile(this string fileName)
        {
            fileName = fileName.MapPath();

            if (File.Exists(fileName))
            {
                try
                {
                    File.Delete(fileName);
                }
                catch (Exception ex)
                {
                    ErrorLogger.LogError(ex);
                }
            }
        }

        public static List<OListItem> EnumToList(this Type type)
        {
            var list = new List<OListItem>();

            foreach (FieldInfo field in type.GetFields(BindingFlags.DeclaredOnly | BindingFlags.Static | BindingFlags.Public | BindingFlags.GetField))
            {
                var rawConstantValue = field.GetRawConstantValue();

                list.Add(new OListItem
                {
                    Text = GetDisplayName(field),
                    Value = rawConstantValue.ToString()
                });
            }

            return list;
        }

        public static IEnumerable<T> FlattenHierarchy<T>(this T node, Func<T, IEnumerable<T>> getChildEnumerator)
        {
            yield return node;
            if (getChildEnumerator?.Invoke(node) != null)
            {
                foreach (var child in getChildEnumerator?.Invoke(node))
                {
                    foreach (var childOrDescendant
                              in child.FlattenHierarchy(getChildEnumerator))
                    {
                        yield return childOrDescendant;
                    }
                }
            }
        }

        public static string GenerateSlug(this string phrase)
        {
            var str = phrase.RemoveAccent().ToLower();

            str = Regex.Replace(str, @"[^a-z0-9\s-]", ""); // invalid chars
            str = Regex.Replace(str, @"\s+", " ").Trim(); // convert multiple spaces into one space
            str = str.Substring(0, str.Length <= 45 ? str.Length : 45).Trim(); // cut and trim it
            str = Regex.Replace(str, @"\s", "-"); // hyphens

            return str;
        }

        public static string GetAbsolutePathFromRelativePath(this string RelativePath)
        {
            var extraPath = string.Empty;

            if (string.IsNullOrWhiteSpace(RelativePath))
            {
                extraPath = string.Empty;
            }
            else if (RelativePath.Substring(0, 1) != "~")
            {
                extraPath = RelativePath;
            }
            else
            {
                extraPath = RelativePath.Remove(0, 1);
            }

            return string.Format("http{0}://{1}{2}{3}",
            (HttpContext.Current.Request.IsSecureConnection) ? "s" : "",
            HttpContext.Current.Request.Url.Host,
            (HttpContext.Current.Request.Url.Port != 80) ? string.Format(":{0}", HttpContext.Current.Request.Url.Port) : "",
            extraPath);
        }

        public static string GetAbsoluteURL(this string relativeURL)
        {
            var url = string.Empty;

            if (!string.IsNullOrWhiteSpace(relativeURL))
            {
                if (relativeURL.Substring(0, 1) == "~")
                {
                    url = relativeURL.Remove(0, 1);
                }
                else
                {
                    url = relativeURL;
                }
            }

            return string.Format("http{0}://{1}{2}{3}",
            (HttpContext.Current.Request.IsSecureConnection) ? "s" : "",
            HttpContext.Current.Request.Url.Host,
            (HttpContext.Current.Request.Url.Port != 80) ? string.Format(":{0}", HttpContext.Current.Request.Url.Port) : "",
            url);
        }

        public static string GetDisplayName(FieldInfo field)
        {
            var customAttribute = CustomAttributeExtensions.GetCustomAttribute<DisplayAttribute>((MemberInfo)field, false);
            if (customAttribute != null)
            {
                var name = customAttribute.GetName();
                if (!string.IsNullOrEmpty(name))
                {
                    return name;
                }
            }
            return field.Name;
        }

        /// <summary>
        /// Gets the SelectListItem Collection from the enum.
        /// </summary>
        /// <param name="type">Enum Type</param>
        /// <param name="takeValue">if set to <c>true</c> the value of the SelectListItem will be the enum integer value.</param>
        /// <param name="friendlyValue">if set to <c>true</c> the value of the SelectListItem will be space separated.</param>
        /// <param name="selectedValue">The default selected value in the SelectListItem collection</param>
        /// <param name="insertSelectOption">if set to <c>true</c> the first item will be the Select prompt.</param>
        /// <param name="selectOptionSelected">if set to <c>true</c> the Select prompt will be selected by default.</param>
        /// <param name="selectOptionLabel">The label of the Select prompt</param>
        /// <param name="selectOptionValue">The value of the Select prompt</param>
        /// <param name="isSelectOptionDisabled">if set to <c>true</c> the Select prompt will be disabled and unelectable.</param>
        /// <param name="friendlyText">todo: describe friendlyText parameter on GetEnumList</param>
        /// <param name="translate">todo: describe translate parameter on GetEnumList</param>
        /// <param name="locale">todo: describe locale parameter on GetEnumList</param>
        /// <param name="isTextTranslatable">todo: describe isTextTranslatable parameter on GetEnumList</param>
        /// <returns></returns>
        public static List<OListItem> GetEnumList(this Type type,
               bool takeValue = false,
               bool friendlyText = true,
               bool friendlyValue = false,
               string selectedValue = null,
               bool insertSelectOption = false,
               bool selectOptionSelected = true,
               string selectOptionLabel = "-- Select --",
               string selectOptionValue = null,
               bool isSelectOptionDisabled = true,
               bool translate = false,
               string locale = "en-US",
               bool isTextTranslatable = true)
        {
            return Utilities.GetSelectList(EnumToList(type),
                takeValue,
                friendlyText,
                friendlyValue,
                selectedValue,
                insertSelectOption,
                selectOptionSelected,
                selectOptionLabel,
                selectOptionValue,
                isSelectOptionDisabled,
                translate,
                locale,
                isTextTranslatable);
        }

        public static List<T> GetIDList<T>(this string ids, string separator = ";")
        {
            if (string.IsNullOrWhiteSpace(ids))
            {
                return null;
            }

            return ids.Split(new string[] { separator }, StringSplitOptions.RemoveEmptyEntries).Select(c => (T)Convert.ChangeType(c, typeof(T))).ToList();
        }

        public static string GetProfilePic(this Gender gender)
        {
            if (gender == Gender.Male)
            {
                return AppConfig.MaleAvatar;
            }

            if (gender == Gender.Female)
            {
                return AppConfig.FemaleAvatar;
            }

            return AppConfig.UnspecifiedAvatar;
        }

        public static string Join<T>(this T[] collection, string delimeter = ";")
        {
            return string.Join(delimeter, collection) + delimeter;
        }

        public static string Join<T>(this List<T> collection, string delimeter = ";")
        {
            return string.Join(delimeter, collection) + delimeter;
        }

        public static string MapPath(this string path)
        {
            return HttpContext.Current.Server.MapPath(path);
        }

        public static bool NullableContains(this string text, string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return false;
            }

            return text.IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) >= 0;
        }

        //public static bool IsNullSelected(this DropDownListAdv dropDownList)
        //{
        //    if (GetNullableSelectedValue(dropDownList) == null)
        //    {
        //        return false;
        //    }
        //    else
        //    {
        //        return true;
        //    }
        //}
        public static int NullReverser(this int? value)
        {
            if (value == null)
            {
                return -1;
            }
            else
            {
                return (int)value;
            }
        }

        public static string RemoveAccent(this string txt)
        {
            var bytes = Encoding.GetEncoding("Cyrillic").GetBytes(txt);
            return Encoding.ASCII.GetString(bytes);
        }

        public static string ToFriendlyCase(this string EnumString)
        {
            return Regex.Replace(EnumString, "(?!^)([A-Z])", " $1");
        }

        public static IDUpdater<T> UpdatedIDs<T>(this IDUpdater<T> updaterModel)
        {
            updaterModel.AddIDs = updaterModel.NewIDs.Where(c => !updaterModel.OldIDs.Contains(c)).ToList();
            updaterModel.RemoveIDs = updaterModel.OldIDs.Where(c => !updaterModel.NewIDs.Contains(c)).ToList();

            return updaterModel;
        }

        public static JsonValidationResult<T> ValidateJsonModel<T>(this string json)
        {
            var vr = new JsonValidationResult<T>
            {
                Model = new List<T>()
            };
            if (!string.IsNullOrWhiteSpace(json))
            {
                vr.IsJsonEmpty = false;

                try
                {
                    vr.Model = JsonHelper.JsonDeserialize<List<T>>(json);

                    vr.IsJsonDeserialized = true;

                    if (vr.Model.Any())
                    {
                        vr.IsModelEmpty = false;
                        vr.Message = "Model Not Empty And Valid";

                        foreach (var model in vr.Model)
                        {
                            if (!ValidateModel(model))
                            {
                                vr.IsModelValid = false;

                                vr.Message = "Model Not Empty And Invalid";

                                break;
                            }
                        }
                    }
                    else
                    {
                        vr.Message = "Model Empty And Valid";
                    }
                }
                catch (Exception)
                {
                    vr.Message = "Json Deserialization Failed";
                }
            }

            return vr;
        }

        public static bool ValidateModel<T>(this T model)
        {
            return System.ComponentModel.DataAnnotations.Validator.TryValidateObject(model, new ValidationContext(model, null, null), new List<ValidationResult>());
        }
    }
}
