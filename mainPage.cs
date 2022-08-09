using System;
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

// Possible list of improvments that are realized before user test but yet not implemented
// > Connection status update done should be done asynchoronously
// > Status messsage filtering with tickboxes
// >> Click to generate focal point mode
// > Tool tips
// > Setting intensity (loudness) of all speakers on load/ on renewed connection
// > Junk code clean ups

namespace cc841.MScProject
{
    public partial class mainPage : Form
    {
        int selectedColor = 0;
        int[] workspaceArray = new int[64];
        // presets are loaded into programs and should not be changeable
        int[] savedArray1 = new int[64] { 0, 16, 32, 48, 64, 80, 96, 112, 128, 144, 160, 176, 192, 208, 224, 240, 256, 272, 288, 304, 320, 336, 352, 368, 384, 400, 416, 432, 448, 464, 480, 496, 512, 528, 544, 560, 576, 592, 608, 624, 640, 656, 672, 688, 704, 720, 736, 752, 768, 768, 768, 768, 768, 768, 768, 768, 768, 768, 768, 768, 768, 768, 768, 768 };
        int[] savedArray2 = new int[64] { 0, 8, 16, 24, 32, 40, 48, 56, 64, 72, 80, 88, 96, 104, 112, 120, 128, 136, 144, 152, 160, 168, 176, 184, 192, 200, 208, 216, 224, 232, 240, 248, 256, 264, 272, 280, 288, 296, 304, 312, 320, 328, 336, 344, 352, 360, 368, 376, 384, 392, 400, 408, 416, 424, 432, 440, 448, 456, 464, 472, 480, 488, 496, 504 };
        int[] savedArray3 = new int[64] { 256, 264, 272, 280, 288, 296, 304, 312, 320, 328, 336, 344, 352, 360, 368, 376, 384, 392, 400, 408, 416, 424, 432, 440, 448, 456, 464, 472, 480, 488, 496, 504, 512, 520, 528, 536, 544, 552, 560, 568, 576, 584, 592, 600, 608, 616, 624, 632, 640, 648, 656, 664, 672, 680, 688, 696, 704, 712, 720, 728, 736, 744, 752, 760 };
        int[] savedArray4 = new int[64] { 768, 16, 32, 48, 64, 80, 96, 112, 128, 144, 160, 176, 192, 208, 224, 240, 256, 272, 288, 304, 320, 336, 352, 368, 384, 400, 416, 432, 448, 464, 480, 496, 512, 528, 544, 560, 576, 592, 608, 624, 640, 656, 672, 688, 704, 720, 736, 752, 768, 768, 768, 768, 768, 768, 768, 768, 768, 768, 768, 768, 768, 768, 768, 768 };
        int[] savedArray5 = new int[64] { 768, 8, 16, 24, 32, 40, 48, 56, 64, 72, 80, 88, 96, 104, 112, 120, 128, 136, 144, 152, 160, 168, 176, 184, 192, 200, 208, 216, 224, 232, 240, 248, 256, 264, 272, 280, 288, 296, 304, 312, 320, 328, 336, 344, 352, 360, 368, 376, 384, 392, 400, 408, 416, 424, 432, 440, 448, 456, 464, 472, 480, 488, 496, 504 };
        int[] savedArray6 = new int[64] { 768, 264, 272, 280, 288, 296, 304, 312, 320, 328, 336, 344, 352, 360, 368, 376, 384, 392, 400, 408, 416, 424, 432, 440, 448, 456, 464, 472, 480, 488, 496, 504, 512, 520, 528, 536, 544, 552, 560, 568, 576, 584, 592, 600, 608, 616, 624, 632, 640, 648, 656, 664, 672, 680, 688, 696, 704, 712, 720, 728, 736, 744, 752, 760 };

