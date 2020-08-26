using Nancy.Json;
using System.IO;

namespace pomodoro
{

    class AppState : AppSettings<AppState>
    {
        public int x_pos = 0;
        public int y_pos = 0;
        public int work_time = 25;
        public int break_time = 5;
        public int long_break_time = 15;
        public int long_break_interval = 4;
        public bool locked = false;
        public string work_ring_tone = "asset/work.wav";
        public string break_ring_tone = "asset/break.wav";
        public string work_color = "#3489eb";
        public string break_color = "#72c938";
        public string long_break_color = "#fffd80";
        public double opacity = 0.7;
    }
    public class AppSettings<T> where T : new()
    {
        private const string DEFAULT_FILENAME = "settings.json";

        public void Save(string fileName = DEFAULT_FILENAME)
        {
            File.WriteAllText(fileName, (new JavaScriptSerializer()).Serialize(this));
        }

        public static void Save(T pSettings, string fileName = DEFAULT_FILENAME)
        {
            File.WriteAllText(fileName, (new JavaScriptSerializer()).Serialize(pSettings));
        }

        public static T Load(string fileName = DEFAULT_FILENAME)
        {
            T t = new T();
            if (File.Exists(fileName))
                t = (new JavaScriptSerializer()).Deserialize<T>(File.ReadAllText(fileName));
            return t;
        }
    }
}
