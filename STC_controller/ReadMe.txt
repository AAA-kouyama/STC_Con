
このソフトウェアをコピーしてつかったり、配布したり、変更を加えたり、
変更を加えたものを配布したり、商用利用したり、有料で販売したり、
すべてGfit株式会社に確認してにつかってください(多分そうだと思う)

このソフトウェアにはなんの保証もついていません。
たとえ、このソフトウェアを利用したことでなにか問題が起こったとしても、
作者はなんの責任も負いません。(Gfit株式会社から修正を依頼されたら対応します)

本アプリケーションはVisualStudio2015を用いて.NetFrameWork4.5 C#で開発しています。
アプリケーションの著作権は多分、Gfit株式会社に帰属します。

Copyright (c) 2016 Gfit CO., INC.
Released under the Gfit CO., INC. license
Writer AAA-system CO., INC. Tsuyoshi Kouyama

****ソースコード構成説明****
１．参照設定
log4net…エラー等ロギング用(nugetにて導入)
Microsoft.CSharp
System
System.configuration
System.Core
System.Data
System.Data.DataSetExtensions
System.Deployment
System.Drawing
System.Management
System.Net.Http
System.Runtime.Serialization
System.Web.Extensions
System.Windows.Forms
System.Xml
System.Xml.Linq

２．ソースクラス
ClientTcpIp.cs…ソケット接続維持用スレッドで使用

DynamicJson.cs…Jsonコンバーター(nugetにて導入)

favicon.ico…シストレクラウドからアイコンを拝借

File_CTRL.cs…ローカルファイルのREAD/WRITE用クラス

Form1.cs…main()を格納した入口。基本的にはタイマーボタンで各種処理を自動化

http_Request_CTRL.cs…http(s)のGET/POSTリクエスト発行クラス

Json_acceptor_abstract.cs…jsonでのリクエスト取得時の基本動作を抽象化実装したクラス

Json_acceptor_base.cs…抽象化実装したJson_acceptor_abstractに基本動作内容を実装したクラス

Json_Add_User.cs…Json_acceptor_baseにadd_user用の処理を実装したクラス

Json_add_user_action.cs…add_userを受信した際に各種呼出しを行うビジネスロジックを実装したクラス

Json_EA_Param.cs…Json_acceptor_baseにEAパラメータ更新用の処理を実装したクラス

Json_EA_Para_actionm.cs…EA更新を受信した際に各種呼出しを行うビジネスロジックを実装したクラス

Json_End_Init.cs…end_ini用の応答json生成処理をするクラス

Json_Request.cs…Json_acceptor_baseにrequest用の処理を実装したクラス

Json_request_action.cs…requestを受信した際に各種呼出しを行うビジネスロジックを実装したクラス

Json_Resopnse.cs…response用の応答json生成処理をするクラス

Json_Util.cs…json処理用のユーティリティ関数群クラス

MT4_CTLR.cs…ope_codeに基づきMT4を操作する為の処理を実装したクラス

Program.cs…こちらでログ出力のコントロール用のパラメータをApp.configから読み込む処理を記述しています
　　　　　　また、リトライ処理用のArrayList queue_listもここで宣言をしています

program_CTRL.cs…MT4を操作する為の基本処理群(ユーティリティかな)を実装したクラス

retry_timer.cs…MT4起動処理で時間が掛かる場合に監視とSTCへの応答を行うタイマーで実装されたクラス

３．メモ
DynamicJsonを導入する。
http://dynamicjson.codeplex.com/
Nugetで以下のコマンドを実行すると取り込めます。
Install-Package DynamicJson