        int[] savedArray5A = new int[64] { 200, 8, 16, 24, 32, 40, 48, 56, 64, 72, 80, 88, 96, 104, 112, 120, 128, 136, 144, 152, 160, 168, 176, 184, 192, 200, 208, 216, 224, 232, 240, 248, 256, 264, 272, 280, 288, 296, 304, 312, 320, 328, 336, 344, 352, 360, 368, 376, 384, 392, 400, 408, 416, 424, 432, 440, 448, 456, 464, 472, 480, 488, 496, 504 };
        int[] savedArray5B = new int[64] { 400, 8, 16, 24, 32, 40, 48, 56, 64, 72, 80, 88, 96, 104, 112, 120, 128, 136, 144, 152, 160, 168, 176, 184, 192, 200, 208, 216, 224, 232, 240, 248, 256, 264, 272, 280, 288, 296, 304, 312, 320, 328, 336, 344, 352, 360, 368, 376, 384, 392, 400, 408, 416, 424, 432, 440, 448, 456, 464, 472, 480, 488, 496, 504 };
        int[] savedArray5C = new int[64] { 600, 8, 16, 24, 32, 40, 48, 56, 64, 72, 80, 88, 96, 104, 112, 120, 128, 136, 144, 152, 160, 168, 176, 184, 192, 200, 208, 216, 224, 232, 240, 248, 256, 264, 272, 280, 288, 296, 304, 312, 320, 328, 336, 344, 352, 360, 368, 376, 384, 392, 400, 408, 416, 424, 432, 440, 448, 456, 464, 472, 480, 488, 496, 504 };
        int[] savedArray5D = new int[64] { 768, 8, 16, 24, 32, 40, 48, 56, 64, 72, 80, 88, 96, 104, 112, 120, 128, 136, 144, 152, 160, 168, 176, 184, 192, 200, 208, 216, 224, 232, 240, 248, 256, 264, 272, 280, 288, 296, 304, 312, 320, 328, 336, 344, 352, 360, 368, 376, 384, 392, 400, 408, 416, 424, 432, 440, 448, 456, 464, 472, 480, 488, 496, 504 };
        int[] savedArray5E = new int[64] { 100, 8, 16, 24, 32, 40, 48, 56, 64, 72, 80, 88, 96, 104, 112, 120, 128, 136, 144, 152, 160, 168, 176, 184, 192, 200, 208, 216, 224, 232, 240, 248, 256, 264, 272, 280, 288, 296, 304, 312, 320, 328, 336, 344, 352, 360, 368, 376, 384, 392, 400, 408, 416, 424, 432, 440, 448, 456, 464, 472, 480, 488, 496, 504 };

        int[] savedArray6A = new int[64] { 100, 264, 272, 280, 288, 296, 304, 312, 320, 328, 336, 344, 352, 360, 368, 376, 384, 392, 400, 408, 416, 424, 432, 440, 448, 456, 464, 472, 480, 488, 496, 504, 512, 520, 528, 536, 544, 552, 560, 568, 576, 584, 592, 600, 608, 616, 624, 632, 640, 648, 656, 664, 672, 680, 688, 696, 704, 712, 720, 728, 736, 744, 752, 760 };
        int[] savedArray6B = new int[64] { 300, 264, 272, 280, 288, 296, 304, 312, 320, 328, 336, 344, 352, 360, 368, 376, 384, 392, 400, 408, 416, 424, 432, 440, 448, 456, 464, 472, 480, 488, 496, 504, 512, 520, 528, 536, 544, 552, 560, 568, 576, 584, 592, 600, 608, 616, 624, 632, 640, 648, 656, 664, 672, 680, 688, 696, 704, 712, 720, 728, 736, 744, 752, 760 };
        int[] savedArray6C = new int[64] { 500, 264, 272, 280, 288, 296, 304, 312, 320, 328, 336, 344, 352, 360, 368, 376, 384, 392, 400, 408, 416, 424, 432, 440, 448, 456, 464, 472, 480, 488, 496, 504, 512, 520, 528, 536, 544, 552, 560, 568, 576, 584, 592, 600, 608, 616, 624, 632, 640, 648, 656, 664, 672, 680, 688, 696, 704, 712, 720, 728, 736, 744, 752, 760 };
        int[] savedArray6D = new int[64] { 700, 264, 272, 280, 288, 296, 304, 312, 320, 328, 336, 344, 352, 360, 368, 376, 384, 392, 400, 408, 416, 424, 432, 440, 448, 456, 464, 472, 480, 488, 496, 504, 512, 520, 528, 536, 544, 552, 560, 568, 576, 584, 592, 600, 608, 616, 624, 632, 640, 648, 656, 664, 672, 680, 688, 696, 704, 712, 720, 728, 736, 744, 752, 760 };
        int[] savedArray6E = new int[64] { 768, 264, 272, 280, 288, 296, 304, 312, 320, 328, 336, 344, 352, 360, 368, 376, 384, 392, 400, 408, 416, 424, 432, 440, 448, 456, 464, 472, 480, 488, 496, 504, 512, 520, 528, 536, 544, 552, 560, 568, 576, 584, 592, 600, 608, 616, 624, 632, 640, 648, 656, 664, 672, 680, 688, 696, 704, 712, 720, 728, 736, 744, 752, 760 };

