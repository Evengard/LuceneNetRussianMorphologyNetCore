using LuceneNetRussianMorphology.Helpers;
using System.Collections.Generic;
using System.IO;
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
namespace LuceneNetRussianMorphology.DictionaryReader.Dictionary
{


    public class RussianAdvSplitterFilter : WordFilter
    {
        private string code;

        public RussianAdvSplitterFilter(WordProcessor wordProcessor) : base(wordProcessor)
        {
            code = new StreamReader(ResourceHelpers.GetResource("russian-adv-main-code.txt"), Encoding.GetEncoding("windows-1251")).ReadLine();
        }

        public override IList<WordCard> transform(WordCard wordCard)
        {
            List<WordCard> result = new List<WordCard>();
            result.Append(wordCard);

            string baseWord = "";
            string canonicalForm = "";
            string canonicalSuffix = "";
            IList<FlexiaModel> flexiaModels = new List<FlexiaModel>();
            foreach (FlexiaModel flexiaModel in wordCard.WordsForms)
            {
                if (flexiaModel.Prefix.Length > 0)
                {
                    flexiaModels.Add(new FlexiaModel(flexiaModel.Code, flexiaModel.Suffix, ""));
                }
                if (flexiaModel.Prefix.Length > 0 && flexiaModel.Code.Equals(code))
                {
                    baseWord = flexiaModel.Prefix + wordCard.Base;
                    canonicalForm = flexiaModel.Code;
                    canonicalSuffix = flexiaModel.Suffix;
                }
            }

            if (baseWord.Length > 0)
            {
                WordCard wc = new WordCard(canonicalForm, baseWord, canonicalSuffix);
                wc.WordsForms = flexiaModels;
                result.Append(wc);
            }

            return result;
        }
    }

}