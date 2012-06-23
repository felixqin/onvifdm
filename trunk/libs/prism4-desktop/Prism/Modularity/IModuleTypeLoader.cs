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
using System;

namespace Microsoft.Practices.Prism.Modularity
{
    /// <summary>
    /// Defines the interface for moduleTypeLoaders
    /// </summary>
    public interface IModuleTypeLoader
    {
        /// <summary>
        /// Evaluates the <see cref="ModuleInfo.Ref"/> property to see if the current typeloader will be able to retrieve the <paramref name="moduleInfo"/>.
        /// </summary>
        /// <param name="moduleInfo">Module that should have it's type loaded.</param>
        /// <returns><see langword="true"/> if the current typeloader is able to retrieve the module, otherwise <see langword="false"/>.</returns>
        bool CanLoadModuleType(ModuleInfo moduleInfo);      

        /// <summary>
        /// Retrieves the <paramref name="moduleInfo"/>.
        /// </summary>
        /// <param name="moduleInfo">Module that should have it's type loaded.</param>
        void LoadModuleType(ModuleInfo moduleInfo);
   
        /// <summary>
        /// Raised repeatedly to provide progress as modules are downloaded in the background.
        /// </summary>
        event EventHandler<ModuleDownloadProgressChangedEventArgs> ModuleDownloadProgressChanged;

        /// <summary>
        /// Raised when a module is loaded or fails to load.
        /// </summary>
        /// <remarks>
        /// This event is raised once per ModuleInfo instance requested in <see cref=" LoadModuleType"/>.
        /// </remarks>
        event EventHandler<LoadModuleCompletedEventArgs> LoadModuleCompleted;
    }
}