        int selectedPattern = 0;
        int selectedLatency = 100;
        int persistentIndex = 0;
        List<int[]> savedArrayList5 = new List<int[]>();
        List<int[]> savedArrayList6 = new List<int[]>();

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
        bool looping = false;
        int toggleMode = 1;
        List<Button> buttonsList = new List<Button>();
        Stack<int[]> historyUndoStack = new Stack<int[]>();
        Stack<int[]> historyRedoStack = new Stack<int[]>();
        static string filepath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location).ToString();


        public mainPage()
        {
            InitializeComponent();

            savedArrayList5.Add(savedArray5A);
            savedArrayList5.Add(savedArray5B);
            savedArrayList5.Add(savedArray5C);
            savedArrayList5.Add(savedArray5D);
            savedArrayList5.Add(savedArray5E);

            savedArrayList6.Add(savedArray6A);
            savedArrayList6.Add(savedArray6B);
            savedArrayList6.Add(savedArray6C);
            savedArrayList6.Add(savedArray6D);
            savedArrayList6.Add(savedArray6E);

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

            // > Setting additional parameters for Status Message Textbox
            // Set the Multiline property to true.
            // Add vertical scroll bars to the TextBox control.
            // Allow the TAB key to be entered in the TextBox control.
            // Set WordWrap to true to allow text to wrap to the next line.
            // Set the default text of the control.
            statusMessagesTextBox.Text = "Initializing";
            statusMessagesTextBox.AppendText(Environment.NewLine + "current bin path is: " + filepath);
            statusMessagesTextBox.AppendText(Environment.NewLine + "(Init) Undo Stack Size:" + historyUndoStack.Count.ToString() + " | Redo Stack Size:" + historyRedoStack.Count.ToString());

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

            //Loading Pattern 1
            savedArray1.CopyTo(workspaceArray, 0);
            updateWorkspaceColor(workspaceArray);
            selectedPattern = 1;

            //Initialize serial port
            CheckSPconnection();
        }

