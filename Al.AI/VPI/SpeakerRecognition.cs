using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using Exocortex.DSP;

namespace Al.AI
{
    /// <summary>
    /// Recognizes a speaker from it's voice
    /// </summary>
    public class SpeakerRecognition
    {
        public int DefaultTime =2;
        int SEC = 0;
        WaveInRecorder s_WaveIn;
        void Record(int seconds)
        {
            try
            {
                VMODEL = new List<ComplexF[]>();
                SEC = seconds;
                Recorded = false;
                WaveFormat _waveFormat = new WaveFormat(44100, 16, 2);
                s_WaveIn = new WaveInRecorder(0, _waveFormat, 16384, 3, new BufferDoneEventHandler(DataArrived));
            }
            catch
            {

            }
        }
        byte[] _recorderBuffer = new byte[16384];
        int s = 0;
        List<ComplexF[]> VMODEL;
        private void DataArrived(IntPtr data, int size)
        {
        
            System.Runtime.InteropServices.Marshal.Copy(data, _recorderBuffer, 0, size);
            float[] waved = new float[4096];
            int h = 0;
            for (int i = 0; i < _recorderBuffer.Length; i += 4)
            {
                waved[h] = (float)BitConverter.ToInt16(_recorderBuffer, i);
                h++;
            }

            Exocortex.DSP.ComplexF[] complexData = new Exocortex.DSP.ComplexF[4096];
            for (int i = 0; i < 4096; ++i)
            {
                // Fill the complex data
                complexData[i].Re = waved[i]; // Add your real part here
                complexData[i].Im = waved[i] + 5; // Add your imaginary part here
            }
            // FFT the time domain data to get frequency domain data
            Exocortex.DSP.Fourier.FFT(complexData, Exocortex.DSP.FourierDirection.Forward);



            VMODEL.Add(complexData);
   
            if (s == 2)
            {
                s = 0;
                SEC--;
                if (SEC == 0)
                {
                    Recorded = true;
                    s_WaveIn.Dispose();
                }
            }
            else
            {
                s++;
            }
        }
        public bool Recorded = false;

        /// <summary>
        /// Normalize a wave file
        /// </summary>
        /// <param name="wavin">wave file</param>
        /// <param name="waveout">out put wave file</param>
        /// <param name="multiplier">ratio</param>
        /// <returns></returns>
        public bool Normalize(string wavin, string waveout)
        {
            try
            {
                ProcessStartInfo info = new ProcessStartInfo();
                info.FileName = Application.StartupPath + @"\sox.exe";
                //info.Arguments = " −r 48k −e float −b 8 −c 2 \"" + wavin + "\"  \"S" + waveout + "\" ";
                info.Arguments = " --norm \"" + wavin + "\"  \"" + waveout + "\" ";
                info.UseShellExecute = false;
                info.WindowStyle = ProcessWindowStyle.Hidden;
                info.RedirectStandardOutput = true;
                info.CreateNoWindow = true;
                bool run = true;
                bool state = true;
              Process p =  Process.Start(info);
              p.BeginOutputReadLine();
              p.OutputDataReceived += delegate(object sender, DataReceivedEventArgs e)
              {
                
                
                      state = false;
                          
      
              };
              while (!p.HasExited)
                  Thread.Sleep(100);

              p.Close();

              return state;
            }
            catch
            {
                return false;
            }
        }

