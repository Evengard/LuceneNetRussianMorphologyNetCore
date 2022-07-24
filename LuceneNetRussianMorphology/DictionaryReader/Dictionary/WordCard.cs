using System.Collections.Generic;

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
    /// Represent word and all it forms.
    /// </summary>
    public class WordCard
    {
        private string canonicalForm;
        private string @base;
        private string canonicalSuffix;
        private IList<FlexiaModel> wordsForms = new List<FlexiaModel>();

        public WordCard(string canonicalForm, string @base, string canonicalSuffix)
        {
            this.canonicalForm = canonicalForm;
            this.canonicalSuffix = canonicalSuffix;
            this.@base = @base;
        }

        public virtual void addFlexia(FlexiaModel flexiaModel)
        {
            wordsForms.Add(flexiaModel);
        }

        public virtual void removeFlexia(FlexiaModel flexiaModel)
        {
            wordsForms.Remove(flexiaModel);
        }

        public virtual string CanonicalForm
        {
            get
            {
                return canonicalForm;
            }
            set
            {
                canonicalForm = value;
            }
        }

        public virtual string CanonicalSuffix
        {
            get
            {
                return canonicalSuffix;
            }
            set
            {
                canonicalSuffix = value;
            }
        }

        public virtual string Base
        {
            get
            {
                return @base;
            }
            set
            {
                @base = value;
            }
        }

        public virtual IList<FlexiaModel> WordsForms
        {
            get
            {
                return wordsForms;
            }
            set
            {
                wordsForms = value;
            }
        }





        public override string ToString()
        {
            return "WordCard{" + "canonicalForm='" + canonicalForm + '\'' + ", base='" + @base + '\'' + ", canonicalSuffix='" + canonicalSuffix + '\'' + ", wordsForms=" + wordsForms + '}';
        }
    }

}