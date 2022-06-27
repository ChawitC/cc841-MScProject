using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace cc841.MScProject
{
    public partial class mainPage : Form
    {
        int selectedColor = 0;
        int[] workspaceArray = new int[16];
        // presets are loaded into programs and should not be changeable
        int[] savedArray1 = new int[16] {0,5,10,15,20,25,30,35,40,45,50,55,60,65,70,75} ;
        int[] savedArray2 = new int[16] {100, 105, 110, 115, 120, 125, 130, 135, 140, 145, 150, 155, 160, 165, 170, 175 };
        int[] savedArray3 = new int[16] { 5, 10, 15, 20, 25, 30, 35, 40, 140, 145, 150, 155, 160, 165, 170, 175 };
        // custom input patterns are to be read from/written to file which is in format of "custom(n).txt"
        //int[] customArray1 = new int[16] { 0, 15, 10, 15, 20, 125, 30, 35, 40, 45, 150, 55, 60, 165, 70, 75 };
        //int[] customArray2 = new int[16] { 100, 105, 110, 15, 120, 125, 130, 105, 140, 145, 150, 155, 160, 105, 170, 175 };
        bool cloneMode = false;
        bool toggleMode = false;
        List<Button> buttonsList = new List<Button>();
        Stack<int[]> historyUndoStack = new Stack<int[]>();
        Stack<int[]> historyRedoStack = new Stack<int[]>();

        public mainPage()
        {
            InitializeComponent();

            buttonsList = new List<Button>{
            button1, button2, button3, button4, button5, button6, button7, button8, button9, button10,
            button11, button12, button13, button14, button15, button16//, button17, button18, button19,
            //button21, button22, button23, button24, button25, button26, button27, button28, button29,
            //button31, button32, button33, button34, button35, button36, button37, button38, button39,
            //button41, button42, button43, button44, button45, button46, button47, button48, button49,
            //button51, button52, button53, button54, button55, button56, button57, button58, button59,
            //button61, button62, button63
            };

            for (int i = 0; i < buttonsList.Count; i++)
            {
                buttonsList[i].Click += Button_Click; // Attach all buttons in the list with button click method.
            }

            presetButton1.Click += PresetsButton_Click;
            presetButton2.Click += PresetsButton_Click;
            presetButton3.Click += PresetsButton_Click;
            loadCustomButton1.Click += PresetsButton_Click;
            loadCustomButton2.Click += PresetsButton_Click;

            saveCustomButton1.Click += saveWorkspaceToCustom_Click;
            saveCustomButton2.Click += saveWorkspaceToCustom_Click;

            // Initializing
            updateWorkspaceColor(workspaceArray);
            int[] pushArray = new int[16];
            workspaceArray.CopyTo(pushArray, 0);
            //historyUndoStack.Push(pushArray);
            Debug.WriteLine("(Init) Undo Stack Size:" + historyUndoStack.Count.ToString() + " |Redo Stack Size:" + historyRedoStack.Count.ToString());

        }

        public static Color ColorFromHSV(double hue)
        {
            // .NET frame work does not have built-in RGB > HSV or HSV > Color conversion
            //adapted from https://stackoverflow.com/questions/1335426/is-there-a-built-in-c-net-system-api-for-hsv-to-rgb
            //in our case there is no change in Saturation or Value
            int saturation = 1; 
            int value = 1;
            if (hue > 255) { hue = 255; } //a fail save to set hue to not exceed 255 since the programme is design to work with 0-255
            hue = 255 - hue; //inverting value so that 255 is represeting red, and 0 representing blue
            //original Hue spectrum runs from 0 to 360, but we decided to use only 0-255 since it is representatively sufficient.
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
                if (toggleMode) //if Toggle Intensity is on, load saved intensity number on the button's text.
                {
                    buttonsList[i].Text = savedArray[i].ToString();
                }
            }
           
        }

        public void saveWorkspaceToCustom_Click(object sender, EventArgs e)
        {
            if (((Button)sender).Tag.ToString() == "sc1")
            {
                //workspaceArray.CopyTo(customArray1, 0);
                writeArrayToFile("custom1.txt");
            }
            if (((Button)sender).Tag.ToString() == "sc2")
            {
                //workspaceArray.CopyTo(customArray2, 0);
                writeArrayToFile("custom2.txt");
            }
        }

        public void readArrayFromFile(string filename)
        {
            // Code adapted from https://social.msdn.microsoft.com/Forums/vstudio/en-US/3ea018ab-ffb0-427a-992a-4e78efcbe1f7/read-text-file-insert-data-into-array?forum=csharpgeneral
            // Number is read from save file, one line at a time, put into list and parsed to int which is then saved to workspace.
            List<int> temporarylist = new List<int>();
            int counter = 0;
            string[] stringArray = System.IO.File.ReadAllLines(@"C:\Users\ASUS\source\repos\cc841-MScProject\"+filename);
            foreach (string readNumber in stringArray)
            {
                temporarylist.Add(Convert.ToInt32(readNumber.Trim()));
                workspaceArray[counter] = Convert.ToInt32(readNumber.Trim());
                ++counter;
                Debug.WriteLine("Value of " + readNumber + " is recorded to workspaceArray[" + counter.ToString() + "]");
            }
        }

        public void writeArrayToFile(string filename)
        {
            // Code adapted from https://www.c-sharpcorner.com/article/c-sharp-write-to-file/
            // Convert workspace array (int) into array of strings  
            string[] writtenNumbers = workspaceArray.Select(x => x.ToString()).ToArray();
            // Write array of strings to a file using WriteAllLines.  
            // If the file does not exists, it will create a new file.  
            // This method automatically opens the file, writes to it, and closes file  
            File.WriteAllLines(@"C:\Users\ASUS\source\repos\cc841-MScProject\" + filename, writtenNumbers);
            // Debug : Read the file  
            string readText = File.ReadAllText(@"C:\Users\ASUS\source\repos\cc841-MScProject\" + filename);
            Debug.WriteLine("The text file " + filename + " is now as follows:\n" + readText);
        }

        private void PresetsButton_Click(object sender, EventArgs e)
        {
            //currently it is possible to Undo to Before clicking the presets
            //Redo to newly selected presets are also possible
            int[] pushUndoArray = new int[16];
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
            else if (((Button)sender).Tag.ToString() == "lc1")
            {
                //customArray1.CopyTo(workspaceArray, 0);
                readArrayFromFile("custom1.txt");
                updateWorkspaceColor(workspaceArray);
            }
            else if (((Button)sender).Tag.ToString() == "lc2")
            {
                //customArray2.CopyTo(workspaceArray, 0);
                readArrayFromFile("custom2.txt");
                updateWorkspaceColor(workspaceArray);
            }
        }

        private void Button_Click(object sender, EventArgs e)
        {
            string buttonText = ((Button)sender).Tag.ToString();
            int arrayIndex = Int32.Parse(buttonText);
            arrayIndex -= 1; //step down since an array's index starts from 0

            if (workspaceArray[arrayIndex] != selectedColor) 
                //if the button's current color is the same as selected color, don't perform any action.
            {
                if (!cloneMode) // Input value from trackBar
                {

                    //write to History Stack
                    int[] pushUndoArray = new int[16];
                    workspaceArray.CopyTo(pushUndoArray, 0);
                    historyUndoStack.Push(pushUndoArray);
                    undoButton.BackColor = SystemColors.Control;
                    historyRedoStack.Clear(); //Redo stack cleared since previous branch is disregarded
                    redoButton.BackColor = SystemColors.ControlDark;
                    Debug.WriteLine("(New) Undo Stack Size:" + historyUndoStack.Count.ToString() + " |Redo Stack Size:" + historyRedoStack.Count.ToString());


                    workspaceArray[arrayIndex] = selectedColor;
                    ((Button)sender).BackColor = ColorFromHSV(selectedColor);

                    // Update Text on button depending on which display mode is selected
                    if (!toggleMode) { ((Button)sender).Text = ((Button)sender).Tag.ToString(); }
                    else { ((Button)sender).Text = selectedColor.ToString(); }


                }
                else //Clone Input value from selected button
                {
                    previewButton.BackColor = ColorFromHSV(workspaceArray[arrayIndex]);
                    previewButton.Text = workspaceArray[arrayIndex].ToString();
                    intensitySelectTrackBar.Value = workspaceArray[arrayIndex] / 5; //there are 51 steps in trackbars, in an increment of 5, producing minimum 0 and maximum 255
                }
            }

        }

        private void intensitySelect_Scroll(object sender, EventArgs e)
        {
            selectedColor = intensitySelectTrackBar.Value * 5; //there are 51 steps in trackbars, in an increment of 5, producing minimum 0 and maximum 255
            previewButton.BackColor = ColorFromHSV(selectedColor);
            previewButton.Text = selectedColor.ToString();
        }
        private void cloneModeCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            cloneMode = cloneModeCheckBox.Checked;
            if (!cloneMode) 
            { 
                previewButton.BackColor = ColorFromHSV(selectedColor);
                previewButton.Text = selectedColor.ToString();
            }
        }
        private void ToggleIndexIntensityCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            toggleMode = toggleModeCheckBox.Checked;

            for (int i = 0; i < workspaceArray.Length; i++)
            {
                
                if (toggleMode) { buttonsList[i].Text = workspaceArray[i].ToString(); }
                else { buttonsList[i].Text = i.ToString(); }
            }
        }
        private void commitButton_Click(object sender, EventArgs e)
        {
            label2.Text = "Inputs sent:";
            for (int i = 0; i < workspaceArray.Length; i++)
            {
                Debug.WriteLine(".1."+i.ToString()+"."+workspaceArray[i].ToString());
                label2.Text += " " + workspaceArray[i].ToString();
            }
        }
        private void undoButton_Click(object sender, EventArgs e)
        {
            if (historyUndoStack.Count > 0)
            {
                Debug.WriteLine("(Begin) Undo Stack Size:" + historyUndoStack.Count.ToString() + " |Redo Stack Size:" + historyRedoStack.Count.ToString());
                int[] pushArrayUndo = new int[16];
                int[] pushArrayRedo = new int[16];
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
            if (historyRedoStack.Count > 0)
            {
                Debug.WriteLine("(Begin) Undo Stack Size:" + historyUndoStack.Count.ToString() + " |Redo Stack Size:" + historyRedoStack.Count.ToString());
                int[] pushArrayUndo = new int[16];
                int[] pushArrayRedo = new int[16];
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
    }
}
