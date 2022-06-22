using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Diagnostics;
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
        int[] savedArray1 = new int[16] {0,5,10,15,20,25,30,35,40,45,50,55,60,65,70,75} ;
        int[] savedArray2 = new int[16] {100, 105, 110, 115, 120, 125, 130, 135, 140, 145, 150, 155, 160, 165, 170, 175 };
        int[] savedArray3 = new int[16] { 5, 10, 15, 20, 25, 30, 35, 40, 140, 145, 150, 155, 160, 165, 170, 175 };
        int[] customArray1 = new int[16] { 0, 15, 10, 15, 20, 125, 30, 35, 40, 45, 150, 55, 60, 165, 70, 75 };
        int[] customArray2 = new int[16] { 100, 105, 110, 15, 120, 125, 130, 105, 140, 145, 150, 155, 160, 105, 170, 175 };
        bool cloneMode = false;
        bool toggleMode = false;
        List<Button> buttonsList = new List<Button>();
        Stack<int[]> historyIntensityStack = new Stack<int[]>();

        public mainPage()
        {
            InitializeComponent();

            buttonsList = new List<Button>{
            button1, button2, button3, button4, button5, button6, button7, button8, button9, button10,
            button11, button12, button13, button14, button15, button16//, button17, button18, button19,
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
            loadSavedArray(workspaceArray);
        }

        public static Color ColorFromHSV(double hue)
        {
            // .NET frame work does not have built-in RGB > HSV or HSV > Color conversion
            //adapted from https://stackoverflow.com/questions/1335426/is-there-a-built-in-c-net-system-api-for-hsv-to-rgb
            //in our case there is no change in Saturation or Value
            int saturation = 1; 
            int value = 1;
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

        public void loadSavedArray (int[] savedArray)
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
                workspaceArray.CopyTo(customArray1, 0);
            }
            if (((Button)sender).Tag.ToString() == "sc2")
            {
                workspaceArray.CopyTo(customArray2, 0);
            }
        }

        private void PresetsButton_Click(object sender, EventArgs e)
        {
            if (((Button)sender).Tag.ToString() == "p1")
            {
                savedArray1.CopyTo(workspaceArray,0);
                loadSavedArray(workspaceArray);
            }
            else if (((Button)sender).Tag.ToString() == "p2")
            {
                savedArray2.CopyTo(workspaceArray, 0);
                loadSavedArray(workspaceArray);
            }
            else if (((Button)sender).Tag.ToString() == "p3")
            {
                savedArray3.CopyTo(workspaceArray, 0);
                loadSavedArray(workspaceArray);
            }
            else if (((Button)sender).Tag.ToString() == "lc1")
            {
                customArray1.CopyTo(workspaceArray, 0);
                loadSavedArray(workspaceArray);
            }
            else if (((Button)sender).Tag.ToString() == "lc2")
            {
                customArray2.CopyTo(workspaceArray, 0);
                loadSavedArray(workspaceArray);
            }
        }
        private void Button_Click(object sender, EventArgs e)
        {
            string buttonText = ((Button)sender).Tag.ToString();
            int arrayIndex = Int32.Parse(buttonText);
            arrayIndex -= 1; //step down since an array's index starts from 0

            if (!cloneMode) // Input value from trackBar
            {
                //write to History Stack
                int[] pushArray = new int[16];
                workspaceArray.CopyTo(pushArray, 0);
                historyIntensityStack.Push(pushArray);

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
            if (historyIntensityStack.Count > 0)
            {
                Debug.WriteLine("Stack Size:" + historyIntensityStack.Count.ToString());
                historyIntensityStack.Pop().CopyTo(workspaceArray, 0);
                loadSavedArray(workspaceArray);
            }
        }
    }
}
