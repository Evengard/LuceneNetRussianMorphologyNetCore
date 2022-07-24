/// <summary>
/// Copyright 2009 Alexander Kuznetsov 
/// 
/// Licensed under the Apache License, Version 2.0 (the "License");
/// you may not use this file except in compliance with the License.
/// You may obtain a copy of the License at
/// 
///     http://www.apache.org/licenses/LICENSE-2.0
/// 
/// Unless required by applicable law or agreed to in writing, software
/// distributed under the License is distributed on an "AS IS" BASIS,
/// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
/// See the License for the specific language governing permissions and
/// limitations under the License.
/// </summary>
namespace LuceneNetRussianMorphology.DictionaryReader.Dictionary
{

    /// <summary>
    /// Represent information of how word form created form it imutible part.
    /// </summary>
    public class FlexiaModel
    {
        private string code;
        private string suffix;
        private string prefix;

        public FlexiaModel(string code, string suffix, string prefix)
        {
            this.code = code;
            this.suffix = suffix;
            this.prefix = prefix;
        }

        public virtual string Code
        {
            get
            {
                return code;
            }
            set
            {
                code = value;
            }
        }


        public virtual string Suffix
        {
            get
            {
                return suffix;
            }
            set
            {
                suffix = value;
            }
        }


        public virtual string Prefix
        {
            get
            {
                return prefix;
            }
            set
            {
                prefix = value;
            }
        }


        public virtual string create(string s)
        {
            return prefix + s + suffix;
        }

        public override string ToString()
        {
            return "FlexiaModel{" + "code='" + code + '\'' + ", suffix='" + suffix + '\'' + ", prefix='" + prefix + '\'' + '}';
        }

        public override bool Equals(object o)
        {
            if (this == o)
            {
                return true;
            }
            if (o == null || GetType() != o.GetType())
            {
                return false;
            }

            FlexiaModel that = (FlexiaModel)o;

            if (!Equals(code, that.code))
            {
                return false;
            }
            if (!Equals(prefix, that.prefix))
            {
                return false;
            }
            return Equals(suffix, that.suffix);
        }

        public override int GetHashCode()
        {
            int result = !ReferenceEquals(code, null) ? code.GetHashCode() : 0;
            result = 31 * result + (!ReferenceEquals(suffix, null) ? suffix.GetHashCode() : 0);
            result = 31 * result + (!ReferenceEquals(prefix, null) ? prefix.GetHashCode() : 0);
            return result;
        }
    }

}