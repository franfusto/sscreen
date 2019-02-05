

/// <summary>
/// File: SplashScreen.cs
/// Author: Francisco Fuster e-mail: franfusto@gmail.com
/// Copyright 2016, Francisco Fuster.
/// Based on code of Tom Clement
/// http://www.codeproject.com/Articles/5454/A-Pretty-Good-Splash-Screen-in-C
/// 
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using Microsoft.Win32;
using System.Runtime.InteropServices;

namespace horizonte.sscreen
{

	public class SplashScreen : Form
	{

		private System.Windows.Forms.Label lblStatus;
		private System.Windows.Forms.Label lblTimeRemaining;
		private System.Windows.Forms.Timer UpdateTimer;
		private System.Windows.Forms.Panel pnlStatus;
		private System.ComponentModel.IContainer components = null;
		#region test

		//public void ImageToBase64(Image image,System.Drawing.Imaging.ImageFormat format)
		public static void ImageToBase64()
		{
			Image image = System.Drawing.Image.FromFile ("Splash.JPG");

			using (MemoryStream ms = new MemoryStream())
			{
				// Convert Image to byte[]
				image.Save(ms,System.Drawing.Imaging.ImageFormat.Jpeg);
				byte[] imageBytes = ms.ToArray();

				// Convert byte[] to Base64 String
				string base64String = Convert.ToBase64String(imageBytes);

				System.IO.File.WriteAllText ("Splash.txt", base64String);
				//return base64String;
			}
		}

		public static void SetImageFile(string filename)
		{
			ImageFile = filename;
		}
		private Image Bgimg()
		{
			string base64String = @"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";
			if (ImageFile != string.Empty) {
				if (System.IO.File.Exists(ImageFile))
				{
					return System.Drawing.Image.FromFile (ImageFile);
				}
			}
			// Convert Base64 String to byte[]
			byte[] imageBytes = Convert.FromBase64String(base64String);
			MemoryStream ms = new MemoryStream(imageBytes, 0, 
				imageBytes.Length);
			// Convert byte[] to Image
			ms.Write(imageBytes, 0, imageBytes.Length);
			Image image = Image.FromStream(ms, true);
			return image;
		}

		#endregion
		public static string ImageFile = string.Empty;

		#region Member Variables
		// Threading
		private static SplashScreen ms_frmSplash = null;
		private static Thread ms_oThread = null;

		// Fade in and out.
		private double m_dblOpacityIncrement = .10;
		private double m_dblOpacityDecrement = .10;
		private const int TIMER_INTERVAL = 1;

		// Status and progress bar
		private string m_sStatus;
		private string m_sTimeRemaining;
		private double m_dblCompletionFraction = 0.0;
		private Rectangle m_rProgress;

		// Progress smoothing
		private double m_dblLastCompletionFraction = 0.0;
		private double m_dblPBIncrementPerTimerInterval = .015;

		// Self-calibration support
		private int m_iIndex = 1;
		private int m_iActualTicks = 0;
		private ArrayList m_alPreviousCompletionFraction;
		private ArrayList m_alActualTimes = new ArrayList();
		private DateTime m_dtStart;
		private bool m_bFirstLaunch = false;
		private bool m_bDTSet = false;

		#endregion Member Variables
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			//System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SplashScreen));
			this.lblStatus = new System.Windows.Forms.Label();
			this.pnlStatus = new System.Windows.Forms.Panel();
			this.lblTimeRemaining = new System.Windows.Forms.Label();
			this.UpdateTimer = new System.Windows.Forms.Timer(this.components);
			this.SuspendLayout();
			// 
			// lblStatus
			// 
			this.lblStatus.BackColor = System.Drawing.Color.Transparent;
			this.lblStatus.ForeColor = Color.DarkBlue;
			this.lblStatus.Location = new System.Drawing.Point(0, 116);
			this.lblStatus.TextAlign = ContentAlignment.TopCenter;
			this.lblStatus.Name = "lblStatus";
			this.lblStatus.Size = new System.Drawing.Size(421, 14);
			this.lblStatus.TabIndex = 0;
			this.lblStatus.DoubleClick += new System.EventHandler(this.SplashScreen_DoubleClick);
			// 
			// pnlStatus
			// 
			this.pnlStatus.BackColor = System.Drawing.Color.Transparent;

