using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using opdracht.interfaces;
using opdracht.data;

public class DataService : IDataService
{
    private List<Track> _tracks;
    private List<Talk> _talks;
    private List<Event> _events;
    private List<Day> _days;
    private List<Session> _sessions;
    public DataService()
    {
        _tracks = new List<Track>();
        _talks = new List<Talk>();
        _events = new List<Event>();
        _days = new List<Day>();
        _sessions = new List<Session>();
    }
    public List<Track> getTracks()
    {
        _tracks = new List<Track>();
        int currentTrack = 1;
        foreach (var day in _days)
        {
            Track track = new Track();
            track.Title = "Track " + currentTrack;
            currentTrack++;

            foreach (var s in _sessions)
            {
                if(s.DayId == day.Id)
                {
                    List<Event> eventsInThisTrack = _events.FindAll(e => e.SessionId == s.Id);
                    foreach (var e in eventsInThisTrack)
                    {
                        track.Talks.Add(new Talk(){Title = e.Title, Time = e.StartTime, Duration = e.Duration});
                    }
                    if (s.Type == SessionType.Morning)
                    {
                        track.Talks.Add(new Talk(){Title = "Lunch", Time = "12:00PM", Duration = 60});
                    }
                    else
                    {
                        track.Talks.Add(new Talk(){Title = "Network Event", Time = "05:00PM", Duration = 60});
                    }
                }
            }
            _tracks.Add(track);
        }
        return _tracks;
    }
    public void addTalk(Talk talk)
    {
        _events.Add(new Event(){Title = talk.Title, Duration = talk.Duration});
        _tracks = new List<Track>();
        _talks = new List<Talk>();
        _days = new List<Day>();
        _sessions = new List<Session>();
        generateTracksAndSessions();
        assignEventsToSessions();
    }
    public void insertTalksFromFile(string fileContent)
    {
        _events = parseEventsFromFile(fileContent);
        generateTracksAndSessions();
        assignEventsToSessions();
    }
    private List<Event> parseEventsFromFile(string fileContent)
    {
        List<Event> events = new List<Event>();
        foreach(var line in fileContent.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries))
        { 
            string title = Regex.Match(line, @"^[^0-9]*").Value.Trim();
            int duration = 0;

            if(title.Contains("lightning"))
            {
                title.Replace(" lightning", "");
                duration = 5;
            }
            else
            {
                duration = Int32.Parse(line.Where(Char.IsDigit).ToArray());
            }
            events.Add(new Event() { Title = title, Duration = duration });
        }
        return events;
    }

    private void generateTracksAndSessions()
    {
        double totalDuration = _events.Sum(x => x.Duration);
        int estimatedNumberOfTracks = (int)Math.Ceiling(totalDuration / 420);

        _days = Enumerable
            .Range(0, estimatedNumberOfTracks)
            .Select(i => new Day()).ToList();

        _sessions = new List<Session>();
        foreach (var day in _days)
        {
            _sessions.Add(new Session(){DayId = day.Id, Type = SessionType.Morning, TimeAvailable = 180 });
            _sessions.Add(new Session(){DayId = day.Id, Type = SessionType.Afternoon, TimeAvailable = 240 });
        }
    }

    private void assignEventsToSessions()
    {
        //Order Events from longest to shortest
        _events = _events.OrderBy(x => x.Duration).ToList();
        foreach (Event e in _events)
        {
            //P1: Assign event to session if the duration mathces timeRemaining
            var session = _sessions.FirstOrDefault(s => s.TimeAvailable == e.Duration);
            if (session != null) 
            {
                addEventToSession(e, session);
            }
            else
            {
                //P2: Assign event to session with the longest timeRemaining
                session = _sessions.OrderByDescending(i=>i.TimeAvailable).First();
                addEventToSession(e, session);
            }
        }
        //P3: Assign event to morning sessions first.
        //P4: Assign events to lowest session number.
    }

    private void addEventToSession(Event e, Session s)
    {
        e.SessionId = s.Id;
                
        if(s.Type == SessionType.Morning)
        {
            int tod = 720 - s.TimeAvailable;
            int hourOfTheDay = tod / 60;
            int minuteOfTheDay = tod % 60;
                    e.StartTime = hourOfTheDay.ToString().PadLeft(2, '0') + ':' + minuteOfTheDay.ToString().PadLeft(2, '0') + "AM";
         }
        else
        {
            int tod = 1020 - s.TimeAvailable;
            int hourOfTheDay = (tod / 60) -12;
            int minuteOfTheDay = tod % 60;
            e.StartTime = hourOfTheDay.ToString().PadLeft(2, '0') + ':' + minuteOfTheDay.ToString().PadLeft(2, '0') + "PM";
        }
        s.TimeAvailable -= e.Duration;
    }
}

public class Event
{
    private static int m_Counter = 0;
    public int Id { get; set; }
    public Event()
    {
        this.Id = System.Threading.Interlocked.Increment(ref m_Counter);
    }
    public int SessionId { get; set; }
    public string StartTime { get; set; }
    public int Duration { get; set; }
    public string Title { get; set; }
}

public class Session
{
    private static int m_Counter = 0;
    public int Id { get; set; }
    public Session()
    {
        this.Id = System.Threading.Interlocked.Increment(ref m_Counter);
    }
    public int DayId { get; set; }
    public SessionType Type { get; set; }
    public int TimeAvailable { get; set; }
}
public class Day
{
    private static int m_Counter = 0;
    public int Id { get; set; }
    public Day()
    {
        this.Id = System.Threading.Interlocked.Increment(ref m_Counter);
    }
}
public enum SessionType { Morning, Afternoon }