        public double degFromValue(int value)
        {
            double deg = -32.22 + (value * 0.342) + (-1.98 * Math.Pow(10,-4) * Math.Pow(value,2));
            return (deg);
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
                try { SP1.Open(); } catch { Debug.WriteLine("COM1 not Connected"); }
                try { SP2.Open(); } catch { Debug.WriteLine("COM2 not Connected"); }
                try { SP3.Open(); } catch { Debug.WriteLine("COM3 not Connected"); }
                try { SP4.Open(); } catch { Debug.WriteLine("COM4 not Connected"); }
                try { SP5.Open(); } catch { Debug.WriteLine("COM5 not Connected"); }
                try { SP6.Open(); } catch { Debug.WriteLine("COM6 not Connected"); }
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
                historyLabel.Text = "Serial Port COM" + lastKnownCOM.ToString() + " connected";
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
                    g.CopyFromScreen(new Point(bounds.Left + 420, bounds.Top + 100), Point.Empty, new Size(450, 450));
                    // alt value g.CopyFromScreen(new Point(bounds.Left + 650, bounds.Top + 200), Point.Empty, new Size(450, 450));
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

            if (hue == 0)
            { return Color.Black; } //black represent maximum flat floor value.
            else
            {

                int saturation = 1;
                int value = 1;
                hue /= 2; //input value ranges from 0-768, but colour representation only ranges in in 0-215 to represent diminishing returns
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
        }

        public void updateWorkspaceColor(int[] savedArray)
        {
            for (int i = 0; i < savedArray.Length; i++)
            {
                buttonsList[i].BackColor = ColorFromHSV(savedArray[i]);

                // recolor button's text to white if color is dark blue or dark red.
                if (savedArray[i] <= 100 || savedArray[i] >= 480) { buttonsList[i].ForeColor = SystemColors.ControlLightLight; }
                else { buttonsList[i].ForeColor = SystemColors.ControlText; }

                // display value on each button based on display mode.
                if (toggleMode == 1)
                {
                    buttonsList[i].Text = (i + 1).ToString();
                }
                else if (toggleMode == 2) //if Toggle Input Value is on, load saved intensity number on the button's text.
                {
                    buttonsList[i].Text = savedArray[i].ToString();
                }
                else // if (toggleMode == 3) //Degree mode
                {
                    buttonsList[i].Text = Math.Round(degFromValue(savedArray[i]), 2).ToString() + "°"; //Degree mode
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
                statusMessagesTextBox.AppendText(Environment.NewLine + "Workspace saved to Custom pattern 1");
            }
            if (((Button)sender).Tag.ToString() == "sc2")
            {
                writeArrayToFile("\\SavedCustomInputs\\custom2.txt");
                CaptureWorkspaceImage("custom2.png");
                statusMessagesTextBox.AppendText(Environment.NewLine + "Workspace saved to Custom pattern 2");
            }
            if (((Button)sender).Tag.ToString() == "sc3")
            {
                writeArrayToFile("\\SavedCustomInputs\\custom3.txt");
                CaptureWorkspaceImage("custom3.png");
                statusMessagesTextBox.AppendText(Environment.NewLine + "Workspace saved to Custom pattern 3");
            }
            if (((Button)sender).Tag.ToString() == "sc4")
            {
                writeArrayToFile("\\SavedCustomInputs\\custom4.txt");
                CaptureWorkspaceImage("custom4.png");
                statusMessagesTextBox.AppendText(Environment.NewLine + "Workspace saved to Custom pattern 4");
            }
        }

        public void readArrayFromFile(string filenamepath)
        {
            // Code adapted from https://social.msdn.microsoft.com/Forums/vstudio/en-US/3ea018ab-ffb0-427a-992a-4e78efcbe1f7/read-text-file-insert-data-into-array?forum=csharpgeneral
            // Number is read from save file, one line at a time, put into list and parsed to int which is then saved to workspace.
            List<int> temporarylist = new List<int>();
            int counter = 0;
            string[] stringArray = System.IO.File.ReadAllLines(@filepath + filenamepath);
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
            //statusMessagesTextBox.AppendText(Environment.NewLine + "The text file " + filenamepath + " is now as follows:\n" + readText);
        }

        private void PresetsButton_Click(object sender, EventArgs e)
        {
            CheckSPconnection();
            //currently it is possible to Undo to Before clicking the presets
            //Redo to newly selected presets are also possible
            int[] pushUndoArray = new int[64];
            workspaceArray.CopyTo(pushUndoArray, 0);
            historyUndoStack.Push(pushUndoArray);
            undoButton.Enabled = true;
            redoButton.Enabled = false;
            historyRedoStack.Clear(); //Redo stack cleared since previous branch is disregarded
            loopStartStopButton.Enabled = false; //disable first, will be enabled accordingly
            loopStartStopButton.Text = "Loop";
            loopPrevPatternButton.Enabled = false;
            loopNextPatternButton.Enabled = false;
            loopLatencyTextBox.Enabled = false;
            persistentIndex = 0;
            if (((Button)sender).Tag.ToString() == "p1")
            {
                savedArray1.CopyTo(workspaceArray, 0);
                updateWorkspaceColor(workspaceArray);
                selectedPattern = 1;
                statusMessagesTextBox.AppendText(Environment.NewLine + "Preset 1 loaded to workspace");
            }
            else if (((Button)sender).Tag.ToString() == "p2")
            {
                savedArray2.CopyTo(workspaceArray, 0);
                updateWorkspaceColor(workspaceArray);
                selectedPattern = 2;
                statusMessagesTextBox.AppendText(Environment.NewLine + "Preset 2 loaded to workspace");
            }
            else if (((Button)sender).Tag.ToString() == "p3")
            {
                savedArray3.CopyTo(workspaceArray, 0);
                updateWorkspaceColor(workspaceArray);
                selectedPattern = 3;
                statusMessagesTextBox.AppendText(Environment.NewLine + "Preset 3 loaded to workspace");
            }
            else if (((Button)sender).Tag.ToString() == "p4")
            {
                savedArray4.CopyTo(workspaceArray, 0);
                updateWorkspaceColor(workspaceArray);
                selectedPattern = 4;
                statusMessagesTextBox.AppendText(Environment.NewLine + "Preset 4 loaded to workspace");
            }
            else if (((Button)sender).Tag.ToString() == "p5")
            {
                savedArrayList5[0].CopyTo(workspaceArray, 0);
                updateWorkspaceColor(workspaceArray);
                selectedPattern = 5;
                loopStartStopButton.Enabled = true;
                loopStartStopButton.Text = "Start Loop";
                loopNextPatternButton.Enabled = true;
                loopPrevPatternButton.Enabled = true;
                loopLatencyTextBox.Enabled = true;
                statusMessagesTextBox.AppendText(Environment.NewLine + "Preset set 5 loaded to workspace");
            }
            else if (((Button)sender).Tag.ToString() == "p6")
            {
                savedArrayList6[0].CopyTo(workspaceArray, 0);
                updateWorkspaceColor(workspaceArray);
                selectedPattern = 6;
                loopStartStopButton.Enabled = true;
                loopStartStopButton.Text = "Start Loop";
                loopNextPatternButton.Enabled = true;
                loopPrevPatternButton.Enabled = true;
                loopLatencyTextBox.Enabled = true;
                statusMessagesTextBox.AppendText(Environment.NewLine + "Preset set 6 loaded to workspace");
            }
            else if (((Button)sender).Tag.ToString() == "lc1" || ((Button)sender).Tag.ToString() == "cib1")
            {
                readArrayFromFile("\\SavedCustomInputs\\custom1.txt");
                updateWorkspaceColor(workspaceArray);
                selectedPattern = 7; 
                statusMessagesTextBox.AppendText(Environment.NewLine + "Custom pattern 1 loaded to workspace");
            }
            else if (((Button)sender).Tag.ToString() == "lc2" || ((Button)sender).Tag.ToString() == "cib2")
            {
                readArrayFromFile("\\SavedCustomInputs\\custom2.txt");
                updateWorkspaceColor(workspaceArray);
                selectedPattern = 8;
                statusMessagesTextBox.AppendText(Environment.NewLine + "Custom pattern 2 loaded to workspace");
            }
            else if (((Button)sender).Tag.ToString() == "lc3" || ((Button)sender).Tag.ToString() == "cib3")
            {
                readArrayFromFile("\\SavedCustomInputs\\custom3.txt");
                updateWorkspaceColor(workspaceArray);
                selectedPattern = 9;
                statusMessagesTextBox.AppendText(Environment.NewLine + "Custom pattern 3 loaded to workspace");
            }
            else if (((Button)sender).Tag.ToString() == "lc4" || ((Button)sender).Tag.ToString() == "cib4")
            {
                readArrayFromFile("\\SavedCustomInputs\\custom4.txt");
                updateWorkspaceColor(workspaceArray);
                selectedPattern = 10;
                statusMessagesTextBox.AppendText(Environment.NewLine + "Custom pattern 4 loaded to workspace");
            }
            looping = false;
        }

        private void Button_Click(object sender, EventArgs e)
        {
            CheckSPconnection();
            string buttonText = ((Button)sender).Tag.ToString();
            int arrayIndex = Int32.Parse(buttonText);
            arrayIndex -= 1; //step down since an array's index starts from 0

            //if the button's current color is the same as selected color, don't perform any action.
            if (!cloneMode && (workspaceArray[arrayIndex] != selectedColor))// Input value from trackBar
            {
                //write to History Stack
                int[] pushUndoArray = new int[64];
                workspaceArray.CopyTo(pushUndoArray, 0);
                historyUndoStack.Push(pushUndoArray);
                undoButton.Enabled = true;
                historyRedoStack.Clear(); //Redo stack cleared since previous branch is disregarded
                redoButton.Enabled = false;
                Debug.WriteLine("(New) Undo Stack Size:" + historyUndoStack.Count.ToString() + " |Redo Stack Size:" + historyRedoStack.Count.ToString());
                statusMessagesTextBox.AppendText(Environment.NewLine + "(New) Undo Stack Size:" + historyUndoStack.Count.ToString() + " | Redo Stack Size:" + historyRedoStack.Count.ToString()); workspaceArray[arrayIndex] = selectedColor;
                statusMessagesTextBox.AppendText(Environment.NewLine + "Assign value to workspaceArray[" + arrayIndex + "] = " + workspaceArray[arrayIndex]);
                ((Button)sender).BackColor = ColorFromHSV(selectedColor);

                // recolor button's text to white if color is dark blue or dark red.
                if (selectedColor <= 100 || selectedColor >= 480) { ((Button)sender).ForeColor = SystemColors.ControlLightLight; }
                else { ((Button)sender).ForeColor = SystemColors.ControlText; }

                // Update Text on button depending on which display mode is selected
                if (toggleMode == 1) { ((Button)sender).Text = ((Button)sender).Tag.ToString(); }
                else if (toggleMode == 2) { ((Button)sender).Text = selectedColor.ToString(); }
                else { ((Button)sender).Text = Math.Round(degFromValue(selectedColor), 2).ToString() + "°"; } //Degree mode
            }
            else if (cloneMode) //Clone Input value from selected button
            {
                previewButton.BackColor = ColorFromHSV(workspaceArray[arrayIndex]);
                inputTextBox.Text = workspaceArray[arrayIndex].ToString();
                intensitySelectTrackBar.Value = workspaceArray[arrayIndex];
                selectedColor = workspaceArray[arrayIndex]; // have selected color take colour from latest clone mode value, so it functions corretly when switch back.
                statusMessagesTextBox.AppendText(Environment.NewLine + "Clone value from workspaceArray[" + arrayIndex + "] = " + workspaceArray[arrayIndex]);
            }
        }

        private void intensitySelect_Scroll(object sender, EventArgs e)
        {
            //CheckSPconnection(); //keep checking for port here causes too much programme lag.
            selectedColor = intensitySelectTrackBar.Value; //minimum 0 and maximum 768
            previewButton.BackColor = ColorFromHSV(selectedColor);
            inputTextBox.Text = selectedColor.ToString();
        }
        
        private void cloneModeCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            CheckSPconnection();
            // Fail safe override, may not be needed
            /*if (!cloneMode)
            {
                previewButton.BackColor = ColorFromHSV(selectedColor);
                inputTextBox.Text = selectedColor.ToString();
            }*/
            cloneMode = cloneModeCheckBox.Checked; 
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
                if (workspaceArray[i] <= 100 || workspaceArray[i] >= 480) { buttonsList[i].ForeColor = SystemColors.ControlLightLight; }
                else { buttonsList[i].ForeColor = SystemColors.ControlText; }

                if (toggleMode == 1) { buttonsList[i].Text = (i + 1).ToString(); }
                else if (toggleMode == 2) { buttonsList[i].Text = workspaceArray[i].ToString(); }
                else //if (toggle Mode == 3) 
                {
                    buttonsList[i].Text = Math.Round(degFromValue(workspaceArray[i]), 2).ToString() + "°"; //Degree mode
                }
            }
        }
        
