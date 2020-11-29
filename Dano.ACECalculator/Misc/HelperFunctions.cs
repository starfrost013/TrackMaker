using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Dano.ACECalculator
{
    partial class CalcMainWindow
    {
        public double GenACE(double intensity, int mode)
        {
            double ACE;
            switch (mode)
            {
                case 0:
                    ACE = Math.Pow(intensity, 2) / 10000;
                    return ACE;
                case 1:
                    ACE = Math.Pow(RoundNearest(intensity / 1.15078, 5), 2) / 10000;
                    return ACE;
                default:
                    return 0;
            }
        }

        private static double RoundNearest(double raw, double n)
        {
            return (Math.Round(raw / n)) * n;
        }

        public void SetDateTimeVisibility(bool visible)
        {
            if (!visible)
            {
                if (!DateTimeOn)
                {
                    return;
                }
                // is it 0?
                DateTimeOn = false;
                Width -= 50;
       
                StormIntensities.Width -= 125;
                StormIntensities_DateTime.Width = 0;
            }
            else
            {
                if (this.DateTimeOn) // if its the same, prevent repeats.
                {
                    return;
                }
                this.DateTimeOn = true;
                this.Width += 50;
                StormIntensities.Width += 125;
                StormIntensities_DateTime.Width = 125;
            }
        }

        public void AddPoint()
        {
            try
            {
                double intensity = Convert.ToDouble(EnterKt.Text);
                double ace = 0;
                double t = 0;

                
                StormIntensityNode node = new StormIntensityNode { DateTime = CurrentDateTime, Intensity = intensity, ACE = ace, Total = t };
                CurrentDateTime = CurrentDateTime.AddHours(6);

                if (IntensityMeasure == 0)
                {
                    node.ACE = GenACE(node.Intensity, 0); // calculate the ACE

                    if (intensity < 34)
                    {
                        node.ACE = 0;
                    }
                }
                else
                {
                    // CANCER CODE
                    node.ACE = GenACE(node.Intensity, 1); // convert to knots 

                    if (intensity < 39)
                    {
                        node.ACE = 0;
                    }

                }
                if (SinglePoint == 0)
                {
                    foreach (StormIntensityNode sin in StormIntensities.Items)
                    {
                        TotalACE += sin.ACE; // add the ace of every node to each system
                    }

                    if (StormIntensities.Items.Count == 0)
                    {
                        TotalACE = node.ACE; // fixes this bug
                        node.Total = TotalACE;
                    }
                    else
                    {
                        node.Total = TotalACE + node.ACE;
                    }

                }
                // fix bug.
                else
                {
                    StormIntensities.Items.Clear();
                    TotalACE += node.ACE;
                    node.Total = TotalACE;
                }

                StormIntensities.Items.Add(node);


                if (SinglePoint == 0)
                {
                    TotalACE = 0; // dont do this if we are in single point mode
                }

                EnterKt.Text = ""; // v1.4: autoclear for usability.
                return;
            }
            catch (FormatException) // someone entered gibberish
            {
                MessageBox.Show("Error: You must input a number.", "ACE Calculator", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
        }
    }
}
