using LuceneNetRussianMorphology.Morph;
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



    public class WordCleaner : WordFilter
    {

        private LetterDecoderEncoder decoderEncoder;

        public WordCleaner(LetterDecoderEncoder decoderEncoder, WordProcessor wordProcessor) : base(wordProcessor)
        {
            this.decoderEncoder = decoderEncoder;
        }

        public override IList<WordCard> transform(WordCard wordCard)
        {
            string word = wordCard.Base + wordCard.CanonicalSuffix;

            if (word.Contains("-"))
            {
                return new List<WordCard>();
            }
            if (!decoderEncoder.checkString(word))
            {
                return new List<WordCard>();
            }

            IList<FlexiaModel> flexiaModelsToRemove = new List<FlexiaModel>();
            foreach (FlexiaModel fm in wordCard.WordsForms)
            {
                if (!decoderEncoder.checkString(fm.create(wordCard.Base)) || fm.create(wordCard.Base).Contains("-"))
                {
                    flexiaModelsToRemove.Add(fm);
                }
            }
            foreach (FlexiaModel fm in flexiaModelsToRemove)
            {
                wordCard.removeFlexia(fm);
            }

            return new List<WordCard>(new[] { wordCard });
        }
    }

}