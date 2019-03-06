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
            Lucene.Net.Analysis.Analyzer analyzer = new Lucene.Net.Analysis.an();
            cutWord(strWord, analyzer);
        }

        public List<String> cutWord(string word , Lucene.Net.Analysis.Analyzer analysis)
        {
            List<string> result = new List<string>();
            TokenStream tokenStream = analysis.ReusableTokenStream("", new StringReader(word));
            //Token token = tokenStream.;

            return result;
        }
    }
}
