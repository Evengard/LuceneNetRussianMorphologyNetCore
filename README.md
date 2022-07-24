# LuceneNetRussianMorphologyNetCore
A quick and dirty Java to C# conversion of https://github.com/AKuznetsov/russianmorphology adapted to Lucene.NET 4.8 and .NET Core

It uses the same "morph.info" dictionary files as the original library, so any updates to theese in the original repo could be easily transplanted here.

The conversion was done by an automated tool and then the resulting code was fixed manually. There may be quirks here and there, also this doesn't know anything about being async, so deal with it.

I converted all the main code and a small subset of tests (the ones I actually needed), it seems to work for the actual usage with Lucene.NET 4.8, but I haven't tested anything beyond that.

The morph.info generator here doesn't work and honestly I don't want to fix it - you can still use the original java one to generate it (with small tweaks, refer to https://github.com/AKuznetsov/russianmorphology/issues/27 ) - but I included here the updated russian morph.info (generated 24.07.2022) - it should also be compatible with the original java version as well.