using J2N.Text;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Core;
using Lucene.Net.Analysis.Miscellaneous;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Analysis.TokenAttributes;
using Lucene.Net.Analysis.Util;
using LuceneNetRussianMorphology.English;
using LuceneNetRussianMorphology.Helpers;
using LuceneNetRussianMorphology.Morph;
using LuceneNetRussianMorphology.Morph.Analyzer;
using LuceneNetRussianMorphology.Russian;
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
namespace LuceneNetRussianMorphologyTest
{
    /*
    using Analyzer = org.apache.lucene.analysis.Analyzer;
    using BaseTokenStreamTestCase = org.apache.lucene.analysis.BaseTokenStreamTestCase;
    using CharArraySet = org.apache.lucene.analysis.CharArraySet;
    using LowerCaseFilter = org.apache.lucene.analysis.LowerCaseFilter;
    using TokenFilter = org.apache.lucene.analysis.TokenFilter;
    using SetKeywordMarkerFilter = org.apache.lucene.analysis.miscellaneous.SetKeywordMarkerFilter;
    using StandardTokenizer = org.apache.lucene.analysis.standard.StandardTokenizer;
    using CharTermAttribute = org.apache.lucene.analysis.tokenattributes.CharTermAttribute;
    using PositionIncrementAttribute = org.apache.lucene.analysis.tokenattributes.PositionIncrementAttribute;
    using MorphologyAnalyzer = org.apache.lucene.morphology.analyzer.MorphologyAnalyzer;
    using MorphologyFilter = org.apache.lucene.morphology.analyzer.MorphologyFilter;
    using EnglishAnalyzer = org.apache.lucene.morphology.english.EnglishAnalyzer;
    using TokenStream = org.apache.lucene.analysis.TokenStream;
    using EnglishLuceneMorphology = org.apache.lucene.morphology.english.EnglishLuceneMorphology;
    using RussianAnalyzer = org.apache.lucene.morphology.russian.RussianAnalyzer;
    using RussianLuceneMorphology = org.apache.lucene.morphology.russian.RussianLuceneMorphology;
    using MatcherAssert = org.hamcrest.MatcherAssert;
    using Test = org.junit.Test;*/


    //	import static org.hamcrest.Matchers.equalTo;


    public class AnalyzersTest : BaseTokenStreamTestCase
    {
        [Test]
        public virtual void shouldGiveCorrectWordsForEnglish()
        {
            Analyzer morphlogyAnalyzer = new EnglishAnalyzer();
            string answerPath = "english-analyzer-answer.txt";
            string testPath = "english-analyzer-data.txt";

            testAnalayzer(morphlogyAnalyzer, answerPath, testPath);
        }

        [Test]
        public virtual void shouldGiveCorrectWordsForRussian()
        {
            Analyzer morphlogyAnalyzer = new RussianAnalyzer();
            string answerPath = "russian-analyzer-answer.txt";
            string testPath = "russian-analyzer-data.txt";

            testAnalayzer(morphlogyAnalyzer, answerPath, testPath);
        }

        [Test]
        public virtual void emptyStringTest()
        {
            LuceneMorphology russianLuceneMorphology = new RussianLuceneMorphology();
            LuceneMorphology englishLuceneMorphology = new EnglishLuceneMorphology();

            MorphologyAnalyzer russianAnalyzer = new MorphologyAnalyzer(russianLuceneMorphology);
            StreamReader reader = new StreamReader(new MemoryStream("тест пм тест".GetBytes(Encoding.UTF8)), Encoding.UTF8);
            TokenStream stream = russianAnalyzer.GetTokenStream(null, reader);
            MorphologyFilter englishFilter = new MorphologyFilter(stream, englishLuceneMorphology);

            englishFilter.Reset();
            while (englishFilter.IncrementToken())
            {
                Console.WriteLine(englishFilter);
            }
        }

