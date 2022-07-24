using System;
using System.Text;

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
namespace LuceneNetRussianMorphology.Morph
{


    [Serializable]
    public class Heuristic
    {
        internal sbyte actualSuffixLength;
        internal string actualNormalSuffix;
        internal short formMorphInfo;
        internal short normalFormMorphInfo;

        public Heuristic(string s)
        {
            string[] strings = s.Split("|");
            actualSuffixLength = sbyte.Parse(strings[0]);
            actualNormalSuffix = strings[1];
            formMorphInfo = short.Parse(strings[2]);
            normalFormMorphInfo = short.Parse(strings[3]);
        }

        public Heuristic(sbyte actualSuffixLength, string actualNormalSuffix, short formMorphInfo, short normalFormMorphInfo)
        {
            this.actualSuffixLength = actualSuffixLength;
            this.actualNormalSuffix = actualNormalSuffix;
            this.formMorphInfo = formMorphInfo;
            this.normalFormMorphInfo = normalFormMorphInfo;
        }

        public virtual StringBuilder transformWord(string w)
        {
            if (w.Length - actualSuffixLength < 0)
            {
                return new StringBuilder(w);
            }
            return new StringBuilder(w.Substring(0, w.Length - actualSuffixLength)).Append(actualNormalSuffix);
        }

        public virtual sbyte ActualSuffixLength
        {
            get
            {
                return actualSuffixLength;
            }
        }

        public virtual string ActualNormalSuffix
        {
            get
            {
                return actualNormalSuffix;
            }
        }

        public virtual short FormMorphInfo
        {
            get
            {
                return formMorphInfo;
            }
        }

        public virtual short NormalFormMorphInfo
        {
            get
            {
                return normalFormMorphInfo;
            }
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

            Heuristic heuristic = (Heuristic)o;

            if (actualSuffixLength != heuristic.actualSuffixLength)
            {
                return false;
            }
            if (formMorphInfo != heuristic.formMorphInfo)
            {
                return false;
            }
            if (normalFormMorphInfo != heuristic.normalFormMorphInfo)
            {
                return false;
            }
            return Equals(actualNormalSuffix, heuristic.actualNormalSuffix);
        }

        public override int GetHashCode()
        {
            int result = actualSuffixLength;
            result = 31 * result + (!ReferenceEquals(actualNormalSuffix, null) ? actualNormalSuffix.GetHashCode() : 0);
            result = 31 * result + formMorphInfo;
            result = 31 * result + normalFormMorphInfo;
            return result;
        }

        public override string ToString()
        {
            return "" + actualSuffixLength + "|" + actualNormalSuffix + "|" + formMorphInfo + "|" + normalFormMorphInfo;
        }
    }

}