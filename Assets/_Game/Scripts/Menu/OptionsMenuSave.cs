[System.Serializable]
public class OptionsMenuSave
{
    public int resWidth;
    public int resHeight;
    public int resRefreshRate;
    public int qualLevel;
    public bool isFullscreen;
    public float masterVolume;
    public float sfxVolume;
    public float musicVolume;
    public float ambienceVolume;

    public OptionsMenuSave(
        int resWidth, int resHeight, int resRefreshRate, int qualLevel, bool isFullscreen,
        float masterVolume, float sfxVolume, float musicVolume, float ambienceVolume)
    {
        this.resWidth = resWidth;
        this.resHeight = resHeight;
        this.resRefreshRate = resRefreshRate;
        this.qualLevel = qualLevel;
        this.isFullscreen = isFullscreen;

        this.masterVolume = masterVolume;
        this.sfxVolume = sfxVolume;
        this.musicVolume = musicVolume;
        this.ambienceVolume = ambienceVolume;
    }
}
