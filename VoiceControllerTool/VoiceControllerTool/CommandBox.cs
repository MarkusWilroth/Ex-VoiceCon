using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using VCTUtility; //Kommer inte behövs när VoiceData blir ersatt av ID system


namespace VoiceControllerTool {
    public class CommandBox : Image { //Allt detta ska tas bort (Har blivit ersatt)

        private int width = 600;
        private int height = 30;

        private int textHeight;
        private string fontFamily = "Times New Roman";
        private int fontSize = 15;
        private string workingDir;
        private string sourcePath;

        //Det som ska finnas i boxen
        Label lbComTitle, lbComText, lbComTag;
        Button btnTrain, btnDelete;

        private VoiceData voiceData; //Har informationen? SKA INTE GÖRAS SÅHÄR, Borde kopplas till listan på något sätt... ID behövs

        public CommandBox(VoiceData voiceData) {
            this.voiceData = voiceData;

            workingDir = Environment.CurrentDirectory;
            sourcePath = Path.Combine(Directory.GetParent(workingDir).Parent.FullName, "VCT-CommandBox.png");
            Console.WriteLine("Path: " + sourcePath);
            SetImage();
            SetText();
            SetButtons();
        }

        private void SetImage() {
            Width = width; //Sätter storlek på den
            Height = height;

            Source = new BitmapImage(new Uri(sourcePath));
        }

        private void SetText() {    //Sätter texten på boxen - Måste fixa position för dem
            CreateLable(lbComTitle, voiceData.voiceTitle);
            CreateLable(lbComText, voiceData.voiceText);
            CreateLable(lbComTag, voiceData.voiceTag);
        }

        private void CreateLable(Label lbl, string content) {
            lbl = new Label() {
                FontFamily = new FontFamily(fontFamily),
                FontSize = fontSize,
                Content = content,
            };
        }

        private void SetButtons() {
            btnTrain = CreateButton("Train");
            btnTrain.Click += new RoutedEventHandler(btnTrain_Click);

            btnDelete = CreateButton("Delete");
            btnDelete.Click += new RoutedEventHandler(btnDelete_Click);
        }

        private Button CreateButton(string content) {
            Button btn = new Button() {
                FontFamily = new FontFamily(fontFamily),
                FontSize = fontSize,
                Content = content,
            };

            return btn;
        }

        private void btnTrain_Click(object sender, RoutedEventArgs e) { //Klicakr på btnTrain
            Console.WriteLine("Train: " + voiceData.voiceTitle + ". Not implemented yet...");
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e) { //Ska vi ha att ett bekräftelse fönster dyker upp innan den tas bort?
            Console.WriteLine("Delete: " + voiceData.voiceTitle + ". Not implemented yet...");
        }
    }
}
