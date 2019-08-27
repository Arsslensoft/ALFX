using System;
using System.Collections.Generic;
using System.Text;
using AForge.Video.DirectShow;
using AForge.Video;
using System.Drawing;
using AForge.Imaging.Filters;
using System.Threading;
using AForge.Video.FFMPEG;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace Al.AI.Vision
{
    /// <summary>
    /// Video surveillance class
    /// </summary>
    public class VideoSurveillance
    {
        public FilterInfoCollection cameras {get;set;} //Collection of Cameras that connected to PC
        public VideoCaptureDevice device { get; set; } //Current chosen device(camera) 
        public VideoCaptureDevice survdevice { get; set; } //Current chosen device(camera) 
        public VideoFileWriter writer { get; set; }
        public event NewFrameEventHandler OnFrameArrived;
        public string SurveillanceSystem { get; set; }
        public MotionDetection Motion { get; set; }
        int fpsrate = 0;
        public int FrameRate { get { return fpsrate; } }
        public bool MotionRecording { get; set; }
        public VideoSurveillance(string videofile, int width, int height, int bitrate, int fps)
        {
            try
            {
                writer = new VideoFileWriter();
                writer.Open(videofile, width,height,fps, VideoCodec.MPEG4,bitrate);
                cameras = new FilterInfoCollection(AForge.Video.DirectShow.FilterCategory.VideoInputDevice);
                fpsrate = fps;
                SurveillanceSystem = "Arsslensoft Surveillance System";
                MotionRecording = false;
            }
            catch
            {

            }
        }
        public VideoSurveillance(string videofile, int width, int height, int bitrate, int fps, string systemname)
        {
            try
            {
                writer = new VideoFileWriter();
                writer.Open(videofile, width, height, fps, VideoCodec.MPEG4, bitrate);
                cameras = new FilterInfoCollection(AForge.Video.DirectShow.FilterCategory.VideoInputDevice);
                fpsrate = fps;
                SurveillanceSystem = systemname;
                MotionRecording = false;
              
            }
            catch
            {

            }
        }
        public VideoSurveillance(string videofile, int width, int height, int bitrate, int fps, string systemname, bool motion)
        {
            try
            {
                writer = new VideoFileWriter();
                writer.Open(videofile, width, height, fps, VideoCodec.MPEG4, bitrate);
                cameras = new FilterInfoCollection(AForge.Video.DirectShow.FilterCategory.VideoInputDevice);
                fpsrate = fps;
                SurveillanceSystem = systemname;
                MotionRecording = motion;
                if (motion)
                    Motion = new MotionDetection(0.015f);
            }
            catch
            {

            }
        }
        public VideoSurveillance(string videofile, int width, int height, int bitrate, int fps, bool motion)
        {
            try
            {
                writer = new VideoFileWriter();
                writer.Open(videofile, width, height, fps, VideoCodec.MPEG4, bitrate);
                cameras = new FilterInfoCollection(AForge.Video.DirectShow.FilterCategory.VideoInputDevice);
                fpsrate = fps;
                MotionRecording = motion;
                if (motion)
                    Motion = new MotionDetection(0.015f);
            }
            catch
            {

            }
        }
        public void Start(string monikerstring)
        {
            try
            {
                device = new VideoCaptureDevice(monikerstring);
                device.NewFrame += new NewFrameEventHandler(videoNewFrame);
                device.DesiredFrameRate = FrameRate;

                device.Start();
            }
            catch
            {

            }
        }
        public void Stop()
        {
            try
            {
                device.Stop();
                writer.Close();
            }
            catch
            {

            }
        }
        public Bitmap ResizeBitmap(Bitmap bmp, int width, int height)
        {
            ResizeBilinear resizer = new ResizeBilinear(width, height);
            return resizer.Apply(bmp);
        }
        public Bitmap AdjustBrightness(Bitmap Image, int Value)
        {
            System.Drawing.Bitmap TempBitmap = Image;
            float FinalValue = (float)Value / 255.0f;
            System.Drawing.Bitmap NewBitmap = new System.Drawing.Bitmap(TempBitmap.Width, TempBitmap.Height);
            System.Drawing.Graphics NewGraphics = System.Drawing.Graphics.FromImage(NewBitmap);
            float[][] FloatColorMatrix ={
                     new float[] {1, 0, 0, 0, 0},
                     new float[] {0, 1, 0, 0, 0},
                     new float[] {0, 0, 1, 0, 0},
                     new float[] {0, 0, 0, 1, 0},
                     new float[] {FinalValue, FinalValue, FinalValue, 1, 1}
                 };

            System.Drawing.Imaging.ColorMatrix NewColorMatrix = new System.Drawing.Imaging.ColorMatrix(FloatColorMatrix);
            System.Drawing.Imaging.ImageAttributes Attributes = new System.Drawing.Imaging.ImageAttributes();
            Attributes.SetColorMatrix(NewColorMatrix);
            NewGraphics.DrawImage(TempBitmap, new System.Drawing.Rectangle(0, 0, TempBitmap.Width, TempBitmap.Height), 0, 0, TempBitmap.Width, TempBitmap.Height, System.Drawing.GraphicsUnit.Pixel, Attributes);
            Attributes.Dispose();
            NewGraphics.Dispose();
            return NewBitmap;
        }

        public Bitmap ToSurveillanceFrame(Bitmap frame)
        {
            Graphics g = Graphics.FromImage(frame);
            RectangleF rectf = new RectangleF(frame.Width - 121, frame.Height - 15, 200, 340);
            RectangleF srectf = new RectangleF(0, frame.Height - 15, 200, 340);
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            g.DrawString(SurveillanceSystem, new Font("Arial", 10, FontStyle.Italic | FontStyle.Bold), Brushes.White, srectf);
            g.DrawString(DateTime.Now.ToString(), new Font("Arial", 10, FontStyle.Italic | FontStyle.Bold), Brushes.White, rectf);

            g.Flush();
     

           return MakeGrayscale(frame);
        }
        public Bitmap MakeGrayscale(Bitmap original)
        {
            //create a blank bitmap the same size as original
            Bitmap newBitmap = new Bitmap(original.Width, original.Height);

            //get a graphics object from the new image
            Graphics g = Graphics.FromImage(newBitmap);

            //create the grayscale ColorMatrix
            ColorMatrix colorMatrix = new ColorMatrix(
               new float[][] 
      {
         new float[] {.3f, .3f, .3f, 0, 0},
         new float[] {.59f, .59f, .59f, 0, 0},
         new float[] {.11f, .11f, .11f, 0, 0},
         new float[] {0, 0, 0, 1, 0},
         new float[] {0, 0, 0, 0, 1}
      });

            //create some image attributes
            ImageAttributes attributes = new ImageAttributes();

            //set the color matrix attribute
            attributes.SetColorMatrix(colorMatrix);

            //draw the original image on the new image
            //using the grayscale color matrix
            g.DrawImage(original, new Rectangle(0, 0, original.Width, original.Height),
               0, 0, original.Width, original.Height, GraphicsUnit.Pixel, attributes);

            //dispose the Graphics object
            g.Dispose();
            return newBitmap;
        }

        Bitmap _currentFrame;
        Bitmap _svframe;
        public Bitmap CurrentFrame
        {
            get
            {
return _currentFrame;
            }
            set
            {
                _currentFrame = value;
            }
        }
        public Bitmap SurveillanceFrame
        {
            get
            {
                return _svframe;
            }
            set
            {
                _svframe = value;
            }
        }
        internal bool bio = false;
        internal byte lfm = 0;

        private void videoNewFrame(object sender, NewFrameEventArgs args)
        {
            try
            {
                CurrentFrame = args.Frame.Clone() as Bitmap;
                SurveillanceFrame = ToSurveillanceFrame((Bitmap)args.Frame.Clone());
              if (Motion != null)
                if (Motion.ProcessMotion(ref _currentFrame))
                    writer.WriteVideoFrame(ToSurveillanceFrame(SurveillanceFrame));
              if (OnFrameArrived != null)
              {
                  NewFrameEventArgs x = new NewFrameEventArgs(CurrentFrame);
                  OnFrameArrived(this, x);
              }
            }
            catch
            {

            }

        }
     
    }
}
