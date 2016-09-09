using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Xml;

namespace WifiPasswordDumper
{
	static class Program
	{
		/*	Author: nonameHDT
		 *	Email: nonameanbu@gmail.com
		 *  Facebook: https://www.facebook.com/hung.de.tien.175
		 *	Release date: 08/09/2016
		 */
		[STAThread]
		static void Main()
		{

			DirectoryInfo dinfo = new DirectoryInfo(Application.StartupPath + "\\" + Path.GetRandomFileName());
			
			if (!dinfo.Exists)
				dinfo.Create();

			ProcessStartInfo pinfo = new ProcessStartInfo(@"C:\Windows\System32\netsh.exe", "wlan export profile key=clear folder=\"" + dinfo.Name + "\"");
			pinfo.UseShellExecute = false;
			pinfo.RedirectStandardOutput = true;
			pinfo.WindowStyle = ProcessWindowStyle.Hidden;

			Process p = new Process();
			p.StartInfo = pinfo;
			p.Start();
			p.WaitForExit();

			foreach (FileInfo finfo in dinfo.GetFiles())
			{
				StreamWriter w = new StreamWriter(File.Open("profiles.txt", FileMode.Append, FileAccess.Write));
				
				XmlDocument root = new XmlDocument();
				root.Load(finfo.FullName);
				XmlNode ssid = root.GetElementsByTagName("SSID").Item(0);
				XmlNode wifiname = ssid.ChildNodes.Item(1);
				XmlNode keyMaterial = root.GetElementsByTagName("keyMaterial").Item(0);

				w.Write(wifiname.InnerText);
				w.WriteLine(":" + keyMaterial.InnerText);
				w.Close();

				finfo.Delete();
			}

			dinfo.Delete();
		}
	}
}
