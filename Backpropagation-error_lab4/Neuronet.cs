using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backpropagation_error_lab4
{
    public class Neuronet
    {
        private Random rnd { get; } = new Random();
        public int inputNeurons { get; set; }
        public int hiddenNeurons { get; set; }
        public int outputNeurons { get; set; }

        //веса
        public double[][] wih { get; set; } //вход-скрытый слой
        public double[][] who { get; set; } //скрытый слой - выход

        //активаторы
        public double[] inputs {get; set; }
        public double[] hidden { get; set; }
        //public double[] target { get; set; }
        public double[] actual { get; set; }

        //ошибки
        public double[] erro { get; set; }
        public double[] errh { get; set; }

        // темп
        public double learnRate { get; set; }

        public Neuronet (int inputNeurons, int hiddenNeurons, int outputNeurons, double learnRate)
        {
            this.inputNeurons = inputNeurons;
            this.hiddenNeurons = hiddenNeurons;
            this.outputNeurons = outputNeurons;
            this.wih = new double[inputNeurons + 1][];

            for (int i = 0; i <= inputNeurons; i++)
            {
                wih[i] = new double[hiddenNeurons];
            }
            this.who = new double[hiddenNeurons + 1][];
            for (int i = 0; i <= hiddenNeurons; i++)
            {
                who[i] = new double[outputNeurons];
            }

            this.inputs = new double[inputNeurons];
            this.hidden = new double[hiddenNeurons];
            //this.target = new double[outputNeurons];
            this.actual = new double[outputNeurons];
            this.erro = new double[outputNeurons];
            this.errh = new double[hiddenNeurons];
            this.learnRate = learnRate;
        }


        //Назначение рандомного веса 
        public void assignRandomWeights()
        {
            int hid, inp, outn;

            for (inp = 0; inp <= inputNeurons; inp++)
            {
                for (hid = 0; hid < hiddenNeurons; hid++)
                {
                    wih[inp][hid] = rnd.NextDouble() - 0.5;
                }
            }

            for (hid = 0; hid <= hiddenNeurons; hid++)
            {
                for (outn = 0; outn < outputNeurons; outn++)
                {
                    who[hid][outn] = rnd.NextDouble() - 0.5;
                }
            }
        }

        // Функция активации -- сигмоидальная
        private double sigmoid (double val)
        {
            return (1.0 / (1.0 + Math.Exp(-val)));
        }

        // Первая производная функция активации 
        private double sigmoidDerivative ( double val)
        {
            return (val * (1.0 - val));
        }

        public void feedForward ()
        {
            int inp, hid, outn;
            double sum;

            //вычислить вход в скрытый слой
            for (hid = 0; hid < hiddenNeurons; hid++)
            {
                sum = 0.0;
                for (inp = 0; inp < inputNeurons; inp++)
                {
                    sum += inputs[inp] * wih[inp][hid];
                }
                //Добавить смещение
                sum += wih[inputNeurons][hid];
                hidden[hid] = sigmoid(sum);
            }

            //вычислить вход в выходной слой
            for (outn = 0; outn < outputNeurons; outn++)
            {
                sum = 0.0;
                for (hid = 0; hid < hiddenNeurons; hid++)
                {
                    sum += hidden[hid] * who[hid][outn];
                }
                //Добавить смещение
                sum += who[hiddenNeurons][outn];
                actual[outn] = sigmoid(sum);
            }
        }

        public void backPropagate(double[] target)
        {
            int inp, hid, outn;

            //вычислить ошибку выходного слоя
            for (outn = 0; outn < outputNeurons; outn++)
            {
                //erro[outn] = (target[outn] - actual[outn]) * sigmoidDerivative(actual[outn]);
                erro[outn] = target[outn] - actual[outn];

            }

            //вычислить ошибку скрытого слоя
            for (hid = 0; hid < hiddenNeurons; hid++)
            {
                errh[hid] = 0.0;
                for (outn = 0; outn < outputNeurons; outn++)
                {
                    errh[hid] += erro[outn] * who[hid][outn];
                }
                //errh[hid] *= sigmoidDerivative(hidden[hid]);
            }

            //обновить веса для выходного слоя
            for (outn = 0; outn < outputNeurons; outn++)
            {
                for (hid = 0; hid < hiddenNeurons; hid++)
                {
                    who[hid][outn] += (learnRate * erro[outn] * hidden[hid] * sigmoidDerivative(actual[outn]));
                }

                //обновить смещение
                who[hiddenNeurons][outn] += (learnRate * erro[outn] * sigmoidDerivative(actual[outn]));
            }

            //обновить веса для скрытого слоя
            for (hid = 0; hid < hiddenNeurons; hid++)
            {
                for (inp = 0; inp < inputNeurons; inp++)
                {
                    wih[inp][hid] += (learnRate * errh[hid] * inputs[inp] * sigmoidDerivative(hidden[hid]));
                }

                //обновить смещение
                wih[inputNeurons][hid] += (learnRate * errh[hid] * sigmoidDerivative(hidden[hid]));
            }
        }

        private int action()
        {
            int i, sel;
            double max;

            sel = 0;
            max = actual[sel];
            for (i = 1; i < actual.Length; i++)
            {
                if (actual[i] > max)
                {
                    max = actual[i];
                    sel = i;
                }
            }
            return sel;
        }

        public int test ( double[] input)
        {
            for (int i = 0; i < inputNeurons; i++)
            {
                inputs[i] = input[i];
            }

            feedForward();

            return action();
        }

        public void learn (double[] input, double[] output )
        {
            for (int i = 0; i < inputNeurons; i++)
            {
                inputs[i] = input[i];
            }
            feedForward();

            backPropagate(output);
        }

        // Эпоха
        public void epoch (double[][] input, double[][] output, int n)
        {
            for (int i = 0; i < n; i++)
            {
                learn(input[i], output[i]);
            }
        }
    }
}
