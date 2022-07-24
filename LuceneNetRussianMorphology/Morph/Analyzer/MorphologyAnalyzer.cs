using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Core;
using Lucene.Net.Analysis.Payloads;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Util;
using System;
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

namespace LuceneNetRussianMorphology.Morph.Analyzer
{
    /*using Analyzer = org.apache.lucene.analysis.Analyzer;
	using LowerCaseFilter = org.apache.lucene.analysis.LowerCaseFilter;
	using TokenFilter = org.apache.lucene.analysis.TokenFilter;
	using PayloadEncoder = org.apache.lucene.analysis.payloads.PayloadEncoder;
	using PayloadHelper = org.apache.lucene.analysis.payloads.PayloadHelper;
	using StandardTokenizer = org.apache.lucene.analysis.standard.StandardTokenizer;
	using BytesRef = org.apache.lucene.util.BytesRef;*/



    public class MorphologyAnalyzer : Lucene.Net.Analysis.Analyzer
    {
        private LuceneMorphology luceneMorph;

        public MorphologyAnalyzer(LuceneMorphology luceneMorph)
        {
            this.luceneMorph = luceneMorph;
        }

        public MorphologyAnalyzer(string pathToMorph, LetterDecoderEncoder letterDecoderEncoder)
        {
            luceneMorph = new LuceneMorphology(pathToMorph, letterDecoderEncoder);
        }

        public MorphologyAnalyzer(Stream inputStream, LetterDecoderEncoder letterDecoderEncoder)
        {
            luceneMorph = new LuceneMorphology(inputStream, letterDecoderEncoder);
        }

        protected override TokenStreamComponents CreateComponents(string s, TextReader reader)
        {

            StandardTokenizer src = new StandardTokenizer(LuceneVersion.LUCENE_48, reader);
            
            //IPayloadEncoder encoder = new PayloadEncoderAnonymousInnerClass(this);

            TokenFilter filter = new LowerCaseFilter(LuceneVersion.LUCENE_48, src);
            filter = new MorphologyFilter(filter, luceneMorph);

            return new TokenStreamComponents(src, filter);
        }
        /*
        private class PayloadEncoderAnonymousInnerClass : IPayloadEncoder
        {
            private readonly MorphologyAnalyzer outerInstance;

            public PayloadEncoderAnonymousInnerClass(MorphologyAnalyzer outerInstance)
            {
                this.outerInstance = outerInstance;
            }

            public BytesRef Encode(char[] buffer)
            {
                float payload = Convert.ToSingle(new string(buffer));
                Console.WriteLine(payload);
                byte[] bytes = PayloadHelper.EncodeSingle(payload);
                return new BytesRef(bytes, 0, bytes.Length);
            }

            public BytesRef Encode(char[] buffer, int offset, int length)
            {
                float payload = Convert.ToSingle(new string(buffer, offset, length));
                Console.WriteLine(payload);
                byte[] bytes = PayloadHelper.EncodeSingle(payload);

                return new BytesRef(bytes, 0, bytes.Length);
            }

        }*/
    }

}