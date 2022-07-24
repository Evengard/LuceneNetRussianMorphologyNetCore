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


    public class GrammarReader
    {
        private string fileName;
        private string fileEncoding = "windows-1251";
        private IList<string> grammarInfo = new List<string>();
        private IDictionary<string, int> inverseIndex = new Dictionary<string, int>();

        public GrammarReader(string fileName)
        {
            this.fileName = fileName;
            setUp();
        }

        public GrammarReader(string fileName, string fileEncoding)
        {
            this.fileName = fileName;
            this.fileEncoding = fileEncoding;
            setUp();
        }

        private void setUp()
        {
            var encoding = Encoding.GetEncoding(fileEncoding);
            StreamReader bufferedReader = new StreamReader(new FileStream(fileName, FileMode.Open, FileAccess.Read), encoding);
            string line = bufferedReader.ReadLine();
            while (!ReferenceEquals(line, null))
            {
                line = line.Trim();
                if (!line.StartsWith("//", StringComparison.Ordinal) && line.Length > 0)
                {
                    string[] strings = line.Split(" ", 2);
                    int i = grammarInfo.Count;
                    inverseIndex[strings[0]] = i;
                    grammarInfo.Insert(i, strings[1]);
                }
                line = bufferedReader.ReadLine();
            }
        }

        public virtual IList<string> GrammarInfo
        {
            get
            {
                return grammarInfo;
            }
        }

        public virtual string[] GrammarInfoAsArray
        {
            get
            {
                return ((List<string>)grammarInfo).ToArray();
            }
        }

        public virtual IDictionary<string, int> GrammarInverseIndex
        {
            get
            {
                return inverseIndex;
            }
        }
    }

}