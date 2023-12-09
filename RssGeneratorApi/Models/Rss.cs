using System;
using System.Collections.Generic;

namespace RssGenerator.Models
{
    [Serializable]
    [System.ComponentModel.DesignerCategory("code")]
    [System.Xml.Serialization.XmlType(AnonymousType = false)]
    [System.Xml.Serialization.XmlRoot(Namespace = "", IsNullable = false)]
    public partial class rss
    {

        private rssChannel channelField;

        private decimal versionField;

        /// <remarks/>
        public rssChannel channel
        {
            get
            {
                return channelField;
            }
            set
            {
                channelField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttribute()]
        public decimal version
        {
            get
            {
                return versionField;
            }
            set
            {
                versionField = value;
            }
        }
    }

    /// <remarks/>
    [Serializable]
    [System.ComponentModel.DesignerCategory("code")]
    [System.Xml.Serialization.XmlType(AnonymousType = true)]
    public partial class rssChannel
    {

        private string titleField;

        private string descriptionField;

        private string linkField;

        private string languageField;

        private string copyrightField;

        private string docsField;

        private string generatorField;

        private link link1Field;

        private List<rssChannelItem> itemField;

        /// <remarks/>
        public string title
        {
            get
            {
                return titleField;
            }
            set
            {
                titleField = value;
            }
        }

        /// <remarks/>
        public string description
        {
            get
            {
                return descriptionField;
            }
            set
            {
                descriptionField = value;
            }
        }

        /// <remarks/>
        public string link
        {
            get
            {
                return linkField;
            }
            set
            {
                linkField = value;
            }
        }

        /// <remarks/>
        public string language
        {
            get
            {
                return languageField;
            }
            set
            {
                languageField = value;
            }
        }

        /// <remarks/>
        public string copyright
        {
            get
            {
                return copyrightField;
            }
            set
            {
                copyrightField = value;
            }
        }

        /// <remarks/>
        public string docs
        {
            get
            {
                return docsField;
            }
            set
            {
                docsField = value;
            }
        }

        /// <remarks/>
        public string generator
        {
            get
            {
                return generatorField;
            }
            set
            {
                generatorField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElement("link", Namespace = "http://www.w3.org/2005/Atom")]
        public link link1
        {
            get
            {
                return link1Field;
            }
            set
            {
                link1Field = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElement("item")]
        public List<rssChannelItem> item
        {
            get
            {
                return itemField;
            }
            set
            {
                itemField = value;
            }
        }
    }

    /// <remarks/>
    [Serializable]
    [System.ComponentModel.DesignerCategory("code")]
    [System.Xml.Serialization.XmlType(AnonymousType = true, Namespace = "http://www.w3.org/2005/Atom")]
    [System.Xml.Serialization.XmlRoot(Namespace = "http://www.w3.org/2005/Atom", IsNullable = false)]
    public partial class link
    {

        private string hrefField;

        private string relField;

        private string typeField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttribute()]
        public string href
        {
            get
            {
                return hrefField;
            }
            set
            {
                hrefField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttribute()]
        public string rel
        {
            get
            {
                return relField;
            }
            set
            {
                relField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttribute()]
        public string type
        {
            get
            {
                return typeField;
            }
            set
            {
                typeField = value;
            }
        }
    }

    /// <remarks/>
    [Serializable]
    [System.ComponentModel.DesignerCategory("code")]
    [System.Xml.Serialization.XmlType(AnonymousType = true)]
    public partial class rssChannelItem
    {

        private string titleField;

        private string descriptionField;

        private string pubDateField;

        private string linkField;

        private rssChannelItemGuid guidField;

        /// <remarks/>
        public string title
        {
            get
            {
                return titleField;
            }
            set
            {
                titleField = value;
            }
        }

        /// <remarks/>
        public string description
        {
            get
            {
                return descriptionField;
            }
            set
            {
                descriptionField = value;
            }
        }

        /// <remarks/>
        public string pubDate
        {
            get
            {
                return pubDateField;
            }
            set
            {
                pubDateField = value;
            }
        }

        /// <remarks/>
        public string link
        {
            get
            {
                return linkField;
            }
            set
            {
                linkField = value;
            }
        }

        /// <remarks/>
        public rssChannelItemGuid guid
        {
            get
            {
                return guidField;
            }
            set
            {
                guidField = value;
            }
        }
    }

    /// <remarks/>
    [Serializable]
    [System.ComponentModel.DesignerCategory("code")]
    [System.Xml.Serialization.XmlType(AnonymousType = true)]
    public partial class rssChannelItemGuid
    {

        private bool isPermaLinkField;

        private string valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttribute()]
        public bool isPermaLink
        {
            get
            {
                return isPermaLinkField;
            }
            set
            {
                isPermaLinkField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlText()]
        public string Value
        {
            get
            {
                return valueField;
            }
            set
            {
                valueField = value;
            }
        }
    }

}