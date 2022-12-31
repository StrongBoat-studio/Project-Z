[System.Serializable]
public class OptionsMenuSave
{
    public int resWidth;
    public int resHeight;
    public int resRefreshRate;
    public int qualLevel;
    public bool isFullscreen;

    public OptionsMenuSave(int resWidth, int resHeight, int resRefreshRate, int qualLevel, bool isFullscreen)
    {
        this.resWidth = resWidth;
        this.resHeight = resHeight;
        this.resRefreshRate = resRefreshRate;
        this.qualLevel = qualLevel;
        this.isFullscreen = isFullscreen;
    }
}
