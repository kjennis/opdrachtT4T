using System;
using System.Collections.Generic;
using opdracht.data;

namespace opdracht.interfaces
{
public interface IDataService
{
    List<Track> getTracks();
    void insertTalksFromFile(string fileContent);
    void addTalk(Talk talk);
}
}