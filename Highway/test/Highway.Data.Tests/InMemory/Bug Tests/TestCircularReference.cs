﻿using System;
using System.Collections.Generic;
using System.Linq;

using Highway.Data;
using Highway.Data.Contexts;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Highway.Data.Tests.InMemory.BugTests
{
    [TestClass]
    public class InMemCircularReferenceTest
    {
        private IDataContext _context;
        private Parent _parent;
        private Child _child;

        [TestInitialize]
        public void SetUp()
        {
            this._context = new InMemoryDataContext();

            this._parent = new Parent();
            this._child = new Child();
            this._parent.Child = this._child;
            this._child.Parent = this._parent;

            this._context.Add(_parent);
            this._context.Commit();
        }

        [TestMethod]
        public void ShouldGetSingleChildWithParent()
        {
            var child = this._context.AsQueryable<Child>().Single();
            
            Assert.AreEqual(this._child, child);
            Assert.AreEqual(this._parent, child.Parent);
        }

        [TestMethod]
        public void ShouldGetSingleParentWithChild()
        {
            var parent = this._context.AsQueryable<Parent>().Single();

            Assert.AreEqual(this._parent, parent);
            Assert.AreEqual(this._child, parent.Child);
        }

        class Child
        {
            public Parent Parent { get; set; }
        }

        class Parent
        {
            public Child Child { get; set; }
        }
    }

    [TestClass]
    public class InMemCircularReferenceComplexTest
    {
        private IDataContext _context;
        private GrandParent _grandParent;
        private Parent _parent;
        private Child _child;

        [TestInitialize]
        public void SetUp()
        {
            this._context = new InMemoryDataContext();

            this._grandParent = new GrandParent();
            this._parent = new Parent();
            this._child = new Child();
            
            this._grandParent.Child = this._parent;
            this._grandParent.GrandChild = this._child;

            this._parent.Child = this._child;
            this._parent.DirectParent = this._grandParent;

            this._child.Parent = this._parent;
            this._child.GrandParent = this._grandParent;

            this._context.Add(_parent);
            this._context.Commit();
        }

        [TestMethod]
        public void ShouldGetSingleChild()
        {
            var child = this._context.AsQueryable<Child>().Single();

            AssertEntities(child.GrandParent, child.Parent, child);
        }

        [TestMethod]
        public void ShouldGetSingleParent()
        {
            var parent = this._context.AsQueryable<Parent>().Single();

            AssertEntities(parent.DirectParent, parent, parent.Child);
        }

        [TestMethod]
        public void ShouldGetSingleGrandParent()
        {
            var grandParent = this._context.AsQueryable<GrandParent>().Single();

            AssertEntities(grandParent, grandParent.Child, grandParent.GrandChild);
        }

        private void AssertEntities(GrandParent grandParent, Parent parent, Child child)
        {
            Assert.AreEqual(this._grandParent, grandParent);
            Assert.AreEqual(this._parent, parent);
            Assert.AreEqual(this._child, child);
        }

        class Child
        {
            public Parent Parent { get; set; }
            public GrandParent GrandParent { get; set; }
        }

        class Parent
        {
            public Child Child { get; set; }
            public GrandParent DirectParent { get; set; }
        }

        class GrandParent
        {
            public Parent Child { get; set; }
            public Child GrandChild { get; set; }
        }
    }
}