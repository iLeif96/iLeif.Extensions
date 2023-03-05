namespace iLeif.Extensions.Animations
{
    public enum DecelerationType { Auto, Time, Length, TimeAndLength }

    public class AnimationProperties
    {
        public double StartValue { get; set; }
        public double Deceleration { get; set; }
        public AnimationLimits Limits { get; set; }

        public AnimationProperties(double startValue, double deceleration, DecelerationType decelerationType, double maxTime, double maxLength)
        {
            StartValue = startValue;
            Deceleration = deceleration;
            Limits = new AnimationLimits(maxTime, maxLength, decelerationType);
        }

        public AnimationProperties(double startValue, double deceleration, AnimationLimits limits)
        {
            StartValue = startValue;
            Deceleration = deceleration;
            Limits = limits;
        }
    }
}
