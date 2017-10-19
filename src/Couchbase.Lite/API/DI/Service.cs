﻿// 
// Service.cs
// 
// Author:
//     Jim Borden  <jim.borden@couchbase.com>
// 
// Copyright (c) 2017 Couchbase, Inc All rights reserved.
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
// http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// 
using System;
using System.Linq;
using System.Threading;
using Microsoft.Extensions.DependencyInjection;

namespace Couchbase.Lite.DI
{
    /// <summary>
    /// This is the entry point for registering dependency injection implementation in Couchbase Lite .NET
    /// </summary>
    public static class Service
    {
        #region Constants

        public static bool IsComplete
        {
            get {
                if(_Collection != null) {
                    var remaining = _RequiredTypes.Intersect(_Collection.Select(x => x.ServiceType));
                    return remaining.Count() == _RequiredTypes.Length;
                }

                foreach(var type in _RequiredTypes) {
                    if(_Provider.GetService(type) == null) {
                        return false;
                    }
                }

                return true;
            }
        }

        /// <summary>
        /// Gets whether or not the services for this program have been finalized.  Further 
        /// attemps to add services will throw exceptions
        /// </summary>
        public static bool IsFinalized => _Collection == null;

        /// <summary>
        /// Gets the service provider that is used to resolve dependencies in the library
        /// </summary>
        public static IServiceProvider Provider
        {
            get {
                if (_Provider == null) {
                    var collection = Interlocked.Exchange(ref _Collection, null);
                    if (collection != null) {
                        _Provider = collection.BuildServiceProvider();
                    }
                }

                return _Provider;
            }
        }

        private static readonly Type[] _RequiredTypes = new[] {
            typeof(IDefaultDirectoryResolver),
            typeof(ISslStreamFactory)
        };

        #endregion

        #region Variables

        private static IServiceCollection _Collection = new ServiceCollection();
        private static IServiceProvider _Provider;

        #endregion

        #region Public Methods

        /// <summary>
        /// Calls an action to add services to the service collection.  This needs to be done
        /// before the library is used.  An exception will be thrown if this method is called
        /// after the provider is created (it is created on the first call to <see cref="Provider"/>
        /// </summary>
        /// <param name="config">The action to configure the service collection</param>
        public static void RegisterServices(Action<IServiceCollection> config)
        {
            if (IsFinalized) {
                throw new InvalidOperationException("Cannot register services after the provider has been created");
            }

            config?.Invoke(_Collection);
        }

        #endregion
    }

    internal static class ServiceProviderExtensions
    {
        #region Public Methods

        public static T TryGetRequiredService<T>(this IServiceProvider provider)
        {
            try {
                return provider.GetRequiredService<T>();
            } catch (InvalidOperationException e) {
                throw new CouchbaseLiteException(StatusCode.MissingDependency,
                    "A required dependency injection class is missing." +
                    "Please ensure that you have called the proper Activate() class in the " +
                    "support assembly (e.g. Couchbase.Lite.Support.UWP.Activate()) or that you " +
                    "have manually registered dependencies via the Couchbase.Lite.DI.Service " +
                    "class.", e);
            }
        }

        #endregion
    }
}