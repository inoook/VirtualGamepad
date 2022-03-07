using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kalman
{

	/// <summary>
	/// Simple kalman wrapper.
	/// </summary>
    [System.Serializable]
	public class KalmanFilter1D
	{
		[SerializeField] private KalmanFilterSimple1D kX;

		public KalmanFilter1D()
		{
			/*
			X0 : predicted state
			P0 : predicted covariance
			
			F : factor of real value to previous real value
			Q : measurement noise
			H : factor of measured value to real value
			R : environment noise
			*/
			double q = 0.4;
			double r = 10;
			double f = 1.0;
			double h = 1.0;

			kX = makeKalmanFilter(q, r, f, h);
		}


		public float Update(float current)
		{
			kX.Correct(current);

			return (float)kX.State;
		}

		public void Dispose()
		{

		}

		#region Privates
		KalmanFilterSimple1D makeKalmanFilter(double q, double r, double f, double h)
		{
			var filter = new KalmanFilterSimple1D(q, r, f, h);
			// set initial value
			//filter.SetState(500, 5.0);
			filter.SetState(0, 5.0);
			return filter;
		}
		#endregion


	}
}