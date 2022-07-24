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

namespace LuceneNetRussianMorphology.DictionaryReader.Dictionary
{



    /// <summary>
    /// This class contain logic how read
    /// dictionary and produce word with it all forms.
    /// </summary>
    public class DictionaryReader
    {
        private string fileName;
        private string fileEncoding = "windows-1251";
        private IList<IList<FlexiaModel>> wordsFlexias = new List<IList<FlexiaModel>>();
        private ISet<string> ignoredForm;

        public DictionaryReader(string fileName, ISet<string> ignoredForm)
        {
            this.fileName = fileName;
            this.ignoredForm = ignoredForm;
        }


        public virtual void process(WordProcessor wordProcessor)
        {
            var encoding = Encoding.GetEncoding(fileEncoding);
            StreamReader bufferedReader = new StreamReader(new FileStream(fileName, FileMode.Open, FileAccess.Read), encoding);
            readFlexias(bufferedReader);
            skipBlock(bufferedReader);
            skipBlock(bufferedReader);
            readPrefix(bufferedReader);
            readWords(bufferedReader, wordProcessor);
        }


        private void readWords(StreamReader reader, WordProcessor wordProcessor)
        {
            string s = reader.ReadLine();
            int count = int.Parse(s);
            int actual = 0;
            for (int i = 0; i < count; i++)
            {
                s = reader.ReadLine();
                if (i % 10000 == 0)
                {
                    Console.WriteLine("Proccess " + i + " wordBase of " + count);
                }

                WordCard card = buildForm(s);

                if (card == null)
                {
                    continue;
                }

                wordProcessor.process(card);
                actual++;

            }
            Console.WriteLine("Finished word processing actual words " + actual);
        }

        private WordCard buildForm(string s)
        {
            string[] wd = s.Split(" ");
            string wordBase = wd[0].ToLower();
            if (wordBase.StartsWith("-", StringComparison.Ordinal))
            {
                return null;
            }
            wordBase = "#".Equals(wordBase) ? "" : wordBase;
            IList<FlexiaModel> models = wordsFlexias[int.Parse(wd[1])];
            FlexiaModel flexiaModel = models[0];
            if (models.Count == 0 || ignoredForm.Contains(flexiaModel.Code))
            {
                return null;
            }

            WordCard card = new WordCard(flexiaModel.create(wordBase), wordBase, flexiaModel.Suffix);

            foreach (FlexiaModel fm in models)
            {
                card.addFlexia(fm);
            }
            return card;
        }


        private void skipBlock(StreamReader reader)
        {
            string s = reader.ReadLine();
            int count = int.Parse(s);
            for (int i = 0; i < count; i++)
            {
                reader.ReadLine();
            }
        }


        private void readPrefix(StreamReader reader)
        {
            string s = reader.ReadLine();
            int count = int.Parse(s);
            for (int i = 0; i < count; i++)
            {
                reader.ReadLine();
            }
        }

        private void readFlexias(StreamReader reader)
        {
            string s = reader.ReadLine();
            int count = int.Parse(s);
            for (int i = 0; i < count; i++)
            {
                s = reader.ReadLine();
                List<FlexiaModel> flexiaModelArrayList = new List<FlexiaModel>();
                wordsFlexias.Add(flexiaModelArrayList);
                foreach (string line in s.Split("%"))
                {
                    addFlexia(flexiaModelArrayList, line);
                }
            }
        }

        private void addFlexia(List<FlexiaModel> flexiaModelArrayList, string line)
        {
            string[] fl = line.Split("*");
            // we inored all forms thats
            if (fl.Length == 3)
            {
                //System.out.println(line);
                flexiaModelArrayList.Add(new FlexiaModel(fl[1], fl[0].ToLower(), fl[2].ToLower()));
            }
            if (fl.Length == 2)
            {
                flexiaModelArrayList.Add(new FlexiaModel(fl[1], fl[0].ToLower(), ""));
            }
        }

    }

}