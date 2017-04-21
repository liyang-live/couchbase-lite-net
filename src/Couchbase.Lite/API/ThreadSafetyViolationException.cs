﻿// 
// ThreadSafetyViolationException.cs
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

namespace Couchbase.Lite
{
    /// <summary>
    /// An exception that is thrown when an <see cref="IThreadSafe"/> object is
    /// accessed inappropriately
    /// </summary>
    public sealed class ThreadSafetyViolationException : Exception
    {
        #region Constants

        private const string ThreadMessage =
            "An attempt was made to use a thread safe object on a different thread than it was created";

        #endregion

        #region Constructors

        internal ThreadSafetyViolationException() : base(ThreadMessage)
        {

        }

        #endregion
    }
}