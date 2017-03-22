using OFrameLibrary.SettingsHelpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace OFrameLibrary.AppCode.Providers
{
    public class LanguageModelMetadataProvider : DataAnnotationsModelMetadataProvider
    {
        protected override ModelMetadata CreateMetadata(IEnumerable<Attribute> attributes, Type containerType, Func<object> modelAccessor, Type modelType, string propertyName)
        {
            foreach (var attr in attributes.Where(c => c is ValidationAttribute).ToList())
            {
                if (attr != null)
                {
                    if (attr is ValidationAttribute)
                    {
                        var sKey = ((ValidationAttribute)attr).ErrorMessage;
                        var attrAppKey = string.Empty;
                        var sLocalizedText = string.Empty;
                        var typeName = attr.GetType().Name;

                        if (!string.IsNullOrEmpty(sKey))
                        {
                            attrAppKey = string.Format("{0}_{1}_{2}", containerType.FullName, propertyName, typeName);

                            if (HttpContext.Current.Application[attrAppKey] == null)
                            {
                                HttpContext.Current.Application[attrAppKey] = sKey;
                            }
                            else
                            {
                                sKey = HttpContext.Current.Application[attrAppKey].ToString();
                            }

                            sLocalizedText = LanguageHelper.GetKey(sKey);
                            if (string.IsNullOrEmpty(sLocalizedText))
                            {
                                sLocalizedText = sKey;
                            }

                        ((ValidationAttribute)attr).ErrorMessage = sLocalizedText;

                        }
                    }
                }
            }

            return base.CreateMetadata(attributes, containerType, modelAccessor, modelType, propertyName);
        }
    }
}
