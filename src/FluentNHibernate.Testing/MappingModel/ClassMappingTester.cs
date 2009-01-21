﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.MappingModel.Identity;
using NUnit.Framework;
using Rhino.Mocks;

namespace FluentNHibernate.Testing.MappingModel
{
    [TestFixture]
    public class ClassMappingTester
    {
        private ClassMapping _classMapping;

        [SetUp]
        public void SetUp()
        {
            _classMapping = new ClassMapping();
        }

        [Test]
        public void CanSetIdToBeStandardIdMapping()
        {
            var idMapping = new IdMapping();
            _classMapping.Id = idMapping;

            _classMapping.Id.ShouldEqual(idMapping);
        }

        [Test]
        public void CanSetIdToBeCompositeIdMapping()
        {
            var idMapping = new CompositeIdMapping();
            _classMapping.Id = idMapping;

            _classMapping.Id.ShouldEqual(idMapping);
        }

        [Test]
        public void CanAddProperty()
        {
            var property = new PropertyMapping { Name = "Property1" };
            _classMapping.AddProperty(property);

            _classMapping.Properties.ShouldContain(property);
        }

        [Test]
        public void CanAddBag()
        {
            var bag = new BagMapping
                          {
                              Name = "bag1",
                              Key = new KeyMapping(),
                              Contents = new OneToManyMapping { ClassName = "class1" }
                          };
            _classMapping.AddCollection(bag);

            _classMapping.Collections.ShouldContain(bag);
        }

        [Test]
        public void CanAddReference()
        {
            var reference = new ManyToOneMapping { Name = "parent" };
            _classMapping.AddReference(reference);

            _classMapping.References.ShouldContain(reference);
        }

        [Test]
        public void Should_pass_id_to_the_visitor()
        {
            var classMap = MappingMother.CreateClassMapping();
            classMap.Id = new IdMapping();

            var visitor = MockRepository.GenerateMock<IMappingModelVisitor>();
            visitor.Expect(x => x.ProcessIdentity(classMap.Id));

            classMap.AcceptVisitor(visitor);

            visitor.VerifyAllExpectations();
        }

        [Test]
        public void Should_not_pass_null_id_to_the_visitor()
        {
            var classMap = MappingMother.CreateClassMapping();
            classMap.Id = null;

            var visitor = MockRepository.GenerateMock<IMappingModelVisitor>();            
            visitor.Expect(x => x.ProcessIdentity(classMap.Id)).Repeat.Never();            
            
            classMap.AcceptVisitor(visitor);
            
            visitor.VerifyAllExpectations();
        }

        
    }
}