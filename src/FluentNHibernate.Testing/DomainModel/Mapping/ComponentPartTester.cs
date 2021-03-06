﻿using NUnit.Framework;

namespace FluentNHibernate.Testing.DomainModel.Mapping
{
    [TestFixture]
    public class ComponentPartTester
    {
        [Test]
        public void ComponentCanIncludeParentReference()
        {
            new MappingTester<PropertyTarget>()
                .ForMapping(m =>
                    m.Component(x => x.Component, c =>
                    {
                        c.Map(x => x.Name);
                        c.WithParentReference(x => x.MyParent);
                    }))
                .Element("class/component/parent").Exists()
                .HasAttribute("name", "MyParent");
        }

        [Test]
        public void ComponentDoesntHaveUniqueAttributeByDefault()
        {
            new MappingTester<PropertyTarget>()
                .ForMapping(m =>
                    m.Component(x => x.Component, c =>
                    {
                        c.Map(x => x.Name);
                        c.WithParentReference(x => x.MyParent);
                    }))
                .Element("class/component").DoesntHaveAttribute("unique");
        }

        [Test]
        public void ComponentIsGeneratedWithOnlyOnePropertyReference()
        {
            //Regression test for issue 223
             new MappingTester<PropertyTarget>()
                .ForMapping(m =>
                    m.Component(x => x.Component, c =>
                    {
                        c.Map(x => x.Name);
                    }))
                .Element("//class/component").HasThisManyChildNodes(1);
        }
    }
}
