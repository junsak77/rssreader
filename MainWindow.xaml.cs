using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace RSSReader
{
	public partial class MainWindow : Window
	{
		private class RssItemInfo
        {
			public string Title { get; set; }
            public string Description { get; set; }
			public string Link { get; set; }
			public string PubDate { get; set; }
		}

		private class RssInfo
        {
			public string Title { get; set; }
			public string Description { get; set; }
			public string Link { get; set; }
			public string PubDate { get; set; }
        }

		private class CustomButton : Button
        {
			public RssItemInfo rii;
			public CustomButton()
            {
				rii = new RssItemInfo();
            }
        }

		public MainWindow()
        {
			InitializeComponent();

            try
            {
				// 取得するRSSのURL
				string rss = "https://news.yahoo.co.jp/rss/topics/it.xml";

                //XElementのLoadメソッドにURLを渡して情報を取得する
                //XElementのリファレンス:https://docs.microsoft.com/ja-jp/dotnet/api/system.xml.linq.xelement?view=net-5.0
                XElement element = XElement.Load(rss);

                //"channel"エレメントを取得
                XElement channelElement = element.Element("channel");

                //RssInfoクラスのインスタンスを生成
                RssInfo rssInfo = new RssInfo();
                //ここではヘッダ情報を取得している
                //それぞれのメンバ変数へ取得したchannelエレメントの中から指定したエレメントの中身を取得
                rssInfo.Title = channelElement.Element("title").Value;
                rssInfo.Description = channelElement.Element("description").Value;
                rssInfo.Link = channelElement.Element("link").Value;
                rssInfo.PubDate = channelElement.Element("pubDate").Value;

                //Itemエレメントを全て取得する
                //<Item>
                //  <item1>
                //</Item>
                //<Item>
                //  <item2>
                //</Item>
                //<item1><item2>が取得される
                IEnumerable<XElement> elementItems = channelElement.Elements("item");

                //ListをRssItemInfoクラス型で作成する
                List<RssItemInfo> rssItemInfos = new List<RssItemInfo>();
                //elementItemsの中身を一つずつ取得
                foreach (XElement elmItem in elementItems)
                {
                    RssItemInfo itemInfo = new RssItemInfo();
                    //それぞれのElementに含まれる値を取得してくる
                    itemInfo.Title = elmItem.Element("title").Value;
                    itemInfo.Description = elmItem.Element("description").Value;
                    itemInfo.Link = elmItem.Element("link").Value;
                    itemInfo.PubDate = elmItem.Element("pubDate").Value;
                    rssItemInfos.Add(itemInfo);
                }

                //タイトルを取得して文字列型に格納
                string rsstext = "";
                foreach (var work in rssItemInfos)
                {
                    //タイトルを文字列結合してTextBlockへ反映
                    rsstext += work.Title + "\n";
                    //ボタンの拡張クラスを作って押すと別ウィンドウに情報を出すようにする
                    CustomButton bt = new CustomButton();
                    bt.Content = work.Title;
                    bt.rii = work;
                    bt.Height = 100;
                    bt.FontSize = 36;
                    //クリックイベントのイベントハンドラを設定
                    bt.Click += new RoutedEventHandler(CustomButton_Click);
                    stack.Children.Add(bt);
                }
                //textblock.Text = rsstext;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
            }
        }

        /// <summary>
        /// カスタムボタンをクリックした時の動作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CustomButton_Click(object sender, RoutedEventArgs e)
        {
             CustomButton cb = (CustomButton)sender;
            CustomWindow cw = new CustomWindow();
            cw.customWindow_Text.Text = DateTime.Parse(cb.rii.PubDate).ToString("yyyy-MM-dd") + "\n";
            cw.customWindow_Text.Text += cb.rii.Description + "\n";
            cw.customWindow_Text.Text += cb.rii.Link;
            cw.Show();
        }

        private void TEST_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