			this.pnlStatus.Location = new System.Drawing.Point(0, 200);
			this.pnlStatus.Name = "pnlStatus";
			this.pnlStatus.Size = new System.Drawing.Size(421, 18);
			this.pnlStatus.TabIndex = 1;
			this.pnlStatus.DoubleClick += new System.EventHandler(this.SplashScreen_DoubleClick);
			// 
			// lblTimeRemaining
			// 
			this.lblTimeRemaining.ForeColor = Color.DarkBlue;
			this.lblTimeRemaining.BackColor = System.Drawing.Color.Transparent;
			this.lblTimeRemaining.TextAlign = ContentAlignment.TopLeft;
			this.lblTimeRemaining.Location = new System.Drawing.Point(21, 180);
			this.lblTimeRemaining.Name = "lblTimeRemaining";
			this.lblTimeRemaining.Size = new System.Drawing.Size(400, 16);
			this.lblTimeRemaining.TabIndex = 2;
			this.lblTimeRemaining.Text = "Tiempo restante";
			this.lblTimeRemaining.DoubleClick += new System.EventHandler(this.SplashScreen_DoubleClick);
			// 
			// UpdateTimer
			// 
			this.UpdateTimer.Tick += new System.EventHandler(this.UpdateTimer_Tick);
			// 
			// SplashScreen
			// 

			//this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.BackColor = System.Drawing.Color.LightGray;
			//this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
			//this.BackgroundImage = System.Drawing.Image.FromFile("Splash.JPG");
			this.BackgroundImage = this.Bgimg();

			this.Size =  new System.Drawing.Size(421, 221);
			this.ClientSize = new System.Drawing.Size(421, 221);
			this.Controls.Add(this.lblTimeRemaining);
			this.Controls.Add(this.pnlStatus);
			this.Controls.Add(this.lblStatus);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Name = "SplashScreen";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "SplashScreen";
			this.DoubleClick += new System.EventHandler(this.SplashScreen_DoubleClick);
			this.ResumeLayout(false);

		}
		#endregion

		public SplashScreen()
		{
			InitializeComponent();
			this.Opacity = 0.0;
			UpdateTimer.Interval = TIMER_INTERVAL;
			UpdateTimer.Start();
			this.ClientSize = this.BackgroundImage.Size;
		}

		#region Public Static Methods
		// A static method to create the thread and 
		// launch the SplashScreen.
		static public void ShowSplashScreen()
		{
			// Make sure it's only launched once.
			if (ms_frmSplash != null)
				return;
			ms_oThread = new Thread(new ThreadStart(SplashScreen.ShowForm));
			ms_oThread.IsBackground = true;
			ms_oThread.SetApartmentState(ApartmentState.STA);
			ms_oThread.Start();
			while (ms_frmSplash == null || ms_frmSplash.IsHandleCreated == false)
			{
				System.Threading.Thread.Sleep(TIMER_INTERVAL);
			}
		}

		// Close the form without setting the parent.
		static public void CloseForm()
		{
			if (ms_frmSplash != null && ms_frmSplash.IsDisposed == false)
			{
				// Make it start going away.
				ms_frmSplash.m_dblOpacityIncrement = -ms_frmSplash.m_dblOpacityDecrement;
			}
			ms_oThread = null;	// we don't need these any more.
			ms_frmSplash = null;
		}

		// A static method to set the status and update the reference.
		static public void SetStatus(string newStatus)
		{
			SetStatus(newStatus, true);
		}

		static public void Mostrar()
		{
			ms_frmSplash.Show ();
		}
		static public void Ocultar()
		{
			ms_frmSplash.Hide ();
		}


		// A static method to set the status and optionally update the reference.
		// This is useful if you are in a section of code that has a variable
		// set of status string updates.  In that case, don't set the reference.
		static public void SetStatus(string newStatus, bool setReference)
		{
			if (ms_frmSplash == null)
				return;

			ms_frmSplash.m_sStatus = newStatus;

			if (setReference)
				ms_frmSplash.SetReferenceInternal();
		}

