﻿using System;
using System.IO;
using VCTUtility;
using VoskEngine;
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
using System.Windows.Shapes;

namespace VCT {
    /// <summary>
    /// Interaction logic for AddWindow.xaml
    /// </summary>
    public partial class AddWindow : Window {
        private bool isRecording;
        private string title, text, tag;
        private MemoryStream rec;
        private VoiceData voiceData;
        private MainWindow main;

        private VCEngine engine = new VCEngine();

        public AddWindow(MainWindow main) {
            InitializeComponent();
            this.main = main;
        }

        private void StartRecording() {
            isRecording = true;
            AudioSystem.StartRecording();
        }

        private void StopRecording() {
            isRecording = false;
            rec = AudioSystem.StopRecording();

            
            engine.ValidateKeyphrase(rec, AudioSystem.GetSampleRate(), AudioSystem.GetChannels(), main.GetKeywords()); //return boolic value if successful then save rec to "database" 
            if (true) //if validation was súccesull ^
            {
                SaveRecording();
            }
            else
            {
                throw new ArgumentException("Recognition failed");
            }
        }

        private bool SaveRecording() {
            title = TbTitle.Text;
            text = TbText.Text;
            tag = TbTag.Text;

            if (VarifySave()) { //Kollar ifall det går att spara
                voiceData = new VoiceData(title, text, tag, rec);
                main.AddVoiceData(voiceData);
                return true;
            }
            return false;
        }

        private bool VarifySave() {
            if (rec == null || title == null || text == null || tag == null) {
                return false;
            } else {
                return true;
            }

        }

        private void BtnRecord_Click(object sender, RoutedEventArgs e) { //Startar - Slutar inspelning (Använda space också)
            if (!isRecording) {
                StartRecording();
                BtnRecord.Content = "Stop Recording";
            } else {
                StopRecording();
                BtnRecord.Content = "Start Recording";
            }
        }

        private void BtnReturn_Click(object sender, RoutedEventArgs e) { //Går tillbaka - sparar ifall man har gjort någon ändring
            SaveRecording();
            Close();
        }
    }
}
