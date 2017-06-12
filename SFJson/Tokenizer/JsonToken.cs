using System;
using System.Collections.Generic;

namespace SFJson
{
    public abstract class JsonToken
    {
        internal bool IsQuoted;
        public string Name;
        public List<JsonToken> Children = new List<JsonToken>();

        public abstract JsonType JsonType { get; }
        
        public abstract T GetValue<T>();
        public abstract object GetValue(Type type);
    }
}