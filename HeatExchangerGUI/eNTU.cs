using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeatExchangerGUI
{
    class eNTU
    {
        #region variables
        private int number_of_turns;
        private double pipe_ID;
        private double coil_length;
        private double water_heat_trans_coef;
        private double air_heat_trasf_coef;       
        private double total_conductance;

        public double q { get; set; }
        public double t_he { get; set; }
        public double t_ce { get; set; }

        public int thermal_conductivity { get; set; }

        public double t_ci { get; set; } 
        public double v_c { get; set; }
        public double t_hi { get; set; }
        public double v_h { get; set; }
        public double exchanger_height { get; set; }
        public double exchanger_diam { get; set; }
        public double pipe_thickness { get; set; }
        public double pipe_OD { get; set; }

        #endregion

        public eNTU()
        {

        }

        #region methods

        public void calculations()
        {
            #region geometry   
            geometry(exchanger_diam, exchanger_height, pipe_OD, pipe_thickness,
                out number_of_turns, out coil_length, out pipe_ID);
            #endregion

            #region heat transfer coeffs.                      
            waterHeatTransferCoef(v_c, pipe_ID, t_ci, out water_heat_trans_coef);
            airHeatTransferCoef(t_hi, t_ci, v_h, pipe_OD, out air_heat_trasf_coef);
            #endregion

            #region conductance
            calculateConductance(coil_length, pipe_ID, pipe_OD, water_heat_trans_coef, air_heat_trasf_coef, thermal_conductivity,
                out total_conductance);
            #endregion

            #region eNTU
            var results = effectiveness(t_ci, pipe_ID, v_c, v_h, t_hi, pipe_OD, exchanger_diam,
                number_of_turns, coil_length, exchanger_height, total_conductance);
            q = results.Item1;
            t_he = results.Item2;
            t_ce = results.Item3;
            #endregion

        }

        private static void geometry(double exchanger_diam, double exchanger_height, double pipe_OD,
            double pipe_thickness,
            out int number_of_turns, out double coil_length, out double pipe_ID)
        {
            double radius = exchanger_diam / 2 - pipe_OD;
            double N = (2 * exchanger_height - 3 * pipe_OD) / (3 * pipe_OD);
            number_of_turns = Convert.ToInt32(Math.Floor(N));
            coil_length = Math.Round(number_of_turns * Math.Sqrt(
                (2 * Math.PI * (exchanger_diam / 2 - pipe_OD) + 1.5 * pipe_OD)), 2);
            pipe_ID = pipe_OD - 2 * pipe_thickness;
        }

        private static void waterHeatTransferCoef(double velocity, double pipe_ID, double t_ci,
            out double water_heat_trans_coef)
        {
            water_heat_trans_coef = (1206 + 23.9 * t_ci) * Math.Pow(velocity, 0.8) / Math.Pow(pipe_ID, 0.2);
        }

        private static void airHeatTransferCoef(double t_hi, double t_ci, double v_h, double pipe_OD,
            out double air_heat_trasf_coef)
        {
            double viscosity = 18.27 * Math.Pow(10, -6) * Math.Pow((t_hi + 273.15) / 291.15, 3 / 2) *
                (291.15 + 120) / (273.15 + t_hi + 120);
            double density = (101325) / (287.05 * (t_hi + 273.15));
            double Re_air = (v_h * 4 * density * pipe_OD) / viscosity;
            if (Re_air < 5000)
            {
                air_heat_trasf_coef = 2.755 * Math.Pow(v_h, 0.471) / Math.Pow(4 * pipe_OD, 0.529);
            }
            else
            {
                air_heat_trasf_coef = (4.22 - 0.00257 * (t_hi - t_ci)) * Math.Pow(v_h, 0.633) / Math.Pow(4 * pipe_OD, 0.367);
            }
        }        

        private static void calculateConductance(double coil_length, double pipe_ID, double pipe_OD, double water_heat_trans_coef,
            double air_heat_trasf_coef, int thermal_conductivity,
            out double total_conductance)
        {
            double total_resistance = (1 / (Math.PI * pipe_ID * coil_length * water_heat_trans_coef) +
                                       Math.Log(pipe_OD / pipe_ID) / (2 * Math.PI * thermal_conductivity * coil_length) +
                                       1 / (Math.PI * pipe_OD * coil_length * air_heat_trasf_coef));
            total_conductance = 1 / total_resistance;
        }

        private static Tuple<double, double, double> effectiveness(double t_ci, double pipe_ID, double v_c, 
            double v_h, double t_hi, double pipe_OD, double exchanger_diam, int number_of_turns,
            double coil_length, double exchanger_height, double total_conductance)
        {
            double air_dens = (101325) / (287.05 * (0.9 * t_hi + 273.15));
            double cph = 1057 - 0.4489 * 0.9 * (t_hi + 273.15) + 0.00114 * Math.Pow(0.9 * (t_hi + 273.15), 2);
            double gas_area = (Math.PI / 4 * (Math.Pow(exchanger_diam, 2) - Math.Pow(exchanger_diam - 2 * pipe_OD, 2)) * 1.5 * pipe_OD *
                number_of_turns - (Math.PI / 4 * (Math.Pow(pipe_OD, 2)) * coil_length)) / (exchanger_height - 2 * pipe_OD);
            double ch = v_h * gas_area * air_dens * cph;
            double water_dens = 1001.1 - 0.0867 * 1.1 * t_ci - 0.0035 * Math.Pow(1.1 * t_ci, 2);
            double cpc = 4214 - 2266 * 0.001 * 1.1 * t_ci + 4991 * 0.00001 * Math.Pow(1.1 * t_ci, 2) - 4519 * 0.0000001 * Math.Pow(1.1 * t_ci, 3);
            double cc = v_c * Math.PI * Math.Pow(pipe_ID, 2) * water_dens / 4 * cpc;
            double capacity_rate_ratio = Math.Min(cc, ch) / Math.Max(cc, ch);
            double NTU = total_conductance / Math.Min(cc, ch);
            double epsilon = 1 - Math.Exp((Math.Exp(-capacity_rate_ratio * Math.Pow(NTU, 0.78)) - 1)
                * Math.Pow(NTU, 0.22) / capacity_rate_ratio);

            double q_max = Math.Min(cc, ch) * (t_hi - t_ci);
            double q = q_max * epsilon;
            double t_he = t_hi - q / ch;
            double t_ce = t_ci + q / cc;
            var tuple = new Tuple <double, double, double> (q, t_he, t_ce);
            return tuple;
        }
        #endregion 
    }
}
