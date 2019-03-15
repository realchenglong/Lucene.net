using Lucene.Net.Index;
using Lucene.Net.Search;
using Lucene.Net.Store;
using LuceneDemo.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

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
            string strAnalyzerType = "StandardAnalyzer";
            if (comboBox1.SelectedItem != null)
            {
                strAnalyzerType = comboBox1.SelectedItem.ToString();
            }

            //Lucene.Net.Util.Version AppLuceneVersion = Lucene.Net.Util.Version.LUCENE_30;
            //Lucene.Net.Analysis.Analyzer analyzer = new Lucene.Net.Analysis.Standard.StandardAnalyzer(AppLuceneVersion);
            //Lucene.Net.Analysis.Analyzer analyzer = new PanGuAnalyzer();
            //Lucene.Net.Analysis.SimpleAnalyzer simpleAnalyzer = new SimpleAnalyzer();

            Lucene.Net.Analysis.Analyzer analyzer = AnalyzerHelper.GetAnalyzerByName(strAnalyzerType);
            FSDirectory dir = FSDirectory.Open(indexLocation);
            IndexWriter iw = new IndexWriter(dir, analyzer, IndexWriter.MaxFieldLength.UNLIMITED);
            {
                int docCount = iw.GetDocCount(1);
                docCount = iw.MaxDoc();
                long ramSizeInBytes = iw.RamSizeInBytes();
                //iw.DeleteDocuments();
                //iw.DeleteDocuments()
            }
            Lucene.Net.Documents.Document document = new Lucene.Net.Documents.Document();
            document.Add(new Lucene.Net.Documents.Field("title", strTitle, Lucene.Net.Documents.Field.Store.YES, Lucene.Net.Documents.Field.Index.NOT_ANALYZED));
            document.Add(new Lucene.Net.Documents.Field("content", strContent, Lucene.Net.Documents.Field.Store.YES, Lucene.Net.Documents.Field.Index.ANALYZED));
            iw.AddDocument(document);
            iw.Optimize();
            iw.Dispose();
            //iw.
        }
        public List<Item> search(string keyWord)
        {

            //string strAnalyzerType = comboBox2.SelectedItem.ToString();
            string strAnalyzerType = "StandardAnalyzer";
            if (comboBox2.SelectedItem != null)
            {
                strAnalyzerType = comboBox2.SelectedItem.ToString();
            }
            string strType = "A";
            if (textBox3.Text != "")
            {
                strType = textBox3.Text.ToString();
            }
            FSDirectory dir = FSDirectory.Open(indexLocation);
            List<Item> results = new List<Item>();

            Lucene.Net.Util.Version AppLuceneVersion = Lucene.Net.Util.Version.LUCENE_30;
            Lucene.Net.Analysis.Analyzer analyzer = AnalyzerHelper.GetAnalyzerByName(strAnalyzerType);
            //Lucene.Net.QueryParsers.MultiFieldQueryParser parser = new Lucene.Net.QueryParsers.MultiFieldQueryParser(AppLuceneVersion,new string[] { "content","title" }, analyzer);
            IDictionary<String, Single> dictionary = new Dictionary<String, Single>();
            dictionary.Add("title", 5);
            dictionary.Add("content", 10);
            Lucene.Net.QueryParsers.MultiFieldQueryParser parser = new Lucene.Net.QueryParsers.MultiFieldQueryParser(AppLuceneVersion, new string[] { "title", "content" }, analyzer);
            //parser.DefaultOperator = Lucene.Net.QueryParsers.QueryParser.Operator.AND;
            Lucene.Net.Search.Query query = parser.Parse(keyWord);
            //parser.    
            //BooleanClause
            //Lucene.Net.Search.BooleanClause booleanClauses = new BooleanClause();
            //BooleanClause
            //    BooleanClause.Occur[] flags = new BooleanClause.Occur[] { BooleanClause.Occur.MUST, BooleanClause.Occur.MUST };
            Occur[] occurs = new Occur[] { Occur.MUST, Occur.MUST };
            Lucene.Net.Search.Query query2 = Lucene.Net.QueryParsers.MultiFieldQueryParser.Parse(AppLuceneVersion, new string[] { keyWord, strType }, new string[] { "content", "title" }, occurs, analyzer);

            Lucene.Net.Search.IndexSearcher searcher = new Lucene.Net.Search.IndexSearcher(dir);
            TopDocs topdocs = searcher.Search(query2, 100);
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

        private void button4_Click(object sender, EventArgs e)
        {
            Form formFenciTest = new PanguAnalysisTest();
            formFenciTest.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Lucene.Net.Analysis.Analyzer analyzer = AnalyzerHelper.GetAnalyzerByName("StandardAnalyzer");
            FSDirectory dir = FSDirectory.Open(indexLocation);
            IndexWriter iw = new IndexWriter(dir, analyzer, IndexWriter.MaxFieldLength.UNLIMITED);
            iw.DeleteAll();
            iw.Commit();
        }

        public List<Item> Search2(string keyWord)
        {

            //string strAnalyzerType = comboBox2.SelectedItem.ToString();
            string strAnalyzerType = "StandardAnalyzer";
            if (comboBox2.SelectedItem != null)
            {
                strAnalyzerType = comboBox2.SelectedItem.ToString();
            }
            string strType = "A";
            if (textBox3.Text != "")
            {
                strType = textBox3.Text.ToString();
            }
            FSDirectory dir = FSDirectory.Open(indexLocation);
            List<Item> results = new List<Item>();

            Lucene.Net.Util.Version AppLuceneVersion = Lucene.Net.Util.Version.LUCENE_30;
            Lucene.Net.Analysis.Analyzer analyzer = AnalyzerHelper.GetAnalyzerByName(strAnalyzerType);

            Lucene.Net.QueryParsers.MultiFieldQueryParser parser = new Lucene.Net.QueryParsers.MultiFieldQueryParser(AppLuceneVersion, new string[] { "title", "content" }, analyzer);
            ///////////////////////////////////////////////// 
            BooleanQuery query = new BooleanQuery();
            Query queryType = new TermQuery(new Term("title", strType));
            Query queryKey = new TermQuery(new Term("content", keyWord));
            query.Add(queryType, Occur.MUST);
            query.Add(queryKey, Occur.MUST);
            Lucene.Net.Search.IndexSearcher searcher = new Lucene.Net.Search.IndexSearcher(dir);
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

        private void button5_Click(object sender, EventArgs e)
        {
            //准备数据
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("ID", typeof(int)));
            dt.Columns.Add(new DataColumn("UserName", typeof(string)));
            dt.Columns.Add(new DataColumn("DeptNo", typeof(string)));
            dt.Columns.Add(new DataColumn("DeptName", typeof(string)));
            DataRow drTemp = null;
            for (int i = 10; i <= 18; i++)
            {
                drTemp = dt.NewRow();
                drTemp["ID"] = i;
                drTemp["UserName"] = "姓名" + i.ToString();

                if (i < 15)
                {
                    drTemp["DeptNo"] = "0001";
                    drTemp["DeptName"] = "人事部";
                }
                else
                {
                    drTemp["DeptNo"] = "0002";
                    drTemp["DeptName"] = "生产部";
                }
                dt.Rows.Add(drTemp);
            }
            for (int i = 10; i <= 18; i++)
            {
                drTemp = dt.NewRow();
                drTemp["ID"] = i;
                drTemp["UserName"] = "姓名" + i.ToString();

                if (i < 15)
                {
                    drTemp["DeptNo"] = "0001";
                    drTemp["DeptName"] = "人事部";
                }
                else
                {
                    drTemp["DeptNo"] = "0002";
                    drTemp["DeptName"] = "生产部";
                }
                dt.Rows.Add(drTemp);
            }

            Console.WriteLine("分组前：");
            foreach (DataRow row in dt.Rows)
            {
                Console.WriteLine(string.Format("{0} {1} {2} {3} ", row.ItemArray));
            }
            Console.WriteLine("");

            Console.WriteLine("分组后：");

            DataTable dt1 = CutTableToPages(dt, "ID", 2, 2);



            //Linq分组查询，并按分组显示人员明细
            var query = from g in dt.AsEnumerable()
                        group g by new { t1 = g.Field<string>("DeptNo"), t2 = g.Field<string>("DeptName") } into companys
                        select new { DeptNo = companys.Key.t1, DeptName = companys.Key.t2, StallInfo = companys };


            var query1 = from r in dt.AsEnumerable()
                         where !(
                         from rr in dt.AsEnumerable()
                         select rr.Field<string>("WebMarkId")
                         ).Contains(r.Field<int>("GBId").ToString())
                         select r;


            foreach (var userInfo in query)
            {
                System.Collections.Generic.List<DataRow> dataRows = userInfo.StallInfo.ToList();

                Console.WriteLine(string.Format("{0}({1})人员名单: ", userInfo.DeptName, userInfo.DeptNo));
                foreach (System.Data.DataRow dr in dataRows)
                {
                    Console.WriteLine(string.Format("{0} {1} ", dr.ItemArray));
                }
            }
            Console.ReadLine();

        }
        /// <summary>
        /// 分页方法
        /// </summary>
        /// <param name="dtSource">源</param>
        /// <param name="intPageSize">每页几条数据</param>
        /// <param name="intPageNo">第几页</param>
        /// <returns></returns>
        public DataTable CutTableToPages(DataTable dtSource, string columnName, int intPageSize, int intPageNo)
        {
            DataTable dtResult = new DataTable();
            List<string> listKey = new List<string>();
            //for (int i = 0; i < dtSource.Rows.Count; i++)
            //{
            //    if (listKey.Count == intPageSize) ;
            //}
            dtSource.DefaultView.Sort = " id desc ";
            dtSource = dtSource.DefaultView.ToTable();
            var query = (from t in dtSource.AsEnumerable()
                         group t by new { t1 = t.Field<int>(columnName) } into m
                         select new
                         {
                             column1 = m.Key.t1
                         }).Skip(intPageSize * (intPageNo - 1)).Take(intPageSize);
 
            var list = query.ToList();
            string strSort = "";
            for (int i = 0; i < list.Count; i++)
            {
                if (i == 0)
                {
                    strSort += " id=" + list[i].column1.ToString();
                }
                else
                {
                    strSort += " or id=" + list[i].column1.ToString();
                }
            }
            dtSource.DefaultView.RowFilter = strSort;

            dtSource = dtSource.DefaultView.ToTable();

            int intResult = list.Count();

            return dtResult;
        }
    }
}
