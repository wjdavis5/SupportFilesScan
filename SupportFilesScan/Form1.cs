using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.Core;
using System.IO;
using System.Threading;
using System.Diagnostics;

namespace SupportFilesScan
{
    public delegate void addtoList(ListBox l, string s);
    public delegate void updateList(ListBox l, object s);
    public delegate void updateList2(object paramList);
    public partial class Form1 : Form
    {
        
        public Form1()
        {
            InitializeComponent();
            
        }
        private void addtList(ListBox l, string s)
        {
            if (l.InvokeRequired)
            {
                l.BeginInvoke(new MethodInvoker(new MethodInvoker(delegate
                    {
                        l.Items.Add(s);
                    })));
            }
            else
            {
                l.Items.Add(s);
            }
            //l.Items.Add(s);
        }
      
        private void Form1_Load(object sender, EventArgs e)
        {
            //comboBox1.SelectedIndex = 0;
            this.Icon = Properties.Resources.Folder_RAR;
            if (!File.Exists(Properties.Settings1.Default.pathToRar))
            {
                MessageBox.Show("WinRar is not installed to: " + Properties.Settings1.Default.pathToRar + "\r\n" + "Please select its location");
                openFileDialog1.ShowDialog();
                if (openFileDialog1.FileName == "")
                {
                    MessageBox.Show("You did not select a location! Bye Bye!");
                    Application.Exit();
                }
                else
                {
                    Properties.Settings1.Default.pathToRar = openFileDialog1.FileName;
                    Properties.Settings1.Default.Save();
                }
            }

            //Bitmap bmp = Properties.Resources.Folder_RAR;

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            setFolder();
        }

       private void setFolder()
       {
           
           folderBrowserDialog1.RootFolder = Environment.SpecialFolder.MyComputer;
            folderBrowserDialog1.ShowDialog();
            if (folderBrowserDialog1.SelectedPath == "")
            {
                //MessageBox.Show("No folder Selected! Closing...");
                //this.Close();
                //Application.Exit();
            }
            textBox1.Text = folderBrowserDialog1.SelectedPath.ToString();
           
        }

     
      

       private void pictureBox1_Click(object sender, EventArgs e)
       {
           if (textBox1.Text != "")
           {
               try
               {
                   string[] dirList = Directory.GetDirectories(folderBrowserDialog1.SelectedPath);
                   foreach (string dir in dirList)
                   {
                       ThreadPool.QueueUserWorkItem(new WaitCallback(getFiles), (object)dir);
                       //string[] gfiles = Directory.GetFiles(folderBrowserDialog1.SelectedPath, "*" + f, SearchOption.AllDirectories);
                       /*
                       foreach(string d in gfiles)
                       {
                           progressBar1.PerformStep();
                           switch (f)
                           {
                               case ".ininlog":
                                   listBox2.Items.Add(d);
                                   break;
                               case ".reg":
                                   listBox3.Items.Add(d);
                                   break;
                               case ".evt":
                                   listBox4.Items.Add(d);
                                   break;
                           }
                       }
                        * */

                   }
                   linkLabel4.Enabled = true;
               }

               catch (Exception e2)
               {
                   MessageBox.Show(e2.Message);
               }
           }
           else 
           {
               MessageBox.Show("Please select a folder");
               setFolder();

           }

       }

       private void getFiles(object f2)
       {
           //string[] f3 = (string[])f2;
           //string dirs = (string[])f2;
           addtoList atl = new addtoList(addtList);
          
               string[] gfiles1 = Directory.GetFiles((string)f2, "*.ininlog", SearchOption.AllDirectories);
               string[] gfiles2 = Directory.GetFiles((string)f2, "*.reg", SearchOption.AllDirectories);
               string[] gfiles3 = Directory.GetFiles((string)f2, "*.evt", SearchOption.AllDirectories);
               
               foreach (string d in gfiles1)
               {
                  
                    atl(listBox2, d);
               }
               foreach (string d in gfiles2)
               {
                    atl(listBox3, d);
               }      
               foreach (string d in gfiles3)
               {
                    atl(listBox4, d);
               }
                   

           
       }

