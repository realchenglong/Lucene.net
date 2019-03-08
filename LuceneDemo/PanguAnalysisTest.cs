using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Lucene.Net.Analysis;

namespace LuceneDemo
{
    public partial class PanguAnalysisTest : Form
    {
        public PanguAnalysisTest()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string strWord = textBox1.Text;
            string strAnalyzerName = comboBox1.SelectedText ;
            Lucene.Net.Analysis.Analyzer analyzer = GetAnalyzerByName(strAnalyzerName); 
            cutWord(strWord, analyzer);
        }

        public List<String> cutWord(string word , Lucene.Net.Analysis.Analyzer analysis)
        {
            List<string> result = new List<string>();
            //TokenStream tokenStream = analysis.ReusableTokenStream("", new StringReader(word));
            TokenStream tokenStream = analysis.ReusableTokenStream("field1", new StringReader(word));
            AttrbuteClass attrbutes = tokenStream.GetAttribute<AttrbuteClass>();
            //Token token = tokenStream.;
            //PanGu.Segment segment = new PanGu.Segment();

            return result;
        }

        public Lucene.Net.Analysis.Analyzer GetAnalyzerByName(string analyzerName)
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

        public class AttrbuteClass:Lucene.Net.Util.IAttribute
        {
            string field1 = "";
        }
    }
}
