using LuceneNetRussianMorphology.English;
using LuceneNetRussianMorphology.Helpers;
using LuceneNetRussianMorphology.Morph;
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
    /*using RussianLuceneMorphology = org.apache.lucene.morphology.russian.RussianLuceneMorphology;
    using EnglishLuceneMorphology = org.apache.lucene.morphology.english.EnglishLuceneMorphology;
    using MatcherAssert = org.hamcrest.MatcherAssert;
    using Test = org.junit.Test;*/


    //	import static org.hamcrest.CoreMatchers.equalTo;


    public class LuceneMorphTest
    {
        [Test]
        public virtual void englishMorphologyShouldGetCorrectNormalForm()
        {
            LuceneMorphology luceneMorph = new EnglishLuceneMorphology();
            string pathToTestData = "english-morphology-test.txt";
            testMorphology(luceneMorph, pathToTestData);
        }

        [Test]
        public virtual void russianMorphologyShouldGetCorrectNormalForm()
        {
            LuceneMorphology luceneMorph = new RussianLuceneMorphology();
            IList<string> v = luceneMorph.getMorphInfo("вина");
            Console.WriteLine(v);
            string pathToTestData = "russian-morphology-test.txt";
            testMorphology(luceneMorph, pathToTestData);
        }

        private void testMorphology(LuceneMorphology luceneMorph, string pathToTestData)
        {
            Stream stream = ResourceHelpers.GetResource<LuceneMorphTest>(pathToTestData);
            StreamReader bufferedReader = new StreamReader(stream, Encoding.UTF8);
            string s = bufferedReader.ReadLine();
            while (!ReferenceEquals(s, null))
            {
                string[] qa = s.Trim().Split(" ");
                ISet<string> result = new HashSet<string>(qa.Skip(1));
                ISet<string> stringList = new HashSet<string>(luceneMorph.getNormalForms(qa[0]));
                Assert.That(stringList.SetEquals(result));
                s = bufferedReader.ReadLine();
            }
        }
    }

}