       private void createZip(object filename)
       {
           try
           {
               /*
               ZipFile z = ZipFile.Create((string)filename + ".zip");
               z.BeginUpdate();
              // z.SetComment("AutoZipped");
               z.Add((string)filename,CompressionMethod.);
               z.CommitUpdate();
               
               z.Close();
               */
               //UpdateListBoxItem(listBox2, filename);
               int procs = Process.GetProcessesByName("rar.exe").Length;
               int procID;
               Process proc = new Process();
               
               if (File.Exists(Properties.Settings1.Default.pathToRar) && File.Exists((string)filename))
               {
                   /*
                   //string launcher = "\"" + Properties.Settings1.Default.pathToRar + "\"" + " a -df -m3 -ep \"" + (string)filename + ".zip\" \"" + (string)filename + "\"";
                   string launcher = "\"" + Properties.Settings1.Default.pathToRar + "\"";
                   launcher += " a -df -m3 -ep ";
                   launcher += "\"" + (string)filename + ".rar\" ";
                   launcher += "\"" + (string)filename + "\"";
                   //MessageBox.Show(launcher);
                   Clipboard.SetText(launcher);
                   Process.Start(launcher);
                    * */
                 
                   proc.StartInfo.FileName = @Properties.Settings1.Default.pathToRar;
                   proc.StartInfo.Arguments = "a -df -m3 -ep \"" + (string)filename + ".zip\" \"" + (string)filename + "\"";
                   proc.StartInfo.CreateNoWindow = true;
                   proc.StartInfo.WindowStyle = ProcessWindowStyle.Minimized;
                  // MessageBox.Show(proc.StartInfo.FileName + " " + proc.StartInfo.Arguments);
                   proc.Start();
                   procID = proc.Id;
                   proc.WaitForExit();
               }
               //MessageBox.Show(procs.ToString());



               
               if(File.Exists((string)filename + ".ininlog_idx"))
               {
                   proc.StartInfo.FileName = @Properties.Settings1.Default.pathToRar;
                   proc.StartInfo.Arguments = "a -df -m3 -ep \"" + (string)filename + ".zip\" \"" + (string)filename + ".ininlog_idx\"";
                   proc.StartInfo.CreateNoWindow = true;
                   proc.StartInfo.WindowStyle = ProcessWindowStyle.Minimized;
                  // MessageBox.Show(proc.StartInfo.FileName + " " + proc.StartInfo.Arguments);
                   proc.Start();
                   procID = proc.Id;
                   proc.WaitForExit();
               }
                   //Process tProc = Process.GetProcessById(procID);
               
              // Process.Start((string)filename + ".rar");
               string tFile = (string)filename;
               
               if(tFile.Contains(".evt"))
               {

                   UpdateListBoxItem(listBox4,filename);
               }
               if(tFile.Contains(".ininlog"))
               {
                   UpdateListBoxItem(listBox2,filename);
               }
               if(tFile.Contains(".reg"))
               {
                   UpdateListBoxItem(listBox3,filename);
               }
           }
           catch(Exception ex)
           {
               MessageBox.Show(ex.Message);
           }
       }
       private void linkLabel1_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e)
       {
           setFolder();
       }

       private void Form1_Resize(object sender, EventArgs e)
       {
           if (FormWindowState.Minimized == this.WindowState)
           {
               mynotifyicon.Visible = true;
               mynotifyicon.ShowBalloonTip(500);
               this.Hide();
           }
           else if (FormWindowState.Normal == this.WindowState)
           {
               mynotifyicon.Visible = false;
               this.Show();
           }
       }
       private void linkLabel4_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
       {
           this.WindowState = FormWindowState.Minimized;
           this.Form1_Resize(sender, e);
           linkLabel4.Enabled = false;
           // int counter = 0;
           for (int counter = 0; counter < listBox2.Items.Count; counter++)
           {
               //listBox2.SelectedIndex = counter;
               //string filename = listBox2.SelectedItem.ToString();
               string filename = listBox2.Items[counter].ToString();
               //MessageBox.Show(filename);
               //ThreadPool.QueueUserWorkItem(new WaitCallback(createZip), filename);
               createZip(filename);
               //createZip(filename);
               //UpdateListBoxItem(listBox2, filename);
               filename = null;
               
           }
           for (int counter2 = 0; counter2 < listBox3.Items.Count; counter2++)
           {
               //listBox3.SelectedIndex = counter2;
               string filename = listBox3.Items[counter2].ToString();
               //MessageBox.Show(filename);
               //ThreadPool.QueueUserWorkItem(new WaitCallback(createZip), filename);
               createZip(filename);
               //createZip(filename);
               //UpdateListBoxItem(listBox3, filename);
               filename = null;
           }
           for (int counter3 = 0; counter3 < listBox4.Items.Count; counter3++)
           {
              // listBox4.SelectedIndex = counter3;
               string filename = listBox4.Items[counter3].ToString();
               //MessageBox.Show(filename);
               //ThreadPool.QueueUserWorkItem(new WaitCallback(createZip), filename);
               createZip(filename);
               //createZip(filename);
               //UpdateListBoxItem(listBox4, filename);
               filename = null;
           }

       }
       private void UpdateListBoxItem(ListBox lb, object item)
       {
           //int index;
           /*
           if (lb.InvokeRequired)
           {
               lb.BeginInvoke(new MethodInvoker(new MethodInvoker(delegate
               {
                   index = lb.items.IndexOf(item);
               })));
           }
           else
           {
               l.Items.Add(s);
           }
            * */
           try
           {
               if (lb.InvokeRequired)
               {
                   object[] paramList = new object[2];
                   paramList[0] = lb;
                   paramList[1] = item;
                   lb.Invoke(new updateList2(UpdateListBoxItem2),(object)paramList);
                   
               }
                 
               else
               {
                   int index = lb.Items.IndexOf(item);
                   //int currIndex = lb.SelectedIndex;
                   lb.BeginUpdate();
                   lb.ClearSelected();
                   
                   lb.Items[index] = (string)item + ": Has been zipped!";
                   
                   //lb.SelectedIndex = currIndex;
               }
               
           }
           finally
           {
               lb.EndUpdate();
           }
       }
       private void UpdateListBoxItem2(object paramList2)
       {
           object[] paramList = (object[])paramList2;
           ListBox lb = (ListBox)paramList[0];
           object item = paramList[1];
           int index = lb.Items.IndexOf(item);
          // int currIndex = lb.SelectedIndex;
           lb.BeginUpdate();
           lb.ClearSelected();
           lb.Items[index] = (string)item + ": Has been zipped!";
          // lb.SelectedIndex = currIndex;
       }

       private void mynotifyicon_MouseDoubleClick(object sender, MouseEventArgs e)
       {
           this.WindowState = FormWindowState.Normal;
           this.Form1_Resize(sender, e);
       }
      

     
    }

       
}
