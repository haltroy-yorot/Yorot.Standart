using HTAlt;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Yorot
{
    public class SiteMan : YorotManager
    {
        public SiteMan(string configFile, YorotMain main) : base(configFile, main)
        {
        }

        /// <summary>
        /// YorotSites of this manager.
        /// </summary>
        public List<YorotSite> Sites { get; set; } = new List<YorotSite>();

        public override void ExtractXml(XmlNode rootNode)
        {
            List<string> appliedSettings = new List<string>();
            for (int i = 0; i < rootNode.ChildNodes.Count; i++)
            {
                XmlNode node = rootNode.ChildNodes[i];
                if (appliedSettings.Contains(node.Name.ToLowerEnglish()))
                {
                    Output.WriteLine("[SiteMan] Threw away \"" + node.OuterXml + "\", configuration already applied.", LogLevel.Warning);
                    break;
                }
                appliedSettings.Add(node.Name.ToLowerEnglish());
                switch (node.Name.ToLowerEnglish())
                {
                    case "sites":
                        for (int ı = 0; ı < node.ChildNodes.Count; ı++)
                        {
                            XmlNode subnode = node.ChildNodes[ı];
                            if (subnode.Name.ToLowerEnglish() == "site")
                            {
                                var site = new YorotSite() { Manager = this };
                                for (int _i = 0; _i < subnode.ChildNodes.Count; _i++)
                                {
                                    var siteNode = subnode.ChildNodes[_i];
                                    var nodeName = siteNode.Name.ToLowerEnglish();
                                    switch (nodeName)
                                    {
                                        case "name":
                                            site.Name = siteNode.InnerXml.XmlToString();
                                            break;

                                        case "url":
                                            site.Url = siteNode.InnerXml.XmlToString();
                                            break;

                                        case "date":
                                            site.Date = DateTime.ParseExact(siteNode.InnerXml.XmlToString(), "dd-MM-yyyy HH-mm-ss", null);
                                            break;

                                        case "allowmic":
                                            YorotPermissionMode allowMic;
                                            if (Enum.TryParse(siteNode.InnerXml.XmlToString(), out allowMic))
                                            {
                                                site.Permissions.allowMic.Allowance = allowMic;
                                            }
                                            else
                                            {
                                                Output.WriteLine("[SiteMan] Threw away \"" + siteNode.OuterXml + "\" in site \"" + (string.IsNullOrWhiteSpace(site.Url) ? "AnonymousSite" : site.Url) + "\",  unrecognized format.", LogLevel.Warning);
                                            }
                                            break;

                                        case "allowcam":
                                            YorotPermissionMode allowCam;
                                            if (Enum.TryParse(siteNode.InnerXml.XmlToString(), out allowCam))
                                            {
                                                site.Permissions.allowCam.Allowance = allowCam;
                                            }
                                            else
                                            {
                                                Output.WriteLine("[SiteMan] Threw away \"" + siteNode.OuterXml + "\" in site \"" + (string.IsNullOrWhiteSpace(site.Url) ? "AnonymousSite" : site.Url) + "\", unrecognized format.", LogLevel.Warning);
                                            }
                                            break;

                                        case "allownotif":
                                            YorotPermissionMode allowNotif;
                                            if (Enum.TryParse(siteNode.InnerXml.XmlToString(), out allowNotif))
                                            {
                                                site.Permissions.allowNotif.Allowance = allowNotif;
                                            }
                                            else
                                            {
                                                Output.WriteLine("[SiteMan] Threw away \"" + siteNode.OuterXml + "\" in site \"" + (string.IsNullOrWhiteSpace(site.Url) ? "AnonymousSite" : site.Url) + "\",  unrecognized format.", LogLevel.Warning);
                                            }
                                            break;

                                        case "allowwe":
                                            YorotPermissionMode allowWE;
                                            if (Enum.TryParse(siteNode.InnerXml.XmlToString(), out allowWE))
                                            {
                                                site.Permissions.allowWE.Allowance = allowWE;
                                            }
                                            else
                                            {
                                                Output.WriteLine("[SiteMan] Threw away \"" + siteNode.OuterXml + "\" in site \"" + (string.IsNullOrWhiteSpace(site.Url) ? "AnonymousSite" : site.Url) + "\",  unrecognized format.", LogLevel.Warning);
                                            }
                                            break;

                                        case "allowys":
                                            YorotPermissionMode allowYS;
                                            if (Enum.TryParse(siteNode.InnerXml.XmlToString(), out allowYS))
                                            {
                                                site.Permissions.allowYS.Allowance = allowYS;
                                            }
                                            else
                                            {
                                                Output.WriteLine("[SiteMan] Threw away \"" + siteNode.OuterXml + "\" in site \"" + (string.IsNullOrWhiteSpace(site.Url) ? "AnonymousSite" : site.Url) + "\",  unrecognized format.", LogLevel.Warning);
                                            }
                                            break;

                                        case "notifpriority":
                                            site.Permissions.notifPriority = int.Parse(siteNode.InnerXml.XmlToString());
                                            break;

                                        case "startnotifonboot":
                                            site.Permissions.startNotifOnBoot = siteNode.InnerXml.XmlToString() == "true";
                                            break;
                                    }
                                }
                            }
                            else
                            {
                                Output.WriteLine("[SiteMan] Threw away \"" + subnode.OuterXml + "\", unsupported.", LogLevel.Warning);
                            }
                        }
                        break;

                    default:
                        if (!node.IsComment())
                        {
                            Output.WriteLine("[SiteMan] Threw away \"" + node.OuterXml + "\", invalid configuration.", LogLevel.Warning);
                        }
                        break;
                }
            }
        }

        public override string ToXml()
        {
            string x = "<?xml version=\"1.0\" encoding=\"utf-16\"?>" + Environment.NewLine +
    "<root>" + Environment.NewLine +
    "<!-- Yorot Sites Config File" + Environment.NewLine + Environment.NewLine +
    "This file is used to save site settings." + Environment.NewLine +
    "Editing this file might cause problems with Yorot." + Environment.NewLine +
    "-->" + Environment.NewLine +
    "<Sites>" + Environment.NewLine;
            for (int i = 0; i < Sites.Count; i++)
            {
                YorotSite site = Sites[i];
                x += "<Site>"
                    + Environment.NewLine
                    + "<Name>"
                    + site.Name.ToXML()
                    + "</Name>"
                    + Environment.NewLine
                    + "<Url>"
                    + site.Url.ToXML()
                    + "</Url>"
                    + Environment.NewLine
                    + "<Date>"
                    + site.Date.ToString("dd-MM-yyyy HH-mm-ss")
                    + "</Date>"
                    + Environment.NewLine
                    + "<allowMic>"
                    + site.Permissions.allowMic.Allowance.ToString()
                    + "</allowMic>"
                    + Environment.NewLine
                    + "<allowCam>"
                    + site.Permissions.allowCam.Allowance.ToString()
                    + "</allowCam>"
                    + Environment.NewLine
                    + "<allowNotif>"
                    + site.Permissions.allowNotif.Allowance.ToString()
                    + "</allowNotif>"
                    + Environment.NewLine
                    + "<allowWE>"
                    + site.Permissions.allowWE.Allowance.ToString()
                    + "</allowWE>"
                    + Environment.NewLine
                    + "<allowYS>"
                    + site.Permissions.allowYS.Allowance.ToString()
                    + "</allowYS>"
                    + Environment.NewLine
                    + "<notifPriority>"
                    + site.Permissions.notifPriority
                    + "</notifPriority>"
                    + Environment.NewLine
                    + "<startNotifOnBoot>"
                    + (site.Permissions.startNotifOnBoot ? "true" : "false")
                    + "</startNotifOnBoot>"
                    + Environment.NewLine
                    + "</Site>"
                    + Environment.NewLine;
            }
            return (x + "</Sites>" + Environment.NewLine + "</root>").BeautifyXML();
        }
    }
}