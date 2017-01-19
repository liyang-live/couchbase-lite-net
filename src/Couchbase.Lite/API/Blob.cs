﻿//
//  Blob.cs
//
//  Author:
//  	Jim Borden  <jim.borden@couchbase.com>
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

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Couchbase.Lite
{
    public sealed class Blob
    {
        public IEnumerable<byte> Data { get; }

        public Stream ContentStream { get; }

        public string ContentType { get; }

        public int Length { get; }

        public string Digest { get; }

        public dynamic Properties { get; }

        public Blob(string contentType, IEnumerable<byte> data)
        {

        }

        public Blob(string contentType, Stream data)
        {

        }

        public Blob(string contentType, Uri fileURL)
        {

        }
    }
}