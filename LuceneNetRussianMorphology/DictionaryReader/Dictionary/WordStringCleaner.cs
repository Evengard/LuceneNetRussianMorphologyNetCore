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



    public class WordStringCleaner : WordFilter
    {

        private LetterDecoderEncoder decoderEncoder;

        public WordStringCleaner(LetterDecoderEncoder decoderEncoder, WordProcessor wordProcessor) : base(wordProcessor)
        {
            this.decoderEncoder = decoderEncoder;
        }

        public override IList<WordCard> transform(WordCard wordCard)
        {
            wordCard.Base = cleanString(wordCard.Base);
            wordCard.CanonicalForm = cleanString(wordCard.CanonicalForm);
            wordCard.CanonicalSuffix = cleanString(wordCard.CanonicalSuffix);
            IList<FlexiaModel> models = wordCard.WordsForms;
            foreach (FlexiaModel m in models)
            {
                m.Suffix = cleanString(m.Suffix);
                m.Prefix = cleanString(m.Prefix);
                //made correct code
                m.Code = m.Code.Substring(0, 2);
            }
            return new List<WordCard>(new[] { wordCard });
        }


        private string cleanString(string s)
        {
            return decoderEncoder.cleanString(s);
        }
    }

}