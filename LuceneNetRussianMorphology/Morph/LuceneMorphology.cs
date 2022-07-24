using System.Collections.Generic;
using System.IO;

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



    public class LuceneMorphology : MorphologyImpl
    {

        public LuceneMorphology(string fileName, LetterDecoderEncoder decoderEncoder) : base(fileName, decoderEncoder)
        {
        }

        public LuceneMorphology(Stream inputStream, LetterDecoderEncoder decoderEncoder) : base(inputStream, decoderEncoder)
        {
        }

        protected internal virtual void readRules(StreamReader bufferedReader)
        {
            string s;
            int amount;
            s = bufferedReader.ReadLine();
            amount = int.Parse(s);
            rules = new Heuristic[amount][];
            for (int i = 0; i < amount; i++)
            {
                string s1 = bufferedReader.ReadLine();
                int ruleLenght = int.Parse(s1);
                Heuristic[] heuristics = new Heuristic[ruleLenght];
                for (int j = 0; j < ruleLenght; j++)
                {
                    heuristics[j] = new Heuristic(bufferedReader.ReadLine());
                }
                rules[i] = modeifyHeuristic(heuristics);
            }
        }


        private Heuristic[] modeifyHeuristic(Heuristic[] heuristics)
        {
            List<Heuristic> result = new List<Heuristic>();
            foreach (Heuristic heuristic in heuristics)
            {
                bool isAdded = true;
                foreach (Heuristic ch in result)
                {
                    isAdded = isAdded && !(ch.ActualNormalSuffix.Equals(heuristic.ActualNormalSuffix) && ch.ActualSuffixLength == heuristic.ActualSuffixLength);
                }
                if (isAdded)
                {
                    result.Add(heuristic);
                }
            }
            return result.ToArray();
        }

        public virtual bool checkString(string s)
        {
            return decoderEncoder.checkString(s);
        }
    }

}