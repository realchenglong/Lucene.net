using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LuceneDemo.Util
{
    public static class AnalyzerHelper
    {

        public static Lucene.Net.Analysis.Analyzer GetAnalyzerByName(string analyzerName)
        {
            Lucene.Net.Analysis.Analyzer result;
            Lucene.Net.Util.Version AppLuceneVersion = Lucene.Net.Util.Version.LUCENE_30;
            switch (analyzerName)
            {
                case "SimpleAnalyzer":
                    result = new Lucene.Net.Analysis.SimpleAnalyzer();
                    break;
                case "StandardAnalyzer":
                    result = new Lucene.Net.Analysis.Standard.StandardAnalyzer(AppLuceneVersion);
                    break;
                case "KeywordAnalyzer":
                    result = new Lucene.Net.Analysis.KeywordAnalyzer();
                    break;
                case "StopAnalyzer":
                    result = new Lucene.Net.Analysis.StopAnalyzer(AppLuceneVersion);
                    break;
                case "WhitespaceAnalyzer":
                    result = new Lucene.Net.Analysis.WhitespaceAnalyzer();
                    break;
                default:
                    result = new Lucene.Net.Analysis.SimpleAnalyzer();
                    break;
            }
            return result;

        }
    }
}
