using Common;
using System;

namespace Clutch
{
    internal class Logic
    {
        private bool _changed = false;

        public bool UseUniformPressure { get; set; } = false;
        
        private double _frictionCoefficient;
        public double FrictionCoefficient
        {
            get => _frictionCoefficient;
            set => SetValue(ref _frictionCoefficient, value);
        }

        private double _plateOutterDiameter;
        public double PlateOutterDiameter
        {
            get => _plateOutterDiameter;
            set => SetValue(ref _plateOutterDiameter, value / 1000);
        }

        private double _plateInnerDiameter;
        public double PlateInnerDiameter
        {
            get => _plateInnerDiameter;
            set => SetValue(ref _plateInnerDiameter, value / 1000);
        }

        private Angle _plateAngle = new();
        public double PlateAngle
        {
            get => _plateAngle.Degrees;
            set
            {
                _changed |= _plateAngle.Degrees != value;
                _plateAngle.Degrees = value;
            }
        }

        private double _axialForce;
        public double AxialForce
        {
            get => _axialForce;
            set => SetValue(ref _axialForce, value);
        }

        public double RPM { get; set; }

        private double _maxTorque;
        public double MaxTorque
        {
            get
            {
                if (_changed)
                {
                    _maxTorque = UseUniformPressure
                        ? CalculateMaxTorqueWear()
                        : CalculateMaxTorquePressure();
                    _changed = false;
                }

                return _maxTorque;
            }
        }

        public double MaxPower => ConvertTorqueToHorsePower(MaxTorque, RPM);

        private void SetValue(ref double field, double value)
        {
            _changed |= field != value;
            field = value;
        }

        private double CalculateMaxTorqueWear()
        {
            return _frictionCoefficient * _axialForce * (_plateOutterDiameter + _plateInnerDiameter)
                / (4 * Math.Sin(_plateAngle.Radians));
        }

        private double CalculateMaxTorquePressure()
        {
            return _frictionCoefficient * _axialForce * (Math.Pow(_plateOutterDiameter, 3) - Math.Pow(_plateInnerDiameter, 3))
                / (3 * Math.Sin(_plateAngle.Radians) * (Math.Pow(_plateOutterDiameter, 2) - Math.Pow(_plateInnerDiameter, 2)));
        }

        private static double ConvertTorqueToHorsePower(double torque, double rpm)
        {
            return torque * rpm / 5.252;
        }
    }
}
