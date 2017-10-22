using System;
using System.Collections.Generic;
using System.Text;

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

        public String FindHighestEmotionScore()
        {
            string result = string.Empty;
            return result;
        }
    }
}
