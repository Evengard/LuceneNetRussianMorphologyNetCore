using LuceneNetRussianMorphology.Russian;
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

namespace LuceneNetRussianMorphology.DictionaryReader.Generator
{
    using LuceneNetRussianMorphology.DictionaryReader.Dictionary;
    using RussianLetterDecoderEncoder = RussianLetterDecoderEncoder;



    public class RussianHeuristicBuilder
    {
        public static void Main(string[] args)
        {
            GrammarReader grammarInfo = new GrammarReader("dictonary/Dicts/Morph/rgramtab.tab");
            RussianLetterDecoderEncoder decoderEncoder = new RussianLetterDecoderEncoder();

            DictionaryReader dictionaryReader = new DictionaryReader("dictonary/Dicts/SrcMorph/RusSrc/morphs.mrd", new HashSet<string>());

            StatisticsCollector statisticsCollector = new StatisticsCollector(grammarInfo, decoderEncoder);
            WordCleaner wordCleaner = new WordCleaner(decoderEncoder, statisticsCollector);
            WordStringCleaner wordStringCleaner = new WordStringCleaner(decoderEncoder, wordCleaner);
            RemoveFlexiaWithPrefixes removeFlexiaWithPrefixes = new RemoveFlexiaWithPrefixes(wordStringCleaner);
            RussianAdvSplitterFilter russianAdvSplitterFilter = new RussianAdvSplitterFilter(removeFlexiaWithPrefixes);
            dictionaryReader.process(russianAdvSplitterFilter);
            statisticsCollector.saveHeuristic("russian/src/main/resources/org/apache/lucene/morphology/russian/morph.info");

        }
    }

}