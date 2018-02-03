namespace MFA.Entities.LogicModels
{
    public class EmotionResponse
    {
        public FaceRectangle FaceRectangle { get; set; }
        public EmotionScores Scores { get; set; }
        public string EmotionAnalyzed => Scores.FindHighestEmotionScore();
    }
}
