﻿// 
//  ISelectResultContainer.cs
// 
//  Author:
//   Jim Borden  <jim.borden@couchbase.com>
// 
//  Copyright (c) 2017 Couchbase, Inc All rights reserved.
// 
//  Licensed under the Apache License, Version 2.0 (the "License");
//  you may not use this file except in compliance with the License.
//  You may obtain a copy of the License at
// 
//  http://www.apache.org/licenses/LICENSE-2.0
// 
//  Unless required by applicable law or agreed to in writing, software
//  distributed under the License is distributed on an "AS IS" BASIS,
//  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//  See the License for the specific language governing permissions and
//  limitations under the License.
// 
#if CBL_LINQ
using Couchbase.Lite.Internal.Serialization;
using LiteCore.Interop;
using Newtonsoft.Json;
using System;
using System.Collections;

namespace Couchbase.Lite.Linq
{
    internal interface ISelectResultContainer
    {
        #region Properties

        object Results { get; }

        #endregion

        #region Public Methods

        void Populate(FLArrayIterator iterator, SharedStringCache sharedStrings);

        #endregion
    }

    internal sealed class SelectResultListContainer : ISelectResultContainer
    {
        #region Variables

        private readonly Type _elementType;

        #endregion

        #region Properties

        public object Results { get; private set; }

        #endregion

        #region Constructors

        public SelectResultListContainer(Type listType)
        {
            _elementType = listType;
        }

        #endregion

        #region ISelectResultContainer

        public unsafe void Populate(FLArrayIterator iterator, SharedStringCache sharedStrings)
        {
            var i = iterator;
            var serializer = JsonSerializer.CreateDefault();
            var count = Native.FLArrayIterator_GetCount(&i);
            Results = Activator.CreateInstance(_elementType.MakeArrayType(), (int) count);
            for (var index = 0U; index < count; index++) {
                using (var reader = new JsonFLValueReader(Native.FLArrayIterator_GetValueAt(&i, index), sharedStrings)) {
                    (Results as IList)[(int)index] = serializer.Deserialize(reader);
                }
            }
        }

        #endregion
    }
}
#endif