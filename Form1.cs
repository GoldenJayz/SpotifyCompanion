using NAudio.CoreAudioApi;
using System;
using System.Diagnostics;
using System.Windows.Forms;
using Utilities;

namespace SpotifyCompanion
{
    public partial class Form1 : Form
    {
        private readonly globalKeyboardHook _gkh = new globalKeyboardHook();
        private readonly SessionCollection _sessions;
        private AudioSessionControl _session;
        private int _processId;
        private bool _hooked = false;


        public Form1()
        {
            InitializeComponent();
            var mde = new MMDeviceEnumerator();
            var md = mde.GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia);
            _sessions = md.AudioSessionManager.Sessions;

        }


        private void Form1_Load(object sender, EventArgs e)
        {
            _gkh.HookedKeys.Add(Keys.RShiftKey);
            _gkh.HookedKeys.Add(Keys.Home);
            _gkh.HookedKeys.Add(Keys.End);
            _gkh.KeyDown += new KeyEventHandler(gkh_KeyDown);
            HookSpotify();
            

        }


        private void HookSpotify()
        {
            while (_hooked == false)
            {
                Process[] pl = Process.GetProcessesByName("Spotify");

                foreach (Process process in pl)
                {
                    if (process.MainWindowTitle.Length <= 0) continue;
                    Console.WriteLine(process.MainWindowTitle);
                    _processId = process.Id;
                    _hooked = true;
                }

            }

            Console.WriteLine("hooked");

            for (int i = 0; i < _sessions.Count; i++)
            {
                if (_sessions[i].GetProcessID == _processId)
                {
                    this._session = _sessions[i];
                    break;
                }
            }
        }

        private void gkh_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.RShiftKey:
                    listBox1.Items.Add("Shift key pressed");
                    this._session.SimpleAudioVolume.Mute = !this._session.SimpleAudioVolume.Mute;
                    break;

                case Keys.Home:
                    listBox1.Items.Add("Home key pressed");
                    this._session.SimpleAudioVolume.Volume += (float) 0.10;
                    break;

                case Keys.End:
                    listBox1.Items.Add("End key pressed");
                    this._session.SimpleAudioVolume.Volume -= (float) 0.10;
                    break;
            }
        }


        private void listBox1_SelectedIndexChanged(object sender, EventArgs e) { }
    }
}
