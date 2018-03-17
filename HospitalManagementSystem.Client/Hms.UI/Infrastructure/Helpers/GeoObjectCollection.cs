namespace Hms.UI.Infrastructure.Providers
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Xml;

    public class GeoObjectCollection : IEnumerable<GeoObject>
    {
        private readonly List<GeoObject> geoObjects;

        public GeoObjectCollection()
        {
            this.geoObjects = new List<GeoObject>();
        }

        public GeoObjectCollection(IEnumerable<GeoObject> geoObjects)
        {
            this.geoObjects = new List<GeoObject>(geoObjects);
        }

        public GeoObjectCollection(string xml)
        {
            this.geoObjects = new List<GeoObject>();
            this.ParseXml(xml);
        }

        private void ParseXml(string xml)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);

            XmlNamespaceManager ns = new XmlNamespaceManager(doc.NameTable);
            ns.AddNamespace("ns", "http://maps.yandex.ru/ymaps/1.x");
            ns.AddNamespace("opengis", "http://www.opengis.net/gml");
            ns.AddNamespace("geocoder", "http://maps.yandex.ru/geocoder/1.x");
            ns.AddNamespace("address", "http://maps.yandex.ru/address/1.x");

            XmlNodeList nodes = doc.SelectNodes(
                "//ns:ymaps/ns:GeoObjectCollection/opengis:featureMember/ns:GeoObject",
                ns);

            if (nodes == null)
            {
                return;
            }

            foreach (XmlNode node in nodes)
            {
                var pointNode = node.SelectSingleNode("opengis:Point/opengis:pos", ns);
                var boundsNode = node.SelectSingleNode("opengis:boundedBy/opengis:Envelope", ns);
                var metaNode = node.SelectSingleNode("opengis:metaDataProperty/geocoder:GeocoderMetaData", ns);

                var components =
                    metaNode?.SelectSingleNode("address:Address", ns)?
                        .ChildNodes.OfType<XmlNode>()
                        .Where(n => n.Name == "Component");

                Address address = new Address();

                components?.ToList()
                    .ForEach(
                        c =>
                        typeof(Address).GetProperty(
                            c["kind"].InnerText,
                            BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance)?
                            .SetValue(address, c["name"].InnerText));


                GeoObject obj = new GeoObject
                {
                    Point = pointNode == null ? new GeoPoint() : GeoPoint.Parse(pointNode.InnerText),
                    BoundedBy =
                        boundsNode == null
                            ? new GeoBound()
                            : new GeoBound(
                                  GeoPoint.Parse(boundsNode["lowerCorner"]?.InnerText),
                                  GeoPoint.Parse(boundsNode["upperCorner"]?.InnerText)),
                    GeocoderMetaData =
                        new GeoMetaData(metaNode?["text"]?.InnerText, metaNode?["kind"]?.InnerText, address)
                };

                this.geoObjects.Add(obj);
            }
        }

        public GeoObject this[int i] => this.geoObjects[i];

        public IEnumerator<GeoObject> GetEnumerator()
        {
            return this.geoObjects.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}