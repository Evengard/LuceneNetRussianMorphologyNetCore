using System;
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
namespace LuceneNetRussianMorphology.Morph
{



    public class MorphologyImpl : Morphology
    {
        protected internal int[][] separators;
        protected internal short[] rulesId;
        protected internal Heuristic[][] rules;
        protected internal string[] grammarInfo;
        protected internal LetterDecoderEncoder decoderEncoder;


        public MorphologyImpl(string fileName, LetterDecoderEncoder decoderEncoder)
        {
            readFromFile(fileName);
            this.decoderEncoder = decoderEncoder;
        }

        public MorphologyImpl(Stream inputStream, LetterDecoderEncoder decoderEncoder)
        {
            readFromInputStream(inputStream);
            this.decoderEncoder = decoderEncoder;
        }

        public MorphologyImpl(int[][] separators, short[] rulesId, Heuristic[][] rules, string[] grammarInfo)
        {
            this.separators = separators;
            this.rulesId = rulesId;
            this.rules = rules;
            this.grammarInfo = grammarInfo;
        }

        public virtual IList<string> getNormalForms(string s)
        {
            List<string> result = new List<string>();
            int[] ints = decoderEncoder.encodeToArray(revertWord(s));
            int ruleId = findRuleId(ints);
            bool notSeenEmptyString = true;
            foreach (Heuristic h in rules[rulesId[ruleId]])
            {
                string e = h.transformWord(s).ToString();
                if (e.Length > 0)
                {
                    result.Add(e);
                }
                else if (notSeenEmptyString)
                {
                    result.Add(s);
                    notSeenEmptyString = false;
                }
            }
            return result;
        }

        public virtual IList<string> getMorphInfo(string s)
        {
            List<string> result = new List<string>();
            int[] ints = decoderEncoder.encodeToArray(revertWord(s));
            int ruleId = findRuleId(ints);
            foreach (Heuristic h in rules[rulesId[ruleId]])
            {
                result.Add(h.transformWord(s).Append("|").Append(grammarInfo[h.FormMorphInfo]).ToString());
            }
            return result;
        }

        protected internal virtual int findRuleId(int[] ints)
        {
            int low = 0;
            int high = separators.Length - 1;
            int mid = 0;
            while (low <= high)
            {
                mid = (int)((uint)(low + high) >> 1);
                int[] midVal = separators[mid];

                int comResult = compareToInts(ints, midVal);
                if (comResult > 0)
                {
                    low = mid + 1;
                }
                else if (comResult < 0)
                {
                    high = mid - 1;
                }
                else
                {
                    break;
                }
            }
            if (compareToInts(ints, separators[mid]) >= 0)
            {
                return mid;
            }
            else
            {
                return mid - 1;
            }

        }

        private int compareToInts(int[] i1, int[] i2)
        {
            int minLength = Math.Min(i1.Length, i2.Length);
            for (int i = 0; i < minLength; i++)
            {
                int i3 = i1[i].CompareTo(i2[i]);
                if (i3 != 0)
                {
                    return i3;
                }
            }
            return i1.Length - i2.Length;
        }

        public virtual void writeToFile(string fileName)
        {
            StreamWriter writer = new StreamWriter(new FileStream(fileName, FileMode.Create, FileAccess.Write), Encoding.UTF8);
            writer.Write(separators.Length + "\n");
            foreach (int[] i in separators)
            {
                writer.Write(i.Length + "\n");
                foreach (int j in i)
                {
                    writer.Write(j + "\n");
                }
            }
            foreach (short i in rulesId)
            {
                writer.Write(i + "\n");
            }
            writer.Write(rules.Length + "\n");
            foreach (Heuristic[] heuristics in rules)
            {
                writer.Write(heuristics.Length + "\n");
                foreach (Heuristic heuristic in heuristics)
                {
                    writer.Write(heuristic.ToString() + "\n");
                }
            }
            writer.Write(grammarInfo.Length + "\n");
            foreach (string s in grammarInfo)
            {
                writer.Write(s + "\n");
            }
            writer.Close();
        }

        public virtual void readFromFile(string fileName)
        {
            FileStream inputStream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            readFromInputStream(inputStream);
        }

        private void readFromInputStream(Stream inputStream)
        {
            StreamReader bufferedReader = new StreamReader(inputStream, Encoding.UTF8);
            string s = bufferedReader.ReadLine();
            int amount = Convert.ToInt32(s);

            readSeparators(bufferedReader, amount);

            readRulesId(bufferedReader, amount);

            readRules(bufferedReader);
            readGrammaInfo(bufferedReader);
            bufferedReader.Close();
        }

        private void readGrammaInfo(StreamReader bufferedReader)
        {
            string s;
            int amount;
            s = bufferedReader.ReadLine();
            amount = int.Parse(s);
            grammarInfo = new string[amount];
            for (int i = 0; i < amount; i++)
            {
                grammarInfo[i] = bufferedReader.ReadLine();
            }
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
                int ruleLength = int.Parse(s1);
                rules[i] = new Heuristic[ruleLength];
                for (int j = 0; j < ruleLength; j++)
                {
                    rules[i][j] = new Heuristic(bufferedReader.ReadLine());
                }
            }
        }

        private void readRulesId(StreamReader bufferedReader, int amount)
        {
            rulesId = new short[amount];
            for (int i = 0; i < amount; i++)
            {
                string s1 = bufferedReader.ReadLine();
                rulesId[i] = short.Parse(s1);
            }
        }

        private void readSeparators(StreamReader bufferedReader, int amount)
        {
            separators = new int[amount][];
            for (int i = 0; i < amount; i++)
            {
                string s1 = bufferedReader.ReadLine();
                int wordLenght = int.Parse(s1);
                separators[i] = new int[wordLenght];
                for (int j = 0; j < wordLenght; j++)
                {
                    separators[i][j] = int.Parse(bufferedReader.ReadLine());
                }
            }
        }

        protected internal virtual string revertWord(string s)
        {
            StringBuilder result = new StringBuilder();
            for (int i = 1; i <= s.Length; i++)
            {
                result.Append(s[s.Length - i]);
            }
            return result.ToString();
        }
    }

}