using LuceneNetRussianMorphology.Morph;
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
namespace LuceneNetRussianMorphology.English
{


    public class EnglishLetterDecoderEncoder : LetterDecoderEncoder
    {
        public const int ENGLISH_SMALL_LETTER_OFFSET = 96;
        public static int SUFFIX_LENGTH = 6;
        public const int DASH_CHAR = 45;
        public const int DASH_CODE = 27;

        public virtual int encode(string @string)
        {
            if (@string.Length > 6)
            {
                throw new SuffixToLongException("Suffix length should not be greater then " + 12);
            }
            int result = 0;
            for (int i = 0; i < @string.Length; i++)
            {
                int c = @string[i] - ENGLISH_SMALL_LETTER_OFFSET;
                if (c == 45 - ENGLISH_SMALL_LETTER_OFFSET)
                {
                    c = DASH_CODE;
                }
                if (c < 0 || c > 27)
                {
                    throw new WrongCharaterException("Symbol " + @string[i] + " is not small cirillic letter");
                }
                result = result * 28 + c;
            }
            for (int i = @string.Length; i < 6; i++)
            {
                result *= 28;
            }
            return result;
        }

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


        public virtual string decode(int suffixN)
        {
            StringBuilder result = new StringBuilder();
            while (suffixN > 27)
            {
                int ci = suffixN % 28 + ENGLISH_SMALL_LETTER_OFFSET;
                if (ci == ENGLISH_SMALL_LETTER_OFFSET)
                {
                    suffixN /= 28;
                    continue;
                }
                if (ci == DASH_CODE + ENGLISH_SMALL_LETTER_OFFSET)
                {
                    ci = DASH_CHAR;
                }
                result.Insert(0, (char)ci);
                suffixN /= 28;
            }
            long c = suffixN + ENGLISH_SMALL_LETTER_OFFSET;
            if (c == DASH_CODE + ENGLISH_SMALL_LETTER_OFFSET)
            {
                c = DASH_CHAR;
            }
            result.Insert(0, (char)c);
            return result.ToString();
        }

        public virtual bool checkCharacter(char c)
        {
            int code = c;
            if (code == 45)
            {
                return true;
            }
            code -= ENGLISH_SMALL_LETTER_OFFSET;
            return code > 0 && code < 27;
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

        public virtual string cleanString(string s)
        {
            return s;
        }

    }

}