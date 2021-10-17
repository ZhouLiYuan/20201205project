/// <summary>
/// 不继承自Congfig基类
/// SpeakSide 0：己方 1：对方
/// </summary>
public class DialogueConfig
{
    public int EpisodeID; // 片段ID
    public int Sequence; // 顺序(大概率用不上,勉为其难加载时用了一下)
    public int SpeakSide; // 对话立场
    public string RoleName; // 角色名称
    public string Emotion; // 角色情绪
    public string Sentence; // 单句台词
}