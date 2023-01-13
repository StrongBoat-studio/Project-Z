[System.Serializable]
public class OptionsMenuSave
{
    public int resScale;
    public int resRefreshRate;
    public int qualLevel;
    public int fullscreenMode;

    public OptionsMenuSave(int resScale, int resRefreshRate, int qualLevel, int fullscreenMode)
    {
        this.resScale = 1;
        this.resRefreshRate = resRefreshRate;
        this.qualLevel = qualLevel;
        this.fullscreenMode = fullscreenMode;
    }
}
