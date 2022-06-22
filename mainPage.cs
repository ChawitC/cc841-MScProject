using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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
        int[] customArray1 = new int[16] { 0, 15, 10, 15, 20, 125, 30, 35, 40, 45, 150, 55, 60, 165, 70, 75 };
        int[] customArray2 = new int[16] { 100, 105, 110, 15, 120, 125, 130, 105, 140, 145, 150, 155, 160, 105, 170, 175 };
        bool cloneMode = false;
        bool toggleMode = false;

        public mainPage()
        {
            InitializeComponent();
            button1.Click += Button_Click;
            button2.Click += Button_Click;
            button3.Click += Button_Click;
            button4.Click += Button_Click;
            button5.Click += Button_Click;
            button6.Click += Button_Click;
            button7.Click += Button_Click;
            button8.Click += Button_Click;
            button9.Click += Button_Click;
            button10.Click += Button_Click;
            button11.Click += Button_Click;
            button12.Click += Button_Click;
            button13.Click += Button_Click;
            button14.Click += Button_Click;
            button15.Click += Button_Click;
            button16.Click += Button_Click;
            presetButton1.Click += PresetsButton_Click;
            presetButton2.Click += PresetsButton_Click;
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
            /*for (int i = 0; i < savedArray.Length; i++)
            {
                int loadedValue = savedArray[i];
                string loadedButtonName = "button" + i.ToString();
            }*/
            button1.BackColor = ColorFromHSV(savedArray[0]);
            button2.BackColor = ColorFromHSV(savedArray[1]);
            button3.BackColor = ColorFromHSV(savedArray[2]);
            button4.BackColor = ColorFromHSV(savedArray[3]);
            button5.BackColor = ColorFromHSV(savedArray[4]);
            button6.BackColor = ColorFromHSV(savedArray[5]);
            button7.BackColor = ColorFromHSV(savedArray[6]);
            button8.BackColor = ColorFromHSV(savedArray[7]);
            button9.BackColor = ColorFromHSV(savedArray[8]);
            button10.BackColor = ColorFromHSV(savedArray[9]);
            button11.BackColor = ColorFromHSV(savedArray[10]);
            button12.BackColor = ColorFromHSV(savedArray[11]);
            button13.BackColor = ColorFromHSV(savedArray[12]);
            button14.BackColor = ColorFromHSV(savedArray[13]);
            button15.BackColor = ColorFromHSV(savedArray[14]);
            button16.BackColor = ColorFromHSV(savedArray[15]);

            if (toggleMode) // Update intensity values on Button instead of Index
            {
                button1.Text = savedArray[0].ToString();
                button2.Text = savedArray[1].ToString();
                button3.Text = savedArray[2].ToString();
                button4.Text = savedArray[3].ToString();
                button5.Text = savedArray[4].ToString();
                button6.Text = savedArray[5].ToString();
                button7.Text = savedArray[6].ToString();
                button8.Text = savedArray[7].ToString();
                button9.Text = savedArray[8].ToString();
                button10.Text = savedArray[9].ToString();
                button11.Text = savedArray[10].ToString();
                button12.Text = savedArray[11].ToString();
                button13.Text = savedArray[12].ToString();
                button14.Text = savedArray[13].ToString();
                button15.Text = savedArray[14].ToString();
                button16.Text = savedArray[15].ToString();
            }
            
        }

        public void saveWorkspaceToCustom_Click(object sender, EventArgs e)
        {
            if (((Button)sender).Tag.ToString() == "sc1")
            {
                customArray1 = workspaceArray;
            }
            if (((Button)sender).Tag.ToString() == "sc2")
            {
                customArray2 = workspaceArray;
            }
        }

        private void PresetsButton_Click(object sender, EventArgs e)
        {
            if (((Button)sender).Tag.ToString() == "p1")
            {
                workspaceArray = savedArray1;
                loadSavedArray(workspaceArray);
            }
            if (((Button)sender).Tag.ToString() == "p2")
            {
                workspaceArray = savedArray2;
                loadSavedArray(workspaceArray);
            }
            if (((Button)sender).Tag.ToString() == "lc1")
            {
                workspaceArray = customArray1;
                loadSavedArray(workspaceArray);
            }
            if (((Button)sender).Tag.ToString() == "lc2")
            {
                workspaceArray = customArray2;
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
                workspaceArray[arrayIndex] = selectedColor;
                ((Button)sender).BackColor = ColorFromHSV(selectedColor);
                // Update Text on button depending on which display mode is selected
                if (!toggleMode) { ((Button)sender).Text = ((Button)sender).Tag.ToString(); }
                else { ((Button)sender).Text = selectedColor.ToString(); }
            }
            else //Input value from selected button
            {
                previewButton.BackColor = ColorFromHSV(workspaceArray[arrayIndex]);
                previewButton.Text = workspaceArray[arrayIndex].ToString();
                trackBar1.Value = workspaceArray[arrayIndex] / 5; //there are 51 steps in trackbars, in an increment of 5, producing minimum 0 and maximum 255
            }
            // debug
            label2.Text = workspaceArray[1].ToString() + workspaceArray[2].ToString() + workspaceArray[3].ToString() + workspaceArray[4].ToString() + cloneMode.ToString() + toggleMode.ToString();

        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            selectedColor = trackBar1.Value * 5; //there are 51 steps in trackbars, in an increment of 5, producing minimum 0 and maximum 255
            previewButton.BackColor = ColorFromHSV(selectedColor);
            previewButton.Text = selectedColor.ToString();
            //button2.BackColor = ColorFromHSV(selectedColor);

        }
        private void cloneModeCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            cloneMode = cloneModeCheckBox.Checked;
            if (!cloneMode) 
            { 
                previewButton.BackColor = ColorFromHSV(selectedColor);
                previewButton.Text = selectedColor.ToString();
            }
            // debug
            label2.Text = workspaceArray[1].ToString() + workspaceArray[2].ToString() + workspaceArray[3].ToString() + workspaceArray[4].ToString() + cloneMode.ToString() + toggleMode.ToString();
        }
        private void ToggleIndexIntensityCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            toggleMode = toggleModeCheckBox.Checked;
            if (toggleMode)
            {
                button1.Text = workspaceArray[0].ToString();
                button2.Text = workspaceArray[1].ToString();
                button3.Text = workspaceArray[2].ToString();
                button4.Text = workspaceArray[3].ToString();
                button5.Text = workspaceArray[4].ToString();
                button6.Text = workspaceArray[5].ToString();
                button7.Text = workspaceArray[6].ToString();
                button8.Text = workspaceArray[7].ToString();
                button9.Text = workspaceArray[8].ToString();
                button10.Text = workspaceArray[9].ToString();
                button11.Text = workspaceArray[10].ToString();
                button12.Text = workspaceArray[11].ToString();
                button13.Text = workspaceArray[12].ToString();
                button14.Text = workspaceArray[13].ToString();
                button15.Text = workspaceArray[14].ToString();
                button16.Text = workspaceArray[15].ToString();
            }
            else
            {
                button1.Text = "1";
                button2.Text = "2";
                button3.Text = "3";
                button4.Text = "4";
                button5.Text = "5";
                button6.Text = "6";
                button7.Text = "7";
                button8.Text = "8";
                button9.Text = "9";
                button10.Text = "10";
                button11.Text = "11";
                button12.Text = "12";
                button13.Text = "13";
                button14.Text = "14";
                button15.Text = "15";
                button16.Text = "16";
            }
            // debug
            label2.Text = workspaceArray[1].ToString() + workspaceArray[2].ToString() + workspaceArray[3].ToString() + workspaceArray[4].ToString() + cloneMode.ToString() + toggleMode.ToString();

        }
    }
}
