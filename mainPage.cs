﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Diagnostics;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace cc841.MScProject
{
    public partial class mainPage : Form
    {
        int selectedColor = 0;
        int[] workspaceArray = new int[64];
        // presets are loaded into programs and should not be changeable
        int[] savedArray1 = new int[64] { 0, 16, 32, 48, 64, 80, 96, 112, 128, 144, 160, 176, 192, 208, 224, 240, 256, 272, 288, 304, 320, 336, 352, 368, 384, 400, 416, 432, 448, 464, 480, 496, 512, 528, 544, 560, 576, 592, 608, 624, 640, 656, 672, 688, 704, 720, 736, 752, 768, 784, 800, 816, 832, 848, 864, 880, 896, 912, 928, 944, 960, 976, 992, 1008 };
        int[] savedArray2 = new int[64] { 0, 8, 16, 24, 32, 40, 48, 56, 64, 72, 80, 88, 96, 104, 112, 120, 128, 136, 144, 152, 160, 168, 176, 184, 192, 200, 208, 216, 224, 232, 240, 248, 256, 264, 272, 280, 288, 296, 304, 312, 320, 328, 336, 344, 352, 360, 368, 376, 384, 392, 400, 408, 416, 424, 432, 440, 448, 456, 464, 472, 480, 488, 496, 504 };
        int[] savedArray3 = new int[64] { 256, 264, 272, 280, 288, 296, 304, 312, 320, 328, 336, 344, 352, 360, 368, 376, 384, 392, 400, 408, 416, 424, 432, 440, 448, 456, 464, 472, 480, 488, 496, 504, 512, 520, 528, 536, 544, 552, 560, 568, 576, 584, 592, 600, 608, 616, 624, 632, 640, 648, 656, 664, 672, 680, 688, 696, 704, 712, 720, 728, 736, 744, 752, 760 };
        int[] savedArray4 = new int[64] { 1023, 16, 32, 48, 64, 80, 96, 112, 128, 144, 160, 176, 192, 208, 224, 240, 256, 272, 288, 304, 320, 336, 352, 368, 384, 400, 416, 432, 448, 464, 480, 496, 512, 528, 544, 560, 576, 592, 608, 624, 640, 656, 672, 688, 704, 720, 736, 752, 768, 784, 800, 816, 832, 848, 864, 880, 896, 912, 928, 944, 960, 976, 992, 1008 };
        int[] savedArray5 = new int[64] { 1023, 8, 16, 24, 32, 40, 48, 56, 64, 72, 80, 88, 96, 104, 112, 120, 128, 136, 144, 152, 160, 168, 176, 184, 192, 200, 208, 216, 224, 232, 240, 248, 256, 264, 272, 280, 288, 296, 304, 312, 320, 328, 336, 344, 352, 360, 368, 376, 384, 392, 400, 408, 416, 424, 432, 440, 448, 456, 464, 472, 480, 488, 496, 504 };
        int[] savedArray6 = new int[64] { 1023, 264, 272, 280, 288, 296, 304, 312, 320, 328, 336, 344, 352, 360, 368, 376, 384, 392, 400, 408, 416, 424, 432, 440, 448, 456, 464, 472, 480, 488, 496, 504, 512, 520, 528, 536, 544, 552, 560, 568, 576, 584, 592, 600, 608, 616, 624, 632, 640, 648, 656, 664, 672, 680, 688, 696, 704, 712, 720, 728, 736, 744, 752, 760 };

        SerialPort SP1 = new SerialPort("COM1", 115200);
        SerialPort SP2 = new SerialPort("COM2", 115200);
        SerialPort SP3 = new SerialPort("COM3", 115200);
        SerialPort SP4 = new SerialPort("COM4", 115200);
        SerialPort SP5 = new SerialPort("COM5", 115200);
        SerialPort SP6 = new SerialPort("COM6", 115200);
        int lastKnownCOM = 0;

        // custom input patterns are to be read from/written to file which is in format of "custom(n).txt"
        bool cloneMode = false;
        bool spdMode = false;
        int toggleMode = 1;
        double degfromvalue = 180.0f / 1024.0f;
        List<Button> buttonsList = new List<Button>();
        Stack<int[]> historyUndoStack = new Stack<int[]>();
        Stack<int[]> historyRedoStack = new Stack<int[]>();
        static string filepath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location).ToString();


        public mainPage()
        {
            InitializeComponent();

            buttonsList = new List<Button>{
            button1, button2, button3, button4, button5, button6, button7, button8, button9, button10,
            button11, button12, button13, button14, button15, button16, button17, button18, button19, button20,
            button21, button22, button23, button24, button25, button26, button27, button28, button29, button30,
            button31, button32, button33, button34, button35, button36, button37, button38, button39, button40,
            button41, button42, button43, button44, button45, button46, button47, button48, button49, button50,
            button51, button52, button53, button54, button55, button56, button57, button58, button59, button60,
            button61, button62, button63, button64
            };

            for (int i = 0; i < buttonsList.Count; i++)
            {
                buttonsList[i].Click += Button_Click; // Attach all buttons in the list with button click method.
            }

            presetButton1.Click += PresetsButton_Click;
            presetButton2.Click += PresetsButton_Click;
            presetButton3.Click += PresetsButton_Click;
            presetButton4.Click += PresetsButton_Click;
            presetButton5.Click += PresetsButton_Click;
            presetButton6.Click += PresetsButton_Click;
            loadCustomButton1.Click += PresetsButton_Click;
            customImageButton1.Click += PresetsButton_Click;
            loadCustomButton2.Click += PresetsButton_Click;
            customImageButton2.Click += PresetsButton_Click;
            loadCustomButton3.Click += PresetsButton_Click;
            customImageButton3.Click += PresetsButton_Click;
            loadCustomButton4.Click += PresetsButton_Click;
            customImageButton4.Click += PresetsButton_Click;

            saveCustomButton1.Click += saveWorkspaceToCustom_Click;
            saveCustomButton2.Click += saveWorkspaceToCustom_Click;
            saveCustomButton3.Click += saveWorkspaceToCustom_Click;
            saveCustomButton4.Click += saveWorkspaceToCustom_Click;

            // Initializing
            updateWorkspaceColor(workspaceArray);
            int[] pushArray = new int[64];
            workspaceArray.CopyTo(pushArray, 0);
            Debug.WriteLine("(Init) Undo Stack Size:" + historyUndoStack.Count.ToString() + " |Redo Stack Size:" + historyRedoStack.Count.ToString());
            Debug.WriteLine("current bin path is: " + filepath);

            // load Saved custom pattern images to preview buttons
            // Resource.resx would automatically assist this anyway, but this code is failsafe.
            customImageButton1.BackgroundImage = nonLockImageFromFile(filepath + "\\SavedCustomInputs\\custom1.png");
            customImageButton2.BackgroundImage = nonLockImageFromFile(filepath + "\\SavedCustomInputs\\custom2.png");
            customImageButton3.BackgroundImage = nonLockImageFromFile(filepath + "\\SavedCustomInputs\\custom3.png");
            customImageButton4.BackgroundImage = nonLockImageFromFile(filepath + "\\SavedCustomInputs\\custom4.png");

            //Initialize serial port
            CheckSPconnection();
        }

        public static Image nonLockImageFromFile(string path)
        {
            // Taken from https://stackoverflow.com/questions/4803935/free-file-locked-by-new-bitmapfilepath/8701748
            //function Image.FromFile that does not cause file locking
            var bytes = File.ReadAllBytes(path);
            var ms = new MemoryStream(bytes);
            var img = Image.FromStream(ms);
            return img;
        }

        public void CheckSPconnection()
        {
            // Check Serial Ports connection status and updates the status button accordingly.
            // Checking status is currently not done asynchoronously.
            // Implemented to be called from most user's action.
            // Only attempt to open if none of COM ports the status is not current open.
            if (!SP1.IsOpen && !SP2.IsOpen && !SP3.IsOpen && !SP4.IsOpen && !SP5.IsOpen && !SP6.IsOpen)
            {
                try { SP1.Open(); } catch (Exception e) { Debug.WriteLine("COM1 not Connected"); }
                try { SP2.Open(); } catch (Exception e) { Debug.WriteLine("COM2 not Connected"); }
                try { SP3.Open(); } catch (Exception e) { Debug.WriteLine("COM3 not Connected"); }
                try { SP4.Open(); } catch (Exception e) { Debug.WriteLine("COM4 not Connected"); }
                try { SP5.Open(); } catch (Exception e) { Debug.WriteLine("COM5 not Connected"); }
                try { SP6.Open(); } catch (Exception e) { Debug.WriteLine("COM6 not Connected"); }
                SPCOMnumLabel.Text = "Not connected to any COM Port";
                lastKnownCOM = 0;
            }
            // If ANY COM port is Open
            if (SP1.IsOpen || SP2.IsOpen || SP3.IsOpen || SP4.IsOpen || SP5.IsOpen || SP6.IsOpen)
            {
                SPstatusButton.Text = "Connected";
                SPstatusButton.BackColor = Color.Lime;
                if (SP1.IsOpen) { lastKnownCOM = 1; }
                else if (SP2.IsOpen) { lastKnownCOM = 2; }
                else if (SP3.IsOpen) { lastKnownCOM = 3; }
                else if (SP4.IsOpen) { lastKnownCOM = 4; }
                else if (SP5.IsOpen) { lastKnownCOM = 5; }
                else if (SP6.IsOpen) { lastKnownCOM = 6; }
                SPCOMnumLabel.Text = "Serial Port COM" + lastKnownCOM.ToString() + " connected";
                historyLabel.Text = "Serial Port COM"+ lastKnownCOM.ToString() +" connected"; 
            }
            else 
            {
                SPstatusButton.Text = "Disconnected";
                SPstatusButton.BackColor = Color.Red;
                historyLabel.Text = "Serial Port Disconnected";
                SPCOMnumLabel.Text = "Not connected to any COM Port";
                lastKnownCOM = 0;
            }
        }

        public void CaptureWorkspaceImage(string sentImgName)
        {
            Rectangle bounds = this.Bounds; // Get location bound of the application from screen size
            using (Bitmap bitmap = new Bitmap(450, 450))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    //Capture picture from indicated coordinate of workspace area
                    g.CopyFromScreen(new Point(bounds.Left + 433, bounds.Top + 122), Point.Empty, new Size(450, 450));
                }
                //bitmap.Save("SavedCustomInputs/" + sentImgName, ImageFormat.Png); //Does not work 
                bitmap.Save("SavedCustomInputs/" + sentImgName, ImageFormat.Png);

                if (sentImgName == "custom1.png")
                { 
                    customImageButton1.BackgroundImage = nonLockImageFromFile(filepath + "\\SavedCustomInputs\\" + sentImgName);
                }
                else if (sentImgName == "custom2.png")
                {
                    customImageButton2.BackgroundImage = nonLockImageFromFile(filepath + "\\SavedCustomInputs\\" + sentImgName);
                }
                else if (sentImgName == "custom3.png")
                {
                    customImageButton3.BackgroundImage = nonLockImageFromFile(filepath + "\\SavedCustomInputs\\" + sentImgName);
                }
                else if (sentImgName == "custom4.png")
                {
                    customImageButton4.BackgroundImage = nonLockImageFromFile(filepath + "\\SavedCustomInputs\\" + sentImgName);
                }
            }
        }

        public static Color ColorFromHSV(double hue)
        {
            // .NET framework does not have built-in RGB > HSV or HSV > Color conversion
            //adapted from https://stackoverflow.com/questions/1335426/is-there-a-built-in-c-net-system-api-for-hsv-to-rgb
            //in our case there is no change in Saturation or Value
            int saturation = 1; 
            int value = 1;
            hue /= 4; //input value ranges from 0-1023
            if (hue > 255) { hue = 255; } //a fail save to set hue to not exceed 255 since the programme is design to work with 0-255
            hue = 255 - hue - 8; //inverting value so that 255 is represeting red, and 0 representing blue
            //original Hue spectrum runs from 0 to 360, but we decided to use only 8-263 since it is representatively sufficient.
            int hi = Convert.ToInt32(Math.Floor(hue / 60)) % 6;
            double f = hue / 60 - Math.Floor(hue / 60);

            value = value * 255;
            int v = Convert.ToInt32(value);
            int p = Convert.ToInt32(value * (1 - saturation));
            int q = Convert.ToInt32(value * (1 - f * saturation));
            int t = Convert.ToInt32(value * (1 - (1 - f) * saturation));

            if (hi == 0)
                return Color.FromArgb(255, v, t, p);
            else if (hi == 1)
                return Color.FromArgb(255, q, v, p);
            else if (hi == 2)
                return Color.FromArgb(255, p, v, t);
            else if (hi == 3)
                return Color.FromArgb(255, p, q, v);
            else if (hi == 4)
                return Color.FromArgb(255, t, p, v);
            else
                return Color.FromArgb(255, v, p, q);
        }

        public void updateWorkspaceColor (int[] savedArray)
        {
           for (int i = 0; i < savedArray.Length; i++)
            {
                buttonsList[i].BackColor = ColorFromHSV(savedArray[i]);

                // recolor button's text to white if color is dark blue or dark red.
                if (savedArray[i] <= 160 || savedArray[i] >= 940) { buttonsList[i].ForeColor = SystemColors.ControlLightLight; }
                else { buttonsList[i].ForeColor = SystemColors.ControlText; }

                // display value on each button based on display mode.
                if (toggleMode == 1)
                {
                    buttonsList[i].Text = (i+1).ToString();
                }
                else if (toggleMode == 2) //if Toggle Input Value is on, load saved intensity number on the button's text.
                {
                    buttonsList[i].Text = savedArray[i].ToString();
                }
                else // if (toggleMode == 3) //Degree mode
                {
                    buttonsList[i].Text = (Math.Round(savedArray[i]*degfromvalue, 2)).ToString(); //Degree mode
                }     
            }    
        }

        public void saveWorkspaceToCustom_Click(object sender, EventArgs e)
        {
            CheckSPconnection();
            if (((Button)sender).Tag.ToString() == "sc1")
            {
                writeArrayToFile("\\SavedCustomInputs\\custom1.txt");
                CaptureWorkspaceImage("custom1.png");
            }
            if (((Button)sender).Tag.ToString() == "sc2")
            {
                writeArrayToFile("\\SavedCustomInputs\\custom2.txt");
                CaptureWorkspaceImage("custom2.png");
            }
            if (((Button)sender).Tag.ToString() == "sc3")
            {
                writeArrayToFile("\\SavedCustomInputs\\custom3.txt");
                CaptureWorkspaceImage("custom3.png");
            }
            if (((Button)sender).Tag.ToString() == "sc4")
            {
                writeArrayToFile("\\SavedCustomInputs\\custom4.txt");
                CaptureWorkspaceImage("custom4.png");
            }
        }

        public void readArrayFromFile(string filenamepath)
        {
            // Code adapted from https://social.msdn.microsoft.com/Forums/vstudio/en-US/3ea018ab-ffb0-427a-992a-4e78efcbe1f7/read-text-file-insert-data-into-array?forum=csharpgeneral
            // Number is read from save file, one line at a time, put into list and parsed to int which is then saved to workspace.
            List<int> temporarylist = new List<int>();
            int counter = 0;
            string[] stringArray = System.IO.File.ReadAllLines(@filepath+filenamepath);
            foreach (string readNumber in stringArray)
            {
                temporarylist.Add(Convert.ToInt32(readNumber.Trim()));
                workspaceArray[counter] = Convert.ToInt32(readNumber.Trim());
                ++counter;
                Debug.WriteLine("Value of " + readNumber + " is recorded to workspaceArray[" + counter.ToString() + "]");
            }
        }

        public void writeArrayToFile(string filenamepath)
        {
            // Code adapted from https://www.c-sharpcorner.com/article/c-sharp-write-to-file/
            // Convert workspace array (int) into array of strings  
            string[] writtenNumbers = workspaceArray.Select(x => x.ToString()).ToArray();
            // Write array of strings to a file using WriteAllLines.  
            // If the file does not exists, it will create a new file.  
            // This method automatically opens the file, writes to it, and closes file  
            File.WriteAllLines(@filepath + filenamepath, writtenNumbers);
            // Debug : Read the file  
            string readText = File.ReadAllText(@filepath + filenamepath);
            Debug.WriteLine("The text file " + filenamepath + " is now as follows:\n" + readText);
        }

        private void PresetsButton_Click(object sender, EventArgs e)
        {
            CheckSPconnection();
            //currently it is possible to Undo to Before clicking the presets
            //Redo to newly selected presets are also possible
            int[] pushUndoArray = new int[64];
            workspaceArray.CopyTo(pushUndoArray, 0);
            historyUndoStack.Push(pushUndoArray);
            undoButton.BackColor = SystemColors.Control;
            historyRedoStack.Clear(); //Redo stack cleared since previous branch is disregarded
            redoButton.BackColor = SystemColors.ControlDark;

            if (((Button)sender).Tag.ToString() == "p1")
            {
                savedArray1.CopyTo(workspaceArray,0);
                updateWorkspaceColor(workspaceArray);
            }
            else if (((Button)sender).Tag.ToString() == "p2")
            {
                savedArray2.CopyTo(workspaceArray, 0);
                updateWorkspaceColor(workspaceArray);
            }
            else if (((Button)sender).Tag.ToString() == "p3")
            {
                savedArray3.CopyTo(workspaceArray, 0);
                updateWorkspaceColor(workspaceArray);
            }
            else if (((Button)sender).Tag.ToString() == "p4")
            {
                savedArray4.CopyTo(workspaceArray, 0);
                updateWorkspaceColor(workspaceArray);
            }
            else if (((Button)sender).Tag.ToString() == "p5")
            {
                savedArray5.CopyTo(workspaceArray, 0);
                updateWorkspaceColor(workspaceArray);
            }
            else if (((Button)sender).Tag.ToString() == "p6")
            {
                savedArray6.CopyTo(workspaceArray, 0);
                updateWorkspaceColor(workspaceArray);
            }
            else if (((Button)sender).Tag.ToString() == "lc1" || ((Button)sender).Tag.ToString() == "cib1")
            {
                readArrayFromFile("\\SavedCustomInputs\\custom1.txt");
                updateWorkspaceColor(workspaceArray);
            }
            else if (((Button)sender).Tag.ToString() == "lc2" || ((Button)sender).Tag.ToString() == "cib2")
            {
                readArrayFromFile("\\SavedCustomInputs\\custom2.txt");
                updateWorkspaceColor(workspaceArray);
            }
            else if (((Button)sender).Tag.ToString() == "lc3" || ((Button)sender).Tag.ToString() == "cib3")
            {
                readArrayFromFile("\\SavedCustomInputs\\custom3.txt");
                updateWorkspaceColor(workspaceArray);
            }
            else if (((Button)sender).Tag.ToString() == "lc4" || ((Button)sender).Tag.ToString() == "cib4")
            {
                readArrayFromFile("\\SavedCustomInputs\\custom4.txt");
                updateWorkspaceColor(workspaceArray);
            }
        }

        private void Button_Click(object sender, EventArgs e)
        {
            CheckSPconnection();
            string buttonText = ((Button)sender).Tag.ToString();
            int arrayIndex = Int32.Parse(buttonText);
            arrayIndex -= 1; //step down since an array's index starts from 0

            if (workspaceArray[arrayIndex] != selectedColor) 
                //if the button's current color is the same as selected color, don't perform any action.
            {
                if (!cloneMode) // Input value from trackBar
                {
                    //write to History Stack
                    int[] pushUndoArray = new int[64];
                    workspaceArray.CopyTo(pushUndoArray, 0);
                    historyUndoStack.Push(pushUndoArray);
                    undoButton.BackColor = SystemColors.Control;
                    historyRedoStack.Clear(); //Redo stack cleared since previous branch is disregarded
                    redoButton.BackColor = SystemColors.ControlDark;
                    Debug.WriteLine("(New) Undo Stack Size:" + historyUndoStack.Count.ToString() + " |Redo Stack Size:" + historyRedoStack.Count.ToString());

                    workspaceArray[arrayIndex] = selectedColor;
                    ((Button)sender).BackColor = ColorFromHSV(selectedColor);

                    // recolor button's text to white if color is dark blue or dark red.
                    if (selectedColor <= 160 || selectedColor >= 940){ ((Button)sender).ForeColor = SystemColors.ControlLightLight; }
                    else { ((Button)sender).ForeColor = SystemColors.ControlText; }

                    // Update Text on button depending on which display mode is selected
                    if (toggleMode == 1) { ((Button)sender).Text = ((Button)sender).Tag.ToString(); }
                    else if (toggleMode == 2) { ((Button)sender).Text = selectedColor.ToString(); }
                    else { ((Button)sender).Text = (Math.Round(selectedColor*degfromvalue, 2)).ToString(); } //Degree mode 1024/180 = 5.68
                }
                else //Clone Input value from selected button
                {
                    previewButton.BackColor = ColorFromHSV(workspaceArray[arrayIndex]);
                    inputTextBox.Text = workspaceArray[arrayIndex].ToString();
                    intensitySelectTrackBar.Value = workspaceArray[arrayIndex];
                }
            }
        }

        private void intensitySelect_Scroll(object sender, EventArgs e)
        {
            //CheckSPconnection(); //keep checking for port here causes too much programme lag.
            selectedColor = intensitySelectTrackBar.Value; //minimum 0 and maximum 1023
            previewButton.BackColor = ColorFromHSV(selectedColor);
            inputTextBox.Text = selectedColor.ToString();
        }
        private void cloneModeCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            CheckSPconnection();
            cloneMode = cloneModeCheckBox.Checked;
            if (!cloneMode) 
            { 
                previewButton.BackColor = ColorFromHSV(selectedColor);
                inputTextBox.Text = selectedColor.ToString();
            }
        }
        private void toggleDisplayModesRadioButtons_CheckedChanged(object sender, EventArgs e)
        {
            CheckSPconnection();
            if (modeRadioButton1.Checked) { toggleMode = 1; }
            else if (modeRadioButton2.Checked) { toggleMode = 2; }
            else //if (modeRadioButton3.Checked) 
            { toggleMode = 3; }

            // Updating displayed value on Radio button click depending on selected mode
            for (int i = 0; i < workspaceArray.Length; i++)
            {
                // recolor button's text to white if color is dark blue or dark red.
                if (workspaceArray[i] <= 160 || workspaceArray[i] >= 940) { buttonsList[i].ForeColor = SystemColors.ControlLightLight; }
                else { buttonsList[i].ForeColor = SystemColors.ControlText; }

                if (toggleMode == 1) { buttonsList[i].Text = i.ToString(); }
                else if (toggleMode == 2) { buttonsList[i].Text = workspaceArray[i].ToString(); }
                else //if (toggle Mode == 3) 
                { buttonsList[i].Text = (Math.Round(workspaceArray[i]*degfromvalue, 2)).ToString(); } //Degree mode
            }
        }
        private void commitButton_Click(object sender, EventArgs e)
        {
            CheckSPconnection();
            if (lastKnownCOM != 0)
            {
                historyLabel.Text = "Input was sent through Serial Port COM" + lastKnownCOM.ToString();
                label2.Text = "Single serial input: 2.";
                String sentData = "2.";
                for (int i = 0; i < workspaceArray.Length; i++)
                {
                    label2.Text += workspaceArray[i].ToString() + ".";
                    sentData += workspaceArray[i].ToString() + ".";
                    Debug.WriteLine("4." + i.ToString() + "." + workspaceArray[i].ToString() + ".");
                }
                //avoid sending incomplete data
                if (lastKnownCOM == 1) { SP1.Write(sentData); }
                else if (lastKnownCOM == 2) { SP2.Write(sentData); }
                else if (lastKnownCOM == 3) { SP3.Write(sentData); }
                else if (lastKnownCOM == 4) { SP4.Write(sentData); }
                else if (lastKnownCOM == 5) { SP5.Write(sentData); }
                else if (lastKnownCOM == 6) { SP6.Write(sentData); }
            }
            else { 
                historyLabel.Text = "None of the Serial Port is open, no input was sent!";
                MessageBox.Show("None of the Serial Port is open, no input was sent!");
            }
        }
        private void undoButton_Click(object sender, EventArgs e)
        {
            CheckSPconnection();
            if (historyUndoStack.Count > 0)
            {
                Debug.WriteLine("(Begin) Undo Stack Size:" + historyUndoStack.Count.ToString() + " |Redo Stack Size:" + historyRedoStack.Count.ToString());
                int[] pushArrayUndo = new int[64];
                int[] pushArrayRedo = new int[64];
                // save current layout to Redo stack first
                workspaceArray.CopyTo(pushArrayRedo, 0);
                historyRedoStack.Push(pushArrayRedo);
                // load top of Undostack
                historyUndoStack.Pop().CopyTo(pushArrayUndo, 0);
                pushArrayUndo.CopyTo(workspaceArray, 0);
                updateWorkspaceColor(workspaceArray);
                if (historyUndoStack.Count > 0) { undoButton.BackColor = SystemColors.Control; }
                else { undoButton.BackColor = SystemColors.ControlDark; }
                redoButton.BackColor = SystemColors.Control;
                Debug.WriteLine("(End) Undo Stack Size:" + historyUndoStack.Count.ToString() + " |Redo Stack Size:" + historyRedoStack.Count.ToString());
            }
        }
        private void redoButton_Click(object sender, EventArgs e)
        {
            CheckSPconnection();
            if (historyRedoStack.Count > 0)
            {
                Debug.WriteLine("(Begin) Undo Stack Size:" + historyUndoStack.Count.ToString() + " |Redo Stack Size:" + historyRedoStack.Count.ToString());
                int[] pushArrayUndo = new int[64];
                int[] pushArrayRedo = new int[64];
                // save current layout to Redo stack first
                workspaceArray.CopyTo(pushArrayUndo, 0);
                historyUndoStack.Push(pushArrayUndo);
                // load top of Undostack
                historyRedoStack.Pop().CopyTo(pushArrayRedo, 0);
                pushArrayRedo.CopyTo(workspaceArray, 0);
                updateWorkspaceColor(workspaceArray);
                if (historyRedoStack.Count > 0) { redoButton.BackColor = SystemColors.Control; }
                else { redoButton.BackColor = SystemColors.ControlDark; }
                undoButton.BackColor = SystemColors.Control;
                Debug.WriteLine("(End) Undo Stack Size:" + historyUndoStack.Count.ToString() + " |Redo Stack Size:" + historyRedoStack.Count.ToString());
            }
        }

        private void inputTextBox_TextChanged(object sender, EventArgs e)
        {
            CheckSPconnection();
            inputTextBox.Text = inputTextBox.Text.Trim();
            int textBoxValue = Int32.Parse(inputTextBox.Text);
            if (textBoxValue < 0) { textBoxValue = 0; }
            else if (textBoxValue > 1023) { textBoxValue = 1023; }
            else
            {
                selectedColor = textBoxValue;
                textBoxValue = intensitySelectTrackBar.Value;
            }
        }

        private void inputTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            CheckSPconnection();
            inputTextBox.Text = inputTextBox.Text.Trim();
            //Check for Enter Key Press first first
            if (e.KeyChar == (char)Keys.Enter)
            {
                //Whatever is in the input field will always be parsable to number due to the conditioning below
                if (inputTextBox.Text == "") { inputTextBox.Text = "0"; } //if field is empty, set value to 0
                int textBoxValue = Int32.Parse(inputTextBox.Text);
                if (textBoxValue < 0) { textBoxValue = 0; MessageBox.Show("Input value cannot be lower than 0"); }
                else if (textBoxValue > 1023) { textBoxValue = 1023; MessageBox.Show("Input value cannot be higher than 1023"); }
                else
                {
                    selectedColor = textBoxValue;
                    intensitySelectTrackBar.Value = textBoxValue;
                    previewButton.BackColor = ColorFromHSV(textBoxValue);
                    e.Handled = true; //To suppress the Ding sounds, indicating that there is no error.
                }

            }
            // Only accepts numbers and backspace, no alphabets or "."
            else if (!char.IsDigit(e.KeyChar) && (e.KeyChar != (char)8))
            {
                MessageBox.Show("Input value can only be numbers");
                inputTextBox.Text = "";
            }
        }

        private void selectPresetDisplayCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            CheckSPconnection();
            spdMode = selectPresetDisplayCheckBox.Checked;
            if (!spdMode)
            {
                presetButton1.BackgroundImage = nonLockImageFromFile(filepath + "\\Assets\\Placeholder_ShapeImage.png");
                presetButton2.BackgroundImage = nonLockImageFromFile(filepath + "\\Assets\\Placeholder_ShapeImage.png");
                presetButton3.BackgroundImage = nonLockImageFromFile(filepath + "\\Assets\\Placeholder_ShapeImage.png");
                presetButton4.BackgroundImage = nonLockImageFromFile(filepath + "\\Assets\\Placeholder_ShapeImage.png");
                presetButton5.BackgroundImage = nonLockImageFromFile(filepath + "\\Assets\\Placeholder_ShapeImage.png");
                presetButton6.BackgroundImage = nonLockImageFromFile(filepath + "\\Assets\\Placeholder_ShapeImage.png");
            }
            else
            {
                presetButton1.BackgroundImage = nonLockImageFromFile(filepath + "\\Assets\\Placeholder_InputPatterns.png");
                presetButton2.BackgroundImage = nonLockImageFromFile(filepath + "\\Assets\\Placeholder_InputPatterns.png");
                presetButton3.BackgroundImage = nonLockImageFromFile(filepath + "\\Assets\\Placeholder_InputPatterns.png");
                presetButton4.BackgroundImage = nonLockImageFromFile(filepath + "\\Assets\\Placeholder_InputPatterns.png");
                presetButton5.BackgroundImage = nonLockImageFromFile(filepath + "\\Assets\\Placeholder_InputPatterns.png");
                presetButton6.BackgroundImage = nonLockImageFromFile(filepath + "\\Assets\\Placeholder_InputPatterns.png");
            }
        }
    }
}