        private void commitButton_Click(object sender, EventArgs e)
        {
            CheckSPconnection();
            sentPatternsThrough(workspaceArray);
        }

        private void sentPatternsThrough(int[] sentPattern)
        {
            if (lastKnownCOM != 0)
            {
                historyLabel.Text = "Input was sent through Serial Port COM" + lastKnownCOM.ToString();
                statusMessagesTextBox.AppendText(Environment.NewLine + "Input was sent through Serial Port COM" + lastKnownCOM.ToString());
                label2.Text = "Last Speed: " + selectedLatency.ToString() + "ms";//, Single serial input: 2.";
                //String sentData = "2.";
                String sentData = "";
                for (int i = 0; i < sentPattern.Length; i++)
                {
                    sentData = "1.";
                    //label2.Text += sentPattern[i].ToString() + ".";
                    sentData += i.ToString() + "." + sentPattern[i].ToString() + ".";
                    statusMessagesTextBox.AppendText(Environment.NewLine + sentData);
                    //Debug.WriteLine("4." + i.ToString() + "." + sentPattern[i].ToString() + ".");
                    //avoid sending incomplete data
                    if (lastKnownCOM == 1) { SP1.Write(sentData); }
                    else if (lastKnownCOM == 2) { SP2.Write(sentData); }
                    else if (lastKnownCOM == 3) { SP3.Write(sentData); }
                    else if (lastKnownCOM == 4) { SP4.Write(sentData); }
                    else if (lastKnownCOM == 5) { SP5.Write(sentData); }
                    else if (lastKnownCOM == 6) { SP6.Write(sentData); }
                }

                /*//avoid sending incomplete data
                if (lastKnownCOM == 1) { SP1.WriteLine(sentData); }
                else if (lastKnownCOM == 2) { SP2.WriteLine(sentData); }
                else if (lastKnownCOM == 3) { SP3.WriteLine(sentData); }
                else if (lastKnownCOM == 4) { SP4.WriteLine(sentData); }
                else if (lastKnownCOM == 5) { SP5.WriteLine(sentData); }*/
            }
            else
            {
                label2.Text = "Last Speed: " + selectedLatency.ToString() + "ms, Single serial input: 2.";
                String sentData = "2.";
                for (int i = 0; i < sentPattern.Length; i++)
                {
                    label2.Text += sentPattern[i].ToString() + ".";
                    sentData += sentPattern[i].ToString() + ".";
                    //Debug.WriteLine("4." + i.ToString() + "." + sentPattern[i].ToString() + ".");
                }
                historyLabel.Text = "None of the Serial Port is open, no input was sent!";
                statusMessagesTextBox.AppendText(Environment.NewLine + "None of the Serial Port is open, no input was sent!");
                //MessageBox.Show("None of the Serial Port is open, no input was sent!");
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
                if (historyUndoStack.Count > 0) { undoButton.Enabled = true; }
                else { undoButton.Enabled = false; }
                redoButton.Enabled = true;
                Debug.WriteLine("(End) Undo Stack Size:" + historyUndoStack.Count.ToString() + " |Redo Stack Size:" + historyRedoStack.Count.ToString());
                statusMessagesTextBox.AppendText(Environment.NewLine + "(Undo) Undo Stack Size:" + historyUndoStack.Count.ToString() + " | Redo Stack Size:" + historyRedoStack.Count.ToString());
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
                if (historyRedoStack.Count > 0) { redoButton.Enabled = true; }
                else { redoButton.Enabled = false; }
                undoButton.Enabled = true;
                Debug.WriteLine("(End) Undo Stack Size:" + historyUndoStack.Count.ToString() + " |Redo Stack Size:" + historyRedoStack.Count.ToString());
                statusMessagesTextBox.AppendText(Environment.NewLine + "(Redo) Undo Stack Size:" + historyUndoStack.Count.ToString() + " | Redo Stack Size:" + historyRedoStack.Count.ToString());
            }
        }

