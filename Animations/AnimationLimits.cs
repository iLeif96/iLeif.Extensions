namespace iLeif.Extensions.Animations
{
    public class AnimationLimits
    {
        public double MaxTime { get; set; }
        public double MaxLength { get; set; }
        public double? MaxSpeed { get; set; }
        public DecelerationType DecelerationType { get; set; }

        public AnimationLimits(double maxTime, double maxLength, DecelerationType decelerationType = DecelerationType.Auto)
        {
            MaxTime = maxTime;
            MaxLength = maxLength;
            DecelerationType = decelerationType;
        }

        public AnimationLimits(double maxTime, double maxLength, double maxSpeed, DecelerationType decelerationType = DecelerationType.Auto)
        {
            MaxTime = maxTime;
            MaxLength = maxLength;
            DecelerationType = decelerationType;
            MaxSpeed = maxSpeed;
        }
    }
}
