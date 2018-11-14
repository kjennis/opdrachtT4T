using System;
using System.Collections.Generic;

namespace opdracht.data
{
public class Track
{
    public string Title { get; set; }
    public List<Talk> Talks { get; set; }

    public Track()
    {
        Talks = new List<Talk>();
    }
}
}