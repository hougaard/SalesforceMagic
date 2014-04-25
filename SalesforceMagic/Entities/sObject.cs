﻿using System;
using System.Reflection;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using FastMember;
using SalesforceMagic.Entities.Abstract;
using SalesforceMagic.Extensions;
using SalesforceMagic.ORM;
using SalesforceMagic.ORM.BaseRequestTemplates;

namespace SalesforceMagic.Entities
{
    public abstract class SObject : ISalesforceObject, IXmlSerializable
    {
        public string Id { get; set; }

        public XmlSchema GetSchema() { return null; }

        public void ReadXml(XmlReader reader) { }

        public void WriteXml(XmlWriter writer)
        {
            // TODO: Implement robust serialization
            Type type = GetType();
            TypeAccessor accessor = ObjectHydrator.GetAccessor(type);
            writer.WriteElementString("type", type.GetName());

            foreach (PropertyInfo info in type.GetProperties())
            {
                var value = accessor[this, info.Name];
                if (value == null) continue;

                string xmlValue = value is DateTime 
                    ? ((DateTime)value).ToString("yyyy-MM-ddTHH:mm:ssZ")
                    : value.ToString();
                writer.WriteElementString(info.GetName(), SalesforceNamespaces.SObject, xmlValue);
            }
        }
    }
}