		// Static method called from the initializing application to 
		// give the splash screen reference points.  Not needed if
		// you are using a lot of status strings.
		static public void SetReferencePoint()
		{
			if (ms_frmSplash == null)
				return;
			ms_frmSplash.SetReferenceInternal();

		}
		#endregion Public Static Methods

		#region Private Methods

		// A private entry point for the thread.
		static private void ShowForm()
		{
			ms_frmSplash = new SplashScreen();
			Application.Run(ms_frmSplash);
		}

		// Internal method for setting reference points.
		private void SetReferenceInternal()
		{
			if (m_bDTSet == false)
			{
				m_bDTSet = true;
				m_dtStart = DateTime.Now;
				ReadIncrements();
			}
			double dblMilliseconds = ElapsedMilliSeconds();
			m_alActualTimes.Add(dblMilliseconds);
			m_dblLastCompletionFraction = m_dblCompletionFraction;
			if (m_alPreviousCompletionFraction != null && m_iIndex < m_alPreviousCompletionFraction.Count)
				m_dblCompletionFraction = (double)m_alPreviousCompletionFraction[m_iIndex++];
			else
				m_dblCompletionFraction = (m_iIndex > 0) ? 1 : 0;
		}

		// Utility function to return elapsed Milliseconds since the 
		// SplashScreen was launched.
		private double ElapsedMilliSeconds()
		{
			TimeSpan ts = DateTime.Now - m_dtStart;
			return ts.TotalMilliseconds;
		}

		// Function to read the checkpoint intervals from the previous invocation of the
		// splashscreen from the XML file.
		private void ReadIncrements()
		{
			string sPBIncrementPerTimerInterval = SplashScreenXMLStorage.Interval;
			double dblResult;

			if (Double.TryParse(sPBIncrementPerTimerInterval, System.Globalization.NumberStyles.Float, System.Globalization.NumberFormatInfo.InvariantInfo, out dblResult) == true)
				m_dblPBIncrementPerTimerInterval = dblResult;
			else
				m_dblPBIncrementPerTimerInterval = .0015;

			string sPBPreviousPctComplete = SplashScreenXMLStorage.Percents;

			if (sPBPreviousPctComplete != "")
			{
				string[] aTimes = sPBPreviousPctComplete.Split(null);
				m_alPreviousCompletionFraction = new ArrayList();

				for (int i = 0; i < aTimes.Length; i++)
				{
					double dblVal;
					if (Double.TryParse(aTimes[i], System.Globalization.NumberStyles.Float, System.Globalization.NumberFormatInfo.InvariantInfo, out dblVal) == true)
						m_alPreviousCompletionFraction.Add(dblVal);
					else
						m_alPreviousCompletionFraction.Add(1.0);
				}
			}
			else
			{
				m_bFirstLaunch = true;
				m_sTimeRemaining = "";
			}
		}

		// Method to store the intervals (in percent complete) from the current invocation of
		// the splash screen to XML storage.
		private void StoreIncrements()
		{
			string sPercent = "";
			double dblElapsedMilliseconds = ElapsedMilliSeconds();
			for (int i = 0; i < m_alActualTimes.Count; i++)
				sPercent += ((double)m_alActualTimes[i] / dblElapsedMilliseconds).ToString("0.####", System.Globalization.NumberFormatInfo.InvariantInfo) + " ";

			SplashScreenXMLStorage.Percents = sPercent;

			m_dblPBIncrementPerTimerInterval = 1.0 / (double)m_iActualTicks;

			SplashScreenXMLStorage.Interval = m_dblPBIncrementPerTimerInterval.ToString("#.000000", System.Globalization.NumberFormatInfo.InvariantInfo);
		}

		public static SplashScreen GetSplashScreen()
		{
			return ms_frmSplash;
		}

		#endregion Private Methods

