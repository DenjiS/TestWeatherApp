using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class WeatherJsonData
{
    public WeatherProperties properties;
}

[Serializable]
public class WeatherProperties
{
    public List<WeatherPeriod> periods;
}

[Serializable]
public class WeatherPeriod
{
    public int temperature;
    public string icon;
}

public struct WeatherData
{
    public int Temperature;
    public Texture Icon;
}