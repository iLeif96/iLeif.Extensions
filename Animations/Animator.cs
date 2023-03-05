using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using iLeif.Extensions.Logging;
using iLeif.Extensions.Multitasking;

namespace iLeif.Extensions.Animations
{
    public class Animator
    {
        public AnimationLimits Limits { get; private set; }

        public int FPS { get; set; } = 60;
        public int Frequency => 1000 / FPS;

        public bool InProgress => GetCurrentStatus();

        private Dictionary<string, ProcessingTask> _processingAnimations;

        private ILogger _logger;

        public Animator(ILogger logger)
        {
            Limits = new AnimationLimits(1000, 300);
            _logger = logger;
            _processingAnimations = new Dictionary<string, ProcessingTask>();
        }

        ~Animator()
        {
            Stop();
        }

        public void DoTransit(double startSpeed, Func<double, bool> perFrameAction, AnimationLimits? limits, string identificator = "Unknown")
        {
            if (limits == null)
            {
                limits = Limits;
            }

            if (limits.MaxSpeed != null && startSpeed > limits.MaxSpeed)
            {
                startSpeed = limits.MaxSpeed.Value;
            }

            var deceleration = ResolveDeceleration(startSpeed, limits);

            AnimationProperties props = new AnimationProperties(startSpeed, deceleration.value, limits);

            Action<CancellationToken> animation = (token) => TransitAnimation(props, perFrameAction, token);
            var processingTask = AddAnimationProcess(identificator, animation);
            processingTask.Start();
        }

        private (DecelerationType type, double value) ResolveDeceleration(double startSpeed, AnimationLimits limits)
        {
            (DecelerationType type, double value) deceleration = (limits.DecelerationType, 0);

            if (limits.DecelerationType == DecelerationType.Auto)
            {
                deceleration = DecelerationAuto(startSpeed, limits.MaxTime, limits.MaxLength);
            }
            else
            {
                deceleration.value = limits.DecelerationType switch
                {
                    DecelerationType.Time => DecelerationByTime(startSpeed, limits.MaxTime),
                    DecelerationType.Length => DecelerationByLength(startSpeed, limits.MaxLength),
                    DecelerationType.TimeAndLength => DecelerationByLengthAndTime(startSpeed, limits.MaxTime, limits.MaxLength),
                    _ => throw new NotImplementedException()
                };
            }

            return deceleration;
        }

        private (DecelerationType type, double value) DecelerationAuto(double startSpeed, double totalTime, double totalLength)
        {
            DecelerationType decelerationType = DecelerationType.Time;
            double deceleration = DecelerationByTime(startSpeed, totalTime);

            var expectedLength = ResolveExpectedLength(startSpeed, deceleration);
            if (expectedLength > totalLength)
            {
                decelerationType = DecelerationType.Length;
                deceleration = DecelerationByLength(startSpeed, totalLength);
            }

            expectedLength = ResolveExpectedLength(startSpeed, deceleration);
            if (expectedLength > totalLength)
            {
                decelerationType = DecelerationType.TimeAndLength;
                deceleration = DecelerationByLengthAndTime(startSpeed, totalLength, totalTime);
            }


            return (decelerationType, deceleration);
        }
        private double DecelerationByTime(double startSpeed, double totalTime)
        {
            var deceleration = -(startSpeed / totalTime);
            return deceleration;
        }
        private double DecelerationByLength(double startSpeed, double totalLength)
        {
            var deceleration = -(startSpeed * startSpeed) / (2 * totalLength);
            return deceleration;
        }
        private double DecelerationByLengthAndTime(double startSpeed, double totalLength, double totalTime)
        {
            var deceleration = -(2 * totalLength / (totalTime * totalTime));
            return deceleration;
        }
        private double ResolveExpectedLength(double startSpeed, double deceleration)
        {
            //return (startSpeed * totalTime) - (deceleration * (totalTime * totalTime) / 2);
            return startSpeed * startSpeed / (2 * deceleration);
        }

        private void TransitAnimation(AnimationProperties props, Func<double, bool> actionPerFrame, CancellationToken? token = null)
        {
            AnimationLimits limits = props.Limits;

            if (limits.DecelerationType == DecelerationType.Auto)
            {
                throw new Exception("On Animation step deceleration type must be resolved from auto");
            }

            int animFrame = 0;
            int frequency = Frequency;
            long animationStartTime = DateTime.Now.Millisecond;
            long animationSpentTime = 0;
            long nextDrawTime = 0;

            double spentLength = 0;
            double curValue = 0;
            while (CheckBreakCondition())
            {
                if (animationSpentTime >= nextDrawTime)
                {
                    curValue = props.StartValue + props.Deceleration * animationSpentTime;
                    spentLength += Math.Abs(curValue); //TODO: Its not work

                    //var timeInSecond = Math.Round((double)animationSpentTime / 1000 * 60, 2);
                    //_logger.Info($"Animation frame {animFrame}, Time spent {timeInSecond}s," +
                    //$"Length spent {spentLength}: " +
                    //$"Acceleration: {acceleration}");

                    if (token?.IsCancellationRequested == true)
                    {
                        break;
                    }

                    if (actionPerFrame(curValue) == false)
                    {
                        break;
                    }

                    animFrame++;
                    nextDrawTime = animFrame * frequency;
                }

                animationSpentTime = (int)(DateTime.Now.Millisecond - animationStartTime);
            }

            bool CheckBreakCondition()
            {
                if (limits.DecelerationType != DecelerationType.Length)
                {
                    if (animationSpentTime > limits.MaxTime)
                    {
                        return false;
                    }
                }

                if (limits.DecelerationType != DecelerationType.Time)
                {
                    if (spentLength > limits.MaxLength)
                    {
                        return false;
                    }
                }

                return true;
            }
        }

        private ProcessingTask AddAnimationProcess(string identificator, Action<CancellationToken> action)
        {
            if (_processingAnimations.TryGetValue(identificator, out ProcessingTask? anim))
            {
                anim?.Stop();
            }

            var processingTask = new ProcessingTask(action);
            _processingAnimations[identificator] = processingTask;

            return processingTask;
        }

        private bool GetCurrentStatus()
        {
            bool isInProgress = false;
            foreach (var animation in _processingAnimations)
            {
                var anim = animation.Value;
                if (anim == null)
                {
                    continue;
                }

                isInProgress = isInProgress || anim.IsRunning;
            }

            return isInProgress;
        }

        public void Stop()
        {
            foreach (var animation in _processingAnimations)
            {
                animation.Value?.Stop();
            }
        }
    }
}
