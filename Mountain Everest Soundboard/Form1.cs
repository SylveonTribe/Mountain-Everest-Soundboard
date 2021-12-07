using Mountain_Everest_Soundboard.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;

namespace Mountain_Everest_Soundboard
{
  public partial class Form1 : Form
  {
    [DllImport("user32.dll")]
    public static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vlc);
    [DllImport("user32.dll")]
    public static extern bool UnregisterHotKey(IntPtr hWnd, int id);

    private NotifyIcon trayicon;
    
    Key key1;
    Key key2;
    Key key3;
    Key key4;
    string configpath = Path.Combine(Application.StartupPath, "keys.conf");
    string soundpath1 = string.Empty;
    string soundpath2 = string.Empty;
    string soundpath3 = string.Empty;
    string soundpath4 = string.Empty;
    bool isRunning = true;
    public Form1()
    {
      InitializeComponent();
      trayicon = new NotifyIcon()
      {
        Icon = this.Icon,
        ContextMenu = new ContextMenu(new MenuItem[] {
              new MenuItem("Show", ShowForm),
              new MenuItem("Exit", Exit)
        }),
        Visible = true
      };
    }

    private void Form1_Load(object sender, EventArgs e)
    {
      ReadConfigFile();

      Thread Keyb = new Thread(KeyboardCheck);
      Keyb.SetApartmentState(ApartmentState.STA);
      Keyb.Start();
    }

    private void ReadConfigFile()
    {
      if (File.Exists(configpath))
      {
        foreach (string line in File.ReadAllLines(configpath))
        {
          if (line.StartsWith("Key1path="))
          {
            soundpath1 = line.Substring(line.IndexOf("=") + 2);
            DPB1Path.Text = soundpath1;
          }
          if (line.StartsWith("Key2path="))
          {
            soundpath2 = line.Substring(line.IndexOf("=") + 2);
            DPB2Path.Text = soundpath2;
          }
          if (line.StartsWith("Key3path="))
          {
            soundpath3 = line.Substring(line.IndexOf("=") + 2);
          }
          if (line.StartsWith("Key4path="))
          {
            soundpath4 = line.Substring(line.IndexOf("=") + 2);
          }
        }
      }

      key1 = Key.F21;
      key2 = Key.F22;
      key3 = Key.F23;
      key4 = Key.F24;
    }

    private void KeyboardCheck()
    {
      while (isRunning)
      {
        Thread.Sleep(40);
        if ((Keyboard.GetKeyStates(key1) & KeyStates.Down) > 0 && !String.IsNullOrEmpty(soundpath1))
        {
          System.Media.SoundPlayer player = new System.Media.SoundPlayer(soundpath1);
          player.Play();
        }
        if ((Keyboard.GetKeyStates(key2) & KeyStates.Down) > 0 && !String.IsNullOrEmpty(soundpath2))
        {
          System.Media.SoundPlayer player = new System.Media.SoundPlayer(soundpath2);
          player.Play();
        }
        if ((Keyboard.GetKeyStates(key3) & KeyStates.Down) > 0 && !String.IsNullOrEmpty(soundpath3))
        {
          System.Media.SoundPlayer player = new System.Media.SoundPlayer(soundpath3);
          player.Play();
        }
        if ((Keyboard.GetKeyStates(key4) & KeyStates.Down) > 0 && !String.IsNullOrEmpty(soundpath4))
        {
          System.Media.SoundPlayer player = new System.Media.SoundPlayer(soundpath4);
          player.Play();
        }
      }
    }
    private void Form1_FormClosed(object sender, FormClosedEventArgs e)
    {
      isRunning = false;
    }

    private void DPB1Select_Click(object sender, EventArgs e)
    {
      SelectSound.FileName = DPB1Path.Text;
      SelectSound.ShowDialog();
      soundpath1 = SelectSound.FileName;
      DPB1Path.Text = SelectSound.FileName;
    }

    private void DPB2Select_Click(object sender, EventArgs e)
    {
      SelectSound.FileName = DPB2Path.Text;
      SelectSound.ShowDialog();
      soundpath2 = SelectSound.FileName;
      DPB2Path.Text = SelectSound.FileName;
    }

    private void DPB3Select_Click(object sender, EventArgs e)
    {
      SelectSound.FileName = DPB3Path.Text;
      SelectSound.ShowDialog();
      soundpath3 = SelectSound.FileName;
      DPB3Path.Text = SelectSound.FileName;
    }

    private void DPB4Select_Click(object sender, EventArgs e)
    {
      SelectSound.FileName = DPB4Path.Text;
      SelectSound.ShowDialog();
      soundpath4 = SelectSound.FileName;
      DPB4Path.Text = SelectSound.FileName;
    }

    private void button3_Click(object sender, EventArgs e)
    {
      WriteConfig();
    }

    private void WriteConfig()
    {
      if (File.Exists(configpath))
      {
        File.Delete(configpath);
      }

      FileStream stream = new FileStream(configpath, FileMode.OpenOrCreate);
      using (StreamWriter writer = new StreamWriter(stream))
      {
        writer.WriteLine("Key1path= " + DPB1Path.Text);
        writer.WriteLine("Key2path= " + DPB2Path.Text);
        writer.WriteLine("Key3path= " + DPB3Path.Text);
        writer.WriteLine("Key4path= " + DPB4Path.Text);
      }
    }

    private void ShowForm(object sender, EventArgs e)
    {
      Show();
      this.WindowState = FormWindowState.Normal;
    }

    private void Exit(object sender, EventArgs e)
    {
      isRunning = false;
      Application.Exit();
    }

    private void Form1_Resize(object sender, EventArgs e)
    {
      if (this.WindowState == FormWindowState.Minimized)
      {
        Hide();
      }
    }
  }
}
