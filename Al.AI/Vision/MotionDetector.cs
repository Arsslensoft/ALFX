using AForge.Vision.Motion;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Al.AI.Vision
{
    public class MotionDetection
    {
        public MotionDetection(float motionAlarmlvl)
        {
            motionAlarmLevel = motionAlarmlvl;
        }
        // motion detector
      public  MotionDetector detector = new MotionDetector(
            new SimpleBackgroundModelingDetector(),
            new BlobCountingObjectsProcessing());
        // motion detection and processing algorithm
        private int motionDetectionType = 1;
        private int motionProcessingType = 1;
        private float motionAlarmLevel = 0.015f;

        public float MotionDetectionLevel
        {
            get
            {
                return motionAlarmLevel;
            }
            set { motionAlarmLevel = value; }
        }
        public void PrecessFrame(ref Bitmap frame)
        {
            detector.ProcessFrame(frame);
        }

        public bool ProcessMotion(ref Bitmap frame)
        {
            float motionLevel = detector.ProcessFrame(frame);

            if (motionLevel > motionAlarmLevel)
                return true;
            else
                return false;
        }

    }
}
