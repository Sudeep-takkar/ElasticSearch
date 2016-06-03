using Nest;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

namespace ElasticSearchNest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //var Json_Symbollookuptable = null;
            DataTable dataTable = new DataTable();
            string connString = @"Data Source=(local);Initial Catalog=CentralSM_Dev;Persist Security Info=True;User ID=sa;Password=NIRvana2@@6";
            string query = "SELECT * from  T_SMSymbolLookUpTable_temp";

            SqlConnection conn = new SqlConnection(connString);
            SqlCommand cmd = new SqlCommand(query, conn);
            conn.Open();

            // create data adapter
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            // this will query your database and return the result to your datatable
            da.Fill(dataTable);
            string Json_Symbollookuptable = JsonConvert.SerializeObject(dataTable);
            System.IO.File.WriteAllText(@"D:\path.json", Json_Symbollookuptable);
            ExportToElastic(Json_Symbollookuptable);
            conn.Close();
            da.Dispose();
        }

        public void ExportToElastic(string json)
        {
    //        var node = new Uri("http://localhost:9200");

    //        var settings = new ConnectionSettings(
    //            node
    //        );

    //        var client = new ElasticClient(settings);
    //        DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(Security));

             IEnumerable<Security> security = JsonConvert.DeserializeObject<IEnumerable<Security>>(json);

    //        var index = client.Index(security, i => i
    //                .Index("securities_index")
    //                .Type("security_type")
    //                .Id(2)
    //                .Refresh()
    //                .Ttl("1m")
    //);

            var connectionSettings = new ConnectionSettings(new Uri("http://localhost:9200/"));
            connectionSettings.DefaultIndex("securities");
            var elasticClient = new ElasticClient(connectionSettings);

            elasticClient.CreateIndex("securities-v1");
            elasticClient.Alias(x => x.Add(a => a.Alias("securities").Index("securities-v1")));
            elasticClient.Map<Security>(d =>
                d.Properties(p => p.String(n => n.Name(x => x.TickerSymbol.ToString()).Index(NonStringIndexOption.NotAnalyzed))));

            elasticClient.Index(new Security { TickerSymbol = "Sudeep-ticker" });

        }


    }
}
