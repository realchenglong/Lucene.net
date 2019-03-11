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
using Lucene.Net.Analysis.Tokenattributes;
using LuceneDemo.Interface;
using LuceneDemo.Util;

namespace LuceneDemo
{
    using IAttribute = Lucene.Net.Util.IAttribute; 
    public partial class PanguAnalysisTest : Form
    {
        public PanguAnalysisTest()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string strWord = textBox1.Text;
            string strAnalyzerName = comboBox1.SelectedItem.ToString() ;
            Lucene.Net.Analysis.Analyzer analyzer = AnalyzerHelper.GetAnalyzerByName(strAnalyzerName);
            List<String> listString = cutWord(strWord, analyzer);
            listBox1.DataSource = listString;
        }

        public List<String> cutWord(string word , Lucene.Net.Analysis.Analyzer analysis)
        {
            List<string> result = new List<string>();
            //TokenStream tokenStream = analysis.ReusableTokenStream("", new StringReader(word));
            TokenStream tokenStream = analysis.TokenStream("field1", new StringReader(word));
            //IndexWriterConfig iwc = new IndexWriterConfig(LuceneVersion.LUCENE_48, analyzer);
            bool boolHas = tokenStream.HasAttributes;
            ITermAttribute attrbutes;// = tokenStream.GetAttribute<ITermAttribute>();
            //IEnumerable<Lucene.Net.Util.Attribute> aaa = tokenStream.GetAttributeImplsIterator();
            //IEnumerable<Type> bbb = tokenStream.GetAttributeTypesIterator();
            if (boolHas)
            { 
                while (tokenStream.IncrementToken())
                {
                    attrbutes = tokenStream.GetAttribute<ITermAttribute>();
                    result.Add(attrbutes.Term.ToString());
                }
            }
             
            tokenStream.Reset();
            //attrbutes.
            //Token token = tokenStream.;
            //PanGu.Segment segment = new PanGu.Segment();

            tokenStream.End();
            return result;
        }


        public class Field
        {
            string field = "";
        }

         
    }
}
