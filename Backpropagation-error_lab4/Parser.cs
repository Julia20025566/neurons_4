using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Backpropagation_error_lab4
{
    public class Parser
    {
        
        public Parser()
        {

        }

        public double[] parse(string path)
        {
            Bitmap image;
            image = new Bitmap(path);
            Color pixelColor = new Color();
            Color tmp;
            tmp = Color.FromArgb(255, 120, 115, 159);
            int m = image.Height;
            int n = image.Width;
            double[] arr = new double[m * n];
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < n; j++)
                {                   
                    arr[i * n + j] = pixelColor.A/255.0;                    
                }
            }
            return arr;
        }

        public double[] wrap(double t, string path)
        {
            double[] arr = parse(path);
            for (int i = 0; i < arr.Length; i++)
            {
                if (arr[i] < t)
                {
                    arr[i] = 0;
                }
            }
            return arr;
        }
    }
}
