using LuceneNetRussianMorphology.Morph;
using System;
using System.Collections.Generic;
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




    //todo made refactoring this class
    public class StatisticsCollector : WordProcessor
    {
        private SortedDictionary<string, ISet<Heuristic>> inverseIndex = new SortedDictionary<string, ISet<Heuristic>>();
        private IDictionary<ISet<Heuristic>, int> ruleInverseIndex = new Dictionary<ISet<Heuristic>, int>();
        private IList<ISet<Heuristic>> rules = new List<ISet<Heuristic>>();
        private GrammarReader grammarReader;
        private LetterDecoderEncoder decoderEncoder;


        public StatisticsCollector(GrammarReader grammarReader, LetterDecoderEncoder decoderEncoder)
        {
            this.grammarReader = grammarReader;
            this.decoderEncoder = decoderEncoder;
        }

        public virtual void process(WordCard wordCard)
        {
            cleanWordCard(wordCard);
            string normalStringMorph = wordCard.WordsForms[0].Code;

            foreach (FlexiaModel fm in wordCard.WordsForms)
            {
                Heuristic heuristic = createEvristic(wordCard.Base, wordCard.CanonicalSuffix, fm, normalStringMorph);
                string form = revertWord(fm.create(wordCard.Base));
                bool retrieved = inverseIndex.TryGetValue(form, out var suffixHeuristics);
                if (!retrieved)
                {
                    suffixHeuristics = new HashSet<Heuristic>();
                    inverseIndex.Add(form, suffixHeuristics);
                }
                suffixHeuristics.Add(heuristic);
            }
        }

        private void cleanWordCard(WordCard wordCard)
        {
            wordCard.Base = cleanString(wordCard.Base);
            wordCard.CanonicalForm = cleanString(wordCard.CanonicalForm);
            wordCard.CanonicalSuffix = cleanString(wordCard.CanonicalSuffix);
            IList<FlexiaModel> models = wordCard.WordsForms;
            foreach (FlexiaModel m in models)
            {
                m.Suffix = cleanString(m.Suffix);
                m.Prefix = cleanString(m.Prefix);
            }
        }


        public virtual void saveHeuristic(string fileName)
        {

            IDictionary<int, int> dist = new SortedDictionary<int, int>();
            ISet<Heuristic> prevSet = null;
            int count = 0;
            foreach (string key in inverseIndex.Keys)
            {
                ISet<Heuristic> currentSet = inverseIndex[key];
                if (!currentSet.SetEquals(prevSet))
                {
                    int? d = dist[key.Length];
                    dist[key.Length] = 1 + (d == null ? 0 : d.Value);
                    prevSet = currentSet;
                    count++;
                    if (!ruleInverseIndex.ContainsKey(currentSet))
                    {
                        ruleInverseIndex[currentSet] = rules.Count;
                        rules.Add(currentSet);
                    }
                }
            }
            Console.WriteLine("Word with diffirent rules " + count);
            Console.WriteLine("All ivers words " + inverseIndex.Count);
            Console.WriteLine(dist);
            Console.WriteLine("diffirent rule count " + ruleInverseIndex.Count);
            Heuristic[][] heuristics = new Heuristic[ruleInverseIndex.Count][];
            int index = 0;
            foreach (ISet<Heuristic> hs in rules)
            {
                heuristics[index] = new Heuristic[hs.Count];
                int indexj = 0;
                foreach (Heuristic h in hs)
                {
                    heuristics[index][indexj] = h;
                    indexj++;
                }
                index++;
            }

            int[][] ints = new int[count][];
            short[] rulesId = new short[count];
            count = 0;
            prevSet = null;
            foreach (string key in inverseIndex.Keys)
            {
                ISet<Heuristic> currentSet = inverseIndex[key];
                if (!currentSet.SetEquals(prevSet))
                {
                    int[] word = decoderEncoder.encodeToArray(key);
                    ints[count] = word;
                    rulesId[count] = (short)ruleInverseIndex[currentSet];
                    count++;
                    prevSet = currentSet;
                }
            }
            MorphologyImpl morphology = new MorphologyImpl(ints, rulesId, heuristics, grammarReader.GrammarInfoAsArray);
            morphology.writeToFile(fileName);
        }

        private string revertWord(string s)
        {
            StringBuilder result = new StringBuilder();
            for (int i = 1; i <= s.Length; i++)
            {
                result.Append(s[s.Length - i]);
            }
            return result.ToString();
        }


        private Heuristic createEvristic(string wordBase, string canonicalSuffix, FlexiaModel fm, string normalSuffixForm)
        {
            string form = fm.create(wordBase);
            string normalForm = wordBase + canonicalSuffix;
            int length = getCommonLength(form, normalForm);
            int actualSuffixLengh = form.Length - length;
            string actualNormalSuffix = normalForm.Substring(length);
            int? integer = grammarReader.GrammarInverseIndex[fm.Code];
            int? nf = grammarReader.GrammarInverseIndex[normalSuffixForm];
            return new Heuristic((sbyte)actualSuffixLengh, actualNormalSuffix, (short)integer.Value, (short)nf.Value);
        }

        public static int getCommonLength(string s1, string s2)
        {
            int length = Math.Min(s1.Length, s2.Length);
            for (int i = 0; i < length; i++)
            {
                if (s1[i] != s2[i])
                {
                    return i;
                }
            }
            return length;
        }

        private string cleanString(string s)
        {
            return decoderEncoder.cleanString(s);
        }

    }

}