		#region Event Handlers
		// Tick Event handler for the Timer control.  Handle fade in and fade out and paint progress bar. 
		private void UpdateTimer_Tick(object sender, System.EventArgs e)
		{
			lblStatus.Text = m_sStatus;

			// Calculate opacity
			if (m_dblOpacityIncrement > 0)		// Starting up splash screen
			{
				m_iActualTicks++;
				if (this.Opacity < 1)
					this.Opacity += m_dblOpacityIncrement;
			}
			else // Closing down splash screen
			{
				if (this.Opacity > 0)
					this.Opacity += m_dblOpacityIncrement;
				else
				{
					StoreIncrements();
					UpdateTimer.Stop();
					this.Close();
				}
			}

			// Paint progress bar
			if (m_bFirstLaunch == false && m_dblLastCompletionFraction < m_dblCompletionFraction)
			{
				m_dblLastCompletionFraction += m_dblPBIncrementPerTimerInterval;
				int width = (int)Math.Floor(pnlStatus.ClientRectangle.Width * m_dblLastCompletionFraction);
				int height = pnlStatus.ClientRectangle.Height;
				int x = pnlStatus.ClientRectangle.X;
				int y = pnlStatus.ClientRectangle.Y;
				if (width > 0 && height > 0)
				{
					m_rProgress = new Rectangle(x, y, width, height);
					if (!pnlStatus.IsDisposed)
					{
						Graphics g = pnlStatus.CreateGraphics();
						LinearGradientBrush brBackground = new LinearGradientBrush(m_rProgress, Color.FromArgb(58, 96, 151), Color.FromArgb(181, 237, 254), LinearGradientMode.Horizontal);
						g.FillRectangle(brBackground, m_rProgress);
						g.Dispose();
					}
					int iSecondsLeft = 1 + (int)(TIMER_INTERVAL * ((1.0 - m_dblLastCompletionFraction) / m_dblPBIncrementPerTimerInterval)) / 1000;
					m_sTimeRemaining = (iSecondsLeft == 1) ? string.Format("1 seg.") : string.Format("{0} seconds remaining", iSecondsLeft);
				}
			}
			//lblTimeRemaining.Text = m_sTimeRemaining;
			lblTimeRemaining.Text ="";
		}

		// Close the form if they double click on it.
		private void SplashScreen_DoubleClick(object sender, System.EventArgs e)
		{
			// Use the overload that doesn't set the parent form to this very window.
			CloseForm();
		}
		#endregion Event Handlers
	}

	#region Auxiliary Classes 
	/// <summary>
	/// A specialized class for managing XML storage for the splash screen.
	/// </summary>
	internal class SplashScreenXMLStorage
	{
		private static string ms_StoredValues = "SplashScreen.xml";
		private static string ms_DefaultPercents = "";
		private static string ms_DefaultIncrement = ".015";


		// Get or set the string storing the percentage complete at each checkpoint.
		static public string Percents
		{
			get { return GetValue("Percents", ms_DefaultPercents); }
			set { SetValue("Percents", value); }
		}
		// Get or set how much time passes between updates.
		static public string Interval
		{
			get { return GetValue("Interval", ms_DefaultIncrement); }
			set { SetValue("Interval", value); }
		}

		// Store the file in a location where it can be written with only User rights. (Don't use install directory).
		static private string StoragePath
		{
			get {return Path.Combine(Application.UserAppDataPath, ms_StoredValues);}
		}

		// Helper method for getting inner text of named element.
		static private string GetValue(string name, string defaultValue)
		{
			if (!File.Exists(StoragePath))
				return defaultValue;

			try
			{
				XmlDocument docXML = new XmlDocument();
				docXML.Load(StoragePath);
				XmlElement elValue = docXML.DocumentElement.SelectSingleNode(name) as XmlElement;
				return (elValue == null) ? defaultValue : elValue.InnerText;
			}
			catch
			{
				return defaultValue;
			}
		}

		// Helper method for setting inner text of named element.  Creates document if it doesn't exist.
		static public void SetValue(string name,
			string stringValue)
		{
			XmlDocument docXML = new XmlDocument();
			XmlElement elRoot = null;
			if (!File.Exists(StoragePath))
			{
				elRoot = docXML.CreateElement("root");
				docXML.AppendChild(elRoot);
			}
			else
			{
				docXML.Load(StoragePath);
				elRoot = docXML.DocumentElement;
			}
			XmlElement value = docXML.DocumentElement.SelectSingleNode(name) as XmlElement;
			if (value == null)
			{
				value = docXML.CreateElement(name);
				elRoot.AppendChild(value);
			}
			value.InnerText = stringValue;
			docXML.Save(StoragePath);
		}
	}
	#endregion Auxiliary Classes
}


