using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace HNBackend.Module.TLanguage
{
    public class TLanguage
    {
        private string _pathLanguage = string.Empty;
        private const string _SECTION = "section";
        private const string _ENTRY = "entry";

        private static Dictionary<string, string> _mapping = null;

        public TLanguage(string pathLanguage)
        {
            _pathLanguage = pathLanguage;
        }

        public static void InitMapping(string pathLanguage)
        {
            try
            {
                _mapping = GetMappingLanguage(pathLanguage);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static Dictionary<string, string> GetMappingLanguage(string fileanguagePath)
        {
            Dictionary<string, string> mapping = new Dictionary<string, string>();
            try
            {
                string sectionName;
                string entryName;
                string entryValue;
                string key;

                if (File.Exists(fileanguagePath))
                {
                    XDocument xDoc = XDocument.Load(fileanguagePath);
                    var lstSection = xDoc.Descendants(_SECTION).ToList();
                    if (lstSection != null && lstSection.Count >= 0)
                    {
                        foreach (XElement section in lstSection)
                        {
                            sectionName = section.Attribute("name").Value;
                            var lstEntry = section.Descendants(_ENTRY).ToList();

                            foreach (XElement entry in lstEntry)
                            {
                                entryName = entry.Attribute("name").Value;
                                entryValue = entry.Value;
                                key = sectionName + "_" + entryName;

                                if (!mapping.ContainsKey(key))
                                    mapping.Add(key, entryValue);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return mapping;
        }

        public string GetValue(string sectionName, string entryName, string defaultValue)
        {
            try
            {
                if (File.Exists(_pathLanguage))
                {
                    if (string.IsNullOrEmpty(sectionName) || string.IsNullOrEmpty(entryName))
                        return defaultValue;

                    XDocument xDoc = XDocument.Load(_pathLanguage);
                    var lstSection = xDoc.Descendants(_SECTION).ToList();
                    if (lstSection != null && lstSection.Count >= 0)
                    {
                        var lstSectionByName = lstSection.Where(ite => ite.Attribute("name").Value == sectionName).ToList();
                        if (lstSectionByName != null && lstSectionByName.Count > 0)
                        {
                            var lstEntry = lstSectionByName.Descendants(_ENTRY).ToList();
                            if (lstEntry != null && lstEntry.Count > 0)
                            {
                                var lstValue = lstEntry.Where(ite => ite.Attribute("name").Value == entryName).ToList();
                                if (lstValue != null && lstValue.Count > 0)
                                {
                                    var value = lstValue.Select(ite => ite.Value).FirstOrDefault();
                                    if (value != null)
                                        return value;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            { }
            return defaultValue;
        }


        public static string FindValue(string sectionName, string entryName, string defaultValue)
        {
            if (_mapping == null)
                return string.Empty;

            string key = sectionName + "_" + entryName;
            if (_mapping.ContainsKey(key))
                return _mapping[key];
            return string.Empty;
        }
    }
}
