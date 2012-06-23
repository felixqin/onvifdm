//===================================================================================
// Microsoft patterns & practices
// Composite Application Guidance for Windows Presentation Foundation and Silverlight
//===================================================================================
// Copyright (c) Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===================================================================================
// The example companies, organizations, products, domain names,
// e-mail addresses, logos, people, places, and events depicted
// herein are fictitious.  No association with any real company,
// organization, product, domain name, email address, logo, person,
// places, or events is intended or should be inferred.
//===================================================================================
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Practices.Prism.UnityExtensions.Tests.Mocks;
using Microsoft.Practices.Prism.Logging;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.Prism.UnityExtensions.Tests
{
    [TestClass]
    public class UnityBootstrapperFixture: BootstrapperFixtureBase
    {
        [TestMethod]
        public void ContainerDefaultsToNull()
        {
            var bootstrapper = new DefaultUnityBootstrapper();
            var container = bootstrapper.BaseContainer;

            Assert.IsNull(container);
        }

        [TestMethod]
        public void CanCreateConcreteBootstrapper()
        {
            new DefaultUnityBootstrapper();
        }

        [TestMethod]
        public void CreateContainerShouldInitializeContainer()
        {
            var bootstrapper = new DefaultUnityBootstrapper();

            IUnityContainer container = bootstrapper.CallCreateContainer();

            Assert.IsNotNull(container);
            Assert.IsInstanceOfType(container, typeof(IUnityContainer));
        }

        [TestMethod]
        public void ConfigureContainerAddsModuleCatalogToContainer()
        {
            var bootstrapper = new DefaultUnityBootstrapper();
            bootstrapper.Run();

            var returnedCatalog = bootstrapper.BaseContainer.Resolve<IModuleCatalog>();
            Assert.IsNotNull(returnedCatalog);
            Assert.IsTrue(returnedCatalog is ModuleCatalog);
        }

        [TestMethod]
        public void ConfigureContainerAddsLoggerFacadeToContainer()
        {
            var bootstrapper = new DefaultUnityBootstrapper();
            bootstrapper.Run();

            var returnedCatalog = bootstrapper.BaseContainer.Resolve<ILoggerFacade>();
            Assert.IsNotNull(returnedCatalog);
        }

        [TestMethod]
        public void ConfigureContainerAddsRegionNavigationJournalEntryToContainer()
        {
            var bootstrapper = new DefaultUnityBootstrapper();
            bootstrapper.Run();

            var actual1 = bootstrapper.BaseContainer.Resolve<IRegionNavigationJournalEntry>();
            var actual2 = bootstrapper.BaseContainer.Resolve<IRegionNavigationJournalEntry>();

            Assert.IsNotNull(actual1);
            Assert.IsNotNull(actual2);
            Assert.AreNotSame(actual1, actual2);
        }

        [TestMethod]
        public void ConfigureContainerAddsRegionNavigationJournalToContainer()
        {
            var bootstrapper = new DefaultUnityBootstrapper();
            bootstrapper.Run();

            var actual1 = bootstrapper.BaseContainer.Resolve<IRegionNavigationJournal>();
            var actual2 = bootstrapper.BaseContainer.Resolve<IRegionNavigationJournal>();

            Assert.IsNotNull(actual1);
            Assert.IsNotNull(actual2);
            Assert.AreNotSame(actual1, actual2);
        }

        [TestMethod]
        public void ConfigureContainerAddsRegionNavigationServiceToContainer()
        {
            var bootstrapper = new DefaultUnityBootstrapper();
            bootstrapper.Run();

            var actual1 = bootstrapper.BaseContainer.Resolve<IRegionNavigationService>();
            var actual2 = bootstrapper.BaseContainer.Resolve<IRegionNavigationService>();

            Assert.IsNotNull(actual1);
            Assert.IsNotNull(actual2);
            Assert.AreNotSame(actual1, actual2);
        }

        [TestMethod]
        public void ConfigureContainerAddsNavigationTargetHandlerToContainer()
        {
            var bootstrapper = new DefaultUnityBootstrapper();
            bootstrapper.Run();

            var actual1 = bootstrapper.BaseContainer.Resolve<IRegionNavigationContentLoader>();
            var actual2 = bootstrapper.BaseContainer.Resolve<IRegionNavigationContentLoader>();

            Assert.IsNotNull(actual1);
            Assert.IsNotNull(actual2);
            Assert.AreSame(actual1, actual2);
        }

        [TestMethod]
        public void RegisterFrameworkExceptionTypesShouldRegisterActivationException()
        {
            var bootstrapper = new DefaultUnityBootstrapper();

            bootstrapper.CallRegisterFrameworkExceptionTypes();

            Assert.IsTrue(ExceptionExtensions.IsFrameworkExceptionRegistered(
                typeof(Microsoft.Practices.ServiceLocation.ActivationException)));
        }

        [TestMethod]
        public void RegisterFrameworkExceptionTypesShouldRegisterResolutionFailedException()
        {
            var bootstrapper = new DefaultUnityBootstrapper();

            bootstrapper.CallRegisterFrameworkExceptionTypes();

            Assert.IsTrue(ExceptionExtensions.IsFrameworkExceptionRegistered(
                typeof(Microsoft.Practices.Unity.ResolutionFailedException)));
        }
    }

    internal class DefaultUnityBootstrapper : UnityBootstrapper
    {
        public List<string> MethodCalls = new List<string>();
        public bool InitializeModulesCalled;
        public bool ConfigureRegionAdapterMappingsCalled;
        public RegionAdapterMappings DefaultRegionAdapterMappings;
        public bool CreateLoggerCalled;
        public bool CreateModuleCatalogCalled;
        public bool ConfigureContainerCalled;
        public bool CreateShellCalled;
        public bool CreateContainerCalled;
        public bool ConfigureModuleCatalogCalled;
        public bool InitializeShellCalled;
        public bool ConfigureServiceLocatorCalled;
        public bool ConfigureDefaultRegionBehaviorsCalled;
        public DependencyObject ShellObject = new UserControl();

        public DependencyObject BaseShell
        {
            get { return base.Shell; }
        }
        public IUnityContainer BaseContainer
        {
            get { return base.Container; }
            set { base.Container = value; }
        }

        public MockLoggerAdapter BaseLogger
        { get { return base.Logger as MockLoggerAdapter; } }

        public IUnityContainer CallCreateContainer()
        {
            return this.CreateContainer();
        }

        protected override IUnityContainer CreateContainer()
        {
            this.MethodCalls.Add(System.Reflection.MethodBase.GetCurrentMethod().Name);
            this.CreateContainerCalled = true;
            return base.CreateContainer();
        }

        protected override void ConfigureContainer()
        {
            this.MethodCalls.Add(System.Reflection.MethodBase.GetCurrentMethod().Name);
            this.ConfigureContainerCalled = true;
            base.ConfigureContainer();
        }

        protected override ILoggerFacade CreateLogger()
        {
            this.MethodCalls.Add(System.Reflection.MethodBase.GetCurrentMethod().Name);
            this.CreateLoggerCalled = true;
            return new MockLoggerAdapter();
        }

        protected override DependencyObject CreateShell()
        {
            this.MethodCalls.Add(System.Reflection.MethodBase.GetCurrentMethod().Name);
            this.CreateShellCalled = true;
            return ShellObject;
        }

        protected override void ConfigureServiceLocator()
        {
            this.MethodCalls.Add(System.Reflection.MethodBase.GetCurrentMethod().Name);
            this.ConfigureServiceLocatorCalled = true;
            base.ConfigureServiceLocator();
        }
        protected override IModuleCatalog CreateModuleCatalog()
        {
            this.MethodCalls.Add(System.Reflection.MethodBase.GetCurrentMethod().Name);
            this.CreateModuleCatalogCalled = true;
            return base.CreateModuleCatalog();
        }

        protected override void ConfigureModuleCatalog()
        {
            this.MethodCalls.Add(System.Reflection.MethodBase.GetCurrentMethod().Name);
            this.ConfigureModuleCatalogCalled = true;
            base.ConfigureModuleCatalog();
        }

        protected override void InitializeShell()
        {
            this.MethodCalls.Add(System.Reflection.MethodBase.GetCurrentMethod().Name);
            this.InitializeShellCalled = true;
            // no op
        }

        protected override void InitializeModules()
        {
            this.MethodCalls.Add(System.Reflection.MethodBase.GetCurrentMethod().Name);
            this.InitializeModulesCalled = true;
            base.InitializeModules();
        }

        protected override IRegionBehaviorFactory ConfigureDefaultRegionBehaviors()
        {
            this.MethodCalls.Add(System.Reflection.MethodBase.GetCurrentMethod().Name);
            this.ConfigureDefaultRegionBehaviorsCalled = true;
            return base.ConfigureDefaultRegionBehaviors();
        }

        protected override RegionAdapterMappings ConfigureRegionAdapterMappings()
        {
            this.MethodCalls.Add(System.Reflection.MethodBase.GetCurrentMethod().Name);
            ConfigureRegionAdapterMappingsCalled = true;
            var regionAdapterMappings = base.ConfigureRegionAdapterMappings();

            DefaultRegionAdapterMappings = regionAdapterMappings;

            return regionAdapterMappings;
        }

        protected override void RegisterFrameworkExceptionTypes()
        {
            this.MethodCalls.Add(System.Reflection.MethodBase.GetCurrentMethod().Name);
            base.RegisterFrameworkExceptionTypes();
        }

        public void CallRegisterFrameworkExceptionTypes()
        {
            base.RegisterFrameworkExceptionTypes();
        }
    }
    
}
