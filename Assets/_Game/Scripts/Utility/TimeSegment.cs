using System;

[System.Serializable]
public class TimeSegment {
    private const int MIN = 60;
    private const int HOUR = 60 * 60;
    private const long TIME_SPAN_TICK_IN_SECOND = 10_000_000L;

    public int sec;
    public int min;
    public int hour;

    public int ToTick() {
        return ConvertToTick(this);
    }

    static public int ConvertToTick(TimeSegment timeSegment) {
        return timeSegment.sec + (timeSegment.min * MIN) + (timeSegment.hour * HOUR);
    }

    static public int ConvertToTick(int sec, int min, int hour) {
        return sec + (min * MIN) + (hour * HOUR);
    }

    static public TimeSpan GetTimeSpan(int tick) {
        return new(tick * TIME_SPAN_TICK_IN_SECOND);
    }

    static public int GetSecond(int tick) => tick % 60;
    static public int GetMinute(int tick) => (tick / 60) % 60;
    static public int GetHour(int tick) => (tick / (60 * 60)) % 24;
}