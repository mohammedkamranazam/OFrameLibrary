using OFrameLibrary.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.UI.WebControls;

namespace OFrameLibrary.Util
{
    public static class Extensions
    {
        public static string MapPath(this string path)
        {
            return HttpContext.Current.Server.MapPath(path);
        }

        /// <summary>
        /// Gets the SelectListItem Collection from the enum.
        /// </summary>
        /// <param name="type">Enum Type</param>
        /// <param name="takeValue">if set to <c>true</c> the value of the SelectListItem will be the enum integer value.</param>
        /// <param name="friendlyValue">if set to <c>true</c> the value of the SelectListItem will be space separated.</param>
        /// <param name="selectedValue">The default selected value in the SelectListItem collection</param>
        /// <param name="insertSelect">if set to <c>true</c> the first item will be the Select prompt.</param>
        /// <param name="selected">if set to <c>true</c> the Select prompt will be selected by default.</param>
        /// <param name="label">The label of the Select prompt</param>
        /// <param name="value">The value of the Select prompt</param>
        /// <param name="isSelectItemDisabled">if set to <c>true</c> the Select prompt will be disabled and unelectable.</param>
        /// <returns></returns>
        public static IEnumerable<SelectListItem> GetEnumList(this System.Type type,
              bool takeValue = false,
              bool friendlyValue = false,
              string selectedValue = null,
              bool insertSelect = false,
              bool selected = true,
              string label = "-- Select --",
              string value = null,
              bool isSelectItemDisabled = true)
        {
            return Utilities.GetSelectList(EnumHelper.GetSelectList(type), takeValue, friendlyValue, selectedValue, insertSelect, selected, label, value, isSelectItemDisabled);
        }

        public static string Join<T>(this T[] collection, string delimeter = ";")
        {
            return string.Join(delimeter, collection) + delimeter;
        }

        public static List<T> GetIDList<T>(this string ids, string separator = ";")
        {
            if (string.IsNullOrWhiteSpace(ids))
            {
                return null;
            }

            return ids.Split(new string[] { separator }, StringSplitOptions.RemoveEmptyEntries).Select(c => (T)Convert.ChangeType(c, typeof(T))).ToList();
        }

        public static IDUpdater<T> UpdatedIDs<T>(this IDUpdater<T> updaterModel)
        {
            updaterModel.AddIDs = updaterModel.NewIDs.Where(c => !updaterModel.OldIDs.Contains(c)).ToList();
            updaterModel.RemoveIDs = updaterModel.OldIDs.Where(c => !updaterModel.NewIDs.Contains(c)).ToList();

            return updaterModel;
        }

        public static SelectList ToSelectList<TEnum>(this TEnum enumObj)
              where TEnum : struct, IComparable, IFormattable, IConvertible
        {
            var values = from TEnum e in Enum.GetValues(typeof(TEnum))
                         select new { Id = e, Name = e.ToString() };
            return new SelectList(values, enumObj);
        }

        public static string ToFriendlyCase(this string EnumString)
        {
            return Regex.Replace(EnumString, "(?!^)([A-Z])", " $1");
        }

        public static void AddSelect(this DropDownList dropDownList)
        {
            dropDownList.Items.Insert(0, new ListItem("Select", "-1"));
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

        //public static Gender GetGender(this DropDownListAdv dropDownList)
        //{
        //    if (dropDownList.SelectedValue == Gender.Male.ToString())
        //    {
        //        return Gender.Male;
        //    }
        //    else
        //    {
        //        if (dropDownList.SelectedValue == Gender.Female.ToString())
        //        {
        //            return Gender.Female;
        //        }
        //        else
        //        {
        //            if (dropDownList.SelectedValue == Gender.Unspecified.ToString())
        //            {
        //                return Gender.Unspecified;
        //            }
        //            else
        //            {
        //                return Gender.Unspecified;
        //            }
        //        }
        //    }
        //}

        //public static string GetImageFileExtension(this AjaxFileUploadEventArgs ee)
        //{
        //    var extension = ".jpg";

        //    if (ee.ContentType.NullableContains("jpg"))
        //    {
        //        extension = ".jpg";
        //    }

        //    if (ee.ContentType.NullableContains("gif"))
        //    {
        //        extension = ".gif";
        //    }

        //    if (ee.ContentType.NullableContains("png"))
        //    {
        //        extension = ".png";
        //    }

        //    if (ee.ContentType.NullableContains("jpeg"))
        //    {
        //        extension = ".jpeg";
        //    }

        //    return extension;
        //}

        //public static int? GetNullableSelectedValue(this DropDownListAdv dropDownList)
        //{
        //    if (dropDownList.SelectedItem == null || dropDownList.SelectedValue == "-1")
        //    {
        //        return null;
        //    }
        //    else
        //    {
        //        return dropDownList.GetSelectedValue();
        //    }
        //}

        public static string GetProfilePic(this Gender gender)
        {
            if (gender == Gender.Male)
            {
                return AppConfig.MaleAvatar;
            }
            else
            {
                if (gender == Gender.Female)
                {
                    return AppConfig.FemaleAvatar;
                }
                else
                {
                    if (gender == Gender.Unspecified)
                    {
                        return AppConfig.UnspecifiedAvatar;
                    }
                    else
                    {
                        return AppConfig.UnspecifiedAvatar;
                    }
                }
            }
        }

        //public static int GetSelectedValue(this DropDownListAdv dropDownList)
        //{
        //    return DataParser.IntParse(dropDownList.SelectedValue);
        //}

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

        public static bool NullableContains(this string text, string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return false;
            }

            return text.IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) >= 0;
        }

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

        //public static void SetNullableSelectedValue(this DropDownListAdv dropDownList, int? value)
        //{
        //    if (value == null)
        //    {
        //        dropDownList.SelectedValue = "-1";
        //    }
        //    else
        //    {
        //        dropDownList.SelectedValue = value.ToString();
        //    }
        //}

        public static HttpCacheability ToCacheType(this string location)
        {
            switch (location)
            {
                case "Server":
                    return HttpCacheability.Server;

                case "Client":
                    return HttpCacheability.Private;

                case "Both":
                    return HttpCacheability.ServerAndPrivate;

                case "None":
                default:
                    return HttpCacheability.NoCache;
            }
        }
    }
}