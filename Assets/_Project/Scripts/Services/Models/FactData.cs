using System;
using System.Collections.Generic;


[Serializable]
public class FactsJsonData
{
    public List<FactItem> data;
}

[Serializable]
public class FactItem
{
    public string id;
    public FactAttributes attributes;
}

[Serializable]
public class FactAttributes
{
    public string name;
    public string description;
}

public struct FactData
{
    public int Id;
    public string WebId;
    public string Name;
}