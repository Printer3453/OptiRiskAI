namespace OptiRiskAI.RiskAnalysis;

public class RiskAnalysisResultDto
{
    public bool IsRiskDetected { get; set; }
    public double FwiScore { get; set; }
    public string Message { get; set; } = string.Empty;
}