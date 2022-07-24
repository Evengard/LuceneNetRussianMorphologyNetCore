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

namespace LuceneNetRussianMorphology.Morph
{


    public abstract class BaseLetterDecoderEncoder : LetterDecoderEncoder
    {
        public abstract string cleanString(string s);
        public abstract bool checkCharacter(char c);
        public abstract string decode(int suffixN);
        public abstract int encode(string @string);
        public virtual int[] encodeToArray(string s)
        {
            List<int> integers = new List<int>();
            while (s.Length > 6)
            {
                integers.Add(encode(s.Substring(0, 6)));
                s = s.Substring(6);
            }
            integers.Add(encode(s));
            int[] ints = new int[integers.Count];
            int pos = 0;
            foreach (int? i in integers)
            {
                ints[pos] = i.Value;
                pos++;
            }
            return ints;
        }

        public virtual string decodeArray(int[] array)
        {
            StringBuilder result = new StringBuilder();
            foreach (int i in array)
            {
                result.Append(decode(i));
            }
            return result.ToString();
        }

        public virtual bool checkString(string word)
        {
            for (int i = 0; i < word.Length; i++)
            {
                if (!checkCharacter(word[i]))
                {
                    return false;
                }
            }
            return true;
        }
    }

}