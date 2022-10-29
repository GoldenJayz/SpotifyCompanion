using NAudio.CoreAudioApi;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Utilities;

namespace SpotifyCompanion
{
    public partial class Form1 : Form
    {
        private readonly globalKeyboardHook gkh = new globalKeyboardHook();
        private readonly MMDeviceEnumerator mde;
        private readonly MMDevice MD;
        private readonly SessionCollection sessions;
        private AudioSessionControl session;
        private int processId;



        public Form1()
        {
            InitializeComponent();

            mde = new MMDeviceEnumerator();
            MD = mde.GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia);
            sessions = MD.AudioSessionManager.Sessions;

        }



        private void Form1_Load(object sender, EventArgs e)
        {
            gkh.HookedKeys.Add(Keys.RShiftKey);
            gkh.HookedKeys.Add(Keys.Home);
            gkh.HookedKeys.Add(Keys.End);


            gkh.KeyDown += new KeyEventHandler(gkh_KeyDown);

            
            Process[] pl = Process.GetProcessesByName("Spotify");

            foreach (Process process in pl)
            {

                if (process.MainWindowTitle.Length > 0)
                {
                    Console.WriteLine(process.MainWindowTitle);
                    processId = process.Id;
                }
            }

            for (int i = 0; i < sessions.Count; i++)
            {
                if (sessions[i].GetProcessID == processId)
                {
                    this.session = sessions[i];
                }
            }
        }

        private void gkh_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.RShiftKey:
                    listBox1.Items.Add("Shift key pressed");

                    this.session.SimpleAudioVolume.Mute = !this.session.SimpleAudioVolume.Mute;
                    
                    break;

                case Keys.Home:
                    listBox1.Items.Add("Home key pressed");
                    this.session.SimpleAudioVolume.Volume += (float) 0.10;
                    break;

                case Keys.End:
                    listBox1.Items.Add("End key pressed");
                    this.session.SimpleAudioVolume.Volume -= (float) 0.10;
                    break;
            }
        }


        private void listBox1_SelectedIndexChanged(object sender, EventArgs e) { }
        

    }
}
