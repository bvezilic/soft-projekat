﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using AForge.Video;
using AForge.Video.DirectShow;

namespace PokerBot
{
    public partial class MainForm : Form
    {
        private FilterInfoCollection VideoCaptureDevices;
        private VideoCaptureDevice FinalVideo;

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            VideoCaptureDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);

            foreach (FilterInfo VideoCaptureDevice in VideoCaptureDevices)
            {
                cbCameras.Items.Add(VideoCaptureDevice.Name);
            }

            FinalVideo = new VideoCaptureDevice();    
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            try
            {
                FinalVideo = new VideoCaptureDevice(VideoCaptureDevices[cbCameras.SelectedIndex].MonikerString);
                FinalVideo.NewFrame += new NewFrameEventHandler(FinalVideo_NewFrame);
                FinalVideo.Start();
                connectControls();
            }
            catch (ArgumentOutOfRangeException r)
            {
                MessageBox.Show("Izaberite kameru!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FinalVideo_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            Bitmap video = (Bitmap)eventArgs.Frame.Clone();
            pbCamera.Image = video;
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            FinalVideo.SignalToStop();
            FinalVideo.WaitForStop();
        }

        private void btnCapture_Click(object sender, EventArgs e)
        {
            pbPicture.Image = (Image)pbCamera.Image.Clone();
        }

        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            FinalVideo.SignalToStop();
            disconnectControls();
        }

        private void connectControls()
        {
            btnConnect.Enabled = false;
            cbCameras.Enabled = false;

            btnDisconnect.Enabled = true;
            btnCapture.Enabled = true;
        }

        private void disconnectControls()
        {
            btnConnect.Enabled = true;
            cbCameras.Enabled = true;

            btnDisconnect.Enabled = false;
            btnCapture.Enabled = false;
        }
    }
}
