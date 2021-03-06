﻿#region License
// Copyright (c) 2007 James Newton-King
//
// Permission is hereby granted, free of charge, to any person
// obtaining a copy of this software and associated documentation
// files (the "Software"), to deal in the Software without
// restriction, including without limitation the rights to use,
// copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following
// conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Newtonsoft.Json.Tests.Documentation.Samples.Serializer
{
    public class SerializeSerializationBinder
    {
        #region Types
        public class KnownTypesBinder : SerializationBinder
        {
            public IList<Type> KnownTypes { get; set; }

            public override Type BindToType(string assemblyName, string typeName)
            {
                return KnownTypes.SingleOrDefault(t => t.Name == typeName);
            }

            public override void BindToName(Type serializedType, out string assemblyName, out string typeName)
            {
                assemblyName = null;
                typeName = serializedType.Name;
            }
        }

        public class Car
        {
            public string Maker { get; set; }
            public string Model { get; set; }
        }
        #endregion

        public void Example()
        {
            #region Usage
            KnownTypesBinder knownTypesBinder = new KnownTypesBinder
            {
                KnownTypes = new List<Type> { typeof(Car) }
            };

            Car car = new Car
            {
                Maker = "Ford",
                Model = "Explorer"
            };

            string json = JsonConvert.SerializeObject(car, Formatting.Indented, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Objects,
                Binder = knownTypesBinder
            });

            Console.WriteLine(json);
            // {
            //   "$type": "Car",
            //   "Maker": "Ford",
            //   "Model": "Explorer"
            // }

            object newValue = JsonConvert.DeserializeObject(json, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Objects,
                Binder = knownTypesBinder
            });

            Console.WriteLine(newValue.GetType().Name);
            // Car
            #endregion
        }
    }
}