        [Test]
        public virtual void shouldProvideCorrectIndentForWordWithMelitaForm()
        {
            Analyzer morphlogyAnalyzer = new RussianAnalyzer();
            StreamReader reader = new StreamReader(new MemoryStream("принеси мне вина на новый год".GetBytes(Encoding.UTF8)), Encoding.UTF8);

            TokenStream tokenStream = morphlogyAnalyzer.GetTokenStream(null, reader);
            tokenStream.Reset();
            ISet<string> foromsOfWine = new HashSet<string>();
            foromsOfWine.Add("вина");
            foromsOfWine.Add("винo");
            bool wordSeen = false;
            while (tokenStream.IncrementToken())
            {
                ICharTermAttribute charTerm = tokenStream.GetAttribute<ICharTermAttribute>();
                IPositionIncrementAttribute position = tokenStream.GetAttribute<IPositionIncrementAttribute>();
                if (foromsOfWine.Contains(charTerm.ToString()) && wordSeen)
                {
                    Assert.That(position.PositionIncrement == 0);
                }
                if (foromsOfWine.Contains(charTerm.ToString()))
                {
                    wordSeen = true;
                }
            }
        }

        private void testAnalayzer(Analyzer morphlogyAnalyzer, string answerPath, string testPath)
        {
            Stream stream = ResourceHelpers.GetResource<AnalyzersTest>(answerPath);
            StreamReader breader = new StreamReader(stream, Encoding.UTF8);
            string[] strings = breader.ReadLine().Replace(" +", " ").Trim().Split(" ");
            HashSet<string> answer = new HashSet<string>(strings);
            stream.Close();

            stream = ResourceHelpers.GetResource<AnalyzersTest>(testPath);

            StreamReader reader = new StreamReader(stream, Encoding.UTF8);

            TokenStream tokenStream = morphlogyAnalyzer.GetTokenStream(null, reader);
            tokenStream.Reset();
            HashSet<string> result = new HashSet<string>();
            while (tokenStream.IncrementToken())
            {
                ICharTermAttribute attribute1 = tokenStream.GetAttribute<ICharTermAttribute>();
                result.Add(attribute1.ToString());
            }

            stream.Close();

            Assert.That(result.SetEquals(answer));
        }

        /*[Test]
        public virtual void testPositionIncrement()
        {
            EnglishAnalyzer englishAnalyzer = new EnglishAnalyzer();
            AssertTokenStreamContents(englishAnalyzer.GetTokenStream("test", "There are tests!"), new string[] { "there", "are", "be", "test" }, new int[] { 0, 6, 6, 10 }, new int[] { 5, 9, 9, 15 }, new string[] { "<ALPHANUM>", "<ALPHANUM>", "<ALPHANUM>", "<ALPHANUM>" }, new int[] { 1, 1, 0, 1 });
        }

        [Test]
        public virtual void testKeywordHandling()
        {
            Analyzer analyzer = new EnglishKeywordTestAnalyzer();
            AssertTokenStreamContents(analyzer.GetTokenStream("test", "Tests shouldn't be stemmed, but tests should!"), new string[] { "tests", "shouldn't", "be", "stem", "but", "test", "shall" });
        }

        private class EnglishKeywordTestAnalyzer : Analyzer
        {

            protected override TokenStreamComponents CreateComponents(string fieldName, TextReader reader)
            {
                StandardTokenizer src = new StandardTokenizer(Lucene.Net.Util.LuceneVersion.LUCENE_48, reader);
                CharArraySet dontStem = new CharArraySet(Lucene.Net.Util.LuceneVersion.LUCENE_48, 1, false);
                dontStem.Add("Tests");
                TokenFilter filter = new SetKeywordMarkerFilter(src, dontStem);
                filter = new LowerCaseFilter(Lucene.Net.Util.LuceneVersion.LUCENE_48, filter);
                try
                {
                    filter = new MorphologyFilter(filter, new EnglishLuceneMorphology());
                }
                catch (IOException ex)
                {
                    throw new Exception("cannot create EnglishLuceneMorphology", ex);
                }
                return new TokenStreamComponents(src, filter);
            }
        }*/
    }

}