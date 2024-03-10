using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using Microsoft.Web.WebView2.Core;

namespace SMZX;
public partial class MainWindow
{
	public MainWindow()
	{
		InitializeComponent();

		WebView.CoreWebView2InitializationCompleted += WebView_CoreWebView2InitializationCompleted;
		WebView.Source = new Uri("https://ymzx.qq.com/cp/a20240223");
	}


	#region Methods
	private void WebView_CoreWebView2InitializationCompleted(object? sender, CoreWebView2InitializationCompletedEventArgs e)
	{
		if (!e.IsSuccess) return;

		WebView.CoreWebView2.Settings.UserAgent = "Mozilla/5.0 (iPhone; CPU iPhone OS 17_4 like Mac OS X) AppleWebKit/537.51.2 (KHTML, like Gecko) Mobile/11D257 > QQ/9.0.21.631 NetType/WIFI Mem/28";
	}

	protected override void OnClosing(CancelEventArgs e)
	{
		Application.Current.Shutdown();
	}


	private async void Login_Click(object sender, RoutedEventArgs e)
	{
		await WebView.CoreWebView2.ExecuteScriptAsync($$"""
			need("biz.login",function(LoginManager){
			  LoginManager.init({
			    needReloadPage:true
			  });
			  LoginManager.login();
			});
			""");
	}

	private void StopGift_Click(object sender, RoutedEventArgs e)
	{
		IsCancel = true;
	}


	private void Commit(sbyte type = 0)
	{
		IsCancel = false;

		Task.Run(async () =>
		{
			while (true)
			{
				if (IsCancel) throw new OperationCanceledException();

				await Dispatcher.Invoke(() => WebView.CoreWebView2.ExecuteScriptAsync($$"""
				flow_1018340.sData.type = {{type}};
				flow_1018340.sData.loginType = 1;
				Milo.emit(flow_1018340);
				"""));

				await Task.Delay(50);
			}
		});
	}

	private void Gift1_Click(object sender, RoutedEventArgs e) => Commit(0);
	private void Gift2_Click(object sender, RoutedEventArgs e) => Commit(1);
	#endregion


	#region Private Fields
	private bool IsCancel = false;
	#endregion
}