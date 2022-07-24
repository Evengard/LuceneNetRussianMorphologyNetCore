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

namespace LuceneNetRussianMorphology.Russian
{

    /// <summary>
    /// This helper class allow encode suffix of russian word
    /// to long value and decode from it.
    /// Assumed that suffix contains only small russian letters and dash.
    /// Also assumed that letter � and � coinsed.
    /// </summary>
    public class RussianLetterDecoderEncoder : LetterDecoderEncoder
    {
        public const int RUSSIAN_SMALL_LETTER_OFFSET = 1071;
        public const int WORD_PART_LENGHT = 6;
        public const int EE_CHAR = 34;
        public const int E_CHAR = 6;
        public const int DASH_CHAR = 45;
        public const int DASH_CODE = 33;

        public virtual int encode(string @string)
        {
            if (@string.Length > WORD_PART_LENGHT)
            {
                throw new SuffixToLongException("Suffix length should not be greater then " + WORD_PART_LENGHT + " " + @string);
            }
            int result = 0;
            for (int i = 0; i < @string.Length; i++)
            {
                int c = @string[i] - RUSSIAN_SMALL_LETTER_OFFSET;
                if (c == 45 - RUSSIAN_SMALL_LETTER_OFFSET)
                {
                    c = DASH_CODE;
                }
                if (c == EE_CHAR)
                {
                    c = E_CHAR;
                }
                if (c < 0 || c > 33)
                {
                    throw new WrongCharaterException("Symbol " + @string[i] + " is not small cirillic letter");
                }
                result = result * 34 + c;
            }
            for (int i = @string.Length; i < WORD_PART_LENGHT; i++)
            {
                result *= 34;
            }
            return result;
        }

        public virtual int[] encodeToArray(string s)
        {
            LinkedList<int> integers = new LinkedList<int>();
            while (s.Length > WORD_PART_LENGHT)
            {
                integers.AddLast(encode(s.Substring(0, WORD_PART_LENGHT)));
                s = s.Substring(WORD_PART_LENGHT);
            }
            integers.AddLast(encode(s));
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
            while (suffixN > 33)
            {
                int ci = suffixN % 34 + RUSSIAN_SMALL_LETTER_OFFSET;
                if (ci == RUSSIAN_SMALL_LETTER_OFFSET)
                {
                    suffixN /= 34;
                    continue;
                }
                if (ci == DASH_CODE + RUSSIAN_SMALL_LETTER_OFFSET)
                {
                    ci = DASH_CHAR;
                }
                result.Insert(0, (char)ci);
                suffixN /= 34;
            }
            long c = suffixN + RUSSIAN_SMALL_LETTER_OFFSET;
            if (c == DASH_CODE + RUSSIAN_SMALL_LETTER_OFFSET)
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
            code -= RUSSIAN_SMALL_LETTER_OFFSET;
            return code > 0 && code < 33;
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
            return s.Replace((char)(EE_CHAR + RUSSIAN_SMALL_LETTER_OFFSET), (char)(E_CHAR + RUSSIAN_SMALL_LETTER_OFFSET));
        }
    }

}