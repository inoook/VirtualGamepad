using UnityEngine;
namespace Kalman
{
    [System.Serializable]
    public class KalmanFilterSimple1D
    {
        public double X0 { get => x0; private set => x0 = value; } // predicted state
        public double P0 { get => p0; private set => p0 = value; } // predicted covariance

        public double F { get => f; private set => f = value; } // factor of real value to previous real value
        public double Q { get => q; private set => q = value; } // measurement noise
        public double H { get => h; private set => h = value; } // factor of measured value to real value
        public double R { get => r; private set => r = value; } // environment noise

        public double State { get => state; private set => state = value; }
        public double Covariance { get => covariance; private set => covariance = value; }

        [SerializeField] double x0;
        [SerializeField] double p0;

        [SerializeField] double q;
        [SerializeField] double r;
        [SerializeField] double f;
        [SerializeField] double h;

        [SerializeField] double state;
        [SerializeField] double covariance;

        public KalmanFilterSimple1D(double q, double r, double f = 1, double h = 1)
        {
            Q = q;
            R = r;
            F = f;
            H = h;
        }

        public void SetState(double state, double covariance)
        {
            State = state;
            Covariance = covariance;
        }

        public void Correct(double data)
        {
            //time update - prediction
            X0 = F * State;
            P0 = F * Covariance * F + Q;

            //measurement update - correction
            var K = H * P0 / (H * P0 * H + R);
            State = X0 + K * (data - H * X0);
            Covariance = (1 - K * H) * P0;
        }
    }
}