        public float[] GetVP()
        {
            Record(DefaultTime);
            while (!Recorded)
                Thread.Sleep(1000);
            float[] f = new float[VMODEL.Count * 4096];
            int x = 0;
   
            for (int i = 0; i <= VMODEL.Count - 1; i++)
            {
                for (int j = 0; j <= 4095; j++)
                {
                    ComplexF cf = VMODEL[i][j];
                    f[x] = cf.Re;
                 
                    x++;
                }
            }
            return f;
        }
        public float[] GetVPFromWave(string wavefile)
        {
            string w = Path.GetDirectoryName(wavefile) + @"\n_" + Path.GetFileName(wavefile);
            Normalize(wavefile, w);
            WAVFile wave = new WAVFile();
            wave.Open(w, WAVFile.WAVFileMode.READ);
            float[] samples = new float[wave.NumSamples];
            for (int i = 0; i < wave.NumSamples; i++)
              samples[i] = wave.GetNextSample_16bit();

            int cp = 0;
            int t = samples.Length / 1024;

            if (t > 0)
            {
                int r = samples.Length % 1024;
                for (int x = 1; x <= t; x++)
                {
                    Exocortex.DSP.ComplexF[] complexData = new Exocortex.DSP.ComplexF[1024];
                    for (int i = cp; i <= 1023; i++)
                    {
                        complexData[i].Re = samples[cp+i];
                        complexData[i].Im = samples[cp+i] + 5;

                    }
                    Exocortex.DSP.Fourier.FFT(complexData,complexData.Length, FourierDirection.Forward);
                    for (int i = 0; i <= 1023; i++)
                      samples[cp + i] = complexData[i].Re;

                    cp += 1024;
                }

               }

            wave.Close();
                return samples;
            
        }
        public short[] GetSecondaryVPFromWave(string wavefile)
        {
          //  string w = Path.GetDirectoryName(wavefile) + @"\n_" + Path.GetFileName(wavefile);
           // Normalize(wavefile, w);
            WAVFile wave = new WAVFile();
            wave.Open(wavefile, WAVFile.WAVFileMode.READ);
            short[] samples = new short[wave.NumSamples];
            for (int i = 0; i < wave.NumSamples; i++)
                samples[i] = wave.GetNextSample_16bit();

            int cp = 0;
            int t = samples.Length / 1024;

            //if (t > 0)
            //{
            //    int r = samples.Length % 1024;
            //    for (int x = 1; x <= t; x++)
            //    {
            //        Exocortex.DSP.ComplexF[] complexData = new Exocortex.DSP.ComplexF[1024];
            //        for (int i = cp; i <= 1023; i++)
            //        {
            //            complexData[i].Re = samples[cp + i];
            //            complexData[i].Im = samples[cp + i] + 5;

            //        }
            //        Exocortex.DSP.Fourier.FFT(complexData, complexData.Length, FourierDirection.Forward);
            //        for (int i = 0; i <= 1023; i++)
            //            samples[cp + i] = complexData[i].Re;

            //        cp += 1024;
            //    }

            //}
            wave.Close();

            return samples;

        }
        public double Match(float[] d1, float[] d2)
        {
            double rprob = 0.0;
            int l = d1.Length;
            int dl = d2.Length;
            if (d1.Length > d2.Length)
            {
                l = d2.Length;
                 dl = d1.Length;
            }
            else if (d2.Length > d1.Length)
            {
                l = d1.Length;
                dl = d2.Length;
            }

            for (int i = 0; i <= l - 1; i++)
            {
                if (d1[i] == d2[i])
                    rprob++;
                else if (d1[i] + 5 >= d2[i])
                    rprob += 0.50;
                else if (d2[i] + 5 >= d1[i])
                    rprob += 0.50;
                else if (d1[i] + 10 >= d2[i])
                    rprob += 0.25;
                else if (d2[i] + 10 >= d1[i])
                    rprob += 0.25;
                else if (d1[i] + 15 >= d2[i])
                    rprob += 0.10;
                else if (d2[i] + 15 >= d1[1])
                    rprob += 0.10;
             
            }

          
return (rprob / dl) * 100;

        }
        public double MatchS(short[] d1, short[] d2)
        {
            double rprob = 0.0;
            int l = d1.Length;
            int dl = d2.Length;
            if (d1.Length > d2.Length)
            {
                l = d2.Length;
                dl = d1.Length;
            }
            else if (d2.Length > d1.Length)
            {
                l = d1.Length;
                dl = d2.Length;
            }

            for (int i = 0; i <= l - 1; i++)
            {
                if (d1[i] == d2[i])
                    rprob++;
                else if (d1[i] + 5 >= d2[i])
                    rprob += 0.50;
                else if (d2[i] + 5 >= d1[i])
                    rprob += 0.50;
                else if (d1[i] + 10 >= d2[i])
                    rprob += 0.25;
                else if (d2[i] + 10 >= d1[i])
                    rprob += 0.25;
                else if (d1[i] + 15 >= d2[i])
                    rprob += 0.10;
                else if (d2[i] + 15 >= d1[1])
                    rprob += 0.10;

            }


            return (rprob / dl) * 100;

        }
    }
}
