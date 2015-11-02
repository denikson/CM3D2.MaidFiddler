﻿using System;
using System.ComponentModel;
using System.Media;
using System.Windows.Forms;
using CM3D2.MaidFiddler.Plugin.Utils;

namespace CM3D2.MaidFiddler.Plugin.Gui
{
    public partial class TextDialog : Form
    {
        private readonly Func<string, bool> checker;
        private readonly string defaultVal;

        public TextDialog(string title,
                          string dialog,
                          string defaultVal,
                          Func<string, bool> checker,
                          string okText = "OK",
                          string cancelText = "CANCEL")
        {
            InitializeComponent();
            AutoValidate = AutoValidate.EnablePreventFocusChange;

            Text = title;
            label_input.Text = dialog;

            button_ok.Text = okText;
            button_cancel.Text = cancelText;

            this.checker = checker;
            this.defaultVal = defaultVal;
            textBox_input.Text = defaultVal;
            textBox_input.Validating += OnValidate;
            VisibleChanged += OnVisibleChanged;
            FormClosing += OnFormClosing;
        }

        public string Input => textBox_input.Text;

        private void button_cancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void button_ok_Click(object sender, EventArgs e)
        {
            if (!ValidateChildren())
                return;
            DialogResult = DialogResult.OK;
            Close();
        }

        private void OnFormClosing(object sender, FormClosingEventArgs e)
        {
            Debugger.WriteLine(LogLevel.Info, $"Closing prompt because: {EnumHelper.GetName(e.CloseReason)}");
            if (DialogResult != DialogResult.OK)
                DialogResult = DialogResult.Cancel;
            e.Cancel = false;
        }

        private void OnValidate(object sender, CancelEventArgs e)
        {
            if (!checker(textBox_input.Text))
            {
                e.Cancel = true;
                textBox_input.Text = defaultVal;
                textBox_input.Select(0, defaultVal.Length);
                SystemSounds.Hand.Play();
            }
        }

        private void OnVisibleChanged(object sender, EventArgs e)
        {
            if (Visible)
                textBox_input.Focus();
        }
    }
}