        private void inputTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            CheckSPconnection();
            inputTextBox.Text = inputTextBox.Text.Trim();
            //Check for Enter Key Press first first
            if (e.KeyChar == (char)Keys.Enter)
            {
                int textBoxValue = 0;
                if (inputTextBox.Text == "") { inputTextBox.Text = "0"; } //if field is empty, set value to 0
                try { textBoxValue = Int32.Parse(inputTextBox.Text); }
                catch
                { //if parse fails (e.g. alphabets inputted), assign value to 0
                    textBoxValue = 0;
                    inputTextBox.Text = "0";
                }
                if (textBoxValue < 0) { 
                    textBoxValue = 0; 
                    inputTextBox.Text = "0"; 
                    MessageBox.Show("Input value cannot be lower than 0"); 
                }
                else if (textBoxValue > 768) 
                { 
                    textBoxValue = 768; 
                    inputTextBox.Text = "768"; 
                    MessageBox.Show("Input value cannot be higher than 768"); 
                }
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

        private async void loopStartStopButton_Click(object sender, EventArgs e)
        {
            CheckSPconnection();

            if (selectedPattern < 5 || selectedPattern > 6)
            //this condition is technically will never be fulfilled anyway, but being kept as a failsafe.
            {
                looping = false;
                historyLabel.Text = "No Pattern was selected or the selected Pattern is not a looping Pattern";
            }
            else { looping = !looping; }
            //if there was no selected pattern, or pattern is not a loop
            //without this conditional checking will loop forever,
            //locking the programme.

            while (looping && (selectedPattern == 5 || selectedPattern == 6))
            {
                if (selectedPattern == 5)
                {
                    if (persistentIndex == savedArrayList5.Count)
                    {
                        persistentIndex = 0; //reset index to 0 if index size exceeds array size
                    }
                    updateWorkspaceColor(savedArrayList5[persistentIndex]);
                    savedArrayList5[persistentIndex].CopyTo(workspaceArray, 0);
                    //there is overhead for this copy action, but it is to support the case
                    //where user stops in the middle of the loop and modifying/save input patterns from there
                    sentPatternsThrough(savedArrayList5[persistentIndex]);
                    await PutTaskDelay(selectedLatency);

                    if (!looping) { loopStartStopButton.Text = "Start Loop"; break; }
                }
                else if (selectedPattern == 6)
                {
                    if (persistentIndex == savedArrayList6.Count)
                    {
                        persistentIndex = 0; //reset index to 0 if index size exceeds array size
                    }
                    updateWorkspaceColor(savedArrayList6[persistentIndex]);
                    savedArrayList6[persistentIndex].CopyTo(workspaceArray, 0);
                    //there is overhead for this copy action, but it is to support the case
                    //where user stops in the middle of the loop and modifying/save input patterns from there
                    sentPatternsThrough(savedArrayList6[persistentIndex]);
                    loopStartStopButton.Text = "Stop Loop";
                    await PutTaskDelay(selectedLatency);
                    if (!looping) { loopStartStopButton.Text = "Start Loop"; break; }
                }
                persistentIndex++;
            }
        }

        async Task PutTaskDelay(int delayMS)
        {
            await Task.Delay(delayMS);
        }

        private void loopNextPatternButton_Click(object sender, EventArgs e)
        {
            CheckSPconnection();
            if (selectedPattern == 5)
            {
                if (persistentIndex == savedArrayList5.Count - 1)
                {
                    persistentIndex = 0; //reset index to 0 if index size exceeds array size
                }
                else
                { persistentIndex++; }
                updateWorkspaceColor(savedArrayList5[persistentIndex]);
                savedArrayList5[persistentIndex].CopyTo(workspaceArray, 0);
                statusMessagesTextBox.AppendText(Environment.NewLine + "Show input patterns 5 loaded from #" + (persistentIndex+1).ToString() + " out of " + savedArrayList5.Count);
                //sentPatternsThrough(savedArrayList5[persistentIndex]);
                //currently next pattern button does not sent pattern through, but user can click "Commit"
            }
            else if (selectedPattern == 6)
            {
                if (persistentIndex == savedArrayList6.Count - 1)
                {
                    persistentIndex = 0; //reset index to 0 if index size exceeds array size
                }
                else
                { persistentIndex++; }
                updateWorkspaceColor(savedArrayList6[persistentIndex]);
                savedArrayList6[persistentIndex].CopyTo(workspaceArray, 0);
                statusMessagesTextBox.AppendText(Environment.NewLine + "Show input patterns 6 loaded from #" + (persistentIndex+1).ToString() + " out of " + savedArrayList6.Count);
                //sentPatternsThrough(savedArrayList6[persistentIndex]);
                //currently next pattern button does not sent pattern through, but user can click "Commit"

            }
        }

        private void loopPrevPatternButton_Click(object sender, EventArgs e)
        {
            CheckSPconnection();
            if (selectedPattern == 5)
            {
                if (persistentIndex == 0)
                {
                    persistentIndex = savedArrayList5.Count - 1; //if index size is 0, start at the last index of the relevant array.
                }
                else
                { persistentIndex--; }
                updateWorkspaceColor(savedArrayList5[persistentIndex]);
                savedArrayList5[persistentIndex].CopyTo(workspaceArray, 0);
                statusMessagesTextBox.AppendText(Environment.NewLine + "Show input patterns 5 loaded from #" + (persistentIndex + 1).ToString() + " out of " + savedArrayList5.Count);
                //sentPatternsThrough(savedArrayList5[persistentIndex]);
                //currently next pattern button does not sent pattern through, but user can click "Commit"

            }
            else if (selectedPattern == 6)
            {
                if (persistentIndex == 0)
                {
                    persistentIndex = savedArrayList6.Count - 1; //if index size is 0, start at the last index of the relevant array.
                }
                else
                { persistentIndex--; }

                updateWorkspaceColor(savedArrayList6[persistentIndex]);
                savedArrayList6[persistentIndex].CopyTo(workspaceArray, 0);
                statusMessagesTextBox.AppendText(Environment.NewLine + "Show input patterns 6 loaded from #" + (persistentIndex + 1).ToString() + " out of " + savedArrayList6.Count);
                //sentPatternsThrough(savedArrayList6[persistentIndex]);
                //currently next pattern button does not sent pattern through, but user can click "Commit"

            }
        }

        private void loopLatencyTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            CheckSPconnection();
            loopLatencyTextBox.Text = loopLatencyTextBox.Text.Trim();
            //Check for Enter Key Press first first
            if (e.KeyChar == (char)Keys.Enter)
            {
                int textBoxValue = 50;
                if (loopLatencyTextBox.Text == "") { loopLatencyTextBox.Text = "50"; } //if field is empty, set value to 50
                try { textBoxValue = Int32.Parse(loopLatencyTextBox.Text); }
                catch //if parse fails (e.g. alphabets inputted), assign value to 50
                {
                    textBoxValue = 50;
                    loopLatencyTextBox.Text = "50";
                }
                if (textBoxValue < 50) { 
                    textBoxValue = 50; loopLatencyTextBox.Text = "50"; 
                    MessageBox.Show("Input value cannot be lower than 50"); 
                }
                //else if (textBoxValue > 768) { textBoxValue = 768; MessageBox.Show("Input value cannot be higher than 768"); }
                else
                {
                    selectedLatency = textBoxValue;
                    e.Handled = true; //To suppress the Ding sounds, indicating that there is no error.
                }
            }
            // Only accepts numbers and backspace, no alphabets or "."
            else if (!char.IsDigit(e.KeyChar) && (e.KeyChar != (char)8))
            {
                loopLatencyTextBox.Text = "50";
                selectedLatency = 50;
                MessageBox.Show("Input value can only be numbers");
            }
        }

        private void statusRefreshButton_Click(object sender, EventArgs e)
        {
            CheckSPconnection();
        }

        private void sentXButton_Click(object sender, EventArgs e)
        {
            CheckSPconnection();
            //Sent "x" command to set all speaker's intensity to 512
            if (lastKnownCOM == 1) { SP1.Write("x"); }
            else if (lastKnownCOM == 2) { SP2.Write("x"); }
            else if (lastKnownCOM == 3) { SP3.Write("x"); }
            else if (lastKnownCOM == 4) { SP4.Write("x"); }
            else if (lastKnownCOM == 5) { SP5.Write("x"); }
            else if (lastKnownCOM == 6) { SP6.Write("x"); }

            if (lastKnownCOM != 0) { MessageBox.Show("The action was performed successfully!\nX command was sent through COM" + lastKnownCOM.ToString()); }
            else { MessageBox.Show("COM port connection closed"); }
        }
    }
}
