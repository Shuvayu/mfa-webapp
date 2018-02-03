using System.Reflection;

namespace MFA.Entities.LogicModels
{
    public class EmotionScores
    {
        //scores":{"
        //anger":0.000229681187,
        //"contempt":8.386975E-05,
        //"disgust":3.1555212E-06,
        //"fear":4.26527731E-06,
        //"happiness":7.907359E-06,
        //"neutral":0.995070457,
        //"sadness":0.000449379266,
        //"surprise":0.0041513117}

        public float Anger { get; set; }
        public float Contempt { get; set; }
        public float Disgust { get; set; }
        public float Fear { get; set; }
        public float Happiness { get; set; }
        public float Neutral { get; set; }
        public float Sadness { get; set; }
        public float Surprise { get; set; }

        public string FindHighestEmotionScore()
        {
            var result = string.Empty;
            var highestNumber = 0.00005;
            var propertyInfo = typeof(EmotionScores).GetProperties();
            foreach (PropertyInfo propInfo in propertyInfo)
            {
                var propValue = (float)(propInfo.GetValue(this));
                if (propValue > highestNumber)
                {
                    highestNumber = propValue;
                    result = propInfo.Name;
                }
            }
            return result;
        }
    }
}
