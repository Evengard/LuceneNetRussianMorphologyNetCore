using Lucene.Net.Analysis;
using Lucene.Net.Analysis.TokenAttributes;
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

namespace LuceneNetRussianMorphology.Morph.Analyzer
{
    /*
	using TokenFilter = org.apache.lucene.analysis.TokenFilter;
	using TokenStream = org.apache.lucene.analysis.TokenStream;
	using CharTermAttribute = org.apache.lucene.analysis.tokenattributes.CharTermAttribute;
	using KeywordAttribute = org.apache.lucene.analysis.tokenattributes.KeywordAttribute;
	using PositionIncrementAttribute = org.apache.lucene.analysis.tokenattributes.PositionIncrementAttribute;*/



    public class MorphologyFilter : TokenFilter
    {
        private LuceneMorphology luceneMorph;
        private IEnumerator<string>? iterator;
        private readonly ICharTermAttribute termAtt;
        private readonly IKeywordAttribute keywordAttr;
        private readonly IPositionIncrementAttribute position;
        private State? state = null;

        public MorphologyFilter(TokenStream tokenStream, LuceneMorphology luceneMorph) : base(tokenStream)
        {
            this.luceneMorph = luceneMorph;
            termAtt = AddAttribute<ICharTermAttribute>();
            keywordAttr = AddAttribute<IKeywordAttribute>();
            position = AddAttribute<IPositionIncrementAttribute>();
        }


        public sealed override bool IncrementToken()
        {
            if (iterator != null)
            {
                if (iterator.MoveNext())
                {
                    RestoreState(state);
                    position.PositionIncrement = 0;
                    termAtt.SetEmpty().Append(iterator.Current);
                    return true;
                }
                else
                {
                    state = null;
                    iterator = null;
                }
            }
            while (true)
            {
                bool b = m_input.IncrementToken();
                if (!b)
                {
                    return false;
                }
                if (!keywordAttr.IsKeyword && termAtt.Length > 0)
                {
                    string s = new string(termAtt.Buffer, 0, termAtt.Length);
                    if (luceneMorph.checkString(s))
                    {
                        IList<string> forms = luceneMorph.getNormalForms(s);
                        if (forms.Count == 0)
                        {
                            continue;
                        }
                        else if (forms.Count == 1)
                        {
                            termAtt.SetEmpty().Append(forms[0]);
                        }
                        else
                        {
                            state = CaptureState();
                            iterator = forms.GetEnumerator();
                            iterator.MoveNext();
                            termAtt.SetEmpty().Append(iterator.Current);
                        }
                    }
                }
                return true;
            }
        }

        public override void Reset()
        {
            base.Reset();
            state = null;
            iterator = null;
        }
    }

}