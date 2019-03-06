using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Lucene.Net;
using Lucene.Net.Analysis; 
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.Search;
using Lucene.Net.Store;
using Lucene.Net.Util;

namespace LuceneDemo
{
    public partial class Form1 : Form
    {
        string indexLocation = @"C:\Index";

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        { 

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string strTitle = textBox1.Text;
            string strContent = richTextBox1.Text;

            Lucene.Net.Util.Version AppLuceneVersion = Lucene.Net.Util.Version.LUCENE_30;
            Lucene.Net.Analysis.Analyzer analyzer = new Lucene.Net.Analysis.Standard.StandardAnalyzer(AppLuceneVersion);
            //Lucene.Net.Analysis.Analyzer analyzer = new PanGuAnalyzer();
            Lucene.Net.Analysis.SimpleAnalyzer simpleAnalyzer = new SimpleAnalyzer();
            FSDirectory dir = FSDirectory.Open(indexLocation);
            //create an index writer 
            IndexWriter iw = new IndexWriter(dir, analyzer, IndexWriter.MaxFieldLength.UNLIMITED);
            Lucene.Net.Documents.Document document = new Lucene.Net.Documents.Document();
            document.Add(new Lucene.Net.Documents.Field("title", strTitle, Lucene.Net.Documents.Field.Store.YES, Lucene.Net.Documents.Field.Index.ANALYZED));
            document.Add(new Lucene.Net.Documents.Field("content", strContent, Lucene.Net.Documents.Field.Store.YES, Lucene.Net.Documents.Field.Index.ANALYZED));
            iw.AddDocument(document);
            iw.Optimize();
            iw.Dispose();
        }
        public List<Item> search(string keyWord)
        {
            FSDirectory dir = FSDirectory.Open(indexLocation);
            List<Item> results = new List<Item>();
            Lucene.Net.Util.Version AppLuceneVersion = Lucene.Net.Util.Version.LUCENE_30;
            Lucene.Net.Analysis.Analyzer analyzer = new Lucene.Net.Analysis.Standard.StandardAnalyzer(AppLuceneVersion);
            Lucene.Net.Search.IndexSearcher searcher = new Lucene.Net.Search.IndexSearcher(dir);
            Lucene.Net.QueryParsers.MultiFieldQueryParser parser = new Lucene.Net.QueryParsers.MultiFieldQueryParser(AppLuceneVersion,new string[] { "title", "content" }, analyzer);
            Lucene.Net.Search.Query query = parser.Parse(keyWord);
            TopDocs topdocs = searcher.Search(query, 100);
            Lucene.Net.Search.ScoreDoc[] hits = topdocs.ScoreDocs;

            foreach (var hit in hits)
            {
                var foundDoc = searcher.Doc(hit.Doc);
                results.Add(new Item { title = foundDoc.Get("title"), content = foundDoc.Get("content") }); 
            } 
            searcher.Dispose();
            return results;
        }

        public class Item 
        {
            public string title { get; set; }
            public string content { get; set; }
             
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string keyWord = textBox2.Text;
            List<Item> items = search(keyWord);
            dataGridView1.DataSource = items;
        }
    }
}
