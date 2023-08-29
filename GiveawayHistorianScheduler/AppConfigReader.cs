using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Fugu.AsciiArt;
using GiveawayHistorianScheduler.Interface;
using GiveawayHistorianScheduler.Attributes;

namespace GiveawayHistorianScheduler
{
    public class AppConfigReader<I> : IAppConfig<I>
    {
        /// <summary>
        /// Gets the App's Config by parsing the KubeArg Attribute of the ConfigModel's properties.
        /// </summary>
        /// <param name="configArg"></param>
        /// <param name="configModel"></param>
        /// <returns></returns>
        public I GetAppConfig(string[] configArg, I configModel)
        {
            foreach (var arg in configArg)
            {
                var matches = Regex.Matches(arg, "(\\--\\w+) ");
                var kubeArgName = matches.FirstOrDefault().Value;
                var configProperty = GetPropertyByConfigName(kubeArgName.Replace("--", ""));
                if (configProperty != null)
                {
                    var value = ConvertConfigValueToBaseType(configProperty, arg, kubeArgName);
                    configProperty?.SetValue(configModel, value);
                }
            }
            return configModel;
        }
        /// <summary>
        /// Gets the Property by the Config's tagName and Kube Argument 
        /// </summary>
        /// <param name="tagName"></param>
        /// <returns></returns>
        private PropertyInfo GetPropertyByConfigName(string tagName)
        {
            Type modelType = typeof(I);

            var requiredPropertiesList = modelType.GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(i => i.CustomAttributes.Any(x => x.AttributeType == typeof(KubeArgAttribute))).ToList();

            return requiredPropertiesList.Where(prop => prop.CustomAttributes
                                                       .Where(att => att.ConstructorArguments
                                                              .Where(i => i.Value.ToString().ToLower() == tagName.ToLower().Replace(" ", "").Trim()).Count() > 0).Count() > 0)
                                                                   .FirstOrDefault();
        }
        /// <summary>
        /// Converts the Config's property to its base type
        /// </summary>
        /// <param name="configProperty"></param>
        /// <param name="arg"></param>
        /// <param name="argName"></param>
        /// <returns></returns>
        private object ConvertConfigValueToBaseType(PropertyInfo configProperty, string arg, string argName)
        {
            if (configProperty?.PropertyType.Name == "List`1")
            {
                var collection = arg.Replace(argName, "").Split(',').ToList();
                return collection;
            }
            if (configProperty.PropertyType.Name == "Nullable`1" && configProperty.PropertyType.GenericTypeArguments.Any(i => i.Name == "Int64"))
                return Int64.Parse(arg.Replace(argName, ""));
            else
                return arg.Replace(argName, "");
        }
        /// <summary>
        /// Gets the Property's string in a printable format. Sensitive connections strings are hashed according the the Reveal parameters set in the Sensitive Tag.
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public string GetPropertyInfoString(I config)
        {
            var configFrameTitle = new Frame("                                           App Configurations                                         ", FrameType.Basic);
            string propertyInfoString = configFrameTitle.ToString() + "\n";
            if (config != null)
            {
                //var configProperties = typeof(I).GetProperties(BindingFlags.Public | BindingFlags.Instance).ToList();
                var configProperties = typeof(I).GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(i => i.CustomAttributes.Any(x => x.AttributeType == typeof(KubeArgAttribute))).ToList();
                foreach (var property in configProperties)
                {
                    if (property.CustomAttributes.Any(i => i.AttributeType == typeof(SensitiveAttribute)))
                    {
                        var revealValue = (int)property.CustomAttributes.Where(i => i.AttributeType == typeof(SensitiveAttribute)).FirstOrDefault().ConstructorArguments.FirstOrDefault().Value;
                        var propertyValue = property.GetValue(config);
                        if (propertyValue.ToString().Length > 10)
                        {
                            var maskedValue = propertyValue.ToString().Substring(0, revealValue).ToString() + "***********************";
                            propertyInfoString += property.Name + ": " + maskedValue + "\n";
                        }
                    }
                    else if (property?.PropertyType.Name == "List`1" && property.PropertyType.GenericTypeArguments.Any(i => i.Name == "String"))
                        propertyInfoString += property.Name + ": " + (property.GetValue(config) as List<string>).Aggregate((current, next) => current + "," + next) + "\n";
                    else
                        propertyInfoString += property.Name + ": " + property.GetValue(config) + "\n";
                }
            }
            else
                propertyInfoString += "Warning: No Configurations Set. App will not function!!!";

            propertyInfoString += "---------------------------------------------------------------------------------------------------------\n";
            return propertyInfoString;
        }
    }
}
